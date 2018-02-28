using System;
using System.IO;
using System.Text;
using Furcadia.IO;

namespace IO
{
    /// <summary>
    /// Use an Ini file to store predefined Monkeyspeak scripts
    /// </summary>
    public class EditorScriptBuilder
    {
        #region Private Fields

        private IniFile ScritpIniFile;

        private StringBuilder stringBuilder;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorScriptBuilder"/> class.
        /// </summary>
        public EditorScriptBuilder()
        {
            ScritpIniFile = new IniFile();
            stringBuilder = new StringBuilder();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorScriptBuilder"/> class.
        /// </summary>
        /// <param name="IniFile">The ini file.</param>
        public EditorScriptBuilder(string IniFile) : this()
        {
            ScritpIniFile.Load(IniFile);
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Reads the script template.
        /// </summary>
        /// <param name="ScriptName">Name of the script.</param>
        /// <param name="ver">The ver.</param>
        public void ReadScriptTemplate(string ScriptName, string ver)
        {
            string MonkeySpeakLine = " ";
            int index = 0;

            stringBuilder.AppendLine(ScritpIniFile.GetKeyValue(ScriptName, "Header").Replace("[VERSION]", ver).TrimEnd('"'));
            int.TryParse(ScritpIniFile.GetKeyValue(ScriptName, "HMax"), out int LimeMax);
            while (index <= LimeMax)
            {
                MonkeySpeakLine = ScritpIniFile.GetKeyValue(ScriptName, "H" + index.ToString()).TrimEnd('"');
                stringBuilder.AppendLine(MonkeySpeakLine);
                index++;
            }

            int.TryParse(ScritpIniFile.GetKeyValue(ScriptName, "InitLineSpaces").TrimEnd('"'), out int iMax);
            for (int i = 0; i <= iMax; i++)
            {
                stringBuilder.AppendLine(string.Empty);
            }

            stringBuilder.Append(ScritpIniFile.GetKeyValue(ScriptName, "Footer").TrimEnd('"'));
        }

        /// <summary>
        /// Writes the Monkeyspeak script to the specified filename
        /// </summary>
        /// <param name="FileName">Name of the file.</param>
        public void WriteScriptToFile(string FileName)
        {
            if (string.IsNullOrWhiteSpace(FileName))
                throw new ArgumentException(FileName);

            if (!File.Exists(FileName))
                using (var script = new FileStream(FileName, FileMode.Create))
                using (var fWriter = new StreamWriter(script))
                {
                    fWriter.Write(stringBuilder.ToString());
                }
        }

        #endregion Public Methods
    }
}