// Developer Express Code Central Example:
// How to provide end-users with the capability to change a chart's appearance on the fly
// 
// This sample demonstrates how a custom palette and appearance selector for
// XtraCharts can be implemented.
// 
// For this, we create two user controls and
// embed them into the application's form.
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=E703

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevExpress.Utils;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Design;

namespace ChartAppearanceSample {
    public partial class PaletteEditControl : XtraUserControl {
        public static Image CreateEditorImage(Palette palette) {
            const int imageSize = 10;
            Bitmap image = null;
            try {
                image = new Bitmap(palette.Count * (imageSize + 1) - 1, imageSize);
                using(Graphics g = Graphics.FromImage(image)) {
                    Rectangle rect = new Rectangle(Point.Empty, new Size(imageSize, imageSize));
                    for(int i = 0; i < palette.Count; i++, rect.X += 11) {
                        using(Brush brush = new SolidBrush(palette[i].Color))
                            g.FillRectangle(brush, rect);
                        Rectangle penRect = rect;
                        penRect.Width--;
                        penRect.Height--;
                        using(Pen pen = new Pen(Color.Gray))
                            g.DrawRectangle(pen, penRect);
                    }
                }
            }
            catch {
                if(image != null) {
                    image.Dispose();
                    image = null;
                }
            }
            return image;
        }

        ChartControl chart;
        PaletteRepository paletteRepository;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ChartControl Chart {
            get {
                return chart;
            }
            set {
                chart = value;
                PaletteRepository = chart.PaletteRepository;
                SelectedPalette = chart.PaletteRepository[chart.PaletteName];
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PaletteRepository PaletteRepository {
            get {
                return paletteRepository;
            }
            set {
                paletteRepository = value;
                FillPalettes();
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Palette SelectedPalette {
            get {
                return lbPalettes.SelectedValue as Palette;
            }
            set {
                if(value == null) {
                    if(lbPalettes.ItemCount > 0)
                        lbPalettes.SelectedIndex = 0;
                }
                else
                    lbPalettes.SelectedValue = value;
            }
        }
        public event EventHandler OnPaletteChanged;
        public event EventHandler OnNeedClose;
        public PaletteEditControl() {
            InitializeComponent();
        }
        public void SetLookAndFeel(UserLookAndFeel lookAndFeel) {
            LookAndFeel.ParentLookAndFeel = lookAndFeel;
            lbPalettes.LookAndFeel.ParentLookAndFeel = lookAndFeel;
        }
        void FillPalettes() {
            string[] names = paletteRepository.PaletteNames;
            Image[] images = new Image[names.Length];
            Size imageSize = Size.Empty;
            lbPalettes.Items.BeginUpdate();
            lbPalettes.Items.Clear();
            for(int i = 0; i < names.Length; i++) {
                string name = names[i];
                Palette palette = paletteRepository[name];
                Image image = CreateEditorImage(palette);
                images[i] = image;
                if(image.Width > imageSize.Width)
                    imageSize.Width = image.Width;
                if(image.Height > imageSize.Height)
                    imageSize.Height = image.Height;
                lbPalettes.Items.Add(palette, i);
            }
            lbPalettes.Items.EndUpdate();
            paletteImages.BeginInit();
            paletteImages.Clear();
            paletteImages.ImageSize = imageSize;
            for(int i = 0; i < images.Length; i++) {
                Image image = images[i];
                Bitmap newImage = null;
                if(image.Size != imageSize) {
                    try {
                        newImage = new Bitmap(imageSize.Width, imageSize.Height);
                        using(Graphics gr = Graphics.FromImage(newImage))
                            gr.DrawImage(image, Point.Empty);
                        image.Dispose();
                    }
                    catch {
                        if(newImage != null) {
                            newImage.Dispose();
                            newImage = null;
                        }
                    }
                }
                paletteImages.AddImage(newImage == null ? image : newImage);
            }
            paletteImages.EndInit();
        }
        void RaisePaletteChanged() {
            if(OnPaletteChanged != null)
                OnPaletteChanged(this, EventArgs.Empty);
        }
        void RaiseNeedClose() {
            if(OnNeedClose != null)
                OnNeedClose(this, EventArgs.Empty);
        }
        void lbPalettes_SelectedIndexChanged(object sender, EventArgs e) {
            if(chart != null)
                chart.PaletteName = SelectedPalette.Name;
            RaisePaletteChanged();
        }
        void lbPalettes_MouseDoubleClick(object sender, MouseEventArgs e) {
            if(e.Button == MouseButtons.Left)
                RaiseNeedClose();
        }
        void lbPalettes_KeyDown(object sender, KeyEventArgs e) {
            if(e.KeyCode == Keys.Enter)
                RaiseNeedClose();
        }
        void btnEdit_Click(object sender, EventArgs e) {
            Palette palette = SelectedPalette;
            using(PaletteEditorForm form = new PaletteEditorForm(paletteRepository)) {
                form.LookAndFeel.ParentLookAndFeel = LookAndFeel.ParentLookAndFeel;
                form.Location = ControlUtils.CalcLocation(Cursor.Position, Cursor.Position, form.Size);
                form.TopMost = true;
                form.CurrentPalette = palette;
                DialogResult result = form.ShowDialog();
                FillPalettes();
                SelectedPalette = result == DialogResult.OK ? form.CurrentPalette : palette;
            }
        }
    }
}
