using Monkeyspeak;
using System;
using static Controls.NativeMethods;
using static Controls.WindowsMessageing;
using static Libraries.MsLibHelper;

namespace Libraries
{
    /// <summary>
    /// Bot to Bot on same commputer messaging
    /// </summary>
    public class MsBotMessage : MonkeySpeakLibrary
    {
        #region Public Properties

        public override int BaseId => 210;

        #endregion Public Properties

        #region Public Methods

        public override void Initialize(params object[] args)
        {
            base.Initialize(args);

            Add(TriggerCategory.Cause,
                 r => true,
                 "When the bot receives a message from another bot on the same computer,");

            Add(TriggerCategory.Cause,
                r => ReceiveMessage(r),
                "When the bot receives message {...} from another bot on the same computer,");

            Add(TriggerCategory.Cause,
                r => ReceiveMessageContaining(r),
                "When the bot receives a message containing {...} from another bot on the same computer,");

            Add(TriggerCategory.Effect,
               r => SendMessage(r),
               "send message {...} to bot named {...}.");

            Add(TriggerCategory.Effect,
               r => SetVariable(r),
               "set Variable %Variable to the Message the bot last received.");
        }

        public override void Unload(Page page)
        {
        }

        #endregion Public Methods

        #region Private Methods

        private bool ReceiveMessage(TriggerReader reader)
        {
            var msMsg = reader.ReadString();
            var msg = reader.Page.GetVariable(MessageVariable).Value.ToString();
            return msg.Equals(msMsg);
        }

        private bool ReceiveMessageContaining(TriggerReader reader)
        {
            var msMsg = reader.ReadString();
            var msg = reader.Page.GetVariable(MessageVariable).Value.ToString();
            return msg.Contains(msMsg);
        }

        private bool SendMessage(TriggerReader reader)
        {
            var msMsg = reader.ReadString().Trim();
            var Fur = reader.ReadString();

            // Step 1.
            // To send a message to another application the first thing we need is the
            // handle of the receiving application.
            // One way is to use the FindWindow API
            var WindowHandleOfToProcess = FindWindow(null, $"Silver Monkey: { Fur}");

            // Step 2.
            // Create some information to send.
            string strTag = "MSG";
            var iResult = IntPtr.Zero;
            if (WindowHandleOfToProcess != IntPtr.Zero)
            {
                iResult = SendWindowsStringMessage(WindowHandleOfToProcess, IntPtr.Zero, ParentBotSession.ConnectedFurre.ShortName, ParentBotSession.ConnectedFurre.FurreID, strTag, msMsg);
                SendClientMessage($"SYSTEM Send Windows Message to {Fur}: {msMsg}");
            }

            return iResult != IntPtr.Zero;
        }

        private bool SetVariable(TriggerReader reader)
        {
            var Var = reader.ReadVariable(true);
            Var.Value = Player.Message;
            return true;
        }

        #endregion Private Methods
    }
}