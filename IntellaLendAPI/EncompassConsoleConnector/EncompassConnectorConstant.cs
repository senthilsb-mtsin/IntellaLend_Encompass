using System.Collections.Generic;

namespace EncompassConsoleConnector
{
    public class EncompassConnectorConstant
    {
        public static string ENCOMPASS_SERVER = "ENCOMPASS_SERVER";
        public static string ENCOMPASS_USERNAME = "ENCOMPASS_USERNAME";
        public static string ENCOMPASS_PASSWORD = "ENCOMPASS_PASSWORD";
        public static string ENCOMPASS_CONFIG_TABLE = "[IL].APPCONFIG";
    }

    public class EncompassConnectorMessage
    {
        protected static readonly Dictionary<string, string> MessageConstant = new Dictionary<string, string>()
        {
            { "1", "Encompass Credentials Unavailable" },
            { "2","Loan Not Found" },
            { "3" ,"Field value NULL"},
            { "4" ,"Login Failed. Enter Proper Credentials"},
        };

        public static bool isError(string _message)
        {
            _message = _message.Trim();

            if (_message.Equals("3"))
                return false;
            else if(MessageConstant.ContainsKey(_message))
                return true;

            return false;
        }

        public static string GetMessageDescription(string Message)
        {

            Message = Message.Trim();

            if (Message.StartsWith("-1|"))
                return Message.Split('|')[1].Trim();
            else if (MessageConstant.ContainsKey(Message))
            {
                if (Message.Equals("3"))
                    return string.Empty;
                else
                    return MessageConstant[Message];
            }
            else
                return Message;
        }
    }
}
