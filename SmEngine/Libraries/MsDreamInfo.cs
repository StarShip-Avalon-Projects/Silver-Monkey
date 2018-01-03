using Monkeyspeak;
using Monkeyspeak.Logging;

namespace Libraries
{
    /// <summary>
    /// Monkey Speak for Dream Info items,
    /// <para/>
    /// things ike Dream Name, Dream Owner, Dream URL ect
    /// </summary>
    public class MsDreamInfo : MonkeySpeakLibrary
    {
        #region Public Methods

        /// <summary>
        /// Initializes this instance. Add your trigger handlers here.
        /// </summary>
        /// <param name="args">Parametized argument of objects to use to pass runtime objects to a library at initialization</param>
        public override void Initialize(params object[] args)
        {
            base.Initialize(args);
            Add(TriggerCategory.Cause, 21, reader =>
            {
                ReadTriggeringFurreParams(reader);
                return !ParentBotSession.IsConnectedCharacter(Player);
            }

            , "When someone emits something,");
            Add(TriggerCategory.Cause, 22,
                MsgIs, "When someone emits {..},");

            Add(TriggerCategory.Cause, 23,
                MsgContains, "When someone emits something with {..} in it,");

            Add(TriggerCategory.Cause, 90,
                ReadDreamParams, "When the bot enters a Dream,");

            Add(TriggerCategory.Cause, 91,
                DreamNameIs, "When the bot enters the Dream named {..},");

            Add(TriggerCategory.Cause, 97,
                ReadDreamParams, "When the bot leaves a Dream,");

            Add(TriggerCategory.Cause, 98,
                DreamNameIs, "When the bot leaves the Dream named {..},");

            Add(TriggerCategory.Condition, 19,
                BotIsDreamOwner, "and the bot is the Dream owner,");
            Add(TriggerCategory.Condition, 20,
                r => !BotIsDreamOwner(r), " and the bot is not the Dream-Owner,");

            Add(TriggerCategory.Condition, 21,
                r => DreamInfo.OwnerShortName == r.ReadString().ToFurcadiaShortName(),
                " and the furre named {..} is the Dream owner,");

            Add(TriggerCategory.Condition, 22,
                r => DreamInfo.OwnerShortName != r.ReadString().ToFurcadiaShortName(),
                " and the furre named {..} is not the Dream owner,");

            Add(TriggerCategory.Condition, 23,
                DreamNameIs, "and the Dream Name is {..},");

            Add(TriggerCategory.Condition, 24,
                DreamNameIsNot, "and the Dream Name is not {..},");

            Add(TriggerCategory.Condition, 25,
                TriggeringFurreIsDreamOwner, "and the triggering furre is the Dream owner,");

            Add(TriggerCategory.Condition, 26,
                TriggeringFurreIsNotDreamOwner, "and the triggering furre is not the Dream owner,");

            Add(TriggerCategory.Condition, 27, r =>
                (ParentBotSession.HasShare || DreamInfo.OwnerShortName == ParentBotSession.ConnectedFurre.ShortName),
                 "and the bot has share control of the Dream or is the Dream owner,");

            Add(TriggerCategory.Condition, 28, (r) =>
                 ParentBotSession.HasShare
            , "and the bot has share control of the Dream,");

            Add(TriggerCategory.Condition, 29, (r) => !ParentBotSession.HasShare
            , "and the bot doesn't have share control in the Dream,");

            Add(TriggerCategory.Effect, 20,
                ShareTrigFurre, "give share control to the triggering furre.");

            Add(TriggerCategory.Effect, 21,
                UnshareTrigFurre, "remove share control from the triggering furre.");

            Add(TriggerCategory.Effect, 22,
                UnshareFurreNamed, "remove share from the furre named {..} if they're in the Dream right now.");

            Add(TriggerCategory.Effect, 23,
                ShareFurreNamed, "give share to the furre named {..} if they're in the Dream right now.");
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

        private bool BotIsDreamOwner(TriggerReader reader)
        {
            return DreamInfo.DreamOwner.ToFurcadiaShortName() == ParentBotSession.ConnectedFurre.ShortName;
        }

        private bool DreamNameIs(TriggerReader reader)
        {
            var ParamsSet = ReadDreamParams(reader);
            return DreamInfo.Name.ToLower() == reader.ReadString().ToLower();
        }

        private bool DreamNameIsNot(TriggerReader reader)
        {
            return !DreamNameIs(reader);
        }

        private bool ShareFurreNamed(TriggerReader reader)
        {
            var Target = DreamInfo.Furres.GerFurreByName(reader.ReadString());
            if (InDream(Target))
            {
                SendServer($"share {Target.ShortName}");
            }
            else
            {
                Logger.Info($"{Target.Name} Is Not in the dream");
            }

            return true;
        }

        private bool ShareTrigFurre(TriggerReader reader)
        {
            return SendServer($"share { Player.ShortName}");
        }

        private bool TriggeringFurreIsDreamOwner(TriggerReader reader)
        {
            return Player.ShortName == DreamInfo.OwnerShortName;
        }

        private bool TriggeringFurreIsNotDreamOwner(TriggerReader reader)
        {
            return !TriggeringFurreIsDreamOwner(reader);
        }

        private bool UnshareFurreNamed(TriggerReader reader)
        {
            var Target = DreamInfo.Furres.GerFurreByName(reader.ReadString());
            if (InDream(Target))
            {
                return SendServer($"unshare {Target.ShortName}");
            }
            else
            {
                Logger.Info($"{Target.Name} Is Not in the dream");
            }

            return false;
        }

        private bool UnshareTrigFurre(TriggerReader reader)
        {
            SendServer($"unshare {Player.ShortName}");
            return true;
        }

        #endregion Private Methods
    }
}