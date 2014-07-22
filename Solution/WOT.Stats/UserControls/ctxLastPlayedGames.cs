using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using WOTStatistics.Core;

namespace WOT.Stats
{
    public partial class ctxLastPlayedGames : DevExpress.XtraEditors.XtraUserControl
    {
        public ctxLastPlayedGames()
        {
            InitializeComponent();
        }

        private void textEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar > 31 && (e.KeyChar < '0' || e.KeyChar > '9'))
            {
                e.Handled = true;
            }
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            radioGroup1.Enabled = CHKBOX_COMPARE.Checked;
            if (CHKBOX_COMPARE.Checked)
                radioGroup1.EditValue = "0";

            SaveSelection();
        }

        private void ctxLastPlayedGames_Load(object sender, EventArgs e)
        {

            CHKBOX_COMPARE.Text = Translations.TranslationGet("CHKBOX_COMPARE", "DE", "Compare (Doesn't apply when grouped)");
            STR_SHOWLASTBATTLES_1.Text = Translations.TranslationGet("STR_SHOWLASTBATTLES_1", "DE", "Show last");
            STR_SHOWLASTBATTLES_2.Text = Translations.TranslationGet("STR_SHOWLASTBATTLES_2", "DE", "battles");

            radioGroup1.Properties.Items[0].Description = Translations.TranslationGet("RADIO_OVERALLSTATS", "DE", "To Overall Stats");
            radioGroup1.Properties.Items[1].Description = Translations.TranslationGet("RADIO_TANKSTATS", "DE", "To Tank Stats");

            lblAutoAfterXBattles.Text = Translations.TranslationGet("STR_SHOWLASTBATTLES_2", "DE", "battles");
            lblAutoAfterXHours.Text = Translations.TranslationGet("STR_CAP_AUTOAFTERXHOURS", "DE", "hours after last battle");
            chkAutoAfterXBattlesMessage.Text = Translations.TranslationGet("STR_CAP_AUTODISPLAYMESSAGE", "DE", "Display message before creating new session");
            chkAutoAfterXHoursMessage.Text = Translations.TranslationGet("STR_CAP_AUTODISPLAYMESSAGE", "DE", "Display message before creating new session");
            chkAutoOnStartUp.Text = Translations.TranslationGet("STR_CAP_AUTOONSTARTUP", "DE", "On Startup");
            chkAutoOnStartUpMessage.Text = Translations.TranslationGet("STR_CAP_AUTODISPLAYMESSAGE", "DE", "Display message before creating new session");
            chkAutoStartSession.Text = Translations.TranslationGet("STR_CAP_AUTOSTARTSESSION", "DE", "Auto create new session");
            chkAutoAfterXBattles.Text = Translations.TranslationGet("STR_CAP_AUTOAFTERXBATTLES", "DE", "After every ");


            FormHelpers.ResizeLables(this.Controls);
            STR_SHOWLASTBATTLES_1.AutoSizeMode = LabelAutoSizeMode.Horizontal;
            STR_SHOWLASTBATTLES_1.Left = 3;
            textLastPlayedMaxNo.Left = STR_SHOWLASTBATTLES_1.Left + STR_SHOWLASTBATTLES_1.Width + 5;
            STR_SHOWLASTBATTLES_2.Left = STR_SHOWLASTBATTLES_1.Left + STR_SHOWLASTBATTLES_1.Width + 5 + textLastPlayedMaxNo.Width + 5;
            STR_SHOWLASTBATTLES_2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;

            lblAutoAfterXBattles.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            lblAutoAfterXHours.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            chkAutoAfterXBattles.AutoSizeInLayoutControl = true;
            txtAutoAfterXBattles.Left = chkAutoAfterXBattles.Left + chkAutoAfterXBattles.Width + 5;
            lblAutoAfterXBattles.Left = txtAutoAfterXBattles.Left + txtAutoAfterXBattles.Width + 5;
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("LastPlayedCompare", out field);
            int compare = Convert.ToInt32(field.NewValue);

            if (compare == 0)
            {
                CHKBOX_COMPARE.Checked = false;
                radioGroup1.Enabled = false;
            }
            else
            {
                CHKBOX_COMPARE.Checked = true;
                radioGroup1.Enabled = true;

                if (compare == 1)
                {
                    radioGroup1.EditValue = "0";
                }
                else
                {
                    radioGroup1.EditValue = "1";
                }
            }

            ((frmSetup)pForm)._propertyFields.TryGetValue("LastPlayedCompareQuota", out field);
            textLastPlayedMaxNo.Text = Convert.ToString(field.NewValue);



            ((frmSetup)pForm)._propertyFields.TryGetValue("AutoCreateSession", out field);
            chkAutoStartSession.Checked = Convert.ToBoolean(field.NewValue);


            ((frmSetup)pForm)._propertyFields.TryGetValue("AutoSessionOnStartUp", out field);
            chkAutoOnStartUp.Checked = Convert.ToBoolean(field.NewValue);



            ((frmSetup)pForm)._propertyFields.TryGetValue("AutoSessionOnStartUpMessage", out field);
            chkAutoOnStartUpMessage.Checked = Convert.ToBoolean(field.NewValue);



            ((frmSetup)pForm)._propertyFields.TryGetValue("AutoSessionXBattles", out field);
            chkAutoAfterXBattles.Checked = Convert.ToBoolean(field.NewValue);



            ((frmSetup)pForm)._propertyFields.TryGetValue("AutoSessionXBattlesMessage", out field);
            chkAutoAfterXBattlesMessage.Checked = Convert.ToBoolean(field.NewValue);



            ((frmSetup)pForm)._propertyFields.TryGetValue("AutoSessionXHours", out field);
            chkAutoAfterXHours.Checked = Convert.ToBoolean(field.NewValue);



            ((frmSetup)pForm)._propertyFields.TryGetValue("AutoSessionXHoursMessage", out field);
            chkAutoAfterXHoursMessage.Checked = Convert.ToBoolean(field.NewValue);



            ((frmSetup)pForm)._propertyFields.TryGetValue("AutoSessionXBattlesValue", out field);
            txtAutoAfterXBattles.Text = Convert.ToString(Convert.ToInt16(field.NewValue) == 0 ? "" : field.NewValue);




            ((frmSetup)pForm)._propertyFields.TryGetValue("AutoSessionXHoursValue", out field);
            txtAutoAfterXHours.Text = Convert.ToString(Convert.ToInt16(field.NewValue) == 0 ? "" : field.NewValue);



            SetAutoStartOptions();

            chkAutoStartSession.CheckedChanged += chkAutoStartSession_CheckedChanged;
            chkAutoOnStartUp.CheckedChanged += chkAutoOnStartUp_CheckedChanged;
            chkAutoOnStartUpMessage.CheckedChanged += chkAutoOnStartUpMessage_CheckedChanged;
            chkAutoAfterXBattles.CheckedChanged += chkAutoAfterXBattles_CheckedChanged;
            chkAutoAfterXBattlesMessage.CheckedChanged += chkAutoAfterXBattlesMessage_CheckedChanged;
            chkAutoAfterXHours.CheckedChanged += chkAutoAfterXHours_CheckedChanged;
            chkAutoAfterXHoursMessage.CheckedChanged += chkAutoAfterXHoursMessage_CheckedChanged;
            txtAutoAfterXBattles.EditValueChanged += txtAutoAfterXBattles_EditValueChanged;
            txtAutoAfterXHours.EditValueChanged += txtAutoAfterXHours_EditValueChanged;



        }

        void chkAutoStartSession_CheckedChanged(object sender, EventArgs e)
        {
            SetAutoStartOptions();
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("AutoCreateSession", out field);
            field.NewValue = chkAutoStartSession.Checked;
        }

        private void SetAutoStartOptions()
        {


            chkAutoAfterXBattles.Enabled = chkAutoStartSession.Checked;
            lblAutoAfterXBattles.Enabled = chkAutoStartSession.Checked;
            txtAutoAfterXBattles.Enabled = chkAutoStartSession.Checked;
            txtAutoAfterXBattles.Enabled = chkAutoAfterXBattles.Checked;
            chkAutoAfterXBattlesMessage.Enabled = chkAutoAfterXBattles.Checked;
            chkAutoAfterXBattlesMessage.Enabled = chkAutoStartSession.Checked;

            chkAutoOnStartUp.Enabled = chkAutoStartSession.Checked;
            chkAutoOnStartUpMessage.Enabled = chkAutoOnStartUp.Checked;
            chkAutoOnStartUpMessage.Enabled = chkAutoStartSession.Checked;


            chkAutoAfterXHours.Enabled = chkAutoStartSession.Checked;
            lblAutoAfterXHours.Enabled = chkAutoStartSession.Checked;
            txtAutoAfterXHours.Enabled = chkAutoStartSession.Checked;
            txtAutoAfterXHours.Enabled = chkAutoAfterXHours.Checked;
            chkAutoAfterXHoursMessage.Enabled = chkAutoAfterXHours.Checked;
            chkAutoAfterXHoursMessage.Enabled = chkAutoStartSession.Checked;

            if (!chkAutoStartSession.Checked)
            {
                txtAutoAfterXBattles.EditValueChanged -= txtAutoAfterXHours_EditValueChanged;
                txtAutoAfterXBattles.Text = "10";
                txtAutoAfterXBattles.EditValueChanged += txtAutoAfterXHours_EditValueChanged;
                txtAutoAfterXBattles.Enabled = false;
                //chkAutoAfterXBattlesMessage.Checked = false;

                //chkAutoOnStartUpMessage.Checked = false;
               

                txtAutoAfterXHours.EditValueChanged -= txtAutoAfterXHours_EditValueChanged;
                txtAutoAfterXHours.Text = "2";
                txtAutoAfterXHours.EditValueChanged += txtAutoAfterXHours_EditValueChanged;
                txtAutoAfterXHours.Enabled = false;
               // chkAutoAfterXHoursMessage.Checked = false;
            }

            if (!chkAutoAfterXHours.Checked)
            {
                txtAutoAfterXHours.EditValueChanged -= txtAutoAfterXHours_EditValueChanged;
                txtAutoAfterXHours.Text = "2";
                txtAutoAfterXHours.EditValueChanged += txtAutoAfterXHours_EditValueChanged;
                txtAutoAfterXHours.Enabled = false;
                chkAutoAfterXHoursMessage.Checked = false;
                
            }

            if (!chkAutoOnStartUp.Checked)
            {
                chkAutoOnStartUpMessage.Checked = false;
                
            }

            if (!chkAutoAfterXBattles.Checked)
            {
                txtAutoAfterXBattles.EditValueChanged -= txtAutoAfterXHours_EditValueChanged;
                txtAutoAfterXBattles.Text = "10";
                txtAutoAfterXBattles.EditValueChanged += txtAutoAfterXHours_EditValueChanged;
                txtAutoAfterXBattles.Enabled = false;
                chkAutoAfterXBattlesMessage.Checked = false;
            }


            chkAutoAfterXBattlesMessage.Enabled = chkAutoAfterXBattles.Checked;
            chkAutoOnStartUpMessage.Enabled = chkAutoOnStartUp.Checked;
            chkAutoAfterXHoursMessage.Enabled = chkAutoAfterXHours.Checked;

            if (!chkAutoStartSession.Checked)
            {
                chkAutoAfterXBattlesMessage.Enabled = false;
                chkAutoOnStartUpMessage.Enabled = false;
                chkAutoAfterXHoursMessage.Enabled = false;
            }

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (!CHKBOX_COMPARE.Checked)
            {
                UserSettings.LastPlayedCompare = 0;
            }
            else
            {
                if (radioGroup1.EditValue.ToString() == "0")
                {
                    UserSettings.LastPlayedCompare = 1;
                }
                else
                {
                    UserSettings.LastPlayedCompare = 2;
                }
            }

            UserSettings.LastPlayedCompareQuota = int.Parse(textLastPlayedMaxNo.Text);

        }

        private void textLastPlayedMaxNo_EditValueChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("LastPlayedCompareQuota", out field);
            int y;
            if (!string.IsNullOrEmpty(textLastPlayedMaxNo.Text))
            {
                if (int.TryParse(textLastPlayedMaxNo.Text, out y))
                    if (ApplicationSettings.MaxNoGamesAllowedRB < y)
                    {
                        field.NewValue = ApplicationSettings.MaxNoGamesAllowedRB;
                    }
                    else
                        field.NewValue = y;
                else
                {
                    textLastPlayedMaxNo_InvalidEntry(int.MaxValue);
                    field.NewValue = int.MaxValue.ToString();
                }
            }
        }

        private void textLastPlayedMaxNo_InvalidEntry(int value)
        {
            DevExpress.XtraEditors.XtraMessageBox.Show(this, Translations.TranslationGet("RB_INVALIDVALUELIMIT", "DE", "Maximum value for this field is : ") + value, "WOT Statistics", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveSelection();
        }

        private void SaveSelection()
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("LastPlayedCompare", out field);
            if (!CHKBOX_COMPARE.Checked)
            {
                field.NewValue = 0;
            }
            else
            {
                if (radioGroup1.EditValue.ToString() == "0")
                {
                    field.NewValue = 1;
                }
                else
                {
                    field.NewValue = 2;
                }
            }
        }

        private void chkAutoAfterXHours_CheckedChanged(object sender, EventArgs e)
        {
            SetAutoStartOptions();

            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("AutoSessionXHours", out field);
            field.NewValue = chkAutoAfterXHours.Checked;
        }

        private void chkAutoOnStartUp_CheckedChanged(object sender, EventArgs e)
        {
            SetAutoStartOptions();
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("AutoSessionOnStartUp", out field);
            field.NewValue = chkAutoOnStartUp.Checked;
        }

        private void chkAutoAfterXBattles_CheckedChanged(object sender, EventArgs e)
        {
            SetAutoStartOptions();
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("AutoSessionXBattles", out field);
            field.NewValue = chkAutoAfterXBattles.Checked;
        }

        private void txtAutoAfterXHours_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar > 31 && (e.KeyChar < '0' || e.KeyChar > '9'))
            {
                e.Handled = true;
            }
        }

        private void txtAutoAfterXBattles_EditValueChanged(object sender, EventArgs e)
        {
            //Form pForm = ParentForm;
            //PropertyFields field;
            //((frmSetup)pForm)._propertyFields.TryGetValue("AutoSessionXBattlesValue", out field);
            //int y;
            //if (!string.IsNullOrEmpty(txtAutoAfterXBattles.Text))
            //{
            //    if (int.TryParse(txtAutoAfterXBattles.Text, out y))
            //        if (y >= 10 && y <= 1000)
            //            field.NewValue = y;
            //        else
            //        {
            //            txtAutoAfterXBattles.EditValueChanged -= txtAutoAfterXBattles_EditValueChanged;
            //            txtAutoAfterXBattles.Text = "";
            //            txtAutoAfterXBattles.EditValueChanged -= txtAutoAfterXBattles_EditValueChanged;
            //            field.NewValue = "0";
            //            DevExpress.XtraEditors.XtraMessageBox.Show(this, Translations.TranslationGet("RB_INVALIDXBATENTERED", "DE", "Invalid value entered. Please enter any value between 10 and 1000 battles."), "WOT Statistics", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            txtAutoAfterXBattles.SelectAll();
            //        }
            //    else
            //    {
            //        txtAutoAfterXBattles.EditValueChanged -= txtAutoAfterXHours_EditValueChanged;
            //        txtAutoAfterXBattles.Text = "";
            //        txtAutoAfterXBattles.EditValueChanged += txtAutoAfterXHours_EditValueChanged;
            //        field.NewValue = "0";
            //        txtAutoAfterXBattles.SelectAll();
            //    }
            //}
        }

        private void txtAutoAfterXBattles_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar > 31 && (e.KeyChar < '0' || e.KeyChar > '9'))
            {
                e.Handled = true;
            }
        }

        private void txtAutoAfterXHours_EditValueChanged(object sender, EventArgs e)
        {

            //Form pForm = ParentForm;
            //PropertyFields field;
            //((frmSetup)pForm)._propertyFields.TryGetValue("AutoSessionXHoursValue", out field);
            //int y;
            //if (!string.IsNullOrEmpty(txtAutoAfterXHours.Text))
            //{
            //    if (int.TryParse(txtAutoAfterXHours.Text, out y))
            //    {

            //        if (y >= 2 && y <= 72)
            //            field.NewValue = y;
            //        else
            //        {
            //            txtAutoAfterXHours.EditValueChanged -= txtAutoAfterXHours_EditValueChanged;
            //            txtAutoAfterXHours.Text = "";
            //            txtAutoAfterXHours.EditValueChanged += txtAutoAfterXHours_EditValueChanged;
            //            field.NewValue = "0";
            //            DevExpress.XtraEditors.XtraMessageBox.Show(this, Translations.TranslationGet("RB_INVALIDXHRSENTERED", "DE", "Invalid value entered. Please enter any value between 2 and 72 hours."), "WOT Statistics", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            txtAutoAfterXHours.SelectAll();
            //        }
            //    }
            //    else
            //    {
            //        txtAutoAfterXHours.EditValueChanged -= txtAutoAfterXHours_EditValueChanged;
            //        txtAutoAfterXHours.Text = "";
            //        txtAutoAfterXHours.EditValueChanged += txtAutoAfterXHours_EditValueChanged;
            //        field.NewValue = "0";
            //        txtAutoAfterXHours.SelectAll();
            //    }
            //}
        }

        private void chkAutoAfterXHoursMessage_CheckedChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("AutoSessionXHoursMessage", out field);
            field.NewValue = chkAutoAfterXHoursMessage.Checked;

        }

        private void chkAutoOnStartUpMessage_CheckedChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("AutoSessionOnStartUpMessage", out field);
            field.NewValue = chkAutoOnStartUpMessage.Checked;

        }

        private void chkAutoAfterXBattlesMessage_CheckedChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("AutoSessionXBattlesMessage", out field);
            field.NewValue = chkAutoAfterXBattlesMessage.Checked;
        }

        private void txtAutoAfterXHours_Leave(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("AutoSessionXHoursValue", out field);
            int y;
            if (!string.IsNullOrEmpty(txtAutoAfterXHours.Text))
            {
                if (int.TryParse(txtAutoAfterXHours.Text, out y))
                {

                    if (y >= 2 && y <= 72)
                        field.NewValue = y;
                    else
                    {
                        txtAutoAfterXHours.EditValueChanged -= txtAutoAfterXHours_EditValueChanged;
                        txtAutoAfterXHours.Text = "2";
                        txtAutoAfterXHours.EditValueChanged += txtAutoAfterXHours_EditValueChanged;
                        field.NewValue = "2";
                        DevExpress.XtraEditors.XtraMessageBox.Show(this, Translations.TranslationGet("RB_INVALIDXHRSENTERED", "DE", "Invalid value entered. Please enter any value between 2 and 72 hours."), "WOT Statistics", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtAutoAfterXHours.Focus();
                        txtAutoAfterXHours.SelectAll();
                    }
                }
                else
                {
                    txtAutoAfterXHours.EditValueChanged -= txtAutoAfterXHours_EditValueChanged;
                    txtAutoAfterXHours.Text = "2";
                    txtAutoAfterXHours.EditValueChanged += txtAutoAfterXHours_EditValueChanged;
                    field.NewValue = "2";
                    txtAutoAfterXHours.Focus();
                    txtAutoAfterXHours.SelectAll();
                }
            }
        }

        private void txtAutoAfterXBattles_Leave(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("AutoSessionXBattlesValue", out field);
            int y;
            if (!string.IsNullOrEmpty(txtAutoAfterXBattles.Text))
            {
                if (int.TryParse(txtAutoAfterXBattles.Text, out y))
                    if (y >= 10 && y <= 1000)
                        field.NewValue = y;
                    else
                    {
                        txtAutoAfterXBattles.EditValueChanged -= txtAutoAfterXBattles_EditValueChanged;
                        txtAutoAfterXBattles.Text = "10";
                        txtAutoAfterXBattles.EditValueChanged -= txtAutoAfterXBattles_EditValueChanged;
                        field.NewValue = "10";
                        DevExpress.XtraEditors.XtraMessageBox.Show(this, Translations.TranslationGet("RB_INVALIDXBATENTERED", "DE", "Invalid value entered. Please enter any value between 10 and 1000 battles."), "WOT Statistics", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtAutoAfterXBattles.Focus();
                        txtAutoAfterXBattles.SelectAll();
                    }
                else
                {
                    txtAutoAfterXBattles.EditValueChanged -= txtAutoAfterXHours_EditValueChanged;
                    txtAutoAfterXBattles.Text = "10";
                    txtAutoAfterXBattles.EditValueChanged += txtAutoAfterXHours_EditValueChanged;
                    field.NewValue = "10";
                    txtAutoAfterXBattles.Focus();
                    txtAutoAfterXBattles.SelectAll();
                }
            }
        }

        private void chkAutoAfterXHours_CheckedChanged_1(object sender, EventArgs e)
        {

        }


    }
}
