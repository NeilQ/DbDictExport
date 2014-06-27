using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using FirstFloor.ModernUI.Presentation;

namespace DbDictExport.WPF.Pages
{
    public sealed class ConnectViewModel : NotifyPropertyChanged, IDataErrorInfo
    {
        private const string SqlServerAuthentication = "Sql Server Authentication";
        private const string WindowsAuthentication = "Windows Authentication";
        private readonly string[] _engines = { "Datebase Engine" };
        private string _selectedEngine;
        private string _server = ".";
        private readonly string[] _authentications = { SqlServerAuthentication, WindowsAuthentication };
        private string _selectedAuthencation;
        private string _username = "sa";
        private string _isUsernameEnable;
        private string _password;
        private string _isPasswordEnable;

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
        /// Gets or sets the server.
        /// </summary>
        /// <value>
        /// The server.
        /// </value>
        public string Server
        {
            get { return _server; }
            set
            {
                if (_server != value)
                {
                    _server = value;
                    OnPropertyChanged("Server");
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
                    OnPropertyChanged("SelectedAuthencation");
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
        public string IsUsernameEnable
        {
            get { return _isUsernameEnable; }
            set
            {
                if (_isUsernameEnable != value)
                {
                    _isUsernameEnable = value;
                    OnPropertyChanged("IsUsernameEnable");
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
        public string IsPasswordEnable
        {
            get { return _isPasswordEnable; }
            set
            {
                if (_isPasswordEnable != value)
                {
                    _isPasswordEnable = value;
                    OnPropertyChanged("IsUsernameEnable");
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
        /// <exception cref="System.NotImplementedException"></exception>
        public string this[string columnName]
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectViewModel"/> class.
        /// </summary>
        public ConnectViewModel()
        {
            SelectedAuthentication = Authentications[0];
            SelectedEngine = Engines[0];

            PropertyChanged += OnPropertyChanged;
        }

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedAuthencation")
            {
                IsUsernameEnable = SelectedAuthentication == SqlServerAuthentication ? bool.TrueString : bool.FalseString;
            }
        }

    }
}
