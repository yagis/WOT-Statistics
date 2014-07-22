namespace WOT.Stats
{
    partial class frmUpdater
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUpdater));
            this.prgBarProgress = new DevExpress.XtraEditors.ProgressBarControl();
            this.gridUpdates = new DevExpress.XtraGrid.GridControl();
            this.viewUpdates = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.oStatus = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            this.imgStatus = new DevExpress.Utils.ImageCollection(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.prgBarProgress.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridUpdates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewUpdates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.oStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // prgBarProgress
            // 
            resources.ApplyResources(this.prgBarProgress, "prgBarProgress");
            this.prgBarProgress.Name = "prgBarProgress";
            this.prgBarProgress.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("prgBarProgress.Properties.Appearance.Font")));
            this.prgBarProgress.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.prgBarProgress.Properties.EndColor = System.Drawing.Color.Empty;
            this.prgBarProgress.Properties.LookAndFeel.SkinName = "Office 2010 Black";
            this.prgBarProgress.Properties.PercentView = false;
            this.prgBarProgress.Properties.ProgressViewStyle = DevExpress.XtraEditors.Controls.ProgressViewStyle.Solid;
            this.prgBarProgress.Properties.ShowTitle = true;
            this.prgBarProgress.Properties.StartColor = System.Drawing.Color.Empty;
            // 
            // gridUpdates
            // 
            resources.ApplyResources(this.gridUpdates, "gridUpdates");
            this.gridUpdates.MainView = this.viewUpdates;
            this.gridUpdates.Name = "gridUpdates";
            this.gridUpdates.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.oStatus});
            this.gridUpdates.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewUpdates});
            // 
            // viewUpdates
            // 
            this.viewUpdates.GridControl = this.gridUpdates;
            this.viewUpdates.Name = "viewUpdates";
            this.viewUpdates.OptionsBehavior.Editable = false;
            this.viewUpdates.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.viewUpdates.OptionsSelection.MultiSelect = true;
            this.viewUpdates.OptionsView.EnableAppearanceEvenRow = true;
            this.viewUpdates.OptionsView.ShowGroupPanel = false;
            this.viewUpdates.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            this.viewUpdates.OptionsView.ShowIndicator = false;
            this.viewUpdates.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.True;
            // 
            // oStatus
            // 
            resources.ApplyResources(this.oStatus, "oStatus");
            this.oStatus.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("oStatus.Buttons"))))});
            this.oStatus.Name = "oStatus";
            // 
            // imgStatus
            // 
            this.imgStatus.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imgStatus.ImageStream")));
            this.imgStatus.Images.SetKeyName(0, "bullet_square_grey.png");
            this.imgStatus.Images.SetKeyName(1, "bullet_square_yellow.png");
            this.imgStatus.Images.SetKeyName(2, "bullet_square_green.png");
            this.imgStatus.Images.SetKeyName(3, "bullet_square_red.png");
            this.imgStatus.Images.SetKeyName(4, "bullet_square_blue.png");
            this.imgStatus.Images.SetKeyName(5, "bullet_square_glass_blue.png");
            // 
            // frmUpdater
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridUpdates);
            this.Controls.Add(this.prgBarProgress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.LookAndFeel.SkinName = "Office 2010 Black";
            this.MaximizeBox = false;
            this.Name = "frmUpdater";
            this.Load += new System.EventHandler(this.UpdateMe_Load);
            ((System.ComponentModel.ISupportInitialize)(this.prgBarProgress.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridUpdates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewUpdates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.oStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgStatus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.ProgressBarControl prgBarProgress;
        private DevExpress.XtraGrid.GridControl gridUpdates;
        private DevExpress.XtraGrid.Views.Grid.GridView viewUpdates;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox oStatus;
        private DevExpress.Utils.ImageCollection imgStatus;
    }
}

