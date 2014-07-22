namespace WOT.Stats
{
    partial class frmCredits
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCredits));
            this.BTN_CLOSE = new DevExpress.XtraEditors.SimpleButton();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.layoutConverter1 = new DevExpress.XtraLayout.Converter.LayoutConverter(this.components);
            this.memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
            this.frmCreditsConvertedLayout = new DevExpress.XtraLayout.LayoutControl();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frmCreditsConvertedLayout)).BeginInit();
            this.frmCreditsConvertedLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // BTN_CLOSE
            // 
            this.BTN_CLOSE.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BTN_CLOSE.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_CLOSE.Appearance.Options.UseFont = true;
            this.BTN_CLOSE.Location = new System.Drawing.Point(12, 163);
            this.BTN_CLOSE.Name = "BTN_CLOSE";
            this.BTN_CLOSE.Size = new System.Drawing.Size(412, 23);
            this.BTN_CLOSE.StyleController = this.frmCreditsConvertedLayout;
            this.BTN_CLOSE.TabIndex = 1;
            this.BTN_CLOSE.Text = "Close";
            this.BTN_CLOSE.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // memoEdit1
            // 
            this.memoEdit1.EditValue = resources.GetString("memoEdit1.EditValue");
            this.memoEdit1.Location = new System.Drawing.Point(12, 28);
            this.memoEdit1.Name = "memoEdit1";
            this.memoEdit1.Properties.ReadOnly = true;
            this.memoEdit1.Size = new System.Drawing.Size(412, 131);
            this.memoEdit1.StyleController = this.frmCreditsConvertedLayout;
            this.memoEdit1.TabIndex = 2;
            this.memoEdit1.UseOptimizedRendering = true;
            // 
            // frmCreditsConvertedLayout
            // 
            this.frmCreditsConvertedLayout.Controls.Add(this.memoEdit1);
            this.frmCreditsConvertedLayout.Controls.Add(this.BTN_CLOSE);
            this.frmCreditsConvertedLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.frmCreditsConvertedLayout.Location = new System.Drawing.Point(0, 0);
            this.frmCreditsConvertedLayout.Name = "frmCreditsConvertedLayout";
            this.frmCreditsConvertedLayout.Root = this.layoutControlGroup1;
            this.frmCreditsConvertedLayout.Size = new System.Drawing.Size(436, 198);
            this.frmCreditsConvertedLayout.TabIndex = 5;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup2});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(436, 198);
            this.layoutControlGroup1.Text = "layoutControlGroup1";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup2.GroupBordersVisible = false;
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "groupControl1item";
            this.layoutControlGroup2.Size = new System.Drawing.Size(416, 178);
            this.layoutControlGroup2.Text = "layoutControlGroup1";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.memoEdit1;
            this.layoutControlItem1.CustomizationFormText = "Thanks for the contributors, translators and beta testers of WOT Statistics";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "memoEdit1item";
            this.layoutControlItem1.Size = new System.Drawing.Size(416, 151);
            this.layoutControlItem1.Text = "Thanks for the contributors, translators and beta testers of WOT Statistics";
            this.layoutControlItem1.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(359, 13);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.BTN_CLOSE;
            this.layoutControlItem2.CustomizationFormText = "BTN_CLOSEitem";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 151);
            this.layoutControlItem2.Name = "BTN_CLOSEitem";
            this.layoutControlItem2.Size = new System.Drawing.Size(416, 27);
            this.layoutControlItem2.Text = "BTN_CLOSEitem";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextToControlDistance = 0;
            this.layoutControlItem2.TextVisible = false;
            // 
            // frmCredits
            // 
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 198);
            this.Controls.Add(this.frmCreditsConvertedLayout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCredits";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Credits";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.frmCredits_HelpButtonClicked);
            this.Load += new System.EventHandler(this.frmCredits_Load);
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frmCreditsConvertedLayout)).EndInit();
            this.frmCreditsConvertedLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton BTN_CLOSE;
        private System.Windows.Forms.HelpProvider helpProvider1;
        private DevExpress.XtraLayout.LayoutControl frmCreditsConvertedLayout;
        private DevExpress.XtraEditors.MemoEdit memoEdit1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.Converter.LayoutConverter layoutConverter1;
    }
}