using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.Query;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncompassFlagUpdator
{
    class Program
    {
        private static Session session;
        static void Main(string[] args)
        {
            new EllieMae.Encompass.Runtime.RuntimeServices().Initialize();
            Connect();
        }

        private static void Connect()
        {
            string str1 = ConfigurationManager.AppSettings["server"].ToString();
            string str2 = ConfigurationManager.AppSettings["loginName"].ToString();
            string str3 = ConfigurationManager.AppSettings["password"].ToString();
            try
            {
                if (session == null)
                {
                    if (!string.IsNullOrEmpty(str1) && !string.IsNullOrEmpty(str2) && !string.IsNullOrEmpty(str3))
                    {
                        Console.WriteLine("Please wait. Connecting to Encompass Server..");
                        Session s = new Session();
                        s.Start(str1, str2, str3);
                        session = s;
                        Console.WriteLine("Thank you for your patience. Server Connected Successfully..");
                        UpdateFlag();
                    }
                    else
                    {
                        Console.WriteLine("Connection Parameters Error. Please enter a key exit. Thank You :) " );
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("Connection Terminated. Please enter a key exit. Thank You :)");
                Console.ReadKey();
            }
        }

        private static void UpdateFlag()
        {           
            try
            {
                Console.WriteLine("Enter Loan Number to be Updated : ");
                string str4 = Console.ReadLine();
                StringFieldCriterion stringFieldCriterion = new StringFieldCriterion();
                stringFieldCriterion.FieldName = "Loan.LoanNumber";
                stringFieldCriterion.Value = str4;
                stringFieldCriterion.MatchType = StringFieldMatchType.Contains;

                Console.WriteLine("Fetching Loan Details. Please Wait.. :) ");
                LoanIdentityList loanIdentityList = session.Loans.Query(stringFieldCriterion);

                if (loanIdentityList.Count > 0)
                {
                    for (int i = 0; i < loanIdentityList.Count; i++)
                    {
                        LoanIdentity item = loanIdentityList[i];

                        Loan loan = session.Loans.Open(item.Guid);
                        Console.WriteLine("Loan Found. Please wait while updating.. :) ");
                        loan.Lock();

                        LoanField _field = loan.Fields["CX.DOWNLOADED"];
                        _field.Value = "";

                        loan.Commit();
                        Console.WriteLine("Loan Updated Successfully :) ");

                        if(i < loanIdentityList.Count)
                            Console.WriteLine("One more loan found on the same Loan Number ");
                    }

                    Console.WriteLine("Do you want to update one more loan ?");
                    Console.WriteLine("Enter Yes/No or Y/N :");
                    string answer = Console.ReadLine();

                    if (answer.ToUpper().Equals("YES") || answer.ToUpper().Equals("Y"))
                        UpdateFlag();

                }
                else
                    Console.WriteLine("Loan Not Found :( ");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                session.End();
            }
            session.End();
            Console.WriteLine("Connection Terminated. Please enter a key exit. Thank You :)");
            Console.ReadKey();
        }

    }
}
