using System.Text;
using Furcadia.IO;

public class NewEditorScript
{
    #region Private Fields

    private IniFile MsIniFile;

    private StringBuilder str = new StringBuilder();

    #endregion Private Fields

    #region Public Constructors

    public NewEditorScript()
    {
        MsIniFile = new IniFile();
    }

    public NewEditorScript(string IniFile)
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
        while (t != "")
        {
            t = MsIniFile.GetKeyValue(ScriptName, "H" + n.ToString()).TrimEnd('"');
            if (t != "")
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
    }

    #endregion Public Methods
}