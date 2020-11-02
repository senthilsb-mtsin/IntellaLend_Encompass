namespace DocumentEvaluator
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
            this.button1 = new System.Windows.Forms.Button();
            this.fileTxtBx = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.docFileds = new System.Windows.Forms.DataGridView();
            this.FieldName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FieldValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.docFileds)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(838, 44);
            this.button1.Margin = new System.Windows.Forms.Padding(6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(156, 44);
            this.button1.TabIndex = 0;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // fileTxtBx
            // 
            this.fileTxtBx.Location = new System.Drawing.Point(222, 46);
            this.fileTxtBx.Margin = new System.Windows.Forms.Padding(6);
            this.fileTxtBx.Name = "fileTxtBx";
            this.fileTxtBx.Size = new System.Drawing.Size(600, 31);
            this.fileTxtBx.TabIndex = 1;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "fileDlg";
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(24, 127);
            this.treeView1.Margin = new System.Windows.Forms.Padding(6);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(682, 633);
            this.treeView1.TabIndex = 4;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 52);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(195, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "Select Batch JSON";
            // 
            // docFileds
            // 
            this.docFileds.AllowUserToAddRows = false;
            this.docFileds.AllowUserToDeleteRows = false;
            this.docFileds.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.docFileds.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FieldName,
            this.FieldValue});
            this.docFileds.Location = new System.Drawing.Point(722, 127);
            this.docFileds.Margin = new System.Windows.Forms.Padding(6);
            this.docFileds.Name = "docFileds";
            this.docFileds.ReadOnly = true;
            this.docFileds.Size = new System.Drawing.Size(858, 637);
            this.docFileds.TabIndex = 6;
            // 
            // FieldName
            // 
            this.FieldName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FieldName.HeaderText = "Field Name";
            this.FieldName.MinimumWidth = 175;
            this.FieldName.Name = "FieldName";
            this.FieldName.ReadOnly = true;
            // 
            // FieldValue
            // 
            this.FieldValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FieldValue.HeaderText = "Field Value";
            this.FieldValue.MinimumWidth = 175;
            this.FieldValue.Name = "FieldValue";
            this.FieldValue.ReadOnly = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1046, 44);
            this.button2.Margin = new System.Windows.Forms.Padding(6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(156, 44);
            this.button2.TabIndex = 7;
            this.button2.Text = "Submit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1604, 787);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.docFileds);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.fileTxtBx);
            this.Controls.Add(this.treeView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Batch XML Viewer";
            ((System.ComponentModel.ISupportInitialize)(this.docFileds)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox fileTxtBx;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView docFileds;
        private System.Windows.Forms.DataGridViewTextBoxColumn FieldName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FieldValue;
        private System.Windows.Forms.Button button2;
    }
}

