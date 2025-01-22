//using System.Net.Security;

//namespace Voxerra
//{
//    public class DevHttpsConnectionHelper
//    {
//        public DevHttpsConnectionHelper(int sslPort)
//        {
//            SslPort = sslPort;
//            DevServerRootUrl = FormattableString.Invariant($"https://{DevServerName}:{SslPort}");
//            LazyHttpClient = new Lazy<HttpClient>(() => new HttpClient(GetPlatformMessageHandler()));
//        }

//        public int SslPort { get; }

//        public string DevServerName =>
//#if WINDOWS
//            //"192.168.68.114";
//            //"localhost";
//            "voxerra.tplinkdns.com";
//#elif ANDROID
//            //"192.168.68.114";
//            //"voxerra.ddnsfree.com";
//            //"10.0.2.2";
//            //"192.168.1.6";
//            "voxerra.tplinkdns.com";
//#else
//        throw new PlatformNotSupportedException("Only Windows and Android currently supported.");
//#endif

//        public string DevServerRootUrl { get; }

//        private Lazy<HttpClient> LazyHttpClient;
//        public HttpClient HttpClient => LazyHttpClient.Value;

//        public HttpMessageHandler? GetPlatformMessageHandler()
//        {
//#if WINDOWS
//            return new HttpClientHandler
//            {
//                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
//                {
//                    // Accept self-signed certificates issued to "CN=localhost"
//                    if (cert != null && cert.Issuer.Equals("CN=localhost"))
//                        return true;

//                    return errors == SslPolicyErrors.None;
//                }
//            };
//#elif ANDROID
//            var handler = new CustomAndroidMessageHandler();
//        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
//        {
//            if (cert != null && cert.Issuer.Equals("CN=localhost"))
//                return true;
//            return errors == SslPolicyErrors.None;
//        };
//        return handler;

//#else
//        throw new PlatformNotSupportedException("Only Windows and Android currently supported.");
//#endif
//        }

//#if ANDROID
//    internal sealed class CustomAndroidMessageHandler : Xamarin.Android.Net.AndroidMessageHandler
//    {
//        protected override Javax.Net.Ssl.IHostnameVerifier GetSSLHostnameVerifier(Javax.Net.Ssl.HttpsURLConnection connection)
//            => new CustomHostnameVerifier();

//        private sealed class CustomHostnameVerifier : Java.Lang.Object, Javax.Net.Ssl.IHostnameVerifier
//        {
//            public bool Verify(string? hostname, Javax.Net.Ssl.ISSLSession? session)
//            {
//                return
//                    Javax.Net.Ssl.HttpsURLConnection.DefaultHostnameVerifier.Verify(hostname, session)
//                    || hostname == "10.0.2.2" && session.PeerPrincipal?.Name == "CN=localhost";
//            }
//        }
//    }
//#endif
//    }
//}
using System.Net.Http;
using System.Net.Security;

namespace Voxerra
{
    public class DevHttpsConnectionHelper
    {
        public DevHttpsConnectionHelper(int sslPort)
        {
            SslPort = sslPort;
            DevServerRootUrl = FormattableString.Invariant($"https://{DevServerName}:{SslPort}");
            LazyHttpClient = new Lazy<HttpClient>(() => new HttpClient(GetPlatformMessageHandler()));
        }

        public int SslPort { get; }

        public string DevServerName =>
#if WINDOWS
            //"voxerra.tplinkdns.com"; // Use your router's domain
            "localhost";
#elif ANDROID
            //"voxerra.tplinkdns.com"; // Use your router's domain
            "10.0.2.2";
#else
        throw new PlatformNotSupportedException("Only Windows and Android currently supported.");
#endif

        public string DevServerRootUrl { get; }

        private Lazy<HttpClient> LazyHttpClient;
        public HttpClient HttpClient => LazyHttpClient.Value;

        public HttpMessageHandler? GetPlatformMessageHandler()
        {
#if WINDOWS
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    // Accept self-signed certificates issued to "CN=voxerra.tplinkdns.com"
                    if (cert != null)
                    {
                        if (cert.Issuer.Equals("CN=localhost") || cert.Subject.Equals("CN=localhost"))
                            return true;
                    }

                    return errors == SslPolicyErrors.None;
                }
            };
#elif ANDROID
            var handler = new CustomAndroidMessageHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert != null)
                {
                    if (cert.Issuer.Equals("CN=localhost") || cert.Subject.Equals("CN=localhost"))
                        return true;
                }
                return errors == SslPolicyErrors.None;
            };
            return handler;
#else
            throw new PlatformNotSupportedException("Only Windows and Android currently supported.");
#endif
        }

#if ANDROID
        internal sealed class CustomAndroidMessageHandler : Xamarin.Android.Net.AndroidMessageHandler
        {
            protected override Javax.Net.Ssl.IHostnameVerifier GetSSLHostnameVerifier(Javax.Net.Ssl.HttpsURLConnection connection)
                => new CustomHostnameVerifier();

            private sealed class CustomHostnameVerifier : Java.Lang.Object, Javax.Net.Ssl.IHostnameVerifier
            {
                public bool Verify(string? hostname, Javax.Net.Ssl.ISSLSession? session)
                {
                    return
                        Javax.Net.Ssl.HttpsURLConnection.DefaultHostnameVerifier.Verify(hostname, session)
                        || hostname == "10.0.2.2" && session.PeerPrincipal?.Name == "CN=localhost";
                }
            }
        }
#endif
    }
}