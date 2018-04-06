using Monkeyspeak;
using Monkeyspeak.Libraries;
using System;
using System.Collections.Generic;
using System.Text;

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
                "When the bot sees a Phoenix Speak response,");

            Add(TriggerCategory.Cause,
                r => MsgIs(r),
                 "When the bot sees the Phoenix Speak response {...},");

            Add(TriggerCategory.Cause,
                r => MsgContains(r),
                "When the bot sees a Phoenix Speak response with {...} in it,");

            Add(TriggerCategory.Effect,
                RemberPSForTrigFurreToVariableTable,
                "remeber all Phoenix Speak info about the triggering furre and put it into Variable-Table %Table.");

            Add(TriggerCategory.Effect,
                RemberPSForFurreNamedToVariableTable,
                "remember all Phoenix Speak info about the furre named {...} and put it into Variable-Table %Table. (use [DREAM] for this Dream Phoenix Speak)");

            Add(TriggerCategory.Effect,
               GetPsCharacterListToVariableTable,
               "get a list of all characters from this dream's Phoenix Speak and put it into the Variable-Table %Table");

            Add(TriggerCategory.Effect,
                MemorizeFieldToPhoenixSpeakForCharacterNamedString,
                "memorize Phoenix Speak info {...} about the furre named {...} will now be {...}. (use [DREAM] for this dream's Phoenix Speak)");

            Add(TriggerCategory.Effect,
                MemorizeFieldToPhoenixSpeakForCharacterNamedNumberOrVariable,
                "memorize Phoenix Speak info {...} about the furre named {...} will now be #. (use [DREAM] for this dream's Phoenix Speak)");

            Add(TriggerCategory.Effect,
               MemorizeVariableTableToPhoenixSpeakForCharacterNamed,
               "memorize %Table to Phoenix Speak character named {...} (use [DREAM] for this dream's Phoenix Speak)");

            Add(TriggerCategory.Effect,
               MemorizeVariableTableToPhoenixSpeakForTriggeringFurre,
               "memorize %Table to Phoenix Speak about the triggering furre. (use [DREAM] for this dream's Phoenix Speak)");

            Add(TriggerCategory.Effect,
                ForgetPSFieldForCharacterNamed,
                "forget Phoenix Speak info {...} about the furre named {...}. (use [DREAM] for this dream's Phoenix Speak)");

            Add(TriggerCategory.Effect,
                MemorizePhoenixSpeakForTriggeringFurreString,
                "memorize Phoenix Speak info {...} about the triggering furre will now be {...}.");

            Add(TriggerCategory.Effect,
                MemorizePhoenixSpeakForTriggeringFurreNumberOrVariable,
                "memorize Phoenix Speak info {...} about the triggering furre will now be #.");

            Add(TriggerCategory.Effect,
                ForgetPSFieldForTriggeringFurre,
                "forget Phoenix Speak info {...} about the triggering furre.");

            Add(TriggerCategory.Effect,
                ForgetAllPSForTriggeringFurre,
                "forget all Phoenix Speak info about the triggering furre");

            Add(TriggerCategory.Effect,
                 ForgetAllPSForFurreNamed,
                "forget all Phoenix Speak info about the furre named {...}. (use [DREAM] for this dream's Phoenix Speak)");
        }

        [TriggerDescription("Sets the specified field as number or variable about the triggering furre.")]
        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool MemorizePhoenixSpeakForTriggeringFurreNumberOrVariable(TriggerReader reader)
        {
            var Field = reader.ReadString();
            var Value = reader.ReadNumber();
            return SendServer($"ps set character.{Player.ShortName}.{Field}={Value}");
        }

        [TriggerDescription("Sets the specified field as string about the triggering furre.")]
        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool MemorizePhoenixSpeakForTriggeringFurreString(TriggerReader reader)
        {
            var Field = reader.ReadString();
            var Value = reader.ReadString();
            return SendServer($"ps set character.{Player.ShortName}.{Field}={Value}");
        }

        [TriggerDescription("Sets the specified field about the specified character.")]
        [TriggerStringParameter]
        [TriggerStringParameter]
        [TriggerStringParameter]
        private bool MemorizeFieldToPhoenixSpeakForCharacterNamedString(TriggerReader reader)
        {
            var Field = reader.ReadString();
            var Furre = reader.ReadString();
            var Value = reader.ReadString();
            if (Furre.ToLower() == "[dream]")
                return SendServer($"ps set dream.{Field}");
            return SendServer($"ps set character.{Furre}.{Field}={Value}");
        }

        [TriggerDescription("Sets the specified field as number or variable about the specified character.")]
        [TriggerStringParameter]
        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool MemorizeFieldToPhoenixSpeakForCharacterNamedNumberOrVariable(TriggerReader reader)
        {
            var Field = reader.ReadString();
            var Furre = reader.ReadString();
            var Value = reader.ReadNumber();
            if (Furre.ToLower() == "[dream]")
                return SendServer($"ps set dream.{Field}");
            return SendServer($"ps set character.{Furre}.{Field}={Value}");
        }

        [TriggerDescription("Clears all Phoenx Speak about the triggering furre")]
        [TriggerStringParameter]
        private bool ForgetAllPSForTriggeringFurre(TriggerReader reader)
        {
            return SendServer($"ps clear character.{Player.ShortName}");
        }

        [TriggerDescription("Clears the specified Phoenx Speak about the triggering furre")]
        [TriggerStringParameter]
        private bool ForgetPSFieldForTriggeringFurre(TriggerReader reader)
        {
            var Field = reader.ReadString();
            return SendServer($"ps clear character.{Player.ShortName}.{Field}");
        }

        [TriggerDescription("Clears the specified field about the specified character")]
        [TriggerStringParameter]
        [TriggerStringParameter]
        private bool ForgetPSFieldForCharacterNamed(TriggerReader reader)
        {
            var Field = reader.ReadString();
            var Furre = reader.ReadString();
            if (Furre.ToLower() == "[dream]")
                return SendServer($"ps clear dream.{Field}");
            return SendServer($"ps clear character.{Field}");
        }

        [TriggerDescription("Formats the variable table to a string readable by the game server and sends the Phoenix Speak data about the specified furre.")]
        [TriggerVariableParameter]
        [TriggerStringParameter]
        private bool MemorizeVariableTableToPhoenixSpeakForCharacterNamed(TriggerReader reader)
        {
            var data = new List<string>();

            var VarTable = reader.ReadVariableTable(true);
            var Furre = reader.ReadString();

            foreach (KeyValuePair<string, object> kvp in VarTable)
            {
                data.Add($"{kvp.Key}={kvp.Value}");
            }
            if (Furre.ToLower() == "[dream]")
                return SendServer($"ps set dream.{string.Join(",", data.ToArray())}");
            return SendServer($"ps set character.{Furre.ToFurcadiaShortName()}.{string.Join(",", data.ToArray())}");
        }

        [TriggerDescription("Formats the variable table to a string readable by the game server and sends the Phoenix Speak data about the triggering furre.")]
        [TriggerVariableParameter]
        private bool MemorizeVariableTableToPhoenixSpeakForTriggeringFurre(TriggerReader reader)
        {
            var data = new List<string>();

            var VarTable = reader.ReadVariableTable(true);

            foreach (KeyValuePair<string, object> kvp in VarTable)
            {
                data.Add($"{kvp.Key}={kvp.Value}");
            }
            return SendServer($"ps set character.{Player.ShortName}.{string.Join(",", data.ToArray())}");
        }

        private bool GetPsCharacterListToVariableTable(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool RemberPSForFurreNamedToVariableTable(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool RemberPSForTrigFurreToVariableTable(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        [TriggerDescription("Clears all the Phoenix Speak about the specified character")]
        [TriggerStringParameter]
        private bool ForgetAllPSForFurreNamed(TriggerReader reader)
        {
            var Furre = reader.ReadString();
            if (Furre.ToLower() == "[dream]")
                return SendServer($"ps clear dream.*");
            return SendServer($"ps clear character.{Furre.ToFurcadiaShortName()}");
        }

        public override void Unload(Page page)
        {
        }
    }
}