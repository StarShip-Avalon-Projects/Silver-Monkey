using Monkeyspeak.Libraries;
using System;
using System.Reflection;

namespace Monkeyspeak
{
    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class TriggerHandlerAttribute : Attribute
    {
        private TriggerCategory triggerCategory;
        private int triggerID;
        private string description;
        internal MethodInfo owner;

        /// <summary>
        ///
        /// </summary>
        /// <param name="category">Trigger Category</param>
        /// <param name="id">Trigger ID</param>
        /// <param name="description">Trigger Description</param>
        public TriggerHandlerAttribute(TriggerCategory category, int id, string description)
        {
            this.triggerCategory = category;
            this.triggerID = id;
            this.description = description;
        }

        internal void Register(Page page)
        {
            var handler = (TriggerHandler)(reader => (bool)owner.Invoke(null, new object[] { reader }));
            if (handler != null)
            {
                Attributes.Instance.Add(new Trigger(triggerCategory, triggerID), handler, description);
                page.LoadLibrary(Attributes.Instance); // reload the library
            }
        }
    }
}