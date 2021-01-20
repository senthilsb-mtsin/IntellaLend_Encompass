using MTS.ServiceBase;
using MTSEntBlocks.DataBlock;
using System;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
//using EllieMae.Encompass.Client;

namespace ServiceDebugger
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //new EllieMae.Encompass.Runtime.RuntimeServices().Initialize();
        }


        //public static void RestOfProgram()
        //{
        //    string server = "https://TEBE11153533.ea.elliemae.net$TEBE11153533";
        //    string loginName = "skharidi";
        //    string password = "Mts@1234";

        //    try
        //    {
        //        // Start the session
        //        Session s = new Session();

        //        s.Start(server, loginName, password);

        //        Console.WriteLine(s.ID);



        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex.Message);
        //    }

        //    Console.Write("Success!!!");
        //    Console.ReadKey();
        //}




        private void button1_Click(object sender, EventArgs e)
        {
            // RestOfProgram();

            object objService = null;
            Type objtype = null;
            //EmailTrackerService dd = new EmailTrackerService();
            //dd.OnStart("");
            //dd.DoTask();
            DataTable dtServiceInfo = new DataTable();
            try
            {
                dtServiceInfo = DataAccess.ExecuteDataTable("[IL].[GETSERVICECONFIGFORSERVICE]", "CalHFA_ImportToIntellaLend"); //serviceList.Text);
                string DLLName = dtServiceInfo.Rows[0]["DLLName"].ToString();
                string ServiceParams = dtServiceInfo.Rows[0]["ServiceParams"].ToString();
                Assembly ServiceDll = Assembly.LoadFile(Environment.CurrentDirectory + @"\" + DLLName);

                foreach (Type type in ServiceDll.GetTypes())
                {
                    if (typeof(IMTSServiceBase).IsAssignableFrom(type))
                    {
                        objtype = type;
                        break;
                    }
                }
                objService = Activator.CreateInstance(objtype);
                objtype.InvokeMember("OnStart", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, objService, new object[] { ServiceParams });
                objtype.InvokeMember("DoTask", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, objService, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void serviceList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
