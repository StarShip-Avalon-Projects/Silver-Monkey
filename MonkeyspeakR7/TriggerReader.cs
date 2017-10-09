using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

// Removed by Gerolkae Looking for threads
//using System.Threading;

namespace Monkeyspeak
{
    [Serializable]
    public class TriggerReaderException : Exception
    {
        public TriggerReaderException()
        {
        }

        public TriggerReaderException(string message)
            : base(message)
        {
        }

        public TriggerReaderException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected TriggerReaderException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// A Reader that is used to get Variables, Strings, and Numbers from Triggers
    /// </summary>
    [CLSCompliant(true)]
    public sealed class TriggerReader
    {
        private Trigger cloneTrigger, originalTrigger;
        private Page page;

        private readonly object syncObject = new object();

        /// <summary>
        /// A Reader that is used to get Variables, Strings, and Numbers from Triggers
        /// </summary>
        /// <param name="page"></param>
        /// <param name="trigger"></param>
        public TriggerReader(Page page, Trigger trigger)
        {
            originalTrigger = trigger;
            cloneTrigger = originalTrigger.Clone();
            this.page = page;
        }

        internal TriggerReader(Page page)
        {
            this.page = page;
        }

        public Trigger Trigger
        {
            get { return cloneTrigger; }
            internal set
            {
                originalTrigger = value;
                cloneTrigger = originalTrigger.Clone();
            }
        }

        public TriggerCategory TriggerCategory
        {
            get { return cloneTrigger.Category; }
        }

        public int TriggerId
        {
            get { return cloneTrigger.Id; }
        }

        public Page Page
        {
            get { return page; }
        }

        /// <summary>
        /// Resets the reader's indexes to 0
        /// </summary>
        public void Reset()
        {
            if (originalTrigger != null)
            {
                cloneTrigger.contents = new Queue<object>(originalTrigger.contents);
            }
        }

        /// <summary>
        /// Reads the next String, throws TriggerReaderException on failure
        /// </summary>
        /// <returns></returns>
        /// <exception cref="TriggerReaderException"></exception>
        public string ReadString(bool processVariables = true)
        {
            if (cloneTrigger.contents.Count == 0) throw new TriggerReaderException("Unexpected end of values");
            if (!(cloneTrigger.contents.Peek() is string)) throw new TriggerReaderException($"Expected string, got {cloneTrigger.contents.Peek().GetType().Name}");
            try
            {
                string str = Convert.ToString(cloneTrigger.contents.Dequeue());
                if (processVariables)
                {
                    for (int i = page.Scope.Count - 1; i >= 0; i--)
                    {
                        // replaced string.replace with Regex because
                        //  %ListName would replace %ListName2 leaving the 2 at the end
                        //- Gerolkae
                        if (!str.Contains(page.Scope[i].Name)) continue;
                        string pattern = page.Scope[i].Name + @"\b";
                        var value = page.Scope[i].Value;
                        string replace = (value != null) ? value.ToString() : "null";
                        str = Regex.Replace(str, pattern, replace, RegexOptions.CultureInvariant);
                    }
                }
                return str;
            }
            catch
            {
                throw new TriggerReaderException("No value found.");
            }
        }

        /// <summary>
        /// Peeks at the next value
        /// </summary>
        /// <returns></returns>
        public bool PeekString()
        {
            if (cloneTrigger.contents.Count == 0) return false;
            if (cloneTrigger.contents.Peek() is string)
            {
                string value = cloneTrigger.contents.Peek().ToString();
                return value[0] != page.Engine.Options.VariableDeclarationSymbol;
            }
            return false;
        }

        public bool TryReadString(out string str)
        {
            if (!PeekString())
            {
                str = String.Empty;
                return false;
            }

            str = Convert.ToString(cloneTrigger.contents.Dequeue());
            return true;
        }

        /// <summary>
        /// Reads the next Variable available, throws TriggerReaderException on failure
        /// </summary>
        /// <param name="addIfNotExist">Add the Variable if it doesn't exist and return that Variable with a Value equal to null.</param>
        /// <returns>Variable</returns>
        /// <exception cref="TriggerReaderException"></exception>
        public IVariable ReadVariable(bool addIfNotExist = false)
        {
            if (cloneTrigger.contents.Count == 0) throw new TriggerReaderException("Unexpected end of values");
            if (!((string)cloneTrigger.contents.Peek()).StartsWith("%")) throw new TriggerReaderException($"Expected variable, got {cloneTrigger.contents.Peek().GetType().Name}");
            try
            {
                var var = Variable.NoValue;
                string varRef = Convert.ToString(cloneTrigger.contents.Dequeue());
                if (!page.HasVariable(varRef, out var))
                    if (addIfNotExist)
                        var = page.SetVariable(varRef, null, false);

                return var;
            }
            catch (Exception ex)
            {
                throw new TriggerReaderException("No value found.", ex);
            }
        }

        /// <summary>
        /// Reads the next Variable list available, throws TriggerReaderException on failure
        /// </summary>
        /// <param name="addIfNotExist">Add the Variable if it doesn't exist and return that Variable with a Value equal to null.</param>
        /// <returns>Variable</returns>
        /// <exception cref="TriggerReaderException"></exception>
        public VariableList ReadVariableList(bool addIfNotExist = false)
        {
            if (cloneTrigger.contents.Count == 0) throw new TriggerReaderException("Unexpected end of values");
            if (!(cloneTrigger.contents.Peek() is VariableList)) throw new TriggerReaderException($"Expected variable list, got {cloneTrigger.contents.Peek().GetType().Name}");
            try
            {
                var var = Variable.NoValue;
                string varRef = Convert.ToString(cloneTrigger.contents.Dequeue());
                if (!page.HasVariable(varRef, out var))
                    if (addIfNotExist)
                        var = page.SetVariable(varRef, null, false);

                return var is VariableList ? (VariableList)var : null;
            }
            catch (Exception ex)
            {
                throw new TriggerReaderException("No value found.", ex);
            }
        }

        /// <summary>
        /// Peeks at the next value
        /// </summary>
        /// <returns></returns>
        public bool PeekVariable()
        {
            if (cloneTrigger.contents.Count == 0) return false;
            string nextContent = cloneTrigger.contents.Peek().ToString();
            return (!String.IsNullOrEmpty(nextContent)) && nextContent[0] == page.Engine.Options.VariableDeclarationSymbol;
        }

        /// <summary>
        /// Peeks at the next value
        /// </summary>
        /// <returns></returns>
        public bool PeekVariableList()
        {
            if (cloneTrigger.contents.Count == 0) return false;
            string nextContent = cloneTrigger.contents.Peek().ToString();
            return (!String.IsNullOrEmpty(nextContent)) && nextContent[0] == page.Engine.Options.VariableDeclarationSymbol &&
                (nextContent.IndexOf('[') != -1 && nextContent.IndexOf(']') != -1);
        }

        /// <summary>
        /// Trys to read the next Variable available
        /// </summary>
        /// <param name="var">Variable is assigned on success</param>
        /// <returns>bool on success</returns>
        public bool TryReadVariable(out IVariable var)
        {
            if (!PeekVariable())
            {
                var = Variable.NoValue;
                return false;
            }
            string varRef = Convert.ToString(cloneTrigger.contents.Dequeue());
            var = page.GetVariable(varRef);
            return true;
        }

        /// <summary>
        /// Reads the next Double available, throws TriggerReaderException on failure
        /// </summary>
        /// <returns>Double</returns>
        /// <exception cref="TriggerReaderException"></exception>
        public double ReadNumber()
        {
            if (cloneTrigger.contents.Count == 0) throw new TriggerReaderException("Unexpected end of values");
            if (!(cloneTrigger.contents.Peek() is double)) throw new TriggerReaderException($"Expected number, got {cloneTrigger.contents.Peek().GetType().Name}");
            try
            {
                double number = Convert.ToDouble(cloneTrigger.contents.Dequeue());
                return number;
            }
            catch
            {
                throw new TriggerReaderException("No value found.");
            }
        }

        /// <summary>
        /// Peeks at the next value
        /// </summary>
        /// <returns></returns>
        public bool PeekNumber()
        {
            if (cloneTrigger.contents.Count == 0) return false;
            return cloneTrigger.contents.Peek() is double;
        }

        public bool TryReadNumber(out double number)
        {
            if (!PeekNumber())
            {
                number = double.NaN;
                return false;
            }
            number = Convert.ToDouble(cloneTrigger.contents.Dequeue());
            return true;
        }
    }
}