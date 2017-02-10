using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DbDictExport.Core.Codes.Acartons;
using DbDictExport.Core.Dal;
using MetroFramework.Forms;

namespace DbDictExport.WinForm.CodeForms
{
    public partial class AcartonsCodeForm : MetroForm
    {
        public Table Table { get; set; }

        public AcartonsCodeForm()
        {
            InitializeComponent();
        }

        public AcartonsCodeForm(Table table)
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

            var idal = new DalInterfaceCodeFactory(txtEntityName.Text, txtModuleName.Text, Table);
            var dal = new DalCodeFactory(txtEntityName.Text, txtModuleName.Text, Table);
            var model = new ModelCodeFactory(txtEntityName.Text, txtModuleName.Text, Table);
            var ibll = new BllInterfaceCodeFactory(txtEntityName.Text, txtModuleName.Text, Table);
            var bll = new BllCodeFactory(txtEntityName.Text, txtModuleName.Text, Table);
            var controller = new ControllerCodeFactory(txtEntityName.Text, txtModuleName.Text, Table);

            var idalPath = folderBrowserDialog.SelectedPath + $"\\I{txtEntityName.Text}Repo.cs";
            var dalPath = folderBrowserDialog.SelectedPath + $"\\{txtEntityName.Text}Repo.cs";
            var modelPath = folderBrowserDialog.SelectedPath + $"\\{txtEntityName.Text}.cs";
            var ibllPath = folderBrowserDialog.SelectedPath + $"\\I{txtEntityName.Text}Service.cs";
            var bllPath = folderBrowserDialog.SelectedPath + $"\\{txtEntityName.Text}Service.cs";
            var controllerPath = folderBrowserDialog.SelectedPath + $"\\{Inflector.MakePlural(txtEntityName.Text)}Controller.cs";

            File.WriteAllText(idalPath, idal.GenerateCodes().ToString());
            File.WriteAllText(dalPath, dal.GenerateCodes().ToString());
            File.WriteAllText(modelPath, model.GenerateCodes().ToString());
            File.WriteAllText(ibllPath, ibll.GenerateCodes().ToString());
            File.WriteAllText(bllPath, bll.GenerateCodes().ToString());
            File.WriteAllText(controllerPath, controller.GenerateCodes().ToString());

            Close();
        }
    }
}
