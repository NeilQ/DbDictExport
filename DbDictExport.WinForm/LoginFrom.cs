using System;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace DbDictExport.WinForm
{
    public partial class LoginForm : Form
    {

        private bool isWindowsAuthentication;
        private SqlConnectionStringBuilder connBuilder;
        public SqlConnectionStringBuilder ConnBuilder
        {
            get { return connBuilder ?? (connBuilder = new SqlConnectionStringBuilder()); }
            set { this.connBuilder = value; }
        }

        public LoginForm()
        {
            InitializeComponent();
            this.cmbServerType.SelectedIndex = 0;
            this.cmbServer.SelectedIndex = 0;
            this.cmbAuthentication.SelectedIndex = 0;
            this.cmbAuthentication.SelectedValueChanged += cmbAuthentication_SelectedValueChanged;
        }

        void cmbAuthentication_SelectedValueChanged(object sender, EventArgs e)
        {
            this.isWindowsAuthentication = cmbAuthentication.Text == "Windows Authentication";
            this.txtUsername.Enabled = !this.isWindowsAuthentication;
            this.txtPassword.Enabled = !this.isWindowsAuthentication;
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
                MessageBox.Show(message.ToString(), "Validation failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ChangeConnectButtonStatus(true);
                return;
            }
            #endregion

            // Builde connection string
            this.ConnBuilder.DataSource = this.cmbServer.Text;
            this.ConnBuilder.PersistSecurityInfo = true;
            if (!this.isWindowsAuthentication)
            {
                this.ConnBuilder.UserID = this.txtUsername.Text;
                this.ConnBuilder.Password = this.txtPassword.Text;
            }
            else
            {
                this.ConnBuilder.IntegratedSecurity = true;
            }

            // Access database
            if (Connect())
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private bool Connect()
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this.ConnBuilder.ConnectionString))
                {
                    sqlConn.Open();
                    sqlConn.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                this.ConnBuilder.Clear();
                ChangeConnectButtonStatus(true);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void ChangeConnectButtonStatus(bool allowClick)
        {
            btnConnect.Text = allowClick ? "Connect" : "Connecting";
            btnConnect.Enabled = allowClick;
        }


    }
}

