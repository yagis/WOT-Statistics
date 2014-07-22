namespace WOT.Stats
{
    partial class frmDefineCharts
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDefineCharts));
            this.BTN_EDIT = new DevExpress.XtraEditors.SimpleButton();
            this.BTN_CLOSE = new DevExpress.XtraEditors.SimpleButton();
            this.BTN_REMOVE = new DevExpress.XtraEditors.SimpleButton();
            this.BTN_ADD = new DevExpress.XtraEditors.SimpleButton();
            this.listView1 = new System.Windows.Forms.ListView();
            this.colChartName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.SuspendLayout();
            // 
            // BTN_EDIT
            // 
            this.BTN_EDIT.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_EDIT.Appearance.Options.UseFont = true;
            this.BTN_EDIT.Location = new System.Drawing.Point(81, 287);
            this.BTN_EDIT.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BTN_EDIT.Name = "BTN_EDIT";
            this.BTN_EDIT.Size = new System.Drawing.Size(75, 23);
            this.BTN_EDIT.TabIndex = 9;
            this.BTN_EDIT.Text = "Edit";
            this.BTN_EDIT.Click += new System.EventHandler(this.simpleButton4_Click);
            // 
            // BTN_CLOSE
            // 
            this.BTN_CLOSE.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_CLOSE.Appearance.Options.UseFont = true;
            this.BTN_CLOSE.Location = new System.Drawing.Point(239, 287);
            this.BTN_CLOSE.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BTN_CLOSE.Name = "BTN_CLOSE";
            this.BTN_CLOSE.Size = new System.Drawing.Size(75, 23);
            this.BTN_CLOSE.TabIndex = 8;
            this.BTN_CLOSE.Text = "Close";
            this.BTN_CLOSE.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // BTN_REMOVE
            // 
            this.BTN_REMOVE.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_REMOVE.Appearance.Options.UseFont = true;
            this.BTN_REMOVE.Location = new System.Drawing.Point(160, 287);
            this.BTN_REMOVE.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BTN_REMOVE.Name = "BTN_REMOVE";
            this.BTN_REMOVE.Size = new System.Drawing.Size(75, 23);
            this.BTN_REMOVE.TabIndex = 7;
            this.BTN_REMOVE.Text = "Remove";
            this.BTN_REMOVE.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // BTN_ADD
            // 
            this.BTN_ADD.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_ADD.Appearance.Options.UseFont = true;
            this.BTN_ADD.Location = new System.Drawing.Point(2, 287);
            this.BTN_ADD.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BTN_ADD.Name = "BTN_ADD";
            this.BTN_ADD.Size = new System.Drawing.Size(75, 23);
            this.BTN_ADD.TabIndex = 6;
            this.BTN_ADD.Text = "Add";
            this.BTN_ADD.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colChartName});
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(2, 4);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(312, 279);
            this.listView1.TabIndex = 10;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // colChartName
            // 
            this.colChartName.Text = "Chart Name";
            this.colChartName.Width = 306;
            // 
            // frmDefineCharts
            // 
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 313);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.BTN_EDIT);
            this.Controls.Add(this.BTN_CLOSE);
            this.Controls.Add(this.BTN_REMOVE);
            this.Controls.Add(this.BTN_ADD);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDefineCharts";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Define Charts";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.frmDefineCharts_HelpButtonClicked);
            this.Load += new System.EventHandler(this.frmDefineCharts_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton BTN_EDIT;
        private DevExpress.XtraEditors.SimpleButton BTN_CLOSE;
        private DevExpress.XtraEditors.SimpleButton BTN_REMOVE;
        private DevExpress.XtraEditors.SimpleButton BTN_ADD;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader colChartName;
        private System.Windows.Forms.HelpProvider helpProvider1;
    }
}