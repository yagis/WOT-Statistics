namespace WOT.Stats
{
    partial class ctxFTPSetup
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
            this.CHKBOX_ALLOWFTP = new DevExpress.XtraEditors.CheckEdit();
            this.STR_FTPSERVER = new DevExpress.XtraEditors.LabelControl();
            this.STR_FTPUSER = new DevExpress.XtraEditors.LabelControl();
            this.STR_FTPPASS = new DevExpress.XtraEditors.LabelControl();
            this.txtFTPServer = new DevExpress.XtraEditors.TextEdit();
            this.txtUserID = new DevExpress.XtraEditors.TextEdit();
            this.txtPassword = new DevExpress.XtraEditors.TextEdit();
            this.CHKBOX_ALLOWCLOUDSHARE = new DevExpress.XtraEditors.CheckEdit();
            this.STR_CLOUDLOC = new DevExpress.XtraEditors.LabelControl();
            this.textCloudSharing = new DevExpress.XtraEditors.ButtonEdit();
            ((System.ComponentModel.ISupportInitialize)(this.CHKBOX_ALLOWFTP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFTPServer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHKBOX_ALLOWCLOUDSHARE.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textCloudSharing.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // CHKBOX_ALLOWFTP
            // 
            this.CHKBOX_ALLOWFTP.Location = new System.Drawing.Point(3, 4);
            this.CHKBOX_ALLOWFTP.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CHKBOX_ALLOWFTP.Name = "CHKBOX_ALLOWFTP";
            this.CHKBOX_ALLOWFTP.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHKBOX_ALLOWFTP.Properties.Appearance.Options.UseFont = true;
            this.CHKBOX_ALLOWFTP.Properties.AutoWidth = true;
            this.CHKBOX_ALLOWFTP.Properties.Caption = "Allow FTP Sharing";
            this.CHKBOX_ALLOWFTP.Size = new System.Drawing.Size(130, 21);
            this.CHKBOX_ALLOWFTP.TabIndex = 0;
            this.CHKBOX_ALLOWFTP.CheckedChanged += new System.EventHandler(this.checkEdit1_CheckedChanged);
            // 
            // STR_FTPSERVER
            // 
            this.STR_FTPSERVER.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STR_FTPSERVER.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.STR_FTPSERVER.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.STR_FTPSERVER.Location = new System.Drawing.Point(6, 34);
            this.STR_FTPSERVER.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.STR_FTPSERVER.Name = "STR_FTPSERVER";
            this.STR_FTPSERVER.Size = new System.Drawing.Size(126, 16);
            this.STR_FTPSERVER.TabIndex = 1;
            this.STR_FTPSERVER.Text = "FTP Server :";
            // 
            // STR_FTPUSER
            // 
            this.STR_FTPUSER.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STR_FTPUSER.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.STR_FTPUSER.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.STR_FTPUSER.Location = new System.Drawing.Point(6, 63);
            this.STR_FTPUSER.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.STR_FTPUSER.Name = "STR_FTPUSER";
            this.STR_FTPUSER.Size = new System.Drawing.Size(126, 16);
            this.STR_FTPUSER.TabIndex = 2;
            this.STR_FTPUSER.Text = "User ID :";
            // 
            // STR_FTPPASS
            // 
            this.STR_FTPPASS.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STR_FTPPASS.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.STR_FTPPASS.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.STR_FTPPASS.Location = new System.Drawing.Point(6, 91);
            this.STR_FTPPASS.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.STR_FTPPASS.Name = "STR_FTPPASS";
            this.STR_FTPPASS.Size = new System.Drawing.Size(126, 16);
            this.STR_FTPPASS.TabIndex = 3;
            this.STR_FTPPASS.Text = "Password :";
            // 
            // txtFTPServer
            // 
            this.txtFTPServer.Enabled = false;
            this.txtFTPServer.Location = new System.Drawing.Point(138, 31);
            this.txtFTPServer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFTPServer.Name = "txtFTPServer";
            this.txtFTPServer.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFTPServer.Properties.Appearance.Options.UseFont = true;
            this.txtFTPServer.Size = new System.Drawing.Size(239, 22);
            this.txtFTPServer.TabIndex = 4;
            this.txtFTPServer.EditValueChanged += new System.EventHandler(this.txtFTPServer_EditValueChanged);
            // 
            // txtUserID
            // 
            this.txtUserID.Enabled = false;
            this.txtUserID.Location = new System.Drawing.Point(138, 59);
            this.txtUserID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserID.Properties.Appearance.Options.UseFont = true;
            this.txtUserID.Size = new System.Drawing.Size(239, 22);
            this.txtUserID.TabIndex = 5;
            this.txtUserID.EditValueChanged += new System.EventHandler(this.txtUserID_EditValueChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Enabled = false;
            this.txtPassword.Location = new System.Drawing.Point(138, 87);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Properties.Appearance.Options.UseFont = true;
            this.txtPassword.Properties.PasswordChar = '*';
            this.txtPassword.Properties.UseSystemPasswordChar = true;
            this.txtPassword.Size = new System.Drawing.Size(239, 22);
            this.txtPassword.TabIndex = 6;
            this.txtPassword.EditValueChanged += new System.EventHandler(this.txtPassword_EditValueChanged);
            // 
            // CHKBOX_ALLOWCLOUDSHARE
            // 
            this.CHKBOX_ALLOWCLOUDSHARE.Location = new System.Drawing.Point(3, 128);
            this.CHKBOX_ALLOWCLOUDSHARE.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CHKBOX_ALLOWCLOUDSHARE.Name = "CHKBOX_ALLOWCLOUDSHARE";
            this.CHKBOX_ALLOWCLOUDSHARE.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHKBOX_ALLOWCLOUDSHARE.Properties.Appearance.Options.UseFont = true;
            this.CHKBOX_ALLOWCLOUDSHARE.Properties.AutoWidth = true;
            this.CHKBOX_ALLOWCLOUDSHARE.Properties.Caption = "Allow Dropbox/Sky Drive/Google Drive Sharing";
            this.CHKBOX_ALLOWCLOUDSHARE.Size = new System.Drawing.Size(290, 21);
            this.CHKBOX_ALLOWCLOUDSHARE.TabIndex = 7;
            this.CHKBOX_ALLOWCLOUDSHARE.CheckedChanged += new System.EventHandler(this.checkCloudSharing_CheckedChanged);
            // 
            // STR_CLOUDLOC
            // 
            this.STR_CLOUDLOC.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STR_CLOUDLOC.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.STR_CLOUDLOC.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.STR_CLOUDLOC.Location = new System.Drawing.Point(6, 157);
            this.STR_CLOUDLOC.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.STR_CLOUDLOC.Name = "STR_CLOUDLOC";
            this.STR_CLOUDLOC.Size = new System.Drawing.Size(126, 16);
            this.STR_CLOUDLOC.TabIndex = 8;
            this.STR_CLOUDLOC.Text = "Location :";
            // 
            // textCloudSharing
            // 
            this.textCloudSharing.Location = new System.Drawing.Point(138, 154);
            this.textCloudSharing.Name = "textCloudSharing";
            this.textCloudSharing.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textCloudSharing.Properties.Appearance.Options.UseFont = true;
            this.textCloudSharing.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.textCloudSharing.Size = new System.Drawing.Size(239, 22);
            this.textCloudSharing.TabIndex = 9;
            this.textCloudSharing.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.textCloudSharing_ButtonClick);
            this.textCloudSharing.EditValueChanged += new System.EventHandler(this.buttonEdit1_EditValueChanged);
            this.textCloudSharing.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textCloudSharing_KeyPress);
            // 
            // ctxFTPSetup
            // 
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textCloudSharing);
            this.Controls.Add(this.STR_CLOUDLOC);
            this.Controls.Add(this.CHKBOX_ALLOWCLOUDSHARE);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUserID);
            this.Controls.Add(this.txtFTPServer);
            this.Controls.Add(this.STR_FTPPASS);
            this.Controls.Add(this.STR_FTPUSER);
            this.Controls.Add(this.STR_FTPSERVER);
            this.Controls.Add(this.CHKBOX_ALLOWFTP);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ctxFTPSetup";
            this.Size = new System.Drawing.Size(749, 205);
            this.Load += new System.EventHandler(this.ctxFTPSetup_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CHKBOX_ALLOWFTP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFTPServer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHKBOX_ALLOWCLOUDSHARE.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textCloudSharing.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.CheckEdit CHKBOX_ALLOWFTP;
        private DevExpress.XtraEditors.LabelControl STR_FTPSERVER;
        private DevExpress.XtraEditors.LabelControl STR_FTPUSER;
        private DevExpress.XtraEditors.LabelControl STR_FTPPASS;
        private DevExpress.XtraEditors.TextEdit txtFTPServer;
        private DevExpress.XtraEditors.TextEdit txtUserID;
        private DevExpress.XtraEditors.TextEdit txtPassword;
        private DevExpress.XtraEditors.CheckEdit CHKBOX_ALLOWCLOUDSHARE;
        private DevExpress.XtraEditors.LabelControl STR_CLOUDLOC;
        private DevExpress.XtraEditors.ButtonEdit textCloudSharing;
    }
}
