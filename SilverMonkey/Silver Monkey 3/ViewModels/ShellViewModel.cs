using Caliburn.Micro;
using Furcadia.Net.DreamInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilverMonkey.ViewModels
{
    public class ShellViewModel : Screen
    {
        #region Private Fields

        private string _LogOutputBox;
        private string _InputTextBox;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets or sets the log output box.
        /// </summary>
        /// <value>
        /// The log output box.
        /// </value>
        public string LogOutputBox
        {
            get { return _LogOutputBox; }
            set
            {
                _LogOutputBox = value;
                NotifyOfPropertyChange(() => LogOutputBox);
            }
        }

        /// <summary>
        /// Gets or sets the input text box.
        /// </summary>
        /// <value>
        /// The input text box.
        /// </value>
        public string InputTextBox
        {
            get { return _InputTextBox; }
            set
            {
                _InputTextBox = value;
                NotifyOfPropertyChange(() => InputTextBox);
            }
        }

        private BindableCollection<Furre> _FurreList = new BindableCollection<Furre>();
        public Furre SelectedFurre { get; set; }

        #endregion Public Properties
    }
}