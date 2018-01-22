using Monkeyspeak;
using System;

namespace Engine.Libraries
{
    // '' <summary>
    // '' backup and restore a dreams
    // '' <see href="https://cms.furcadia.com/creations/dreammaking/dragonspeak/psalpha">Phoenix
    // '' Speak</see> database to Silver Monkey's built in SQLite database
    // '' system. Silver Monkey uses the Command Line interface to walk and
    // '' backup or restore the database for the dream.
    // '' <para>
    // '' NOTE: PhoenixSpeak Database is not SQL based like SQLite. Phoenix
    // '' Speak resembles an XML style system
    // '' </para>
    // '' <pra>Bot Testers: Be aware this class needs to be tested any way possible!</pra>
    // '' </summary>
    // '' <remarks>
    // '' furre List Looping.
    // '' <para>
    // '' first build the furre list
    // '' </para>
    // '' <para>
    // '' Send the First PhoenixSpeak Query-set to the server Enqueue
    // '' </para>
    // '' <para>
    // '' PsReceived will read the incoming server responses and keep track
    // '' which mode we're in
    // '' </para>
    // '' <para>
    // '' Read CharacterList(0).name into a variable
    // '' </para>
    // '' <para>
    // '' Remove furre at CharacterList(0)
    // '' </para>
    // '' <para>
    // '' Enqueue the Next Phoenix Speak Command
    // '' </para>
    // '' <para>
    // '' Change mode as necessary IE CharacterList.Count = 0
    // '' </para>
    // '' <para>
    // '' PSiInfoCache is the List of PhoenixSpeak.Variables last transmitted
    // '' by the server
    // '' </para>
    // '' <para>
    // '' this list does take into account Multi-Page responses from the
    // '' server and will flag an 'an overflow if page 6 is detected.
    // '' </para>
    // '' </remarks>
    public sealed class MsPhoenixSpeakBackupAndRestore : MonkeySpeakLibrary
    {
        /// <summary>
        /// Gets the base identifier.
        /// </summary>
        /// <value>
        /// The base identifier.
        /// </value>
        public override int BaseId => 550;

        /// <summary>
        /// Initializes this instance. Add your trigger handlers here.
        /// </summary>
        /// <param name="args">Parametized argument of objects to use to pass runtime objects to a library at initialization</param>
        public override void Initialize(params object[] args)
        {
            base.Initialize(args);
            // (0:500) When the bot starts backing up the furre Phoenix Speak,
            Add(TriggerCategory.Cause,
             r => true,
             "When the bot starts a Phoenix Speak backup process,");

            // (0:501) When the bot completes backing up the characters Phoenix Speak,
            Add(TriggerCategory.Cause,
             r => true,
             "When the bot completes a Phoenix Speak backup process,");

            // (0:502) When the bot starts restoring the dream's furre Phoenix Speak,
            Add(TriggerCategory.Cause,
             r => true,
             "When the bot starts restoring the dream's Phoenix Speak,");

            // (0:503) When the bot finishes restoring the dreams furre Phoenix Speak,
            Add(TriggerCategory.Cause,
             r => true,
             "When the bot finishes restoring the dream's Phoenix Speak,");

            Add(TriggerCategory.Cause,
             r => true,
             "When the bot backs up the Phoenix Speak for any furre,");

            Add(TriggerCategory.Cause,
             r => BackUpCharacterNamed(r),
             "When the bot backs up the Phoenix Speak for the furre named {...},");

            Add(TriggerCategory.Cause,
             r => true,
             "When the bot restores any furre\'s Phoenix Speak,");

            Add(TriggerCategory.Cause,
             r => BackUpCharacterNamed(r),
             "When the bot restores the Phoenix Speak for the furre named {...},");

            // (1:520) and the bot is not in the middle of a Phoenix Speak backup process
            Add(TriggerCategory.Condition,
             r => BotBackup(r),
             "and the bot is not in the middle of a Phoenix Speak backup process,");

            // (1:521) and the bot is in the middle of a Phoenix Speak backup process.
            Add(TriggerCategory.Condition,
             r => NotBotBackup(r),
             "and the bot is in the middle of a Phoenix Speak backup process,");

            // (1:522) and the bot is not in the middle of a Phoenix Speak restore process,
            Add(TriggerCategory.Condition,
             r => BotRestore(r),
             "and the bot is not in the middle of a Phoenix Speak restore process,");

            // (1:523) and the bot is in the middle of a Phoenix Speak restore process,
            Add(TriggerCategory.Condition,
             r => NotBotRestore(r),
             "and the bot is in the middle of a Phoenix Speak restore process,");

            // TODO: Add missing Phoenix Speak lines
            // (1:) and the backed up Phoenix Speak database info {...} for the triggering furre exists,
            // (1:) and the backed up Phoenix Speak database info {...} for the furre named {...} exists, (use ""[DREAM]"" to check specific info for this dream.)
            // (1:) and the backed up Phoenix Speak database info for the triggering furre exists,
            // (1:) and the backed up Phoenix Speak database info for the furre named {...} exists, (use ""[DREAM]"" to check specific info for this dream.)
            // (1:) and the backed up Phoenix Speak database info {...} for the triggering furre does not exist,
            // (1:) and the backed up Phoenix Speak database info {...} for the furre named {...} does not exist, (use ""[DREAM]"" to check specific info for this dream.)
            // (1:) and the backed up Phoenix Speak database info for the triggering furre does not exist,
            // (1:) and the backed up Phoenix Speak database info for the furre named {...} does not eist, (use ""[DREAM]"" to check specific info for this dream.)

            // (5:553) backup all furre phoenixspeak for the dream
            Add(TriggerCategory.Effect,
             r => BackupAllPS(r),
             "backup all Phoenix Speak for the dream.");

            // (5:554) backup furre named {...} Phoenix Speak
            Add(TriggerCategory.Effect,
             r => BackupSingleCharacterPS(r),
             "backup the Phoenix Speak for the furre named {...} . (use \"[DREAM]\" to restore information specific to the dream)");

            // (5:555) restore Phoenix Speak for furre {...}
            Add(TriggerCategory.Effect,
             r => RestoreCharacterPS(r),
             "restore the Phoenix Speak for furre named {...}. (use \"[DREAM]\" to restore information specific to the dream)");

            // (5:557) remove entries older then # days from Phoenix Speak furre backup.
            Add(TriggerCategory.Effect,
             r => PruneCharacterBackup(r),
             "remove entries older than # days from Phoenix Speak backup.");

            Add(TriggerCategory.Effect,
             r => RestorePS_DataOldrThanDays(r),
             "restore Phoenix Speak furre records newer then # days. (zero equals all furre records)");

            Add(TriggerCategory.Effect,
             r => AbortPS(r),
             "abort Phoenix Speak backup or restore process.");

            // (5:x) Add Settings Info {...} to Database Settings Table {...}.
            // (5:x) update Database Info {...} for Settings Table {...} to {...}.
            // (5:x) remove Database info {...} from Settings Table{...}.
            // (5:x) clear all Settings Table Database info.
            // (5:x) remove setting table {...}.
        }

        private bool AbortPS(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool RestorePS_DataOldrThanDays(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool PruneCharacterBackup(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool RestoreCharacterPS(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool BackupSingleCharacterPS(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool BackupAllPS(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool NotBotRestore(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool BotRestore(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool NotBotBackup(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool BotBackup(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool BackUpCharacterNamed(TriggerReader reader)
        {
            throw new NotImplementedException();
        }
    }
}