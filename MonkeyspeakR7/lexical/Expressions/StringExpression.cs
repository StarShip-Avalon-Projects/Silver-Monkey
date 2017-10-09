namespace Monkeyspeak.lexical.Expressions
{
    public sealed class StringExpression : Expression<string>
    {
        public StringExpression(SourcePosition pos, string value)
            : base(ref pos) { Value = value; }
    }
}