using Furcadia.Net;
using Microsoft.VisualBasic.CompilerServices;
using Monkeyspeak;
using SilverMonkeyEngine.Engine;
using SilverMonkeyEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Media;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SilverMonkey
{
    public class SoundModule : ImsPlugin
    {
        #region Private Fields

        private const string REGEX_NameFilter = "[^a-zA-Z0-9\\0x0020_.|]+";
        private ImsHost msHost;

        private MonkeySpeakPage mspage;
        private SoundPlayer simpleSound;

        #endregion Private Fields

        #region Public Properties

        public string Description
        {
            get
            {
                return "Allows Silver Monkey to play sounds";
            }
        }

        public bool enabled
        {
            [DebuggerNonUserCode]
            get;
            [DebuggerNonUserCode]
            set;
        }

        public bool Enabled
        {
            [DebuggerNonUserCode]
            get;
            [DebuggerNonUserCode]
            set;
        }

        public MonkeySpeakPage MsPage
        {
            get
            {
                return mspage;
            }
            set
            {
                mspage = value;
                msHost.MsPage = mspage;
            }
        }

        public DREAM MyDream
        {
            get
            {
                return msHost.Dream;
            }
            set
            {
                msHost.Dream = value;
            }
        }

        public string Name
        {
            get
            {
                return "Sound Module";
            }
        }

        public FURRE Player
        {
            get
            {
                return msHost.Player;
            }
            set
            {
                msHost.Player = value;
            }
        }

        public string Version
        {
            get
            {
                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                string[] str = new string[] { Conversions.ToString(version.Major), ".", Conversions.ToString(version.Minor), ".", Conversions.ToString(version.Build), ".", Conversions.ToString(version.Revision) };
                return string.Concat(str);
            }
        }

        #endregion Public Properties

        #region Public Methods

        public static string ToFurcShortName(string value)
        {
            return Regex.Replace(value, REGEX_NameFilter, "").ToLower();
        }

        public void Initialize(ImsHost Host)
        {
            msHost = Host;
        }

        public bool IsBot(ref FURRE p)
        {
            bool flag = Operators.CompareString(p.ShortName, ToFurcShortName(msHost.BotName), false) == 0;
            return flag;
        }

        public bool MessagePump(ref string ServerInstruction)
        {
            return false;
        }

        public FURRE NametoFurre(ref string sname, ref bool UbdateMSVariableName)
        {
            Dictionary<uint, FURRE>.Enumerator enumerator = new Dictionary<uint, FURRE>.Enumerator();
            FURRE value = new FURRE(sname);
            //value.Name = sname;
            //try
            //{
            //    enumerator = MyDream.FurreList.GetEnumerator();
            //    while (enumerator.MoveNext())
            //    {
            //        KeyValuePair<uint, FURRE> current = enumerator.Current;
            //        if (Operators.CompareString(current.Value.ShortName, ToFurcShortName(sname), false) == 0)
            //        {
            //            value = current.Value;
            //            break;
            //        }
            //    }
            //}
            //finally
            //{
            //    ((IDisposable)enumerator).Dispose();
            //}
            //if (UbdateMSVariableName)
            //{
            //    Page.SetVariable("%NAME", sname, true);
            //}
            return value;
        }

        public double ReadVariableOrNumber(TriggerReader reader, bool addIfNotExist = false)
        {
            double num = 0;
            if (reader.PeekVariable())
            {
                string str = reader.ReadVariable(addIfNotExist).Value.ToString();
                double.TryParse(str, out num);
            }
            else if (reader.PeekNumber())
            {
                num = reader.ReadNumber();
            }
            return num;
        }

        public void Start()
        {
            MsPage.SetTriggerHandler(TriggerCategory.Effect, 2010, delegate (TriggerReader reader)
            {
                simpleSound = new SoundPlayer(reader.ReadString(true));
                simpleSound.Play();
                return true;
            }, "(5:2010) play the wave file {...}.");
            MsPage.SetTriggerHandler(TriggerCategory.Effect, 2011, delegate (TriggerReader reader)
            {
                simpleSound = new SoundPlayer(reader.ReadString(true));
                simpleSound.PlayLooping();
                return true;
            }, "(5:2011) play the wave file {...} in a loop.");
            MsPage.SetTriggerHandler(TriggerCategory.Effect, 2012, delegate (TriggerReader reader)
            {
                //simpleSound = new SoundPlayer(reader.ReadString(true));
                simpleSound.Stop();
                return true;
            }, "(5:2012) stop playing the sound file.");
        }

        #endregion Public Methods

        #region Private Methods

        private FURRE fIDtoFurre(ref string ID)
        {
            FURRE value = new FURRE();
            Dictionary<uint, FURRE>.Enumerator enumerator = new Dictionary<uint, FURRE>.Enumerator();
            //try
            //{
            //    enumerator = MyDream.FurreList.GetEnumerator();
            //    while (enumerator.MoveNext())
            //    {
            //        KeyValuePair<uint, FURRE> current = enumerator.Current;
            //        if (current.Value.ID == Conversions.ToDouble(ID))
            //        {
            //            value = current.Value;
            //            return value;
            //        }
            //    }
            //}
            //finally
            //{
            //    ((IDisposable)enumerator).Dispose();
            //}
            return value;
        }

        #endregion Private Methods
    }
}