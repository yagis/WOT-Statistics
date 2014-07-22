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
    public partial class ctxKillCounts : DevExpress.XtraEditors.XtraUserControl
    {
        public ctxKillCounts()
        {
            InitializeComponent();
        }

        private void ctxKillCounts_Load(object sender, EventArgs e)
        {
            CHKBOX_TIERTOTAL.Text = Translations.TranslationGet("CHKBOX_TIERTOTAL", "DE", "Display Tier Totals");
            CHKBOX_ROWTOTAL.Text = Translations.TranslationGet("CHKBOX_ROWTOTAL", "DE", "Display Row Totals");
            CHKBOX_COLTOTALS.Text = Translations.TranslationGet("CHKBOX_COLTOTALS", "DE", "Display Column Totals");

            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("KillCountsShowTierTotals", out field);
            CHKBOX_TIERTOTAL.Checked = Convert.ToBoolean(field.NewValue);

            ((frmSetup)pForm)._propertyFields.TryGetValue("KillCountsShowColumnTotals", out field);
            CHKBOX_COLTOTALS.Checked = Convert.ToBoolean(field.NewValue);

            ((frmSetup)pForm)._propertyFields.TryGetValue("KillCountsShowRowTotals", out field);
            CHKBOX_ROWTOTAL.Checked = Convert.ToBoolean(field.NewValue);
        }

        private void checkEdit3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            
        }

        private void checkTierTotals_CheckedChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("KillCountsShowTierTotals", out field);
            field.NewValue = CHKBOX_TIERTOTAL.Checked;
        }

        private void checkRowTotals_CheckedChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("KillCountsShowRowTotals", out field);
            field.NewValue = CHKBOX_ROWTOTAL.Checked;
        }

        private void checkColumnTotals_CheckedChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("KillCountsShowColumnTotals", out field);
            field.NewValue = CHKBOX_COLTOTALS.Checked;
        }
    }
}
