using Monkeyspeak;
using System;

namespace Libraries
{
    // '' <summary>
    // '' Monkey Speak Interface to the
    // '' <see href="https://cms.furcadia.com/creations/dreammaking/dragonspeak/psalpha">Phoenix
    // '' Speak</see> server command line interface
    // '' <para>
    // '' Checks and executes predefined Phoenix Speak commands to manages a
    // '' dreams database.
    // '' </para>
    // '' <pra>Bot Testers: Be aware this class needs to be tested any way possible!</pra>
    // '' </summary>
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
            // (5:60) get all Phoenix Speak info for the triggering furre and put it into the PSInfo Cache.
            Add(TriggerCategory.Effect,
                r => RemberPSInforTrigFurre(r),
                "get all Phoenix Speak info for the triggering furre and put it into the PSInfo Cache.");

            // (5:61) get all Phoenix Speak info for the Furre Named {...} and put it into the PSInfo Cache.
            Add(TriggerCategory.Effect,
                r => RemberPSInforFurreNamed(r),
                "get all Phoenix Speak info for the Furre Named {...} and put it into the PSInfo Cache.");

            // (5:62) get all Phoenix Speak info for the dream and put it into the PSInfo Cache.
            Add(TriggerCategory.Effect,
                r => RemberPSInfoAllDream(r),
                "get all Phoenix Speak info for the dream and put it into the PSInfo Cache.");

            // (5:63) get all Phoenix Speak info for all characters and put it into the PSInfo cache.
            Add(TriggerCategory.Effect,
                r => RemberPSInfoAllCharacters(r),
                "get all list of all characters and put it into the PSInfo cache.");

            // (5:80) retrieve  Phoenix Speak info {...} and place the value into variable %variable.
            Add(TriggerCategory.Effect,
                r => GetPSinfo(r),
                "retrieve  Phoenix Speak info {...} and place the value into variable %variable.");

            // (5:81) store PSInfo Key Names to variable %variable.
            Add(TriggerCategory.Effect,
                r => PSInfoKeyToVariable(r),
                "store PSInfo Key Names to variable %variable.");

            // (5:82) memorize Phoenix Speak info {...} for the Furre Named {...}.
            Add(TriggerCategory.Effect,
                r => MemorizeFurreNamedPS(r),
                "memorize Phoenix Speak info {...} for the Furre Named {...}.");

            // (5:83) forget Phoenix Speak info {...} for the Furre Named {...}.
            Add(TriggerCategory.Effect,
                r => ForgetFurreNamedPS(r),
                "forget Phoenix Speak info {...} for the Furre Named {...}.");

            // (5:84) memorize Phoenix Speak info {...} for the Triggering Furre.
            Add(TriggerCategory.Effect,
                r => MemorizeTrigFurrePS(r),
                "memorize Phoenix Speak info {...} for the Triggering Furre.");

            // (5:85) forget Phoenix Speak info {...} for the Triggering Furre.
            Add(TriggerCategory.Effect,
                r => ForgetTrigFurrePS(r),
                "forget Phoenix Speak info {...} for the Triggering Furre.");

            // (5:90) memorize Phoenix Speak info {...} for this dream.
            Add(TriggerCategory.Effect,
                r => MemorizeDreamPS(r),
                "memorize Phoenix Speak info {...} for this dream.");

            // (5:91) forget Phoenix Speak info {...} for this dream.
            Add(TriggerCategory.Effect,
                r => ForgetDreamPS(r),
                "forget Phoenix Speak info {...} for this dream.");

            // (5:94) execute Phoenix Speak command {...}.
            Add(TriggerCategory.Effect,
                r => PSCommand(r),
                "execute Phoenix Speak command {...}.");

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

        private bool PSCommand(TriggerReader reader)
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

        private bool RemberPSInfoAllCharacters(TriggerReader reader)
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