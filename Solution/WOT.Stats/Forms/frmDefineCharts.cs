using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using WOTStatistics.Core;
using DevExpress.XtraEditors.Controls;
using System.IO;

namespace WOT.Stats
{
    public partial class frmDefineCharts : DevExpress.XtraEditors.XtraForm
    {
        public frmDefineCharts(MessageQueue message)
        {
            InitializeComponent();
            helpProvider1.HelpNamespace = Path.Combine(WOTHelper.GetEXEPath(), "Help", "WoT_Stats.chm");
            helpProvider1.SetHelpNavigator(this, HelpNavigator.TopicId);
            helpProvider1.SetHelpKeyword(this, "520");
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.DialogResult result = DevExpress.XtraEditors.XtraMessageBox.Show(Translations.TranslationGet("", "DE", "Are you sure you want to remove chart:") + " " + listView1.SelectedItems[0].Text + "?", "WOT Statistics", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            // Confirm user wants to delete
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                GraphsSettings cg = new GraphsSettings(new MessageQueue());
                cg.Remove(listView1.SelectedItems[0].Name);
                listView1.SelectedItems[0].Remove();
                //listView1.Items.Remove(listView1.SelectedIndices[0]);
                Refresh();
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            using (GraphSetup frm = new GraphSetup(new MessageQueue(), listView1.SelectedItems[0].Name))
            {
                frm.ShowDialog();
            }
            Refresh();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            using (GraphSetup frm = new GraphSetup(new MessageQueue()))
            {
                frm.ShowDialog();
            }
            Refresh();
        }

        private void frmDefineCharts_Load(object sender, EventArgs e)
        {
            Text = Translations.TranslationGet("STR_DEFCHARTS", "DE", "Define Charts");
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
            GraphsSettings cg = new GraphsSettings(new MessageQueue());
            foreach (KeyValuePair<string, GraphFields> group in cg)
            {
                try
                {
                    listView1.Items.Add(new ListViewItem() { Name = group.Key, Text = group.Value.Caption});
                }
                catch { }
            }

            BTN_EDIT.Enabled = (listView1.Items.Count > 0) && (listView1.SelectedItems.Count > 0);
            BTN_REMOVE.Enabled = (listView1.Items.Count > 0) && (listView1.SelectedItems.Count > 0);
            //try
            //{
            //    listView1.Items[0].Selected = true;
            //}
            //catch {            }
        }

        private void frmDefineCharts_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            
            Help.ShowHelp(this, helpProvider1.HelpNamespace, HelpNavigator.TopicId, "520");
            e.Cancel = true;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BTN_EDIT.Enabled = (listView1.Items.Count > 0) && (listView1.SelectedItems.Count > 0);
            BTN_REMOVE.Enabled = (listView1.Items.Count > 0) && (listView1.SelectedItems.Count > 0);
        }
    }
}