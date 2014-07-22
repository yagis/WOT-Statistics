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
    public partial class ctxVersion : DevExpress.XtraEditors.XtraUserControl
    {
        public ctxVersion()
        {
            InitializeComponent();
        }

        private void ctxVersion_Load(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("AllowVersionCheck", out field);
            CHKBOX_NEWVERSION.Checked = Convert.ToBoolean(field.NewValue);
            
            STR_CURRENTVERSION.Text = Translations.TranslationGet("STR_CURRENTVERSION", "DE", "Current Installed Version : ") + ProductVersion;
            CHKBOX_NEWVERSION.Text = Translations.TranslationGet("CHKBOX_NEWVERSION", "DE", "Inform me when a new version is available");
        }

        private void checkUpdateCheck_CheckedChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("AllowVersionCheck", out field);
            field.NewValue = CHKBOX_NEWVERSION.Checked;
        }
    }
}
