using System.Text;
using System.IO;
using System;
using Engine.BotSession;
using IO;
using System.Text.RegularExpressions;

namespace Engine.BotSession
{
    /// <summary>
    /// Silver Monkey's MonkeySpeak Engine with our Customizations
    /// </summary>
    public sealed class MsEngineExtentionFunctions
    {
        #region Private Fields

        private static string RES_MS_begin => "*MSPK V";

        private static string RES_MS_end => "*Endtriggers* 8888 *Endtriggers*";

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Loads from script file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="ver">Specified Version</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Filepath npt given</exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="Exception"></exception>
        public static string LoadFromScriptFile(string file, Version ver)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentException("Filepath npt given", file);
            }

            StringBuilder ScriptContents = new StringBuilder();
            if (!File.Exists(Paths.CheckBotFolder(file)))
            {
                throw new FileNotFoundException($"MonkeySpeak file ({file}) not found. Did you forget to define on or check the file path?");
            }

            using (var MonkeySpeakScriptReader = new StreamReader(file))
            {
                string line = "";
                while (!MonkeySpeakScriptReader.EndOfStream)
                {
                    line = MonkeySpeakScriptReader.ReadLine();
                    if (!line.StartsWith(RES_MS_begin))
                    {
                        ScriptContents.AppendLine(line);
                    }
                    else if (line.StartsWith(RES_MS_begin))
                    {
                        var vers = line;
                        var versionregex = new Regex("(\\d{1,3}\\.\\d{1,3})", RegexOptions.Compiled);
                        vers = versionregex.Match(vers).Groups[1].Value;
                        if (Version.Parse($"{vers}.0.0").MajorRevision < ver.MajorRevision || Version.Parse(vers).Major < ver.Major)
                            throw new Exception($"Version Mismatch error, Please upgrade the script to MSPKV{ver.Major},{ver.MajorRevision}");

                        ScriptContents.AppendLine(line);
                        // MonkeySpeak Script Version Check
                    }

                    if (line == RES_MS_end)
                    {
                        break;
                    }
                }
                return ScriptContents.ToString();
            }
        }

        #endregion Public Methods
    }
}