namespace DatabaseMigrationTool
{
    partial class Form1
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this._resetBtn = new System.Windows.Forms.Button();
            this._testConnectionBtn = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this._tenantTableProgressGrid = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._sysTableProgressGrid = new System.Windows.Forms.DataGridView();
            this.TableName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ErrorMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._destDatabaseTxt = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this._sourceDatabaseTxt = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this._migrateBtn = new System.Windows.Forms.Button();
            this._destPwdTxt = new System.Windows.Forms.TextBox();
            this._destUserNameTxt = new System.Windows.Forms.TextBox();
            this._destServerTxt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this._sourcePwdTxt = new System.Windows.Forms.TextBox();
            this._sourceUserNameTxt = new System.Windows.Forms.TextBox();
            this._sourceServerTxt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this._plsWaitLabel = new System.Windows.Forms.Label();
            this._moveLoanType = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this._sourceLoanTypes = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._tenantTableProgressGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._sysTableProgressGrid)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1051, 530);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Click += new System.EventHandler(this._moveLoanTypeTab_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this._resetBtn);
            this.tabPage1.Controls.Add(this._testConnectionBtn);
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Controls.Add(this._tenantTableProgressGrid);
            this.tabPage1.Controls.Add(this._sysTableProgressGrid);
            this.tabPage1.Controls.Add(this._destDatabaseTxt);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this._sourceDatabaseTxt);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this._migrateBtn);
            this.tabPage1.Controls.Add(this._destPwdTxt);
            this.tabPage1.Controls.Add(this._destUserNameTxt);
            this.tabPage1.Controls.Add(this._destServerTxt);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this._sourcePwdTxt);
            this.tabPage1.Controls.Add(this._sourceUserNameTxt);
            this.tabPage1.Controls.Add(this._sourceServerTxt);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1043, 504);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Migrate Database";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // _resetBtn
            // 
            this._resetBtn.Location = new System.Drawing.Point(445, 121);
            this._resetBtn.Name = "_resetBtn";
            this._resetBtn.Size = new System.Drawing.Size(113, 23);
            this._resetBtn.TabIndex = 48;
            this._resetBtn.Text = "Reset";
            this._resetBtn.UseVisualStyleBackColor = true;
            // 
            // _testConnectionBtn
            // 
            this._testConnectionBtn.Location = new System.Drawing.Point(445, 91);
            this._testConnectionBtn.Name = "_testConnectionBtn";
            this._testConnectionBtn.Size = new System.Drawing.Size(113, 23);
            this._testConnectionBtn.TabIndex = 47;
            this._testConnectionBtn.Text = "Test Connection";
            this._testConnectionBtn.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(912, 186);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(82, 13);
            this.label12.TabIndex = 46;
            this.label12.Text = "Tenant Tables :";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(49, 186);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(82, 13);
            this.label11.TabIndex = 45;
            this.label11.Text = "System Tables :";
            // 
            // _tenantTableProgressGrid
            // 
            this._tenantTableProgressGrid.AllowUserToAddRows = false;
            this._tenantTableProgressGrid.AllowUserToDeleteRows = false;
            this._tenantTableProgressGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._tenantTableProgressGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this._tenantTableProgressGrid.Location = new System.Drawing.Point(528, 212);
            this._tenantTableProgressGrid.Name = "_tenantTableProgressGrid";
            this._tenantTableProgressGrid.ReadOnly = true;
            this._tenantTableProgressGrid.Size = new System.Drawing.Size(466, 269);
            this._tenantTableProgressGrid.TabIndex = 44;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "TableName";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 205;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Status";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "ErrorMessage";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // _sysTableProgressGrid
            // 
            this._sysTableProgressGrid.AllowUserToAddRows = false;
            this._sysTableProgressGrid.AllowUserToDeleteRows = false;
            this._sysTableProgressGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._sysTableProgressGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TableName,
            this.Status,
            this.ErrorMessage});
            this._sysTableProgressGrid.Location = new System.Drawing.Point(52, 212);
            this._sysTableProgressGrid.Name = "_sysTableProgressGrid";
            this._sysTableProgressGrid.ReadOnly = true;
            this._sysTableProgressGrid.Size = new System.Drawing.Size(470, 269);
            this._sysTableProgressGrid.TabIndex = 43;
            // 
            // TableName
            // 
            this.TableName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.TableName.Frozen = true;
            this.TableName.HeaderText = "TableName";
            this.TableName.MinimumWidth = 50;
            this.TableName.Name = "TableName";
            this.TableName.ReadOnly = true;
            this.TableName.Width = 205;
            // 
            // Status
            // 
            this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Status.Frozen = true;
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // ErrorMessage
            // 
            this.ErrorMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ErrorMessage.Frozen = true;
            this.ErrorMessage.HeaderText = "ErrorMessage";
            this.ErrorMessage.Name = "ErrorMessage";
            this.ErrorMessage.ReadOnly = true;
            // 
            // _destDatabaseTxt
            // 
            this._destDatabaseTxt.Location = new System.Drawing.Point(679, 124);
            this._destDatabaseTxt.Name = "_destDatabaseTxt";
            this._destDatabaseTxt.Size = new System.Drawing.Size(267, 20);
            this._destDatabaseTxt.TabIndex = 36;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(585, 127);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(90, 13);
            this.label10.TabIndex = 42;
            this.label10.Text = "Database Name :";
            // 
            // _sourceDatabaseTxt
            // 
            this._sourceDatabaseTxt.Location = new System.Drawing.Point(143, 124);
            this._sourceDatabaseTxt.Name = "_sourceDatabaseTxt";
            this._sourceDatabaseTxt.Size = new System.Drawing.Size(267, 20);
            this._sourceDatabaseTxt.TabIndex = 31;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(49, 127);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(90, 13);
            this.label9.TabIndex = 41;
            this.label9.Text = "Database Name :";
            // 
            // _migrateBtn
            // 
            this._migrateBtn.Location = new System.Drawing.Point(445, 176);
            this._migrateBtn.Name = "_migrateBtn";
            this._migrateBtn.Size = new System.Drawing.Size(113, 23);
            this._migrateBtn.TabIndex = 38;
            this._migrateBtn.Text = "Migrate";
            this._migrateBtn.UseVisualStyleBackColor = true;
            // 
            // _destPwdTxt
            // 
            this._destPwdTxt.Location = new System.Drawing.Point(679, 98);
            this._destPwdTxt.Name = "_destPwdTxt";
            this._destPwdTxt.PasswordChar = '*';
            this._destPwdTxt.Size = new System.Drawing.Size(267, 20);
            this._destPwdTxt.TabIndex = 34;
            // 
            // _destUserNameTxt
            // 
            this._destUserNameTxt.Location = new System.Drawing.Point(679, 74);
            this._destUserNameTxt.Name = "_destUserNameTxt";
            this._destUserNameTxt.Size = new System.Drawing.Size(267, 20);
            this._destUserNameTxt.TabIndex = 33;
            // 
            // _destServerTxt
            // 
            this._destServerTxt.Location = new System.Drawing.Point(679, 49);
            this._destServerTxt.Name = "_destServerTxt";
            this._destServerTxt.Size = new System.Drawing.Size(267, 20);
            this._destServerTxt.TabIndex = 32;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(585, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 40;
            this.label5.Text = "Password :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(585, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 39;
            this.label6.Text = "Username :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(585, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 13);
            this.label7.TabIndex = 37;
            this.label7.Text = "Server Name :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(585, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(115, 13);
            this.label8.TabIndex = 35;
            this.label8.Text = "Destination Database :";
            // 
            // _sourcePwdTxt
            // 
            this._sourcePwdTxt.Location = new System.Drawing.Point(143, 98);
            this._sourcePwdTxt.Name = "_sourcePwdTxt";
            this._sourcePwdTxt.PasswordChar = '*';
            this._sourcePwdTxt.Size = new System.Drawing.Size(267, 20);
            this._sourcePwdTxt.TabIndex = 30;
            // 
            // _sourceUserNameTxt
            // 
            this._sourceUserNameTxt.Location = new System.Drawing.Point(143, 74);
            this._sourceUserNameTxt.Name = "_sourceUserNameTxt";
            this._sourceUserNameTxt.Size = new System.Drawing.Size(267, 20);
            this._sourceUserNameTxt.TabIndex = 28;
            // 
            // _sourceServerTxt
            // 
            this._sourceServerTxt.Location = new System.Drawing.Point(143, 49);
            this._sourceServerTxt.Name = "_sourceServerTxt";
            this._sourceServerTxt.Size = new System.Drawing.Size(267, 20);
            this._sourceServerTxt.TabIndex = 25;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(49, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 29;
            this.label4.Text = "Password :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(49, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "Username :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(49, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "Server Name :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Source Database :";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this._plsWaitLabel);
            this.tabPage2.Controls.Add(this._moveLoanType);
            this.tabPage2.Controls.Add(this.label13);
            this.tabPage2.Controls.Add(this._sourceLoanTypes);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1043, 504);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Migrate LoanType";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // _plsWaitLabel
            // 
            this._plsWaitLabel.AutoSize = true;
            this._plsWaitLabel.Location = new System.Drawing.Point(136, 65);
            this._plsWaitLabel.Name = "_plsWaitLabel";
            this._plsWaitLabel.Size = new System.Drawing.Size(176, 13);
            this._plsWaitLabel.TabIndex = 3;
            this._plsWaitLabel.Text = "Moving Loan type.... Please wait.....";
            // 
            // _moveLoanType
            // 
            this._moveLoanType.Location = new System.Drawing.Point(509, 23);
            this._moveLoanType.Name = "_moveLoanType";
            this._moveLoanType.Size = new System.Drawing.Size(127, 23);
            this._moveLoanType.TabIndex = 2;
            this._moveLoanType.Text = "Move To Destination";
            this._moveLoanType.UseVisualStyleBackColor = true;
            this._moveLoanType.Click += new System.EventHandler(this._moveLoanType_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(17, 28);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(103, 13);
            this.label13.TabIndex = 1;
            this.label13.Text = "Source LoanTypes :";
            // 
            // _sourceLoanTypes
            // 
            this._sourceLoanTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._sourceLoanTypes.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._sourceLoanTypes.Location = new System.Drawing.Point(136, 25);
            this._sourceLoanTypes.Name = "_sourceLoanTypes";
            this._sourceLoanTypes.Size = new System.Drawing.Size(367, 21);
            this._sourceLoanTypes.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1075, 545);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Database Migration Tool";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._tenantTableProgressGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._sysTableProgressGrid)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TabControl tabControl1;
        public System.Windows.Forms.TabPage tabPage1;
        public System.Windows.Forms.Button _resetBtn;
        public System.Windows.Forms.Button _testConnectionBtn;
        public System.Windows.Forms.Label label12;
        public System.Windows.Forms.Label label11;
        public System.Windows.Forms.DataGridView _tenantTableProgressGrid;
        public System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        public System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        public System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        public System.Windows.Forms.DataGridView _sysTableProgressGrid;
        public System.Windows.Forms.DataGridViewTextBoxColumn TableName;
        public System.Windows.Forms.DataGridViewTextBoxColumn Status;
        public System.Windows.Forms.DataGridViewTextBoxColumn ErrorMessage;
        public System.Windows.Forms.TextBox _destDatabaseTxt;
        public System.Windows.Forms.Label label10;
        public System.Windows.Forms.TextBox _sourceDatabaseTxt;
        public System.Windows.Forms.Label label9;
        public System.Windows.Forms.Button _migrateBtn;
        public System.Windows.Forms.TextBox _destPwdTxt;
        public System.Windows.Forms.TextBox _destUserNameTxt;
        public System.Windows.Forms.TextBox _destServerTxt;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.Label label6;
        public System.Windows.Forms.Label label7;
        public System.Windows.Forms.Label label8;
        public System.Windows.Forms.TextBox _sourcePwdTxt;
        public System.Windows.Forms.TextBox _sourceUserNameTxt;
        public System.Windows.Forms.TextBox _sourceServerTxt;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.TabPage tabPage2;
        public System.Windows.Forms.Button _moveLoanType;
        public System.Windows.Forms.Label label13;
        public System.Windows.Forms.ComboBox _sourceLoanTypes;
        public System.Windows.Forms.Label _plsWaitLabel;
    }
}

