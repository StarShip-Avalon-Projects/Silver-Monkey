using Caliburn.Micro;
using Engine.BotSession;
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
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Caliburn.Micro.Screen" />
    public class ShellViewModel : Screen
    {
        #region Private Fields

        private int dreamCount;
        private string dreamOwner = "Dream Owner";
        private string dreamTitle = "title";
        private int furreCount;
        private FurreList furreList = new FurreList(100);
        private string rating;

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
            get { return dreamOwner; }
            set
            {
                dreamOwner = value;
                NotifyOfPropertyChange(() => DreamOwner);
            }
        }

        /// <summary>
        /// Gets or sets the dream title.
        /// </summary>
        /// <value>
        /// The dream title.
        /// </value>
        public string DreamTitle
        {
            get { return dreamTitle; }
            set
            {
                dreamTitle = value;
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
            get { return rating; }
            set
            {
                rating = value;
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
            get { return $"furc://{dreamOwner.ToFurcadiaShortName()}:{dreamTitle.ToFurcadiaShortName()}/"; }
            set
            {
                dreamTitle = value;
                NotifyOfPropertyChange(() => DreamURL);
            }
        }

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
                return $"Total Furres: {dreamCount}";
            }

            set
            {
                dreamCount = 0;
                int.TryParse(value, out dreamCount);
                NotifyOfPropertyChange(() => DreamCount);
            }
        }

        /// <summary>
        /// Gets or sets the furre count.
        /// </summary>
        /// <value>
        /// The furre count.
        /// </value>
        public string FurreCount
        {
            get
            {
                return $"Total Furres: {furreCount}";
            }

            set
            {
                furreCount = 0;
                int.TryParse(value, out furreCount);
                NotifyOfPropertyChange(() => FurreCount);
            }
        }

        /// <summary>
        /// Gets or sets the furres.
        /// </summary>
        /// <value>
        /// The furres.
        /// </value>
        public FurreList Furres
        {
            get
            {
                return furreList;
            }

            set
            {
                furreList = value;
                NotifyOfPropertyChange(() => Furres);
            }
        }

        #endregion Public Properties
    }
}