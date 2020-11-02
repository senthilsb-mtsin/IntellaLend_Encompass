namespace MigrationMoveExceptionPDF
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
            this._cancelBtn = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this._exceptionLoanIDs = new System.Windows.Forms.RichTextBox();
            this._processingLabel = new System.Windows.Forms.Label();
            this._startTimeLabel = new System.Windows.Forms.Label();
            this._migrationDuration = new System.Windows.Forms.Label();
            this._migrationDurationLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this._processBtn = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label5 = new System.Windows.Forms.Label();
            this._exceptionPDFCount = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._minIOSecretKeyTextBox = new System.Windows.Forms.TextBox();
            this._minIOAccessKeyTextBox = new System.Windows.Forms.TextBox();
            this._minIOURLTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _cancelBtn
            // 
            this._cancelBtn.Location = new System.Drawing.Point(258, 340);
            this._cancelBtn.Name = "_cancelBtn";
            this._cancelBtn.Size = new System.Drawing.Size(75, 23);
            this._cancelBtn.TabIndex = 46;
            this._cancelBtn.Text = "Cancel";
            this._cancelBtn.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(470, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 13);
            this.label7.TabIndex = 45;
            this.label7.Text = "Exception IDs :";
            // 
            // _exceptionLoanIDs
            // 
            this._exceptionLoanIDs.Location = new System.Drawing.Point(473, 51);
            this._exceptionLoanIDs.Name = "_exceptionLoanIDs";
            this._exceptionLoanIDs.Size = new System.Drawing.Size(204, 341);
            this._exceptionLoanIDs.TabIndex = 44;
            this._exceptionLoanIDs.Text = "";
            // 
            // _processingLabel
            // 
            this._processingLabel.AutoSize = true;
            this._processingLabel.Location = new System.Drawing.Point(202, 185);
            this._processingLabel.Name = "_processingLabel";
            this._processingLabel.Size = new System.Drawing.Size(0, 13);
            this._processingLabel.TabIndex = 43;
            // 
            // _startTimeLabel
            // 
            this._startTimeLabel.AutoSize = true;
            this._startTimeLabel.Location = new System.Drawing.Point(212, 224);
            this._startTimeLabel.Name = "_startTimeLabel";
            this._startTimeLabel.Size = new System.Drawing.Size(0, 13);
            this._startTimeLabel.TabIndex = 42;
            // 
            // _migrationDuration
            // 
            this._migrationDuration.AutoSize = true;
            this._migrationDuration.Location = new System.Drawing.Point(234, 266);
            this._migrationDuration.Name = "_migrationDuration";
            this._migrationDuration.Size = new System.Drawing.Size(0, 13);
            this._migrationDuration.TabIndex = 41;
            // 
            // _migrationDurationLabel
            // 
            this._migrationDurationLabel.AutoSize = true;
            this._migrationDurationLabel.Location = new System.Drawing.Point(90, 266);
            this._migrationDurationLabel.Name = "_migrationDurationLabel";
            this._migrationDurationLabel.Size = new System.Drawing.Size(56, 13);
            this._migrationDurationLabel.TabIndex = 40;
            this._migrationDurationLabel.Text = "Duration : ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(85, 224);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 39;
            this.label6.Text = "Strat Time : ";
            // 
            // _processBtn
            // 
            this._processBtn.Location = new System.Drawing.Point(159, 340);
            this._processBtn.Name = "_processBtn";
            this._processBtn.Size = new System.Drawing.Size(75, 23);
            this._processBtn.TabIndex = 38;
            this._processBtn.Text = "Migrate";
            this._processBtn.UseVisualStyleBackColor = true;
            this._processBtn.Click += new System.EventHandler(this._processBtn_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(88, 369);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(344, 23);
            this.progressBar1.TabIndex = 37;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(85, 185);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 13);
            this.label5.TabIndex = 36;
            this.label5.Text = "Processing PDFID : ";
            // 
            // _exceptionPDFCount
            // 
            this._exceptionPDFCount.AutoSize = true;
            this._exceptionPDFCount.Location = new System.Drawing.Point(201, 149);
            this._exceptionPDFCount.Name = "_exceptionPDFCount";
            this._exceptionPDFCount.Size = new System.Drawing.Size(0, 13);
            this._exceptionPDFCount.TabIndex = 35;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(85, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 34;
            this.label4.Text = "Total PDFs : ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(85, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 33;
            this.label3.Text = "SecretKey";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(85, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 32;
            this.label2.Text = "AccessKey";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(85, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "Url :";
            // 
            // _minIOSecretKeyTextBox
            // 
            this._minIOSecretKeyTextBox.Location = new System.Drawing.Point(204, 84);
            this._minIOSecretKeyTextBox.Name = "_minIOSecretKeyTextBox";
            this._minIOSecretKeyTextBox.Size = new System.Drawing.Size(228, 20);
            this._minIOSecretKeyTextBox.TabIndex = 30;
            this._minIOSecretKeyTextBox.Text = "12345678";
            // 
            // _minIOAccessKeyTextBox
            // 
            this._minIOAccessKeyTextBox.Location = new System.Drawing.Point(204, 51);
            this._minIOAccessKeyTextBox.Name = "_minIOAccessKeyTextBox";
            this._minIOAccessKeyTextBox.Size = new System.Drawing.Size(228, 20);
            this._minIOAccessKeyTextBox.TabIndex = 29;
            this._minIOAccessKeyTextBox.Text = "Intellalend";
            // 
            // _minIOURLTextBox
            // 
            this._minIOURLTextBox.Location = new System.Drawing.Point(204, 25);
            this._minIOURLTextBox.Name = "_minIOURLTextBox";
            this._minIOURLTextBox.Size = new System.Drawing.Size(228, 20);
            this._minIOURLTextBox.TabIndex = 28;
            this._minIOURLTextBox.Text = "azwsapp02.az.ext:9000";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 442);
            this.Controls.Add(this._cancelBtn);
            this.Controls.Add(this.label7);
            this.Controls.Add(this._exceptionLoanIDs);
            this.Controls.Add(this._processingLabel);
            this.Controls.Add(this._startTimeLabel);
            this.Controls.Add(this._migrationDuration);
            this.Controls.Add(this._migrationDurationLabel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this._processBtn);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this._exceptionPDFCount);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._minIOSecretKeyTextBox);
            this.Controls.Add(this._minIOAccessKeyTextBox);
            this.Controls.Add(this._minIOURLTextBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _cancelBtn;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RichTextBox _exceptionLoanIDs;
        private System.Windows.Forms.Label _processingLabel;
        private System.Windows.Forms.Label _startTimeLabel;
        private System.Windows.Forms.Label _migrationDuration;
        private System.Windows.Forms.Label _migrationDurationLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button _processBtn;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label _exceptionPDFCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _minIOSecretKeyTextBox;
        private System.Windows.Forms.TextBox _minIOAccessKeyTextBox;
        private System.Windows.Forms.TextBox _minIOURLTextBox;
    }
}

