using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DatabaseLibrary;
using TreeWPF.Annotations;
using TreeWPF.Enums;

namespace TreeWPF.Helpers
{
    public class ConnectionStateData
    {
        #region Fields

        private static ConnectionState _connectionState = ConnectionState.Disconnected;

        private static ConnectionData _connectionData;

        private static string _serverName;
        

        #endregion

        #region Properties

        public static ConnectionData ConnectionData
        {
            get { return _connectionData; }
            set
            {
                _connectionData = value;
                ServerName = value == null ? "" : _connectionData.ServerName;
                OnPropertyChanged("ConnectionData");
                OnConnectionDataChanged();
            }
        }

        public static string ServerName
        {
            get { return _serverName; }
            set
            {
                _serverName = value;
                OnPropertyChanged("ServerName");
                OnConnectionDataChanged();
            }
        }

        public static ConnectionState ConnectionState
        {
            get
            {
                return _connectionState;
            }
            set
            {
                _connectionState = value;
                OnConnectionStateChanged();
                OnPropertyChanged("ConnectionState");
            }
        }

        #endregion

        #region NotifyPropertyChangedEvent

        public static event PropertyChangedEventHandler StaticPropertyChanged;

        private static void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region ConnectionChangedEvent

        public static event EventHandler ConnectionDataChanged;

        private static void OnConnectionDataChanged()
        {
            ConnectionDataChanged?.Invoke(null, EventArgs.Empty);
        }

        public static event EventHandler ConnectionStateChanged;

        private static void OnConnectionStateChanged()
        {
            ConnectionStateChanged?.Invoke(null, EventArgs.Empty);
        }

        #endregion
    }
}