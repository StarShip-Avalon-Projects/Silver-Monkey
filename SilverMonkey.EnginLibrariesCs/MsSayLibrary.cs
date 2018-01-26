using Monkeyspeak;

namespace Engine.Libraries
{
    /// <summary>
    /// Legacy Furcadia channel processing
    /// <para>
    /// This lib handles the basic channels, Emote, Say (Speech and Spoken Furcadia commands), Whispers
    /// </para>
    /// <pra>Bot Testers: Be aware this class needs to be tested any way possible!</pra>
    /// <para>
    /// TODO: Upgrade to AngelCat style Channels and Reintegrate into the engine. These channels may
    ///       still work with the existing system. <see
    ///       href="http://bugtraq.tsprojects.org/view.php?id=107">[BUG: 107]</see>
    /// </para>
    /// </summary>
    /// <remarks>
    /// This Lib contains the following unnamed delegates:
    /// <para>(0:1) When the bot logs into Furcadia,</para>
    /// <para>(0:2) When the bot logs out of Furcadia,</para>
    /// <para>(0:3) When the Furcadia client disconnects or closes,"</para>
    /// <para>
    /// (0:5) When anyone says something,
    /// <para>Ignores Bots speech</para>
    /// </para>
    /// <para>
    /// (0:8) When anyone shouts something,
    /// <para>Ignores Bots speech</para>
    /// </para>
    /// <para>
    /// (0:11) When anyone emotes something,
    /// <para>Ignores Bots speech</para>
    /// </para>
    /// <para>
    /// (0:15) When anyone whispers something,
    /// <para>Ignores Bots speech</para>
    /// </para>
    /// <para>
    /// (0:18) When anyone says or emotes something,
    /// <para>Ignores Bots speech</para>
    /// </para>
    /// <para>
    /// (0:21) When anyone emits something,
    /// <para>Ignores Bots speech</para>
    /// </para>
    /// <para>(0:24) When anyone enters the Dream,</para>
    /// <para>(0:26) When anyone leaves the Dream,</para>
    /// <para>
    /// (0:32) When anyone requests to summon the bot,
    /// <para>Ignores Bots speech</para>
    /// </para>
    /// <para>
    /// (0:34) When anyone requests to join the bot,
    /// <para>Ignores Bots speech</para>
    /// </para>
    /// <para>
    /// (0:36) When anyone requests to follow the bot,
    /// <para>Ignores Bots speech</para>
    /// </para>
    /// <para>
    /// (0:38) When anyone requests to lead the bot,
    /// <para>Ignores Bots speech</para>
    /// </para>
    /// <para>
    /// (0:40) When anyone requests to cuddle with the bot,
    /// <para>Ignores Bots speech</para>
    /// (0:92) When the bot detects the "Your throat is tired. Please wait a few seconds"message,
    /// </para>
    /// <para>(0:93) When the bot resumes processing after seeing ""Your throat is tired""message,</para>
    /// <para>(5:0) say {...}. (Normal Furcadia Text commands)</para>
    /// <para>(5:1) emote {...}.</para>
    /// <para>(5:2) shout {...}.</para>
    /// <para>(5:3) Emit {...}.</para>
    /// <para>(5:4) Emitloud {...}.</para>
    /// <para>(5:5) whisper {...} to the triggering furre.</para>
    /// <para>(5:6) whisper {...} to furre named {...}.</para>
    /// <para>(5:7) whisper {...} to furre named {...} even if they're off-line.</para>
    /// </remarks>
    public sealed class MsSayLibrary : MonkeySpeakLibrary
    {
        #region Public Properties

        public override int BaseId => 10;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Initializes this instance. Add your trigger handlers here.
        /// </summary>
        /// <param name="args">Parametized argument of objects to use to pass runtime objects to a library at initialization</param>
        public override void Initialize(params object[] args)
        {
            base.Initialize(args);

            // (0:10) When anyone says something,
            Add(TriggerCategory.Cause,
                r => ReadTriggeringFurreParams(r) && !IsConnectedCharacter(Player),
                "When anyone says something,");
            // (0:11) When anyone says {...},
            Add(TriggerCategory.Cause,
                r => MsgIs(r),
                "When anyone says {...},");

            // (0:12) When anyone says something with {...} in it,
            Add(TriggerCategory.Cause,
                r => MsgContains(r),
                "When anyone says something with {...} in it,");

            // (0:13: When anyone shouts something,
            Add(TriggerCategory.Cause,
                 r => ReadTriggeringFurreParams(r) && !IsConnectedCharacter(Player),
                "When anyone shouts something,");
            // (0:14) When anyone shouts {...},
            Add(TriggerCategory.Cause,
                r => MsgIs(r),
                "When anyone shouts {...},");

            // (0:15) When anyone shouts something with {...} in it,
            Add(TriggerCategory.Cause,
                r => MsgContains(r),
                "When anyone shouts something with {...} in it,");

            // (0:16) When anyone emotes something,
            Add(TriggerCategory.Cause,
                r => ReadTriggeringFurreParams(r) && !IsConnectedCharacter(Player),
                "When anyone emotes something,");

            // (0:17) When anyone emotes {...},
            Add(TriggerCategory.Cause,
                r => MsgIs(r),
                "When anyone emotes {...},");

            // (0:18) When anyone emotes something with {...} in it,
            Add(TriggerCategory.Cause,
                r => MsgContains(r),
                "When anyone emotes something with {...} in it,");

            // (0:19) When anyone whispers something,
            Add(TriggerCategory.Cause,
                r => ReadTriggeringFurreParams(r) && !IsConnectedCharacter(Player),
                "When anyone whispers something,");

            // (0:20) When anyone whispers {...},
            Add(TriggerCategory.Cause,
                r => MsgIs(r),
                "When anyone whispers {...},");

            // (0:21) When anyone whispers something with {...} in it,
            Add(TriggerCategory.Cause,
                r => MsgContains(r),
                "When anyone whispers something with {...} in it,");

            // Says or Emotes
            // (0:22) When anyone says or emotes something,
            Add(TriggerCategory.Cause,
                r => ReadTriggeringFurreParams(r),
                "When anyone says or emotes something,");
            // (0:23) When anyone says or emotes {...},
            Add(TriggerCategory.Cause,
                r => MsgIs(r),
                "When anyone says or emotes {...},");

            // (0:24) When anyone says or emotes something with {...} in it,
            Add(TriggerCategory.Cause,
                r => MsgContains(r),
                "When anyone says or emotes something with {...} in it,");

            // (0:25) When someone emits something,
            Add(TriggerCategory.Cause,
                   r => ReadTriggeringFurreParams(r) && ReadDreamParams(r),
                 "When someone emits something,");
            // (0:26) When someone emits {...},
            Add(TriggerCategory.Cause,
                r => MsgIs(r),
                "When someone emits {...},");

            // (0:27) When someone emits something with {...} in it,
            Add(TriggerCategory.Cause,
              r => MsgContains(r),
              "When someone emits something with {...} in it,");

            //  (1:3) and the triggering furre's name is {...},
            Add(TriggerCategory.Condition,
                r => NameIs(r),
                "and the triggering furre\'s name is {...},");

            //  (1:4) and the triggering furre's name is not {...},
            Add(TriggerCategory.Condition,
                r => !NameIs(r),
                "and the triggering furre\'s name is not {...},");

            //  (1:5) and the Triggering Furre's message is {...}, (say, emote,
            //  shot, whisper, or emit Channels)
            Add(TriggerCategory.Condition,
                r => MsgIs(r),
                "and the triggering furre\'s message is {...},");

            //  (1:8) and the triggering furre's message contains {...} in it,
            //  (say, emote, shot, whisper, or emit Channels)
            Add(TriggerCategory.Condition,
                r => MsgContains(r),
                "and the triggering furre\'s message contains {...} in it,");

            // (1:9) and the triggering furre's message does not contain {...} in it,
            // (say, emote, shot, whisper, or emit Channels)
            Add(TriggerCategory.Condition,
                r => !MsgContains(r),
                "and the triggering furre\'s message does not contain {...} in it,");

            // (1:10) and the triggering furre's message is not {...},
            // (say, emote, shot, whisper, or emit Channels)
            Add(TriggerCategory.Condition,
                r => !MsgIs(r),
                "and the triggering furre\'s message is not {...},");

            // (1:11) and triggering furre's message starts with {...},
            Add(TriggerCategory.Condition,
                r => MsgStartsWith(r),
                "and triggering furre\'s message starts with {...},");

            // (1:12) and triggering furre's message doesn't start with {...},
            Add(TriggerCategory.Condition,
                r => MsgNotStartsWith(r),
                "and triggering furre\'s message doesn\'t start with {...},");

            // (1:13) and triggering furre's message  ends with {...},
            Add(TriggerCategory.Condition,
                r => MsgEndsWith(r),
                "and triggering furre\'s message  ends with {...},");

            // (1:14) and triggering furre's message doesn't end with {...},
            Add(TriggerCategory.Condition,
                r => MsgNotEndsWith(r),
                "and triggering furre\'s message doesn\'t end with {...},");

            // Says
            //  (5:0) say {...}.
            Add(TriggerCategory.Effect,
                r => SendSay(r.ReadString()),
                "say {...} (Furcadia commands or normal speech).");

            // emotes
            //  (5:1) emote {...}.
            Add(TriggerCategory.Effect,
                r => SendEmote(r.ReadString()),
                "emote  message {...}.");

            // Shouts
            //  (5:2) shout {...}.
            Add(TriggerCategory.Effect,
                r => SndShout(r.ReadString()),
                "shout message {...}.");

            // Emits
            //  (5:3) emit {...}.
            Add(TriggerCategory.Effect,
                r => SendEmit(r.ReadString()),
                "emit message {...}.");

            //  (5:4) emitloud {...}.
            Add(TriggerCategory.Effect,
                r => SendEmitLoud(r.ReadString()),
                "emit-loud message {...}.");

            // Whispers
            //  (5:5) whisper {...} to the triggering furre.
            Add(TriggerCategory.Effect,
                r => SndWhisper(Player.ShortName, r.ReadString()),
                "whisper {...} to the triggering furre.");

            //  (5:6) whisper {...} to {...}.
            Add(TriggerCategory.Effect,
                r =>
                    {
                        string msg = r.ReadString();
                        string tname = r.ReadString();
                        return SndWhisper(tname, msg);
                    }, "whisper {...} to furre named {...}.");

            //  (5:7) whisper {...} to {...} even if they're off-line.
            Add(TriggerCategory.Effect,
                r =>
                    {
                        string msg = r.ReadString();
                        string tname = r.ReadString();
                        return SendOffLineWhisper(tname, msg);
                    }, "whisper {...} to furre named {...} even if they\'re off-line.");

            Add(TriggerCategory.Effect,
                r =>
                {
                    var var = r.ReadVariable(true);
                    var.Value = Player.Message;
                    return true;
                }, "set variable %variable to the last message seen.");
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

        private bool MsgNotStartsWith(TriggerReader reader)
        {
            return !MsgStartsWith(reader);
        }

        /// <summary>
        /// send a local emit to the server queue
        /// </summary>
        /// <param name="msg">
        /// message to send
        /// </param>
        private bool SendEmit(string msg)
        {
            if (!string.IsNullOrWhiteSpace(msg))
            {
                return SendServer($"emit {msg}");
            }

            return false;
        }

        /// <summary>
        /// send an emitloud command to the server queue
        /// </summary>
        /// <param name="msg">
        /// message to send
        /// </param>
        private bool SendEmitLoud(string msg)
        {
            if (!string.IsNullOrWhiteSpace(msg))
            {
                return SendServer($"emitloud {msg}");
            }

            return false;
        }

        /// <summary>
        /// Send an emote to the server queue
        /// </summary>
        /// <param name="msg">
        /// message to send
        /// </param>
        private bool SendEmote(string msg)
        {
            if (!string.IsNullOrWhiteSpace(msg))
            {
                return SendServer($":{ msg}");
            }

            return false;
        }

        /// <summary>
        /// Send an off line whisper to the server queue
        /// </summary>
        /// <param name="name">
        /// recipients name
        /// </param>
        /// <param name="msg">
        /// Message to send
        /// </param>
        private bool SendOffLineWhisper(string name, string msg)
        {
            if (!string.IsNullOrWhiteSpace(msg))
            {
                return SendServer($"/%%{name.ToFurcadiaShortName()} {msg}");
            }

            return false;
        }

        /// <summary>
        /// send a speech command to the server queue
        /// </summary>
        /// <param name="msg">
        /// message to send
        /// </param>
        private bool SendSay(string msg)
        {
            if (!string.IsNullOrWhiteSpace(msg))
            {
                return SendServer(msg);
            }

            return false;
        }

        /// <summary>
        /// Send a shout to the server queue
        /// </summary>
        /// <param name="msg">
        /// Message to send
        /// </param>
        private bool SndShout(string msg)
        {
            if (!string.IsNullOrWhiteSpace(msg))
            {
                return SendServer($"-{ msg}");
            }

            return false;
        }

        /// <summary>
        /// Send a whisper to the server queue
        /// </summary>
        /// <param name="name">
        /// recipients name
        /// </param>
        /// <param name="msg">
        /// Message to send
        /// </param>
        private bool SndWhisper(string name, string msg)
        {
            if (!string.IsNullOrWhiteSpace(msg))
            {
                return SendServer($"/%{name.ToFurcadiaShortName()} {msg}");
            }

            return false;
        }

        #endregion Private Methods
    }
}