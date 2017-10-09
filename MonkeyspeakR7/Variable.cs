using System;
using System.Collections.Generic;
using System.Linq;

namespace Monkeyspeak
{
    [Serializable]
    public class VariableIsConstantException : Exception
    {
        public VariableIsConstantException()
        {
        }

        public VariableIsConstantException(string message)
            : base(message)
        {
        }

        public VariableIsConstantException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected VariableIsConstantException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    public interface IVariable : IEquatable<IVariable>
    {
        string Name { get; }
        object Value { get; set; }
        bool IsConstant { get; }
    }

    [Serializable]
    public class Variable : IVariable
    {
        public bool Equals(IVariable other)
        {
            return Equals(value, other.Value) && string.Equals(name, other.Name);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = IsConstant.GetHashCode();
                hashCode = (hashCode * 397) ^ (value != null ? value.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (name != null ? name.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static readonly IVariable NoValue = new Variable("none", null, true);

        public bool IsConstant
        {
            get;
            private set;
        }

        private object value;

        public object Value
        {
            get { return value; }
            set
            {
                // removed Value = as it interfered with page.setVariable - Gerolkae
                if (!CheckType(value)) throw new TypeNotSupportedException(value.GetType().Name +
                " is not a supported type. Expecting string or double.");

                if (value != null && IsConstant)
                    throw new VariableIsConstantException($"Attempt to assign a value to constant '{Name}'");
                this.value = value;
            }
        }

        private string name;

        public string Name
        {
            get
            {
                return name;
            }
            internal set
            {
                name = value;
            }
        }

        internal Variable(string name, bool constant = false)
        {
            IsConstant = constant;
            this.name = name;
        }

        internal Variable(string name, object value, bool constant = false)
        {
            IsConstant = constant;
            this.name = name;
            this.value = value;
        }

        public void ForceAssignValue(object _value)
        {
            value = _value;
        }

        private bool CheckType(object _value)
        {
            if (_value == null) return true;

            return _value is string ||
                   _value is double;
        }

        /// <summary>
        /// Returns a const identifier if the variable is constant followed by name,
        /// <para>otherwise just the name is returned.</para>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ((IsConstant) ? "const " : "") + $"{name} = {value ?? "null"}";
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="asConstant">Clone as Constant</param>
        /// <returns></returns>
        public Variable Clone(bool asConstant = false)
        {
            return new Variable(name, value, asConstant);
        }

        public static bool operator ==(Variable varA, Variable varB)
        {
            return varA.Value == varB.Value;
        }

        public static bool operator !=(Variable varA, Variable varB)
        {
            return varA.Value != varB.Value;
        }

        public static Variable operator +(Variable varA, double num)
        {
            varA.Value = varA.AsDouble() + num;
            return varA;
        }

        public static Variable operator -(Variable varA, double num)
        {
            varA.Value = varA.AsDouble() - num;
            return varA;
        }

        public static Variable operator *(Variable varA, double num)
        {
            if (varA.Value is double)
            {
                varA.Value = varA.AsDouble() * num;
            }
            return varA;
        }

        public static Variable operator /(Variable varA, double num)
        {
            varA.Value = varA.AsDouble() / num;
            return varA;
        }

        public static Variable operator +(Variable varA, string str)
        {
            varA.Value = varA.AsString() + str;
            return varA;
        }

        public static implicit operator string(Variable var)
        {
            return var.AsString();
        }

        public static implicit operator double(Variable var)
        {
            double num;
            Double.TryParse(var.Value as string, out num);
            return num;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is Variable && Equals((Variable)obj);
        }

        public double AsDouble()
        {
            double num = 0;
            if (value == null)
            {
                value = 0d;
            }
            if (double.TryParse(value.ToString(), out num))
                return num;
            return 0d;
        }

        public string AsString()
        {
            return Convert.ToString(Value);
        }
    }

    [Serializable]
    public sealed class VariableList : IVariable
    {
        public string Name { get; private set; }

        private List<object> values;

        public object Value
        {
            get { return values.FirstOrDefault(); } // this is intended behavior just in case you try to get variable value without [index]
            set { this.values.Add(value); }
        }

        public object this[int index]
        {
            get { return values[index]; }
        }

        public int Count { get => values.Count; }

        public bool IsConstant { get; private set; }

        public VariableList(string name, bool isConstant = false)
        {
            Name = name;
            this.values = new List<object>();
            IsConstant = isConstant;
        }

        public VariableList(string name, IList<object> value, bool isConstant = false)
        {
            Name = name;
            this.values = new List<object>(value.AsEnumerable());
            IsConstant = isConstant;
        }

        public bool Equals(IVariable other)
        {
            return Equals(values, other.Value) && string.Equals(Name, other.Name);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = IsConstant.GetHashCode();
                hashCode = (hashCode * 397) ^ (values != null ? values.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}