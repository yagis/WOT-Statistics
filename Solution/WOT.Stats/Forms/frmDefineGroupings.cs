using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using WOTStatistics.Core;
using System.IO;

namespace WOT.Stats
{
    public partial class frmDefineGroupings : DevExpress.XtraEditors.XtraForm
    {
        public frmDefineGroupings()
        {
            InitializeComponent();
            helpProvider1.HelpNamespace = Path.Combine(WOTHelper.GetEXEPath(), "Help", "WoT_Stats.chm");
            helpProvider1.SetHelpNavigator(this, HelpNavigator.TopicId);
            helpProvider1.SetHelpKeyword(this, "500");
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            using (CustomGroupings frm = new CustomGroupings(new MessageQueue()))
            {
                frm.ShowDialog();
            }
            Refresh();
        }

        private void frmDefineGroupings_Load(object sender, EventArgs e)
        {
            Text = Translations.TranslationGet("STR_DEFGROUPINGS", "DE", "Define Groupings");
            BTN_ADD.Text = Translations.TranslationGet("BTN_ADD", "DE", "Add");
            BTN_EDIT.Text = Translations.TranslationGet("BTN_EDIT", "DE", "Edit");
            BTN_REMOVE.Text = Translations.TranslationGet("BTN_REMOVE", "DE", "Remove");
            BTN_CLOSE.Text = Translations.TranslationGet("BTN_CLOSE", "DE", "Close");

            Refresh();
            

        }

        private new void Refresh()
        {
            listView1.View = View.List;
            listView1.Clear();
            CustomGrouping cg = new CustomGrouping(new MessageQueue());
            foreach (KeyValuePair<string, Tuple<string, string>> group in cg)
            {
                try
                {
                    listView1.Items.Add(new ListViewItem() { Name = group.Key, Text = group.Value.Item1 });
                }
                catch{}
            }

            BTN_EDIT.Enabled = (listView1.Items.Count > 0) && (listView1.SelectedItems.Count > 0);
            BTN_REMOVE.Enabled = (listView1.Items.Count > 0) && (listView1.SelectedItems.Count > 0);

            //try
            //{
            //    listView1.Items[0].Selected = true;
            //}
            //catch { }
            
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {

            using (CustomGroupings frm = new CustomGroupings(new MessageQueue(), listView1.SelectedItems[0].Name))
                {
                    frm.ShowDialog();
                }
                Refresh();
           
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.DialogResult result = DevExpress.XtraEditors.XtraMessageBox.Show(Translations.TranslationGet("STR_DEFINEGROUPREMOVENOTICCE", "DE", "Are you sure you want to remove grouping:") + " " + listView1.SelectedItems[0].Text + "?", "WOT Statistics", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            // Confirm user wants to delete
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                CustomGrouping cg = new CustomGrouping(new MessageQueue());
                cg.Remove(listView1.SelectedItems[0].Name);
                listView1.SelectedItems[0].Remove();
                Refresh();
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmDefineGroupings_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            
            Help.ShowHelp(this, helpProvider1.HelpNamespace, HelpNavigator.TopicId, "500");
            e.Cancel = true;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BTN_EDIT.Enabled = (listView1.Items.Count > 0) && (listView1.SelectedItems.Count > 0);
            BTN_REMOVE.Enabled = (listView1.Items.Count > 0) && (listView1.SelectedItems.Count > 0);
        }
    }
}