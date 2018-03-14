using System.Collections.Generic;
using MsLog = Monkeyspeak.Logging;
using FurcLog = Furcadia.Logging;

namespace MonkeyCore.Logging
{
    /// <summary>
    /// Multi log output
    /// </summary>
    /// <seealso cref="Furcadia.Logging.ILogOutput" />
    public class MultiLogOutput : ILogOutput, MsLog.ILogOutput, FurcLog.ILogOutput
    {
        private List<ILogOutput> outputs;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLogOutput"/> class.
        /// </summary>
        /// <param name="outputs">The outputs.</param>
        public MultiLogOutput(params ILogOutput[] outputs)
        {
            this.outputs = new List<ILogOutput>(outputs);
        }

        /// <summary>
        /// Logs the specified log MSG.
        /// </summary>
        /// <param name="logMsg">The log MSG.</param>
        public void Log(LogMessage logMsg)
        {
            foreach (var output in outputs) output.Log(logMsg);
        }

        /// <summary>
        /// Gets the outputs.
        /// </summary>
        /// <value>
        /// The outputs.
        /// </value>
        public IEnumerable<ILogOutput> Outputs => outputs;

        /// <summary>
        /// Adds the specified outputs.
        /// </summary>
        /// <param name="outputs">The outputs.</param>
        public void Add(params ILogOutput[] outputs)
        {
            this.outputs.AddRange(outputs);
        }

        /// <summary>
        /// Removes the specified output.
        /// </summary>
        /// <param name="output">The output.</param>
        public void Remove(ILogOutput output)
        {
            outputs.Remove(output);
        }

        void MsLog.ILogOutput.Log(MsLog.LogMessage logMsg)
        {
            Log(logMsg);
        }

        void FurcLog.ILogOutput.Log(FurcLog.LogMessage logMsg)
        {
            Log(logMsg);
        }
    }
}