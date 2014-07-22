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
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors.Drawing;


namespace DevExpress.XtraCharts.Design {
    internal class StyleEditPainter : PictureEditPainter, IDisposable {
        protected Pen highlightPen;

        protected virtual int InnerIndent { get { return 0; } }

        public StyleEditPainter(Color focusColor) {
            highlightPen = new Pen(focusColor, 2.0f);
        }
        public void Dispose() {
            if (highlightPen != null) {
                highlightPen.Dispose();
                highlightPen = null;
            }
        }
        protected override void DrawFocusRect(ControlGraphicsInfoArgs info) {
            BaseEditViewInfo vi = info.ViewInfo as BaseEditViewInfo;
            if (vi != null && vi.DrawFocusRect) {
                DrawCustomBorder(info);
                return;
            }
            base.DrawFocusRect(info);
        }
        protected void DrawCustomBorder(ControlGraphicsInfoArgs info) {
            SmoothingMode smoothing = info.Graphics.SmoothingMode;
            try {
                info.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                using (GraphicsPath path = new GraphicsPath()) {
                    int halfWidth = -(int)Math.Ceiling(highlightPen.Width / 2.0) - InnerIndent;
                    Rectangle bounds = Rectangle.Inflate(info.Bounds, halfWidth, halfWidth);
                    int fraction = 6;
                    int right = bounds.Right - fraction - 1;
                    int bottom = bounds.Bottom - fraction - 1;
                    path.AddArc(bounds.X, bounds.Y, fraction, fraction, 180, 90);
                    path.AddArc(right, bounds.Y, fraction, fraction, 270, 90);
                    path.AddArc(right, bottom, fraction, fraction, 0, 90);
                    path.AddArc(bounds.Left, bottom, fraction, fraction, 90, 90);
                    path.CloseFigure();
                    info.Graphics.DrawPath(highlightPen, path);
                }
            }
            finally {
                info.Graphics.SmoothingMode = smoothing;
            }
        }
    }
    internal class StyleEdit : PictureEdit {
        StyleEditPainter painter;
        protected override BaseControlPainter Painter {
            get {
                if (painter == null)
                    painter = new StyleEditPainter(CommonSkins.GetSkin(LookAndFeel).Colors["Highlight"]);
                return painter;
            }
        }
        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
            if (disposing && painter != null) {
                painter.Dispose();
                painter = null;
            }
        }
        protected override void OnGotFocus(EventArgs e) {
            base.OnGotFocus(e);
            StylesContainerControl container = Parent as StylesContainerControl;
            if (container != null)
                container.OnFocusChanged();
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e) {
            if (e.Button == MouseButtons.Left)
                CloseContainer();
            else
                base.OnMouseDoubleClick(e);
        }
        protected override void OnKeyDown(KeyEventArgs e) {
            if (e.KeyCode == Keys.Return)
                CloseContainer();
            else
                base.OnKeyDown(e);
        }
        void CloseContainer() {
            StylesContainerControl container = Parent as StylesContainerControl;
            if (container != null)
                container.RaiseNeedClose();
        }
    }
}
