using Furcadia.Net.Pounce;
using Monkeyspeak;
using Monkeyspeak.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using static Libraries.MsLibHelper;

namespace Libraries
{
    /// <summary>
    /// Pounce Server interface with a list of furres contained in a simple text file. This system is styled after <see cref="MsMemberList"/>
    /// </summary>
    /// <remarks>
    /// This classe is the Monkey Speak interface for using the Furcadia Pounce server. It does not read the online.ini located in the Furcadia App-Data Folder.
    /// Instead it uses a text file With a list Of Furcadia Names(Long Or Short formats Don't matter). Furcadia.Net.Pounce is a HTTP(S)  Async Post Method
    /// class that sends the requests to the server once every 30 seconds.
    /// <para/>
    /// Why 30 seconds? Because the Furcadia pounce server runs on a 30 second cron job, Therefore it makes sense to stick with it update time.
    /// </remarks>
    public class MsPounce : MonkeySpeakLibrary
    {
        public MsPounce()
        {
            SmPounce = null;
        }

        #region Private Fields

        /// <summary>
        /// Pounce List File Name
        /// </summary>
        private string _onlineListFile;

        private bool disposedValue;

        /// <summary>
        /// Pounce Furre List
        /// </summary>
        private List<PounceFurre> PounceFurres;

        private PounceClient SmPounce;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets the base identifier.
        /// </summary>
        /// <value>
        /// The base identifier.
        /// </value>
        public override int BaseId => 950;

        /// <summary>
        /// Default File we use
        /// </summary>
        public string ListFile => "onlineList.txt";

        /// <summary>
        /// the File of the Friends List to Check
        /// <para/>
        /// Defaults to 'BotFolder\onlineList.txt"
        /// </summary>
        /// <returns>
        /// </returns>
        public string OnlineListFile => _onlineListFile;

        #endregion Public Properties

        #region Public Methods

        public override void Initialize(params object[] args)
        {
            base.Initialize(args);

            PounceFurres = new List<PounceFurre>();
            _onlineListFile = IO.Paths.CheckBotFolder(ListFile);
            //    OnlineFurres = New IO.NameList(_onlineListFile)
            // SmPounce = new PounceClient(PounceFurres, null);
            Add(TriggerCategory.Cause,
                r => true,
                "When a furre logs on,");

            Add(TriggerCategory.Cause,
                r => true,
                "When a furre logs off,");

            Add(TriggerCategory.Cause,
                NameIs,
                "When the furre named {...} logs on,");

            Add(TriggerCategory.Cause,
                NameIs,
                "When the furre named {...} logs off,");

            Add(TriggerCategory.Condition,
                FurreNamedonline,
                "and the furre named {...} is on-line,");

            Add(TriggerCategory.Condition,
                FurreNamedNotOnline,
                "and the furre named {...} is off-line,");

            Add(TriggerCategory.Condition,
                TrigFurreIsMember,
                "and triggering furre is on the smPounce List,");

            Add(TriggerCategory.Condition,
                TrigFurreIsNotMember,
                "and the triggering furre is not on the smPounce List,");

            Add(TriggerCategory.Condition,
                FurreNamedIsMember,
                "and the furre named {...} is on the smPounce list,");

            Add(TriggerCategory.Condition,
                FurreNamedIsNotMember,
                "and the furre named {...} is not on the smPounce list,");

            Add(TriggerCategory.Effect,
                AddTriggeringFurreToMemberList,
                "add the triggering furre to the smPounce List.");

            Add(TriggerCategory.Effect,
                AddFurreNamed,
                "add the furre named {...} to the smPounce list.");

            Add(TriggerCategory.Effect,
                RemoveTrigFurre,
                "remove the triggering furre from the smPounce list.");

            Add(TriggerCategory.Effect,
                RemoveFurreNamed,
                "remove the furre named {...} from the smPounce list.");

            Add(TriggerCategory.Effect,
                UseMemberFile,
                "use the file named {...} as the smPounce list and start the Pounce Clinet Interface.");
        }

        /// <summary>
        /// Called when page is disposing or resetting.
        /// </summary>
        /// <param name="page">The page.</param>
        public override void Unload(Page page)
        {
            Dispose(true);
        }

        #endregion Public Methods

        #region Protected Methods

        //  To detect redundant calls
        //  IDisposable
        protected void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (SmPounce != null)
                    {
                        SmPounce.Dispose();
                    }
                }

                disposedValue = true;
            }

            // TODO: #End Region ... Warning!!! not translated
            // TODO: #Region ... Warning!!! not translated
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// (5:951) add the furre named {...} to the smPounce list.
        /// they aren't already on it.
        /// </summary>
        /// <param name="reader"> <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// </returns>
        private bool AddFurreNamed(TriggerReader reader)
        {
            // TODO: Fix to Pounce Reader
            if (FurreNamedIsMember(reader) == false)
            {
                using (var sw = new StreamWriter(_onlineListFile, true))
                    sw.WriteLine(reader.ReadString());
            }
            return true;
        }

        private bool AddTriggeringFurreToMemberList(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private void CheckonlineList()
        {
            _onlineListFile = IO.Paths.CheckBotFolder(_onlineListFile);
            if (File.Exists(_onlineListFile) == false)
            {
                Logger.Warn<MsPounce>($"On-line List File: '{_onlineListFile}' Doesn't Exist, Creating new file");
                using (var fStream = new FileStream(_onlineListFile, FileMode.Create))
                using (var sw = new StreamWriter(fStream))
                    sw.Close();
            }
        }

        /// <summary>
        /// (1:954) and the furre named {...} is on the smPounce list,
        /// </summary>
        /// <param name="reader"><see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// </returns>
        private bool FurreNamedIsMember(Monkeyspeak.TriggerReader reader)
        {
            CheckonlineList();
            var Furre = reader.ReadString();
            var f = File.ReadAllLines(_onlineListFile);
            foreach (var l in f)
            {
                if (l.ToFurcadiaShortName() == Furre.ToFurcadiaShortName())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// (1:955) and the furre named {...} is not on the smPounce list,
        /// </summary>
        /// <param name="reader"> <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// </returns>
        private bool FurreNamedIsNotMember(Monkeyspeak.TriggerReader reader)
        {
            return !FurreNamedIsMember(reader);
        }

        /// <summary>
        /// (1:951) and the furre named {...} is off-line,
        /// </summary>
        /// <param name="reader"><see cref="TriggerReader"/></param>
        /// <returns></returns>
        private bool FurreNamedNotOnline(TriggerReader reader)
        {
            var TmpName = reader.ReadString();
            foreach (var Fur in PounceFurres)
            {
                if (Fur.ShortName == TmpName.ToFurcadiaShortName())
                {
                    return !Fur.Online;
                }
            }

            // add Machine Name parser
            return false;
        }

        /// <summary>
        /// (1:950) and the furre named {...} is on-line,
        /// </summary>
        /// <param name="reader"><see cref="TriggerReader"/></param>
        /// <returns></returns>
        private bool FurreNamedonline(TriggerReader reader)
        {
            var TmpName = reader.ReadString();
            foreach (var Fur in PounceFurres)
            {
                if (Fur.ShortName == TmpName.ToFurcadiaShortName())
                {
                    return Fur.Online;
                }
            }

            // add Machine Name parser
            return false;
        }

        /// <summary>
        /// (5:953) remove the furre named {...} from the smPounce list.
        /// </summary>
        /// <param name="reader"><see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// </returns>
        private bool RemoveFurreNamed(TriggerReader reader)
        {
            CheckonlineList();
            var Furre = reader.ReadString();
            string line = null;
            var linesList = new List<string>(File.ReadAllLines(_onlineListFile));
            using (var SR = new StreamReader(_onlineListFile))
            {
                line = SR.ReadLine();
                for (int i = 0; i <= linesList.Count; i++)
                {
                    if (line.ToFurcadiaShortName() == Furre.ToFurcadiaShortName())
                    {
                        linesList.RemoveAt(i);
                        File.WriteAllLines(_onlineListFile, linesList.ToArray());
                        return true;
                    }

                    line = SR.ReadLine();
                }
            }
            return true;
        }

        /// <summary>
        /// (5:952) remove the triggering furre from the smPounce list.
        /// </summary>
        /// <param name="reader"> <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// </returns>
        private bool RemoveTrigFurre(TriggerReader reader)
        {
            CheckonlineList();
            var Furre = reader.Page.GetVariable(TriggeringFurreNameVariable).Value.ToString();
            Furre = Furre.ToFurcadiaShortName();
            string line = null;
            var linesList = new List<string>(File.ReadAllLines(_onlineListFile));
            using (var SR = new StreamReader(_onlineListFile))
            {
                line = SR.ReadLine();
                for (int i = 0; i <= linesList.Count - 1; i++)
                {
                    if (line.ToFurcadiaShortName() == Furre)
                    {
                        linesList.RemoveAt(i);
                        File.WriteAllLines(_onlineListFile, linesList.ToArray());
                        return true;
                    }

                    line = SR.ReadLine();
                }

                return true;
            }
        }

        /// <summary>
        /// (1:952) and triggering furre is on the smPounce List,
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// </returns>
        private bool TrigFurreIsMember(TriggerReader reader)
        {
            CheckonlineList();
            var Furr = reader.Page.GetVariable(TriggeringFurreNameVariable).Value.ToString();
            var f = File.ReadAllLines(_onlineListFile);
            foreach (string Fur in f)
            {
                if (Fur.ToFurcadiaShortName() == Furr.ToFurcadiaShortName())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// (1:953) and the triggering furre is not on the smPounce List,
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// </returns>
        private bool TrigFurreIsNotMember(Monkeyspeak.TriggerReader reader)
        {
            return !TrigFurreIsMember(reader);
        }

        /// <summary>
        /// (5:904) Use file {...} as the dream member list.
        /// <para/>
        /// Defaults to 'BotFolder\on-lineList.txt"
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// True on Success
        /// </returns>
        private bool UseMemberFile(TriggerReader reader)
        {
            throw new NotImplementedException();
            // Dim FileList = reader.ReadString
            // CheckonlineList()
            // If SmPounce IsNot Nothing Then SmPounce.Dispose()
            // SmPounce = New PounceClient(OnlineFurres.ToArray, Nothing)
            // Return True
        }

        #endregion Private Methods
    }
}