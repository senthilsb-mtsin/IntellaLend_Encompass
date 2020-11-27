
namespace Encompass.WebConnector.Models
{
    public class WebConnectorSession
    {

        public string Token { get; set; }
        public string TokenType { get; set; }


        public WebConnectorSession()
        {
           
        }

        public override string ToString()
        {
            return string.Format($"Token : {Token}, TokenType : {TokenType}");
        }

    }
}