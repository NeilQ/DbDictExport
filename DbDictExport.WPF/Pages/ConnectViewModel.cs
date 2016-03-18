using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.ComponentModel;
using FirstFloor.ModernUI.Presentation;
using System.Windows;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using DbDictExport.WPF.Common;
using FirstFloor.ModernUI.Windows.Controls;

namespace DbDictExport.WPF.Pages
{
    public sealed class ConnectViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region private variables and contants
        private const string SQL_SERVER_AUTHENTICATION = "Sql Server Authentication";
        private const string WINDOWS_AUTHENTICATION = "Windows Authentication";
        private string _selectedEngine;
        private ObservableCollection<string> _servers;
        private string _selectedServer;
        private string _selectedAuthencation;
        private string _username = "sa";
        private string _isUsernameEnabled;
        private string _password;
        private string _isPasswordEnabled;

        private List<SqlServerAuth> _auths = AuthHistoryService.GetHistories() as List<SqlServerAuth>;
        #endregion


        #region Attributes
        public SqlConnectionStringBuilder ConnStringBuilder { get; set; }

        public ICommand ConnectCommand { get; }

        public string[] Engines { get; } = { "Datebase Engine" };

        public ObservableCollection<string> Servers
        {
            get { return _servers ?? (_servers = new ObservableCollection<string>()); }
            set
            {
                _servers = value;
                RaisePropertyChanged("Servers");
            }
        }

        public string SelectedEngine
        {
            get { return _selectedEngine; }
            set
            {
                if (_selectedEngine != value)
                {
                    _selectedEngine = value;
                    RaisePropertyChanged("SelectedEngine");
                }
            }
        }

        public string SelectedServer
        {
            get { return _selectedServer; }
            set
            {
                if (SelectedServer != value)
                {
                    _selectedServer = value;
                    RaisePropertyChanged("SelectedServer");
                }
            }
        }

        public string NewServer
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (!Servers.Contains(value))
                    {
                        Servers.Add(value);
                    }
                    SelectedServer = value;
                }
            }
            get
            {
                return SelectedServer;
            }
        }

        public string[] Authentications { get; } = { SQL_SERVER_AUTHENTICATION, WINDOWS_AUTHENTICATION };

        public string SelectedAuthentication
        {
            get { return _selectedAuthencation; }
            set
            {
                if (_selectedAuthencation != value)
                {
                    _selectedAuthencation = value;
                    RaisePropertyChanged("SelectedAuthentication");
                }
            }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    RaisePropertyChanged("Username");
                }
            }
        }

        public string IsUsernameEnabled
        {
            get { return _isUsernameEnabled; }
            set
            {
                if (_isUsernameEnabled != value)
                {
                    _isUsernameEnabled = value;
                    RaisePropertyChanged("IsUsernameEnabled");
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    RaisePropertyChanged("Password");
                }
            }
        }

        /// <summary>
        /// Gets or sets the is password enable.
        /// </summary>
        /// <value>
        /// The is password enable.
        /// </value>
        public string IsPasswordEnabled
        {
            get { return _isPasswordEnabled; }
            set
            {
                if (_isPasswordEnabled != value)
                {
                    _isPasswordEnabled = value;
                    RaisePropertyChanged("IsPasswordEnabled");
                }
            }
        }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        public string Error => null;


        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get
            {
                if (columnName == "Username")
                {
                    return string.IsNullOrEmpty(Username) ? "Required value" : null;
                }
                if (columnName == "Password")
                {
                    return string.IsNullOrEmpty(Password) ? "Required value" : null;
                }
                return null;
            }
        }

        #endregion

        public static ConnectViewModel Current = new ConnectViewModel();

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectViewModel"/> class.
        /// </summary>
        public ConnectViewModel()
        {
            SelectedAuthentication = Authentications[0];

            foreach (var sqlServerAuth in _auths)
            {
                if (sqlServerAuth == null) continue;
                Servers.Add(sqlServerAuth.Server);
            }

            SelectedEngine = Engines[0];
            SelectedServer = Servers[0];


            PropertyChanged += OnPropertyChanged;

            ConnectCommand = new RelayCommand(o =>
            {
                var message = new StringBuilder();
                var ok = true;

                // Validate inputs.
                if (string.IsNullOrEmpty(SelectedServer))
                {
                    ok = false;
                    message.AppendLine("Server empty.");
                }
                if (string.IsNullOrEmpty(SelectedAuthentication))
                {
                    ok = false;
                    message.AppendLine("Authentication empty.");
                }
                if (SelectedAuthentication == SQL_SERVER_AUTHENTICATION)
                {
                    if (string.IsNullOrEmpty(Username))
                    {
                        ok = false;
                        message.AppendLine("Username empty.");
                    }
                    if (string.IsNullOrEmpty(Password))
                    {
                        ok = false;
                        message.AppendLine("Password empty.");
                    }
                }
                if (!ok)
                {
                    ModernDialog.ShowMessage(message.ToString(), "Error", MessageBoxButton.OK);
                    return;
                }


                ConnStringBuilder = new SqlConnectionStringBuilder { DataSource = SelectedServer };
                if (SelectedAuthentication == WINDOWS_AUTHENTICATION)
                {
                    ConnStringBuilder.IntegratedSecurity = true;
                }
                else
                {
                    ConnStringBuilder.UserID = Username;
                    ConnStringBuilder.Password = Password;
                }

                // Try connecting the database.
                using (var conn = new SqlConnection(ConnStringBuilder.ConnectionString))
                {
                    try
                    {
                        conn.Open();
                        OnConnectSucceed();

                    }
                    catch (Exception ex)
                    {
                        ModernDialog.ShowMessage(ex.Message, "Error", MessageBoxButton.OK);
                    }
                }
            });
        }

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedAuthentication")
            {
                IsUsernameEnabled = SelectedAuthentication == SQL_SERVER_AUTHENTICATION
                    ? bool.TrueString
                    : bool.FalseString;
                IsPasswordEnabled = SelectedAuthentication == SQL_SERVER_AUTHENTICATION
                    ? bool.TrueString
                    : bool.FalseString;
            }
            if (e.PropertyName == "SelectedServer")
            {
                var auth = AuthHistoryService.GetHistory(SelectedServer);
                if (auth != null)
                {
                    Username = auth.Username ?? "";
                    Password = auth.Password ?? "";
                }
                else
                {
                    Username = "";
                    Password = "";
                }
            }
        }


        private void RaisePropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            var handler = PropertyChanged;

            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnConnectSucceed()
        {
            AuthHistoryService.PersistHistory(new SqlServerAuth()
            {
                Server = SelectedServer,
                AuthType =
                    SelectedAuthentication == SQL_SERVER_AUTHENTICATION
                        ? SqlServerAuthType.SqlServer
                        : SqlServerAuthType.Windows,
                Username = Username,
                Password = Password
            });

            var routedCommand = NavigationCommands.GoToPage;
            routedCommand.Execute("Pages/Home.xaml", AppManagement.Current.AppMainWindow);
        }
    }
}
