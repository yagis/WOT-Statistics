using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WOTStatistics.Core;
using DevExpress.XtraEditors;
using System.IO;

namespace WOT.Stats
{
    public partial class GraphSetup : XtraForm
    {
        MessageQueue _message;

        TankDescriptions _tDescription;
        CountryDescriptions _cDescription;
        TankTypeDescription _typeDescription;
        GraphsSettings _graphSettings;
        private string _ID = "";

        public GraphSetup(MessageQueue message)
        {
            InitializeComponent();

            _message = message;
            
            _tDescription = new TankDescriptions(_message);
            _cDescription = new CountryDescriptions(_message);
            _typeDescription = new TankTypeDescription(_message);
            _graphSettings = new GraphsSettings(_message);

            helpProvider1.HelpNamespace = Path.Combine(WOTHelper.GetEXEPath(), "Help", "WoT_Stats.chm");

            helpProvider1.SetHelpNavigator(this, HelpNavigator.TopicId);
            helpProvider1.SetHelpKeyword(this, "520");

            ListViewSettings();
            LoadSettings();
        }


        public GraphSetup(MessageQueue message, string graphID)
        {
            InitializeComponent();

            _message = message;
            _ID = graphID;
            //textName.Text = graphName;
            //textName.Enabled = false;

            _tDescription = new TankDescriptions(_message);
            _cDescription = new CountryDescriptions(_message);
            _typeDescription = new TankTypeDescription(_message);
            _graphSettings = new GraphsSettings(_message);

            ListViewSettings();
            LoadSettings();
        }

        private void LoadSettings()
        {
            GraphsSettings gs = new GraphsSettings(_message);
            GraphFields gf = gs.FieldValues(_ID);

            textName.Text = gf.Caption;

            radioGroup1.EditValue = gf.StatsBase;

            if (gf.StatsBase == "Tanks")
            {
                Width = 684;
                Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - Width / 2, Screen.PrimaryScreen.WorkingArea.Height / 2 - Height / 2);
            }

            radioGroupPeriod.EditValue = gf.Period.ToString();

            comboDataField.Text = gf.DataField;
        }

        private void butSave_Click(object sender, EventArgs e)
        {
            if (textName.Text != "" && comboDataField.Text != "")
            {
                if (radioGroup1.EditValue.ToString() == "Tanks" && listTanks.CheckedItems.Count == 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(Translations.TranslationGet("STR_CHARTTANKSAVENOTICE", "DE", "Please select tank/s to be displayed in the chart."), "WOT Statistics", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (_ID == "")
                        _ID = Guid.NewGuid().ToString();
                    GraphsSettings gs = new GraphsSettings(_message);
                    GraphFields gf = new GraphFields()
                    {
                        Caption = textName.Text,
                        Type = "Line",
                        DataField = comboDataField.Text,
                        Name = _ID,
                        StatsBase = radioGroup1.EditValue.ToString(),
                        InnerText = CreateInnerText(),
                        Period = int.Parse(radioGroupPeriod.EditValue.ToString())
                    };
                    gs.Save(gf);
                    Close();
                }
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(Translations.TranslationGet("STR_CHARTFIELDSSAVENOTICE", "DE", "Please complete all fields."), "WOT Statistics", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private string CreateInnerText()
        {
            string retValue = "";
            if (radioGroup1.EditValue.ToString() == "Tanks")
            {
                foreach (ListViewItem selected in listTanks.CheckedItems)
                {
                    if (retValue.Length > 0)
                        retValue += "|";

                    retValue += selected.Name;
                }
            }
            return retValue;
        }


        private void butClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void GraphSetup_Load(object sender, EventArgs e)
        {
            Width = 274; //223
            Text = Translations.TranslationGet("WIN_CAPSETUPCHARTS", "DE", "Chart Setup");
            STR_GRPSTATSBASE.Text = Translations.TranslationGet("STR_GRPSTATSBASE", "DE", "Statistic Base");
            STR_GRPPERIOD.Text = Translations.TranslationGet("STR_GRPPERIOD", "DE", "Period");
            STR_CATEGORY.Text = Translations.TranslationGet("STR_CATEGORY", "DE", "Category");
            STR_CHARTNAME.Text = Translations.TranslationGet("STR_CHARTNAME", "DE", "Chart Name");

            FormHelpers.ResizeLables(this.Controls);

            textName.Left = STR_CHARTNAME.Left + STR_CHARTNAME.Width + 5;
            comboDataField.Left = textName.Left;

            if (this.ClientRectangle.Width - (STR_CHARTNAME.Left + STR_CHARTNAME.Width + 10) < textName.Width)
                textName.Width = this.ClientRectangle.Width - (STR_CHARTNAME.Left + STR_CHARTNAME.Width + 10 );

            if (this.ClientRectangle.Width - (STR_CHARTNAME.Left + STR_CHARTNAME.Width + 10) < comboDataField.Width)
                comboDataField.Width = this.ClientRectangle.Width - (STR_CHARTNAME.Left + STR_CHARTNAME.Width + 10);

            radioGroup1.Properties.Items[0].Description = Translations.TranslationGet("STR_OVERALL", "DE", "Overall");
            radioGroup1.Properties.Items[1].Description = Translations.TranslationGet("STR_TANKS", "DE", "Tanks");

            radioGroupPeriod.Properties.Items[0].Description = "1 " + Translations.TranslationGet("STR_WEEK", "DE", "Week");
            radioGroupPeriod.Properties.Items[1].Description = "2 " + Translations.TranslationGet("STR_WEEKS", "DE", "Weeks");
            radioGroupPeriod.Properties.Items[2].Description = "3 " + Translations.TranslationGet("STR_WEEKS", "DE", "Weeks");
            radioGroupPeriod.Properties.Items[3].Description = "4 " + Translations.TranslationGet("STR_WEEKS", "DE", "Weeks");
            radioGroupPeriod.Properties.Items[4].Description = "5 " + Translations.TranslationGet("STR_WEEKS", "DE", "Weeks");
            radioGroupPeriod.Properties.Items[5].Description = "6 " + Translations.TranslationGet("STR_WEEKS", "DE", "Weeks");
            radioGroupPeriod.Properties.Items[6].Description = "3 " + Translations.TranslationGet("STR_MONTHS", "DE", "Months");
            radioGroupPeriod.Properties.Items[7].Description = "6 " + Translations.TranslationGet("STR_MONTHS", "DE", "Months");
            radioGroupPeriod.Properties.Items[8].Description = Translations.TranslationGet("STR_YEAR", "DE", "Year");

            foreach (KeyValuePair<int, string> country in _cDescription)
            {
                listTanks.Groups.Add(new ListViewGroup(country.Key.ToString(), country.Value) { HeaderAlignment = HorizontalAlignment.Center });
            }

            foreach (KeyValuePair<TankKey, TankValue> tank in _tDescription.OrderBy(x => x.Value.Tier).ThenBy(y => y.Value.TankType).Where(z=>z.Value.Active == true))
            {
                ListViewItem newListViewItem = new ListViewItem(tank.Value.Description,
                                                   0,
                                                   listTanks.Groups[tank.Key.CountryID.ToString()])
                                                   {
                                                       Name = String.Format("{0}_{1}", tank.Key.CountryID, tank.Key.TankID)
                                                   };

                ListViewItem.ListViewSubItem subItem1 = new ListViewItem.ListViewSubItem(newListViewItem, _typeDescription.Description(tank.Value.TankType));
                ListViewItem.ListViewSubItem subItem2 = new ListViewItem.ListViewSubItem(newListViewItem, tank.Value.Tier.ToString());

                newListViewItem.SubItems.Add(subItem1);
                newListViewItem.SubItems.Add(subItem2);
                listTanks.Items.Add(newListViewItem);
            }

            if (textName.Text != "")
            {
                List<string> list = _graphSettings.InnerText(_ID).Split('|').ToList<string>();

                foreach (string item in list)
                {
                    if (listTanks.Items[item] != null)
                    {
                        listTanks.Items[item].Checked = true;
                    }
                }
            }

            listTanks.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            listTanks.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            listTanks.Columns[2].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);

            if (radioGroup1.EditValue.ToString() == "Tanks")
            {
                Width = 684;
                Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - Width / 2, Screen.PrimaryScreen.WorkingArea.Height / 2 - Height / 2);
            }
            else
            { 
                Width = 274; //223
            }
        }

        void ListViewSettings()
        {
            listTanks.View = View.Details;
            listTanks.Columns.Add(Translations.TranslationGet("HTML_HEAD_TANK", "DE", "Tank"));
            listTanks.Columns.Add(Translations.TranslationGet("HTML_HEAD_VEHICLECLASS", "DE", "Vehicle Class"));
            listTanks.Columns.Add(Translations.TranslationGet("HTML_HEAD_VEHICLETIER", "DE", "Vehicle Tier"));


            listTanks.CheckBoxes = true;
            listTanks.ShowGroups = true;
            listTanks.GridLines = true;
        }

        private void radioTypeBar_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioBaseTank_CheckedChanged(object sender, EventArgs e)
        {
            //listTanks.Visible = radioBaseTank.Checked;
            //Width = 640;
        }

        private void radioBaseOverall_CheckedChanged(object sender, EventArgs e)
        {
            Width = 274;
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup1.EditValue.ToString() == "Tanks")
            {
                Width = 684;
                Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - Width / 2, Screen.PrimaryScreen.WorkingArea.Height / 2 - Height / 2);
            }
            else
            {
                Width = 274; //223
                Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - Width / 2, Screen.PrimaryScreen.WorkingArea.Height / 2 - Height / 2);
            }
        }

        private void GraphSetup_ResizeEnd(object sender, EventArgs e)
        {
           
        }

        private void GraphSetup_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            
            Help.ShowHelp(this, helpProvider1.HelpNamespace,HelpNavigator.TopicId, "520");
            e.Cancel = true;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
