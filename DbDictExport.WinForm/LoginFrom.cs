using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using Aspose.Cells;

namespace DbDictExport.WinForm
{
    public partial class LoginForm : Form
    {

        public bool IsWindowsAuthentication { get; set; }
        //private Thread connectThread;
        public bool IsLogin { get; set; }
        private SqlConnectionStringBuilder connBuilder;
        public SqlConnectionStringBuilder ConnBuilder
        {
            get
            {
                if (connBuilder == null)
                {
                    connBuilder = new SqlConnectionStringBuilder();
                }
                return connBuilder;
            }
            set { this.connBuilder = value; }
        }

        public LoginForm()
        {
            LoginForm_Init();
        }

        private void LoginForm_Init()
        {
            InitializeComponent();
            Button.CheckForIllegalCrossThreadCalls = false;
            this.IsWindowsAuthentication = false;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.cmbServerType.SelectedIndex = 0;
            this.cmbServer.SelectedIndex = 0;
            this.cmbAuthentication.SelectedIndex = 0;
            this.cmbAuthentication.SelectedValueChanged += cmbAuthentication_SelectedValueChanged;
        }

        void cmbAuthentication_SelectedValueChanged(object sender, EventArgs e)
        {
            this.IsWindowsAuthentication = cmbAuthentication.Text == "Windows Authentication";
            this.txtUsername.Enabled = !this.IsWindowsAuthentication;
            this.txtPassword.Enabled = !this.IsWindowsAuthentication;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ChangeConnectButtonStatus(false);

            #region Validation
            StringBuilder message = new StringBuilder();
            bool Ok = true;

            if (String.IsNullOrEmpty(cmbServer.Text))
            {
                Ok = false;
                message.AppendLine(" Server empty.");
            }
            if (String.IsNullOrEmpty(cmbAuthentication.Text))
            {
                Ok = false;
                message.AppendLine("Authentication empty.");
            }
            if (!IsWindowsAuthentication)
            {
                if (String.IsNullOrEmpty(txtUsername.Text))
                {
                    Ok = false;
                    message.AppendLine("Username empty.");
                }
                if (String.IsNullOrEmpty(txtPassword.Text))
                {
                    Ok = false;
                    message.AppendLine("Password empty.");
                }
            }
            if (!Ok)
            {
                MessageBox.Show(message.ToString(), "Validation failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ChangeConnectButtonStatus(true);
                return;
            }
            #endregion

            //builde connection string
            this.ConnBuilder.DataSource = this.cmbServer.Text;
            this.ConnBuilder.PersistSecurityInfo = true;
            if (!this.IsWindowsAuthentication)
            {
                this.ConnBuilder.UserID = this.txtUsername.Text;
                this.ConnBuilder.Password = this.txtPassword.Text;
            }
            else
            {
                this.ConnBuilder.IntegratedSecurity = true;
            }

            //access database
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
                    this.IsLogin = true;
                }
            }
            catch (Exception ex)
            {
                this.ConnBuilder.Clear();
                ChangeConnectButtonStatus(true);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.IsLogin = false;
            }
            return IsLogin;
        }

        public void ChangeConnectButtonStatus(bool allowClick)
        {
            btnConnect.Text = allowClick ? "Connect" : "Connecting";
            btnConnect.Enabled = allowClick;
        }


    }
}

