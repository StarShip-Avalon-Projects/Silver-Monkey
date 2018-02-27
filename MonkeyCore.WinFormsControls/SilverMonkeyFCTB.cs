#region Copyright Notice

//
//  THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
//  PURPOSE.
//
//  License: GNU Lesser General Public License (LGPLv3)
//
//  Email: pavel_torgashov@mail.ru.
//
//  Copyright (C) Pavel Torgashov, 2013.

#endregion Copyright Notice

using FastColoredTextBoxNS;
using Irony.Parsing;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace Controls
{
    /// <summary>
    /// FastColoredTextBox with Irony parser support
    /// </summary>
    /// <seealso href="Http://www.codeproject.com/articles/161871/fast-colored-textbox-for-syntax-highlighting"/>
    /// <seealso href="https://github.com/PavelTorgashov/FastColoredTextBox"/>
    /// <seealso href="Http://irony.codeplex.com/"/>
    public class SilverMonkeyFCTB : FastColoredTextBox
    {
        #region Public Fields

        public Style WavyStyle = new WavyLineStyle(255, Color.Red);

        #endregion Public Fields

        #region Protected Fields

        protected Parser parser;

        #endregion Protected Fields

        #region Public Constructors

        public SilverMonkeyFCTB()
        {
        }

        #endregion Public Constructors

        #region Public Events

        public event EventHandler<StyleNeededEventArgs> StyleNeeded;

        #endregion Public Events

        #region Public Properties

        /// <summary>
        /// Grammar of custom language
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public Grammar Grammar
        {
            get
            {
                if (parser != null && parser.Language != null && parser.Language.Grammar != null)
                    return parser.Language.Grammar;
                return null;
            }
            set
            {
                SetParser(value);
            }
        }

        /// <summary>
        /// Parser of custom language
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public Parser Parser
        {
            get
            {
                return parser;
            }
            set
            {
                SetParser(value);
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Returns range of token
        /// </summary>
        public Range GetTokenRange(Token t)
        {
            var loc = t.Location;

            var place = new Place(loc.Column, loc.Line);
            var r = new Range(this, place, place);

            foreach (var c in t.Text)
                if (c != '\r')
                    r.GoRight(true);

            return r;
        }

        public virtual void OnStyleNeeded(StyleNeededEventArgs e)
        {
            StyleNeeded?.Invoke(this, e);
        }

        public override void OnTextChangedDelayed(Range changedRange)
        {
            DoHighlighting();
            base.OnTextChangedDelayed(changedRange);
        }

        /// <summary>
        /// Sets Irony parser (based on Grammar)
        /// </summary>
        public virtual void SetParser(Grammar grammar)
        {
            SetParser(new LanguageData(grammar));
        }

        /// <summary>
        /// Sets Irony parser (based on LanguageData)
        /// </summary>
        public virtual void SetParser(LanguageData language)
        {
            SetParser(new Parser(language));
        }

        /// <summary>
        /// Sets Irony parser
        /// </summary>
        public virtual void SetParser(Parser parser)
        {
            this.parser = parser;
            ClearStylesBuffer();
            AddStyle(WavyStyle);
            SyntaxHighlighter.InitStyleSchema(Language.CSharp);
            InitBraces();
            OnTextChanged(Range);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void DoHighlighting()
        {
            if (parser == null)
                return;

            //parse text
            ParseTree tree;

            try
            {
                tree = parser.Parse(Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); //oops...
                return;
            }

            //highlight errors
            if (tree.Status == ParseTreeStatus.Error)
            {
                ClearStyle(GetStyleIndexMask(new Style[] { WavyStyle }));
                foreach (var msg in tree.ParserMessages)
                {
                    var loc = msg.Location;
                    var place = new Place(loc.Column, loc.Line);
                    var r = new Range(this, place, place);
                    var f = r.GetFragment(@"[\S]");
                    f.SetStyle(WavyStyle);
                }
                return;
            }

            //highlight syntax
            ClearStyle(StyleIndex.All);
            foreach (var t in tree.Tokens)
            {
                var arg = new StyleNeededEventArgs(t);
                OnStyleNeeded(arg);

                if (arg.Cancel)
                    continue;

                if (arg.Style != null)
                {
                    GetTokenRange(t).SetStyle(arg.Style);
                    continue;
                }

                switch (t.Terminal.GetType().Name)
                {
                    case "KeyTerm":
                        if ((t.Terminal.Flags & TermFlags.IsKeyword) != 0) //keywords are highlighted only
                            GetTokenRange(t).SetStyle(SyntaxHighlighter.KeywordStyle);
                        break;

                    case "NumberLiteral":
                        GetTokenRange(t).SetStyle(SyntaxHighlighter.NumberStyle);
                        break;

                    case "StringLiteral":
                        GetTokenRange(t).SetStyle(SyntaxHighlighter.StringStyle);
                        break;

                    case "CommentTerminal":
                        GetTokenRange(t).SetStyle(SyntaxHighlighter.CommentStyle);
                        break;
                }
            }
        }

        protected virtual void InitBraces()
        {
            LeftBracket = '\x0';
            RightBracket = '\x0';
            LeftBracket2 = '\x0';
            RightBracket2 = '\x0';

            // select the first two pair of braces with the length of exactly one char (FCTB restrictions)
            var braces = parser.Language.Grammar.KeyTerms
              .Select(pair => pair.Value)
              .Where(term => term.Flags.IsSet(TermFlags.IsOpenBrace))
              .Where(term => term.IsPairFor != null && term.IsPairFor is KeyTerm)
              .Where(term => term.Text.Length == 1)
              .Where(term => ((KeyTerm)term.IsPairFor).Text.Length == 1)
              .Take(2);
            if (braces.Any())
            {
                // first pair
                var brace = braces.First();
                LeftBracket = brace.Text.First();
                RightBracket = ((KeyTerm)brace.IsPairFor).Text.First();
                // second pair
                if (braces.Count() > 1)
                {
                    brace = braces.Last();
                    LeftBracket2 = brace.Text.First();
                    RightBracket2 = ((KeyTerm)brace.IsPairFor).Text.First();
                }
            }
            else
            {
                LeftBracket = '(';
                RightBracket = ')';
            }
        }

        #endregion Protected Methods
    }

    public class StyleNeededEventArgs : EventArgs
    {
        #region Public Fields

        public readonly Token Token;

        #endregion Public Fields

        #region Public Constructors

        public StyleNeededEventArgs(Token t)
        {
            Token = t;
        }

        #endregion Public Constructors

        #region Public Properties

        public bool Cancel { get; set; }
        public Style Style { get; set; }

        #endregion Public Properties
    }
}