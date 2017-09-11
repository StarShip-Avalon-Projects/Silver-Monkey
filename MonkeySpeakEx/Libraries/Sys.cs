﻿using System;

namespace Monkeyspeak.Libraries
{
    /// <summary>
    /// Monkey Speak Sys library
    /// </summary>
    internal class Sys : AbstractBaseLibrary
    {
        #region Public Constructors

        public Sys()
        {
            // (1:100) and variable %Variable is defined,
            Add(new Trigger(TriggerCategory.Condition, 100), IsVariableDefined,
                "(1:100) and variable % is defined,");

            // (1:101) and variable %Variable is not defined,
            Add(new Trigger(TriggerCategory.Condition, 101), IsVariableNotDefined,
                "(1:101) and variable % is not defined,");

            // (1:102) and variable %Variable equals #,
            Add(new Trigger(TriggerCategory.Condition, 102), IsVariableEqualToNumberOrVar,
                "(1:102) and variable % equals #,");

            // (1:103) and variable %Variable does not equal #,
            Add(new Trigger(TriggerCategory.Condition, 103), IsVariableNotEqualToNumberOrVar,
                "(1:103) and variable % does not equal #,");

            // (1:104) and variable %Variable equals {...},
            Add(new Trigger(TriggerCategory.Condition, 104), IsVariableEqualToString,
                "(1:104) and variable % equals {...},");

            // (1:105) and variable %Variable does not equal {...},
            Add(new Trigger(TriggerCategory.Condition, 105), IsVariableNotEqualToString,
                "(1:105) and variable % does not equal {...},");

            // (1:106) and variable %Variable is constant,
            Add(new Trigger(TriggerCategory.Condition, 106), VariableIsConstant,
                "(1:106) and variable % is constant,");

            // (1:107) and variable %Variable is not constant,
            Add(new Trigger(TriggerCategory.Condition, 107), VariableIsNotConstant,
                "(1:107) and variable % is not constant,");

            // (5:100) set variable %Variable to {...}.
            Add(new Trigger(TriggerCategory.Effect, 100), SetVariableToString,
                "(5:100) set variable % to {...}.");

            // (5:101) set variable %Variable to #.
            Add(new Trigger(TriggerCategory.Effect, 101), SetVariableToNumberOrVariable,
                "(5:101) set variable % to #.");

            // (5:102) print {...} to the console.
            Add(new Trigger(TriggerCategory.Effect, 102), PrintToConsole,
                "(5:102) print {...} to the console.");

            // (5:103) get the environment variable named {...} and put it
            // into #,
            Add(new Trigger(TriggerCategory.Effect, 103), GetEnvVariable,
                "(5:103) get the environment variable named {...} and put it into %, (ex: PATH)");

            Add(TriggerCategory.Effect, 104, RandomValueToVar,
                "(5:104) create random number and put it into variable %.");
            // (5:105) raise an error.
            Add(new Trigger(TriggerCategory.Effect, 105), RaiseAnError,
                "(5:105) raise an error.");

            // (5:110) load library from file {...}.
            Add(new Trigger(TriggerCategory.Effect, 110), LoadLibraryFromFile,
                "(5:110) load library from file {...}. (example Monkeyspeak.dll)");
        }

        #endregion Public Constructors

        #region Private Methods

        /// <summary>
        /// (5:103) get the environment variable named {...} and put it into
        /// %, (ex: PATH)
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// </returns>
        private bool GetEnvVariable(TriggerReader reader)
        {
            string envVar = Environment.GetEnvironmentVariable(reader.ReadString());
            Variable var = reader.ReadVariable(true);
            var.Value = envVar;
            return true;
        }

        /// <summary>
        /// (1:100) and variable % is defined,
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// </returns>
        private bool IsVariableDefined(TriggerReader reader)
        {
            Variable var = reader.ReadVariable(false);
            return reader.Page.HasVariable(var.Name) && var.Value != null;
        }

        /// <summary>
        /// (1:102) and variable % equals #,
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// </returns>
        private bool IsVariableEqualToNumberOrVar(TriggerReader reader)
        {
            Variable var = reader.ReadVariable(false);
            Variable optVar;
            double number = 0;
            double num = 0;
            double optNum = 0;
            if (reader.TryReadVariable(out optVar))
            {
                // var can be undefined or Single Digit number Compensate
                // for it. -Gerolkae
                try
                {
                    if (Double.TryParse(var.Value.ToString(), out num) && Double.TryParse(optVar.Value.ToString(), out optNum))
                        return num == optNum;
                    else
                        return var.Value.Equals(optVar.Value);
                }
                catch
                {
                    System.Diagnostics.Debug.Print(var.ToString());
                    System.Diagnostics.Debug.Print(optVar.ToString());
                    return false;
                }
            }
            else if (reader.TryReadNumber(out number))
            {
                try
                {
                    if (Double.TryParse(var.Value.ToString(), out num))
                        return num == number;
                    return false;
                }
                catch
                {
                    System.Diagnostics.Debug.Print(var.ToString());
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// (1:104) and variable % equals {...},
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// </returns>
        private bool IsVariableEqualToString(TriggerReader reader)
        {
            Variable var = reader.ReadVariable(false);
            string str = reader.ReadString();
            return str.Equals(var.Value.ToString());
        }

        /// <summary>
        /// (1:101) and variable % is not defined,
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// </returns>
        private bool IsVariableNotDefined(TriggerReader reader)
        {
            return IsVariableDefined(reader) == false;
        }

        /// <summary>
        /// (1:103) and variable % does not equal #,
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// </returns>
        private bool IsVariableNotEqualToNumberOrVar(TriggerReader reader)
        {
            Variable var = reader.ReadVariable(false);
            Variable optVar;
            double number = 0;
            double num = 0;
            double optNum = 0;
            if (reader.TryReadVariable(out optVar))
            {
                // var can be undefined or Single Digit number Compensate
                // for it. -Gerolkae
                try
                {
                    if (Double.TryParse(var.Value.ToString(), out num) && Double.TryParse(optVar.Value.ToString(), out optNum))
                        return num != optNum;
                    else
                        return var.Value != optVar.Value;
                }
                catch
                {
                    System.Diagnostics.Debug.Print(var.ToString());
                    System.Diagnostics.Debug.Print(optVar.ToString());
                    return false;
                }
            }
            else if (reader.TryReadNumber(out number))
            {
                try
                {
                    if (Double.TryParse(var.Value.ToString(), out num))
                        return num != number;
                    return false;
                }
                catch
                {
                    System.Diagnostics.Debug.Print(var.ToString());
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// (1:105) and variable % does not equal {...},
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// </returns>
        private bool IsVariableNotEqualToString(TriggerReader reader)
        {
            //bool test = IsVariableEqualToString(reader);
            //return IsVariableEqualToString(reader) == false;
            Variable var = reader.ReadVariable(false);
            string str = reader.ReadString();
            if (str.Equals(var.Value.ToString()))
                return false;
            else
                return true;
        }

        /// <summary>
        /// (5:110) load library from file {...}. (example Monkeyspeak.dll)
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// </returns>
        private bool LoadLibraryFromFile(TriggerReader reader)
        {
            if (reader.PeekString() == false) return false;
            reader.Page.LoadLibraryFromAssembly(reader.ReadString());
            return true;
        }

        /// <summary>
        /// (5:102) print {...} to the console.
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// </returns>
        private bool PrintToConsole(TriggerReader reader)
        {
            string output = reader.ReadString();
            Console.WriteLine(output);
            return true;
        }

        /// <summary>
        /// (5:105) raise an error.
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// </returns>
        private bool RaiseAnError(TriggerReader reader)
        {
            string errorMsg = "";
            if (reader.PeekString()) errorMsg = reader.ReadString();
            RaiseError(errorMsg);
            return false;
        }

        /// <summary>
        /// (5:104) create random number and put it into variable %.
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// </returns>
        private bool RandomValueToVar(TriggerReader reader)
        {
            Variable var = reader.ReadVariable(true);
            var.Value = (double)new Random().Next();
            return true;
        }

        /// <summary>
        /// (5:101) set variable % to #.
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// </returns>
        private bool SetVariableToNumberOrVariable(TriggerReader reader)
        {
            if (reader.PeekVariable() == false) return false;

            Variable var = reader.ReadVariable(true);
            if (reader.PeekVariable())
            {
                Variable otherVar = reader.ReadVariable();
                var.Value = otherVar.Value;
            }
            else if (reader.PeekNumber())
            {
                var.Value = reader.ReadNumber();
            }

            return true;
        }

        /// <summary>
        /// (5:100) set variable % to {...}.
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// </returns>
        private bool SetVariableToString(TriggerReader reader)
        {
            if (reader.PeekVariable() == false) return false;

            Variable var = reader.ReadVariable(true);
            var.Value = reader.ReadString(true);
            return true;
        }

        /// <summary>
        /// (1:106) and variable % is constant,
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// </returns>
        private bool VariableIsConstant(TriggerReader reader)
        {
            return reader.ReadVariable().IsConstant;
        }

        /// <summary>
        /// (1:107) and variable % is not constant,
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// </returns>
        private bool VariableIsNotConstant(TriggerReader reader)
        {
            return VariableIsConstant(reader) == false;
        }

        #endregion Private Methods
    }
}