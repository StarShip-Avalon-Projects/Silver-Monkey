using Furcadia.Extensions;
using Furcadia.Net;
using Furcadia.Net.Utils.ServerParser;
using Libraries.PhoenixSpeak;
using MonkeyCore.Logging;
using Monkeyspeak;
using Monkeyspeak.Libraries;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static Libraries.PhoenixSpeak.PhoenixSpeakDataObject;

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
        #region Private Fields

        private const string PhoenixSpeakErrorMessage = "Sorry, I do not have access to Phoenix Speak in this dream. Please have the dream owner set my Access_Level. Reference: Furcadia Phoenix Speak https://cms.furcadia.com/creations/dreammaking/dragonspeak/psalpha";
        private ConcurrentQueue<PhoenixSpeakDataObject> phoenxSpeakObjects;

        #endregion Private Fields

        #region Public Properties

        public override int BaseId => 80;

        /// <summary>
        /// increments and gets the current Phoenix Speak ID for the current Phoenix Speak Query,
        /// <para/>
        /// Use currentPsID to check the status of the current Phoenix Speak ID
        /// </summary>
        public static short GetPsId { get; private set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public override void Initialize(params object[] args)
        {
            base.Initialize(args);
            GetPsId = 0;
            phoenxSpeakObjects = new ConcurrentQueue<PhoenixSpeakDataObject>();

            Add(TriggerCategory.Effect,
                RememberPSForTrigFurreToVariableTable,
                "remember all Phoenix Speak info about the triggering furre and put it into Variable-Table %Table.");

            Add(TriggerCategory.Effect,
                RememberPSForFurreNamedToVariableTable,
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
               "memorize Phoenix Speak table %Table about the triggering furre.");

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

            if (ParentBotSession != null)
                ParentBotSession.ProcessServerChannelData += ParsePhoenixSpeak;
        }

        public override void Unload(Page page)
        {
            GetPsId = 0;
            if (ParentBotSession != null)
                ParentBotSession.ProcessServerChannelData -= ParsePhoenixSpeak;
        }

        #endregion Public Methods

        #region Private Methods

        [TriggerDescription("Clears all the Phoenix Speak about the specified character")]
        [TriggerStringParameter]
        private bool ForgetAllPSForFurreNamed(TriggerReader reader)
        {
            var Furre = reader.ReadString();
            GetPsId++;
            if (Furre.ToLower() == "[dream]")
                SendServer($"ps {GetPsId} clear dream.*");
            else
                SendServer($"ps {GetPsId} clear character.{Furre.ToFurcadiaShortName()}");
            var result = GetPhoenixSpeakData(GetPsId).Result;

            if (result is null || result.IsError)
            {
                Logger.Debug<MsPhoenixSpeak>("An error has occurred.");
                return false;
            }
            if (result.PhoenixSpeakID <= 0)
            {
                return false;
            }
            if (GetPsId == result.PhoenixSpeakID && result.phoenixSpeakType == PhoenixSpeakTypes.Clear)
            {
                Logger.Info($"Success: {Furre} has been forgotten from this dream");
                return true;
            }

            return false;
        }

        [TriggerDescription("Clears all Phoenix Speak about the triggering furre")]
        private bool ForgetAllPSForTriggeringFurre(TriggerReader reader)
        {
            GetPsId++;
            SendServer($"ps {GetPsId} clear character.{Player.ShortName}");

            var result = GetPhoenixSpeakData(GetPsId).Result;

            if (result is null || result.IsError)
            {
                Logger.Debug<MsPhoenixSpeak>("An error has occurred.");
                return false;
            }
            if (result.PhoenixSpeakID <= 0)
            {
                return false;
            }
            if (GetPsId == result.PhoenixSpeakID && result.phoenixSpeakType == PhoenixSpeakTypes.Clear)
            {
                Logger.Info($"Success: {Player.Name} has been forgotten from this dream");
                return true;
            }

            return false;
        }

        [TriggerDescription("Clears the specified field about the specified character")]
        [TriggerStringParameter]
        [TriggerStringParameter]
        private bool ForgetPSFieldForCharacterNamed(TriggerReader reader)
        {
            var Field = reader.ReadString();
            var Furre = reader.ReadString();
            GetPsId++;
            if (Furre.ToLower() == "[dream]")
                SendServer($"ps {GetPsId} clear dream.{Field}");
            else
                SendServer($"ps {GetPsId} clear character.{Field}");
            var result = GetPhoenixSpeakData(GetPsId).Result;

            if (result is null || result.IsError)
            {
                Logger.Debug<MsPhoenixSpeak>("An error has occurred.");
                return false;
            }
            if (result.PhoenixSpeakID <= 0)
            {
                return false;
            }
            if (GetPsId == result.PhoenixSpeakID && result.phoenixSpeakType == PhoenixSpeakTypes.Clear)
            {
                Logger.Info($"Success: {Furre}'s field '{Field}' has been forgotten.");
                return true;
            }

            return false;
        }

        [TriggerDescription("Clears the specified Phoenix Speak about the triggering furre")]
        [TriggerStringParameter]
        private bool ForgetPSFieldForTriggeringFurre(TriggerReader reader)
        {
            var Field = reader.ReadString();
            GetPsId++;
            SendServer($"ps {GetPsId} clear character.{Player.ShortName}.{Field}");

            var result = GetPhoenixSpeakData(GetPsId).Result;

            if (result is null || result.IsError)
            {
                Logger.Debug<MsPhoenixSpeak>("An error has occurred.");
                return false;
            }
            if (result.PhoenixSpeakID <= 0)
            {
                return false;
            }
            if (GetPsId == result.PhoenixSpeakID && result.phoenixSpeakType == PhoenixSpeakTypes.Clear)
            {
                Logger.Info($"Success: {Player.Name}'s field '{Field}' has been forgotten.");
                return true;
            }

            return false;
        }

        private bool GetPsCharacterListToVariableTable(TriggerReader reader)
        {
            //This needs a Name walker. (increment Letter)
            throw new NotImplementedException();
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

            GetPsId++;

            if (Furre.ToLower() == "[dream]")
                SendServer($"ps {GetPsId} set dream.{Field}='{Value}'");
            else
                SendServer($"ps {GetPsId} set character.{Furre}.{Field}='{Value}'");
            var result = GetPhoenixSpeakData(GetPsId).Result;

            if (result is null || result.IsError)
            {
                Logger.Debug<MsPhoenixSpeak>("An error has occurred.");
                return false;
            }
            if (result.PhoenixSpeakID <= 0)
            {
                return false;
            }
            if (GetPsId == result.PhoenixSpeakID && result.phoenixSpeakType == PhoenixSpeakTypes.Set)
            {
                Logger.Info($"Success: {Furre}'s field '{Field}' has been memorized.");
                return true;
            }

            return false;
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

            GetPsId++;

            if (Furre.ToLower() == "[dream]")
                SendServer($"ps {GetPsId} set dream.{Field}='{Value}'");
            else
                SendServer($"ps {GetPsId} set character.{Furre}.{Field}='{Value}'");
            var result = GetPhoenixSpeakData(GetPsId).Result;

            if (result is null || result.IsError)
            {
                Logger.Debug<MsPhoenixSpeak>("An error has occurred.");
                return false;
            }
            if (result.PhoenixSpeakID <= 0)
            {
                return false;
            }
            if (GetPsId == result.PhoenixSpeakID && result.phoenixSpeakType == PhoenixSpeakTypes.Set)
            {
                Logger.Info($"Success: {Furre}'s field '{Field}' has been memorized.");
                return true;
            }

            return false;
        }

        [TriggerDescription("Sets the specified field as number or variable about the triggering furre.")]
        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool MemorizePhoenixSpeakForTriggeringFurreNumberOrVariable(TriggerReader reader)
        {
            var Field = reader.ReadString();
            var Value = reader.ReadNumber();

            GetPsId++;

            SendServer($"ps {GetPsId} set character.{Player.ShortName}.{Field}='{Value}'");
            var result = GetPhoenixSpeakData(GetPsId).Result;

            if (result is null || result.IsError)
            {
                Logger.Debug<MsPhoenixSpeak>("An error has occurred.");
                return false;
            }
            if (result.PhoenixSpeakID <= 0)
            {
                return false;
            }
            if (GetPsId == result.PhoenixSpeakID && result.phoenixSpeakType == PhoenixSpeakTypes.Set)
            {
                Logger.Info($"Success: {Player.Name}'s field '{Field}' has been memorized.");
                return true;
            }

            return false;
        }

        [TriggerDescription("Sets the specified field as string about the triggering furre.")]
        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool MemorizePhoenixSpeakForTriggeringFurreString(TriggerReader reader)
        {
            var Field = reader.ReadString();
            var Value = reader.ReadString();

            GetPsId++;

            SendServer($"ps {GetPsId} set character.{Player.ShortName}.{Field}='{Value}'");

            var result = GetPhoenixSpeakData(GetPsId).Result;

            if (result is null || result.IsError)
            {
                Logger.Debug<MsPhoenixSpeak>("An error has occurred.");
                return false;
            }
            if (result.PhoenixSpeakID <= 0)
            {
                return false;
            }
            if (GetPsId == result.PhoenixSpeakID && result.phoenixSpeakType == PhoenixSpeakTypes.Set)
            {
                Logger.Info($"Success: {Player.Name}'s field '{Field}' has been memorized.");
                return true;
            }

            return false;
        }

        [TriggerDescription("Formats the variable table to a string readable by the game server and sends the Phoenix Speak data about the specified furre.")]
        [TriggerVariableParameter]
        [TriggerStringParameter]
        private bool MemorizeVariableTableToPhoenixSpeakForCharacterNamed(TriggerReader reader)
        {
            var data = new List<string>();

            var VarTable = reader.ReadVariableTable(true);
            var Furre = reader.ReadString();

            GetPsId++;

            foreach (KeyValuePair<string, object> kvp in VarTable)
            {
                data.Add($"{kvp.Key}='{kvp.Value}'");
            }
            if (Furre.ToLower() == "[dream]")
                SendServer($"ps {GetPsId} set dream.{string.Join(",", data.ToArray())}");
            else
                SendServer($"ps {GetPsId} set character.{Furre.ToFurcadiaShortName()}.{string.Join(",", data.ToArray())}");
            var result = GetPhoenixSpeakData(GetPsId).Result;

            if (result is null || result.IsError)
            {
                Logger.Debug<MsPhoenixSpeak>("An error has occurred.");
                return false;
            }
            if (result.PhoenixSpeakID <= 0)
            {
                return false;
            }
            if (GetPsId == result.PhoenixSpeakID && result.phoenixSpeakType == PhoenixSpeakTypes.Set)
            {
                Logger.Info($"Success: {Furre}'s variable table '{VarTable.Name}' has been memorized.");
                return true;
            }

            return false;
        }

        /// <summary>
        /// memorize Phoenix Speak table %Table about the triggering furre.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        [TriggerDescription("Formats the variable table to a string readable by the game server and sends the Phoenix Speak data about the triggering furre.")]
        [TriggerVariableParameter]
        private bool MemorizeVariableTableToPhoenixSpeakForTriggeringFurre(TriggerReader reader)
        {
            var data = new List<string>();
            var VarTable = reader.ReadVariableTable(true);

            GetPsId++;

            foreach (KeyValuePair<string, object> kvp in VarTable)
            {
                data.Add($"{kvp.Key}='{kvp.Value}'");
            }
            SendServer($"ps {GetPsId} set character.{Player.ShortName}.{string.Join(",", data.ToArray())}");

            var result = GetPhoenixSpeakData(GetPsId).Result;

            if (result is null || result.IsError)
            {
                Logger.Debug<MsPhoenixSpeak>("An error has occurred.");
                return false;
            }
            if (result.PhoenixSpeakID <= 0)
            {
                return false;
            }
            if (GetPsId == result.PhoenixSpeakID && result.phoenixSpeakType == PhoenixSpeakTypes.Set)
            {
                Logger.Info($"Success: {Player.Name}'s variable table '{VarTable.Name}' has been memorized.");
                return true;
            }

            return false;
        }

        private void ParsePhoenixSpeak(object sender, ParseChannelArgs Args)
        {
            var ParseChannelThread = new Thread(() => OnParePhoenixSpeak(sender, Args));
            ParseChannelThread.Start();
        }

        /// <summary>
        /// Parse Phoenix Speak data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Args"></param>
        private void OnParePhoenixSpeak(object sender, ParseChannelArgs Args)
        {
            if (sender is ChannelObject ChanObject && (Args.Channel == "text" || Args.Channel == "error"))
            {
                Logger.Debug<MsPhoenixSpeak>(ChanObject.ChannelText);
                // Snag the PS data only if the API has sent a PS Query
                // Sample Data, Thanks Wiren ~ Gero
                // PS ### Ok: get: result: money='500', partysize='1', playerexp='0', playerlevel='1', pokeballs='15', pokemon1='7 1 n Squirtle 1 0 1 tackle', pokemon2='0', pokemon3='0', pokemon4='0', pokemon5='0', pokemon6='0', sys_lastused_date=1523076301, totalpokemon='1'
                switch (Args.Channel)
                {
                    case "text":
                        if (ChanObject.ChannelText.StartsWith("PS ") && GetPsId > 0) //
                        {
                            var PsQuery = ChanObject.ChannelText.Split(new char[] { ' ' }, 5);

                            if (short.TryParse(PsQuery[1].AsString(), out short psID) && psID > 0)
                            {
                                if (PsQuery[2] == "Error:")
                                    Logger.Warn("Sorry. there was an error detected, I cannot process the Phoenix Speak query. :(");
                                phoenxSpeakObjects.Enqueue(new PhoenixSpeakDataObject(ChanObject.RawInstruction));
                                break;
                            }
                        }
                        break;
                    //Sorry, you do not have access to this PhoenixSpeak command.
                    case "error":
                        if (ChanObject.ChannelText == "Sorry, you do not have access to this PhoenixSpeak command.")
                        {
                            Logger.Warn(PhoenixSpeakErrorMessage);
                        }
                        break;
                        // PS ### Error: get: Query error: Field 'field' does not exist
                }
                if (phoenxSpeakObjects.Count == 0)
                    GetPsId = 0;
            }
        }

        [TriggerDescription("Sends a Phoenix Speak Command to the Furcadia game-server, that requests all data for the specified furre and puts the results to a variable table. On error, this will stop return false and stop executing.")]
        [TriggerVariableParameter]
        private bool RememberPSForFurreNamedToVariableTable(TriggerReader reader)
        {
            var Furre = reader.ReadString();
            var table = reader.ReadVariableTable(true);

            GetPsId++;

            if (Furre.ToLower() == "[dream]")
                SendServer($"ps {GetPsId} get dream.*");
            else
                SendServer($"ps {GetPsId} get character.{Furre.ToFurcadiaShortName()}.*");

            // Wait for game server to give us the data
            var result = GetPhoenixSpeakData(GetPsId).Result;

            if (result is null || result.IsError)
            {
                Logger.Debug<MsPhoenixSpeak>("An error has occurred.");
                return false;
            }
            if (result.PhoenixSpeakID <= 0)
            {
                return false;
            }

            if (GetPsId == result.PhoenixSpeakID)
            {
                foreach (var variable in result.PsTable)
                    table.Add(variable);
                Logger.Info($"Success: {Furre}'s Phoenix Speak has been remembered to Variable-Table '{table.Name}'.");
            }

            return false;
        }

        [TriggerDescription("Sends a Phoenix Speak Command to the Furcadia game-server, that requests all data for the triggering furre and puts the results to a variable table. On error, this will stop return false and stop executing.")]
        [TriggerVariableParameter]
        private bool RememberPSForTrigFurreToVariableTable(TriggerReader reader)
        {
            var table = reader.ReadVariableTable(true);

            GetPsId++;
            SendServer($"ps {GetPsId} get character.{Player.ShortName}.*");
            Logger.Debug<MsPhoenixSpeak>($"ps {GetPsId} get character.{Player.ShortName}.*");

            // Snag Ps Data and throw it into the specified Table
            var result = GetPhoenixSpeakData(GetPsId).Result;

            if (result is null || result.IsError)
            {
                Logger.Debug<MsPhoenixSpeak>("An error has occurred.");
                return false;
            }
            if (result.PhoenixSpeakID <= 0)
            {
                return false;
            }

            if (GetPsId == result.PhoenixSpeakID)
            {
                foreach (var variable in result.PsTable)
                    table.Add(variable);

                Logger.Info($"Success: {Player.Name}'s Phoenix Speak has been remembered to Variable-Table '{table.Name}'.");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Used after sending a Phoenix Speak command to the game server to retrieve the Phoenix Speak data
        /// </summary>
        /// <param name="CurrentPsId"></param>
        /// <returns></returns>
        private Task<PhoenixSpeakDataObject> GetPhoenixSpeakData(short CurrentPsId = -1)
        {
            if (CurrentPsId < 1)
                throw new ArgumentOutOfRangeException("CurrentPsId", CurrentPsId, "Current Phoenix Speak ID is out of range.");
            var TimeOut = DateTime.Now.AddSeconds(3);
            do
            {
                if (DateTime.Now > TimeOut)
                {
                    Logger.Warn($"Timed out while waiting for Phoenix Speak Data with ID {CurrentPsId}");
                    break;
                }
                Task.Delay(300);
            } while (phoenxSpeakObjects.Count == 0);

            if (phoenxSpeakObjects.TryPeek(out PhoenixSpeakDataObject result))
            {
                if (result.PhoenixSpeakID == CurrentPsId)
                    phoenxSpeakObjects.TryDequeue(out result);
                else
                    Logger.Error($"Expected CurrentPsId: '{CurrentPsId}' but got result.PhoenixSpeakID: '{result.PhoenixSpeakID}'");
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            return Task.FromResult(result);
        }

        #endregion Private Methods
    }
}