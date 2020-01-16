using Android.Net;
using Javax.Net.Ssl;
using NL.HNOGames.Domoticz.Droid.Helpers;
using NL.HNOGames.Domoticz.Helpers;
using System.Net.Http;
using Xamarin.Android.Net;

[assembly: Xamarin.Forms.Dependency(typeof(HTTPClientHandlerCreationService))]
namespace NL.HNOGames.Domoticz.Droid.Helpers
{
    /// <summary>
    /// Defines the <see cref="HTTPClientHandlerCreationService" />
    /// </summary>
    public class HTTPClientHandlerCreationService : IHTTPClientHandlerCreationService
    {
        #region Public

        /// <summary>
        /// The GetInsecureHandler
        /// </summary>
        /// <returns>The <see cref="HttpClientHandler"/></returns>
        public HttpClientHandler GetInsecureHandler()
        {
            return new IgnoreSSLClientHandler();
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="IgnoreSSLClientHandler" />
    /// </summary>
    internal class IgnoreSSLClientHandler : AndroidClientHandler
    {
        /// <summary>
        /// The ConfigureCustomSSLSocketFactory
        /// </summary>
        /// <param name="connection">The connection<see cref="HttpsURLConnection"/></param>
        /// <returns>The <see cref="SSLSocketFactory"/></returns>
        protected override SSLSocketFactory ConfigureCustomSSLSocketFactory(HttpsURLConnection connection)
        {
            return SSLCertificateSocketFactory.GetInsecure(10000, null);
        }

        /// <summary>
        /// The GetSSLHostnameVerifier
        /// </summary>
        /// <param name="connection">The connection<see cref="HttpsURLConnection"/></param>
        /// <returns>The <see cref="IHostnameVerifier"/></returns>
        protected override IHostnameVerifier GetSSLHostnameVerifier(HttpsURLConnection connection)
        {
            return new IgnoreSSLHostnameVerifier();
        }
    }

    /// <summary>
    /// Defines the <see cref="IgnoreSSLHostnameVerifier" />
    /// </summary>
    internal class IgnoreSSLHostnameVerifier : Java.Lang.Object, IHostnameVerifier
    {
        #region Public

        /// <summary>
        /// The Verify
        /// </summary>
        /// <param name="hostname">The hostname<see cref="string"/></param>
        /// <param name="session">The session<see cref="ISSLSession"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool Verify(string hostname, ISSLSession session)
        {
            return true;
        }

        #endregion
    }
}
