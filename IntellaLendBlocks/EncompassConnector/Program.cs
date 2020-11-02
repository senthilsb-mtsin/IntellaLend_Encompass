using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncompassConnector
{
    class Program
    {
        private static Session Session = null;

        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 5)
                {
                    new EllieMae.Encompass.Runtime.RuntimeServices().Initialize();
                    Connect(args[0], args[1], args[2]);
                    GetFieldValueByLoanGUID(args[3], args[4]);
                }
                else
                    Console.WriteLine("1");   // 1  
            }
            catch (Exception ex)
            {
                Console.WriteLine($"-1| {ex.Message}");
            }

        }

        private static void GetFieldValueByLoanGUID(string loanGUID, string fieldID)
        {
            Loan _loan = Session.Loans.Open("{" + loanGUID + "}");

            if (_loan != null)
            {
                if (_loan.Fields[fieldID].Value != null)
                {
                    Console.WriteLine(_loan.Fields[fieldID].Value.ToString());
                }
                else
                    Console.WriteLine("3"); // 3
            }
            else
                Console.WriteLine("2");  // 2
        }

        private static void Connect(string _server, string _userName, string _password)
        {
            if (Session == null)
            {
                if (!string.IsNullOrEmpty(_server) && !string.IsNullOrEmpty(_userName) && !string.IsNullOrEmpty(_password))
                {
                    Session s = new Session();
                    s.Start(_server, _userName, _password);
                    Session = s;
                }
                else
                {
                    Console.WriteLine("4"); //4
                }
            }
        }
    }
}
