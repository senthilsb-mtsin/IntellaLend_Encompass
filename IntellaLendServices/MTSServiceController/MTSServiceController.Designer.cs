namespace MTSServiceController
{
    partial class MTSServiceController
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MTSServiceController));
            this.tbServicePurge = new System.Windows.Forms.TabControl();
            this.tbServiceController = new System.Windows.Forms.TabPage();
            this.tbCtrl = new System.Windows.Forms.TabControl();
            this.tbPageServiceLog = new System.Windows.Forms.TabPage();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.tbPropertyGrid = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.lstServices = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnRunOnce = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnInstall = new System.Windows.Forms.Button();
            this.btnUninstall = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.tbPurge = new System.Windows.Forms.TabPage();
            this.tbServicePurge.SuspendLayout();
            this.tbServiceController.SuspendLayout();
            this.tbCtrl.SuspendLayout();
            this.tbPageServiceLog.SuspendLayout();
            this.tbPropertyGrid.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbServicePurge
            // 
            this.tbServicePurge.Controls.Add(this.tbServiceController);
            this.tbServicePurge.Controls.Add(this.tbPurge);
            this.tbServicePurge.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbServicePurge.Location = new System.Drawing.Point(-16, 2);
            this.tbServicePurge.Margin = new System.Windows.Forms.Padding(6);
            this.tbServicePurge.Name = "tbServicePurge";
            this.tbServicePurge.SelectedIndex = 0;
            this.tbServicePurge.ShowToolTips = true;
            this.tbServicePurge.Size = new System.Drawing.Size(2196, 877);
            this.tbServicePurge.TabIndex = 0;
            this.tbServicePurge.SelectedIndexChanged += new System.EventHandler(this.tbServicePurge_SelectedIndexChanged);
            // 
            // tbServiceController
            // 
            this.tbServiceController.BackColor = System.Drawing.Color.LightSlateGray;
            this.tbServiceController.Controls.Add(this.tbCtrl);
            this.tbServiceController.Controls.Add(this.lstServices);
            this.tbServiceController.Controls.Add(this.panel2);
            this.tbServiceController.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbServiceController.Location = new System.Drawing.Point(8, 43);
            this.tbServiceController.Margin = new System.Windows.Forms.Padding(6);
            this.tbServiceController.Name = "tbServiceController";
            this.tbServiceController.Padding = new System.Windows.Forms.Padding(6);
            this.tbServiceController.Size = new System.Drawing.Size(2180, 826);
            this.tbServiceController.TabIndex = 0;
            this.tbServiceController.Text = "Service Controller";
            // 
            // tbCtrl
            // 
            this.tbCtrl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tbCtrl.Controls.Add(this.tbPageServiceLog);
            this.tbCtrl.Controls.Add(this.tbPropertyGrid);
            this.tbCtrl.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCtrl.Location = new System.Drawing.Point(832, 150);
            this.tbCtrl.Margin = new System.Windows.Forms.Padding(6);
            this.tbCtrl.Name = "tbCtrl";
            this.tbCtrl.SelectedIndex = 0;
            this.tbCtrl.ShowToolTips = true;
            this.tbCtrl.Size = new System.Drawing.Size(1348, 685);
            this.tbCtrl.TabIndex = 12;
            // 
            // tbPageServiceLog
            // 
            this.tbPageServiceLog.Controls.Add(this.txtLog);
            this.tbPageServiceLog.Location = new System.Drawing.Point(4, 44);
            this.tbPageServiceLog.Margin = new System.Windows.Forms.Padding(6);
            this.tbPageServiceLog.Name = "tbPageServiceLog";
            this.tbPageServiceLog.Padding = new System.Windows.Forms.Padding(6);
            this.tbPageServiceLog.Size = new System.Drawing.Size(1340, 637);
            this.tbPageServiceLog.TabIndex = 0;
            this.tbPageServiceLog.Text = "Service Log";
            this.tbPageServiceLog.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLog.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.Location = new System.Drawing.Point(12, 12);
            this.txtLog.Margin = new System.Windows.Forms.Padding(6);
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(1306, 612);
            this.txtLog.TabIndex = 7;
            this.txtLog.Text = "";
            // 
            // tbPropertyGrid
            // 
            this.tbPropertyGrid.AutoScroll = true;
            this.tbPropertyGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tbPropertyGrid.Controls.Add(this.panel1);
            this.tbPropertyGrid.Location = new System.Drawing.Point(4, 44);
            this.tbPropertyGrid.Margin = new System.Windows.Forms.Padding(6);
            this.tbPropertyGrid.Name = "tbPropertyGrid";
            this.tbPropertyGrid.Padding = new System.Windows.Forms.Padding(6);
            this.tbPropertyGrid.Size = new System.Drawing.Size(1340, 637);
            this.tbPropertyGrid.TabIndex = 2;
            this.tbPropertyGrid.Text = "Service Configuration";
            this.tbPropertyGrid.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.AutoScrollMinSize = new System.Drawing.Size(500, 0);
            this.panel1.Controls.Add(this.propertyGrid1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Margin = new System.Windows.Forms.Padding(6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1324, 621);
            this.panel1.TabIndex = 0;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.propertyGrid1.Location = new System.Drawing.Point(20, 33);
            this.propertyGrid1.Margin = new System.Windows.Forms.Padding(6);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.propertyGrid1.Size = new System.Drawing.Size(1272, 538);
            this.propertyGrid1.TabIndex = 1;
            this.propertyGrid1.ToolbarVisible = false;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // lstServices
            // 
            this.lstServices.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.lstServices.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstServices.FormattingEnabled = true;
            this.lstServices.ItemHeight = 32;
            this.lstServices.Location = new System.Drawing.Point(12, 150);
            this.lstServices.Margin = new System.Windows.Forms.Padding(6);
            this.lstServices.Name = "lstServices";
            this.lstServices.Size = new System.Drawing.Size(812, 676);
            this.lstServices.TabIndex = 11;
            this.lstServices.SelectedIndexChanged += new System.EventHandler(this.lstServices_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.btnRunOnce);
            this.panel2.Controls.Add(this.btnStart);
            this.panel2.Controls.Add(this.btnInstall);
            this.panel2.Controls.Add(this.btnUninstall);
            this.panel2.Controls.Add(this.btnStop);
            this.panel2.Controls.Add(this.btnExit);
            this.panel2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel2.Location = new System.Drawing.Point(4, -12);
            this.panel2.Margin = new System.Windows.Forms.Padding(6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(2180, 146);
            this.panel2.TabIndex = 10;
            // 
            // btnRunOnce
            // 
            this.btnRunOnce.BackColor = System.Drawing.Color.Lavender;
            this.btnRunOnce.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnRunOnce.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunOnce.Location = new System.Drawing.Point(752, 38);
            this.btnRunOnce.Margin = new System.Windows.Forms.Padding(6);
            this.btnRunOnce.Name = "btnRunOnce";
            this.btnRunOnce.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnRunOnce.Size = new System.Drawing.Size(280, 69);
            this.btnRunOnce.TabIndex = 6;
            this.btnRunOnce.Text = "Run Once";
            this.btnRunOnce.UseVisualStyleBackColor = false;
            this.btnRunOnce.Click += new System.EventHandler(this.btnRunOnce_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.Lavender;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnStart.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(44, 38);
            this.btnStart.Margin = new System.Windows.Forms.Padding(6);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(280, 69);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnInstall
            // 
            this.btnInstall.BackColor = System.Drawing.Color.Lavender;
            this.btnInstall.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnInstall.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInstall.Location = new System.Drawing.Point(1106, 38);
            this.btnInstall.Margin = new System.Windows.Forms.Padding(6);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(280, 69);
            this.btnInstall.TabIndex = 3;
            this.btnInstall.Text = "Install";
            this.btnInstall.UseVisualStyleBackColor = false;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // btnUninstall
            // 
            this.btnUninstall.BackColor = System.Drawing.Color.Lavender;
            this.btnUninstall.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnUninstall.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUninstall.Location = new System.Drawing.Point(1460, 38);
            this.btnUninstall.Margin = new System.Windows.Forms.Padding(6);
            this.btnUninstall.Name = "btnUninstall";
            this.btnUninstall.Size = new System.Drawing.Size(280, 69);
            this.btnUninstall.TabIndex = 4;
            this.btnUninstall.Text = "Un-Install";
            this.btnUninstall.UseVisualStyleBackColor = false;
            this.btnUninstall.Click += new System.EventHandler(this.btnUninstall_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.Lavender;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnStop.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Location = new System.Drawing.Point(398, 38);
            this.btnStop.Margin = new System.Windows.Forms.Padding(6);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(280, 69);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Lavender;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(1814, 38);
            this.btnExit.Margin = new System.Windows.Forms.Padding(6);
            this.btnExit.Name = "btnExit";
            this.btnExit.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnExit.Size = new System.Drawing.Size(280, 69);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // tbPurge
            // 
            this.tbPurge.BackColor = System.Drawing.Color.LightSlateGray;
            this.tbPurge.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPurge.Location = new System.Drawing.Point(8, 43);
            this.tbPurge.Margin = new System.Windows.Forms.Padding(6);
            this.tbPurge.Name = "tbPurge";
            this.tbPurge.Padding = new System.Windows.Forms.Padding(6);
            this.tbPurge.Size = new System.Drawing.Size(2180, 826);
            this.tbPurge.TabIndex = 1;
            this.tbPurge.Text = "Purge";
            // 
            // MTSServiceController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.LightSlateGray;
            this.ClientSize = new System.Drawing.Size(2192, 892);
            this.Controls.Add(this.tbServicePurge);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "MTSServiceController";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Service Controller";
            this.Load += new System.EventHandler(this.MTSServiceController_Load);
            this.ResizeBegin += new System.EventHandler(this.MTSServiceController_ResizeBegin);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MTSServiceController_KeyDown);
            this.Resize += new System.EventHandler(this.MTSServiceController_Resize);
            this.tbServicePurge.ResumeLayout(false);
            this.tbServiceController.ResumeLayout(false);
            this.tbCtrl.ResumeLayout(false);
            this.tbPageServiceLog.ResumeLayout(false);
            this.tbPropertyGrid.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbServicePurge;
        private System.Windows.Forms.TabPage tbServiceController;
        private System.Windows.Forms.TabPage tbPurge;
        private System.Windows.Forms.ListBox lstServices;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.Button btnUninstall;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TabControl tbCtrl;
        private System.Windows.Forms.TabPage tbPageServiceLog;
        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.TabPage tbPropertyGrid;
        private System.Windows.Forms.Button btnRunOnce;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
    }
}