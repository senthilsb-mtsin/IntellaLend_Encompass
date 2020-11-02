using MTSEntBlocks.UtilsBlock;
using Newtonsoft.Json;
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

namespace LicenseGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            object SelectedItem = comboBox1.SelectedItem;
            if (SelectedItem == null)
                SelectedItem = string.Empty;

            if (!string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrEmpty(textBox3.Text) && !string.IsNullOrEmpty(Convert.ToString(SelectedItem)))
            {
                Dictionary<string, string> _licConfig = new Dictionary<string, string>();
                _licConfig.Add("ID", new Random().Next(654982, 3215888).ToString());
                _licConfig.Add("SECRET_KEY", new Random().Next(654982, 3215888).ToString());
                _licConfig.Add("LICENSE_TYPE", Convert.ToString(SelectedItem).ToUpper());
                _licConfig.Add("LICENSE_CREATEDDATE", DateTime.Now.ToString());
                _licConfig.Add("TOTAL_USERS", textBox2.Text);
                _licConfig.Add("TOTAL_CONCURRENT_USERS", textBox3.Text);
                _licConfig.Add("LICENSE_EXPIRYDATE", new DateTime(2999, 12, 31).ToString());

                string jData = JsonConvert.SerializeObject(_licConfig);

                string _encryptedLicense = CommonUtils.EnDecrypt(jData);               

                File.WriteAllText("IntellaLend.lic", _encryptedLicense);
                MessageBox.Show("File Generated Successfully");

                //To Decrypt
                //string _decryptedLicense = CommonUtils.EnDecrypt(_encryptedLicense, true);
                //Dictionary<string, string> _test = JsonConvert.DeserializeObject<Dictionary<string, string>>(_decryptedLicense);
            }
            else
            {
                MessageBox.Show("License Type, No of Users & Concurrent Users Required");
            }
        }
    }
}
