using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using WOTStatistics.Core;

namespace WOT.Stats
{
    public partial class frmCredits : DevExpress.XtraEditors.XtraForm
    {
        public frmCredits()
        {
            InitializeComponent();
            helpProvider1.HelpNamespace = Path.Combine(WOTHelper.GetEXEPath(), "Help", "WoT_Stats.chm");
            helpProvider1.SetHelpNavigator(this, HelpNavigator.TopicId);
            helpProvider1.SetHelpKeyword(this, "100");
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmCredits_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            Help.ShowHelp(this, helpProvider1.HelpNamespace, HelpNavigator.TopicId, "100");
            e.Cancel = true;
        }

        private void frmCredits_Load(object sender, EventArgs e)
        {
            Text = Translations.TranslationGet("STR_CREDITS", "DE", "Credits");
            BTN_CLOSE.Text = Translations.TranslationGet("BTN_CLOSE", "DE", "Close");
        }
    }
}