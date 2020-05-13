namespace DbDictExport.WinForm.CodeForms
{
    partial class FmeaCodeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnRun = new System.Windows.Forms.Button();
            this.txtEntityName = new System.Windows.Forms.TextBox();
            this.txtModuleName = new System.Windows.Forms.TextBox();
            this.lblTableName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRun.Location = new System.Drawing.Point(160, 196);
            this.btnRun.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(197, 44);
            this.btnRun.TabIndex = 11;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // txtEntityName
            // 
            this.txtEntityName.Location = new System.Drawing.Point(161, 151);
            this.txtEntityName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtEntityName.Name = "txtEntityName";
            this.txtEntityName.Size = new System.Drawing.Size(196, 23);
            this.txtEntityName.TabIndex = 10;
            // 
            // txtModuleName
            // 
            this.txtModuleName.Location = new System.Drawing.Point(161, 113);
            this.txtModuleName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtModuleName.Name = "txtModuleName";
            this.txtModuleName.Size = new System.Drawing.Size(196, 23);
            this.txtModuleName.TabIndex = 9;
            this.txtModuleName.Text = "FMEA.Api";
            // 
            // lblTableName
            // 
            this.lblTableName.AutoSize = true;
            this.lblTableName.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTableName.Location = new System.Drawing.Point(37, 75);
            this.lblTableName.Name = "lblTableName";
            this.lblTableName.Size = new System.Drawing.Size(100, 20);
            this.lblTableName.TabIndex = 8;
            this.lblTableName.Text = "{TableName}";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "Namespace Prefix: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 154);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Entity Name: ";
            // 
            // FmeaCodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 262);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.txtEntityName);
            this.Controls.Add(this.txtModuleName);
            this.Controls.Add(this.lblTableName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FmeaCodeForm";
            this.Padding = new System.Windows.Forms.Padding(23, 75, 23, 25);
            this.Text = "FMEA Codes";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.TextBox txtEntityName;
        private System.Windows.Forms.TextBox txtModuleName;
        private System.Windows.Forms.Label lblTableName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}