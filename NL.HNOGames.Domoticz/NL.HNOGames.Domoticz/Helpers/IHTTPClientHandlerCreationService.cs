using System.Net.Http;

namespace NL.HNOGames.Domoticz.Helpers
{
    /// <summary>
    /// Defines the <see cref="IHTTPClientHandlerCreationService" />
    /// </summary>
    public interface IHTTPClientHandlerCreationService
    {
        /// <summary>
        /// The GetInsecureHandler
        /// </summary>
        /// <returns>The <see cref="HttpClientHandler"/></returns>
        HttpClientHandler GetInsecureHandler();
    }
}
