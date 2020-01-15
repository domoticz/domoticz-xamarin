using System.Net.Http;

namespace NL.HNOGames.Domoticz.Helpers
{
    public interface IHTTPClientHandlerCreationService
    {
        HttpClientHandler GetInsecureHandler();
    }
}
