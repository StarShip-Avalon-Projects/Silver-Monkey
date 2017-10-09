using Monkeyspeak.lexical;
using Monkeyspeak.Libraries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Monkeyspeak
{
    [Serializable]
    public class TypeNotSupportedException : Exception
    {
        public TypeNotSupportedException()
        {
        }

        public TypeNotSupportedException(string message)
            : base(message)
        {
        }

        public TypeNotSupportedException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected TypeNotSupportedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Used for handling triggers at runtime.
    /// </summary>
    /// <param name="reader"></param>
    /// <returns>True = Continue to the Next Trigger, False = Stop executing current block of Triggers</returns>
    public delegate bool TriggerHandler(TriggerReader reader);

    public delegate void TriggerAddedEventHandler(Trigger trigger, TriggerHandler handler);

    public delegate bool TriggerHandledEventHandler(Trigger trigger);

    /// <summary>
    /// Event for any errors that occur during execution
    /// If not assigned Exceptions will be thrown.
    /// </summary>
    /// <param name="trigger"></param>
    /// <param name="ex"></param>
    public delegate void TriggerHandlerErrorEvent(TriggerHandler handler, Trigger trigger, Exception ex);

    public delegate Token TokenVisitorHandler(ref Token token);

    [Serializable]
    public sealed class Page : IDisposable
    {
        public object syncObj = new Object();

        private Parser parser;
        private List<TriggerList> triggerBlocks;
        private volatile HashSet<IVariable> scope;
        private HashSet<AbstractBaseLibrary> libraries;

        private volatile Dictionary<Trigger, TriggerHandler> handlers = new Dictionary<Trigger, TriggerHandler>();
        private MonkeyspeakEngine engine;

        public event Action Resetting;

        internal TokenVisitorHandler VisitingToken;

        /// <summary>
        /// Called when an Exception is raised during execution
        /// </summary>
        public event TriggerHandlerErrorEvent Error;

        /// <summary>
        /// Called when a Trigger and TriggerHandler is added to the Page
        /// </summary>
        public event TriggerAddedEventHandler TriggerAdded;

        /// <summary>
        /// Called before the Trigger's TriggerHandler is called.  If there is no TriggerHandler for that Trigger
        /// then this event is not raised.
        /// </summary>
        public event TriggerHandledEventHandler BeforeTriggerHandled;

        /// <summary>
        /// Called after the Trigger's TriggerHandler is called.  If there is no TriggerHandler for that Trigger
        /// then this event is not raised.
        /// </summary>
        public event TriggerHandledEventHandler AfterTriggerHandled;

        internal Page(MonkeyspeakEngine engine)
        {
            this.engine = engine;
            parser = new Parser(engine);
            triggerBlocks = new List<TriggerList>();
            scope = new HashSet<IVariable>();
            libraries = new HashSet<AbstractBaseLibrary>();
            Initiate();
        }

        internal void Initiate()
        {
            LoadLibrary(Attributes.Instance);
            scope.Add(Variable.NoValue);
            scope.Add(new Variable("MONKEY", engine.Banner, true));
            scope.Add(new Variable("VERSION", engine.Options.Version.ToString(2), true));
        }

        internal void GenerateBlocks(AbstractLexer lexer)
        {
            parser.VisitToken = VisitingToken;
            var blocksArray = parser.Parse(lexer).ToArray();
            triggerBlocks.AddRange(blocksArray);
            for (int i = blocksArray.Length - 1; i >= 0; i--)
            {
                Size += blocksArray[i].Count;
            }
            if (Size > engine.Options.TriggerLimit)
            {
                TrimToLimit(Size);
                throw new Exception("Trigger limit exceeded.");
            }
        }

        internal void AddBlocks(IEnumerable<TriggerList> blocks)
        {
            var blocksArray = blocks.ToArray();
            triggerBlocks.AddRange(blocksArray);
            for (int i = blocksArray.Length - 1; i >= 0; i--)
            {
                Size += blocksArray[i].Count;
            }
            if (Size > engine.Options.TriggerLimit)
            {
                TrimToLimit(Size);
                throw new Exception("Trigger limit exceeded.");
            }
        }

        private void TrimToLimit(int limit)
        {
            lock (syncObj)
            {
                for (int i = triggerBlocks.Count - 1; i >= 0; i--)
                {
                    for (int j = triggerBlocks[i].Count - 1; j >= 0; j--)
                    {
                        var curSize = i + j;
                        if (curSize >= limit)
                        {
                            triggerBlocks[i].RemoveAt(j);
                        }
                    }
                    if (triggerBlocks[i].Count == 0) triggerBlocks.RemoveAt(i);
                }
            }
        }

        internal bool CheckType(object value)
        {
            if (value == null) return true;

            return value is string ||
                   value is double;
        }

        internal MonkeyspeakEngine Engine
        {
            get { return engine; }
        }

        public void CompileToStream(Stream stream)
        {
            try
            {
                Compiler compiler = new Compiler(engine);
                compiler.CompileToStream(triggerBlocks, stream);
            }
            catch (Exception ex)
            {
                throw new MonkeyspeakException(String.Format("Could not compile to file.  Reason:{0}", ex.Message), ex);
            }
        }

        public void CompileToFile(string filePath)
        {
            try
            {
                var compiler = new Compiler(engine);
                using (Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read, 2048))
                {
                    compiler.CompileToStream(triggerBlocks, stream);
                }
            }
            catch (IOException ioex)
            {
                throw ioex;
            }
            catch (Exception ex)
            {
                throw new MonkeyspeakException(String.Format("Could not compile to file.  Reason:{0}", ex.Message), ex);
            }
        }

        /// <summary>
        /// Clears all Variables and optionally clears all TriggerHandlers from this Page.
        /// </summary>
        public void Reset(bool resetTriggerHandlers = false)
        {
            Resetting?.Invoke();
            scope.Clear();
            Initiate();
            if (resetTriggerHandlers) handlers.Clear();
        }

        /// <summary>
        /// Removes the triggers.
        /// </summary>
        public void Clear()
        {
            triggerBlocks.Clear();
            Size = 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>IEnumerable of Triggers</returns>
        public IEnumerable<string> GetTriggerDescriptions()
        {
            lock (syncObj)
            {
                foreach (var lib in libraries)
                {
                    yield return lib.ToString();
                }
            }
        }

        public ReadOnlyCollection<IVariable> Scope
        {
            get { return new ReadOnlyCollection<IVariable>(scope.ToArray()); }
        }

        /// <summary>
        /// Loads Monkeyspeak Sys Library into this Page
        /// <para>Used for System operations involving the Environment or Operating System</para>
        /// </summary>
        public void LoadSysLibrary()
        {
            LoadLibrary(new Libraries.Sys());
        }

        /// <summary>
        /// Loads Monkeyspeak String Library into this Page
        /// <para>Used for basic String operations</para>
        /// </summary>
        public void LoadStringLibrary()
        {
            LoadLibrary(new Libraries.StringOperations());
        }

        /// <summary>
        /// Loads Monkeyspeak IO Library into this Page
        /// <para>Used for File Input/Output operations</para>
        /// </summary>
        public void LoadIOLibrary()
        {
            LoadLibrary(new Libraries.IO());
        }

        /// <summary>
        /// Loads Monkeyspeak Math Library into this Page
        /// <para>Used for Math operations (add, subtract, multiply, divide)</para>
        /// </summary>
        public void LoadMathLibrary()
        {
            LoadLibrary(new Libraries.Math());
        }

        /// <summary>
        /// Loads Monkeyspeak Timer Library into this Page
        /// </summary>
        public void LoadTimerLibrary()
        {
            LoadLibrary(new Libraries.Timers());
        }

        /// <summary>
        /// Loads Monkeyspeak Debug Library into this Page
        /// <para>Used for Debug breakpoint insertion. Won't work without Debugger attached.</para>
        /// </summary>
        public void LoadDebugLibrary()
        {
            LoadLibrary(new Libraries.Debug());
        }

        /// <summary>
        /// Loads a <see cref="Libraries.AbstractBaseLibrary"/> into this Page
        /// </summary>
        /// <param name="lib"></param>
        public void LoadLibrary(Libraries.AbstractBaseLibrary lib)
        {
            if (libraries.Contains(lib)) return;
            lock (syncObj)
            {
                foreach (var kv in lib.handlers)
                {
                    SetTriggerHandler(kv.Key, kv.Value);
                }

                libraries.Add(lib);
            }
        }

        /// <summary>
        /// Loads trigger handlers from a assembly instance
        /// </summary>
        /// <param name="assemblyFile">The assembly instance</param>
        public void LoadLibraryFromAssembly(Assembly asm)
        {
            if (asm == null) return;
            foreach (var types in ReflectionHelper.GetAllTypesWithAttributeInMembers<TriggerHandlerAttribute>(asm))
                foreach (MethodInfo method in types.GetMethods().Where(method => method.IsDefined(typeof(TriggerHandlerAttribute), false)))
                {
                    foreach (TriggerHandlerAttribute attribute in ReflectionHelper.GetAllAttributesFromMethod<TriggerHandlerAttribute>(method))
                    {
                        attribute.owner = method;
                        try
                        {
                            attribute.Register(this);
                        }
                        catch (Exception ex)
                        {
                            throw new MonkeyspeakException(String.Format("Failed to load library from assembly '{0}', couldn't bind to method '{1}.{2}'", asm.FullName, method.DeclaringType.Name, method.Name), ex);
                        }
                    }
                }
        }

        /// <summary>
        /// Loads trigger handlers from a assembly dll file
        /// </summary>
        /// <param name="assemblyFile">The assembly in the local file system</param>
        public void LoadLibraryFromAssembly(string assemblyFile)
        {
            Assembly asm;
            if (!File.Exists(assemblyFile))
                throw new MonkeyspeakException("Load library from file '" + assemblyFile + "' failed, file not found.");
            else if (!ReflectionHelper.TryLoad(assemblyFile, out asm))
            {
                throw new MonkeyspeakException("Load library from file '" + assemblyFile + "' failed.");
            }

            foreach (var types in ReflectionHelper.GetAllTypesWithAttributeInMembers<TriggerHandlerAttribute>(asm))
                foreach (MethodInfo method in types.GetMethods().Where(method => method.IsDefined(typeof(TriggerHandlerAttribute), false)))
                {
                    foreach (TriggerHandlerAttribute attribute in ReflectionHelper.GetAllAttributesFromMethod<TriggerHandlerAttribute>(method))
                    {
                        attribute.owner = method;
                        try
                        {
                            attribute.Register(this);
                        }
                        catch (Exception ex)
                        {
                            throw new MonkeyspeakException(String.Format("Failed to load library from file '{0}', couldn't bind to method '{1}.{2}'", assemblyFile, method.DeclaringType.Name, method.Name));
                        }
                    }
                }
        }

        public bool RemoveVariable(string name)
        {
            return scope.RemoveWhere(var => var.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)) > 0;
        }

        public void SetVariable<T>(T variable) where T : IVariable
        {
            if (!CheckType(variable.Value)) throw new TypeNotSupportedException(String.Format("{0} is not a supported type. Expecting string or double.", variable.Value.GetType().Name));

            lock (syncObj)
            {
                RemoveVariable(variable.Name);

                if (scope.Count + 1 > engine.Options.VariableCountLimit) throw new Exception("Variable limit exceeded, operation failed.");
                scope.Add(variable);
            }
        }

        public IVariable SetVariable(string name, object value, bool isConstant)
        {
            if (!CheckType(value)) throw new TypeNotSupportedException(String.Format("{0} is not a supported type. Expecting string or double.", value.GetType().Name));

            IVariable var;

            lock (syncObj)
            {
                foreach (var v in scope)
                {
                    if (v.Name.Equals(name))
                    {
                        var = v;
                        var.Value = value;
                        return var;
                    }
                }

                if (scope.Count + 1 > engine.Options.VariableCountLimit) throw new Exception("Variable limit exceeded, operation failed.");
                var = new Variable(name, value, isConstant);
                scope.Add(var);
                return var;
            }
        }

        /// <summary>
        /// Gets a Variable with Name set to <paramref name="name"/>
        /// <b>Throws Exception if Variable not found.</b>
        /// </summary>
        /// <param name="name">The name of the Variable to retrieve</param>
        /// <returns>The variable found with the specified <paramref name="name"/> or throws Exception</returns>
        public IVariable GetVariable(string name)
        {
            if (!name.StartsWith(engine.Options.VariableDeclarationSymbol.ToString()))
                name = engine.Options.VariableDeclarationSymbol + name;
            if (name.IndexOf('[') != -1)
                name = name.Substring(0, name.IndexOf('[') - 1);
            lock (syncObj)
            {
                foreach (var v in scope)
                {
                    if (v.Name.Equals(name))
                    {
                        return v;
                    }
                }
                throw new Exception("Variable \"" + name + "\" not found.");
            }
        }

        /// <summary>
        /// Checks the scope for the Variable with Name set to <paramref name="name"/>
        /// </summary>
        /// <param name="name">The name of the Variable to retrieve</param>
        /// <returns>True on Variable found.  <para>False if Variable not found.</para></returns>
        public bool HasVariable(string name)
        {
            if (!name.StartsWith(engine.Options.VariableDeclarationSymbol.ToString()))
                name = engine.Options.VariableDeclarationSymbol + name;
            if (name.IndexOf('[') != -1)
                name = name.Substring(0, name.IndexOf('[') - 1);
            lock (syncObj)
            {
                foreach (var v in scope)
                {
                    if (v.Name.Equals(name))
                        return true;
                }
                return false;
            }
        }

        public bool HasVariable(string name, out IVariable var)
        {
            if (!name.StartsWith(engine.Options.VariableDeclarationSymbol.ToString()))
                name = engine.Options.VariableDeclarationSymbol + name;
            if (name.IndexOf('[') != -1)
                name = name.Substring(0, name.IndexOf('[') - 1);
            lock (syncObj)
            {
                foreach (var v in scope)
                {
                    if (v.Name.Equals(name))
                    {
                        var = v;
                        return true;
                    }
                }
                var = Variable.NoValue;
                return false;
            }
        }

        /// <summary>
        /// Assigns the TriggerHandler to a trigger with <paramref name="category"/> and <paramref name="id"/>
        /// </summary>
        /// <param name="category"></param>
        /// <param name="id"></param>
        /// <param name="handler"></param>
        /// <param name="description"></param>
        public void SetTriggerHandler(TriggerCategory category, int id, TriggerHandler handler, string description = null)
        {
            SetTriggerHandler(new Trigger(category, id), handler, description);
        }

        /// <summary>
        /// Assigns the TriggerHandler to <paramref name="trigger"/>
        /// </summary>
        /// <param name="trigger"><see cref="Monkeyspeak.Trigger"/></param>
        /// <param name="handler"><see cref="Monkeyspeak.TriggerHandler"/></param>
        /// <param name="description">optional description of the trigger, normally the human readable form of the trigger
        /// <para>Example: "(0:1) when someone says something,"</para></param>
        public void SetTriggerHandler(Trigger trigger, TriggerHandler handler, string description = null)
        {
            Attributes.Instance.AddDescription(trigger, description);
            lock (syncObj)
            {
                if (!handlers.ContainsKey(trigger))
                {
                    handlers.Add(trigger, handler);
                    TriggerAdded?.Invoke(trigger, handler);
                }
                else if (engine.Options.CanOverrideTriggerHandlers)
                {
                    handlers[trigger] = handler;
                }
                else throw new UnauthorizedAccessException($"Override of existing Trigger handler from {handler.Method} for {trigger} set to handler in {handlers[trigger].Method}");
            }
        }

        /// <summary>
        /// Removes the trigger handler.
        /// </summary>
        /// <param name="cat">The category.</param>
        /// <param name="id">The identifier.</param>
        public void RemoveTriggerHandler(TriggerCategory cat, int id)
        {
            lock (syncObj)
            {
                handlers.Remove(new Trigger(cat, id));
            }
        }

        /// <summary>
        /// Removes the trigger handler.
        /// </summary>
        /// <param name="trigger">The trigger.</param>
        public void RemoveTriggerHandler(Trigger trigger)
        {
            lock (syncObj)
            {
                handlers.Remove(trigger);
            }
        }

        /// <summary>
        /// Returns the Trigger count on this Page.
        /// </summary>
        /// <returns></returns>
        public int Size
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Page"/> is in debug mode.
        /// </summary>
        /// <value>
        ///   <c>true</c> if debug; otherwise, <c>false</c>.
        /// </value>
        public bool Debug { get; set; }

        // Changed id to array for multiple Trigger processing.
        // This Compensates for a Design Flaw Lothus Marque spotted - Gerolkae

        /*
         * [1/7/2013 9:26:22 PM] Lothus Marque: Okay. Said feeling doesn't explain why 48 is
         * happening before 46, since your execute does them in increasing order. But what I
         * was suddenly staring at is that this has the definite potential to "run all 46,
         * then run 47, then run 48" ... and they're not all at once, in sequence.
         */

        private void ExecuteBlock<T>(T triggerBlock) where T : IList<Trigger>
        {
            TriggerReader reader = new TriggerReader(this);
            lock (syncObj)
            {
                for (int j = 0; j <= triggerBlock.Count - 1; j++)
                {
                    var current = triggerBlock[j];

                    if (!handlers.ContainsKey(current))
                    {
                        continue;
                    }

                    reader.Trigger = current;
                    try
                    {
                        BeforeTriggerHandled?.Invoke(current);
                        bool canContinue = handlers[current](reader);
                        AfterTriggerHandled?.Invoke(current);
                        // if canContinue was assigned by a Condition, we care, otherwise continue
                        if (current.Category == TriggerCategory.Condition && !canContinue)
                        {
                            // skip ahead for another condition to meet
                            bool foundCond = false;
                            for (int i = j + 1; i <= triggerBlock.Count - 1; i++)
                            {
                                Trigger possibleCondition = triggerBlock[i];
                                if (possibleCondition.Category == TriggerCategory.Condition)
                                {
                                    j = i; // set the current index of the outer loop
                                    foundCond = true;
                                    break;
                                }
                            }

                            if (!foundCond) break;
                        }
                    }
                    catch (Exception e)
                    {
                        var ex = new MonkeyspeakException(String.Format("{0}",
                            e.Message));
                        if (Error != null)
                            Error(handlers[current], current, ex);
                        else throw ex;

                        break;
                    }
                    //End of main loop
                }
            }
        }

        /// <summary>
        /// Executes a trigger block containing TriggerCategory.Cause with ID equal to <param name="id" />
        ///
        /// </summary>
        /// <param name="id">I dunno</param>
        public void Execute(int id = 0)
        {
            for (int j = 0; j <= triggerBlocks.Count - 1; j++)
            {
                int causeIndex;
                if ((causeIndex = triggerBlocks[j].IndexOfTrigger(TriggerCategory.Cause, id)) != -1)
                {
                    ExecuteBlock(triggerBlocks[j]);
                }
            }
        }

        /// <summary>
        /// Executes a trigger block containing TriggerCategory.Cause with ID equal to <param name="id" />
        ///
        /// </summary>
        /// <param name="ids">I dunno</param>
        public void Execute(params int[] ids)
        {
            for (int i = 0; i <= ids.Length - 1; i++)
            {
                int id = ids[i];
                for (int j = 0; j <= triggerBlocks.Count - 1; j++)
                {
                    int causeIndex;
                    if ((causeIndex = triggerBlocks[j].IndexOfTrigger(TriggerCategory.Cause, id)) != -1)
                    {
                        ExecuteBlock(triggerBlocks[j]);
                    }
                }
            }
        }

        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        public async Task ExecuteAsync(CancellationToken cancellationToken, params int[] ids)
        {
            for (int i = 0; i <= ids.Length - 1; i++)
            {
                int id = ids[i];
                for (int j = 0; j <= triggerBlocks.Count - 1; j++)
                {
                    int causeIndex = triggerBlocks[j].IndexOfTrigger(TriggerCategory.Cause, id);
                    if (causeIndex != -1)
                    {
                        await Task.Run(() => ExecuteBlock(triggerBlocks[j]), cancellationToken);
                    }
                }
            }
        }

        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        public async Task ExecuteAsync(params int[] ids)
        {
            for (int i = 0; i <= ids.Length - 1; i++)
            {
                int id = ids[i];
                for (int j = 0; j <= triggerBlocks.Count - 1; j++)
                {
                    int causeIndex = triggerBlocks[j].IndexOfTrigger(TriggerCategory.Cause, id);
                    if (causeIndex != -1)
                    {
                        await Task.Run(() => ExecuteBlock(triggerBlocks[j]), CancellationToken.None);
                    }
                }
            }
        }

        public void Dispose()
        {
            foreach (var lib in libraries)
            {
                lib.OnPageDisposing(this);
            }
            Clear();
            Reset(true);
        }
    }
}