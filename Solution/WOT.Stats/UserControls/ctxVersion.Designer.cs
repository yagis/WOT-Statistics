namespace WOT.Stats
{
    partial class ctxVersion
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.STR_CURRENTVERSION = new DevExpress.XtraEditors.LabelControl();
            this.CHKBOX_NEWVERSION = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.CHKBOX_NEWVERSION.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // STR_CURRENTVERSION
            // 
            this.STR_CURRENTVERSION.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STR_CURRENTVERSION.Location = new System.Drawing.Point(5, 3);
            this.STR_CURRENTVERSION.Name = "STR_CURRENTVERSION";
            this.STR_CURRENTVERSION.Size = new System.Drawing.Size(75, 16);
            this.STR_CURRENTVERSION.TabIndex = 0;
            this.STR_CURRENTVERSION.Text = "labelControl1";
            // 
            // CHKBOX_NEWVERSION
            // 
            this.CHKBOX_NEWVERSION.AutoSizeInLayoutControl = true;
            this.CHKBOX_NEWVERSION.Location = new System.Drawing.Point(3, 22);
            this.CHKBOX_NEWVERSION.Name = "CHKBOX_NEWVERSION";
            this.CHKBOX_NEWVERSION.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHKBOX_NEWVERSION.Properties.Appearance.Options.UseFont = true;
            this.CHKBOX_NEWVERSION.Properties.AutoWidth = true;
            this.CHKBOX_NEWVERSION.Properties.Caption = "Inform me when a new version is available";
            this.CHKBOX_NEWVERSION.Size = new System.Drawing.Size(263, 21);
            this.CHKBOX_NEWVERSION.TabIndex = 1;
            this.CHKBOX_NEWVERSION.Visible = false;
            this.CHKBOX_NEWVERSION.CheckedChanged += new System.EventHandler(this.checkUpdateCheck_CheckedChanged);
            // 
            // ctxVersion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CHKBOX_NEWVERSION);
            this.Controls.Add(this.STR_CURRENTVERSION);
            this.Name = "ctxVersion";
            this.Size = new System.Drawing.Size(590, 150);
            this.Load += new System.EventHandler(this.ctxVersion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CHKBOX_NEWVERSION.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl STR_CURRENTVERSION;
        private DevExpress.XtraEditors.CheckEdit CHKBOX_NEWVERSION;
    }
}
