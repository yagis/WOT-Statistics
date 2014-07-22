using System;
using System.Linq;
using System.Windows.Forms;
using WOTStatistics.Core;
using System.IO;

namespace WOT.Stats
{
    public partial class CompareSelection : DevExpress.XtraEditors.XtraForm
    {
        MessageQueue _message;
        private DossierManager _dm;
        private PlayerListing _players;

        public CompareSelection(MessageQueue message,  DossierManager dm)
        {
            InitializeComponent();
            _message = message;
            _dm = dm;
            _players = new PlayerListing(_message);
            SetValues();
            Text = _dm.GetPlayerName + " " + Translations.TranslationGet("WNDCAPTION_PERSELECT", "DE", "Period Selection");
        }

        private void SetValues()
        {
            Player pl = _players.GetPlayer(_dm.GetPlayerID);

            if (new string[] { "0", "1", "2", "3" }.Contains(pl.PreviousFile))
                OldDossier.EditValue = pl.PreviousFile;
            else
            {
                OldDossier.EditValue = "4";
                oldSelection.Text = pl.PreviousFile.Insert(4, "-").Insert(7, "-");
            }

            if (new string[] { "0", "1", "2", "3" }.Contains(pl.CurrentFile))
                NewDossier.EditValue = pl.CurrentFile;
            else
            {
                NewDossier.EditValue = "4";
                newSelection.Text = pl.CurrentFile.Insert(4, "-").Insert(7, "-");
            }
            helpProvider1.HelpNamespace = Path.Combine(WOTHelper.GetEXEPath(), "Help", "WoT_Stats.chm");
            helpProvider1.SetHelpNavigator(this, HelpNavigator.TopicId);
            helpProvider1.SetHelpKeyword(this, "420");
        }

        private void CompareSelection_Load(object sender, EventArgs e)
        {
            STR_OLDDOSSIER.Text = Translations.TranslationGet("STR_OLDDOSSIER","DE", "Old Dossier");
            STR_NEWDOSSIER.Text = Translations.TranslationGet("STR_NEWDOSSIER", "DE", "New Dossier");

            OldDossier.Properties.Items[0].Description = Translations.TranslationGet("STR_CURRENT", "DE", "Current");
            OldDossier.Properties.Items[1].Description = Translations.TranslationGet("STR_PREVIOUS", "DE", "Previous");
            OldDossier.Properties.Items[2].Description = "1 " + Translations.TranslationGet("STR_WEEK", "DE", "Week");
            OldDossier.Properties.Items[3].Description = "2 " + Translations.TranslationGet("STR_WEEKS", "DE", "Weeks");
            OldDossier.Properties.Items[4].Description = Translations.TranslationGet("STR_CUSTOM", "DE", "Custom");

            NewDossier.Properties.Items[0].Description = Translations.TranslationGet("STR_CURRENT", "DE", "Current");
            NewDossier.Properties.Items[1].Description = Translations.TranslationGet("STR_PREVIOUS", "DE", "Previous");
            NewDossier.Properties.Items[2].Description = "1 " + Translations.TranslationGet("STR_WEEK", "DE", "Week");
            NewDossier.Properties.Items[3].Description = "2 " + Translations.TranslationGet("STR_WEEKS", "DE", "Weeks");
            NewDossier.Properties.Items[4].Description = Translations.TranslationGet("STR_CUSTOM", "DE", "Custom");

            BTN_OK.Text = Translations.TranslationGet("BTN_OK", "DE", "Ok");
            BTN_CLOSE.Text = Translations.TranslationGet("BTN_CLOSE", "DE", "Close");
            BTN_USEDEFAULT.Text = Translations.TranslationGet("BTN_USEDEFAULT", "DE", "Use Default");

            oldSelection.Properties.Items.AddRange(_dm.GetAllFilesForPlayerFriendly().Values.OrderByDescending(x => x).ToArray());
            newSelection.Properties.Items.AddRange(_dm.GetAllFilesForPlayerFriendly().Values.OrderByDescending(x => x).ToArray());

            if (OldDossier.EditValue.ToString() == "4")
                oldSelection.Enabled = true;
            else
                oldSelection.Enabled = false;

            if (NewDossier.EditValue.ToString() == "4")
                newSelection.Enabled = true;
            else
                newSelection.Enabled = false;
        }

        private void butClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void butApply_Click(object sender, EventArgs e)
        {
            string prevF = "0";
            string currF = "0";

            prevF = (string)OldDossier.EditValue;

            if (prevF == "4")
            {
                if (oldSelection.Text != "")
                    prevF = oldSelection.Text.Replace("-","");
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Please select date.");
                    return;
                }
            }

            currF = (string)NewDossier.EditValue;
            if (currF == "4")
            {
                if (newSelection.Text != "")
                    currF = newSelection.Text.Replace("-", "");
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Please select date.");
                    return;
                }
            }

            _players.SetPlayer(_dm.GetPlayerID, prevF, currF);
            _players.Save();
            Close();
        }

        private void newSelection_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void oldSelection_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void OldDossier_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OldDossier.EditValue.ToString() == "4")
                oldSelection.Enabled = true;
            else
                oldSelection.Enabled = false;
        }

        private void NewDossier_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NewDossier.EditValue.ToString() == "4")
                newSelection.Enabled = true;
            else
                newSelection.Enabled = false;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            NewDossier.EditValue = "0";
            OldDossier.EditValue = "1";
        }

        private void CompareSelection_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
            Help.ShowHelp(this, helpProvider1.HelpNamespace, HelpNavigator.TopicId, "420");
            e.Cancel = true;
        }
    }
}
