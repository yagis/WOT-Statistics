using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using WOTStatistics.Core;
using System.Xml;
using System.Linq;

namespace WOT.Stats
{
    public partial class ctxSetup : DevExpress.XtraEditors.XtraUserControl
    {
        public ctxSetup()
        {
            InitializeComponent();
        }

        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void labelControl1_Click(object sender, EventArgs e)
        {

        }

        private void textEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar > 31 && (e.KeyChar < '0' || e.KeyChar > '9'))
            {
                e.Handled = true;
            }
        }

        private void textEdit2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar > 31 && (e.KeyChar < '0' || e.KeyChar > '9'))
            {
                e.Handled = true;
            }
        }

        private void ctxMiscellaneous_Load(object sender, EventArgs e)
        {
            STR_FONTSIZE.Text = Translations.TranslationGet("STR_FONTSIZE", "DE", "Font Size");
            STR_HEADING.Text = Translations.TranslationGet("STR_HEADING", "DE", "HTML Heading :");
            STR_DETAIL.Text = Translations.TranslationGet("STR_DETAIL", "DE", "HTML Detail :");
            STR_IMAGES.Text = Translations.TranslationGet("STR_IMAGES", "DE", "Images");
            CHKBOX_SHOWICONS.Text = Translations.TranslationGet("CHKBOX_SHOWICONS", "DE", "Show Thumbs Up/Down Icons");
            STR_FORMATS.Text = Translations.TranslationGet("STR_FORMATS", "DE", "Date/Time Format");
            STR_DATE.Text = Translations.TranslationGet("STR_DATE", "DE", "Date :");
            CHKBOX_TIMESTAMP.Text = Translations.TranslationGet("CHKBOX_TIMESTAMP", "DE", "Display Time Stamp (where applicable)");
            STR_TIME.Text = Translations.TranslationGet("STR_TIME", "DE", "Time :");
            STR_LBLLANG.Text = Translations.TranslationGet("STR_LBLLANG", "DE", "Language :");
            STR_CAPTRANS.Text = Translations.TranslationGet("STR_CAPTRANS", "DE", "Translations");
            STR_CAPTOPLIST.Text = Translations.TranslationGet("STR_CAPTOPLIST", "DE", "Top 5 Lists");
            STR_LBLTOPLIST.Text = Translations.TranslationGet("STR_LBLTOPLIST", "DE", "Minimum No of Battles : ");
            //STR_RATINGSYSTEM.Text = Translations.TranslationGet("STR_RATINGSYSTEM", "DE", "System : ");
            //STR_GROUPRATINGSYSTEM.Text = Translations.TranslationGet("STR_GROUPRATINGSYSTEM", "DE", "Rating System");

            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("HTMLHeaderFont", out field);
            txtHTMLHeading.Text = field.NewValue.ToString();

            ((frmSetup)pForm)._propertyFields.TryGetValue("HTMLCellFont", out field);
            txtHTMLDetail.Text = field.NewValue.ToString();

            ((frmSetup)pForm)._propertyFields.TryGetValue("TopMinPlayed", out field);
            textMinNoGames.Text = field.NewValue.ToString();

            ((frmSetup)pForm)._propertyFields.TryGetValue("HTMLShowMovementPics", out field);
            CHKBOX_SHOWICONS.Checked = Convert.ToBoolean(field.NewValue);

            ((frmSetup)pForm)._propertyFields.TryGetValue("DateFormat", out field);
            comboDateFormat.Text = Convert.ToString(field.NewValue);

            ((frmSetup)pForm)._propertyFields.TryGetValue("TimeStamp", out field);
            CHKBOX_TIMESTAMP.Checked = Convert.ToBoolean(field.NewValue);

            ((frmSetup)pForm)._propertyFields.TryGetValue("TimeFormat", out field);
            comboTimeFormat.Text = Convert.ToString(field.NewValue);

            comboTimeFormat.Enabled = CHKBOX_TIMESTAMP.Checked;

            

            //((frmSetup)pForm)._propertyFields.TryGetValue("RatingSystem", out field);
            //comboRatingSystem.SelectedItem = Convert.ToString(field.NewValue);
            WOTHelper.AddToLog("Adding Languages ");
            foreach (KeyValuePair<string, Languages> lang in Translations.LanguageListGet().OrderBy(x=>x.Value.English))
            {
                WOTHelper.AddToLog("Adding Language: " + lang.Value.English);
                comboLangID.Properties.Items.Add(lang.Value.English);
            }

            ((frmSetup)pForm)._propertyFields.TryGetValue("LangID", out field);
            string setLangID = Convert.ToString(field.NewValue);
            comboLangID.SelectedItem = Translations.LanguageGet(setLangID, Language.English);
        }

        private void comboBoxEdit1_Properties_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void checkDisplayTime_CheckedChanged(object sender, EventArgs e)
        {
            comboTimeFormat.Enabled = CHKBOX_TIMESTAMP.Checked;

            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("TimeStamp", out field);
            field.NewValue = CHKBOX_TIMESTAMP.Checked;
        }

        private void txtHTMLHeading_EditValueChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("HTMLHeaderFont", out field);
            field.NewValue = txtHTMLHeading.Text;
        }

        private void txtHTMLDetail_EditValueChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("HTMLCellFont", out field);
            field.NewValue = txtHTMLDetail.Text;
        }

        private void checkMovementImages_CheckedChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("HTMLShowMovementPics", out field);
            field.NewValue = CHKBOX_SHOWICONS.Checked;
        }

        private void comboDateFormat_SelectedValueChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("DateFormat", out field);
            field.NewValue = comboDateFormat.Text;
        }

        private void comboTimeFormat_SelectedValueChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("TimeFormat", out field);
            field.NewValue = comboTimeFormat.Text;
        }

        private void textMinNoGames_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar > 31 && (e.KeyChar < '0' || e.KeyChar > '9'))
            {
                e.Handled = true;
            }
        }

        private void textMinNoGames_EditValueChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("TopMinPlayed", out field);
            field.NewValue = textMinNoGames.Text;
        }

        private void comboTimeFormat_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboDateFormat_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupControl4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboLangID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboLangID_SelectedValueChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("LangID", out field);
            field.NewValue = Translations.LanguageKeyGet(comboLangID.Text, Language.English);
        }

        private void STR_LBLTOPLIST_Validated(object sender, EventArgs e)
        {
           
        }

        private void STR_LBLTOPLIST_Resize(object sender, EventArgs e)
        {
            textMinNoGames.Left = STR_LBLTOPLIST.Width + 10;
        }

        private void STR_HEADING_Resize(object sender, EventArgs e)
        {
            txtHTMLHeading.Left = STR_HEADING.Width + 10;
            HTMLheadpx.Left = STR_HEADING.Width + 10 + txtHTMLHeading.Width + 5;
            STR_DETAIL.Width = STR_HEADING.Width;
            STR_DETAIL.Left = STR_HEADING.Left;
            txtHTMLDetail.Left = txtHTMLHeading.Left;
            HTMLDetailpx.Left = HTMLheadpx.Left;
        }

        private void STR_DETAIL_Resize(object sender, EventArgs e)
        {
          
        }


        private void comboRatingSystem_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

    

     

 
    }
}
