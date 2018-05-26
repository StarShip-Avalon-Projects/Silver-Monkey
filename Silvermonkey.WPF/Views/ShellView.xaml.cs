using Engine.BotSession;
using Furcadia.Net;
using Furcadia.Net.DreamInfo;
using Furcadia.Net.Utils.ServerParser;
using MonkeyCore.Logging;
using SilverMonkey.Extentions;
using SilverMonkey.Logging;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using FurcLog = Furcadia.Logging;
using MsLog = Monkeyspeak.Logging;

namespace SilverMonkey.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        #region Public Constructors

        /// <summary>
        /// The session options
        /// </summary>
        public BotOptions SessionOptions = new BotOptions();

        private static Bot furcadiaSession;

        /// <summary>
        /// The furcadia session
        /// </summary>
        public Bot FurcadiaSession { get => furcadiaSession; set => furcadiaSession = value; }

        /// <summary>
        /// Gets or sets the selected furre.
        /// </summary>
        /// <value>
        /// The selected furre.
        /// </value>
        public Furre SelectedFurre { get; set; } = new Furre();

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellView"/> class.
        /// </summary>
        public ShellView()
        {
            InitializeComponent();
            SessionOptions = new BotOptions();
            FurcadiaSession = new Bot(SessionOptions);
            FurcadiaSession.ProcessServerChannelData += OnProcessServerChannelData;
            FurcadiaSession.ProcessServerInstruction += OnProcessServerInstruction;
            FurcadiaSession.ServerStatusChanged += OnServerStatusChanged;
            FurcadiaSession.Error += OnError;
            Furres.ItemsSource = FurcadiaSession.Furres;
            MsLog.Logger.InfoEnabled = true;
            MsLog.Logger.ErrorEnabled = true;
            FurcLog.Logger.InfoEnabled = true;
            FurcLog.Logger.ErrorEnabled = true;
            Logger.InfoEnabled = true;
            Logger.ErrorEnabled = true;
            Logger.LogOutput = new MultiLogOutput(
                new ConsoleWindowLogOutput(LogOutputBox, Level.Error),
                new ConsoleWindowLogOutput(LogOutputBox, Level.Info),
                new ConsoleWindowLogOutput(LogOutputBox, Level.Warning)
                );
            FurcLog.Logger.LogOutput = new MultiLogOutput(
                new ConsoleWindowLogOutput(LogOutputBox, FurcLog.Level.Error.ToLevel()),
                new ConsoleWindowLogOutput(LogOutputBox, FurcLog.Level.Info.ToLevel()),
                new ConsoleWindowLogOutput(LogOutputBox, FurcLog.Level.Warning.ToLevel())
                );
            MsLog.Logger.LogOutput = new MultiLogOutput(
                new ConsoleWindowLogOutput(LogOutputBox, MsLog.Level.Error.ToLevel()),
                new ConsoleWindowLogOutput(LogOutputBox, MsLog.Level.Info.ToLevel()),
                new ConsoleWindowLogOutput(LogOutputBox, MsLog.Level.Warning.ToLevel())
                );
        }

        #endregion Public Constructors

        #region Private Methods

        private void ButtonGetDrop_Click(object sender, RoutedEventArgs e)
        {
        }

        private async void ButtonGo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ButtonGo.IsEnabled = false;
                if (FurcadiaSession.IsServerSocketConnected)
                {
                    FurcadiaSession.Disconnect();
                    ButtonGo.Background = Brushes.LightGray;
                }
                else
                {
                    await FurcadiaSession.ConnetAsync();
                }
            }
            catch (Exception crap)
            {
                crap.Log();
            }
            finally
            {
                if (FurcadiaSession.IsServerSocketConnected)
                {
                    ButtonGo.Content = "Stop!";
                    ButtonGo.Background = Brushes.Green;
                }
                else
                {
                    ButtonGo.Content = "Go!";
                    ButtonGo.Background = Brushes.LightGray;
                }

                ButtonGo.IsEnabled = true;
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
                InitialDirectory = IO.Paths.SilverMonkeyBotPath
            };

            // Display OpenFileDialog by calling ShowDialog method
            bool? result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                SessionOptions = new BotOptions(filename);
                FurcadiaSession.SetOptions(SessionOptions);
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

        private void UpdateFurreList()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                    (Action)(() =>
                    {
                        Furres.ItemsSource = FurcadiaSession.Furres;
                        Furres.Items.Refresh();
                        Furres.UpdateLayout();
                        FurreCount.Text = $"Total Furres: {FurcadiaSession.Furres.Count}";
                    }));
            }
            else
            {
                Furres.ItemsSource = FurcadiaSession.Furres;
                Furres.Items.Refresh();
                Furres.UpdateLayout();
                FurreCount.Text = $"Total Furres: {FurcadiaSession.Furres.Count}";
            }
        }

        #endregion Private Methods

        private void OnError(Exception e, object o)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                    (Action)(() =>
                    {
                        LogOutputBox.AppendParagraph($"{e} {o}");
                    }));
            }
        }

        private void OnServerStatusChanged(object Sender, NetServerEventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                     (Action)(() =>
                     {
                         switch (e.ConnectPhase)
                         {
                             case ConnectionPhase.Disconnected:
                                 ButtonGo.Background = Brushes.Red;
                                 ButtonGo.Content = "Disconnected!";

                                 UpdateFurreList();
                                 break;

                             case ConnectionPhase.Connecting:
                                 ButtonGo.Background = Brushes.Orange;
                                 ButtonGo.Content = "Connecting ...";

                                 break;

                             case ConnectionPhase.Connected:
                                 ButtonGo.Background = Brushes.LightGreen;
                                 ButtonGo.Content = "Connected!!!";

                                 break;
                         }
                     }));
            }
        }

        private void OnProcessServerChannelData(object sender, ParseChannelArgs Args)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                    (Action)(() =>
                    {
                        if (sender is ChannelObject InstructionObject)
                        {
                            LogOutputBox.AppendParagraph(InstructionObject.ChannelText);
                        }
                    }));
            }
            else if (sender is ChannelObject InstructionObject)
            {
                LogOutputBox.AppendParagraph(InstructionObject.ChannelText);
            }
        }

        private void OnProcessServerInstruction(object sender, ParseServerArgs Args)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                     (Action)(() =>
                     {
                         if (sender is BaseServerInstruction instructionObject)
                         {
                             if (furcadiaSession.ServerStatus == ConnectionPhase.MOTD)
                                 LogOutputBox.AppendParagraph(instructionObject.RawInstruction);
                             switch (instructionObject.InstructionType)
                             {
                                 case ServerInstructionType.LoadDreamEvent:
                                 case ServerInstructionType.SpawnAvatar:
                                 case ServerInstructionType.RemoveAvatar:
                                     UpdateFurreList();
                                     break;
                             }
                             switch (instructionObject.InstructionType)
                             {
                                 case ServerInstructionType.BookmarkDream:
                                 case ServerInstructionType.LoadDreamEvent:
                                     DreamOwner.Text = FurcadiaSession.Dream.DreamOwner;
                                     DreamTitle.Text = FurcadiaSession.Dream.Title;
                                     DreamRating.Text = FurcadiaSession.Dream.Rating;
                                     // DreamURL.NavigateUri = new Uri(FurcadiaSession.Dream.DreamUrl);
                                     break;
                             }
                         }
                     }));
            }
            else if (sender is BaseServerInstruction instructionObject)
            {
                if (furcadiaSession.ServerStatus == ConnectionPhase.MOTD)
                    LogOutputBox.AppendParagraph(instructionObject.RawInstruction);
                switch (instructionObject.InstructionType)
                {
                    case ServerInstructionType.LoadDreamEvent:
                    case ServerInstructionType.SpawnAvatar:
                    case ServerInstructionType.RemoveAvatar:
                        UpdateFurreList();
                        break;
                }
                switch (instructionObject.InstructionType)
                {
                    case ServerInstructionType.BookmarkDream:
                    case ServerInstructionType.LoadDreamEvent:
                        DreamOwner.Text = FurcadiaSession.Dream.DreamOwner;
                        DreamTitle.Text = FurcadiaSession.Dream.Title;
                        DreamRating.Text = FurcadiaSession.Dream.Rating;
                        // DreamURL.NavigateUri = new Uri(FurcadiaSession.Dream.DreamUrl);
                        break;
                }
            }
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
        }

        private void MsEditor_Click(object sender, RoutedEventArgs e)
        {
            var msProcassStarInfo = new ProcessStartInfo
            {
                Arguments = SessionOptions.MonkeySpeakEngineOptions.MonkeySpeakScriptFile,
                WorkingDirectory = IO.Paths.ApplicationPath,
                FileName = "Monkeyspeak Editor.exe"
            };
            var MsProcess = new Process
            {
                StartInfo = msProcassStarInfo
            };
            MsProcess.Start();
        }

        private void DataMonkey_Click(object sender, RoutedEventArgs e)
        {
            var msProcassStarInfo = new ProcessStartInfo
            {
                WorkingDirectory = IO.Paths.ApplicationPath,
                FileName = "Data Monkey.exe"
            };
            var MsProcess = new Process
            {
                StartInfo = msProcassStarInfo
            };
            MsProcess.Start();
        }
    }
}