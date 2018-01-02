using Monkeyspeak;
using Monkeyspeak.Logging;

namespace Engine.Libraries
{
    public class MsDreamInfo : MonkeySpeakLibrary
    {
        private bool BotIsDreamOwner(TriggerReader reader)
        {
            return DreamInfo.ShortName == ParentBotSession.ConnectedFurre.ShortName;
        }

        private bool DreamNameIs(TriggerReader reader)
        {
            var ParamsSet = ReadDreamParams(reader);
            return DreamInfo.ShortName == reader.ReadString().ToFurcadiaShortName();
        }

        private bool DreamNameIsNot(TriggerReader reader)
        {
            return !DreamNameIs(reader);
        }

        public override void Initialize(params object[] args)
        {
            base.Initialize(args);
            Add(TriggerCategory.Cause, 21, reader =>
            {
                ReadTriggeringFurreParams(reader);
                return !ParentBotSession.IsConnectedCharacter(Player);
            }

            , " When someone emits something,");
            Add(TriggerCategory.Cause, 22, MsgIs, " When someone emits {..},");
            Add(TriggerCategory.Cause, 23, MsgContains, " When someone emits something with {..} in it,");
            Add(TriggerCategory.Cause, 90, ReadDreamParams, " When the bot enters a Dream,");
            Add(TriggerCategory.Cause, 91, DreamNameIs, " When the bot enters the Dream named {..},");
            Add(TriggerCategory.Cause, 97, ReadDreamParams, " When the bot leaves a Dream,");
            Add(TriggerCategory.Cause, 98, DreamNameIs, " When the bot leaves the Dream named {..},");
            Add(TriggerCategory.Condition, 19, BotIsDreamOwner, " and the bot is the Dream owner,");
            Add(TriggerCategory.Condition, 20, reader => !BotIsDreamOwner(reader), " and the bot is not the Dream-Owner,");
            Add(TriggerCategory.Condition, 21, reader => DreamInfo.OwnerShortName == reader.ReadString().ToFurcadiaShortName(), " and the furre named {..} is the Dream owner,");
            Add(TriggerCategory.Condition, 22, reader => DreamInfo.OwnerShortName != reader.ReadString().ToFurcadiaShortName(), " and the furre named {..} is not the Dream owner,");
            Add(TriggerCategory.Condition, 23, DreamNameIs, " and the Dream Name is {..},");
            Add(TriggerCategory.Condition, 24, DreamNameIsNot, " and the Dream Name is not {..},");
            Add(TriggerCategory.Condition, 25, TriggeringFurreIsDreamOwner, " and the triggering furre is the Dream owner,");
            Add(TriggerCategory.Condition, 26, TriggeringFurreIsNotDreamOwner, " and the triggering furre is not the Dream owner,");
            Add(TriggerCategory.Condition, 27, reader =>
            {
                if (ParentBotSession.HasShare || DreamInfo.OwnerShortName == ParentBotSession.ConnectedFurre.ShortName)
                {
                    return true;
                }

                return false;
            }

            , " and the bot has share control of the Dream or is the Dream owner,");
            Add(TriggerCategory.Condition, 28, (v) =>
            {
                return ParentBotSession.HasShare;
            }

            , " and the bot has share control of the Dream,");
            Add(TriggerCategory.Condition, 29, (v) =>
            {
                return !ParentBotSession.HasShare;
            }

            , " and the bot doesn't have share control in the Dream,");
            Add(TriggerCategory.Effect, 20, ShareTrigFurre, " give share control to the triggering furre.");
            Add(TriggerCategory.Effect, 21, UnshareTrigFurre, " remove share control from the triggering furre.");
            Add(TriggerCategory.Effect, 22, UnshareFurreNamed, " remove share from the furre named {..} if they're in the Dream right now.");
            Add(TriggerCategory.Effect, 23, ShareFurreNamed, " give share to the furre named {..} if they're in the Dream right now.");
        }

        public bool ShareFurreNamed(TriggerReader reader)
        {
            var Target = DreamInfo.Furres.GerFurreByName(reader.ReadString());
            if (InDream(Target))
            {
                if (InDream(Target))
                    SendServer("share " + Target.ShortName);
            }
            else
            {
                Logger.Info<MsDreamInfo>($"{Target.Name} Is Not in the dream");
            }

            return true;
        }

        public bool ShareTrigFurre(TriggerReader reader)
        {
            return SendServer("share " + Player.ShortName);
        }

        private bool TriggeringFurreIsDreamOwner(TriggerReader reader)
        {
            return Player.ShortName == DreamInfo.OwnerShortName;
        }

        private bool TriggeringFurreIsNotDreamOwner(TriggerReader reader)
        {
            return !TriggeringFurreIsDreamOwner(reader);
        }

        public override void Unload(Page page)
        {
        }

        public bool UnshareFurreNamed(TriggerReader reader)
        {
            var Target = DreamInfo.Furres.GerFurreByName(reader.ReadString());
            if (InDream(Target))
            {
                return SendServer("unshare " + Target.ShortName);
            }
            else
            {
                Logger.Info<MsDreamInfo>($"{Target.Name} Is Not in the dream");
            }

            return false;
        }

        public bool UnshareTrigFurre(TriggerReader reader)
        {
            SendServer("unshare " + Player.ShortName);
            return true;
        }

        protected override bool NameIs(TriggerReader reader)
        {
            return base.NameIs(reader);
        }
    }
}