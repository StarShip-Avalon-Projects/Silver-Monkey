using BotSession;
using Furcadia.Net;
using Furcadia.Net.Utils.ServerParser;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace SilverMonkey.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        #region Public Constructors

        public BotOptions SessionOptions = new BotOptions();
        public Bot FurcadiaSession;

        public ShellView()
        {
            InitializeComponent();
            SessionOptions = new BotOptions();
            FurcadiaSession = new Bot(SessionOptions);
            FurcadiaSession.ProcessServerChannelData += OnProcessServerChannelData;
            FurcadiaSession.ProcessServerInstruction += OnProcessServerInstruction;
            FurreList.ItemsSource = FurcadiaSession.Dream.Furres.ToIList;
        }

        private void OnProcessServerInstruction(object sender, ParseServerArgs Args)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                     (Action)(() =>
                     {
                         var instructionObject = (BaseServerInstruction)sender;
                         switch (instructionObject.InstructionType)
                         {
                             case ServerInstructionType.LoadDreamEvent:
                             case ServerInstructionType.SpawnAvatar:
                             case ServerInstructionType.RemoveAvatar:

                                 break;
                         }
                     }));
            }
            else
            {
                var instructionObject = (BaseServerInstruction)sender;
                switch (instructionObject.InstructionType)
                {
                    case ServerInstructionType.LoadDreamEvent:
                    case ServerInstructionType.SpawnAvatar:
                    case ServerInstructionType.RemoveAvatar:
                        FurreList.DataContext = FurcadiaSession.Dream.Furres.ToIList;
                        break;
                }
            }
        }

        private void OnProcessServerChannelData(object sender, ParseChannelArgs Args)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                    (Action)(() =>
                    {
                        var InstructionObject = (ChannelObject)sender;
                        if (!string.IsNullOrWhiteSpace(InstructionObject.FormattedChannelText))
                        {
                            LogOutputBox.AppendParagraph(InstructionObject.FormattedChannelText);
                        }
                        else if (!string.IsNullOrWhiteSpace(InstructionObject.Player.Message))
                        {
                            LogOutputBox.AppendParagraph(InstructionObject.Player.Message.ToStrippedFurcadiaMarkupString());
                        }
                        else
                        {
                            LogOutputBox.AppendParagraph(InstructionObject.RawInstruction);
                        }
                    }));
            }
            else
            {
                var InstructionObject = (ChannelObject)sender;
                if (!string.IsNullOrWhiteSpace(InstructionObject.FormattedChannelText))
                {
                    LogOutputBox.AppendParagraph(InstructionObject.FormattedChannelText);
                }
                else if (!string.IsNullOrWhiteSpace(InstructionObject.Player.Message))
                {
                    LogOutputBox.AppendParagraph(InstructionObject.Player.Message.ToStrippedFurcadiaMarkupString());
                }
                else
                {
                    LogOutputBox.AppendParagraph(InstructionObject.RawInstruction);
                }
            }
        }

        #endregion Public Constructors

        #region Private Methods

        private void ButtonGetDrop_Click(object sender, RoutedEventArgs e)
        {
        }

        private async void ButtonGo_Click(object sender, RoutedEventArgs e)
        {
            if (FurcadiaSession.IsServerSocketConnected)
                FurcadiaSession.Disconnect();
            else
            {
                await FurcadiaSession.ConnetAsync();
            }
        }

        private void ButtonMoveNe_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonMoveNw_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonMoveSe_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonMoveSw_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            SendTextToServer();
        }

        private void ButtonStandSitLie_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonTurnClocwise_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonTurnCounterClockwise_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonUse_Click(object sender, RoutedEventArgs e)
        {
        }

        private void EditBotMenuItem_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
        }

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendTextToServer();
            }
        }

        private void NewBotMenuItem_Click(object sender, RoutedEventArgs e)
        {
        }

        private void OpenBotMeuItem_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                // Set filter for file extension and default file extension
                DefaultExt = ".bini",
                Filter = "Bot Information Files (.bini)|*.bini",
                InitialDirectory = MonkeyCore.Paths.SilverMonkeyBotPath
            };

            // Display OpenFileDialog by calling ShowDialog method
            bool? result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                SessionOptions = new BotOptions(filename);
                FurcadiaSession.Options = SessionOptions;
            }
        }

        private void SendTextToServer()
        {
            if (InputTextBox.Document.Blocks.Count > 0)
            {
                var Txt = InputTextBox.GetText();
                FurcadiaSession.SendFormattedTextToServer(Txt);
                InputTextBox.Document.Blocks.Clear();
                InputTextBox.Focus();
            }
        }

        #endregion Private Methods
    }
}