using static Libraries.MsLibHelper;
using Furcadia.Net.Utils.ServerObjects;
using System.Text.RegularExpressions;

namespace Libraries.PhoenixSpeak
{
    // '' <summary>
    // '' Store a Phoenix Speak 'Get' response and send it to the MonkeySpeak
    // '' Library for PhonixSpeak Processing.
    // '' <para>
    // '' MonkeySpeak library grabs the PS Page and transforms it into a
    // '' PsInfo object. A PsInfo object is a list of PhoenixSpeak Variables
    // '' retrieved from the Furcadia Game-Server via the command line
    // '' interface.
    // '' <see href="https://cms.furcadia.com/creations/dreammaking/dragonspeak/psalpha">Phoenix Speak</see>
    // '' </para>
    // '' </summary>
    public class PhoenicSpeakDataObject : DataObject
    {
        private Regex PsPage = new Regex("multi_result?(\\ d +)?/(\\d+)?", SmRegExOptions);
        private int _CurrentPage;

        private int _PageCount;

        private short _PhoenixSpeakId;

        private string _PS_Page;

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

        public short PhoenixSpeakID
        {
            get
            {
                return _PhoenixSpeakId;
            }
            set
            {
                _PhoenixSpeakId = value;
            }
        }

        public string PS_Page
        {
            get
            {
                return _PS_Page;
            }
        }

        public PhoenicSpeakDataObject()
        {
        }

        public PhoenicSpeakDataObject(string data)
        {
            int.TryParse(PsPage.Match(data, 0).Groups[1].Value, out _CurrentPage);
            int.TryParse(PsPage.Match(data, 0).Groups[2].Value, out _PageCount);
            _PS_Page = PsPage.Replace(data, "");
        }

        // '' <summary>
        // '' returns number of Phoenix Speak pages remaining
        // '' </summary>
        public int PagesRemaining
        {
            get
            {
                return (_PageCount - _CurrentPage);
                // Add "," to the end of match #1.
                // Input: "bank=200, clearance=10, member=1, message='test', stafflv=2, sys_lastused_date=1340046340,"
            }
        }
    }
}