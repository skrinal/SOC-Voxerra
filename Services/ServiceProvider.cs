﻿namespace Voxerra.Services
{
    public class ServiceProvider
    {
        //private static ServiceProvider _instance;
        //private string _serverRootUrl = "https://192.168.0.114:7264";
        
        
        public string _accesToken = "";
        public string _refreshToken = "";
        public const string RefreshTokenKey = "refresh_token";
        private DevHttpsConnectionHelper _devSslHelper;
        
        
        //private ServiceProvider() { }
        //public static ServiceProvider GetInstance()
        //{
        //    if (_instance == null)
        //        _instance = new ServiceProvider();

        //    return _instance;
        //}

        public ServiceProvider()
        {
            _devSslHelper = new DevHttpsConnectionHelper(sslPort: 7264);
        }

        //public async Task <AuthenticateResponse> Authenticate ( AuthenticateRequest request)
        //{
        //    var devSslHelper = new DevHttpsConnectionHelper(sslPort: 7264);
        //    using (HttpClient client = devSslHelper.HttpClient)
        //    {
        //        client.Timeout = TimeSpan.FromSeconds(10);
        //        var httpRequestMessage = new HttpRequestMessage();
        //        httpRequestMessage.Method = HttpMethod.Post;
        //        httpRequestMessage.RequestUri = new Uri(devSslHelper.DevServerRootUrl + "/Authenticate/Authenticate");

        //        if (request != null)
        //        {
        //            string jsonContent = JsonConvert.SerializeObject(request);
        //            var httpContent = new StringContent(jsonContent, encoding: Encoding.UTF8, "application/json"); ;
        //            httpRequestMessage.Content = httpContent;
        //        }
        //        try
        //        {
        //            var response = await client.SendAsync(httpRequestMessage);
        //            var responseContent = await response.Content.ReadAsStringAsync();

        //            var result = JsonConvert.DeserializeObject<AuthenticateResponse>(responseContent);
        //            result.StatusCode = (int)response.StatusCode;

        //            if (result.StatusCode == 200)
        //            {
        //                _accesToken = result.Token;
        //            }
        //            return result;
        //        }
        //        catch (Exception ex)
        //        {
        //            var result = new AuthenticateResponse
        //            {
        //                StatusCode = 500,
        //                StatusMessage = ex.Message
        //            };
        //            return result;
        //        }
        //    }
        //}

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest request)
        {
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = new Uri(_devSslHelper.DevServerRootUrl + "/Authenticate/Authenticate");

            if (request != null)
            {
                string jsonContent = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(jsonContent, encoding: Encoding.UTF8, "application/json"); ;
                httpRequestMessage.Content = httpContent;
            }
            try
            {
                var response = await _devSslHelper.HttpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<AuthenticateResponse>(responseContent);
                result.StatusCode = (int)response.StatusCode;

                if (result.StatusCode == 200)
                {
                    _accesToken = result.Token;
                    _refreshToken = result.RefreshToken;

                    Task.Run(async () =>
                    {
                        await SaveRefreshToken(_refreshToken)
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                var result = new AuthenticateResponse
                {
                    StatusCode = 500,
                    StatusMessage = ex.Message
                };
                return result;
            }
        }

        public async Task SaveRefreshToken(string refreshToken) {
            try 
           {
                await SecureStorage.SetAsync(RefreshTokenKey, refreshToken);
           }
           catch { }
            
        }

        //public async Task<TResponse> CallWebApi<TRequest, TResponse>(
        //    string apiUrl, HttpMethod httpMethod, TRequest request) where TResponse:BaseResponse
        //{
        //    var devSslHelper = new DevHttpsConnectionHelper(sslPort: 7264);
        //    using (HttpClient client = devSslHelper.HttpClient)
        //    {
        //        client.Timeout = TimeSpan.FromSeconds(10);
        //        var httpRequestMessage = new HttpRequestMessage();
        //        httpRequestMessage.Method = httpMethod;
        //        httpRequestMessage.RequestUri = new Uri(devSslHelper.DevServerRootUrl + apiUrl);
        //        httpRequestMessage.Headers.Authorization =
        //            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer" + _accesToken);

        //        if (request != null)
        //        {
        //            string jsonContent = JsonConvert.SerializeObject(request);
        //            var httpContent = new StringContent(jsonContent, encoding: Encoding.UTF8, "application/json"); ;
        //            httpRequestMessage.Content = httpContent;
        //        }
        //        try
        //        {
        //            var response = await client.SendAsync(httpRequestMessage);
        //            var responseContent = await response.Content.ReadAsStringAsync();

        //            var result = JsonConvert.DeserializeObject<TResponse>(responseContent);
        //            result.StatusCode = (int)response.StatusCode;

        //            return result;
        //        }
        //        catch (Exception ex)
        //        {
        //            var result = Activator.CreateInstance<TResponse>();
        //            result.StatusCode = 500;
        //            result.StatusMessage = $"Unexpected error: {ex.Message}";
        //            return result;
        //        }
        //    }
        //}

        public async Task<TResponse> CallWebApi<TRequest, TResponse>(
            string apiUrl, HttpMethod httpMethod, TRequest request) where TResponse : BaseResponse
        {
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = httpMethod;
            httpRequestMessage.RequestUri = new Uri(_devSslHelper.DevServerRootUrl + apiUrl);
            httpRequestMessage.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accesToken);

            if (request != null)
            {
                string jsonContent = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(jsonContent, encoding: Encoding.UTF8, "application/json"); ;
                httpRequestMessage.Content = httpContent;
            }
            try
            {
                var response = await _devSslHelper.HttpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<TResponse>(responseContent);
                result.StatusCode = (int)response.StatusCode;

                return result;
            }
            catch (Exception ex)
            {
                var result = Activator.CreateInstance<TResponse>();
                result.StatusCode = 500;
                result.StatusMessage = ex.Message;
                return result;
            }
        }

    }
}
