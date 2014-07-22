using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using WOTStatistics.Core;
using System.Xml;
using System.IO;
using WOTStatistics.SQLite;

namespace WOT.Stats
{
    public partial class frmAddEditPlayer : DevExpress.XtraEditors.XtraForm
    {
        public frmAddEditPlayer()
        {
            InitializeComponent();
            Text = Translations.TranslationGet("WNDCAPTION_ADDPLAYER", "DE", "Add Player");
            helpProvider1.HelpNamespace = Path.Combine(WOTHelper.GetEXEPath(), "Help", "WoT_Stats.chm");
            helpProvider1.SetHelpNavigator(this, HelpNavigator.TopicId);
            helpProvider1.SetHelpKeyword(this, "120");
        }

        public frmAddEditPlayer(Player player)
        {
            InitializeComponent();

            txtWatchFile.Text = player.WatchFile;
            txtPlayerID.Text = player.PlayerID;
            if (player.Monitor.ToUpper() == "YES")
                checkFTPFile.Checked = true;
            else
                checkFTPFile.Checked = false;

            Text = Translations.TranslationGet("WNDCAPTION_EDITPLAYER", "DE", "Edit Player");
            helpProvider1.HelpNamespace = Path.Combine(WOTHelper.GetEXEPath(), "Help", "WoT_Stats.chm");
            helpProvider1.SetHelpNavigator(this, HelpNavigator.TopicId);
            helpProvider1.SetHelpKeyword(this, "120");
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            using (PlayerListing pl = new PlayerListing(new MessageQueue()))
            {

                pl.SetPlayer(txtPlayerID.Text, txtPlayerID.Text, txtPlayerRealm.Text, txtWatchFile.Text, checkFTPFile.Checked);
                pl.Save();
                if (!File.Exists(Path.Combine(WOTHelper.GetApplicationData(), "Hist_" + txtPlayerID.Text, "LastBattle", "WOTSStore.db")))
                {
                    Directory.CreateDirectory(Path.Combine(WOTHelper.GetApplicationData(), "Hist_" + txtPlayerID.Text, "LastBattle"));
                    DatabaseSanityChecker.Create(Path.Combine(WOTHelper.GetApplicationData(), "Hist_" + txtPlayerID.Text, "LastBattle", "WOTSStore.db"));
                }
            }
            Close();
        }

        private void txtWatchFile_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            using (OpenFileDialog fbd = new OpenFileDialog())
            {
                fbd.Filter = "World of Tanks Dossier File (*.dat)|*.dat";

                //doing a quick check that the directory actually exists before setting it as the default dir. else just let it open where ever it was last.
                if (Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"wargaming.net\WorldOfTanks\dossier_cache")))
                {
                    fbd.InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"wargaming.net\WorldOfTanks\dossier_cache");
                }

                if (fbd.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        txtWatchFile.Text = fbd.FileName;
                        
                        Encoding byteConverter = Encoding.GetEncoding("UTF-8");
                        txtPlayerID.Text = byteConverter.GetString(Base32.Decode(fbd.SafeFileName.Remove(fbd.SafeFileName.IndexOf('.')).ToLower())).Split(';')[1];
                        string sServer = byteConverter.GetString(Base32.Decode(fbd.SafeFileName.Remove(fbd.SafeFileName.IndexOf('.')).ToLower())).Split(';')[0];
                        txtPlayerRealm.Text = getServerRealm(sServer);
                    }
                    catch 
                    {
                        try
                        {
                            txtWatchFile.Text = fbd.FileName;
                            
                            Encoding byteConverter = Encoding.GetEncoding("UTF-8");
                            txtPlayerID.Text = byteConverter.GetString(Base32.Decode(fbd.SafeFileName.Remove(fbd.SafeFileName.IndexOf('.')))).Split(';')[1];
                            string sServer = byteConverter.GetString(Base32.Decode(fbd.SafeFileName.Remove(fbd.SafeFileName.IndexOf('.')))).Split(';')[0];


                            txtPlayerRealm.Text = getServerRealm(sServer);

      

                        }
                        catch (Exception ex)
                        {
                            txtWatchFile.Text = string.Empty;
                            txtPlayerID.Text = string.Empty;
                            txtPlayerRealm.Text = string.Empty;
                            DevExpress.XtraEditors.XtraMessageBox.Show("Invalid dossier file selected. Please select valid dossier file." + Environment.NewLine + "Select File : " + fbd.FileName + Environment.NewLine + "Message : " + ex.Message + Environment.NewLine + "Stack Track : " + ex.StackTrace, "Dossier File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private string getServerRealm(string sServer)
        {
            if (sServer.Contains("worldoftanks"))
            {
                sServer = sServer.Substring(sServer.IndexOf("worldoftanks"));
            }

            if (sServer.Contains(":"))
            {
                sServer = sServer.Substring(0, sServer.IndexOf(":"));
            }

            return sServer;

        }

        private void txtPlayerID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void frmAddEditPlayer_HelpButtonClicked(object sender, CancelEventArgs e)
        {

            
            Help.ShowHelp(this, helpProvider1.HelpNamespace, HelpNavigator.TopicId, "120");
            e.Cancel = true;
        }

        private void frmAddEditPlayer_Load(object sender, EventArgs e)
        {
            STR_DOSSIERFILE.Text = Translations.TranslationGet("STR_DOSSIERFILE", "DE", "Dossier File") + " :";
            STR_PLAYERID.Text = Translations.TranslationGet("STR_PLAYERID", "DE", "Player ID") + " :";
            STR_PLAYERREALM.Text = Translations.TranslationGet("STR_PLAYERREALM", "DE", "Player Realm") + " :";
            CHKBOX_FTPFILE.Text = Translations.TranslationGet("CHKBOX_FTPFILE", "DE", "Shared File") + " :";

            FormHelpers.ResizeLables(this.Controls);

            txtWatchFile.Left = STR_DOSSIERFILE.Left + STR_DOSSIERFILE.Width + 5;
            txtPlayerID.Left = txtWatchFile.Left;
            txtPlayerRealm.Left = txtWatchFile.Left;
            checkFTPFile.Left = txtWatchFile.Left;

            if (this.ClientRectangle.Width - (STR_DOSSIERFILE.Left + STR_DOSSIERFILE.Width + 10) < txtWatchFile.Width)
                txtWatchFile.Width = this.ClientRectangle.Width - (STR_DOSSIERFILE.Left + STR_DOSSIERFILE.Width + 10);

            if (this.ClientRectangle.Width - (STR_DOSSIERFILE.Left + STR_DOSSIERFILE.Width + 10) < txtPlayerID.Width)
                txtPlayerID.Width = this.ClientRectangle.Width - (STR_DOSSIERFILE.Left + STR_DOSSIERFILE.Width + 10);

            if (this.ClientRectangle.Width - (STR_DOSSIERFILE.Left + STR_DOSSIERFILE.Width + 10) < CHKBOX_FTPFILE.Width)
                checkFTPFile.Width = this.ClientRectangle.Width - (STR_DOSSIERFILE.Left + STR_DOSSIERFILE.Width + 10);


            BTN_SAVE.Text = Translations.TranslationGet("BTN_SAVE", "DE", "Save");
            BTN_CANCEL.Text = Translations.TranslationGet("BTN_CANCEL", "DE", "Cancel");
        }

        private void labelControl3_Click(object sender, EventArgs e)
        {

        }

        private void labelControl2_Click(object sender, EventArgs e)
        {

        }

        private void CHKBOX_FTPFILE_Resize(object sender, EventArgs e)
        {
            checkFTPFile.Left = CHKBOX_FTPFILE.Width + 10;
        }

        private void txtWatchFile_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}