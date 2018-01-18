using Monkeyspeak;

namespace Engine.Libraries
{
    /// <summary>
    /// <para>
    /// Conditions: (1:60) - (1:61)
    /// </para>
    /// <para>
    /// Effects: (5:120)- (5:127)
    /// </para>
    /// Tied width a string!!
    /// <para>
    /// well not really but, this class extends monkey Speak ability to work
    /// with strings in <see cref="Monkeyspeak.Variable"/> s
    /// </para>
    /// </summary>
    public sealed class MsStringExtentions : MonkeySpeakLibrary
    {
        public override int BaseId => 120;

        public override void Initialize(params object[] args)
        {
            base.Initialize(args);

            Add(TriggerCategory.Condition,
                r => AndVariableContains(r),
                "and variable %variable contains text {...},");
            // (5:110) use variable % and take word # and put it into variable %
            Add(TriggerCategory.Effect,
                r => StringSplit(r),
                "use variable %Variable and take word position # and put it into variable %Variable.");
            // (5:111) use variable % then remove character {.} and put it into variable %.
            Add(TriggerCategory.Effect,
                r => StripCharacters(r),
                "use variable %Variable then remove all occurrences of character {.} and put it into variable %Variable.");
            // (5:122) chop off the beginning of variable %variable, removing the first # characters of it.
            Add(TriggerCategory.Effect,
                r => ChopStartString(r),
                "chop off the beginning of variable %variable, removing the first # characters of it.");
            // (5:123) chop off the end of variable %Variable, removing the last # characters of it.
            Add(TriggerCategory.Effect,
                r => ChopEndString(r),
                "chop off the end of variable %Variable, removing the last # characters of it.");
            // (5:126) count the number of characters in string variable %variable and put them into variable %Variable .
            Add(TriggerCategory.Effect,
                r => CountChars(r),
                "count the number of characters in string variable %variable and put them into variable %Variable.");
            // (5:127) take variable %Variable and Convert it to Furcadia short name. (with out special Characters or spaces)
            Add(TriggerCategory.Effect,
                r => ToShortName(r),
                "take variable %Variable and convert it to Furcadia short name. (without special characters or spaces or pipe \"|\").");
        }

        /// <summary>
        /// (5:123) chop off the end of variable %Variable, removing the
        /// last # characters of it.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        public bool ChopEndString(TriggerReader reader)
        {
            var Var = reader.ReadVariable(true);
            var test = int.TryParse(reader.ReadNumber().ToString(), out int Count);
            var str = Var.Value.ToString();
            if ((str.Length < Count))
            {
                Var.Value = str;
            }
            else
            {
                Var.Value = str.Substring(0, (str.Length - Count));
            }

            return true;
        }

        /// <summary>
        /// (5:122) chop off the beginning of variable %variable, removing
        /// the first # characters of it.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        public bool ChopStartString(TriggerReader reader)
        {
            var Var = reader.ReadVariable(true);
            var test = int.TryParse(reader.ReadNumber().ToString(), out int Count);
            var str = Var.Value.ToString();
            if ((str.Length < Count))
            {
                Var.Value = null;
            }
            else
            {
                Var.Value = str.Substring(Count);
            }

            return true;
        }

        /// <summary>
        /// (5:126) count the number of characters in string variable
        /// %variable and put them into variable %Variable .
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        public bool CountChars(TriggerReader reader)
        {
            var var1 = reader.ReadVariable();
            var var2 = reader.ReadVariable(true);
            var Count = var1.Value.ToString().Length;
            var2.Value = Count;
            return true;
        }

        /// <summary>
        /// (5:120) use variable % and take word # and put it into variable %
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool StringSplit(TriggerReader reader)
        {
            var Var = reader.ReadVariable();
            var i = reader.ReadNumber();
            var NewVar = reader.ReadVariable(true);
            var fields = Var.Value.ToString().Split(' ');
            if (i < fields.Length)
            {
                NewVar.Value = fields[(int)i];
            }

            return true;
        }

        /// <summary>
        /// (5:121) use variable % then remove character {.} and put it into
        /// variable %.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        public bool StripCharacters(TriggerReader reader)
        {
            var Var = reader.ReadVariable();
            var ch = reader.ReadString()[0].ToString();
            var NewVar = reader.ReadVariable();
            var varStr = Var.Value.ToString();
            var NewStr = varStr.Replace(@ch, string.Empty);
            NewVar.Value = NewStr;
            return true;
        }

        /// <summary>
        /// (1:62) and variable %variable contains text {...},
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        public static bool AndVariableContains(TriggerReader reader)
        {
            var VariableToCheck = reader.ReadVariable();
            string Argument;
            if (reader.PeekVariable())
            {
                Argument = reader.ReadVariable().Value.ToString();
            }
            else if (reader.PeekNumber())
            {
                Argument = reader.ReadNumber().ToString();
            }
            else
            {
                Argument = reader.ReadString();
            }

            return VariableToCheck.Value.ToString().Contains(Argument);
        }

        /// <summary>
        /// (5:127) take variable %Variable and convert it to Furcadia short
        /// name. (without special characters or spaces or pipe ""|"").
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private static bool ToShortName(TriggerReader reader)
        {
            if (reader.PeekVariable())
            {
                var var = reader.ReadVariable();
                if (string.IsNullOrEmpty(var.Value.ToString()))
                {
                    return true;
                }

                var.Value = var.Value.ToString().ToFurcadiaShortName();
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Unload(Page page)
        {
        }
    }
}