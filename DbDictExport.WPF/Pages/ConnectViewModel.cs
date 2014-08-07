using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.ComponentModel;
using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows;
using System.Windows;
using System.Windows.Input;
using System.Data.SqlClient;
using FirstFloor.ModernUI.Windows.Controls;

namespace DbDictExport.WPF.Pages
{
    public sealed class ConnectViewModel : NotifyPropertyChanged, IDataErrorInfo
    {
        #region private variables and contants
        private const string SqlServerAuthentication = "Sql Server Authentication";
        private const string WindowsAuthentication = "Windows Authentication";
        private readonly string[] _engines = { "Datebase Engine" };
        private string _selectedEngine;
        private readonly string[] _servers = { ".", "(local)" };
        private string _selectedServer;
        private readonly string[] _authentications = { SqlServerAuthentication, WindowsAuthentication };
        private string _selectedAuthencation;
        private string _username = "sa";
        private string _isUsernameEnabled;
        private string _password;
        private string _isPasswordEnabled;
        private readonly ICommand _connectCommand;
        #endregion


        #region Attributes
        public SqlConnectionStringBuilder ConnStringBuilder { get; set; }

        /// <summary>
        /// Gets the connect command.
        /// </summary>
        /// <value>
        /// The connect command.
        /// </value>
        public ICommand ConnectCommand
        {
            get { return _connectCommand; }
        }

        /// <summary>
        /// Gets the engines.
        /// </summary>
        /// <value>
        /// The engines.
        /// </value>
        public string[] Engines
        {
            get { return _engines; }
        }

        /// <summary>
        /// Gets the servers.
        /// </summary>
        /// <value>
        /// The servers.
        /// </value>
        public string[] Servers
        {
            get { return _servers; }
        }

        /// <summary>
        /// Gets or sets the selected engine.
        /// </summary>
        /// <value>
        /// The selected engine.
        /// </value>
        public string SelectedEngine
        {
            get { return _selectedEngine; }
            set
            {
                if (_selectedEngine != value)
                {
                    _selectedEngine = value;
                    OnPropertyChanged("SelectedEngine");
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected server.
        /// </summary>
        /// <value>
        /// The selected server.
        /// </value>
        public string SelectedServer
        {
            get { return _selectedServer; }
            set
            {
                if (SelectedServer != value)
                {
                    _selectedServer = value;
                    OnPropertyChanged("SelectedServer");
                }
            }
        }

        /// <summary>
        /// Gets the authentications.
        /// </summary>
        /// <value>
        /// The authentications.
        /// </value>
        public string[] Authentications
        {
            get { return _authentications; }
        }

        /// <summary>
        /// Gets or sets the selected authentication.
        /// </summary>
        /// <value>
        /// The selected authentication.
        /// </value>
        public string SelectedAuthentication
        {
            get { return _selectedAuthencation; }
            set
            {
                if (_selectedAuthencation != value)
                {
                    _selectedAuthencation = value;
                    OnPropertyChanged("SelectedAuthentication");
                }
            }
        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged("Username");
                }
            }
        }

        /// <summary>
        /// Gets or sets the is username enable.
        /// </summary>
        /// <value>
        /// The is username enable.
        /// </value>
        public string IsUsernameEnabled
        {
            get { return _isUsernameEnabled; }
            set
            {
                if (_isUsernameEnabled != value)
                {
                    _isUsernameEnabled = value;
                    OnPropertyChanged("IsUsernameEnabled");
                }
            }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged("Password");
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
                    OnPropertyChanged("IsPasswordEnabled");
                }
            }
        }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        public string Error
        {
            get { return null; }
        }


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

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectViewModel"/> class.
        /// </summary>
        public ConnectViewModel()
        {
            SelectedAuthentication = Authentications[0];
            SelectedEngine = Engines[0];
            SelectedServer = Servers[0];

            PropertyChanged += OnPropertyChanged;

            _connectCommand = new RelayCommand(o =>
            {
                StringBuilder message = new StringBuilder();
                bool ok = true;

                // Validate inputs.
                if (String.IsNullOrEmpty(SelectedServer))
                {
                    ok = false;
                    message.AppendLine(" Server empty.");
                }
                if (String.IsNullOrEmpty(SelectedAuthentication))
                {
                    ok = false;
                    message.AppendLine("Authentication empty.");
                }
                if (SelectedAuthentication == SqlServerAuthentication)
                {
                    if (String.IsNullOrEmpty(Username))
                    {
                        ok = false;
                        message.AppendLine("Username empty.");
                    }
                    if (String.IsNullOrEmpty(Password))
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
                if (SelectedAuthentication == WindowsAuthentication)
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
                        var routedCommand =NavigationCommands.GoToPage;
                        routedCommand.Execute("Pages/Home.xaml", AppManagement.Current.AppMainWindow);
                    }
                    catch (Exception ex)
                    {
                        ModernDialog.ShowMessage(ex.Message,"Error",MessageBoxButton.OK);
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
                IsUsernameEnabled = SelectedAuthentication == SqlServerAuthentication
                    ? bool.TrueString
                    : bool.FalseString;
                IsPasswordEnabled = SelectedAuthentication == SqlServerAuthentication
                    ? bool.TrueString
                    : bool.FalseString;
            }
        }


    }
}
