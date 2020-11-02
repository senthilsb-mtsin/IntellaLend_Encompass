
using EllieMae.Encompass.Client;
using IntellaLend.Constance;
using IntellaLend.EncompassWrapper;
using IntellaLend.Model;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.UtilsBlock;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EncompassExportConsole
{
    public class EncompassExport
    {
        public static void RestOfProgram()
        {
            string server = "https://TEBE11153533.ea.elliemae.net$TEBE11153533";
            string loginName = "skharidi";
            string password = "Mts@1234";

            try
            {
                // Start the session
                Session s = new Session();

                s.Start(server, loginName, password);

                Console.WriteLine(s.ID);



            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            Console.Write("Success!!!");
            Console.ReadKey();
        }
        static void Main(string[] args)
        {
            //new EllieMae.Encompass.Runtime.RuntimeServices().Initialize();
            RestOfProgram();
        }
    }
}

