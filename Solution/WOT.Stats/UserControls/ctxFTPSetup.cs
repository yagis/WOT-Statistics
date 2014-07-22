using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using WOTStatistics.Core;

namespace WOT.Stats
{
    public partial class ctxFTPSetup : DevExpress.XtraEditors.XtraUserControl
    {
        public ctxFTPSetup()
        {
            InitializeComponent();
        }

        private void ctxFTPSetup_Load(object sender, EventArgs e)
        {
            CHKBOX_ALLOWFTP.Text = Translations.TranslationGet("CHKBOX_ALLOWFTP", "DE", "Allow Dossier Sharing");
            STR_FTPSERVER.Text = Translations.TranslationGet("STR_FTPSERVER", "DE", "FTP Server :");
            STR_FTPUSER.Text = Translations.TranslationGet("STR_FTPUSER", "DE", "User ID :");
            STR_FTPPASS.Text = Translations.TranslationGet("STR_FTPPASS", "DE", "Password :");

            CHKBOX_ALLOWCLOUDSHARE.Text = Translations.TranslationGet("CHKBOX_ALLOWCLOUDSHARE", "DE", "Allow Dropbox/Sky Drive/Google Drive Sharing");
            STR_CLOUDLOC.Text = Translations.TranslationGet("STR_CLOUDLOC", "DE", "Location :");

            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("FTP_Host", out field);
            txtFTPServer.Text = Convert.ToString(field.NewValue);

            ((frmSetup)pForm)._propertyFields.TryGetValue("FTP_AllowFTP", out field);
            CHKBOX_ALLOWFTP.Checked = Convert.ToBoolean(field.NewValue);

            ((frmSetup)pForm)._propertyFields.TryGetValue("FTP_UserID", out field);
            txtUserID.Text = Convert.ToString(field.NewValue);

            ((frmSetup)pForm)._propertyFields.TryGetValue("FTP_UserPWD", out field);
            txtPassword.Text = Convert.ToString(field.NewValue);

            ((frmSetup)pForm)._propertyFields.TryGetValue("Cloud_Allow", out field);
            CHKBOX_ALLOWCLOUDSHARE.Checked = Convert.ToBoolean(field.NewValue);

            ((frmSetup)pForm)._propertyFields.TryGetValue("Cloud_Path", out field);
            textCloudSharing.Text = Convert.ToString(field.NewValue);
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            txtFTPServer.Enabled = CHKBOX_ALLOWFTP.Checked;
            txtUserID.Enabled = CHKBOX_ALLOWFTP.Checked;
            txtPassword.Enabled = CHKBOX_ALLOWFTP.Checked;

            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("FTP_AllowFTP", out field);
            field.NewValue = CHKBOX_ALLOWFTP.Checked;
        }

        private void txtFTPServer_EditValueChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("FTP_Host", out field);
            field.NewValue = txtFTPServer.Text;

            
        }

        private void txtUserID_EditValueChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("FTP_UserID", out field);
            field.NewValue = txtUserID.Text;
        }

        private void txtPassword_EditValueChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("FTP_UserPWD", out field);
            field.NewValue = txtPassword.Text;
        }

        private void buttonEdit1_EditValueChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("Cloud_Path", out field);
            field.NewValue = textCloudSharing.Text;
        }

        private void checkCloudSharing_CheckedChanged(object sender, EventArgs e)
        {
            textCloudSharing.Enabled = CHKBOX_ALLOWCLOUDSHARE.Checked;

            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("Cloud_Allow", out field);
            field.NewValue = CHKBOX_ALLOWCLOUDSHARE.Checked;
        }

        private void textCloudSharing_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            using (FolderBrowserDialog fb = new FolderBrowserDialog())
            {
                if (fb.ShowDialog(this) == DialogResult.OK)
                {
                    textCloudSharing.Text = fb.SelectedPath;
                }
            }
        }

        private void textCloudSharing_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
