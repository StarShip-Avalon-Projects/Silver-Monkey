using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System;
using IO;

namespace MonkeyCore.WinForms.Controls
{
    public class RecentFileMenu : ToolStripMenuItem
    {
        public const string RecentFile = "Recent.txt";
        public List<string> MRUlist = new List<string>(MRUnumber);

        /// <summary>
        /// how many list will save
        /// </summary>
        private const int MRUnumber = 15;

        private static ToolStripMenuItem menu;

        public RecentFileMenu()
        {
            menu = null;
        }

        private void ReadRecentMenutList()
        {
            using (StreamReader listToRead = new StreamReader(System.IO.Path.Combine(Paths.ApplicationSettingsPath, RecentFile), true))
            {
                while (!listToRead.EndOfStream)
                {
                    MRUlist.Add(listToRead.ReadLine());
                }
            }
        }

        public void RemoveItemFromList(string item)
        {
        }

        /// <summary>
        /// click menu handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void RecentFile_click(object sender, EventArgs e)
        { }

        /// <summary>
        /// store a list to file and refresh list
        /// </summary>
        /// <param name="path"></param>
        public void SaveRecentFile(string path)
        {
            menu.DropDownItems.Clear();
            // clear all recent list from menu
            ReadRecentMenutList();
            // load list from file
            if (!MRUlist.Contains(path))
            {
                // prevent duplication on recent list
                MRUlist.Add(path);
            }

            for (int i = MRUnumber; i < MRUlist.Count; i++)
            {
                MRUlist.RemoveAt(i);
            }

            foreach (string item in MRUlist)
            {
                ToolStripMenuItem fileRecent = new ToolStripMenuItem(item, null, new System.EventHandler(this.RecentFile_click));
                // create new menu for each item in list
                // add the menu to "recent" menu
                menu.DropDownItems.Add(fileRecent);
            }

            // writing menu list to file
            using (StreamWriter stringToWrite = new StreamWriter(System.IO.Path.Combine(Paths.ApplicationSettingsPath, RecentFile)))
            {
                // create file called "Recent.txt" located on app folder
                foreach (string item in MRUlist)
                {
                    // write list to stream
                    stringToWrite.WriteLine(item);
                }
            }
        }
    }
}