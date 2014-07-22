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

namespace ChartAppearanceSample {
    partial class PaletteEditControl {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaletteEditControl));
            this.paletteImages = new DevExpress.Utils.ImageCollection(this.components);
            this.lbPalettes = new DevExpress.XtraEditors.ImageListBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.paletteImages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbPalettes)).BeginInit();
            this.SuspendLayout();
            // 
            // paletteImages
            // 
            this.paletteImages.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("paletteImages.ImageStream")));
            // 
            // lbPalettes
            // 
            this.lbPalettes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPalettes.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lbPalettes.ImageList = this.paletteImages;
            this.lbPalettes.Location = new System.Drawing.Point(0, 0);
            this.lbPalettes.Name = "lbPalettes";
            this.lbPalettes.Size = new System.Drawing.Size(258, 293);
            this.lbPalettes.TabIndex = 2;
            this.lbPalettes.SelectedIndexChanged += new System.EventHandler(this.lbPalettes_SelectedIndexChanged);
            this.lbPalettes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbPalettes_KeyDown);
            this.lbPalettes.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbPalettes_MouseDoubleClick);
            // 
            // PaletteEditControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbPalettes);
            this.Name = "PaletteEditControl";
            this.Size = new System.Drawing.Size(258, 293);
            ((System.ComponentModel.ISupportInitialize)(this.paletteImages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbPalettes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.Utils.ImageCollection paletteImages;
        private DevExpress.XtraEditors.ImageListBoxControl lbPalettes;
    }
}
