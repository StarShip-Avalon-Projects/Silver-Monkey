using System.IO;
using System.Text;
using Furcadia.IO;

namespace MonkeyCore2.IO
{
    public class EditorScript
    {
        #region Private Fields

        private IniFile MsIniFile;

        private StringBuilder str = new StringBuilder();

        #endregion Private Fields

        #region Public Constructors

        public EditorScript()
        {
            MsIniFile = new IniFile();
        }

        public EditorScript(string IniFile)
        {
            MsIniFile = new IniFile();
            MsIniFile.Load(IniFile);
        }

        #endregion Public Constructors

        #region Public Methods

        public void ReadScriptTemplate(string ScriptName, string ver)
        {
            str.AppendLine(MsIniFile.GetKeyValue(ScriptName, "Header").Replace("[VERSION]", ver).TrimEnd('"'));
            string t = " ";
            int n = 0;
            int.TryParse(MsIniFile.GetKeyValue(ScriptName, "HMax"), out int HMax);
            while (n != HMax)
            {
                t = MsIniFile.GetKeyValue(ScriptName, "H" + n.ToString()).TrimEnd('"');
                str.AppendLine(t);
                n += 1;
            }

            int.TryParse(MsIniFile.GetKeyValue(ScriptName, "InitLineSpaces").TrimEnd('"'), out int iMax);
            for (int i = 0; i <= iMax; i++)
            {
                str.AppendLine("");
            }

            str.Append(MsIniFile.GetKeyValue(ScriptName, "Footer").TrimEnd('"'));
        }

        public void WriteScriptToFile(string FileName)
        {
            using (var script = new FileStream(FileName, FileMode.Create))
            using (var fWriter = new StreamWriter(script))
            {
                fWriter.Write(str.ToString());
            }
        }

        #endregion Public Methods
    }
}