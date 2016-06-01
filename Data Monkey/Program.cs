using System.Reflection;
using System;
using System.IO;
using System.Windows.Forms;

namespace SQLiteEditor
{
	public class Program
	{
		/// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        { 
/*        	
        	String appropriateFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath),"DLLx64" + Path.DirectorySeparatorChar + "System.Data.SQLite.dll");
  			if(!Environment.Is64BitOperatingSystem)
  				appropriateFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath),"DLLx86" + Path.DirectorySeparatorChar + "System.Data.SQLite.dll");
        	Assembly.LoadFrom(appropriateFile);
*/
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SQLiteEditor.frmExplorer()) ;
           
        }
	}

}

