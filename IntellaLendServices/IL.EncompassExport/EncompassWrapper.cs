
using System;
using System.Collections.Generic;
using System.Linq;

namespace IL.EncompassExport
{
    public  class EncompassConnector
    {
        #region Private Variables
        private EncompassWrapperAPI _api;
        #endregion

        #region Public Variables



        #endregion

        #region Constructor

        public EncompassConnector(string EncompassSDKAPIURL, string TenantSchema)
        {
            _api = new EncompassWrapperAPI(EncompassSDKAPIURL, TenantSchema);
        }
        
        #endregion

        #region Public Methods

        /// <summary>
        /// This is Public helper method
        /// </summary>
        /// <param name="loan"> Loan to be Updated</param>
        /// <param name="fields">Fields to be Updated. Key as FieldID, Value as value</param>
        /// <returns>DbCommand</returns>
        public  void UpdateLoan(Loan loan, Dictionary<string, object> fields, bool forceUnlock = false)
        {
            try
            {
                try
                {
                    loan.Lock();
                }
                catch (LockException ex)
                {
                    if (forceUnlock)
                    {
                        loan.ForceUnlock();
                        loan.Lock();
                    }
                    else
                    {
                        throw new EncompassWrapperException($"Loan Lock by User : {ex.CurrentLock.LockedBy}", ex);
                    }
                }

                foreach (string fieldID in fields.Keys)
                {
                    LoanField _field = loan.Fields[fieldID];
                    _field.Value = fields[fieldID];
                }
                loan.Commit();
                loan.Unlock();
            }
            catch (Exception ex)
            {
                throw new EncompassWrapperException("Update Failed", ex);
            }
        }

        /// <summary>
        /// This is Public helper method
        /// </summary>
        /// <param name="query">Condition to fetch List of Loans</param>
        /// <returns>List of Loans</returns>
        public  List<Loan> GetLoans(QueryCriterion query,List<string> _enExceptionLoans)
        {
            try
            {
                List<string> _lsLoans = new List<Loan>();

                List<string> _loans = _api.GetPipeLineLoans(query);

                foreach (string _loanItem in _loans)
                {
                    string loanGUID = _loanItem.Guid.Substring(1, _loanItem.Guid.Length - 2);

                    string upperLoanGUID = loanGUID.ToUpper();

                    if (!(_enExceptionLoans.Any(e => e == upperLoanGUID)))
                    {
                        Loan _loan = _api.GetLoanByGUID(loanGUID);
                        _lsLoans.Add(_loan);
                    }
                }

                return _lsLoans;
            }
            catch (Exception ex)
            {
                throw new EncompassWrapperException("Get Loans Failed", ex);
            }
        }

        /// <summary>
        /// This is Public helper method
        /// </summary>
        /// <param name="query">Condition to fetch List of Loans</param>
        /// <returns>List of Loans</returns>
        public  List<Loan> GetLoans(QueryCriterion query)
        {
            try
            {
                List<Loan> _lsLoans = new List<Loan>();

                LoanIdentityList _loans = Session.Loans.Query(query);

                foreach (LoanIdentity _loanItem in _loans)
                {
                    string loanGUID = _loanItem.Guid.Substring(1, _loanItem.Guid.Length - 2);
                    Loan _loan = GetLoanByGUID(loanGUID);
                    _lsLoans.Add(_loan);
                }

                return _lsLoans;
            }
            catch (Exception ex)
            {
                throw new EncompassWrapperException("Get Loans Failed", ex);
            }
        }

        /// <summary>
        /// This is Public helper method
        /// </summary>
        /// <param name="query">Condition to fetch a Loan from Encompass</param>
        /// <returns>A Loan</returns>
        public  Loan GetLoan(QueryCriterion query)
        {
            try
            {
                Loan _loan = null;

                LoanIdentityList _loans = Session.Loans.Query(query);

                foreach (LoanIdentity _loanItem in _loans)
                {
                    _loan = GetLoanByGUID(_loanItem.Guid);
                    break;
                }

                return _loan;
            }
            catch (Exception ex)
            {
                throw new EncompassWrapperException("Get Loans Failed", ex);
            }
        }

        /// <summary>
        /// This is Public helper method
        /// </summary>
        /// <param name="loanGUID"> Loan GUID</param>
        /// <returns>Loan</returns>
        public  Loan GetLoanByGUID(string loanGUID)
        {
            try
            {
                return Session.Loans.Open("{" + loanGUID + "}");
            }
            catch (Exception ex)
            {
                throw new EncompassWrapperException($"Get Loan Failed using GUID : {loanGUID}", ex);
            }
        }

        /// <summary>
        /// This is Public helper method
        /// </summary>
        /// <param name="loanGUID">Loan GUID</param>
        /// <param name="loanFieldID">Loan Field ID</param>
        /// <returns>Loan</returns>
        public  string GetFieldValueByLoanGUID(string loanGUID, string fieldID)
        {
            try
            {
                Loan _loan = Session.Loans.Open("{" + loanGUID + "}");

                if (_loan != null)
                {
                    if (_loan.Fields[fieldID].Value != null)
                        return _loan.Fields[fieldID].Value.ToString();
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new EncompassWrapperException($"Get Loan Failed using GUID : {loanGUID}, FieldID : {fieldID}", ex);
            }
        }

        /// <summary>
        /// This is Public helper method
        /// </summary>
        /// <param name="loan">Loan</param>
        /// <param name="loanFieldID">Loan Field ID</param>
        /// <returns>Loan</returns>
        public  string GetFieldValueByLoan(Loan _loan, string fieldID)
        {
            try
            {
                if (_loan != null)
                {
                    if (_loan.Fields[fieldID].Value != null)
                        return _loan.Fields[fieldID].Value.ToString();
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new EncompassWrapperException($"Get Loan Failed using GUID : {_loan.Guid} , FieldID : {fieldID}", ex);
            }
        }


        /// <summary>
        /// This is Public helper method
        /// </summary>
        /// <param name="FieldID">Field ID</param>
        /// <param name="FieldValue">Field Value</param>
        /// <param name="FieldMatchType">0 : Exact Match</param>
        /// <returns>Loan</returns>
        public  Dictionary<string, string> FieldLoanTypeLookup(string FieldID, string FieldValue, Dictionary<string, string> FetchFields, int FieldMatchType = 0)
        {
            StringFieldCriterion _query = new StringFieldCriterion();
            _query.FieldName = FieldID;
            _query.Value = FieldValue;
            _query.MatchType = (StringFieldMatchType)FieldMatchType;

            Loan _loan = GetLoan(_query);

            if (_loan != null)
            {
                foreach (string FieldKey in FetchFields.Keys)
                    FetchFields[FieldKey] = _loan.Fields[FieldKey].Value.ToString();

                return FetchFields;
            }

            return FetchFields;
        }

        /// <summary>
        /// This is Public helper method
        /// </summary>
        /// <param name="FieldID">Field ID</param>
        /// <param name="FieldValue">Field Value</param>
        /// <param name="FieldMatchType">0 : Exact Match</param>
        /// <returns>Loan</returns>
        public  Dictionary<string, string> FieldLoanTypeLookup(List<Dictionary<string, string>> EFields, Dictionary<string, string> FetchFields, int FieldMatchType = 0)
        {
            if (EFields.Count == 2)
            {
                StringFieldCriterion _query = new StringFieldCriterion();
                //EFields[0] : BorrowerName
                _query.FieldName = EFields[0]["FieldID"];
                _query.Value = EFields[0]["FieldValue"];
                _query.MatchType = (StringFieldMatchType)FieldMatchType;

                List<Loan> _loans = GetLoans(_query);
                Loan loan = null;
                foreach (Loan _loan in _loans)
                {
                    if ((string)(_loan.Fields[EFields[1]["FieldID"]].Value) == EFields[1]["FieldValue"])
                    {
                        loan = _loan;

                        foreach (string FieldID in FetchFields.Keys)
                            FetchFields[FieldID] = _loan.Fields[FieldID].Value.ToString();

                        return FetchFields;
                    }
                }
            }

            return FetchFields;
        }

        /// <summary>
        /// This is Public helper method
        /// </summary>
        /// <param name="FieldID">Field ID</param>
        /// <param name="FieldValue">Field Value</param>
        /// <param name="FieldMatchType">0 : Exact Match</param>
        /// <returns>Loan</returns>
        public  List<string> GetLoansToBeImported(List<Dictionary<string, string>> EFields, List<string> _enExceptionLoans, int FieldMatchType = 0)
        {
            List<StringFieldCriterion> _lsCriterion = new List<StringFieldCriterion>();

            List<string> _loans = new List<string>();

            StringFieldCriterion _sQuery = null;

            foreach (Dictionary<string, string> item in EFields)
            {
                _sQuery = new StringFieldCriterion();
                _sQuery.FieldName = item.Keys.FirstOrDefault();
                _sQuery.Value = item.Values.FirstOrDefault();
                _sQuery.MatchType = (StringFieldMatchType)FieldMatchType;

                _lsCriterion.Add(_sQuery);
            }

            QueryCriterion _query = null;

            if (_lsCriterion.Count == 2)
                _query = _lsCriterion[0].And(_lsCriterion[1]);
            else if (_lsCriterion.Count == 1)
                _query = _lsCriterion[0];

            if (_query != null)
                _loans = GetLoans(_query, _enExceptionLoans);

            return _loans;
        }


        #endregion

        #region Private Methods

        private  void Connect()
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
                    throw new EncompassWrapperLoginFailedException("Login Failed. Enter Proper Credentials");
                }
            }
        }

        #endregion

    }
    public class EncompassWrapperException : Exception
    {
        public EncompassWrapperException() { }

        public EncompassWrapperException(string message) : base(message)
        { }

        public EncompassWrapperException(string message, Exception innerException) : base(message, innerException)
        { }
    }

    public class EncompassWrapperLoginFailedException : Exception
    {
        public EncompassWrapperLoginFailedException() { }

        public EncompassWrapperLoginFailedException(string message) : base(message)
        { }

        public EncompassWrapperLoginFailedException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
