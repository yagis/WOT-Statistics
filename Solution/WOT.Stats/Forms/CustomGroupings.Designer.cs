namespace WOT.Stats
{
    partial class CustomGroupings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomGroupings));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tankListView = new System.Windows.Forms.ListView();
            this.txtGroupName = new DevExpress.XtraEditors.TextEdit();
            this.HTML_GROUPNAME = new DevExpress.XtraEditors.LabelControl();
            this.BTN_CLOSE = new DevExpress.XtraEditors.SimpleButton();
            this.BTN_SAVE = new DevExpress.XtraEditors.SimpleButton();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            ((System.ComponentModel.ISupportInitialize)(this.txtGroupName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tankListView
            // 
            this.tankListView.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tankListView.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tankListView.FullRowSelect = true;
            this.tankListView.Location = new System.Drawing.Point(3, 32);
            this.tankListView.Name = "tankListView";
            this.tankListView.Size = new System.Drawing.Size(490, 609);
            this.tankListView.SmallImageList = this.imageList1;
            this.tankListView.TabIndex = 0;
            this.tankListView.UseCompatibleStateImageBehavior = false;
            this.tankListView.View = System.Windows.Forms.View.Details;
            // 
            // txtGroupName
            // 
            this.txtGroupName.Location = new System.Drawing.Point(82, 7);
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGroupName.Properties.Appearance.Options.UseFont = true;
            this.txtGroupName.Size = new System.Drawing.Size(411, 22);
            this.txtGroupName.TabIndex = 3;
            this.txtGroupName.TextChanged += new System.EventHandler(this.txtGroupName_TextChanged);
            // 
            // HTML_GROUPNAME
            // 
            this.HTML_GROUPNAME.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HTML_GROUPNAME.Location = new System.Drawing.Point(3, 10);
            this.HTML_GROUPNAME.Name = "HTML_GROUPNAME";
            this.HTML_GROUPNAME.Size = new System.Drawing.Size(73, 16);
            this.HTML_GROUPNAME.TabIndex = 2;
            this.HTML_GROUPNAME.Text = "Group Name";
            // 
            // BTN_CLOSE
            // 
            this.BTN_CLOSE.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_CLOSE.Appearance.Options.UseFont = true;
            this.BTN_CLOSE.Location = new System.Drawing.Point(418, 647);
            this.BTN_CLOSE.Name = "BTN_CLOSE";
            this.BTN_CLOSE.Size = new System.Drawing.Size(75, 23);
            this.BTN_CLOSE.TabIndex = 1;
            this.BTN_CLOSE.Text = "Close";
            this.BTN_CLOSE.Click += new System.EventHandler(this.Close_Click);
            // 
            // BTN_SAVE
            // 
            this.BTN_SAVE.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_SAVE.Appearance.Options.UseFont = true;
            this.BTN_SAVE.Location = new System.Drawing.Point(337, 647);
            this.BTN_SAVE.Name = "BTN_SAVE";
            this.BTN_SAVE.Size = new System.Drawing.Size(75, 23);
            this.BTN_SAVE.TabIndex = 0;
            this.BTN_SAVE.Text = "Save";
            this.BTN_SAVE.Click += new System.EventHandler(this.butSave_Click);
            // 
            // CustomGroupings
            // 
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 674);
            this.Controls.Add(this.HTML_GROUPNAME);
            this.Controls.Add(this.txtGroupName);
            this.Controls.Add(this.tankListView);
            this.Controls.Add(this.BTN_SAVE);
            this.Controls.Add(this.BTN_CLOSE);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.LookAndFeel.SkinName = "Office 2010 Black";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomGroupings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Custom Groupings";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.CustomGroupings_HelpButtonClicked);
            this.Load += new System.EventHandler(this.CustomGroupings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtGroupName.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraEditors.SimpleButton BTN_CLOSE;
        private DevExpress.XtraEditors.SimpleButton BTN_SAVE;
        private System.Windows.Forms.ListView tankListView;
        private DevExpress.XtraEditors.TextEdit txtGroupName;
        private DevExpress.XtraEditors.LabelControl HTML_GROUPNAME;
        private System.Windows.Forms.HelpProvider helpProvider1;
    }
}