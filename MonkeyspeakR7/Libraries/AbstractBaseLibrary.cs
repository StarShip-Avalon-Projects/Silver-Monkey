using System;
using System.Collections.Generic;
using System.Text;

namespace Monkeyspeak.Libraries
{
    public abstract class AbstractBaseLibrary
    {
        internal Dictionary<Trigger, TriggerHandler> handlers;
        internal Dictionary<Trigger, string> descriptions;

        /// <summary>
        /// Base abstract class for Monkeyspeak Libraries
        /// </summary>
        protected AbstractBaseLibrary()
        {
            handlers = new Dictionary<Trigger, TriggerHandler>();
            descriptions = new Dictionary<Trigger, string>();
        }

        /// <summary>
        /// Raises a MonkeyspeakException
        /// </summary>
        /// <param name="reason">Reason for the error</param>
        public virtual void RaiseError(string reason)
        {
            throw new MonkeyspeakException(reason);
        }

        /// <summary>
        /// Registers a Trigger to the TriggerHandler with optional description
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="handler"></param>
        /// <param name="description"></param>
        public virtual void Add(Trigger trigger, TriggerHandler handler, string description = null)
        {
            if (description != null && !descriptions.ContainsKey(trigger)) descriptions.Add(trigger, description);
            if (!handlers.ContainsKey(trigger))
                handlers.Add(trigger, handler);
            else throw new UnauthorizedAccessException($"Override of existing Trigger {trigger}'s handler with handler in {handler.Method}.");
        }

        /// <summary>
        /// Registers a Trigger to the TriggerHandler with optional description
        /// </summary>
        /// <param name="cat"></param>
        /// <param name="id"></param>
        /// <param name="handler"></param>
        /// <param name="description"></param>
        public virtual void Add(TriggerCategory cat, int id, TriggerHandler handler, string description = null)
        {
            Trigger trigger = new Trigger(cat, id);
            if (description != null) descriptions.Add(trigger, description);
            if (!handlers.ContainsKey(trigger))
                handlers.Add(trigger, handler);
            else throw new UnauthorizedAccessException($"Override of existing Trigger {trigger}'s handler with handler in {handler.Method}.");
        }

        /// <summary>
        /// Called when [page disposing].
        /// </summary>
        /// <param name="page">The page.</param>
        public abstract void OnPageDisposing(Page page);

        /// <summary>
        /// Builds a string representation of the descriptions of each trigger.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(GetType().Name);
            foreach (var kv in descriptions)
            {
                sb.AppendLine(kv.Value);
            }
            return sb.ToString();
        }
    }
}