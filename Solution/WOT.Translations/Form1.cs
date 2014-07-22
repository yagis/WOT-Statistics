using System;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace WOT.Translations
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        string _file = "";
        public DataTable _dt { get; set; }
        public Form1()
        {
            InitializeComponent();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                _file = ofd.FileName;
                PopulateGrid();
                barButtonItem2.Enabled = true;
                barButtonItem4.Enabled = true;
            }
        }

        private void PopulateGrid()
        {
            ((GridView)gridControl1.MainView).Columns.Clear();
            Translations tr = new Translations(_file);
            _dt = tr.GetData();
            foreach (DataColumn item in _dt.Columns)
            {
                GridColumn column = ((GridView)gridControl1.MainView).Columns.Add();
                column.Caption = item.Caption;
                column.FieldName = item.ColumnName;
                column.Visible = true;

#if DEBUG
#else
                if (column.FieldName == "Key" || column.FieldName == "ENG")
                {
                    column.OptionsColumn.AllowEdit = false;
                    column.OptionsColumn.AllowShowHide = false;
                    column.OptionsColumn.ReadOnly = true;
                }
#endif
            }

            gridControl1.DataSource = _dt;
            
        }

 

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                XmlDeclaration dec = xDoc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                xDoc.AppendChild(dec);
                XmlElement worldoftanks = xDoc.CreateElement("WorldOfTanks");
                XmlElement translations = xDoc.CreateElement("Translation");
                XmlElement languages = xDoc.CreateElement("Languages");
                foreach (GridColumn item in ((GridView)gridControl1.MainView).Columns)
                {
                    if (item.FieldName != "Key")
                    {
                        XmlElement langID = xDoc.CreateElement("Lang");
                        langID.SetAttribute("ID", item.FieldName);

                        XmlElement langItemENG = xDoc.CreateElement("NameENG");
                        langItemENG.InnerText = item.Caption.Split('-')[0].Trim();
                        langID.AppendChild(langItemENG);

                        XmlElement langItemNAT = xDoc.CreateElement("NameNat");
                        langItemNAT.InnerText = item.Caption.Split('-')[1].Trim();
                        langID.AppendChild(langItemNAT);

                        languages.AppendChild(langID);
                    }
                }
                translations.AppendChild(languages);

                XmlElement fields = xDoc.CreateElement("Fields");


                for (int i = 0; i < gridControl1.MainView.RowCount; i++)
                {
                    XmlElement field = xDoc.CreateElement("Field");
                    field.SetAttribute("Name", gridView1.GetRowCellValue(i, "Key").ToString());
                    field.SetAttribute("DefaultLanID", "ENG");
                    

                    foreach (GridColumn item in ((GridView)gridControl1.MainView).Columns)
                    {
                        if (item.FieldName != "Key" & item.Visible == true)
                        {
                            XmlElement fieldItem = xDoc.CreateElement("Item");
                            fieldItem.SetAttribute("LanID", item.FieldName);
                            fieldItem.InnerText = gridView1.GetRowCellValue(i, item.FieldName).ToString();
                            if (!string.IsNullOrEmpty(fieldItem.InnerText))
                                field.AppendChild(fieldItem);
                        }
                    }

                    fields.AppendChild(field);

                }

                translations.AppendChild(fields);
                worldoftanks.AppendChild(translations);

                xDoc.AppendChild(worldoftanks);

                xDoc.Save(_file);

                DevExpress.XtraEditors.XtraMessageBox.Show("File has been saved." + Environment.NewLine + _file, "WOT Statistics Translation");
            }
            catch (Exception ex)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Error saving file." + Environment.NewLine + ex.Message, "WOT Statistics Translation");
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (frmNewID form = new frmNewID(_file, this))
            {
                form.ShowDialog(this);
            }
        }

        private void btnVisitGitHub_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://github.com/Phalynx/WOTStatistics/blob/master/translations.xml");
            }
            catch
            {

            }
        }
    }
}
