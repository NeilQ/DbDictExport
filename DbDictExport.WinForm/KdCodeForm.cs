using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DbDictExport.Core;
using DbDictExport.Core.Codes;
using MetroFramework.Forms;

namespace DbDictExport.WinForm
{
    public partial class KdCodeForm : MetroForm
    {
        public DbTable Table { get; set; }

        public KdCodeForm()
        {
            InitializeComponent();
        }

        public KdCodeForm(DbTable table)
        {
            InitializeComponent();
            Table = table;
            lblTableName.Text = table.Name;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEntityName.Text))
            {
                txtEntityName.Focus();
                txtModuleName.BackColor = Color.DarkSalmon;
                return;
            }
            if (string.IsNullOrEmpty(txtModuleName.Text))
            {
                txtModuleName.Focus();
                txtModuleName.BackColor = Color.DarkSalmon;
                return;
            }
            var folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;

            var idal = new DalInterfaceKdCodeFactory(txtEntityName.Text, txtModuleName.Text, Table);
            var dal = new DalKdCodeFactory(txtEntityName.Text, txtModuleName.Text, Table);
            var model = new ModelKdCodeFactory(txtEntityName.Text, txtModuleName.Text, Table);

            var idalPath = folderBrowserDialog.SelectedPath + $"\\I{txtEntityName.Text}Manager.cs";
            var dalPath = folderBrowserDialog.SelectedPath + $"\\{txtEntityName.Text}Manager.cs";
            var modelPath = folderBrowserDialog.SelectedPath + $"\\{txtEntityName.Text}.cs";

            File.WriteAllText(idalPath, idal.GenerateCodes().ToString());
            File.WriteAllText(dalPath, dal.GenerateCodes().ToString());
            File.WriteAllText(modelPath, model.GenerateCodes().ToString());

            Close();
        }
    }
}
