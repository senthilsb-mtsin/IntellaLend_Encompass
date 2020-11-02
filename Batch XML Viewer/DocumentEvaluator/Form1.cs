using MTSXMLParsing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocumentEvaluator
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            UploadFile();
        }

        private void UploadFile()
        {
            OpenFileDialog fileDlg = new OpenFileDialog();
            System.Windows.Forms.DialogResult dlg = fileDlg.ShowDialog();
            if (dlg == DialogResult.OK)
            {
                fileTxtBx.Text = fileDlg.FileName;
                if (fileTxtBx.Text!="")
                {
                    string extensions = Path.GetExtension(fileTxtBx.Text);
                    if (extensions == ".json")
                    {
                        
                    }
                    else
                    {
                        MessageBox.Show("Only .json file is accepted");
                    }
                }
                else
                {
                    MessageBox.Show("Select File");
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                docFileds.Rows.Clear();
                List<DocumentLevelFields> docflds = e.Node.Tag as List<DocumentLevelFields>;
                foreach (DocumentLevelFields item in docflds)
                {
                    docFileds.Rows.Add(item.Name, item.Value);
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string fileText = File.ReadAllText(fileTxtBx.Text);
                Batch bObj = JsonConvert.DeserializeObject<Batch>(fileText);
                bObj.Documents.ForEach(elt =>
                {
                    TreeNode _tree = new TreeNode();
                    _tree.Text = $"{elt.Description} - {elt.Identifier}";
                    _tree.Tag = elt.DocumentLevelFields;
                    treeView1.Nodes.Add(_tree);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(textBox1.Text))
        //    {
        //        string _connectionString = "Database=RepublicBankTesting;Server=34.224.49.101;User ID=sa;Password=sadmin;Trusted_Connection=False";
        //        using (SqlConnection conn = new SqlConnection(_connectionString))
        //        {
        //            conn.Open();
        //            string sql = $"select top 1 LoanObject from[t1].loanDetails with(nolock)where LoanID = {textBox1.Text} order by 1";
        //            SqlCommand cmd = new SqlCommand(sql, conn);
        //            SqlDataReader _reader = cmd.ExecuteReader();
        //            System.Data.DataTable _dt = new System.Data.DataTable();
        //            _dt.Load(_reader);
        //            Batch bObj = JsonConvert.DeserializeObject<Batch>(_dt.Rows[0]["LoanObject"].ToString());

        //            if (!Directory.Exists("Output"))
        //                Directory.CreateDirectory("Output");

        //            File.WriteAllText($@"Output\{textBox1.Text}_Document_{DateTime.Now.ToString("MMddyyyyHHmmss")}.json",JsonConvert.SerializeObject(bObj.Documents[0]));
        //            File.WriteAllText($@"Output\{textBox1.Text}_Table_{DateTime.Now.ToString("MMddyyyyHHmmss")}.json", JsonConvert.SerializeObject(bObj.Documents[0].DataTables));

        //            bObj.Documents.ForEach(elt =>
        //            {
        //                TreeNode _tree = new TreeNode();
        //                _tree.Text = $"{elt.Type} - {elt.Identifier} - {elt.DocumentTypeID}";
        //                _tree.Tag = elt.DocumentLevelFields;
        //                treeView1.Nodes.Add(_tree);
        //            });
        //            conn.Close();
        //        }
        //    }
        //    else {
        //        MessageBox.Show("Enter Audit ID");
        //    }
        //}
    }
}
