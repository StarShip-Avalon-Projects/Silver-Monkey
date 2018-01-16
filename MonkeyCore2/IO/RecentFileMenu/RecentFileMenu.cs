using Furcadia.IO;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonkeyCore2.IO
{
    public class RecentFileMenu
    {
        public Queue<string> MRUlist = new Queue<string>(MRUnumber);

        /// <summary>
        /// how many list will save
        /// </summary>
        private const int MRUnumber = 15;

        public RecentFileMenu()
        {
        }

        private StreamReader listToRead;//= //new StreamReader(System.IO.Path.Combine(MonkeyCore2.IO.Paths.ApplicationSettingsPath, RecentFile), true);

        // read file stream
        private string line = "";

        /// <summary>
        /// store a list to file and refresh list
        /// </summary>
        /// <param name="path"></param>
        public void SaveRecentFile(string path)
        {
            // _Menu.DropDownItems.Clear();
            // clear all recent list from menu
            //  LoadRecentList();
            // load list from file
            if (!MRUlist.Contains(path))
            {
                // prevent duplication on recent list
                MRUlist.Enqueue(path);
            }

            // insert given path into list
            while ((MRUlist.Count > MRUnumber))
            {
                // keep list number not exceeded given value
                MRUlist.Dequeue();
            }

            foreach (string item in MRUlist)
            {
                //   ToolStripMenuItem fileRecent = new ToolStripMenuItem(item, null, new System.EventHandler(this.RecentFile_click));
                // create new menu for each item in list
                // add the menu to "recent" menu
                //   _Menu.DropDownItems.Add(fileRecent);
            }

            // writing menu list to file
            // StreamWriter stringToWrite = new StreamWriter(System.IO.Path.Combine(Paths.ApplicationSettingsPath, RecentFile));
            // create file called "Recent.txt" located on app folder
            foreach (string item in MRUlist)
            {
                // write list to stream
                //stringToWrite.WriteLine(item);
            }

            // stringToWrite.Flush();
            // write stream to file
            //stringToWrite.Close();
            // close the stream and reclaim memory
        }

        private static T InlineAssignHelper<T>(ref T target, T value)
        {
            target = value;
            return value;
        }
    }
}