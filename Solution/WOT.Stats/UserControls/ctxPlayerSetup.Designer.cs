namespace WOT.Stats
{
    partial class ctxPlayerSetup
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.PlayerID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PlayerRealm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ftp = new DevExpress.XtraGrid.Columns.GridColumn();
            this.watchFile = new DevExpress.XtraGrid.Columns.GridColumn();
            this.butDelete = new DevExpress.XtraEditors.SimpleButton();
            this.butAdd = new DevExpress.XtraEditors.SimpleButton();
            this.butEdit = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControl1.Font = new System.Drawing.Font("Arial Unicode MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(825, 171);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridView1.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView1.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridView1.Appearance.Row.Options.UseFont = true;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.PlayerID,
            this.PlayerRealm,
            this.ftp,
            this.watchFile});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowDetailButtons = false;
            this.gridView1.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.True;
            this.gridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.True;
            // 
            // PlayerID
            // 
            this.PlayerID.AppearanceCell.Font = new System.Drawing.Font("Arial Unicode MS", 9F);
            this.PlayerID.AppearanceCell.Options.UseFont = true;
            this.PlayerID.AppearanceHeader.Font = new System.Drawing.Font("Arial Unicode MS", 10F, System.Drawing.FontStyle.Bold);
            this.PlayerID.AppearanceHeader.Options.UseFont = true;
            this.PlayerID.Caption = "Player ID";
            this.PlayerID.FieldName = "PlayerID";
            this.PlayerID.Name = "PlayerID";
            this.PlayerID.OptionsColumn.AllowEdit = false;
            this.PlayerID.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.PlayerID.OptionsFilter.AllowAutoFilter = false;
            this.PlayerID.OptionsFilter.AllowFilter = false;
            this.PlayerID.OptionsFilter.ImmediateUpdateAutoFilter = false;
            this.PlayerID.Visible = true;
            this.PlayerID.VisibleIndex = 0;
            // 
            // PlayerRealm
            // 
            this.PlayerRealm.Caption = "PlayerRealm";
            this.PlayerRealm.FieldName = "PlayerRealm";
            this.PlayerRealm.Name = "PlayerRealm";
            this.PlayerRealm.Visible = true;
            this.PlayerRealm.VisibleIndex = 1;
            // 
            // ftp
            // 
            this.ftp.AppearanceCell.Font = new System.Drawing.Font("Arial Unicode MS", 9F);
            this.ftp.AppearanceCell.Options.UseFont = true;
            this.ftp.AppearanceHeader.Font = new System.Drawing.Font("Arial Unicode MS", 10F, System.Drawing.FontStyle.Bold);
            this.ftp.AppearanceHeader.Options.UseFont = true;
            this.ftp.Caption = "Retrieve From FTP";
            this.ftp.FieldName = "ftp";
            this.ftp.Name = "ftp";
            this.ftp.OptionsColumn.AllowEdit = false;
            this.ftp.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.ftp.OptionsFilter.AllowAutoFilter = false;
            this.ftp.OptionsFilter.AllowFilter = false;
            this.ftp.OptionsFilter.ImmediateUpdateAutoFilter = false;
            this.ftp.Visible = true;
            this.ftp.VisibleIndex = 2;
            // 
            // watchFile
            // 
            this.watchFile.AppearanceCell.Font = new System.Drawing.Font("Arial Unicode MS", 9F);
            this.watchFile.AppearanceCell.Options.UseFont = true;
            this.watchFile.AppearanceHeader.Font = new System.Drawing.Font("Arial Unicode MS", 10F, System.Drawing.FontStyle.Bold);
            this.watchFile.AppearanceHeader.Options.UseFont = true;
            this.watchFile.Caption = "Watching File";
            this.watchFile.FieldName = "watchFile";
            this.watchFile.Name = "watchFile";
            this.watchFile.OptionsColumn.AllowEdit = false;
            this.watchFile.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.watchFile.OptionsFilter.AllowAutoFilter = false;
            this.watchFile.OptionsFilter.AllowFilter = false;
            this.watchFile.OptionsFilter.ImmediateUpdateAutoFilter = false;
            this.watchFile.Visible = true;
            this.watchFile.VisibleIndex = 3;
            // 
            // butDelete
            // 
            this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butDelete.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butDelete.Appearance.Options.UseFont = true;
            this.butDelete.Location = new System.Drawing.Point(747, 177);
            this.butDelete.Name = "butDelete";
            this.butDelete.Size = new System.Drawing.Size(75, 23);
            this.butDelete.TabIndex = 1;
            this.butDelete.Text = "Remove";
            this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
            // 
            // butAdd
            // 
            this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butAdd.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butAdd.Appearance.Options.UseFont = true;
            this.butAdd.Location = new System.Drawing.Point(666, 177);
            this.butAdd.Name = "butAdd";
            this.butAdd.Size = new System.Drawing.Size(75, 23);
            this.butAdd.TabIndex = 0;
            this.butAdd.Text = "Add";
            this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
            // 
            // butEdit
            // 
            this.butEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butEdit.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butEdit.Appearance.Options.UseFont = true;
            this.butEdit.Location = new System.Drawing.Point(585, 177);
            this.butEdit.Name = "butEdit";
            this.butEdit.Size = new System.Drawing.Size(75, 23);
            this.butEdit.TabIndex = 2;
            this.butEdit.Text = "Edit";
            this.butEdit.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // ctxPlayerSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.butEdit);
            this.Controls.Add(this.butAdd);
            this.Controls.Add(this.butDelete);
            this.Controls.Add(this.gridControl1);
            this.Name = "ctxPlayerSetup";
            this.Size = new System.Drawing.Size(825, 204);
            this.Load += new System.EventHandler(this.ctxPlayerSetup_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.SimpleButton butDelete;
        private DevExpress.XtraEditors.SimpleButton butAdd;
        private DevExpress.XtraGrid.Columns.GridColumn PlayerID;
        private DevExpress.XtraGrid.Columns.GridColumn ftp;
        private DevExpress.XtraGrid.Columns.GridColumn watchFile;
        private DevExpress.XtraEditors.SimpleButton butEdit;
        private DevExpress.XtraGrid.Columns.GridColumn PlayerRealm;

    }
}
