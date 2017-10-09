namespace Monkeyspeak.lexical.Expressions
{
    /// <summary>
    /// Expression pointing to a Variable reference
    /// <para>This expression does not have the value of the variable because the variable would not have been assigned yet</para>
    /// </summary>
    public sealed class VariableExpression : Expression<string>
    {
        public VariableExpression(ref SourcePosition pos, string varRef) : base(ref pos)
        {
            Value = varRef;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString() { return Value; }
    }
}