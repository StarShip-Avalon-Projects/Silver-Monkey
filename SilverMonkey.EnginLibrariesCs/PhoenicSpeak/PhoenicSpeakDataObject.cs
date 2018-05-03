using static Libraries.MsLibHelper;
using Furcadia.Net.Utils.ServerObjects;
using System.Text.RegularExpressions;
using Furcadia.Extensions;
using Monkeyspeak;

namespace Libraries.PhoenixSpeak
{
    /// <summary>
    /// Store a Phoenix Speak 'Get' response and send it to the MonkeySpeak
    /// Library for PhonixSpeak Processing.
    /// <para>
    /// MonkeySpeak library grabs the PS Page and transforms it into a
    /// PsInfo var. A PsInfo var is a list of PhoenixSpeak Variables
    /// retrieved from the Furcadia Game-Server via the command line
    /// interface.
    /// <see href="https://cms.furcadia.com/creations/dreammaking/dragonspeak/psalpha">Phoenix Speak</see>
    /// </para>
    /// </summary>
    public class PhoenixSpeakDataObject : DataObject
    {
        #region Private Fields

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; private set; }

        public bool IsError { get; private set; }

        /// <summary>
        /// Gets the type of the phoenix speak.
        /// </summary>
        /// <value>
        /// The type of the phoenix speak.
        /// </value>
        public PhoenixSpeakTypes phoenixSpeakType { get; private set; } = PhoenixSpeakTypes.Unknown;

        private int _CurrentPage;
        private int _PageCount;
        private Regex PsPage = new Regex("multi_result?([0-9]+)?/([0-9]+)?", SmRegExOptions);
        private Regex PsData = new Regex(@"\s(.*?)=('(.*?)'|[0-9]+)");

        /// <summary>
        /// Phoenix Speak Response Types
        /// </summary>
        public enum PhoenixSpeakTypes : byte
        {
            Unknown,
            Get,
            Set,
            Clear,
            Error
        }

        #endregion Private Fields

        #region Public Constructors

        public PhoenixSpeakDataObject()
        {
        }

        public PhoenixSpeakDataObject(string data)
        {
            int.TryParse(PsPage.Match(data, 0).Groups[1].Value, out _CurrentPage);
            int.TryParse(PsPage.Match(data, 0).Groups[2].Value, out _PageCount);

            PS_Page = PsPage.Replace(data, "");

            var PsQuery = data.Split(new char[] { ' ' }, 5);
            PsTable = new VariableTable(PsQuery[1]);

            //Query isn't something we sent
            if (PsQuery[1].AsInt16(0) <= 0)
                return;

            PhoenixSpeakID = PsQuery[1].AsInt16(0);

            IsError = PsQuery[2] != "Ok:";

            switch (PsQuery[3])
            {
                case "get:":
                    phoenixSpeakType = PhoenixSpeakTypes.Get;
                    // Data available

                    // PS ### Ok: get: result: money='500', partysize='1', playerexp='0', playerlevel='1', pokeballs='15', pokemon1='7 1 n Squirtle 1 0 1 tackle', pokemon2='0', pokemon3='0', pokemon4='0', pokemon5='0', pokemon6='0', sys_lastused_date=1523076301, totalpokemon='1'

                    var PsMatches = PsData.Matches(PsQuery[4]);
                    foreach (Match psMatch in PsMatches)
                        if (short.TryParse(psMatch.Groups[2].Value, out short num))
                            PsTable.Add(psMatch.Groups[1].Value, psMatch.Groups[2].Value);
                        else
                            PsTable.Add(psMatch.Groups[1].Value, psMatch.Groups[3].Value);

                    break;

                //PS Error: clear: Query error: Field 'characer' does not exist

                case "Error:":

                    phoenixSpeakType = PhoenixSpeakTypes.Error;
                    ErrorMessage = $"{PsQuery[3]} {PsQuery[4]}";
                    // No data available
                    // PS ### Error: get: Query error: Field 'bulldog' does not exist
                    break;
                //PS ### Ok: clear: Ok
                case "clear:":
                    phoenixSpeakType = PhoenixSpeakTypes.Clear;
                    break;

                //PS ### Ok: set: Ok
                case "set:":
                    phoenixSpeakType = PhoenixSpeakTypes.Set;
                    break;
            }
        }

        #endregion Public Constructors

        #region Public Properties

        public int CurrentPage
        {
            get
            {
                return _CurrentPage;
            }
            set
            {
                _CurrentPage = value;
            }
        }

        public bool IsMultiPage
        {
            get
            {
                return _PageCount > 0;
            }
        }

        /// <summary>
        /// returns number of Phoenix Speak pages remaining
        /// </summary>
        public int PagesRemaining => _PageCount - _CurrentPage;

        /// <summary>
        /// 16 bit integer for Phoenix Speak command tracking.
        /// </summary>
        public short PhoenixSpeakID { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public string PS_Page { get; }

        /// <summary>
        /// Monkeyspeak Variable table Key = PS Field, Value = PS data
        /// </summary>
        public VariableTable PsTable { get; private set; }

        #endregion Public Properties
    }
}