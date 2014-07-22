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
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;


namespace DevExpress.XtraCharts.Design {

    public class StylesContainerControl : XtraScrollableControl {
        class AppearanceColors {
            ChartAppearance appearance;
            Palette palette;
            StyleEdit[] editors;
            public ChartAppearance Appearance { get { return appearance; } }
            public int ColorsCount { get { return palette.Count + 1; } }
            public StyleEdit this[int index] {
                get { return editors[index]; }
                set { editors[index] = value; }
            }
            public AppearanceColors(ChartAppearance appearance, Palette palette) {
                this.appearance = appearance;
                this.palette = palette;
                editors = new StyleEdit[ColorsCount];
            }
        }
        PaletteRepository paletteRepository;
        List<AppearanceColors> matrix;
        int lastRegisteredAppearance;
        Size elementSize;
        Point current;
        ChartControl chart;
        bool lockChangeCurrent = false;
        public ChartAppearance CurrentAppearance { get { return matrix[current.Y].Appearance; } }
        public int CurrentPaletteIndex { get { return current.X; } }
        public event EventHandler OnEditValueChanged;
        public event EventHandler OnNeedClose;
        public void Initialize(ChartControl chart) {
            this.chart = chart;
            UpdateIt();

        }

        public void UpdateIt() {
            DisposeAppearanceImages();
            SuspendLayout();
            try {
                SetPaletteRepository(chart.PaletteRepository);
                SetAppearancesCount(chart.AppearanceRepository.Names.Length);
                foreach (ChartAppearance appearance in chart.AppearanceRepository) {
                    Palette currentPalette = chart.PaletteRepository[chart.PaletteName];
                    if (!currentPalette.Predefined || appearance == chart.AppearanceRepository[chart.AppearanceName] || 
                        String.IsNullOrEmpty(appearance.PaletteName) || appearance.PaletteName == currentPalette.Name)
                        RegisterAppearance(appearance, ViewType.Line, currentPalette);
                }
            }
            finally {
                ResumeLayout();
            }
        }
        public void SetPaletteRepository(PaletteRepository paletteRepository) {
            this.paletteRepository = paletteRepository;
        }
        public void SetAppearancesCount(int count) {
            matrix = new List<AppearanceColors>(count);
            lastRegisteredAppearance = 0;
        }
        public void SelectStyle(ChartAppearance appearance, int paletteIndex) {
            for (int i = 0; i < matrix.Count; i++) {
                AppearanceColors row = matrix[i];
                if (row.Appearance == appearance && paletteIndex < row.ColorsCount) {
                    current = new Point(paletteIndex, i);
                    lockChangeCurrent = true;
                    return;
                }
            }
            current = Point.Empty;
        }
        public void RegisterAppearance(ChartAppearance appearance, ViewType viewType, Palette palette) {
            AppearanceColors row = new AppearanceColors(appearance, palette);
            matrix.Add(row);
            for (int i = 0; i <= palette.Count; i++)
                row[i] = AddStyleEditor(AppearanceImageHelper.CreateImage(viewType, appearance, palette, i), i);
            lastRegisteredAppearance++;
        }
        void DisposeAppearanceImages() {
            foreach (Control control in Controls) {
                PictureEdit pictureEdit = control as PictureEdit;
                if (pictureEdit != null && pictureEdit.Image != null)
                    pictureEdit.Image.Dispose();
                control.Dispose();
            }
            Controls.Clear();
        }
        StyleEdit AddStyleEditor(Image image, int column) {
            elementSize = image.Size;
            StyleEdit styleEdit = new StyleEdit();
            styleEdit.Location = new Point(elementSize.Width * column, elementSize.Height * lastRegisteredAppearance);
            styleEdit.Size = elementSize;
            styleEdit.Image = image;
            styleEdit.BorderStyle = BorderStyles.NoBorder;
            styleEdit.Properties.ReadOnly = true;
            Controls.Add(styleEdit);
            return styleEdit;
        }
        void SelectCurrentChild() {
            lockChangeCurrent = true;
            try {
                matrix[current.Y][current.X].Focus();
            }
            finally {
                lockChangeCurrent = false;
            }
        }
        void CheckCurrentX() {
            int colorsCount = matrix[current.Y].ColorsCount;
            if (current.X >= colorsCount)
                current.X = colorsCount - 1;
        }
        void EditValueChanged() {
            string paletteName = chart.PaletteName;
            chart.AppearanceName = CurrentAppearance.Name;
            chart.PaletteName = paletteName;
            chart.PaletteBaseColorNumber = CurrentPaletteIndex;
            if (OnEditValueChanged != null)
                OnEditValueChanged(this, EventArgs.Empty);

        }
        internal void RaiseNeedClose() {
            if (OnNeedClose != null)
                OnNeedClose(this, EventArgs.Empty);
        }
        internal void OnFocusChanged() {
            if (lockChangeCurrent)
                return;
            for (int y = 0; y < matrix.Count; y++) {
                AppearanceColors row = matrix[y];
                for (int x = 0; x < row.ColorsCount; x++)
                    if (row[x].Focused) {
                        current.X = x;
                        current.Y = y;
                        EditValueChanged();
                        return;
                    }
            }
        }
        protected override void OnGotFocus(EventArgs e) {
            SelectCurrentChild();
        }
        protected override bool ProcessDialogKey(Keys keyData) {
            switch (keyData) {
                case Keys.Left:
                    if (--current.X < 0)
                        current.X = matrix[current.Y].ColorsCount - 1;
                    break;
                case Keys.Right:
                    if (++current.X >= matrix[current.Y].ColorsCount)
                        current.X = 0;
                    break;
                case Keys.Up:
                    if (--current.Y < 0)
                        current.Y = matrix.Count - 1;
                    CheckCurrentX();
                    break;
                case Keys.Down:
                    if (++current.Y >= matrix.Count)
                        current.Y = 0;
                    CheckCurrentX();
                    break;
                case Keys.Home:
                    current.X = 0;
                    current.Y = 0;
                    break;
                case Keys.End:
                    current.Y = matrix.Count - 1;
                    current.X = matrix[current.Y].ColorsCount - 1;
                    break;
                case Keys.PageUp: {
                        int step = ClientSize.Height / elementSize.Height;
                        if (step == 0)
                            step++;
                        current.Y -= step;
                        if (current.Y < 0)
                            current.Y = 0;
                        current.X = 0;
                        break;
                    }
                case Keys.PageDown: {
                        int step = ClientSize.Height / elementSize.Height;
                        if (step == 0)
                            step++;
                        current.Y += step;
                        if (current.Y >= matrix.Count)
                            current.Y = matrix.Count - 1;
                        current.X = int.MaxValue;
                        CheckCurrentX();
                        break;
                    }
                default:
                    return base.ProcessDialogKey(keyData);
            }
            SelectCurrentChild();
            EditValueChanged();
            return true;
        }
        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
            DisposeAppearanceImages();
        }
    }
}
