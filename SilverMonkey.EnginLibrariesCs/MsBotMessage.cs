using Utils;
using Monkeyspeak;
using static Controls.NativeMethods;
using static Libraries.MsLibHelper;
using Libraries;
using System;

namespace Libraries
{
    /// <summary>
    /// Bot to Bot Messaging using Window Calls
    /// </summary>
    public class MsBotMessage : MonkeySpeakLibrary
    {
        public override void Initialize(params object[] args)
        {
            base.Initialize(args);
            // (0:75) When the bot receives a message from another bot on the same computer,
            // Add(TriggerCategory.Cause, 75,
            //      Function()
            //          Return True
            //      End Function, "When the bot receives a message from another bot on the same computer,")
            ///(0:76) When the bot receives message {...} from another bot on the same computer,
            // Add(TriggerCategory.Cause, 76,
            //     AddressOf ReceiveMessage, "When the bot receives message {...} from another bot on the same computer,")
            ///(0:77) When the bot receives a message containing {...} from another bot on the same computer,
            // Add(TriggerCategory.Cause, 77,
            //    AddressOf ReceiveMessageContaining, "When the bot receives a message containing {...} from another bot on the same computer,")
            ///(5:75) send message {...} to bot named {...}.
            // Add(TriggerCategory.Effect, 75,
            //     AddressOf SendMessage, "send message {...} to bot named {...}.")
            ///(5:76) set Variable %Variable to the Message the bot last received.
            // Add(TriggerCategory.Effect, 76,
            //     AddressOf SetVariable, "set Variable %Variable to the Message the bot last received.")
        }

        public override void Unload(Page page)
        {
        }

        /// <summary>
        /// (0:76) When the bot receives message {...} from another bot on
        /// the same computer,
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// </returns>
        private bool ReceiveMessage(TriggerReader reader)
        {
            var msMsg = reader.ReadString();
            var msg = reader.Page.GetVariable(MessageVariable).Value.ToString();
            return msg.Equals(msMsg);
        }

        /// <summary>
        /// (0:77) When the bot receives a message containing {...} from
        /// another bot on the same computer,
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// </returns>
        private bool ReceiveMessageContaining(TriggerReader reader)
        {
            var msMsg = reader.ReadString();
            var msg = reader.Page.GetVariable(MessageVariable).Value.ToString();
            return msg.Contains(msMsg);
        }

        /// <summary>
        /// (5:75) send message {...} to bot named {...}.
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// </returns>
        public bool SendMessage(TriggerReader reader)
        {
            // Debug.Print("msgContains Begin Execution")
            var msMsg = reader.ReadString().Trim();
            // Debug.Print("msMsg = "& msMsg)
            var Fur = reader.ReadString();
            // Step 1.
            // To send a message to another application the first thing we need is the
            // handle of the receiving application.
            // One way is to use the FindWindow API
            string cstrReceiverWindowName = $"Silver Monkey: { Fur}";
            var WindowHandleOfToProcess = FindWindow(null, cstrReceiverWindowName);

            var msg = new MessageHelper();
            // Step 2.
            // Create some information to send.
            string strTag = "MSG";
            var iResult = IntPtr.Zero;
            if (WindowHandleOfToProcess != IntPtr.Zero)
            {
                iResult = msg.SendWindowsStringMessage(WindowHandleOfToProcess, IntPtr.Zero, ParentBotSession.ConnectedFurre.ShortName, ParentBotSession.ConnectedFurre.FurreID, strTag, msMsg);
                SendClientMessage($"SYSTEM Send Windows Message to {Fur}: {msMsg}");
            }

            // Debug.Print("Msg = "& msg)
            return true;
        }

        /// <summary>
        /// (5:76) set Variable %Variable to the Message the bot last received.
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// </returns>
        private bool SetVariable(TriggerReader reader)
        {
            // TODO: Add markup filtering
            var Var = reader.ReadVariable(true);
            Var.Value = Player.Message;
            return true;
        }
    }
}