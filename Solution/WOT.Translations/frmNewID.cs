using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;

namespace WOT.Translations
{
    public partial class frmNewID : DevExpress.XtraEditors.XtraForm
    {
        private readonly string _file;
        private Form1 _parent;

        public frmNewID(string file, Form1 parent)
        {
            InitializeComponent();
            _file = file;
            _parent = parent;
        }

        private void labelControl2_Click(object sender, EventArgs e)
        {

        }

        private void butClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void butSave_Click(object sender, EventArgs e)
        {
            Translations tr = new Translations(_file);
            textLangID.Text = textLangID.Text.ToUpper();
            if (tr.LanguageGet(textLangID.Text, Language.English) != "Unknown")
                DevExpress.XtraEditors.XtraMessageBox.Show(textLangID.Text + " has already been allocated. [" + textLangID.Text + " - " + tr.LanguageGet(textLangID.Text, Language.English) + "]");
            else
            {
                DataTable dt = _parent._dt;
                DataColumn dc = dt.Columns.Add(textLangID.Text, typeof(string));
                GridControl grid =  (GridControl)_parent.Controls["gridControl1"];
                GridView view = (GridView)grid.MainView;
                view.BeginUpdate();
                view.Columns.AddVisible(dc.ColumnName, textEngDesc.Text + " - " + textNatDesc.Text);
                view.EndUpdate();
                this.Close();
            }

        }
    }
}