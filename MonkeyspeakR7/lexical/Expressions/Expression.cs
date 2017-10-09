using System;

namespace Monkeyspeak.lexical.Expressions
{
    public class Expression : IExpression<object>, IComparable<Expression>
    {
        private readonly SourcePosition sourcePosition;

        protected Expression(ref SourcePosition pos)
        {
            sourcePosition = pos;
        }

        public SourcePosition Position
        {
            get { return sourcePosition; }
        }

        public object Value { get; set; }

        public int CompareTo(Expression other)
        {
            return Value.GetHashCode().CompareTo(other.GetHashCode());
        }

        public override string ToString()
        {
            return String.Format("{0} at {1}", GetType().Name, sourcePosition);
        }
    }

    public class Expression<T> : IExpression<T>, IComparable<Expression<T>>
    {
        private readonly SourcePosition sourcePosition;

        protected Expression(ref SourcePosition pos)
        {
            sourcePosition = pos;
        }

        public SourcePosition Position
        {
            get { return sourcePosition; }
        }

        public T Value { get; set; }

        public int CompareTo(Expression<T> other)
        {
            return Value.GetHashCode().CompareTo(other.GetHashCode());
        }

        public override string ToString()
        {
            return String.Format("{0} at {1}", GetType().Name, sourcePosition);
        }
    }
}