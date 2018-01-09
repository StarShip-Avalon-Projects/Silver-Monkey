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

        private int _DreamCount;
        private string _DreamOwner = "Dream Owner";
        private string _DreamTitle = "title";
        private int _FurreCount;
        private List<Furre> _FurreList = new List<Furre>();
        private string _rating;

        /// <summary>
        /// Gets or sets the selected furre.
        /// </summary>
        /// <value>
        /// The selected furre.
        /// </value>
        public Furre SelectedFurre { get; set; } = new Furre();

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets or sets the dream owner.
        /// </summary>
        /// <value>
        /// The dream owner.
        /// </value>
        public string DreamOwner
        {
            get { return _DreamOwner; }
            set
            {
                _DreamOwner = value;
                NotifyOfPropertyChange(() => DreamOwner);
            }
        }

        public string DreamTitle
        {
            get { return _DreamTitle; }
            set
            {
                _DreamTitle = value;
                NotifyOfPropertyChange(() => DreamTitle);
            }
        }

        /// <summary>
        /// Gets or sets the dream rating.
        /// </summary>
        /// <value>
        /// The dream rating.
        /// </value>
        public string DreamRating
        {
            get { return _rating; }
            set
            {
                _rating = value;
                NotifyOfPropertyChange(() => DreamRating);
            }
        }

        /// <summary>
        /// Gets or sets the dream URL.
        /// </summary>
        /// <value>
        /// The dream URL.
        /// </value>
        public string DreamURL
        {
            get { return $"furc://{_DreamOwner.ToFurcadiaShortName()}:{_DreamTitle.ToFurcadiaShortName()}/"; }
            set
            {
                _DreamTitle = value;
                NotifyOfPropertyChange(() => DreamURL);
            }
        }

        /// <summary>

        /// <summary>
        /// Gets or sets the dream count.
        /// </summary>
        /// <value>
        /// The dream count.
        /// </value>
        public string DreamCount
        {
            get
            {
                return $"Total Furres: {_DreamCount}";
            }

            set
            {
                _DreamCount = 0;
                int.TryParse(value, out _DreamCount);
                NotifyOfPropertyChange(() => DreamCount);
            }
        }

        /// Gets or sets the furre count.
        /// </summary>
        /// <value>
        /// The furre count.
        /// </value>
        public string FurreCount
        {
            get
            {
                return $"Total Furres: {_FurreCount}";
            }

            set
            {
                _FurreCount = 0;
                int.TryParse(value, out _FurreCount);
                NotifyOfPropertyChange(() => FurreCount);
            }
        }

        public List<Furre> Furres
        {
            get
            {
                return _FurreList;
            }

            set
            {
                _FurreList = value;
                NotifyOfPropertyChange(() => Furres);
            }
        }

        #endregion Public Properties
    }
}