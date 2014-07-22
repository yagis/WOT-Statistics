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
    public partial class ctxVBAddict : DevExpress.XtraEditors.XtraUserControl
    {
        public ctxVBAddict()
        {
            InitializeComponent();
        }

        private void ctxVBAddict_Load(object sender, EventArgs e)
        {


            chkEnablevBAddict.Text = Translations.TranslationGet("CHKBOX_ENABLE_VBADDICT", "DE", "Enable uploading to vBAddict.net");
            radioButton1.Text = Translations.TranslationGet("CHKBOX_SUBMITDOSSIER", "DE", "Submit Dossier file to vBAddict.net");
            radioButton2.Text = Translations.TranslationGet("CHKBOX_SUBMITDOSSIER_BATTLERESULT", "DE", "Submit Dossier file and Battle Results to vBAddict.net");
            radioButton3.Text = Translations.TranslationGet("CHKBOX_SUBMITDOSSIER_BATTLERESULT_REPLAY", "DE", "Submit Dossier file, Battle Results and Replays to vBAddict.net");
            Form pForm = ParentForm;
            PropertyFields field;

            ((frmSetup)pForm)._propertyFields.TryGetValue("AllowvBAddictUpload", out field);
            chkEnablevBAddict.Checked = Convert.ToBoolean(field.NewValue);
            ((frmSetup)pForm)._propertyFields.TryGetValue("AllowvBAddictUploadDossier", out field);
            radioButton1.Checked = Convert.ToBoolean(field.NewValue);
            ((frmSetup)pForm)._propertyFields.TryGetValue("AllowvBAddictUploadDossierBattleResult", out field);
            radioButton2.Checked = Convert.ToBoolean(field.NewValue);
            ((frmSetup)pForm)._propertyFields.TryGetValue("AllowvBAddictUploadDossierBattleResultReplay", out field);
            radioButton3.Checked = Convert.ToBoolean(field.NewValue);

        }

        private void checkAllowSubmit_CheckedChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("AllowvBAddictUpload", out field);
            field.NewValue = chkEnablevBAddict.Checked;
            ((frmSetup)pForm)._propertyFields.TryGetValue("AllowvBAddictUploadDossier", out field);
            field.NewValue = radioButton1.Checked;
            ((frmSetup)pForm)._propertyFields.TryGetValue("AllowvBAddictUploadDossierBattleResult", out field);
            field.NewValue = radioButton2.Checked;
            ((frmSetup)pForm)._propertyFields.TryGetValue("AllowvBAddictUploadDossierBattleResultReplay", out field);
            field.NewValue = radioButton3.Checked;
        }

        private void pictureEdit1_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.vbaddict.net/wotstatistics");
            }
            catch
            {

            }
        }

        private void chkEnablevBAddict_CheckedChanged(object sender, EventArgs e)
        {
            
            radioButton1.Enabled = chkEnablevBAddict.Checked;
            radioButton2.Enabled = chkEnablevBAddict.Checked;
            radioButton3.Enabled = chkEnablevBAddict.Checked;


        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            WOTStatistics.Core.WOTHelper.WriteADURegistry("EnableBattleResults", "0");
            WOTStatistics.Core.WOTHelper.WriteADURegistry("EnableReplays", "0");
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            WOTStatistics.Core.WOTHelper.WriteADURegistry("EnableBattleResults", "1");
            WOTStatistics.Core.WOTHelper.WriteADURegistry("EnableReplays", "0");
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            WOTStatistics.Core.WOTHelper.WriteADURegistry("EnableBattleResults", "1");
            WOTStatistics.Core.WOTHelper.WriteADURegistry("EnableReplays", "1");

        }
    }
}
