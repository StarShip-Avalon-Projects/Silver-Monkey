using Monkeyspeak;
using Monkeyspeak.Logging;

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

        // Furre Enters
        // (0:24) When anyone enters the Dream,
        Add(TriggerCategory.Cause, 24,
               r => ReadTriggeringFurreParams(r) && !IsConnectedCharacter(Player),
                 "When anyone enters the Dream,");

        // (0:25) When the furre named {..} enters the Dream,
        Add(TriggerCategory.Cause, 25,
            r => NameIs(r),
            "When the furre named {..} enters the Dream,");

        // Furre Leaves
        // (0:26) When anyone leaves the Dream,
        Add(TriggerCategory.Cause, 26,
            r => ReadTriggeringFurreParams(r),
            "When anyone leaves the Dream,");

        // (0:27) When a furre named {..} leaves the Dream,
        Add(TriggerCategory.Cause, 27,
            r => NameIs(r),
            "When a furre named {..} leaves the Dream,");

        Add(TriggerCategory.Cause, 90,
         r => ReadDreamParams(r),
            "When the bot enters a Dream,");

        Add(TriggerCategory.Cause, 91,
            r => DreamNameIs(r),
            "When the bot enters the Dream named {..},");

        Add(TriggerCategory.Cause, 97,
           r => ReadDreamParams(r),
           "When the bot leaves a Dream,");

        Add(TriggerCategory.Cause, 98,
           r => DreamNameIs(r),
           "When the bot leaves the Dream named {..},");

        Add(TriggerCategory.Condition, 19,
             r => BotIsDreamOwner(r),
             "and the bot is the Dream owner,");

        Add(TriggerCategory.Condition, 20,
            r => !BotIsDreamOwner(r),
            " and the bot is not the Dream-Owner,");

        Add(TriggerCategory.Condition, 21,
            r => DreamInfo.DreamOwner.ToFurcadiaShortName() == r.ReadString().ToLower(),
            " and the furre named {..} is the Dream owner,");

        Add(TriggerCategory.Condition, 22,
            r => DreamInfo.DreamOwner.ToFurcadiaShortName() != r.ReadString().ToLower(),
            " and the furre named {..} is not the Dream owner,");

        Add(TriggerCategory.Condition, 23,
          r => DreamNameIs(r),
          "and the Dream Name is {..},");

        Add(TriggerCategory.Condition, 24,
            r => DreamNameIsNot(r),
            "and the Dream Name is not {..},");

        Add(TriggerCategory.Condition, 25,
            r => TriggeringFurreIsDreamOwner(r),
            "and the triggering furre is the Dream owner,");

        Add(TriggerCategory.Condition, 26,
            r => TriggeringFurreIsNotDreamOwner(r),
            "and the triggering furre is not the Dream owner,");

        Add(TriggerCategory.Condition, 27, r =>
            (ParentBotSession.HasShare || DreamInfo.DreamOwner.ToFurcadiaShortName() == ParentBotSession.ConnectedFurre.ShortName),
             "and the bot has share control of the Dream or is the Dream owner,");

        Add(TriggerCategory.Condition, 28,
            (r) => ParentBotSession.HasShare,
         "and the bot has share control of the Dream,");

        Add(TriggerCategory.Condition, 29,
            (r) => !ParentBotSession.HasShare,
         "and the bot doesn't have share control in the Dream,");

        Add(TriggerCategory.Effect, 20,
            r => ShareTrigFurre(r),
            "give share control to the triggering furre.");

        Add(TriggerCategory.Effect, 21,
           r => UnshareTrigFurre(r),
           "remove share control from the triggering furre.");

        Add(TriggerCategory.Effect, 22,
           r => UnshareFurreNamed(r),
           "remove share from the furre named {..} if they're in the Dream right now.");

        Add(TriggerCategory.Effect, 23,
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

    private bool BotIsDreamOwner(TriggerReader reader)
    {
        return DreamInfo.DreamOwner.ToFurcadiaShortName() == ParentBotSession.ConnectedFurre.ShortName;
    }

    private bool DreamNameIs(TriggerReader reader)
    {
        ReadDreamParams(reader);
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
            return SendServer($"share {Target.ShortName}");
        }

        Logger.Info($"{Target.Name} Is Not in the dream");
        return false;
    }

    private bool ShareTrigFurre(TriggerReader reader)
    {
        return SendServer($"share { Player.ShortName}");
    }

    private bool TriggeringFurreIsDreamOwner(TriggerReader reader)
    {
        return Player.ShortName == DreamInfo.DreamOwner.ToFurcadiaShortName();
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

        Logger.Info($"{Target.Name} Is Not in the dream");
        return false;
    }

    private bool UnshareTrigFurre(TriggerReader reader)
    {
        return SendServer($"unshare {Player.ShortName}");
    }

    #endregion Private Methods
}