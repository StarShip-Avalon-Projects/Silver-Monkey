using Furcadia.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Controls
{
    /// <summary>
    /// Standard menu class for HelpLink Menu Items
    /// </summary>
    public class HelpLinkToolStripMenu
    {
        private List<ToolStripMenuItem> _MenuItems;
        private IniFile HelpIni;

        private IniFile.IniSection HelpSection;

        /// <summary>
        /// Read the default Section by <see cref="Application.ProductName"/>
        /// </summary>
        public HelpLinkToolStripMenu() : this(Application.ProductName)
        {
        }

        /// <summary>
        /// Read section from the ini file and converit it to a list of ToolStripMenuItems
        /// </summary>
        /// <param name="SectionName">
        /// Arbitrary Secion Name
        /// </param>
        public HelpLinkToolStripMenu(string SectionName)
        {
            _MenuItems = new List<ToolStripMenuItem>();
            HelpIni = new IniFile();
            HelpIni.Load(Path.Combine(IO.Paths.ApplicationPath, "HelpLink.ini"));
            HelpSection = HelpIni.GetSection(SectionName);
            if (HelpSection != null)
            {
                foreach (IniFile.IniSection.IniKey KeyVal in HelpSection.Keys)
                {
                    ToolStripMenuItem HelpItem = new ToolStripMenuItem(KeyVal.Name, null, new System.EventHandler(this.RecentFile_click));
                    _MenuItems.Add(HelpItem);
                }
            }
        }

        /// <summary>
        /// Gets the menu items.
        /// </summary>
        /// <value>
        /// The menu items.
        /// </value>
        public List<ToolStripMenuItem> MenuItems
        {
            get
            {
                return _MenuItems;
            }
        }

        /// <summary>
        /// click menu handler
        /// </summary>
        /// <param name="sender">
        /// the triggering ToolstripMenu object
        /// </param>
        /// <param name="e">
        /// event arguments (not needed)
        /// </param>
        public virtual void RecentFile_click(object sender, EventArgs e)
        {
            ToolStripMenuItem HelpItem = (ToolStripMenuItem)sender;
            IniFile.IniSection.IniKey MyKey = HelpSection.GetKey(HelpItem.Text);
            Process.Start(MyKey.Value.Trim());
        }
    }
}