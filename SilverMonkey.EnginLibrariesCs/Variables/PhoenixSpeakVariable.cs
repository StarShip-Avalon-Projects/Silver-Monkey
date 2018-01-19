using Monkeyspeak;
using System;

namespace Engine.Libraries.Variables
{
    public sealed class PhoenixSpeakVariable : IVariable
    {
        #region Private Fields

        private object value;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoenixSpeakVariable"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public PhoenixSpeakVariable(string name, object value)
        {
            Name = name;
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoenixSpeakVariable"/> class.
        /// </summary>
        /// <param name="variable">The variable.</param>
        public PhoenixSpeakVariable(IVariable variable)
        {
            Name = variable.Name;
            this.value = variable.Value;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether this instance is constant.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is constant; otherwise, <c>false</c>.
        /// </value>
        public bool IsConstant => false;

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value
        {
            get { return value; }
            set
            {
                this.value = value;
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(IVariable other)
        {
            return this.Name.Equals(other.Name, StringComparison.InvariantCultureIgnoreCase) && this.Value.Equals(other.Value);
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetValue(object value)
        {
            this.value = value;
        }

        /// <summary>
        /// Returns a const identifier if the variable is constant followed by name,
        /// <para>otherwise just the name is returned.</para>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ((IsConstant) ? "const " : "") + $"{Name} = {value ?? "null"}";
        }

        #endregion Public Methods
    }
}