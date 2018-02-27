using IO;
using Monkeyspeak;
using Monkeyspeak.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using static Libraries.MsLibHelper;

namespace Libraries
{
    /// <summary>
    /// Dream Member List management
    /// <para>
    /// a Simple dream administration system using a text file to contain a
    /// list of Furre as staff
    /// </para>
    /// <para>
    /// NOTE: The BotController is considered to be on the list even if the
    ///       furres name is not in the text file
    /// </para>
    /// <para>
    /// Default Member-List file: <see cref="Paths.SilverMonkeyBotPath"/>\MemberList.txt
    /// </para>
    /// <conceptualLink target="d1358c3d-d6d3-4063-a0ef-259e13752a0f"/>
    /// <para/>
    /// Credits: Drake for assistance with designing this system
    /// </summary>
    public class MsMemberList : MonkeySpeakLibrary
    {
        #region Private Fields

        private List<string> Members = new List<string>();

        #endregion Private Fields

        #region Public Properties

        public override int BaseId
        {
            get
            {
                return 900;
            }
        }

        /// <summary>
        /// Member List file path
        /// <para/>
        /// Defaults to <see cref="IO.Paths.SilverMonkeyBotPath"/>\MemberList.txt
        /// </summary>
        /// <returns></returns>
        public string MemberList
        {
            get;
            private set;
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Initializes this instance. Add your trigger handlers here.
        /// </summary>
        /// <param name="args">Parametized argument of vars to use to pass runtime vars to a library at initialization</param>
        public override void Initialize(params object[] args)
        {
            base.Initialize(args);
            MemberList = Paths.CheckBotFolder("MemberList.txt");

            Add(TriggerCategory.Condition,
                TrigFurreIsMember,
                "and the triggering furre is on my dream Member List,");

            Add(TriggerCategory.Condition,
                FurreNamedIsMember,
                "and the furre named {...} is on my Dream Member list,");

            Add(TriggerCategory.Condition,
                TrigFurreIsNotMember,
                "and the triggering furre is not on my Dream Member list,");

            Add(TriggerCategory.Condition,
                FurreNamedIsNotMember,
                "and the furre named {...} is not on my Dream Member list,");

            Add(TriggerCategory.Effect,
                AddTrigFurre,
                "add the triggering furre to my Dream Member list if they aren\'t already on it.");

            Add(TriggerCategory.Effect,
                AddFurreNamed,
                "add the furre named {...} to my Dream Member list if they aren\'t already on it.");

            Add(TriggerCategory.Effect,
                RemoveTrigFurre,
                "remove the triggering furre to my Dream Member list if they are on it.");

            Add(TriggerCategory.Effect,
                RemoveFurreNamed,
                "remove the furre named {...} from my Dream Member list if they are on it.");

            Add(TriggerCategory.Effect,
                UseMemberFile,
                "Use file {...} as the dream member list.");
            // (5:905) store member list to variable %Variable.
            Add(TriggerCategory.Effect,
                ListToVariable,
                "store member list to variable %Variable.");
        }

        /// <summary>
        /// Called when page is disposing or resetting.
        /// </summary>
        /// <param name="page">The page.</param>
        public override void Unload(Page page)
        {
        }

        #endregion Public Methods

        #region Private Methods

        private bool AddFurreNamed(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool AddTrigFurre(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private void CheckMemberList()
        {
            MemberList = Paths.CheckBotFolder(MemberList);
            if (!File.Exists(MemberList))
            {
                using (FileStream fStream = new FileStream(MemberList, FileMode.OpenOrCreate))
                using (StreamWriter writer = new StreamWriter(fStream))
                {
                    writer.Close();
                }
            }
        }

        private bool FurreNamedIsMember(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool FurreNamedIsNotMember(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool ListToVariable(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool RemoveFurreNamed(TriggerReader reader)
        {
            try
            {
                this.CheckMemberList();
                var Furre = reader.ReadString();
                string line;
                using (var fStream = new FileStream(MemberList, FileMode.OpenOrCreate))
                using (var SR = new StreamReader(fStream))
                {
                    while (SR.Peek() != -1)
                    {
                        line = SR.ReadLine();
                        for (int i = 0; i <= Members.Count - 1; i++)
                        {
                            if (line.ToFurcadiaShortName() == Furre.ToFurcadiaShortName())
                            {
                                Members.RemoveAt(i);
                                File.WriteAllLines(MemberList, Members.ToArray());
                                break;
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Info<MsMemberList>("A problem occurred checking the member-list");
                Logger.Error<MsMemberList>($"{ex.Message}");
            }
            return false;
        }

        private bool RemoveTrigFurre(TriggerReader reader)
        {
            try
            {
                CheckMemberList();
                var Furre = reader.Page.GetVariable(TriggeringFurreNameVariable).Value.ToString();
                List<string> linesList = new List<string>(File.ReadAllLines(MemberList));
                using (var fStream = new FileStream(MemberList, FileMode.OpenOrCreate))
                using (var SR = new StreamReader(fStream))
                {
                    while (SR.Peek() != -1)
                    {
                        var line = SR.ReadLine();
                        for (int i = 0; i <= linesList.Count - 1; i++)
                        {
                            if (line.ToFurcadiaShortName() == Furre.ToFurcadiaShortName())
                            {
                                linesList.RemoveAt(i);
                                File.WriteAllLines(MemberList, linesList.ToArray());
                                break;
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Info<MsMemberList>("A problem occurred checking the member-list");
                Logger.Error<MsMemberList>($"{ex.Message}");
            }
            return false;
        }

        private bool TrigFurreIsMember(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool TrigFurreIsNotMember(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool UseMemberFile(TriggerReader reader)
        {
            try
            {
                MemberList = reader.ReadString();
                this.CheckMemberList();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Info<MsMemberList>("A problem occurred checking the member-list");
                Logger.Error<MsMemberList>($"{ex.Message}");
            }

            return false;
        }

        #endregion Private Methods
    }
}