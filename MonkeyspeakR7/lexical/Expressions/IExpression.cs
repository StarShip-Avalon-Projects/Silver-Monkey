namespace Monkeyspeak.lexical.Expressions
{
    internal interface IExpression
    {
        SourcePosition Position { get; }
    }

    internal interface IExpression<T>
    {
        SourcePosition Position { get; }
        T Value { get; set; }
    }
}