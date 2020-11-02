namespace MoveImageAndPDFToMinIO
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
            this._stackingOrderPDFtxt = new System.Windows.Forms.TextBox();
            this._moveBtn = new System.Windows.Forms.Button();
            this._loanIDtxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._ephesoftOutputPathtxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _stackingOrderPDFtxt
            // 
            this._stackingOrderPDFtxt.Location = new System.Drawing.Point(153, 60);
            this._stackingOrderPDFtxt.Name = "_stackingOrderPDFtxt";
            this._stackingOrderPDFtxt.Size = new System.Drawing.Size(260, 20);
            this._stackingOrderPDFtxt.TabIndex = 0;
            // 
            // _moveBtn
            // 
            this._moveBtn.Location = new System.Drawing.Point(153, 121);
            this._moveBtn.Name = "_moveBtn";
            this._moveBtn.Size = new System.Drawing.Size(75, 23);
            this._moveBtn.TabIndex = 1;
            this._moveBtn.Text = "Move";
            this._moveBtn.UseVisualStyleBackColor = true;
            this._moveBtn.Click += new System.EventHandler(this._moveBtn_Click);
            // 
            // _loanIDtxt
            // 
            this._loanIDtxt.Location = new System.Drawing.Point(153, 34);
            this._loanIDtxt.Name = "_loanIDtxt";
            this._loanIDtxt.Size = new System.Drawing.Size(115, 20);
            this._loanIDtxt.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "LoanID :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "StackingOrderPDFPath :";
            // 
            // _ephesoftOutputPathtxt
            // 
            this._ephesoftOutputPathtxt.Location = new System.Drawing.Point(153, 86);
            this._ephesoftOutputPathtxt.Name = "_ephesoftOutputPathtxt";
            this._ephesoftOutputPathtxt.Size = new System.Drawing.Size(260, 20);
            this._ephesoftOutputPathtxt.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Ephesoft Output Path :";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 165);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._ephesoftOutputPathtxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._loanIDtxt);
            this.Controls.Add(this._moveBtn);
            this.Controls.Add(this._stackingOrderPDFtxt);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _stackingOrderPDFtxt;
        private System.Windows.Forms.Button _moveBtn;
        private System.Windows.Forms.TextBox _loanIDtxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _ephesoftOutputPathtxt;
        private System.Windows.Forms.Label label3;
    }
}

