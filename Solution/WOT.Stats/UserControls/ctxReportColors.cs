using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using WOTStatistics.Core;

namespace WOT.Stats
{
    public partial class ctxReportColors : DevExpress.XtraEditors.XtraUserControl
    {
        public ctxReportColors()
        {
            InitializeComponent();
        }

       

        private void ctxMiscellaneous_Load(object sender, EventArgs e)
        {

            STR_POS.Text = Translations.TranslationGet("STR_POS", "DE", "Positive :");
            STR_NEU.Text = Translations.TranslationGet("STR_NEU", "DE", "Neutral :");
            STR_NEG.Text = Translations.TranslationGet("STR_NEG", "DE", "Negative :");

            HTML_EFF_VERYBAD.Text = Translations.TranslationGet("HTML_EFF_VERYBAD", "DE", "Very Bad");
            HTML_EFF_BAD.Text = Translations.TranslationGet("HTML_EFF_BAD", "DE", "Bad");
            HTML_EFF_BELAVG.Text = Translations.TranslationGet("HTML_EFF_BELAVG", "DE", "Below Average");
            HTML_EFF_AVG.Text = Translations.TranslationGet("HTML_EFF_AVG", "DE", "Average");
            HTML_EFF_GOOD.Text = Translations.TranslationGet("HTML_EFF_GOOD", "DE", "Good");
            HTML_EFF_VERYGOOD.Text = Translations.TranslationGet("HTML_EFF_VERYGOOD", "DE", "Very Good");
            HTML_EFF_GREAT.Text = Translations.TranslationGet("HTML_EFF_GREAT", "DE", "Great");
            HTML_EFF_UNICUM.Text = Translations.TranslationGet("HTML_EFF_UNICUM", "DE", "Unicum");
            HTML_EFF_SUPERUNICUM.Text = Translations.TranslationGet("HTML_EFF_SUPERUNICUM", "DE", "Super Unicum");

            STR_PROGRESS.Text = Translations.TranslationGet("STR_PROGRESS", "DE", "Progress");
            HTML_CONT_EFFICIENCY.Text = Translations.TranslationGet("HTML_CONT_EFFICIENCY", "DE", "Efficiency");

            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("ColorPositive", out field);
            colorPositive.Color = Color.FromArgb(Convert.ToInt32(field.NewValue));

            ((frmSetup)pForm)._propertyFields.TryGetValue("ColorNeutral", out field);
            colorNeutral.Color = Color.FromArgb(Convert.ToInt32(field.NewValue));

            ((frmSetup)pForm)._propertyFields.TryGetValue("ColorNegative", out field);
            colorNegative.Color = Color.FromArgb(Convert.ToInt32(field.NewValue));

            ((frmSetup)pForm)._propertyFields.TryGetValue("colorWNClass1", out field);
            colorWNClass1.Color = Color.FromArgb(Convert.ToInt32(field.NewValue));

            ((frmSetup)pForm)._propertyFields.TryGetValue("colorWNClass2", out field);
            colorWNClass2.Color = Color.FromArgb(Convert.ToInt32(field.NewValue));

            ((frmSetup)pForm)._propertyFields.TryGetValue("colorWNClass3", out field);
            colorWNClass3.Color = Color.FromArgb(Convert.ToInt32(field.NewValue));

            ((frmSetup)pForm)._propertyFields.TryGetValue("colorWNClass4", out field);
            colorWNClass4.Color = Color.FromArgb(Convert.ToInt32(field.NewValue));

            ((frmSetup)pForm)._propertyFields.TryGetValue("colorWNClass5", out field);
            colorWNClass5.Color = Color.FromArgb(Convert.ToInt32(field.NewValue));

            ((frmSetup)pForm)._propertyFields.TryGetValue("colorWNClass6", out field);
            colorWNClass6.Color = Color.FromArgb(Convert.ToInt32(field.NewValue));

            ((frmSetup)pForm)._propertyFields.TryGetValue("colorWNClass7", out field);
            colorWNClass7.Color = Color.FromArgb(Convert.ToInt32(field.NewValue));
            
            ((frmSetup)pForm)._propertyFields.TryGetValue("colorWNClass8", out field);
            colorWNClass8.Color = Color.FromArgb(Convert.ToInt32(field.NewValue));

            ((frmSetup)pForm)._propertyFields.TryGetValue("colorWNClass9", out field);
            colorWNClass9.Color = Color.FromArgb(Convert.ToInt32(field.NewValue));

        }

     

        private void colorPositive_EditValueChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("ColorPositive", out field);
            field.NewValue =  colorPositive.Color.ToArgb();

            
        }

        private void colorNeutral_EditValueChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("ColorNeutral", out field);
            field.NewValue = colorNeutral.Color.ToArgb();

            
        }

        private void colorNegative_EditValueChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("ColorNegative", out field);
            field.NewValue = colorNegative.Color.ToArgb();
        }





        private void colorWNClass1_EditValueChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("colorWNClass1", out field);
            field.NewValue = colorWNClass1.Color.ToArgb();
        }

        private void colorWNClass2_EditValueChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("colorWNClass2", out field);
            field.NewValue = colorWNClass2.Color.ToArgb();
        }

        private void colorWNClass3_EditValueChanged(object sender, EventArgs e)
        {

            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("colorWNClass3", out field);
            field.NewValue = colorWNClass3.Color.ToArgb();
        }

        private void colorWNClass4_EditValueChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("colorWNClass4", out field);
            field.NewValue = colorWNClass4.Color.ToArgb();
        }

        private void colorWNClass5_EditValueChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("colorWNClass5", out field);
            field.NewValue = colorWNClass5.Color.ToArgb();
        }

        private void colorWNClass6_EditValueChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("colorWNClass6", out field);
            field.NewValue = colorWNClass6.Color.ToArgb();
        }

        private void colorWNClass7_EditValueChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("colorWNClass7", out field);
            field.NewValue = colorWNClass7.Color.ToArgb();
        }

        private void colorWNClass8_EditValueChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("colorWNClass8", out field);
            field.NewValue = colorWNClass8.Color.ToArgb();
        }

        private void colorWNClass9_EditValueChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("colorWNClass9", out field);
            field.NewValue = colorWNClass9.Color.ToArgb();
        }



    }
}
