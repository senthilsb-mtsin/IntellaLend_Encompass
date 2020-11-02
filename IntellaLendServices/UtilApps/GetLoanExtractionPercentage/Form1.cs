using MTSEntBlocks.DataBlock;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetLoanExtractionPercentage
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Batch Class ID & Batch Instance ID are Required");
            }
            else {
                GetExtractionPercentage();
            }
        }


        private void GetExtractionPercentage()
        {
            // string _sql = File.ReadAllText(Path.Combine( Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "GetLoanExtractionPercentage.sql"));
            string _sql = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "TestSQL.sql"));

            _sql = _sql.Replace("{LOANID}", 3.ToString());

            //_sql = _sql.Replace("{BATCHCLASSID}", textBox1.Text).Replace("{BATCHINSTANCEID}", textBox2.Text);

            DataSet _ds = DynamicDataAccess.ExecuteSQLDataSet(ConfigurationManager.ConnectionStrings["DBConnection"].ToString(), _sql);
        }
    }
}
