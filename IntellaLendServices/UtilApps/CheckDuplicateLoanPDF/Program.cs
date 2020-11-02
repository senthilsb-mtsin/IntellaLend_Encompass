using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CheckDuplicateLoanPDF
{
    class Program
    {
        static void Main(string[] args)
        {
            options();
        }

        public static void options()
        {
            Console.WriteLine("Press 1 to check all loans on the path");
            Console.WriteLine("Press 2 to check all loans on the path");

            string optionValue = Console.ReadLine().Trim();

            if (optionValue.Equals("1"))
                GetAllLoans();
            else if (optionValue.Equals("2"))
                GetSpecificLoans();
            else
            {
                Console.WriteLine("Enter Correct Value");
                options();
            }
        }

        public static void GetAllLoans()
        {
            Console.WriteLine("Enter Cloud Storage Path : ");

            string parentPath = Console.ReadLine().Trim();

            Dictionary<string, Dictionary<string, string>> _loans = new Dictionary<string, Dictionary<string, string>>();

            string[] bucketPaths = Directory.GetDirectories(parentPath);

            Console.WriteLine("In Process... Please Wait");

            foreach (string bucketPath in bucketPaths)
            {
                string[] loanPaths = Directory.GetDirectories(bucketPath);

                foreach (string loanPath in loanPaths)
                {
                    string[] loanFiles = Directory.GetFiles(loanPath);

                    Console.WriteLine($"Processing : {Path.GetFileName(loanPath)}");

                    foreach (string loanFile in loanFiles)
                    {
                        FileInfo file = new FileInfo(loanFile);

                        Console.Write($"\r {file.Name}");

                        Int64 size = file.Length / (1024 * 1024);

                        if (size > 10)
                        {
                            string loanGUID = Path.GetFileName(loanPath);
                            string loanPDFGUID = Path.GetFileName(loanFile);

                            if (_loans.ContainsKey(loanGUID))
                                _loans[loanGUID].Add(loanPDFGUID, size.ToString());
                            else
                                _loans.Add(loanGUID, new Dictionary<string, string>() { { loanPDFGUID, size.ToString() } });
                        }
                    }
                }
            }

            Console.WriteLine("Loan Files Fetched");

            var lsLoans = _loans.Where(l => l.Value.Count > 1).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var loan in lsLoans)
            {
                foreach (var loanPDF in loan.Value)
                {
                    sb.AppendLine($"{loan.Key},{loanPDF.Key},{loanPDF.Value}");
                }
            }

            if (File.Exists("AllLoans.txt"))
                File.Delete("AllLoans.txt");

            File.WriteAllText("AllLoans.txt", sb.ToString());

            Console.WriteLine("Output Created. Please enter any key to Exit");

            Console.ReadKey();
        }

        public static void GetSpecificLoans()
        {
            Console.WriteLine("Enter Cloud Storage Path : ");

            string parentPath = Console.ReadLine().Trim();

            Dictionary<string, Dictionary<string, string>> _loans = new Dictionary<string, Dictionary<string, string>>();

            if (File.Exists("prodLoans.txt"))
            {

                string[] bucketPaths = File.ReadAllLines("prodLoans.txt");

                Console.WriteLine("In Process... Please Wait");

                foreach (string loan in bucketPaths)
                {
                    string[] _loanDetails = loan.Split(',');

                    string loanPath = Path.Combine(parentPath, _loanDetails[0], _loanDetails[1]);

                    string[] loanFiles = Directory.GetFiles(loanPath);

                    Console.WriteLine($"Processing : {_loanDetails[2]}");

                    foreach (string loanFile in loanFiles)
                    {
                        FileInfo file = new FileInfo(loanFile);

                        Console.Write($"\r {file.Name}");

                        Int64 size = file.Length / (1024 * 1024);

                        if (size > 10)
                        {
                            string loanGUID = _loanDetails[2];
                            string loanPDFGUID = Path.GetFileName(loanFile);

                            if (_loans.ContainsKey(loanGUID))
                                _loans[loanGUID].Add(loanPDFGUID, size.ToString());
                            else
                                _loans.Add(loanGUID, new Dictionary<string, string>() { { loanPDFGUID, size.ToString() } });
                        }
                    }
                }

                Console.WriteLine("Loan Files Fetched");

                var lsLoans = _loans.Where(l => l.Value.Count > 1).ToList();

                StringBuilder sb = new StringBuilder();

                foreach (var loan in lsLoans)
                {
                    foreach (var loanPDF in loan.Value)
                    {
                        sb.AppendLine($"{loan.Key},{loanPDF.Key},{loanPDF.Value}");
                    }
                }

                if (File.Exists("SpecificLoans.txt"))
                    File.Delete("SpecificLoans.txt");

                File.WriteAllText("SpecificLoans.txt", sb.ToString());

                Console.WriteLine("Output Created. Please enter any key to Exit");
            }
            else {
                Console.WriteLine("File not found : prodLoans.txt");
                Console.WriteLine("Please enter any key to Exit");
            }

            Console.ReadKey();
        }
    }
}
