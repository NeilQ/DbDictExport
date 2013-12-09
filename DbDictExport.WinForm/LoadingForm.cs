using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DbDictExport.WinForm
{
    public partial class LoadingForm : Form
    {
        public LoadingForm()
        {
            InitializeComponent();
        }

        private delegate void SetTextHandle(string text);
        public void SetText(string text)
        {
            if (lblMessage.InvokeRequired)
            {
                this.Invoke(new SetTextHandle(SetText), text);
            }
            else
            {
                this.lblMessage.Text = text;
            }
        }
    }
}
