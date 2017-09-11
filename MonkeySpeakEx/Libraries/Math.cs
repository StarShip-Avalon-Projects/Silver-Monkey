﻿using System;

namespace Monkeyspeak.Libraries
{
    /// <summary>
    /// Monkey Speak Math Library
    /// </summary>
    internal class Math : AbstractBaseLibrary
    {
        #region Public Constructors

        public Math()
        {
            // (1:150) and variable %Variable is greater than #,
            Add(new Trigger(TriggerCategory.Condition, 150), VariableGreaterThan,
                "(1:150) and variable %Variable is greater than #,");
            // (1:151) and variable %Variable is greater than or equal to #,
            Add(new Trigger(TriggerCategory.Condition, 151), VariableGreaterThanOrEqual,
                "(1:151) and variable %Variable is greater than or equal to #,");

            // (1:152) and variable %Variable is less than #,
            Add(new Trigger(TriggerCategory.Condition, 152), VariableLessThan,
                "(1:152) and variable %Variable is less than #,");

            // (1:153) and variable %Variable is less than or equal to #,
            Add(new Trigger(TriggerCategory.Condition, 153), VariableLessThanOrEqual,
                "(1:153) and variable %Variable is less than or equal to #,");

            // (5:150) take variable %Variable and add # to it.
            Add(new Trigger(TriggerCategory.Effect, 150), AddToVariable,
                "(5:150) take variable %Variable and add # to it.");

            // (5:151) take variable %Variable and substract it by #.
            Add(new Trigger(TriggerCategory.Effect, 151), SubtractFromVariable,
                "(5:151) take variable %Variable and subtract # from it.");

            // (5:152) take variable %Variable and multiply it by #.
            Add(new Trigger(TriggerCategory.Effect, 152), MultiplyByVariable,
                "(5:152) take variable %Variable and multiply it by #.");

            // (5:153) take variable %Variable and divide it by #.
            Add(new Trigger(TriggerCategory.Effect, 153), DivideByVariable,
                "(5:153) take variable %Variable and divide it by #.");
        }

        #endregion Public Constructors

        #region Private Methods

        /// <summary>
        /// (5:150) take variable %Variable and add # to it.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// </returns>
        private bool AddToVariable(TriggerReader reader)
        {
            Variable var = reader.ReadVariable(true);
            double num = 0;
            double numOut = 0;
            if (reader.PeekNumber())
            {
                num = reader.ReadNumber();
            }

            if (Double.TryParse(var.Value.ToString(), out numOut) == true)
            {
                var.Value = (numOut + num);
            }
            else var.Value = num;

            return true;
        }

        /// <summary>
        /// (5:153) take variable %Variable and divide it by #.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// </returns>
        private bool DivideByVariable(TriggerReader reader)
        {
            Variable var = reader.ReadVariable(true);
            double num = 0;
            double numOut = 0;
            if (reader.PeekNumber())
            {
                num = reader.ReadNumber();
            }

            if (Double.TryParse(var.Value.ToString(), out numOut) == true)
            {
                var.Value = numOut / num;
            }
            else var.Value = num;
            return true;
        }

        /// <summary>
        /// "(5:152) take variable %Variable and multiply it by #.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// </returns>
        private bool MultiplyByVariable(TriggerReader reader)
        {
            Variable var = reader.ReadVariable(true);
            double num = 0;
            double numOut = 0;
            if (reader.PeekNumber())
            {
                num = reader.ReadNumber();
            }

            if (Double.TryParse(var.Value.ToString(), out numOut) == true)
            {
                var.Value = numOut * num;
            }
            else var.Value = num;
            return true;
        }

        /// <summary>
        /// (5:151) take variable %Variable and subtract # from it.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// </returns>
        private bool SubtractFromVariable(TriggerReader reader)
        {
            Variable var = reader.ReadVariable(true);
            double num = 0;
            double numOut = 0;
            if (reader.PeekNumber())
            {
                num = reader.ReadNumber();
            }

            if (Double.TryParse(var.Value.ToString(), out numOut) == true)
            {
                var.Value = numOut - num;
            }
            else var.Value = num;
            return true;
        }

        /// <summary>
        /// (1:150) and variable %Variable is greater than #,
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// </returns>
        private bool VariableGreaterThan(TriggerReader reader)
        {
            Variable mainVar = reader.ReadVariable();
            Variable var;
            double num = 0;
            double mainNum = 0;
            double Num = 0;
            if (reader.TryReadVariable(out var))
            {
                if (Double.TryParse(mainVar.Value.ToString(), out mainNum) && Double.TryParse(var.Value.ToString(), out Num))
                    return mainNum > Num;
                if (Double.TryParse(mainVar.Value.ToString(), out mainNum) == false && Double.TryParse(var.Value.ToString(), out Num))
                    return 0.0 > Num;
                if (Double.TryParse(mainVar.Value.ToString(), out mainNum) && Double.TryParse(var.Value.ToString(), out Num) == false)
                    return mainNum > 0.0;
            }
            else if (reader.TryReadNumber(out num))
            {
                if (Double.TryParse(mainVar.Value.ToString(), out mainNum))
                    return mainNum > num;
            }
            return false;
        }

        /// <summary>
        /// (1:151) and variable %Variable is greater than or equal to #,
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// </returns>
        private bool VariableGreaterThanOrEqual(TriggerReader reader)
        {
            Variable mainVar = reader.ReadVariable();
            Variable var;
            double num;
            double mainNum;
            double Num;
            if (reader.TryReadVariable(out var))
            {
                if (Double.TryParse(mainVar.Value.ToString(), out mainNum) && Double.TryParse(var.Value.ToString(), out Num))
                    return mainNum >= Num;
                if (Double.TryParse(mainVar.Value.ToString(), out mainNum) == false && Double.TryParse(var.Value.ToString(), out Num))
                    return 0.0 >= Num;
                if (Double.TryParse(mainVar.Value.ToString(), out mainNum) && Double.TryParse(var.Value.ToString(), out Num) == false)
                    return mainNum >= 0.0;
                return false;
            }
            else if (reader.TryReadNumber(out num))
            {
                if (Double.TryParse(mainVar.Value.ToString(), out mainNum))
                    return mainNum >= num;
                return false;
            }
            return false;
        }

        /// <summary>
        /// (1:152) and variable %Variable is less than #,
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// </returns>
        private bool VariableLessThan(TriggerReader reader)
        {
            Variable mainVar = reader.ReadVariable();
            Variable var;
            double num;
            double mainNum;
            double Num;
            if (reader.TryReadVariable(out var))
            {
                if (Double.TryParse(mainVar.Value.ToString(), out mainNum) && Double.TryParse(var.Value.ToString(), out Num))
                    return mainNum < Num;
                if (Double.TryParse(mainVar.Value.ToString(), out mainNum) == false && Double.TryParse(var.Value.ToString(), out Num))
                    return 0.0 < Num;
                if (Double.TryParse(mainVar.Value.ToString(), out mainNum) && Double.TryParse(var.Value.ToString(), out Num) == false)
                    return mainNum < 0.0;
                return false;
            }
            else if (reader.TryReadNumber(out num))
            {
                if (Double.TryParse(mainVar.Value.ToString(), out mainNum))
                    return mainNum < num;
                return false;
            }
            return false;
        }

        /// <summary>
        /// (1:153) and variable %Variable is less than or equal to #,
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// </returns>
        private bool VariableLessThanOrEqual(TriggerReader reader)
        {
            Variable mainVar = reader.ReadVariable();
            Variable var;
            double num;
            double mainNum;
            double Num;
            if (reader.TryReadVariable(out var))
            {
                if (Double.TryParse(mainVar.Value.ToString(), out mainNum) && Double.TryParse(var.Value.ToString(), out Num))
                    return mainNum <= Num;
                if (Double.TryParse(mainVar.Value.ToString(), out mainNum) == false && Double.TryParse(var.Value.ToString(), out Num))
                    return 0.0 <= Num;
                if (Double.TryParse(mainVar.Value.ToString(), out mainNum) && Double.TryParse(var.Value.ToString(), out Num) == false)
                    return mainNum <= 0.0;
                return false;
            }
            else if (reader.TryReadNumber(out num))
            {
                if (Double.TryParse(mainVar.Value.ToString(), out mainNum))
                    return mainNum <= num;
                return false;
            }
            return false;
        }

        #endregion Private Methods
    }
}