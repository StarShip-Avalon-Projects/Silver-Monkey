using Libraries.Variables;
using Monkeyspeak;
using Monkeyspeak.Libraries;
using MonkeyCore.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using static Libraries.MsLibHelper;

namespace Libraries
{
    /// <summary>
    /// Cause:s (0:50) - (0:62
    /// <para>
    /// Conditions: (1:50) - (1:53)
    /// </para>
    /// Effects: (5:49) - (5:56)
    /// <para>
    /// Banish Monkey Speak
    /// </para>
    /// This system mirrors Furcadia's banish system by tracking the banish
    /// commands sent to the game server and keep a list of banished furres
    /// locally. To help keep the list accurate, It is recommended to ask
    /// the server for a banish-list upon joining the dream. It maybe a good
    /// idea to run a daily schedule to refresh the list for temp banishes
    /// to drop off.
    /// </summary>
    public class MsBanish : MonkeySpeakLibrary
    {
        #region Public Properties

        private List<string> BanishedFurreList = new List<string>();
        public override int BaseId => 51;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Initializes this instance. Add your trigger handlers here.
        /// </summary>
        /// <param name="args">Parametrized argument of vars to use to pass runtime vars to a library at initialization</param>
        public override void Initialize(params object[] args)
        {
            base.Initialize(args);

            Add(TriggerCategory.Cause,
                r => WhenAnyFurreBanished(r),
                "When the bot fails to banish a furre,");

            Add(TriggerCategory.Cause,
                r => WhenFurreNamedBanished(r),
                "When the bot fails to banish the furre named {...},");

            Add(TriggerCategory.Cause,
                r => WhenAnyFurreBanished(r),
                "When the bot successfully banishes a furre,");

            Add(TriggerCategory.Cause,
                r => WhenFurreNamedBanished(r),
                "When the bot successfully banishes the furre named {...},");

            Add(TriggerCategory.Cause,
                r => WhenAnyFurreBanished(r),
                "When the bot sees the banish-list,");

            Add(TriggerCategory.Cause,
                r => WhenAnyFurreBanished(r),
                "When the bot fails to remove a furre from the banish-list,");

            Add(TriggerCategory.Cause,
                  r => WhenFurreNamedBanished(r),
                  "When the bot fails to remove the furre named {...} from the banish-list,");

            Add(TriggerCategory.Cause,
                r => WhenAnyFurreBanished(r),
                "When the bot successfully removes a furre from the banish-list,");

            Add(TriggerCategory.Cause,
                 r => WhenFurreNamedBanished(r),
                 "When the bot successfully removes the furre named {...} from the banish-list,");

            Add(TriggerCategory.Cause,
                 r => throw new NotImplementedException(),
                "When the bot fails to empty the banish-list,");

            Add(TriggerCategory.Cause,
                r =>
                {
                    var banishList = r.GetParametersOfType<List<string>>().FirstOrDefault();
                    if (banishList != null)
                    {
                        BanishedFurreList = banishList;
                        ((ConstantVariable)r.Page.GetVariable(BanishListVariable)).SetValue(null);
                        ((ConstantVariable)r.Page.GetVariable(BanishNameVariable)).SetValue(null);
                        return true;
                    }
                    Logger.Error($"Null parameter banishList = '{banishList}'");
                    return false;
                },
                "When the bot successfully clears the banish-list,");

            Add(TriggerCategory.Cause,
                r => WhenAnyFurreBanished(r),
                "When the bot successfully temp banishes a furre,");

            Add(TriggerCategory.Cause,
                 r => WhenFurreNamedBanished(r),
                 "When the bot successfully temp banishes the furre named {...},");

            Add(TriggerCategory.Condition,
                r => !TriggeringFurreIsBanished(r),
                "and the triggering furre is not on the banish-list,");

            Add(TriggerCategory.Condition,
                r => TriggeringFurreIsBanished(r),
                "and the triggering furre is on the banish-list,");

            Add(TriggerCategory.Condition,
                r => FurreNamedIsBanished(r),
                "and the furre named {...} is not on the banish-list,");

            Add(TriggerCategory.Condition,
                r => !FurreNamedIsBanished(r),
                "and the furre named {...} is on the banish-list,");

            Add(TriggerCategory.Effect,
                 r => SaveBanishListToVariableTable(r),
                 "save the banish-list to table % .");

            Add(TriggerCategory.Effect,
                r => SendServer("banish-list"),
                "ask the server for the banish-list.");

            Add(TriggerCategory.Effect,
                 r => SendServer($"banish { Player.ShortName}"),
                 "banish the triggering furre.");

            Add(TriggerCategory.Effect,
                r => BanishFurreNamed(r),
                "banish the furre named {...}.");

            Add(TriggerCategory.Effect,
                r => TempBanishTrigFurre(r),
                "temporarily  banish the triggering furre for three days.");

            Add(TriggerCategory.Effect,
                 r => SendServer($"tempbanish { Player.ShortName}"),
                 "temporarily banish the furre named {...} for three days.");

            Add(TriggerCategory.Effect,
                 r => SendServer($"banish-off { Player.ShortName}"),
                 "unbanish the triggering furre.");

            Add(TriggerCategory.Effect,
                 r => UnBanishFurreNamed(r),
                 "unbanish the furre named {...}.");

            Add(TriggerCategory.Effect,
                r => CreateBanisListTable(r),
                "store the banish-list to table %variable.");
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

        [TriggerDescription("Tell the game-server to add the specified furre to the Dreams Banish-list. This can error when the specified furre is off line.")]
        [TriggerStringParameter]
        private bool BanishFurreNamed(TriggerReader reader)
        {
            string Furre = reader.ReadString();
            return SendServer($"banish { Furre.ToFurcadiaShortName()}");
        }

        [TriggerDescription("Stores every furre on the banish-list to the specified Table")]
        [TriggerVariableParameter]
        private bool SaveBanishListToVariableTable(TriggerReader reader)
        {
            var table = reader.ReadVariableTable(true);
            var index = 0;
            foreach (string Furre in BanishedFurreList)
            {
                index++;
                table.Add($"{index}", Furre);
            }
            return true;
        }

        [TriggerDescription("Checks Banish-List for the Specified furre")]
        [TriggerStringParameter]
        private bool FurreNamedIsBanished(TriggerReader reader)
        {
            var result = false;
            string Furre = reader.ReadString();

            foreach (var furre in BanishedFurreList)
            {
                if (furre.ToFurcadiaShortName() == Furre.ToFurcadiaShortName())
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        [TriggerDescription("Temporarily Banish a Furre from the dream for 72 hours.")]
        [TriggerStringParameter]
        private bool TempBanishTrigFurre(TriggerReader reader)
        {
            string Furre = reader.ReadString();
            return SendServer($"tempbanish { Furre.ToFurcadiaShortName()}");
        }

        [TriggerDescription("triggered when the triggering furre is added to the banished list")]
        private bool TriggeringFurreIsBanished(TriggerReader reader)
        {
            var result = false;
            foreach (var fur in BanishedFurreList)
            {
                if (fur.ToFurcadiaShortName() == Player.ShortName)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        [TriggerDescription("Ask the server to remove a furre from the banish list.")]
        [TriggerStringParameter]
        private bool UnBanishFurreNamed(TriggerReader reader)
        {
            string Furre = reader.ReadString();
            return SendServer($"banish-off { Furre.ToFurcadiaShortName()}");
        }

        [TriggerDescription("Triggered when the specified furre is added to the banish list.")]
        [TriggerStringParameter]
        private bool WhenFurreNamedBanished(TriggerReader reader)
        {
            var name = reader.GetParametersOfType<string>().FirstOrDefault();
            string Furre = reader.ReadString();
            if (name != null)
            {
                ((ConstantVariable)reader.Page.GetVariable(BanishNameVariable)).SetValue(name);

                return Furre.ToFurcadiaShortName() == name.ToFurcadiaShortName();
            }
            throw new ArgumentNullException("Name", $"Null parameter = {name} ");
        }

        [TriggerDescription("Triggered when any furre is added to the banish list")]
        private bool WhenAnyFurreBanished(TriggerReader reader)
        {
            var name = reader.GetParametersOfType<string>().FirstOrDefault();
            ((ConstantVariable)reader.Page.GetVariable(BanishNameVariable)).SetValue(name);
            if (name != null)
            {
                return true;
            }
            throw new ArgumentNullException("Name", $"Null parameter = {name} ");
        }

        [TriggerDescription("Creates a table or clears a table if the specified table already exists with the list of names in the banish-list")]
        [TriggerVariableParameter]
        private bool CreateBanisListTable(TriggerReader reader)
        {
            var var = reader.ReadVariableTable(true);
            int i = 0;
            foreach (var furre in BanishedFurreList)
            {
                var.Add(new BanishVariable($"%Name{i}", furre));
                i++;
            }
            return true;
        }

        #endregion Private Methods
    }
}