using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using DatabaseLibrary;
using TreeWPF.Commands;
using TreeWPF.Enums;
using TreeWPF.Helpers;
using TreeWPF.Interfaces;

namespace TreeWPF.ViewModels
{
    public class ConnectToServerViewModel : ViewModelBase, IClosableViewModel, IDataErrorInfo
    {
        #region Fields

        private ConnectionState _connectionState;

        private bool _connectIsEnabled;

        private bool _serversIsEnabled;

        private string _serverText;

        private string WindowsUserName { get; }

        private string _sqlServerLogin = "";

        private string _login;

        private SecureString _password;

        private bool _loginPasswordIsEnabled;

        private AuthType _authType;

        #endregion

        #region Events

        public event EventHandler Close;

        #endregion

        #region Ctor

        public ConnectToServerViewModel()
        {
            #region Commands

            ConnectIsEnabled = true;
            ConnectCommand = new ActionCommand(OnConnectCommand, ConnectCanExecute);
            CancelCommand = new ActionCommand(OnCancelCommand);

            #endregion

            ServersList = new ObservableCollection<string>();
            ServerText = "Loading...";
            Task networkServersLoader = Task.Run(() => ServersLoader.LoadServers(ServersList));
            networkServersLoader.ContinueWith(task =>
            {
                ServersIsEnabled = true;//
                ServerText = "";
            });
            WindowsUserName = $"{Environment.UserDomainName}\\{Environment.UserName}";
            Login = WindowsUserName;
        }

        #endregion

        #region Properties

        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        public SecureString Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public AuthType AuthType
        {
            get { return _authType; }
            set
            {
                if (value == AuthType.Windows)
                {
                    LoginPasswordIsEnabled = false;
                    _sqlServerLogin = Login;
                    _authType = value;
                    Login = WindowsUserName;

                }
                if (value == AuthType.SqlServer)
                {
                    LoginPasswordIsEnabled = true;
                    Login = _sqlServerLogin;
                    _authType = value;
                }
                OnPropertyChanged(nameof(AuthType));
            }
        }

        public bool LoginPasswordIsEnabled
        {
            get { return _loginPasswordIsEnabled; }
            set
            {
                _loginPasswordIsEnabled = value;
                OnPropertyChanged(nameof(LoginPasswordIsEnabled));
            }
        }

        public bool ServersIsEnabled
        {
            get { return _serversIsEnabled; }
            set
            {
                _serversIsEnabled = value;
                OnPropertyChanged(nameof(ServersIsEnabled));
            }
        }

        public string ServerText
        {
            get { return _serverText; }
            set
            {
                _serverText = value;
                OnPropertyChanged(nameof(ServerText));
            }
        }

        public ObservableCollection<string> ServersList { get; set; }

        public ICommand ConnectCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public bool ConnectIsEnabled
        {
            get { return _connectIsEnabled; }
            set
            {
                _connectIsEnabled = value;
                OnPropertyChanged(nameof(ConnectIsEnabled));
            }
        }

        public CancellationTokenSource TokenSource { get; set; }
        #endregion

        /// <summary>
        /// Задание connectionString и ожидание результата подключения по этой строке подключения
        /// </summary>
        /// <param name="testConnectionData"></param>
        /// <returns></returns>
        private bool TryConnect(ConnectionData testConnectionData)
        {
            try
            {
                switch (AuthType)
                {
                    case AuthType.Windows:
                        {
                            testConnectionData.CreateConnectionString(ServerText);
                            break;
                        }
                    case AuthType.SqlServer:
                        {
                            testConnectionData.CreateConnectionString(ServerText, Login, SecureStringConverter.ConvertToString(Password));
                            break;
                        }
                    default:
                        throw new ArgumentOutOfRangeException($"Invalid Authentification type");
                }
            }
            catch (Exception e)
            {
                MessageDialog.ShowMessage(e.Message);
                return false;
            }
            var result = ConnectToTheServer(testConnectionData);
            result.Wait();
            return result.Result;
        }

        /// <summary>
        /// подключения к серверу 
        /// </summary>
        /// <param name="connectionData"></param>
        /// <returns></returns>
        private Task<bool> ConnectToTheServer(ConnectionData connectionData)
        {
            return Task.Run(() => connectionData.CheckServerConnectionAsync(TokenSource));
        }

        /// <summary>
        /// Execute для кнопки Connect
        /// </summary>
        /// <param name="obj"></param>
        private void OnConnectCommand(object obj)
        {
            TokenSource = new CancellationTokenSource();
            _connectionState = ConnectionStateData.ConnectionState;
            ConnectionStateData.ConnectionState = ConnectionState.Connecting;
            ConnectionData connectionData = new ConnectionData();
            var task = new Task<bool>(() => TryConnect(connectionData));
            task.Start();
            task.ContinueWith(t =>
            {
                if (task.Result)
                {
                    _connectionState = ConnectionStateData.ConnectionState = ConnectionState.Connected;
                    ConnectionStateData.ConnectionData = connectionData;
                    OnCancelCommand(null);
                }
                else
                {
                    ConnectionStateData.ConnectionState = _connectionState;
                    MessageDialog.ShowMessage("Error while connecting to the server");
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        /// <summary>
        /// CanExecute для кнопки Connect
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool ConnectCanExecute(object obj)
        {
            if (LoginPasswordIsEnabled && string.IsNullOrWhiteSpace(Login))
                return false;
            if (ConnectionStateData.ConnectionState == ConnectionState.Connecting
                || string.IsNullOrWhiteSpace(ServerText))
                return false;
            return true;
        }

        /// <summary>
        /// Execute для кнопки Cancel
        /// </summary>
        /// <param name="parameter"></param>
        protected virtual void OnCancelCommand(object parameter)
        {
            if (ConnectionStateData.ConnectionState == ConnectionState.Connecting)
                TokenSource.Cancel();
            else
            {
                ServerText = ConnectionStateData.ConnectionData?.ServerName;
                var handler = Close;
                handler?.Invoke(this, EventArgs.Empty);
            }
        }

        #region Data validation

        public string this[string columnName]
        {
            get { return OnValidate(columnName); }
        }

        private string OnValidate(string propertyName)
        {
            string error = string.Empty;
            switch (propertyName)
            {
                case nameof(ServerText):
                    if (string.IsNullOrWhiteSpace(ServerText))
                        error = "Server name is incorrect";
                    break;
                case nameof(Login):
                    if (AuthType == AuthType.SqlServer && string.IsNullOrWhiteSpace(Login))
                        error = "Login is not correct";
                    break;
            }
            return error;
        }

        public string Error { get; }

        #endregion
    }
}