using System;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using DbDictExport.Common;
using MetroFramework.Forms;
using System.Configuration;

namespace DbDictExport.WinForm
{
    public partial class LoginForm : MetroForm
    {

        private bool isWindowsAuthentication;
        private SqlConnectionStringBuilder connBuilder;
        public SqlConnectionStringBuilder ConnBuilder
        {
            get { return connBuilder ?? (connBuilder = new SqlConnectionStringBuilder()); }
            set { connBuilder = value; }
        }

        public LoginForm()
        {
            InitializeComponent();
            LoadConnectHistiry();
            cmbAuthentication.SelectedValueChanged += cmbAuthentication_SelectedValueChanged;
        }

        void cmbAuthentication_SelectedValueChanged(object sender, EventArgs e)
        {
            isWindowsAuthentication = cmbAuthentication.Text == Constants.WINDOWS_AUTHENTICATION_TEXT;
            txtUsername.Enabled = !isWindowsAuthentication;
            txtPassword.Enabled = !isWindowsAuthentication;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ChangeConnectButtonStatus(false);

            #region Validation
            StringBuilder message = new StringBuilder();
            bool ok = true;

            if (String.IsNullOrEmpty(cmbServer.Text))
            {
                ok = false;
                message.AppendLine(" Server empty.");
            }
            if (String.IsNullOrEmpty(cmbAuthentication.Text))
            {
                ok = false;
                message.AppendLine("Authentication empty.");
            }
            if (!isWindowsAuthentication)
            {
                if (String.IsNullOrEmpty(txtUsername.Text))
                {
                    ok = false;
                    message.AppendLine("Username empty.");
                }
                if (String.IsNullOrEmpty(txtPassword.Text))
                {
                    ok = false;
                    message.AppendLine("Password empty.");
                }
            }
            if (!ok)
            {
                MessageBox.Show(message.ToString(), Constants.VALIDATE_FAIL_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Information);
                ChangeConnectButtonStatus(true);
                return;
            }
            #endregion

            // Builde connection string
            ConnBuilder.DataSource = cmbServer.Text;
            ConnBuilder.PersistSecurityInfo = true;
            if (!isWindowsAuthentication)
            {
                ConnBuilder.UserID = txtUsername.Text;
                ConnBuilder.Password = txtPassword.Text;
            }
            else
            {
                ConnBuilder.IntegratedSecurity = true;
            }

            // Access database
            if (Connect())
            {
                RefreshConnectHistory();
                DialogResult = DialogResult.OK;
            }
        }

        private bool Connect()
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConnBuilder.ConnectionString))
                {
                    sqlConn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                ConnBuilder.Clear();
                ChangeConnectButtonStatus(true);
                MessageBox.Show(ex.Message, Constants.ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void ChangeConnectButtonStatus(bool allowClick)
        {
            btnConnect.Text = allowClick ? "Connect" : "Connecting";
            btnConnect.Enabled = allowClick;
        }

        private void LoadConnectHistiry()
        {
            var lastConnectServer = ConfigurationManager.AppSettings["lastConnectServer"];
            var lastConnectUser = ConfigurationManager.AppSettings["lastConnectUser"];
            var lastConnectPassword = ConfigurationManager.AppSettings["lastConnectPassword"];
            var lastConnectAuth = ConfigurationManager.AppSettings["lastConnectAuthentication"];

            if (!String.IsNullOrEmpty(lastConnectServer))
            {
                cmbServer.Text = lastConnectServer;
                cmbServer.Items.Add(lastConnectServer);
            }
            else
            {
                cmbServer.SelectedIndex = 0;
            }
            if (!String.IsNullOrEmpty(lastConnectUser))
            {
                txtUsername.Text = lastConnectUser;
            }
            if (!String.IsNullOrEmpty(lastConnectPassword))
            {
                txtPassword.Text = lastConnectPassword;
            }
            if (!String.IsNullOrEmpty(lastConnectAuth))
            {
                cmbAuthentication.Text = lastConnectAuth;
            }
            else
            {
                cmbAuthentication.SelectedIndex = 0;
            }


            cmbServerType.SelectedIndex = 0;
        }

        private void RefreshConnectHistory()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["lastConnectServer"].Value = cmbServer.Text;
            config.AppSettings.Settings["lastConnectUser"].Value = txtUsername.Text;
            config.AppSettings.Settings["lastConnectPassword"].Value = txtPassword.Text;
            config.AppSettings.Settings["lastConnectAuthentication"].Value = cmbAuthentication.Text;
            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}

