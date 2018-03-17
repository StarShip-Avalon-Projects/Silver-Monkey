using Monkeyspeak.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using furcLog = Furcadia.Logging;

using MsLog = Monkeyspeak.Logging;

namespace MonkeyCore.Logging
{
    /// <summary>
    /// Multi log output
    /// </summary>
    /// <seealso cref="Monkeyspeak.Logging.ILogOutput" />
    public class MultiLogOutput : ILogOutput, MsLog.ILogOutput, furcLog.ILogOutput
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

        public void Add(params furcLog.ILogOutput[] outputs)
        {
            Add((ILogOutput[])outputs);
        }

        public void Remove(furcLog.ILogOutput output)
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

        public void Log(furcLog.LogMessage logMsg)
        {
            Log((LogMessage)logMsg);
        }
    }
}