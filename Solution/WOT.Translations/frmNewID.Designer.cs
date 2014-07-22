namespace WOT.Translations
{
    partial class frmNewID
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.textLangID = new DevExpress.XtraEditors.TextEdit();
            this.textEngDesc = new DevExpress.XtraEditors.TextEdit();
            this.textNatDesc = new DevExpress.XtraEditors.TextEdit();
            this.butSave = new DevExpress.XtraEditors.SimpleButton();
            this.butClose = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.textLangID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEngDesc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textNatDesc.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl1.Location = new System.Drawing.Point(12, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(72, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Language ID :";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl2.Location = new System.Drawing.Point(12, 42);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(72, 13);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "English Desc :";
            this.labelControl2.Click += new System.EventHandler(this.labelControl2_Click);
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl3.Location = new System.Drawing.Point(12, 74);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(72, 13);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "National Desc :";
            // 
            // textLangID
            // 
            this.textLangID.Location = new System.Drawing.Point(90, 9);
            this.textLangID.Name = "textLangID";
            this.textLangID.Size = new System.Drawing.Size(79, 20);
            this.textLangID.TabIndex = 3;
            // 
            // textEngDesc
            // 
            this.textEngDesc.Location = new System.Drawing.Point(90, 39);
            this.textEngDesc.Name = "textEngDesc";
            this.textEngDesc.Size = new System.Drawing.Size(196, 20);
            this.textEngDesc.TabIndex = 4;
            // 
            // textNatDesc
            // 
            this.textNatDesc.Location = new System.Drawing.Point(90, 71);
            this.textNatDesc.Name = "textNatDesc";
            this.textNatDesc.Size = new System.Drawing.Size(196, 20);
            this.textNatDesc.TabIndex = 5;
            // 
            // butSave
            // 
            this.butSave.Location = new System.Drawing.Point(176, 110);
            this.butSave.Name = "butSave";
            this.butSave.Size = new System.Drawing.Size(75, 23);
            this.butSave.TabIndex = 6;
            this.butSave.Text = "Save";
            this.butSave.Click += new System.EventHandler(this.butSave_Click);
            // 
            // butClose
            // 
            this.butClose.Location = new System.Drawing.Point(257, 110);
            this.butClose.Name = "butClose";
            this.butClose.Size = new System.Drawing.Size(75, 23);
            this.butClose.TabIndex = 7;
            this.butClose.Text = "Close";
            this.butClose.Click += new System.EventHandler(this.butClose_Click);
            // 
            // frmNewID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 145);
            this.Controls.Add(this.butClose);
            this.Controls.Add(this.butSave);
            this.Controls.Add(this.textNatDesc);
            this.Controls.Add(this.textEngDesc);
            this.Controls.Add(this.textLangID);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmNewID";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add new Language";
            ((System.ComponentModel.ISupportInitialize)(this.textLangID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEngDesc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textNatDesc.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit textLangID;
        private DevExpress.XtraEditors.TextEdit textEngDesc;
        private DevExpress.XtraEditors.TextEdit textNatDesc;
        private DevExpress.XtraEditors.SimpleButton butSave;
        private DevExpress.XtraEditors.SimpleButton butClose;
    }
}