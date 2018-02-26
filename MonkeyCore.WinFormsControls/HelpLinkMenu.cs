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
    public class HelpLinkMenu
    {
        #region Private Fields

        private List<MenuItem> _MenuItems;
        private IniFile HelpIni;

        private IniFile.IniSection HelpSection;

        #endregion Private Fields

        #region Private Constructors

        /// <summary>
        /// Read the default Section by <see cref="Application.ProductName"/>
        /// </summary>
        public HelpLinkMenu() : this(Application.ProductName)
        {
        }

        /// <summary>
        /// Read section from the ini file and converit it to a list of ToolStripMenuItems
        /// </summary>
        /// <param name="SectionName">
        /// Arbitrary Secion Name
        /// </param>
        public HelpLinkMenu(string SectionName)
        {
            _MenuItems = new List<MenuItem>();
            HelpIni = new IniFile();
            HelpIni.Load(Path.Combine(IO.Paths.ApplicationPath, "HelpLink.ini"));
            HelpSection = HelpIni.GetSection(SectionName);
            if (HelpSection != null)
            {
                foreach (IniFile.IniSection.IniKey KeyVal in HelpSection.Keys)
                {
                    MenuItem HelpItem = new MenuItem(KeyVal.Name, new System.EventHandler(this.RecentFile_click));
                    HelpItem.Name = KeyVal.Name;
                    _MenuItems.Add(HelpItem);
                }
            }
        }

        #endregion Private Constructors

        #region Public Properties

        /// <summary>
        /// Gets the menu items.
        /// </summary>
        /// <value>
        /// The menu items.
        /// </value>
        public List<MenuItem> MenuItems
        {
            get
            {
                return _MenuItems;
            }
        }

        #endregion Public Properties

        #region Public Methods

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
            MenuItem HelpItem = ((MenuItem)(sender));
            IniFile.IniSection.IniKey MyKey = HelpSection.GetKey(HelpItem.Text);
            Process.Start(MyKey.Value.Trim());
        }

        #endregion Public Methods
    }
}