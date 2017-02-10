using System;
using System.Text;
using System.Windows.Forms;
using DbDictExport.Core.Common;
using MetroFramework.Forms;
using MySql.Data.MySqlClient;

namespace DbDictExport.WinForm.LoginForms
{
    public partial class MysqlLoginForm : MetroForm
    {

        private MySqlConnectionStringBuilder connBuilder;
        public MySqlConnectionStringBuilder ConnBuilder
        {
            get { return connBuilder ?? (connBuilder = new MySqlConnectionStringBuilder()); }
            set { connBuilder = value; }
        }

        public MysqlLoginForm()
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
            ConnBuilder.Server = cmbServer.Text;
            ConnBuilder.PersistSecurityInfo = true;
            ConnBuilder.UserID = txtUsername.Text;
            ConnBuilder.Password = txtPassword.Text;
            ConnBuilder.CharacterSet = "utf8";

            // Access database
            if (Connect())
            {
                InitGlobal();
                DialogResult = DialogResult.OK;
            }
        }

        private static void InitGlobal()
        {
            Global.DataBaseType = DataBaseType.Mysql;
            Global.ProviderName = "MySql.Data.MySqlClient";
        }

        private bool Connect()
        {
            try
            {
                using (var sqlConn = new MySqlConnection(ConnBuilder.ConnectionString))
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

