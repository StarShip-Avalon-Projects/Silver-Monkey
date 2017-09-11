﻿using Monkeyspeak.lexical;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Monkeyspeak
{
    public class MonkeyspeakEngine
    {
        #region Public Fields

        public Options options;

        #endregion Public Fields

        #region Private Fields

        private ILexer lexer;
        private List<Page> pages;
        private AbstractParser parser;

        #endregion Private Fields

        #region Public Constructors

        public MonkeyspeakEngine()
        {
            pages = new List<Page>();

            CreateDefaultOptions();

            CreateDefaultLexer();
            parser = new Parser(this, lexer);
        }

        public MonkeyspeakEngine(Options options)
        {
            pages = new List<Page>();

            this.options = options;

            CreateDefaultLexer();
            parser = new Parser(this, lexer);
        }

        #endregion Public Constructors

        #region Public Properties

        [CLSCompliant(false)]
        public Options Options
        {
            get { return options; }
            set { options = value; }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Compiles a Page to a file
        /// </summary>
        /// <param name="page">
        /// </param>
        /// <param name="filePath">
        /// </param>
        public void CompileToFile(Page page, string filePath)
        {
            page.CompileToFile(filePath);
        }

        /// <summary>
        /// Compiles a Page to a stream
        /// </summary>
        /// <param name="page">
        /// </param>
        /// <param name="stream">
        /// </param>
        public void CompileToStream(Page page, Stream stream)
        {
            page.CompileToStream(stream);
        }

        /// <summary>
        /// Loads compiled script from file
        /// </summary>
        /// <param name="filePath">
        /// </param>
        /// <returns>
        /// </returns>
        public Page LoadCompiledFile(string filePath)
        {
            try
            {
                using (Stream stream = new FileStream(filePath, FileMode.Open))
                {
                    return LoadCompiledStream(stream);
                }
            }
            catch (Exception ex)
            {
                throw new MonkeyspeakException(String.Format("Could not load compiled script from file.  Reason:{0}", ex.Message), ex);
            }
        }

        /// <summary>
        /// Loads a compiled script from stream
        /// </summary>
        /// <param name="stream">
        /// </param>
        /// <returns>
        /// </returns>
        public Page LoadCompiledStream(Stream stream)
        {
            try
            {
                var page = new Page(this);
                var compiler = new Compiler(this);
                using (Stream zipStream = new System.IO.Compression.DeflateStream(stream, System.IO.Compression.CompressionMode.Decompress))
                {
                    page.Write(compiler.DecompileFromStream(zipStream));
                }
                return page;
            }
            catch (Exception ex)
            {
                throw new MonkeyspeakException(String.Format("Could not load compiled script from stream.  Reason:{0}", ex.Message), ex);
            }
        }

        /// <summary>
        /// Loads a Monkeyspeak script from a string into a <see cref="Monkeyspeak.Page"/>.
        /// </summary>
        /// <param name="chunk">
        /// String that contains the Monkeyspeak script source.
        /// </param>
        /// <returns>
        /// </returns>
        public Page LoadFromString(string chunk)
        {
            try
            {
                Page page = new Page(this);
                page.Write(parser.Parse(chunk));
                pages.Add(page);
                return page;
            }
            catch (Exception ex)
            {
                throw new MonkeyspeakException(String.Format("Could not load script from chunk.  Reason:{0}", ex.Message));
            }
        }

        /// <summary>
        /// Loads a Monkeyspeak script from a string into
        /// <paramref name="existingPage"/>. and clears the old page
        /// </summary>
        /// <param name="existingPage">
        /// Reference to an existing Page
        /// </param>
        /// <param name="chunk">
        /// String that contains the Monkeyspeak script source.
        /// </param>
        /// <returns>
        /// </returns>
        public Page LoadFromString(ref Page existingPage, string chunk)
        {
            try
            {
                existingPage.OverWrite(parser.Parse(chunk));
            }
            catch (Exception ex)
            {
                throw new MonkeyspeakException(String.Format("Could not load script from chunk.  Reason:{0}", ex.Message));
            }
            return existingPage;
        }

        #endregion Public Methods

        #region Internal Methods

        /// <summary>
        /// Loads a Monkeyspeak script from a Stream into a <see cref="Monkeyspeak.Page"/>.
        /// </summary>
        /// <param name="stream">
        /// Stream that contains the Monkeyspeak script. Closes the stream.
        /// </param>
        /// <returns>
        /// <see cref="Monkeyspeak.Page"/>
        /// </returns>
        public Page LoadFromStream(Stream stream)
        {
            try
            {
                Page page;
                using (var reader = new StreamReader(stream, Encoding.Unicode, true, 1024, true))
                {
                    page = LoadFromString(reader.ReadToEnd());
                }
                return page;
            }
            catch (Exception ex)
            {
                throw new MonkeyspeakException(String.Format("Could not load script from stream.  Reason:{0}", ex.Message), ex);
            }
        }

        /// <summary>
        /// Loads a Monkeyspeak script from a Stream into <paramref name="existingPage"/>.
        /// </summary>
        /// <param name="existingPage">
        /// Reference to an existing Page
        /// </param>
        /// <param name="stream">
        /// Stream that contains the Monkeyspeak script. Closes the stream.
        /// </param>
        public Page LoadFromStream(ref Page existingPage, Stream stream)
        {
            try
            {
                Page page = null;
                using (var reader = new StreamReader(stream, Encoding.Unicode, true, 1024, true))
                {
                    page = LoadFromString(ref existingPage, reader.ReadToEnd());
                }
                return page;
            }
            catch (Exception ex)
            {
                throw new MonkeyspeakException(String.Format("Could not load script from stream.  Reason:{0}", ex.Message), ex);
            }
        }

        internal void CreateDefaultLexer()
        {
            lexer = new Lexer();

            lexer.AddDefinition(new TokenDefinition(
                TokenType.Trigger,
                new Regex(@"\([0-9]{1}\:[0-9]{1," + Int32.MaxValue + @"}\)", RegexOptions.Compiled)));

            lexer.AddDefinition(new TokenDefinition(
                TokenType.Variable,
                new Regex(String.Format(@"\{0}[\ba-zA-Z\d\D][\ba-zA-Z\d_]*", options.VariableDeclarationSymbol), RegexOptions.Compiled)));

            lexer.AddDefinition(new TokenDefinition(
                TokenType.String,
                new Regex(@"\" + options.StringBeginSymbol + @"(.*?)\" + options.StringEndSymbol, RegexOptions.Compiled)));

            lexer.AddDefinition(new TokenDefinition(
                TokenType.Number,
                new Regex(@"[-+]?([0-9]*\.[0-9]+|[0-9]+)", RegexOptions.Compiled)));

            lexer.AddDefinition(new TokenDefinition(
                TokenType.Comment,
                new Regex(@"\" + options.CommentSymbol + @".*[\r|\n]", RegexOptions.Compiled), true));
            lexer.AddDefinition(new TokenDefinition(
                TokenType.Word,
                new Regex(@"\w+", RegexOptions.Compiled), true));
            lexer.AddDefinition(new TokenDefinition(
                TokenType.Symbol,
                new Regex(@"\W", RegexOptions.Compiled), true));
            lexer.AddDefinition(new TokenDefinition(
                TokenType.WhiteSpace,
                new Regex(@"\s+", RegexOptions.Compiled), true));
        }

        internal void CreateDefaultOptions()
        {
            options = new Options
            {
                CanOverrideTriggerHandlers = false,
                StringBeginSymbol = '{',
                StringEndSymbol = '}',
                VariableDeclarationSymbol = '%',
                CommentSymbol = "*",
                TriggerLimit = 6000,
                VariableCountLimit = 1000,
                StringLengthLimit = Int32.MaxValue,
                TimerLimit = 100,
                Version = Assembly.GetExecutingAssembly().GetName().Version
            };
        }

        #endregion Internal Methods
    }

    [Serializable]
    public class MonkeyspeakException : Exception
    {
        #region Public Constructors

        public MonkeyspeakException()
        {
        }

        public MonkeyspeakException(string message) : base(message)
        {
        }

        public MonkeyspeakException(string format, params object[] message) : base(String.Format(format, message))
        {
        }

        public MonkeyspeakException(string message, Exception inner) : base(message, inner)
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected MonkeyspeakException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        #endregion Protected Constructors
    }
}