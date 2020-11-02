using EncompassConsoleConnector;
using NCalc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
//using NCalc.Domain;
using System.Text.RegularExpressions;

namespace MTSRuleEngine
{
    #region Rule Exception
    public class RuleException : Exception
    {
        // Summary:
        //     Initializes a new instance of the System.Exception class.
        public RuleException()
        {

        }
        //
        // Summary:
        //     Initializes a new instance of the System.Exception class with a specified
        //     error message.
        //
        // Parameters:
        //   message:
        //     The message that describes the error.
        public RuleException(string message)
        {
        }
        //
        // Summary:
        //     Initializes a new instance of the System.Exception class with serialized
        //     data.
        //
        // Parameters:
        //   info:
        //     The System.Runtime.Serialization.SerializationInfo that holds the serialized
        //     object data about the exception being thrown.
        //
        //   context:
        //     The System.Runtime.Serialization.StreamingContext that contains contextual
        //     information about the source or destination.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     The info parameter is null.
        //
        //   System.Runtime.Serialization.SerializationException:
        //     The class name is null or System.Exception.HResult is zero (0).
        [SecuritySafeCritical]
        protected RuleException(SerializationInfo info, StreamingContext context)
        {
        }
        //
        // Summary:
        //     Initializes a new instance of the System.Exception class with a specified
        //     error message and a reference to the inner exception that is the cause of
        //     this exception.
        //
        // Parameters:
        //   message:
        //     The error message that explains the reason for the exception.
        //
        //   innerException:
        //     The exception that is the cause of the current exception, or a null reference
        //     (Nothing in Visual Basic) if no inner exception is specified.
        public RuleException(string message, Exception innerException)
        {
        }

    }

    #endregion Rule Exception

    #region comment begin
    //public enum MTSRuleType
    //{
    //    Expression,
    //    Assign
    //}

    //public class MTSRule
    //{
    //    //public RuleType ruleType;
    //    public string Name;
    //    public string Expression;
    //    public string Value;
    //    public object Result;
    //    private MTSRule()
    //    {
    //    }
    //    public MTSRule(string name, string expression)
    //    {
    //        this.Name = name;
    //        this.Expression = expression;
    //    }
    //}


    //private void ExtractIdentifiers(LogicalExpression expression, List<string> identifiers)
    //{

    //    /*USAGE******

    //     *  List<string> identifiers = new List<string>();
    //        bool bHasErrors = e.HasErrors();
    //        if (!bHasErrors)
    //        {
    //            ExtractIdentifiers(e.ParsedExpression, identifiers);
    //        }
    //     */


    //    if (expression is UnaryExpression)
    //    {
    //        UnaryExpression ue = expression as UnaryExpression;

    //        ExtractIdentifiers(ue.Expression, identifiers);
    //    }
    //    else if (expression is BinaryExpression)
    //    {
    //        BinaryExpression be = expression as BinaryExpression;

    //        ExtractIdentifiers(be.LeftExpression, identifiers);
    //        ExtractIdentifiers(be.RightExpression, identifiers);
    //    }
    //    else if (expression is TernaryExpression)
    //    {
    //        TernaryExpression te = expression as TernaryExpression;

    //        ExtractIdentifiers(te.LeftExpression, identifiers);
    //        ExtractIdentifiers(te.MiddleExpression, identifiers);
    //        ExtractIdentifiers(te.RightExpression, identifiers);
    //    }
    //    else if (expression is FunctionExpression)
    //    {
    //        FunctionExpression fn = expression as FunctionExpression;

    //        LogicalExpression[] expressions = fn.Expressions;
    //        if (expressions != null && expressions.Length > 0)
    //        {
    //            for (int i = 0; i < expressions.Length; i++)
    //            {
    //                ExtractIdentifiers(expressions[i], identifiers);
    //            }
    //        }
    //    }
    //    else if (expression is IdentifierExpression)
    //    {
    //        IdentifierExpression identifier = expression as IdentifierExpression;

    //        if (!identifiers.Contains(identifier.Name))
    //        {
    //            identifiers.Add(identifier.Name);
    //        }
    //    }
    //}
    #endregion comment end

    #region PrameterHandler
    public delegate void MTSEvaluateParameterHandler(string name, MTSParameterArgs args);

    public class MTSParameterArgs : NCalc.ParameterArgs
    {

    }
    #endregion PrameterHandler

    #region class MTSRuleResult
    public class MTSRuleResult
    {
        public object Result;
        public string Expressions;
        public string Message;
        public string ErrorMessage;
        public string Category;
        public override string ToString()
        {
            return Result.ToString();
        }
    }

    public class MTSResult
    {
        public object Result;
        public string Message;
        public string EvaluatedExpression;
    }

    public class MTSRuleResults : Dictionary<string, MTSRuleResult>
    {
        public bool AnyRuleFailed()
        {
            bool result = true;
            foreach (var item in this)
            {
                if (Boolean.FalseString == item.Value.ToString())
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
    }
    #endregion class MTSRuleResult


    public class MTSRules : Dictionary<string, string>
    {
        MTSRuleResults ruleResult = new MTSRuleResults();
        public MTSEvaluateParameterHandler EvaluateParameter;

        #region Utils
        private Expression getExpression(string expr)
        {
            expr = expr.Replace("'#", "#").Replace("#'", "#");
            Expression e = new Expression(expr, EvaluateOptions.IgnoreCase);
            //delegate for external 
            //e.EvaluateParameter
            e.EvaluateFunction += evalFunction;
            return e;
        }

        public static string getParsedExpression(Expression e)
        {
            string matchPattern = @"\[(.*?)\]";
            string reslt = e.ParsedExpression.ToString();
            MatchCollection parameters = Regex.Matches(reslt, matchPattern);

            if (!reslt.ToLower().Contains("checkall"))
            {
                foreach (Match m in parameters)
                {
                    string key = m.Groups[1].ToString();
                    if (e.Parameters.ContainsKey(key))
                    {
                        reslt = reslt.Replace(string.Concat("[", m.Groups[1].ToString(), "]"), e.Parameters[key].ToString());
                    }
                }
            }

            return reslt;
        }

        public static string tryGetParsedExpression(Expression e)
        {
            string matchPattern = @"\[(.*?)\]";
            string reslt = e.ParsedExpression.ToString();
            MatchCollection parameters = Regex.Matches(reslt, matchPattern);
            List<string> missingDocs = new List<string>();
            if (!reslt.ToLower().Contains("checkall"))
            {
                foreach (Match m in parameters)
                {
                    string key = m.Groups[1].ToString();
                    if (e.Parameters.ContainsKey(key))
                    {
                        reslt = reslt.Replace(string.Concat("[", m.Groups[1].ToString(), "]"), e.Parameters[key].ToString());
                    }
                    else
                    {
                        missingDocs.Add(key.Split('.').Count() > 0 ? key.Split('.')[0] : "Document Type");
                    }
                }
            }

            return string.Join(",", missingDocs) + " document not found";
        }


        #endregion Utils

        #region custom function Handler
        private void evalFunction(string name, FunctionArgs args)
        {
            double d = 0;
            switch (name.ToUpper())
            {
                case "DATATABLE":
                    if (args.Parameters.Length > 1)
                        throw new ArgumentException("DataTable() takes only 1 arguments");

                    string fPattern = @"\[(.*?)\]";
                    string reslt = args.Parameters[0].ParsedExpression.ToString();
                    MatchCollection fParameters = Regex.Matches(reslt, fPattern);
                    string docType = string.Empty, tableName = string.Empty, columnName = string.Empty, key = string.Empty;
                    DataTable _docTable = null;
                    foreach (Match m in fParameters)
                    {
                        string[] fieldTypes = m.Groups[1].ToString().Split('.');
                        if (fieldTypes.Count() == 4)
                        {
                            docType = fieldTypes[0];
                            tableName = fieldTypes[1];
                            columnName = fieldTypes[2];
                            key = fieldTypes[3];
                            string docTablesStr = args.Parameters[0].Parameters[$"{docType}.{tableName}.{columnName}"].ToString();

                            if (string.IsNullOrEmpty(docTablesStr.Trim()))
                                throw new DataTableException($"{tableName} not found");

                            _docTable = JsonConvert.DeserializeObject<DataTable>(docTablesStr);
                            object cellVal;
                            if (key.ToUpper().Equals("#SUM#"))
                                cellVal = SumTableColumn(_docTable, columnName);
                            else
                                cellVal = GetCellValue(_docTable, columnName, key);

                            args.Parameters[0].Parameters[$"{docType}.{tableName}.{columnName}.{key}"] = cellVal;
                        }
                    }

                    MTSResult mtsDatatable = args.Parameters[0].Evaluate();
                    args.Result = mtsDatatable.Result;
                    args.Message = mtsDatatable.Message;
                    break;
                case "ENCOMPASS":
                    if (args.Parameters.Length > 1)
                        throw new ArgumentException("Encompass() takes only 1 arguments ");
                    // string enFieldID = string.Empty;
                    // string enFieldVal = string.Empty;

                    string expression = args.Parameters[0].ParsedExpression.ToString();

                    object tenantSchema = "";

                    args.Parameters[0].Parameters.TryGetValue("TENANTSCHEMA", out tenantSchema);

                    if (args.Parameters[0].Parameters.ContainsKey("EncompassLoanGUID"))
                    {

                        string matchPattern = @"\[(.*?)\]";
                        MatchCollection parameters = Regex.Matches(expression, matchPattern);
                        foreach (Match m in parameters)
                        {
                            var regexObj = new Regex(@"#.+?#");
                            //var regexObj = new Regex(@"\[(.*?)\]");
                            MatchCollection allMatchResults = regexObj.Matches(m.Groups[1].ToString());
                            if (allMatchResults.Count > 0)
                            {
                                for (int i = 0; i < allMatchResults.Count; i++)
                                {
                                    string enFieldID = allMatchResults[i].Value.Split('-')[0].Trim().Replace("#", "");

                                    if (enFieldID.Equals(string.Empty))
                                        throw new EncompassException("Encompass Field Not Found");

                                    if (args.Parameters[0].Parameters["EncompassLoanGUID"].ToString().Trim().Equals(string.Empty))
                                        throw new EncompassException("Encompass Loan Identity Not Found");

                                    try
                                    {
                                        object enFieldVal = DataType.Find(EncompassConnectorApp.QueryEncompass(args.Parameters[0].Parameters["EncompassLoanGUID"].ToString(), enFieldID, tenantSchema.ToString()));

                                        args.Parameters[0].Parameters[m.Groups[1].ToString()] = enFieldVal;
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new EncompassException(ex.Message);
                                    }
                                }
                            }
                        }

                        MTSResult mtsRS = args.Parameters[0].Evaluate();
                        args.Result = mtsRS.Result;
                        args.Message = mtsRS.Message;
                    }
                    else
                    {
                        throw new EncompassException("Not an Encompass Loan Package");
                    }
                    break;
                case "ISEMPTY":
                    if (args.Parameters.Length > 1)
                        throw new ArgumentException("isEmpty() takes at only 1 arguments");

                    //args.Parameters[0].ParsedExpression = FormatExtrargs.Parameters[0].ParsedExpression
                    List<string> _objEmpty = new List<string>();
                    foreach (Expression item in args.Parameters)
                        _objEmpty.Add(GetValueFromDictionary(item));

                    if (_objEmpty.Count > 0)
                        args.Result = string.IsNullOrEmpty(_objEmpty[0]);
                    else
                        throw new ArgumentException("values not found");
                    //args.Result = string.IsNullOrEmpty(args.Parameters[0].Parameters.Values.FirstOrDefault().ToString());
                    break;
                case "EMPTY":
                    if (args.Parameters.Length > 1)
                        throw new ArgumentException("empty() takes at only 1 arguments");

                    //args.Parameters[0].ParsedExpression = FormatExtrargs.Parameters[0].ParsedExpression
                    List<string> _objIsEmpty = new List<string>();
                    foreach (Expression item in args.Parameters)
                        _objIsEmpty.Add(GetValueFromDictionary(item));

                    if (_objIsEmpty.Count > 0)
                        args.Result = string.IsNullOrEmpty(_objIsEmpty[0]);
                    else
                        throw new ArgumentException("values not found");
                    //args.Result = string.IsNullOrEmpty(args.Parameters[0].Parameters.Values.FirstOrDefault().ToString());
                    break;
                //case "ISEXIST":
                //    if (args.Parameters.Length > 1)
                //        throw new ArgumentException("isExist() takes at only 1 arguments");

                //    List<string> _objExist = new List<string>();

                //    foreach (Expression item in args.Parameters)
                //        _objExist.Add(TryGetValueFromDictionary(name.ToLower(), item));

                //    if (_objExist.Count > 0)
                //        args.Result = !string.IsNullOrEmpty(_objExist[0]);
                //    else
                //        throw new ArgumentException("values not found");
                //    break;

                case "ISEXIST":
                    if (args.Parameters.Length == 0)
                        throw new ArgumentException("isExist() takes at least 1 arguments");

                    Dictionary<string, bool> _objExist = new Dictionary<string, bool>();
                    bool IsNotExist = false;

                    _objExist = TryGetValueFromDictionary(args);

                    IsNotExist = _objExist.Any(pair => pair.Value == false);
                    if (IsNotExist)
                        throw new DocFoundException(string.Join(",", _objExist.Where(pair => pair.Value == false).Select(pair => pair.Key).ToArray()) + " Not Found");
                    else
                        args.Result = true;

                    break;
                case "ISEXISTANY":
                    if (args.Parameters.Length == 0)
                        throw new ArgumentException("isExistAny() takes at least 1 arguments");

                    Dictionary<string, bool> _objAnyExist = new Dictionary<string, bool>();
                    bool IsAnyExist = false;

                    _objAnyExist = TryGetValueFromDictionary(args);

                    IsAnyExist = _objAnyExist.Any(pair => pair.Value == true);
                    if (IsAnyExist)
                        args.Result = true;

                    else
                        throw new DocFoundException(string.Join(",", _objAnyExist.Where(pair => pair.Value == false).Select(pair => pair.Key).ToArray()) + " Not Found");
                    break;

                case "ISNOTEMPTY":
                    if (args.Parameters.Length > 1)
                        throw new ArgumentException("isNotEmpty() takes at only 1 arguments");

                    List<string> _objNotEmpty = new List<string>();
                    foreach (Expression item in args.Parameters)
                        _objNotEmpty.Add(TryGetValueFromDictionary(item));

                    if (_objNotEmpty.Count > 0)
                        args.Result = !string.IsNullOrEmpty(_objNotEmpty[0]);
                    else
                        throw new ArgumentException("values not found");

                    break;
                case "COMPARE":
                    if (args.Parameters.Length < 2)
                        throw new ArgumentException("compare() takes at least 2 arguments");

                    List<string> _objCompare = new List<string>();
                    List<string> _objDocCompare = new List<string>();
                    try
                    {
                        foreach (Expression item in args.Parameters)
                            _objCompare.Add(TryCompareGetValueFromDictionary(item));
                    }
                    catch (ParameterNotFoundException ex)
                    {
                        _objDocCompare.Add(ex.Message);
                    }

                    if (_objDocCompare.Count > 0)
                        throw new ComparDocNotFoundException();

                    if (_objCompare.Count > 0)
                        args.Result = !_objCompare.Any(l => l != _objCompare[0]);
                    else
                        throw new ArgumentException("values not found");

                    break;
                //case "STRINGCOMPARE":
                //    if (args.Parameters.Length < 2)
                //        throw new ArgumentException("Compare() takes at least 2 arguments");
                //    args.Result = string.Equals(args.Parameters[0].Evaluate().Result.ToString(), args.Parameters[1].Evaluate().Result.ToString(), StringComparison.OrdinalIgnoreCase);
                //    break;

                case "CHECKALL":
                    if (args.Parameters.Length > 1)
                        throw new ArgumentException("checkall() takes at only 1 arguments");

                    Dictionary<string, string> _objCheckAll = GetListFromDictionary(args.Parameters[0]);

                    if (_objCheckAll.Count > 0)
                    {
                        var baseValue = _objCheckAll.FirstOrDefault();
                        var _result = _objCheckAll.Where(x => x.Value != baseValue.Value).Select(y => y.Key).ToList();
                        if (_result.Count > 1)
                        {
                            args.Message = "Document types";
                            foreach (var item in _result)
                            {
                                args.Message += $" '{item}' ,";
                            }
                            args.Message = args.Message.Substring(0, args.Message.LastIndexOf(','));

                            args.Message += "are not matching against Document type '" + baseValue.Key + "'";
                        }
                        else if (_result.Count == 1)
                        {
                            args.Message = "Document type";
                            foreach (var item in _result)
                            {
                                args.Message += $" '{item}' ";
                            }                            
                            args.Message += "is not matching against Document type '" + baseValue.Key + "'";                            
                        }

                        args.Result = _result.Count == 0;//!_objCheckAll.Any(l => l != _objCheckAll[0]);
                    }
                    else
                        throw new ArgumentException("values not found");
                    break;
                case "ISCONTAINS":
                    if (args.Parameters.Length < 2)
                        throw new ArgumentException("isContains() takes at least 2 arguments");
                    args.Result = args.Parameters[0].Evaluate().Result.ToString().Split(',').Any(args.Parameters[0].Evaluate().Result.ToString().Contains);
                    break;
                case "FORMAT":
                    if (args.Parameters.Length < 2)
                        throw new ArgumentException("format() takes at least 2 arguments");
                    args.Result = String.Format(args.Parameters[0].Evaluate().Result.ToString(), args.Parameters[1].Evaluate().Result.ToString());
                    break;
                case "TODAY":
                    try
                    {
                        if (args.Parameters.Length == 1)
                            d = Convert.ToDouble(args.Parameters[0].Evaluate().Result);
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException("today() arguments should be Integer or Double type");
                    }
                    //args.Result = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(d);
                    args.Result = DateTime.Now.Date.AddDays(d);

                    break;

                case "DATEDIF":
                    try
                    {
                        if (args.Parameters.Length == 2)
                        {
                            bool hasError = false;
                            string errorMsg = string.Empty;
                            DateTime stDate = new DateTime();
                            DateTime edDate = new DateTime();

                            string matchPattern = @"\[(.*?)\]";
                            string _parsedExp = args.Parameters[0].ParsedExpression.ToString();
                            MatchCollection parameters = Regex.Matches(_parsedExp, matchPattern);
                            string _docTypeOne = string.Empty;
                            foreach (Match m in parameters)
                            {
                                _docTypeOne = m.Groups[1].ToString();
                                break;
                            }

                            _parsedExp = args.Parameters[1].ParsedExpression.ToString();
                            parameters = Regex.Matches(_parsedExp, matchPattern);
                            string _docTypeTwo = string.Empty;
                            foreach (Match m in parameters)
                            {
                                _docTypeTwo = m.Groups[1].ToString();
                                break;
                            }


                            try
                            {
                                stDate = Convert.ToDateTime(args.Parameters[0].Evaluate().Result);
                            }
                            catch (Exception)
                            {
                                hasError = true;
                                errorMsg += "'" + _docTypeOne + "' format mismatch. ";
                            }

                            try
                            {
                                edDate = Convert.ToDateTime(args.Parameters[1].Evaluate().Result);
                            }
                            catch (Exception)
                            {
                                hasError = true;
                                errorMsg += "'" + _docTypeTwo + "' format mismatch.";
                            }


                            if (!hasError)
                            {
                                TimeSpan diff = edDate - stDate;
                                d = diff.TotalDays;
                            }
                            else
                            {
                                throw new ArgumentException(errorMsg);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException(ex.Message);
                    }
                    //args.Result = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(d);
                    args.Result = d;

                    break;


                default:
                    break;
            }
        }

        private decimal SumTableColumn(DataTable _docTable, string ColumnName)
        {
            List<decimal> _cellValues = new List<decimal>();

            int colIndex = _docTable.HeaderRow.HeaderColumns.IndexOf(_docTable.HeaderRow.HeaderColumns.Where(h => h.Name == ColumnName).FirstOrDefault());

            if (colIndex < 0)
                throw new DataTableException($"{ColumnName} column not found");

            foreach (Row _row in _docTable.Rows)
            {
                string cellVal = _row.RowColumns[colIndex].Value;
                cellVal = string.IsNullOrEmpty(cellVal) ? string.Empty : cellVal.Replace("%", "").Replace("$", "").Replace(",", "").Trim();
                decimal cellValDecimal = 0m;
                Decimal.TryParse(cellVal, out cellValDecimal);
                decimal roundVal = Math.Round(cellValDecimal);
                _cellValues.Add(roundVal);
            }
            return _cellValues.Sum();
        }

        private string TryCompareGetValueFromDictionary(Expression e)
        {
            //string _value = e.ParsedExpression.ToString();
            string matchPattern = @"\[(.*?)\]";
            string reslt = e.ParsedExpression.ToString();
            MatchCollection parameters = Regex.Matches(reslt, matchPattern);
            reslt = string.Empty;
            bool parameterFound = false;
            string docType = string.Empty;
            foreach (Match m in parameters)
            {
                string key = m.Groups[1].ToString();
                if (e.Parameters.ContainsKey(key))
                {
                    reslt = e.Parameters[key].ToString();
                    parameterFound = true;
                    break;
                }
                docType = key.Split('.').Count() > 0 ? key.Split('.')[0] : "Document Type";
            }

            if (!parameterFound)
                throw new ParameterNotFoundException(docType);

            return reslt;
        }


        private object GetCellValue(DataTable _docTable, string columnName, string rowKey)
        {
            rowKey = rowKey.Substring(1, rowKey.Length - 2);

            int colIndex = _docTable.HeaderRow.HeaderColumns.IndexOf(_docTable.HeaderRow.HeaderColumns.Where(h => h.Name == columnName).FirstOrDefault());

            if (colIndex < 0)
                throw new DataTableException($"{columnName} column not found");

            foreach (Row _row in _docTable.Rows)
            {
                if (_row.RowColumns.Any(rc => rc.Value.Contains(rowKey)))
                    return _row.RowColumns[colIndex].Value;
            }
            return string.Empty;
        }


        private Dictionary<string, object> FormatParams(Dictionary<string, object> _params)
        {
            Dictionary<string, object> _newParams = new Dictionary<string, object>();
            var _keys = _params.Keys;
            Regex _regex = new Regex(@"(?=\[).+?(?=\])");
            foreach (var item in _keys)
            {
                string _key = item;

                if (_regex.IsMatch(_key))
                    _key = _key.Substring(1, _key.Length - 1);

                _newParams.Add(_key, _params[item]);
            }

            //for (int i = 0; i < _params.Count; i++)
            //{
            //    _params.Keys
            //}

            return _newParams;
        }

        private Dictionary<string, string> GetListFromDictionary(Expression e)
        {
            Dictionary<string, string> _value = new Dictionary<string, string>();
            string matchPattern = @"\[(.*?)\]";
            string reslt = e.ParsedExpression.ToString();
            MatchCollection parameters = Regex.Matches(reslt, matchPattern);

            foreach (Match m in parameters)
            {
                string key = m.Groups[1].ToString();
                if (e.Parameters.ContainsKey(key))
                {
                    _value = (Dictionary<string, string>)e.Parameters[key];
                    break;
                }
            }

            return _value;
        }

        private string GetValueFromDictionary(Expression e)
        {
            //string _value = e.ParsedExpression.ToString();
            string matchPattern = @"\[(.*?)\]";
            string reslt = e.ParsedExpression.ToString();
            MatchCollection parameters = Regex.Matches(reslt, matchPattern);
            reslt = string.Empty;
            bool isParamFound = false;
            string paramKey = string.Empty;
            foreach (Match m in parameters)
            {
                string key = m.Groups[1].ToString();
                paramKey = key;
                if (e.Parameters.ContainsKey(key))
                {
                    isParamFound = true;
                    reslt = e.Parameters[key].ToString();
                    break;
                }
            }

            if (!isParamFound)
            {
                throw new ArgumentException($"DocumentType/Field '{paramKey}' not found");
            }

            return reslt;
        }
        private string TryGetValueFromDictionary(Expression e)
        {
            string _value = string.Empty;
            string matchPattern = @"\[(.*?)\]";
            string reslt = e.ParsedExpression.ToString();
            MatchCollection parameters = Regex.Matches(reslt, matchPattern);
            string docType = string.Empty;
            bool parameterFound = false;
            foreach (Match m in parameters)
            {
                docType = m.Groups[1].ToString();
                if (e.Parameters.ContainsKey(docType))
                {
                    _value = Convert.ToString(e.Parameters[docType]);
                    parameterFound = true;
                    break;
                }
            }

            if (!parameterFound)
                throw new DocFoundException(docType + " Not Found");

            return _value;
        }

        private Dictionary<string, bool> TryGetValueFromDictionary(FunctionArgs args)
        {

            Dictionary<string, bool> Rslt = new Dictionary<string, bool>();
            string _value = string.Empty;
            string matchPattern = @"\[(.*?)\]";
            string reslt = "";
            string docType = string.Empty;
            MatchCollection parameters;
            bool parameterFound = false;
            //  List<string> IsExistDoc = new List<string>();
     
            foreach (Expression item in args.Parameters)
            {

                reslt = item.ParsedExpression.ToString();
                parameters = Regex.Matches(reslt, matchPattern);
                docType = parameters[0].Groups[1].ToString();
               object outObj = null;
                item.Parameters.TryGetValue("isexist", out outObj);
                foreach (KeyValuePair<string, object> i in item.Parameters)
                {
                    if(i.Key=="isexist")
                    {

                        var  IsExistDoc = ((IEnumerable)i.Value).Cast<object>()
                                    .Select(x => x == null ? x : x.ToString())
                                    .ToList();
                       
                         parameterFound = false;
                        if ( IsExistDoc.Contains(docType))
                        {
                            parameterFound = true;
                        }
                        Rslt.Add(reslt, parameterFound);
                       
                    }

                  
                }
                
            }

            return Rslt;
        }


        private string TryGetValueFromDictionary(string key, Expression e)
        {
            List<string> _value = new List<string>();
            string matchPattern = @"\[(.*?)\]";
            string reslt = e.ParsedExpression.ToString();
            MatchCollection parameters = Regex.Matches(reslt, matchPattern);
            string docType = string.Empty;
            bool parameterFound = false;
            foreach (Match m in parameters)
            {
                docType = m.Groups[1].ToString();
                if (e.Parameters.ContainsKey(key))
                {
                    _value = (List<string>)e.Parameters[key];
                    parameterFound = _value.Any(x => x == docType);
                    break;
                }                
            }

            if (!parameterFound)
            {
                docType = "'" + docType + "' Document Type";
                throw new DocFoundException(docType + " Not Found");
            }

            return reslt;
        }

        #endregion custom function Handler

        #region Evals
        public MTSRuleResults Eval(MTSEvaluateParameterHandler parameterHandler)
        {
            EvaluateParameter = parameterHandler;
            ruleResult.Clear();
            foreach (var rule in this)
            {
                Expression e = getExpression(rule.Value);

                if (parameterHandler != null)
                {
                    e.EvaluateParameter += ParameterHandler;
                }
                try
                {
                    ruleResult.Add(rule.Key, new MTSRuleResult { Result = e.Evaluate().Result });
                }
                catch (Exception ex)
                {
                    ruleResult.Add(rule.Key, new MTSRuleResult { Result = "Error", Expressions = string.Format("Error Rule Failed Expression:{0} Message:{1} ", getParsedExpression(e), ex.Message) });
                }
            }
            return ruleResult;
        }


        private Dictionary<string, object> ValidateParams(Dictionary<string, object> parameters)
        {
            Dictionary<string, object> copyparameters = new Dictionary<string, object>(parameters, StringComparer.OrdinalIgnoreCase);
            foreach (var item in copyparameters.ToArray())
            {
                Double d;
                DateTime date;
                if (item.Value == null)
                {
                    copyparameters[item.Key] = string.Empty;
                }
                //else if (Double.TryParse(item.Value.ToString(), out d))
                //{
                //    copyparameters[item.Key] = d;
                //}
                else if (Double.TryParse(item.Value.ToString(), NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, new CultureInfo("en-US"), out d))
                {
                    copyparameters[item.Key] = d;
                }
                else if (DateTime.TryParse(item.Value.ToString(), out date))
                {
                    copyparameters[item.Key] = date;
                }
            }
            return copyparameters;

        }

        public MTSRuleResults Eval(Dictionary<string, object> parameters)
        {

            ruleResult.Clear();
            Dictionary<string, object> param = ValidateParams(parameters);
            foreach (var rule in this)
            {
                Expression e = getExpression(rule.Value);
                e.Parameters = param;
                try
                {
                    MTSResult _res = e.Evaluate();
                    ruleResult.Add(rule.Key, new MTSRuleResult { Result = _res.Result, Expressions = getParsedExpression(e), ErrorMessage = string.Empty, Message = string.IsNullOrEmpty(_res.Message) ? string.Empty : _res.Message });

                }
                catch (DocFoundException ex)
                {
                    ruleResult.Add(rule.Key, new MTSRuleResult { Result = false, Expressions = string.Format("{0}", rule.Value), ErrorMessage = string.Empty, Message = ex.Message });
                }
                catch (ComparDocNotFoundException ex)
                {
                    ruleResult.Add(rule.Key, new MTSRuleResult { Result = false, Expressions = getParsedExpression(e), ErrorMessage = tryGetParsedExpression(e), Message = string.Empty });
                }
                catch (EncompassException ex)
                {
                    ruleResult.Add(rule.Key, new MTSRuleResult { Result = false, Expressions = string.Format("{0}", rule.Value), ErrorMessage = string.Empty, Message = ex.Message });
                }
                catch (DataTableException ex)
                {
                    ruleResult.Add(rule.Key, new MTSRuleResult { Result = false, Expressions = string.Format("{0}", rule.Value), ErrorMessage = string.Empty, Message = ex.Message });
                }
                catch (ParameterNotFoundException ex)
                {
                    ruleResult.Add(rule.Key, new MTSRuleResult { Result = false, Expressions = string.Format("{0}", rule.Value), ErrorMessage = ex.Message, Message = ex.Message });
                }
                catch (CustomFormatException ex)
                {
                    ruleResult.Add(rule.Key, new MTSRuleResult { Result = false, Expressions = string.Format("{0}", rule.Value), ErrorMessage = ex.Message, Message = ex.Message });
                }
                catch (Exception ex)
                {
                    ruleResult.Add(rule.Key, new MTSRuleResult { Result = false, Expressions = string.Format("{0}", rule.Value), ErrorMessage = ex.Message, Message = string.Empty });
                    //ruleResult.Add(rule.Key, new MTSRuleResult { Result = "Error", Expressions = string.Format("Error Rule Failed Expression:'{0}' Message:{1} Errors{2}", rule.Value, ex.Message, e.Error) });
                }
            }

            return ruleResult;
        }

        //public MTSRuleResults EvalAsString(Dictionary<string, object> parameters)
        //{
        //    Dictionary<string, object> validparams = ValidateParams(parameters);
        //    Dictionary<string, string> customfunction = new Dictionary<string, string>();

        //    ruleResult.Clear();
        //    string matchPattern = @"\[(.*?)\]";
        //    MatchCollection mc;

        //    foreach (var rule in this)
        //    {
        //        string reslt = rule.Value;
        //        //Remove Custom functions*******************
        //        foreach (var item in customFunctions)
        //        {
        //            mc = Regex.Matches(reslt, item, RegexOptions.IgnoreCase);
        //            foreach (Match m in mc)
        //            {
        //                string guid = Guid.NewGuid().ToString();
        //                customfunction.Add(guid, m.Groups[0].ToString());
        //                reslt = reslt.Replace(m.Groups[0].ToString(), guid);
        //            }

        //        }

        //        //Replace parametesrr
        //        mc = Regex.Matches(reslt, matchPattern);
        //        foreach (Match m in mc)
        //        {
        //            if (validparams[m.Groups[1].ToString()].ToString().Trim() != string.Empty)
        //            {
        //                reslt = reslt.Replace(string.Concat("[", m.Groups[1].ToString(), "]"), validparams[m.Groups[1].ToString()].ToString());
        //            }

        //        }
        //        //Add Custom functions back ;-)****************************

        //        foreach (var item in customfunction)
        //        {
        //            reslt = reslt.Replace(item.Key, item.Value);
        //        }

        //        Expression e = getExpression(reslt);
        //        e.Parameters = validparams;
        //        try
        //        {
        //            ruleResult.Add(rule.Key, new MTSRuleResult { Result = e.Evaluate().Result, Expressions = getPassedExpression(e) });
        //        }
        //        catch (Exception ex)
        //        {

        //            ruleResult.Add(rule.Key, new MTSRuleResult { Result = "Error", Expressions = string.Format("Error Rule Failed Expression:{0} Message:{1} ", getPassedExpression(e), ex.Message) });
        //        }
        //    }

        //    return ruleResult;
        //}
        #endregion Evals

        #region PrameterHandler
        public void ParameterHandler(string name, ParameterArgs args)
        {
            if (EvaluateParameter != null)
            {
                MTSParameterArgs mArgs = new MTSParameterArgs();
                EvaluateParameter(name, mArgs);
                args.Result = mArgs.Result;
            }
        }
        #endregion PrameterHandler
    }

    public class ParameterNotFoundException : Exception
    {

        public ParameterNotFoundException()
        { }

        public ParameterNotFoundException(string message)
            : base(message)
        { }

        public ParameterNotFoundException(string message, Exception StackTrace)
            : base(message, StackTrace)
        { }
    }

    public class DocFoundException : Exception
    {

        public DocFoundException()
        { }

        public DocFoundException(string message)
            : base(message)
        { }

        public DocFoundException(string message, Exception StackTrace)
            : base(message, StackTrace)
        { }
    }

    public class ComparDocNotFoundException : Exception
    {

        public ComparDocNotFoundException()
        { }

        public ComparDocNotFoundException(string message)
            : base(message)
        { }

        public ComparDocNotFoundException(string message, Exception StackTrace)
            : base(message, StackTrace)
        { }
    }

    public class EncompassException : Exception
    {

        public EncompassException()
        { }

        public EncompassException(string message)
            : base(message)
        { }

        public EncompassException(string message, Exception StackTrace)
            : base(message, StackTrace)
        { }
    }

    public class DataTableException : Exception
    {

        public DataTableException()
        { }

        public DataTableException(string message)
            : base(message)
        { }

        public DataTableException(string message, Exception StackTrace)
            : base(message, StackTrace)
        { }
    }

    public class CustomFormatException : Exception
    {

        public CustomFormatException()
        { }

        public CustomFormatException(string message)
            : base(message)
        { }

        public CustomFormatException(string message, Exception StackTrace)
            : base(message, StackTrace)
        { }
    }
}
