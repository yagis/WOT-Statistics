namespace WOT.Stats
{
    partial class frmAddEditPlayer
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddEditPlayer));
            this.STR_DOSSIERFILE = new DevExpress.XtraEditors.LabelControl();
            this.STR_PLAYERID = new DevExpress.XtraEditors.LabelControl();
            this.CHKBOX_FTPFILE = new DevExpress.XtraEditors.LabelControl();
            this.checkFTPFile = new DevExpress.XtraEditors.CheckEdit();
            this.BTN_CANCEL = new DevExpress.XtraEditors.SimpleButton();
            this.BTN_SAVE = new DevExpress.XtraEditors.SimpleButton();
            this.txtWatchFile = new DevExpress.XtraEditors.ButtonEdit();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.txtPlayerID = new DevExpress.XtraEditors.TextEdit();
            this.STR_PLAYERREALM = new DevExpress.XtraEditors.LabelControl();
            this.txtPlayerRealm = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.checkFTPFile.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWatchFile.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPlayerID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPlayerRealm.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // STR_DOSSIERFILE
            // 
            this.STR_DOSSIERFILE.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STR_DOSSIERFILE.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.STR_DOSSIERFILE.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.STR_DOSSIERFILE.Location = new System.Drawing.Point(12, 12);
            this.STR_DOSSIERFILE.Name = "STR_DOSSIERFILE";
            this.STR_DOSSIERFILE.Size = new System.Drawing.Size(83, 20);
            this.STR_DOSSIERFILE.TabIndex = 1;
            this.STR_DOSSIERFILE.Text = "Dossier File :";
            // 
            // STR_PLAYERID
            // 
            this.STR_PLAYERID.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STR_PLAYERID.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.STR_PLAYERID.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.STR_PLAYERID.Location = new System.Drawing.Point(12, 41);
            this.STR_PLAYERID.Name = "STR_PLAYERID";
            this.STR_PLAYERID.Size = new System.Drawing.Size(83, 16);
            this.STR_PLAYERID.TabIndex = 3;
            this.STR_PLAYERID.Text = "Player ID :";
            // 
            // CHKBOX_FTPFILE
            // 
            this.CHKBOX_FTPFILE.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHKBOX_FTPFILE.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.CHKBOX_FTPFILE.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.CHKBOX_FTPFILE.Location = new System.Drawing.Point(12, 103);
            this.CHKBOX_FTPFILE.Name = "CHKBOX_FTPFILE";
            this.CHKBOX_FTPFILE.Size = new System.Drawing.Size(83, 16);
            this.CHKBOX_FTPFILE.TabIndex = 5;
            this.CHKBOX_FTPFILE.Text = "Shared File :";
            // 
            // checkFTPFile
            // 
            this.checkFTPFile.Location = new System.Drawing.Point(101, 101);
            this.checkFTPFile.Name = "checkFTPFile";
            this.checkFTPFile.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkFTPFile.Properties.Appearance.Options.UseFont = true;
            this.checkFTPFile.Properties.Caption = "";
            this.checkFTPFile.Size = new System.Drawing.Size(75, 19);
            this.checkFTPFile.TabIndex = 6;
            // 
            // BTN_CANCEL
            // 
            this.BTN_CANCEL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BTN_CANCEL.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_CANCEL.Appearance.Options.UseFont = true;
            this.BTN_CANCEL.Location = new System.Drawing.Point(519, 160);
            this.BTN_CANCEL.Name = "BTN_CANCEL";
            this.BTN_CANCEL.Size = new System.Drawing.Size(75, 23);
            this.BTN_CANCEL.TabIndex = 7;
            this.BTN_CANCEL.Text = "Cancel";
            this.BTN_CANCEL.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // BTN_SAVE
            // 
            this.BTN_SAVE.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BTN_SAVE.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_SAVE.Appearance.Options.UseFont = true;
            this.BTN_SAVE.Location = new System.Drawing.Point(438, 160);
            this.BTN_SAVE.Name = "BTN_SAVE";
            this.BTN_SAVE.Size = new System.Drawing.Size(75, 23);
            this.BTN_SAVE.TabIndex = 8;
            this.BTN_SAVE.Text = "Save";
            this.BTN_SAVE.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // txtWatchFile
            // 
            this.txtWatchFile.EditValue = "";
            this.txtWatchFile.Location = new System.Drawing.Point(101, 8);
            this.txtWatchFile.Name = "txtWatchFile";
            this.txtWatchFile.Properties.Appearance.Font = new System.Drawing.Font("Arial Unicode MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWatchFile.Properties.Appearance.Options.UseFont = true;
            serializableAppearanceObject1.Font = new System.Drawing.Font("Arial Unicode MS", 8.25F);
            serializableAppearanceObject1.Options.UseFont = true;
            this.txtWatchFile.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "Browse", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)), serializableAppearanceObject1, "", null, null, true)});
            this.txtWatchFile.Size = new System.Drawing.Size(487, 24);
            this.txtWatchFile.TabIndex = 2;
            this.txtWatchFile.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtWatchFile_ButtonClick);
            // 
            // txtPlayerID
            // 
            this.txtPlayerID.EditValue = "";
            this.txtPlayerID.Location = new System.Drawing.Point(103, 38);
            this.txtPlayerID.Name = "txtPlayerID";
            this.txtPlayerID.Properties.Appearance.Font = new System.Drawing.Font("Arial Unicode MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPlayerID.Properties.Appearance.Options.UseFont = true;
            this.txtPlayerID.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.White;
            this.txtPlayerID.Properties.AppearanceDisabled.Font = new System.Drawing.Font("Arial Unicode MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPlayerID.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.DimGray;
            this.txtPlayerID.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtPlayerID.Properties.AppearanceDisabled.Options.UseFont = true;
            this.txtPlayerID.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtPlayerID.Size = new System.Drawing.Size(199, 24);
            this.txtPlayerID.TabIndex = 4;
            this.txtPlayerID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPlayerID_KeyPress);
            // 
            // STR_PLAYERREALM
            // 
            this.STR_PLAYERREALM.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STR_PLAYERREALM.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.STR_PLAYERREALM.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.STR_PLAYERREALM.Location = new System.Drawing.Point(12, 71);
            this.STR_PLAYERREALM.Name = "STR_PLAYERREALM";
            this.STR_PLAYERREALM.Size = new System.Drawing.Size(83, 16);
            this.STR_PLAYERREALM.TabIndex = 9;
            this.STR_PLAYERREALM.Text = "Player ID :";
            // 
            // txtPlayerRealm
            // 
            this.txtPlayerRealm.EditValue = "";
            this.txtPlayerRealm.Location = new System.Drawing.Point(103, 68);
            this.txtPlayerRealm.Name = "txtPlayerRealm";
            this.txtPlayerRealm.Properties.Appearance.Font = new System.Drawing.Font("Arial Unicode MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPlayerRealm.Properties.Appearance.Options.UseFont = true;
            this.txtPlayerRealm.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.White;
            this.txtPlayerRealm.Properties.AppearanceDisabled.Font = new System.Drawing.Font("Arial Unicode MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPlayerRealm.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.DimGray;
            this.txtPlayerRealm.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtPlayerRealm.Properties.AppearanceDisabled.Options.UseFont = true;
            this.txtPlayerRealm.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtPlayerRealm.Size = new System.Drawing.Size(199, 24);
            this.txtPlayerRealm.TabIndex = 10;
            // 
            // frmAddEditPlayer
            // 
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 184);
            this.Controls.Add(this.STR_PLAYERREALM);
            this.Controls.Add(this.txtPlayerRealm);
            this.Controls.Add(this.BTN_SAVE);
            this.Controls.Add(this.BTN_CANCEL);
            this.Controls.Add(this.checkFTPFile);
            this.Controls.Add(this.CHKBOX_FTPFILE);
            this.Controls.Add(this.STR_PLAYERID);
            this.Controls.Add(this.txtWatchFile);
            this.Controls.Add(this.STR_DOSSIERFILE);
            this.Controls.Add(this.txtPlayerID);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.LookAndFeel.SkinName = "Office 2010 Black";
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddEditPlayer";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmAddEditPlayer";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.frmAddEditPlayer_HelpButtonClicked);
            this.Load += new System.EventHandler(this.frmAddEditPlayer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.checkFTPFile.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWatchFile.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPlayerID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPlayerRealm.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl STR_DOSSIERFILE;
        private DevExpress.XtraEditors.LabelControl STR_PLAYERID;
        private DevExpress.XtraEditors.LabelControl CHKBOX_FTPFILE;
        private DevExpress.XtraEditors.CheckEdit checkFTPFile;
        private DevExpress.XtraEditors.SimpleButton BTN_CANCEL;
        private DevExpress.XtraEditors.SimpleButton BTN_SAVE;
        private DevExpress.XtraEditors.ButtonEdit txtWatchFile;
        private System.Windows.Forms.HelpProvider helpProvider1;
        private DevExpress.XtraEditors.TextEdit txtPlayerID;
        private DevExpress.XtraEditors.LabelControl STR_PLAYERREALM;
        private DevExpress.XtraEditors.TextEdit txtPlayerRealm;
    }
}