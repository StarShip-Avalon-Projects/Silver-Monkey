using Monkeyspeak;
using System;

namespace Libraries.Variables
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Monkeyspeak.IVariable" />
    public sealed class WebVariable : IVariable
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WebVariable"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public WebVariable(string name, object value = null)
        {
            Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebVariable"/> class.
        /// </summary>
        /// <param name="variable">The variable.</param>
        public WebVariable(IVariable variable)
        {
            Name = variable.Name;
            this.Value = variable.Value;
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
        public object Value { get; set; }

        #endregion Public Properties

        #region Public Methods

        public bool Equals(IVariable other)
        {
            return Name.Equals(other.Name, StringComparison.InvariantCultureIgnoreCase) && this.Value.Equals(other.Value);
        }

        public void SetValue(object value)
        {
            Value = value;
        }

        /// <summary>
        /// Returns a const identifier if the variable is constant followed by name,
        /// <para>otherwise just the name is returned.</para>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return IsConstant ? "const " : string.Empty + $"{Name} = {Value ?? "null"}";
        }

        #endregion Public Methods
    }
}