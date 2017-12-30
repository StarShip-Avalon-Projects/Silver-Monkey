using System.Windows;
using System.Windows.Input;

namespace SilverMonkey.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        #region Public Constructors

        public ShellView()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void ButtonGetDrop_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonGo_Click(object sender, RoutedEventArgs e)
        {
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
        }

        private void SendTextToServer()
        {
            if (InputTextBox.Document.Blocks.Count > 0)
            {
                var text = InputTextBox.GetText();
                LogOutputBox.AppendParagraph(text);
                InputTextBox.Focus();
            }
        }

        #endregion Private Methods
    }
}