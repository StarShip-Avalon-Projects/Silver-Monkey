using Furcadia.Net.DreamInfo;
using Furcadia.Text;
using IO;
using Monkeyspeak;
using Monkeyspeak.Libraries;
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
        #region Private Classes

        /// <summary>
        /// Small furre object for Memberlist comparason
        /// </summary>
        /// <seealso cref="Furcadia.Net.DreamInfo.IFurre" />
        private class Furr : IFurre
        {
            // Can use this as prototype for new PounceFurre class?

            #region Private Fields

            private Base220 id;

            private string message;

            private string name;

            #endregion Private Fields

            #region Public Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="Furr"/> class.
            /// </summary>
            public Furr()
            {
                name = "unknown";
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Furr"/> class.
            /// </summary>
            /// <param name="Name">The Furre's name.</param>
            public Furr(string Name)
            {
                name = Name;
            }

            #endregion Public Constructors

            #region Public Properties

            /// <summary>
            /// Implements the FurreID or unique furre identifyer
            /// </summary>
            public Base220 FurreID { get => id; set => id = value; }

            /// <summary>
            /// Gets or sets the message.
            /// </summary>
            /// <value>
            /// The message.
            /// </value>
            public string Message { get => message; set => message = value; }

            /// <summary>
            /// implements the Furre;s Name Property
            /// </summary>
            public string Name { get => name; set => name = value; }

            /// <summary>
            /// implements the Furre;s Name Property
            /// </summary>
            public string ShortName => name.ToFurcadiaShortName();

            #endregion Public Properties

            #region Public Methods

            /// <summary>
            /// Implements the operator !=.
            /// </summary>
            /// <param name="Furre1">a.</param>
            /// <param name="b">The b.</param>
            /// <returns>
            /// The result of the operator.
            /// </returns>
            public static bool operator !=(Furr Furre1, IFurre Furre2)
            {
                // If left hand side is null...
                if (Furre1 is null)
                {
                    return Furre2 is null;
                }

                // Return true if the fields match:
                return !Furre1.Equals(Furre2);
            }

            /// <summary>
            /// Implements the operator ==.
            /// </summary>
            /// <param name="Furre1">a.</param>
            /// <param name="Furre2">The b.</param>
            /// <returns>
            /// The result of the operator.
            /// </returns>
            public static bool operator ==(Furr Furre1, IFurre Furre2)
            {
                // If left hand side is null...
                if (Furre1 is null)
                {
                    return Furre2 is null;
                }

                // Return true if the fields match:
                return Furre1.Equals(Furre2);
            }

            /// <summary>
            /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
            /// </summary>
            /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
            /// <returns>
            ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
            /// </returns>
            public override bool Equals(object obj)
            {
                if (obj is null)
                    return false;
                if (obj is IFurre ob)
                    return ShortName == ob.ShortName;
                return base.Equals(obj);
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <returns>
            /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
            /// </returns>
            public override int GetHashCode()
            {
                return ShortName.GetHashCode() ^ id;
            }

            /// <summary>
            /// Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>
            /// A <see cref="System.String" /> that represents this instance.
            /// </returns>
            public override string ToString()
            {
                return name;
            }

            #endregion Public Methods
        }

        #endregion Private Classes

        #region Private Fields

        private IList<IFurre> MembersList;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets the base identifier.
        /// </summary>
        /// <value>
        /// The base identifier.
        /// </value>
        public override int BaseId => 900;

        /// <summary>
        /// Member List file path
        /// <para/>
        /// Defaults to <see cref="IO.Paths.SilverMonkeyBotPath"/>\MemberList.txt
        /// </summary>
        /// <returns></returns>
        public string MemberListFile
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
            MembersList = new List<IFurre>();
            MemberListFile = Paths.CheckBotFolder("MemberList.txt");

            Add(TriggerCategory.Condition,
                TrigFurreIsMember,
                "and the triggering furre is on the Dream-Member List,");

            Add(TriggerCategory.Condition,
                FurreNamedIsMember,
                "and the furre named {...} is on the Dream-Member list,");

            Add(TriggerCategory.Condition,
                TrigFurreIsNotMember,
                "and the triggering furre is not on the Dream-Member list,");

            Add(TriggerCategory.Condition,
                FurreNamedIsNotMember,
                "and the furre named {...} is not on the Dream-Member list,");

            Add(TriggerCategory.Effect,
                AddTrigFurre,
                "add the triggering furre to the Dream-Member list if they aren't already on it.");

            Add(TriggerCategory.Effect,
                AddFurreNamed,
                "add the furre named {...} to the Dream-Member list if they aren't already on it.");

            Add(TriggerCategory.Effect,
                RemoveTrigFurre,
                "remove the triggering furre to the Dream-Member list if they are on it.");

            Add(TriggerCategory.Effect,
                RemoveFurreNamed,
                "remove the furre named {...} from the Dream-Member list if they are on it.");

            Add(TriggerCategory.Effect,
                UseMemberFile,
                "Use file {...} as the dream member list.");

            Add(TriggerCategory.Effect,
                ListToVariableTable,
                "store member list to variable table %VariableTable.");
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
            CheckMemberList();
            Furr furr = new Furr(reader.ReadString());
            MembersList.Add(furr);

            if (!MembersList.Contains(furr))
                using (FileStream fileStream = new FileStream(MemberListFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    MembersList.Add(furr);
                    foreach (var fur in MembersList)
                        streamWriter.Write($"{fur}");
                }

            return true;
        }

        private bool AddTrigFurre(TriggerReader reader)
        {
            CheckMemberList();

            MembersList.Add(Player);

            if (!MembersList.Contains(Player))
                using (FileStream fileStream = new FileStream(MemberListFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    MembersList.Add(Player);
                    foreach (var fur in MembersList)
                        streamWriter.Write($"{fur}");
                }

            return true;
        }

        private void CheckMemberList()
        {
            MemberListFile = Paths.CheckBotFolder(MemberListFile);
            if (!File.Exists(MemberListFile))
            {
                using (FileStream fileStream = new FileStream(MemberListFile, FileMode.OpenOrCreate, FileAccess.Read))
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    MembersList.Clear();
                    while (streamReader.Peek() != -1)
                    {
                        MembersList.Add(new Furr(streamReader.ReadLine()));
                    }
                }
            }
        }

        [TriggerDescription("Checks to see it the active player or triggering furre is on the member list")]
        [TriggerStringParameter]
        private bool FurreNamedIsMember(TriggerReader reader)
        {
            CheckMemberList();
            var fur = new Furr(reader.ReadString());
            if (MembersList.Contains(fur))
                return true;
            return FurreNamedIsBotController(reader);
        }

        [TriggerDescription("Checks to see it the active player or triggering furre isnot on the member list")]
        [TriggerStringParameter]
        private bool FurreNamedIsNotMember(TriggerReader reader)
        {
            return !FurreNamedIsMember(reader);
        }

        [TriggerDescription("reads the member list and puts each furre in the variable table as %shortname = \"Name\"")]
        [TriggerVariableParameter]
        private bool ListToVariableTable(TriggerReader reader)
        {
            CheckMemberList();
            VariableTable table = reader.ReadVariableTable(true);
            foreach (var fur in MembersList)
            {
                table.Add($"%{fur.ShortName}", fur.Name);
            }
            return true;
        }

        [TriggerStringParameter]
        private bool RemoveFurreNamed(TriggerReader reader)
        {
            CheckMemberList();
            string furre = reader.ReadString();
            string line;
            using (FileStream fileStream = new FileStream(MemberListFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    while (streamReader.Peek() != -1)
                    {
                        line = streamReader.ReadLine();
                        for (int i = 0; i < MembersList.Count; i++)
                        {
                            if (line.ToFurcadiaShortName() == furre.ToFurcadiaShortName())
                            {
                                MembersList.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                    foreach (var fur in MembersList)
                        streamWriter.Write($"{fur}");
            }
            return true;
        }

        private bool RemoveTrigFurre(TriggerReader reader)
        {
            CheckMemberList();
            string line;

            using (FileStream fileStream = new FileStream(MemberListFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    while (streamReader.Peek() != -1)
                    {
                        line = streamReader.ReadLine();
                        for (int i = 0; i < MembersList.Count; i++)
                        {
                            if (line.ToFurcadiaShortName() == Player.ShortName)
                            {
                                MembersList.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                    foreach (Furr fur in MembersList)
                        streamWriter.Write($"{fur}");
            }
            return true;
        }

        private bool TrigFurreIsMember(TriggerReader reader)
        {
            CheckMemberList();
            if (MembersList.Contains(Player))
                return true;
            return TriggeringFurreIsBotController(reader);
        }

        private bool TrigFurreIsNotMember(TriggerReader reader)
        {
            return !TrigFurreIsMember(reader);
        }

        [TriggerDescription("sets the file name of the memberlist file, This defaults to \"Documents\\Silver Monkey\\MembersList.txt\"")]
        [TriggerStringParameter]
        private bool UseMemberFile(TriggerReader reader)
        {
            try
            {
                MemberListFile = reader.ReadString();
                CheckMemberList();
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