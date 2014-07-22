using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using WOTStatistics.Core;
using DevExpress.XtraEditors;
using System.Drawing;
using System.IO;


namespace WOT.Stats
{
    public partial class CustomGroupings : XtraForm
    {
        MessageQueue _message;
        readonly TankDescriptions _tDescription;
        readonly CountryDescriptions _cDescription;
        readonly TankTypeDescription _typeDescription;
        readonly CustomGrouping _customGrouping;
        private string _ID = "";

    
        public CustomGroupings(MessageQueue message)
        {
            InitializeComponent();
            _message = message;
            _tDescription = new TankDescriptions(_message);
            _cDescription = new CountryDescriptions(_message);
            _typeDescription = new TankTypeDescription(_message);

            _ID = "";

            ListViewSettings();
        }

        public CustomGroupings(MessageQueue message, string groupName)
        {
            InitializeComponent();
            _message = message;
            _tDescription = new TankDescriptions(_message);
            _cDescription = new CountryDescriptions(_message);
            _typeDescription = new TankTypeDescription(_message);
            _customGrouping = new CustomGrouping(_message);
            _ID = groupName.Replace(" ", "_");
            txtGroupName.Text = _customGrouping.Description(_ID);
            ListViewSettings();
        }

        void ListViewSettings()
        {
            tankListView.View = View.Details;
            tankListView.Columns.Add(Translations.TranslationGet("HTML_HEAD_TANK", "DE", "Tank"));
            tankListView.Columns.Add(Translations.TranslationGet("HTML_HEAD_VEHICLECLASS", "DE", "Vehicle Class"));
            tankListView.Columns.Add(Translations.TranslationGet("HTML_HEAD_VEHICLETIER", "DE", "Vehicle Tier"));
            

            tankListView.CheckBoxes = true;
            tankListView.ShowGroups = true;
            tankListView.GridLines = true;

            helpProvider1.HelpNamespace = Path.Combine(WOTHelper.GetEXEPath(), "Help", "WoT_Stats.chm");
            helpProvider1.SetHelpNavigator(this, HelpNavigator.TopicId);
            helpProvider1.SetHelpKeyword(this, "500");
            
        }

        private void CustomGroupings_Load(object sender, EventArgs e)
        {

            Text = Translations.TranslationGet("STR_CUSTGROUPINGS", "DE", "Custom Groupings");
            HTML_GROUPNAME.Text = Translations.TranslationGet("HTML_GROUPNAME", "DE", "Group Name");
            BTN_SAVE.Text = Translations.TranslationGet("BTN_SAVE", "DE", "Save");
            BTN_CLOSE.Text = Translations.TranslationGet("BTN_CLOSE", "DE", "Close");

            FormHelpers.ResizeLables(this.Controls);

            txtGroupName.Left = HTML_GROUPNAME.Left + HTML_GROUPNAME.Width + 5;
            if (this.ClientRectangle.Width - (HTML_GROUPNAME.Left + HTML_GROUPNAME.Width + 10) < txtGroupName.Width)
                txtGroupName.Width = this.ClientRectangle.Width - (HTML_GROUPNAME.Left + HTML_GROUPNAME.Width + 10);

            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageSize = new Size(50, 24);

            foreach (KeyValuePair<int,string> country in _cDescription)
            {
                tankListView.Groups.Add(new ListViewGroup(country.Key.ToString(), country.Value) { HeaderAlignment = HorizontalAlignment.Center });
            }


            foreach (KeyValuePair<TankKey, TankValue> tank in _tDescription.OrderBy(x=>x.Value.Tier).ThenBy(y=>y.Value.TankType).Where(z=>z.Value.Active == true))
            {
                ListViewItem newListViewItem = new ListViewItem(tank.Value.Description, 0, tankListView.Groups[tank.Key.CountryID.ToString()]);
                newListViewItem.Name = tank.Key.CountryID + "_" + tank.Key.TankID;
                ListViewItem.ListViewSubItem subItem1 = new ListViewItem.ListViewSubItem(newListViewItem, _typeDescription.Description(tank.Value.TankType));
                ListViewItem.ListViewSubItem subItem2 = new ListViewItem.ListViewSubItem(newListViewItem, tank.Value.Tier.ToString());
                newListViewItem.SubItems.Add(subItem1);
                newListViewItem.SubItems.Add(subItem2);

                try
                {
                    if (!imageList1.Images.ContainsKey(String.Format("{0}_{1}", tank.Key.CountryID, tank.Key.TankID)))
                        imageList1.Images.Add(String.Format("{0}_{1}", tank.Key.CountryID, tank.Key.TankID), Image.FromFile(String.Format(@"{0}", WOTHelper.GetImagePath(tank.Key.CountryID + "_" + tank.Key.TankID + "_Large.png"))));

                    newListViewItem.ImageKey = String.Format("{0}_{1}", tank.Key.CountryID, tank.Key.TankID);
                }
                catch { }
                tankListView.Items.Add(newListViewItem);
            }


            if (_ID != "")
            {
                List<string> list = _customGrouping.Values(_ID).Split('|').ToList<string>();

                foreach (string item in list)
                {
                    if (tankListView.Items[item] != null)
                    {
                        tankListView.Items[item].Checked = true;
                    }
                }
            }

            
            tankListView.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            tankListView.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            tankListView.Columns[2].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void butSave_Click(object sender, EventArgs e)
        {
            if (txtGroupName.Text != "")
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(WOTHelper.GetUserFile());
                XmlElement root = xmlDoc.DocumentElement;
                XmlNodeList oldCustomGroups = root.SelectNodes(string.Format(@"CustomGroups/CustomGroup[@Name=""{0}""]",_ID.Replace(" ", "_")));
                XmlNode customGroups = root.SelectSingleNode("CustomGroups");
                XmlElement customGroup = null;
                if (oldCustomGroups.Count !=  0)
                {
                    foreach (XmlNode item in oldCustomGroups)
                    {
                        customGroup = (XmlElement)item;
                    }
                    customGroup.SetAttribute("Caption", txtGroupName.Text);
                }
                else
                {

                    if (_ID == "")
                        _ID = Guid.NewGuid().ToString();

                    customGroup = xmlDoc.CreateElement("CustomGroup");
                    customGroup.SetAttribute("Name", _ID);
                    customGroup.SetAttribute("Caption", txtGroupName.Text);
                }

                customGroup.InnerText = "";
                foreach (ListViewItem selected in tankListView.CheckedItems)
                {
                    if (customGroup.InnerText.Length > 0)
                        customGroup.InnerText += "|";

                    customGroup.InnerText += selected.Name;
                }

                if (oldCustomGroups.Count == 0)
                    customGroups.AppendChild(customGroup);

                xmlDoc.Save(WOTHelper.GetUserFile());
                
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(Translations.TranslationGet("STR_CUSTOMGROUPSAVENOTICE", "DE", "Please specify a group name."), "WOT Statistics", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtGroupName.Focus();
                return;
            }
            
            Close();
        }

        private void Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtGroupName_TextChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CustomGroupings_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
            Help.ShowHelp(this, helpProvider1.HelpNamespace, HelpNavigator.TopicId, "260");
            e.Cancel = true;
        }
    }
}
