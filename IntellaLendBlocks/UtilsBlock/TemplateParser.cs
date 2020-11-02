using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace MTSEntBlocks.UtilsBlock
{

    public class TemplateParser
    {
        //this is for single word
        // private string _matchPattern = @"(\[%\w+%\])";

        //this is for anything between the symbols 
        private string _matchPattern = @"(\<%(.*?)%\>)";

        public delegate string TokenHandler(string strToken, string strfmt, DataRow dr = null);
        public event TokenHandler OnToken;

        public string MatchPattern
        {
            get { return _matchPattern; }
            set { _matchPattern = value; }
        }


        public string ParseString(string Template, DataRow dr = null)
        {

            string outText = Template;
            MatchCollection tags = Regex.Matches(Template, _matchPattern);

            foreach (Match tag in tags)
            {
                string replacement = string.Empty;
                string token = Regex.Match(tag.ToString(), @"%([^%]*)").Groups[1].Value;
                string[] str = token.Split(new char[] { ' ' }, 2);

                if (OnToken != null)
                {
                    //check for fromat string? yes send that else {0}
                    replacement = OnToken(str[0], str.Length == 2 ? str[1] : "{0}", dr);
                    outText = outText.Replace(tag.ToString(), replacement);
                }
            }
            return outText;
        }

        public String ParseFile(string filename)
        {
            return ParseString(ReadFile(filename));
        }

        private String ReadFile(string filename)
        {
            string result;
            try
            {
                TextReader reader = new StreamReader(filename);
                result = reader.ReadToEnd();
                reader.Close();
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }

    }
}
