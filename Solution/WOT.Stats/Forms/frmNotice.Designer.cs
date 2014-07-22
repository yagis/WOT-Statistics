namespace WOT.Stats
{
    partial class frmNotice
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNotice));
            this.STR_DISCLAIMER = new DevExpress.XtraEditors.LabelControl();
            this.BTN_CLOSE = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // STR_DISCLAIMER
            // 
            this.STR_DISCLAIMER.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STR_DISCLAIMER.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.STR_DISCLAIMER.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.STR_DISCLAIMER.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.STR_DISCLAIMER.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.STR_DISCLAIMER.Location = new System.Drawing.Point(0, 0);
            this.STR_DISCLAIMER.Name = "STR_DISCLAIMER";
            this.STR_DISCLAIMER.Size = new System.Drawing.Size(1068, 105);
            this.STR_DISCLAIMER.TabIndex = 0;
            this.STR_DISCLAIMER.Text = "World of Tanks and Wargaming.net are registered trademarks or trademarks of Warga" +
    "ming.net LLP. \r\nThis application is in no way associated with Wargaming.net.";
            // 
            // BTN_CLOSE
            // 
            this.BTN_CLOSE.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_CLOSE.Appearance.Options.UseFont = true;
            this.BTN_CLOSE.Location = new System.Drawing.Point(493, 111);
            this.BTN_CLOSE.Name = "BTN_CLOSE";
            this.BTN_CLOSE.Size = new System.Drawing.Size(75, 23);
            this.BTN_CLOSE.TabIndex = 1;
            this.BTN_CLOSE.Text = "Close";
            this.BTN_CLOSE.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // frmNotice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 150);
            this.Controls.Add(this.BTN_CLOSE);
            this.Controls.Add(this.STR_DISCLAIMER);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmNotice";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Important Notice";
            this.Load += new System.EventHandler(this.frmNotice_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl STR_DISCLAIMER;
        private DevExpress.XtraEditors.SimpleButton BTN_CLOSE;
    }
}