using Monkeyspeak;
using Monkeyspeak.Libraries;
using System;
using MonkeyCore.Logging;

namespace Libraries
{
    /// <summary>
    /// Monkey Speak for Dream Info items,
    /// <para/>
    /// things ike Dream Name, Dream-Owner, Dream URL ect
    /// </summary>
    public class MsDreamInfo : MonkeySpeakLibrary
    {
        #region Public Properties

        /// <summary>
        /// Gets the base identifier.
        /// </summary>
        /// <value>
        /// The base identifier.
        /// </value>
        public override int BaseId => 30;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Initializes this instance. Add your trigger handlers here.
        /// </summary>
        /// <param name="args">Parametized argument of vars to use to pass runtime vars to a library at initialization</param>
        public override void Initialize(params object[] args)
        {
            base.Initialize(args);

            Add(TriggerCategory.Cause,
                r =>
                {
                    ReadTriggeringFurreParams(r);
                    return true;
                },
                "When anyone enters the Dream,");

            Add(TriggerCategory.Cause,
                 NameIs,
                "When the furre named {..} enters the Dream,");

            Add(TriggerCategory.Cause,
                r =>
                {
                    ReadTriggeringFurreParams(r);
                    return true;
                },
                "When anyone leaves the Dream,");

            Add(TriggerCategory.Cause,
                r => NameIs(r),
                "When a furre named {..} leaves the Dream,");

            Add(TriggerCategory.Cause,
                  r =>
                  {
                      ReadDreamParams(r);
                      return true;
                  },
                "When the bot enters a Dream,");

            Add(TriggerCategory.Cause,
                EnterOrLeaveTheDreamNamed,
                "When the bot enters the Dream named {..},");

            Add(TriggerCategory.Cause,
               EnterOrLeaveTheDreamNamed,
               "When the bot leaves a Dream,");

            Add(TriggerCategory.Cause,
               r => DreamNameIs(r),
               "When the bot leaves the Dream named {..},");

            Add(TriggerCategory.Condition,
                 r => BotIsDreamOwner(r),
                 "and the bot is the Dream-Owner,");

            Add(TriggerCategory.Condition,
                r => !BotIsDreamOwner(r),
                "and the bot is not the Dream-Owner,");

            Add(TriggerCategory.Condition,
                r => AndFurreNamedIsDreamOwner(r),
                "and the furre named {..} is the Dream-Owner,");

            Add(TriggerCategory.Condition,
                r => !AndFurreNamedIsDreamOwner(r),
                "and the furre named {..} is not the Dream-Owner,");

            Add(TriggerCategory.Condition,
               DreamNameIs,
              "and the Dream Name is {..},");

            Add(TriggerCategory.Condition,
                 DreamNameIsNot,
                "and the Dream Name is not {..},");

            Add(TriggerCategory.Condition,
                r => TriggeringFurreIsDreamOwner(r),
                "and the triggering furre is the Dream-Owner,");

            Add(TriggerCategory.Condition,
                r => !TriggeringFurreIsDreamOwner(r),
                "and the triggering furre is not the Dream-Owner,");

            Add(TriggerCategory.Condition,
                r =>
                (ParentBotSession.HasShare || DreamInfo.DreamOwner.ToFurcadiaShortName() == ParentBotSession.ConnectedFurre.ShortName),
                 "and the bot has share control of the Dream or is the Dream-Owner,");

            Add(TriggerCategory.Condition,
                r => ParentBotSession.HasShare,
                "and the bot has share control of the Dream,");

            Add(TriggerCategory.Condition,
                r => !ParentBotSession.HasShare,
                "and the bot doesn't have share control in the Dream,");

            Add(TriggerCategory.Effect,
                r => ShareTrigFurre(r),
                "give share control to the triggering furre.");

            Add(TriggerCategory.Effect,
               r => UnshareTrigFurre(r),
               "remove share control from the triggering furre.");

            Add(TriggerCategory.Effect,
               r => UnshareFurreNamed(r),
               "remove share from the furre named {..} if they're in the Dream right now.");

            Add(TriggerCategory.Effect,
              r => ShareFurreNamed(r),
              "give share to the furre named {..} if they're in the Dream right now.");
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

        [TriggerStringParameter]
        private static bool AndFurreNamedIsDreamOwner(TriggerReader reader)
        {
            return DreamInfo.DreamOwner.ToFurcadiaShortName()
                != reader.ReadString().ToLower();
        }

        private static bool BotIsDreamOwner(TriggerReader reader)
        {
            return DreamInfo.DreamOwner.ToFurcadiaShortName() == ParentBotSession.ConnectedFurre.ShortName;
        }

        [TriggerStringParameter]
        private static bool DreamNameIs(TriggerReader reader)
        {
            var url = reader.ReadString();

            if (string.IsNullOrWhiteSpace(url)) return false;

            url = url.ToLower().Trim().Replace("furc://", "").TrimEnd('/');

            var urlSegments = url.Split(new char[] { ':' }, 2, StringSplitOptions.None);

            if (urlSegments.Length < 2)
            {
                //One Argument suppplied, Should be Dream Owner Test
                return DreamInfo.Name == urlSegments[0].ToFurcadiaShortName()
                    || DreamInfo.DreamOwner.ToFurcadiaShortName() == urlSegments[0].ToFurcadiaShortName();
            }
            //Full Dream Name Supplied
            return DreamInfo.Name == $"{urlSegments[0].ToFurcadiaShortName()}:{urlSegments[1].ToFurcadiaShortName()}";
        }

        private static bool DreamNameIsNot(TriggerReader reader)
        {
            return !DreamNameIs(reader);
        }

        private static bool TriggeringFurreIsDreamOwner(TriggerReader reader)
        {
            return Player.ShortName == DreamInfo.DreamOwner.ToFurcadiaShortName();
        }

        private bool EnterOrLeaveTheDreamNamed(TriggerReader reader)
        {
            ReadDreamParams(reader);
            return DreamNameIs(reader);
        }

        [TriggerStringParameter]
        private bool ShareFurreNamed(TriggerReader reader)
        {
            var TargetFurre = Furres.GetFurreByName(reader.ReadString());
            if (InDream(TargetFurre))
                return SendServer($"share {TargetFurre.ShortName}");
            Logger.Warn($"Cannot give share to {TargetFurre.Name} because they're not in the dream");
            return false;
        }

        private bool ShareTrigFurre(TriggerReader reader)
        {
            if (InDream(Player))
                return SendServer($"share { Player.ShortName}");
            Logger.Warn($"Cannot give share to {Player.Name} because they're not in the dream");
            return false;
        }

        [TriggerStringParameter]
        private bool UnshareFurreNamed(TriggerReader reader)
        {
            var TargetFurre = Furres.GetFurreByName(reader.ReadString());
            if (InDream(TargetFurre))
                return SendServer($"unshare {TargetFurre.ShortName}");
            Logger.Warn($"cannot thake share from {TargetFurre.Name} they're not in the dream");
            return false;
        }

        private bool UnshareTrigFurre(TriggerReader reader)
        {
            if (InDream(Player))
                return SendServer($"unshare {Player.ShortName}");
            Logger.Warn($"Cannot take share from {Player.Name} because they're not in the dream");
            return false;
        }

        #endregion Private Methods
    }
}