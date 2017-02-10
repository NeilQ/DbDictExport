using System;
using System.Text;
using System.Windows.Forms;
using DbDictExport.Core.Common;
using MetroFramework.Forms;
using Npgsql;

namespace DbDictExport.WinForm.LoginForms
{
    public partial class PostgresLoginForm : MetroForm
    {

        private NpgsqlConnectionStringBuilder _connBuilder;
        public NpgsqlConnectionStringBuilder ConnBuilder
        {
            get { return _connBuilder ?? (_connBuilder = new NpgsqlConnectionStringBuilder()); }
            set { _connBuilder = value; }
        }

        public PostgresLoginForm()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ChangeConnectButtonStatus(false);

            #region Validation
            var message = new StringBuilder();
            var ok = true;

            if (string.IsNullOrEmpty(cmbServer.Text))
            {
                ok = false;
                message.AppendLine(" Server empty.");
            }
            if (string.IsNullOrEmpty(txtPort.Text))
            {
                ok = false;
                message.AppendLine(" Port empty.");
            }
            int port;
            if (!int.TryParse(txtPort.Text, out port))
            {
                ok = false;
                message.AppendLine(" Invalid port, must be integer.");
            }
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                ok = false;
                message.AppendLine("Username empty.");
            }
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                ok = false;
                message.AppendLine("Password empty.");
            }
            if (!ok)
            {
                MessageBox.Show(message.ToString(), Constants.VALIDATE_FAIL_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Information);
                ChangeConnectButtonStatus(true);
                return;
            }
            #endregion

            // Builde connection string
            ConnBuilder.Host = cmbServer.Text;
            ConnBuilder.Port = int.Parse(txtPort.Text);
            ConnBuilder.PersistSecurityInfo = true;
            ConnBuilder.Username = txtUsername.Text;
            ConnBuilder.Password = txtPassword.Text;
            ConnBuilder.ClientEncoding = "utf8";

            // Access database
            if (Connect())
            {
                InitGlobal();
                DialogResult = DialogResult.OK;
            }
        }

        private static void InitGlobal()
        {
            Global.DataBaseType = DataBaseType.Postgresql;
            Global.ProviderName = "Npgsql";
        }

        private bool Connect()
        {
            try
            {
                using (var sqlConn = new NpgsqlConnection(ConnBuilder.ConnectionString))
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
    }
}

