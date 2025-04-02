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

/*using System.Net.Security;
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
            "voxerra.tplinkdns.com"; // Use your router's domain
            //"localhost";
#elif ANDROID
            "voxerra.tplinkdns.com"; // Use your router's domain
            //"10.0.2.2";
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
}*/


using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

#if ANDROID
using Xamarin.Android.Net;
using Javax.Net.Ssl;
using Java.Lang;
#endif

namespace Voxerra.Helpers
{
    public class DevHttpsConnectionHelper
    {
        private readonly Lazy<HttpClient> _lazyHttpClient;

        public DevHttpsConnectionHelper(int sslPort)
        {
            SslPort = sslPort;
            DevServerRootUrl = $"https://{DevServerName}:{SslPort}";
            _lazyHttpClient = new Lazy<HttpClient>(() => new HttpClient(GetPlatformMessageHandler()));
        }

        public int SslPort { get; }
        public string DevServerName =>
#if WINDOWS
            "localhost";
            //"ec2-51-20-3-224.eu-north-1.compute.amazonaws.com"; // Use your router's domain
#elif ANDROID
            //"10.0.2.2";
            "192.168.68.110";
            //"192.168.68.114";
            //"ec2-51-20-3-224.eu-north-1.compute.amazonaws.com"; // Use your router's domain
#else
            throw new PlatformNotSupportedException("Only Windows and Android currently supported.");
#endif

        public string DevServerRootUrl { get; }

        public HttpClient HttpClient => _lazyHttpClient.Value;

        public HttpMessageHandler? GetPlatformMessageHandler()
        {
#if WINDOWS
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = ValidateServerCertificate,
                UseProxy = false,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
#elif ANDROID
            var handler = new CustomAndroidMessageHandler();
            handler.ServerCertificateCustomValidationCallback = ValidateServerCertificate;
            return handler;
#else
            throw new PlatformNotSupportedException("Only Windows and Android currently supported.");
#endif
        }

        private bool ValidateServerCertificate(
            HttpRequestMessage message,
            X509Certificate2? cert,
            X509Chain? chain,
            SslPolicyErrors errors)
        {
            try
            {
                if (cert == null)
                    return true;

                if (cert.Issuer.Equals("CN=localhost", StringComparison.OrdinalIgnoreCase) ||
                    cert.Subject.Equals("CN=localhost", StringComparison.OrdinalIgnoreCase) ||
                    cert.Issuer.Equals("CN=ec2-51-20-3-224.eu-north-1.compute.amazonaws.com", StringComparison.OrdinalIgnoreCase) ||
                    cert.Subject.Equals("CN=ec2-51-20-3-224.eu-north-1.compute.amazonaws.com", StringComparison.OrdinalIgnoreCase) ||
                    cert.Issuer.Equals("CN=192.168.68.110", StringComparison.OrdinalIgnoreCase) ||
                    cert.Subject.Equals("CN=192.168.68.110", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Self-signed certificate accepted.");
                    return true;
                }

                if (errors == SslPolicyErrors.None)
                {
                    Console.WriteLine("Valid certificate accepted.");
                    return true;
                }

                Console.WriteLine($"Certificate validation failed: {errors}");
                return false;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error during certificate validation: {ex.Message}");
                return false;
            }
        }

#if ANDROID
        internal sealed class CustomAndroidMessageHandler : AndroidMessageHandler
        {
            protected override IHostnameVerifier GetSSLHostnameVerifier(HttpsURLConnection connection)
                => new CustomHostnameVerifier();

            private sealed class CustomHostnameVerifier : Java.Lang.Object, IHostnameVerifier
            {
                public bool Verify(string? hostname, ISSLSession? session)
                {
                    try
                    {
                        if (hostname == "192.168.68.110") // && session.PeerPrincipal?.Name == "CN=localhost
                            return true;

                        return HttpsURLConnection.DefaultHostnameVerifier.Verify(hostname, session);
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine($"Error during hostname verification: {ex.Message}");
                        return false;
                    }
                }
            }
        }
#endif
    }
}

