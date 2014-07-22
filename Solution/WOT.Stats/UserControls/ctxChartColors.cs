using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraCharts;
using WOTStatistics.Core;

namespace WOT.Stats
{
    public partial class ctxChartColors : DevExpress.XtraEditors.XtraUserControl
    {
        public ctxChartColors()
        {
            InitializeComponent();
            stylesContainerControl1.OnEditValueChanged += new EventHandler(stylesContainerControl1_OnEditValueChanged);
        }

        void stylesContainerControl1_OnEditValueChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
                PropertyFields appearance;
                ((frmSetup)pForm)._propertyFields.TryGetValue("ChartAppearance", out appearance);
                appearance.NewValue = chartControl1.AppearanceNameSerializable;

                PropertyFields palette;
                ((frmSetup)pForm)._propertyFields.TryGetValue("ChartPalette", out palette);
                palette.NewValue = chartControl1.PaletteName;
      


        }

        private void paletteEditControl1_OnPaletteChanged(object sender, EventArgs e)
        {
            
            
            stylesContainerControl1.Initialize(chartControl1);
            Form pForm = ParentForm;
            PropertyFields appearance;
            ((frmSetup)pForm)._propertyFields.TryGetValue("ChartAppearance", out appearance);
            appearance.NewValue = chartControl1.AppearanceNameSerializable;

            PropertyFields palette;
            ((frmSetup)pForm)._propertyFields.TryGetValue("ChartPalette", out palette);
            palette.NewValue = chartControl1.PaletteName;

           
        }

        private void ctxChartColors_Load(object sender, EventArgs e)
        {

            STR_SELECTAPP.Text = Translations.TranslationGet("STR_SELECTAPP", "DE", "Select an Appearance :");
            STR_SELECTPAL.Text = Translations.TranslationGet("STR_SELECTPAL", "DE", "Select a Palette :");

            Form pForm = ParentForm;
                PropertyFields appearance;
                ((frmSetup)pForm)._propertyFields.TryGetValue("ChartAppearance", out appearance);
                chartControl1.AppearanceNameSerializable = appearance.NewValue.ToString();

                PropertyFields palette;
                ((frmSetup)pForm)._propertyFields.TryGetValue("ChartPalette", out palette);
                chartControl1.PaletteName = palette.NewValue.ToString();
        

            stylesContainerControl1.Initialize(chartControl1);
            paletteEditControl1.Chart = chartControl1;
        }
    }
}
