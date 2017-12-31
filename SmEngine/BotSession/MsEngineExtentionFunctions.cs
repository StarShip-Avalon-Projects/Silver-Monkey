using Monkeyspeak;
using MonkeyCore;
using System.Text;
using System.IO;
using System;
using BotSession;

namespace Engine
{
    // '' <summary>
    // '' Silver Monkey's MonkeySpeak Engine with our Customizations
    // '' </summary>
    public sealed class MsEngineExtentionFunctions
    {
        private const string MS_Footer = "*Endtriggers* 8888 *Endtriggers*";

        private const string MS_Header = "*MSPK V04.00 Silver Monkey";

        public Bot FurcadiaSession;

        private const string RES_MS_begin = "*MSPK V";

        private const string RES_MS_end = "*Endtriggers* 8888 *Endtriggers*";

        public static string LoadFromScriptFile(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentException("Filepath npt given", file);
            }

            StringBuilder ScriptContents = new StringBuilder();
            if (!System.IO.File.Exists(Paths.CheckBotFolder(ref file)))
            {
                throw new FileNotFoundException("MonkeySpeak file ({file}) not found. Did you forget to define on or check the file path?");
            }

            using (var MonkeySpeakScriptReader = new StreamReader(file))
            {
                string line = "";
                while (MonkeySpeakScriptReader.Peek() != -1)
                {
                    line = MonkeySpeakScriptReader.ReadLine();
                    if (!line.StartsWith(RES_MS_begin))
                    {
                        ScriptContents.AppendLine(line);
                    }
                    else if (line.StartsWith(RES_MS_begin))
                    {
                        // MonkeySpeak Script Version Check
                    }

                    if ((line == RES_MS_end))
                    {
                        break;
                    }
                }
                return ScriptContents.ToString();
            }
        }
    }
}