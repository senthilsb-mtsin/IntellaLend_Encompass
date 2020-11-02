using MTSEntBlocks.DataBlock;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetBatchIDs
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty) {
                List<string> batchIDs = new List<string>();
                string[] _files = Directory.GetFiles(textBox1.Text, "*.xml", SearchOption.AllDirectories);
                foreach (string file in _files)
                {
                    string path = Path.GetFileName(Path.GetDirectoryName(file));
                    richTextBox1.Text += path + Environment.NewLine;
                    batchIDs.Add($"'{path}'");
                }
                if (_files.Length > 0)
                    richTextBox3.Text = $"SELECT EphesoftBatchInstanceID, * FROM [T1].Loans where EphesoftBatchInstanceID in ({string.Join(",", batchIDs)})";

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty)
            {
                List<string> batchIDs = new List<string>();
                string[] _files = Directory.GetFiles(textBox1.Text, "*.error", SearchOption.AllDirectories);
                foreach (string file in _files)
                {
                    string path = Path.GetFileName(Path.GetDirectoryName(file));
                    richTextBox2.Text += path + Environment.NewLine;
                    batchIDs.Add($"'{path}'");
                }

                if (_files.Length > 0)
                    richTextBox3.Text = $"SELECT EphesoftBatchInstanceID, * FROM [T1].Loans where EphesoftBatchInstanceID in ({string.Join(",", batchIDs)})";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                new DataAccess2("IntellaLendReportingDB").ExecuteDataSet("Select top 1 * from [t1].loans");
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty)
            {
                List<string> batchIDs = new List<string>();
                string[] _files = Directory.GetFiles(textBox1.Text, "*.lck", SearchOption.AllDirectories);
                foreach (string file in _files)
                {
                    string path = Path.GetFileName(Path.GetDirectoryName(file));
                    richTextBox4.Text += path + Environment.NewLine;
                    batchIDs.Add($"'{path}'");
                }

                if (_files.Length > 0)
                    richTextBox3.Text = $"SELECT EphesoftBatchInstanceID, * FROM [T1].Loans where EphesoftBatchInstanceID in ({string.Join(",", batchIDs)})";
            }
        }
    }
}
