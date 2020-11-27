
namespace Encompass.WebConnector.Models
{
    public class EncompassWebConnectorSession : WebConnectorSession
    {
        public EncompassWebConnectorSession(WebConnectorSession webConnectorSession) : base()
        {
            Token = webConnectorSession.Token;
            TokenType = webConnectorSession.TokenType;
        }

        public override string ToString()
        {
            return string.Format($"Token {Token} TokenType {TokenType}");
        }

    }
}