using System;
using System.Windows;
using System.Windows.Input;
using TreeWPF.Commands;
using TreeWPF.Enums;
using TreeWPF.Helpers;
using TreeWPF.Views;

namespace TreeWPF.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private ConnectToServerViewModel _connectToServerViewModel = null;

        private string _connectHeader = "Connect";
        #endregion

        #region Properties

        public ICommand CloseWindowCommand { get; set; }

        public ICommand ConnectCommand
        {
            get;
            private set;
        }

        public ActionCommand DisconnectCommand { get; set; }

        public string ConnectHeader
        {
            get { return _connectHeader; }
            set
            {
                _connectHeader = value;
                OnPropertyChanged(nameof(ConnectHeader));
            }
        }

        #endregion

        #region Ctor

        public MainWindowViewModel()
        {
            ConnectCommand = new ActionCommand(OpenConnectWindow);
            CloseWindowCommand = new Commands.ActionCommand(window => ((Window)window).Close());//не уверен, что это хорошо
            _connectToServerViewModel = new ConnectToServerViewModel();
            DisconnectCommand = new ActionCommand(DisconnectAction, DisconnectCanExecute);
            ConnectionStateData.ConnectionStateChanged +=ConnectionStateDataOnConnectionStateChanged;
            ConnectCommand.Execute(null);
        }

        private void ConnectionStateDataOnConnectionStateChanged(object sender, EventArgs eventArgs)
        {
            if (ConnectionStateData.ConnectionState == ConnectionState.Disconnected)
                ConnectHeader = "Connect";
            else
                ConnectHeader = "Reconnect";
        }

        #endregion
        
        private void DisconnectAction(object parameter)
        {
            ConnectionStateData.ConnectionState = ConnectionState.Disconnected;
            ConnectionStateData.ConnectionData = null;
        }

        private bool DisconnectCanExecute(object parameter)
        {
            if (_connectToServerViewModel == null)
                return false;
            if (ConnectionStateData.ConnectionState == ConnectionState.Disconnected)
                return false;
            return true;
        }

        public void OpenConnectWindow(object parameter)
        {
            ConnectToServerView connectToServerView = new ConnectToServerView(_connectToServerViewModel);
            connectToServerView.ShowDialog();
        }

       
    }
}