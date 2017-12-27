using Caliburn.Micro;
using Furcadia.Net.DreamInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace SilverMonkey.ViewModels
{
    public class ShellViewModel : Screen
    {
        #region Private Fields

        private string _DreamURL;
        private string _DreamName;
        private string _rating;

        private BindableCollection<Furre> _FurreList = new BindableCollection<Furre>();
        public Furre SelectedFurre { get; set; }

        #endregion Private Fields

        #region Public Properties

        public string DreamName
        {
            get { return _DreamName; }
            set
            {
                _DreamName = value;
                NotifyOfPropertyChange(() => DreamName);
            }
        }

        public string DreamRating
        {
            get { return _rating; }
            set
            {
                _rating = value;
                NotifyOfPropertyChange(() => DreamRating);
            }
        }

        public string DreamURL
        {
            get { return _DreamURL; }
            set
            {
                _DreamURL = value;
                NotifyOfPropertyChange(() => DreamURL);
            }
        }

        #endregion Public Properties
    }
}