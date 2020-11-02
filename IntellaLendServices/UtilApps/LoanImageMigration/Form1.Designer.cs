namespace LoanImageMigration
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
            this._fromLoanIDTextBox = new System.Windows.Forms.TextBox();
            this._countTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this._minIOURLTextBox = new System.Windows.Forms.TextBox();
            this._minIOAccessKeyTextBox = new System.Windows.Forms.TextBox();
            this._minIOSecretKeyTextBox = new System.Windows.Forms.TextBox();
            this._processBtn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this._recursiveCheckBox = new System.Windows.Forms.CheckBox();
            this._processingLabel = new System.Windows.Forms.Label();
            this._migrationDuration = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this._startTimeLabel = new System.Windows.Forms.Label();
            this._cancelBtn = new System.Windows.Forms.Button();
            this._endDateTimeLabel = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this._pageProcessingLabel = new System.Windows.Forms.Label();
            this._exceptionLoanIDs = new System.Windows.Forms.RichTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this._migratedLoanIDs = new System.Windows.Forms.RichTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this._resetBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _fromLoanIDTextBox
            // 
            this._fromLoanIDTextBox.Location = new System.Drawing.Point(147, 21);
            this._fromLoanIDTextBox.Name = "_fromLoanIDTextBox";
            this._fromLoanIDTextBox.Size = new System.Drawing.Size(61, 20);
            this._fromLoanIDTextBox.TabIndex = 0;
            // 
            // _countTextBox
            // 
            this._countTextBox.Location = new System.Drawing.Point(262, 21);
            this._countTextBox.Name = "_countTextBox";
            this._countTextBox.Size = new System.Drawing.Size(61, 20);
            this._countTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "From LoanID : ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "MinIO URL :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "MinIO Access Key :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "MinIO Secret Key :";
            // 
            // _minIOURLTextBox
            // 
            this._minIOURLTextBox.Location = new System.Drawing.Point(147, 55);
            this._minIOURLTextBox.Name = "_minIOURLTextBox";
            this._minIOURLTextBox.Size = new System.Drawing.Size(272, 20);
            this._minIOURLTextBox.TabIndex = 7;
            this._minIOURLTextBox.Text = "azwsapp02.az.ext:9000";
            // 
            // _minIOAccessKeyTextBox
            // 
            this._minIOAccessKeyTextBox.Location = new System.Drawing.Point(147, 88);
            this._minIOAccessKeyTextBox.Name = "_minIOAccessKeyTextBox";
            this._minIOAccessKeyTextBox.Size = new System.Drawing.Size(272, 20);
            this._minIOAccessKeyTextBox.TabIndex = 8;
            this._minIOAccessKeyTextBox.Text = "Intellalend";
            // 
            // _minIOSecretKeyTextBox
            // 
            this._minIOSecretKeyTextBox.Location = new System.Drawing.Point(147, 116);
            this._minIOSecretKeyTextBox.Name = "_minIOSecretKeyTextBox";
            this._minIOSecretKeyTextBox.Size = new System.Drawing.Size(272, 20);
            this._minIOSecretKeyTextBox.TabIndex = 9;
            this._minIOSecretKeyTextBox.Text = "12345678";
            // 
            // _processBtn
            // 
            this._processBtn.Location = new System.Drawing.Point(147, 154);
            this._processBtn.Name = "_processBtn";
            this._processBtn.Size = new System.Drawing.Size(75, 23);
            this._processBtn.TabIndex = 10;
            this._processBtn.Text = "Migrate";
            this._processBtn.UseVisualStyleBackColor = true;
            this._processBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(212, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Count : ";
            // 
            // _recursiveCheckBox
            // 
            this._recursiveCheckBox.AutoSize = true;
            this._recursiveCheckBox.Location = new System.Drawing.Point(345, 23);
            this._recursiveCheckBox.Name = "_recursiveCheckBox";
            this._recursiveCheckBox.Size = new System.Drawing.Size(74, 17);
            this._recursiveCheckBox.TabIndex = 12;
            this._recursiveCheckBox.Text = "Recursive";
            this._recursiveCheckBox.UseVisualStyleBackColor = true;
            // 
            // _processingLabel
            // 
            this._processingLabel.AutoSize = true;
            this._processingLabel.Location = new System.Drawing.Point(147, 285);
            this._processingLabel.Name = "_processingLabel";
            this._processingLabel.Size = new System.Drawing.Size(124, 13);
            this._processingLabel.TabIndex = 13;
            this._processingLabel.Text = "                                       ";
            // 
            // _migrationDuration
            // 
            this._migrationDuration.AutoSize = true;
            this._migrationDuration.Location = new System.Drawing.Point(144, 254);
            this._migrationDuration.Name = "_migrationDuration";
            this._migrationDuration.Size = new System.Drawing.Size(127, 13);
            this._migrationDuration.TabIndex = 14;
            this._migrationDuration.Text = "                                        ";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(24, 310);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(395, 23);
            this.progressBar1.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 254);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Duration :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 199);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Start Time :";
            // 
            // _startTimeLabel
            // 
            this._startTimeLabel.AutoSize = true;
            this._startTimeLabel.Location = new System.Drawing.Point(144, 199);
            this._startTimeLabel.Name = "_startTimeLabel";
            this._startTimeLabel.Size = new System.Drawing.Size(127, 13);
            this._startTimeLabel.TabIndex = 18;
            this._startTimeLabel.Text = "                                        ";
            // 
            // _cancelBtn
            // 
            this._cancelBtn.Location = new System.Drawing.Point(248, 154);
            this._cancelBtn.Name = "_cancelBtn";
            this._cancelBtn.Size = new System.Drawing.Size(75, 23);
            this._cancelBtn.TabIndex = 19;
            this._cancelBtn.Text = "Cancel";
            this._cancelBtn.UseVisualStyleBackColor = true;
            this._cancelBtn.Click += new System.EventHandler(this._cancelBtn_Click);
            // 
            // _endDateTimeLabel
            // 
            this._endDateTimeLabel.AutoSize = true;
            this._endDateTimeLabel.Location = new System.Drawing.Point(144, 227);
            this._endDateTimeLabel.Name = "_endDateTimeLabel";
            this._endDateTimeLabel.Size = new System.Drawing.Size(127, 13);
            this._endDateTimeLabel.TabIndex = 21;
            this._endDateTimeLabel.Text = "                                        ";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 227);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "End Time :";
            // 
            // _pageProcessingLabel
            // 
            this._pageProcessingLabel.AutoSize = true;
            this._pageProcessingLabel.Location = new System.Drawing.Point(119, 336);
            this._pageProcessingLabel.Name = "_pageProcessingLabel";
            this._pageProcessingLabel.Size = new System.Drawing.Size(0, 13);
            this._pageProcessingLabel.TabIndex = 22;
            // 
            // _exceptionLoanIDs
            // 
            this._exceptionLoanIDs.Location = new System.Drawing.Point(473, 40);
            this._exceptionLoanIDs.Name = "_exceptionLoanIDs";
            this._exceptionLoanIDs.ReadOnly = true;
            this._exceptionLoanIDs.Size = new System.Drawing.Size(153, 293);
            this._exceptionLoanIDs.TabIndex = 23;
            this._exceptionLoanIDs.Text = "";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(470, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(108, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "Exception Loan ID\'s :";
            // 
            // _migratedLoanIDs
            // 
            this._migratedLoanIDs.Location = new System.Drawing.Point(647, 40);
            this._migratedLoanIDs.Name = "_migratedLoanIDs";
            this._migratedLoanIDs.ReadOnly = true;
            this._migratedLoanIDs.Size = new System.Drawing.Size(153, 293);
            this._migratedLoanIDs.TabIndex = 25;
            this._migratedLoanIDs.Text = "";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(644, 24);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 13);
            this.label10.TabIndex = 26;
            this.label10.Text = "Migrated Loan ID\'s :";
            // 
            // _resetBtn
            // 
            this._resetBtn.Location = new System.Drawing.Point(344, 154);
            this._resetBtn.Name = "_resetBtn";
            this._resetBtn.Size = new System.Drawing.Size(75, 23);
            this._resetBtn.TabIndex = 27;
            this._resetBtn.Text = "Reset";
            this._resetBtn.UseVisualStyleBackColor = true;
            this._resetBtn.Click += new System.EventHandler(this._resetBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 372);
            this.Controls.Add(this._resetBtn);
            this.Controls.Add(this.label10);
            this.Controls.Add(this._migratedLoanIDs);
            this.Controls.Add(this.label8);
            this.Controls.Add(this._exceptionLoanIDs);
            this.Controls.Add(this._pageProcessingLabel);
            this.Controls.Add(this._endDateTimeLabel);
            this.Controls.Add(this.label9);
            this.Controls.Add(this._cancelBtn);
            this.Controls.Add(this._startTimeLabel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this._migrationDuration);
            this.Controls.Add(this._processingLabel);
            this.Controls.Add(this._recursiveCheckBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this._processBtn);
            this.Controls.Add(this._minIOSecretKeyTextBox);
            this.Controls.Add(this._minIOAccessKeyTextBox);
            this.Controls.Add(this._minIOURLTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._countTextBox);
            this.Controls.Add(this._fromLoanIDTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Loan Migration Tool";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _fromLoanIDTextBox;
        private System.Windows.Forms.TextBox _countTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox _minIOURLTextBox;
        private System.Windows.Forms.TextBox _minIOAccessKeyTextBox;
        private System.Windows.Forms.TextBox _minIOSecretKeyTextBox;
        private System.Windows.Forms.Button _processBtn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox _recursiveCheckBox;
        private System.Windows.Forms.Label _processingLabel;
        private System.Windows.Forms.Label _migrationDuration;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label _startTimeLabel;
        private System.Windows.Forms.Button _cancelBtn;
        private System.Windows.Forms.Label _endDateTimeLabel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label _pageProcessingLabel;
        private System.Windows.Forms.RichTextBox _exceptionLoanIDs;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RichTextBox _migratedLoanIDs;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button _resetBtn;
    }
}

