using System.Collections.Generic;
using FurcLog = Furcadia.Logging;
using MsLog = Monkeyspeak.Logging;

namespace MonkeyCore.Logging
{
    /// <summary>
    /// Multi log output
    /// </summary>
    /// <seealso cref="MsLog.ILogOutput" />
    /// <seealso cref="FurcLog.ILogOutput" />
    public class MultiLogOutput : ILogOutput, MsLog.ILogOutput, FurcLog.ILogOutput
    {
        private List<ILogOutput> outputs;

        public MultiLogOutput(params ILogOutput[] outputs)
        {
            this.outputs = new List<ILogOutput>(outputs);
        }

        public void Log(LogMessage logMsg)
        {
            foreach (var output in outputs) output.Log(logMsg);
        }

        public IEnumerable<ILogOutput> Outputs => outputs;

        public void Add(params ILogOutput[] outputs)
        {
            this.outputs.AddRange(outputs);
        }

        public void Remove(ILogOutput output)
        {
            outputs.Remove(output);
        }

        public void Add(params FurcLog.ILogOutput[] outputs)
        {
            Add((ILogOutput[])outputs);
        }

        public void Remove(FurcLog.ILogOutput output)
        {
            outputs.Remove((ILogOutput)output);
        }

        public void Add(params MsLog.ILogOutput[] outputs)
        {
            this.outputs.AddRange((ILogOutput[])outputs);
        }

        public void Remove(MsLog.ILogOutput output)
        {
            outputs.Remove((ILogOutput)output);
        }

        public void Log(MsLog.LogMessage logMsg)
        {
            Log((LogMessage)logMsg);
        }

        public void Log(FurcLog.LogMessage logMsg)
        {
            Log((LogMessage)logMsg);
        }
    }
}