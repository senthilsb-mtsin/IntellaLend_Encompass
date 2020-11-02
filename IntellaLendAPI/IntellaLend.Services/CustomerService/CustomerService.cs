using IntellaLend.EntityDataHandler;
using IntellaLend.Model;
using System;
using System.Collections.Generic;

namespace IntellaLend.CommonServices
{
    public class CustomerService
    {
        private static string TableSchema;

        #region Constructor

        public CustomerService() { }
        public CustomerService(string tableSchema)
        {
            TableSchema = tableSchema;
        }
        #endregion

        #region Public Methods        

        public List<CustomerMaster> GetCustomerList(bool Active)
        {
            return new CustomerDataAccess(TableSchema).GetCustomerList(Active);
        }

        public object AddCustomer(CustomerMaster customerMaster)
        {
            customerMaster.CreatedOn = DateTime.Now;
            customerMaster.ModifiedOn = DateTime.Now;
            return new CustomerDataAccess(TableSchema).AddCustomer(customerMaster);
        }

        public object EditCustomer(CustomerMaster customerMaster)
        {
            return new CustomerDataAccess(TableSchema).EditCustomer(customerMaster);
        }

        public bool RetentionPurge(long[] loanIDs, long userID, string username)
        {            
            CustomerDataAccess customerDataAccess = new CustomerDataAccess(TableSchema);
            foreach (var loanID in loanIDs)
            {
                new LoanDataAccess(TableSchema).DeleteLoanPDF(loanID);

                customerDataAccess.DeleteLoanPDF(loanID);

                customerDataAccess.DeleteLoanImages(loanID);

                customerDataAccess.RetentionPurgeStatusChange(loanID, userID, username);
            }

            return false;
        }

        #endregion
    }
}
