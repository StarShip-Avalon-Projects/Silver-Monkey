using System;

namespace Monkeyspeak.Libraries
{
    internal class Math : AbstractBaseLibrary
    {
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
            Add(new Trigger(TriggerCategory.Effect, 153), MultiplyByVariable,
                "(5:153) take variable %Variable and divide it by #.");
        }

        public override void OnPageDisposing(Page page)
        {
        }

        private bool AddToVariable(TriggerReader reader)
        {
            var toAssign = reader.ReadVariable(true);
            double num = 0;
            if (reader.PeekVariable())
            {
                var valueVariable = reader.ReadVariable();
                if (valueVariable.Value is double)
                    num = Convert.ToDouble(valueVariable.Value);
            }
            else if (reader.PeekNumber())
            {
                num = reader.ReadNumber();
            }

            toAssign.Value = Convert.ToDouble(toAssign.Value) + num;
            return true;
        }

        private bool DivideByVariable(TriggerReader reader)
        {
            var toAssign = reader.ReadVariable(true);
            double num = 0;
            if (reader.PeekVariable())
            {
                var valueVariable = reader.ReadVariable();
                if (valueVariable.Value is double)
                    num = Convert.ToDouble(valueVariable.Value);
            }
            else if (reader.PeekNumber())
            {
                num = reader.ReadNumber();
            }

            toAssign.Value = Convert.ToDouble(toAssign.Value) / num;
            return true;
        }

        private bool MultiplyByVariable(TriggerReader reader)
        {
            var toAssign = reader.ReadVariable(true);
            double num = 0;
            if (reader.PeekVariable())
            {
                var valueVariable = reader.ReadVariable();
                if (valueVariable.Value is double)
                    num = Convert.ToDouble(valueVariable.Value);
            }
            else if (reader.PeekNumber())
            {
                num = reader.ReadNumber();
            }

            toAssign.Value = Convert.ToDouble(toAssign.Value) * num;
            return true;
        }

        private bool SubtractFromVariable(TriggerReader reader)
        {
            var toAssign = reader.ReadVariable(true);
            double num = 0;
            if (reader.PeekVariable())
            {
                var valueVariable = reader.ReadVariable();
                if (valueVariable.Value is double)
                    num = Convert.ToDouble(valueVariable.Value);
            }
            else if (reader.PeekNumber())
            {
                num = reader.ReadNumber();
            }

            toAssign.Value = Convert.ToDouble(toAssign.Value) - num;
            return true;
        }

        private bool VariableGreaterThan(TriggerReader reader)
        {
            var mainVar = reader.ReadVariable();
            double num = 0;
            double mainNum = 0;
            double Num = 0;
            if (reader.TryReadVariable(out IVariable var))
            {
                if (Double.TryParse(mainVar.Value.ToString(), out mainNum) && Double.TryParse(var.Value.ToString(), out Num))
                    return mainNum > Num;
                if (!Double.TryParse(mainVar.Value.ToString(), out mainNum) && Double.TryParse(var.Value.ToString(), out Num))
                    return 0.0 > Num;
                if (Double.TryParse(mainVar.Value.ToString(), out mainNum) && !Double.TryParse(var.Value.ToString(), out Num))
                    return mainNum > 0.0;
            }
            else if (reader.TryReadNumber(out num))
            {
                if (Double.TryParse(mainVar.Value.ToString(), out mainNum))
                    return mainNum > num;
            }
            return false;
        }

        private bool VariableGreaterThanOrEqual(TriggerReader reader)
        {
            var mainVar = reader.ReadVariable();
            double num;
            double mainNum;
            double Num;
            if (reader.TryReadVariable(out IVariable var))
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

        private bool VariableLessThan(TriggerReader reader)
        {
            var mainVar = reader.ReadVariable();
            double num;
            double mainNum;
            double Num;
            if (reader.TryReadVariable(out IVariable var))
            {
                if (Double.TryParse(mainVar.Value.ToString(), out mainNum) && Double.TryParse(var.Value.ToString(), out Num))
                    return mainNum < Num;
                if (!Double.TryParse(mainVar.Value.ToString(), out mainNum) && Double.TryParse(var.Value.ToString(), out Num))
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

        private bool VariableLessThanOrEqual(TriggerReader reader)
        {
            var mainVar = reader.ReadVariable();
            double num;
            double mainNum;
            double Num;
            if (reader.TryReadVariable(out IVariable var))
            {
                if (Double.TryParse(mainVar.Value.ToString(), out mainNum) && Double.TryParse(var.Value.ToString(), out Num))
                    return mainNum <= Num;
                if (!Double.TryParse(mainVar.Value.ToString(), out mainNum) && Double.TryParse(var.Value.ToString(), out Num))
                    return 0.0 <= Num;
                if (Double.TryParse(mainVar.Value.ToString(), out mainNum) && Double.TryParse(var.Value.ToString(), out Num) == false)
                    return mainNum <= 0.0;
                return false;
            }
            if (reader.TryReadNumber(out num))
            {
                if (Double.TryParse(mainVar.Value.ToString(), out mainNum))
                    return mainNum <= num;
                return false;
            }
            return false;
        }
    }
}