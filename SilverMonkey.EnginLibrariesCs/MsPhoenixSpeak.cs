using Monkeyspeak;
using System;

namespace Libraries
{
    /// <summary>
    /// Monkey Speak Interface to the
    /// <see href="https://cms.furcadia.com/creations/dreammaking/dragonspeak/psalpha">Phoenix
    /// Speak</see> server command line interface
    /// <para>
    /// Checks and executes predefined Phoenix Speak commands to manages a
    /// dreams database.
    /// </para>
    /// <pra>Bot Testers: Be aware this class needs to be tested any way possible!</pra>
    /// </summary>
    public class MsPhoenixSpeak : MonkeySpeakLibrary
    {
        public override int BaseId => 80;

        public override void Initialize(params object[] args)
        {
            base.Initialize(args);
            Add(TriggerCategory.Cause,
                r => true,
                "When the bot sees a Phoenix Speak response");

            Add(TriggerCategory.Cause,
                r => MsgIs(r),
                 "When the bot sees the Phoenix Speak response {...},");

            Add(TriggerCategory.Cause,
                r => MsgContains(r),
                "When the bot sees a Phoenix Speak response with {...} in it,");

            // TODO: Add Monkeu Speak
            // (1xxx) And the Database info {...} about the triggerig furre exists.
            // (1:xxx) And the Database info {...} about the triggerig furre does Not exist.
            // (1:xxx) And the Database info {...} about the furre named {...} exists.
            // (1:xxx) And the Database info {...} about the furre named {...} does Not exist.

            Add(TriggerCategory.Effect,
                r => RemberPSInforTrigFurre(r),
                "get all Phoenix Speak info for the triggering furre and put it into Variable Table %Table.");

            Add(TriggerCategory.Effect,
                r => RemberPSInforFurreNamed(r),
                "get all Phoenix Speak info for the Furre Named {...} and put it into Variable Table %Table");

            Add(TriggerCategory.Effect,
                r => RemberPSInfoAllDream(r),
                "get all Phoenix Speak info for the dream and put it into Variable Table %Table");

            Add(TriggerCategory.Effect,
               r => RememberPSInfoAllCharacters(r),
               "get all list of all characters and put it into the Variable Table %Table");

            Add(TriggerCategory.Effect,
                r => PSInfoKeyToVariable(r),
                "store PSInfo Key Names to variable %variable.");

            Add(TriggerCategory.Effect,
                r => MemorizeFurreNamedPS(r),
                "memorize Phoenix Speak info {...} for the Furre Named {...}.");

            Add(TriggerCategory.Effect,
                r => ForgetFurreNamedPS(r),
                "forget Phoenix Speak info {...} for the Furre Named {...}.");

            Add(TriggerCategory.Effect,
                r => MemorizeTrigFurrePS(r),
                "memorize Phoenix Speak info {...} for the Triggering Furre.");

            Add(TriggerCategory.Effect,
                r => ForgetTrigFurrePS(r),
                "forget Phoenix Speak info {...} for the Triggering Furre.");

            Add(TriggerCategory.Effect,
                r => MemorizeDreamPS(r),
                "memorize Phoenix Speak info {...} for this dream.");

            Add(TriggerCategory.Effect,
                r => ForgetDreamPS(r),
                "forget Phoenix Speak info {...} for this dream.");

            Add(TriggerCategory.Effect,
                r => PSforgetTriggeringFurre(r),
                "forget ALL Phoenix Speak info for the triggering furre");

            Add(TriggerCategory.Effect,
                r => SendServer($"ps clear {r.ReadString().ToFurcadiaShortName()}"),
                "forget ALL Phoenix Speak info for the furre named {...}.");

            Add(TriggerCategory.Effect,
                r => SendServer("ps clear dream"),
                "forget ALL Phoenix Speak info for this dream.");
        }

        private bool PSforgetTriggeringFurre(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool ForgetDreamPS(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool MemorizeDreamPS(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool ForgetTrigFurrePS(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool MemorizeTrigFurrePS(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool ForgetFurreNamedPS(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool MemorizeFurreNamedPS(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool PSInfoKeyToVariable(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool GetPSinfo(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool RememberPSInfoAllCharacters(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool RemberPSInfoAllDream(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool RemberPSInforFurreNamed(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool RemberPSInforTrigFurre(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        public override void Unload(Page page)
        {
        }
    }
}