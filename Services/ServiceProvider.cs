using System.Net.Http.Headers;

namespace Voxerra.Services
{
    public class ServiceProvider
    {
        public string _accesToken = "";

        //public string _refreshToken = "";
        //public const string RefreshTokenKey = "refresh_token";
        private DevHttpsConnectionHelper _devSslHelper;

        public ServiceProvider()
        {
            _devSslHelper = new DevHttpsConnectionHelper(sslPort: 42069);
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest request)
        {
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = new Uri(_devSslHelper.DevServerRootUrl + "/Authenticate/Authenticate");

            if (request != null)
            {
                string jsonContent = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(jsonContent, encoding: Encoding.UTF8, "application/json");
                ;
                httpRequestMessage.Content = httpContent;
            }

            try
            {
                var response = await _devSslHelper.HttpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<AuthenticateResponse>(responseContent);
                result.StatusCode = (int)response.StatusCode;

                if (result.StatusCode == 222)
                {
                    return result;
                }

                if (result.StatusCode == 200)
                {
                    _accesToken = result.Token;
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

        public async Task<BaseResponse> ChangePassword(string email)
        {
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = new Uri(_devSslHelper.DevServerRootUrl + "/Password/ResetPassword");

            if (email != null)
            {
                string jsonContent = JsonConvert.SerializeObject(email);
                var httpContent = new StringContent(jsonContent, encoding: Encoding.UTF8, "application/json");
                httpRequestMessage.Content = httpContent;
            }

            try
            {
                var response = await _devSslHelper.HttpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<AuthenticateResponse>(responseContent);
                result.StatusCode = (int)response.StatusCode;

                if (result.StatusCode == 222)
                {
                    return result;
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

        
        public async Task<TResponse> CallWebApi<TRequest, TResponse>(
            string apiUrl, HttpMethod httpMethod, TRequest request) where TResponse : BaseResponse, new()
        {
            var httpRequest = new HttpRequestMessage
            {
                Method = httpMethod,
                RequestUri = new Uri(_devSslHelper.DevServerRootUrl + apiUrl),
                Headers = { Authorization = new AuthenticationHeaderValue("Bearer", _accesToken) }
            };

            if (request != null)
            {
                httpRequest.Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8,
                    "application/json");
            }

            try
            {
                using var response = await _devSslHelper.HttpClient.SendAsync(httpRequest);
                var responseContent = await response.Content.ReadAsStringAsync();

                
                var result = new TResponse
                {
                    StatusCode = (int)response.StatusCode,
                    StatusMessage = response.IsSuccessStatusCode ? "Success" : "Failure"
                };
                
                
                if (string.IsNullOrEmpty(responseContent)) return result;

                var targetProperty = typeof(TResponse).GetProperties()
                    .FirstOrDefault(p => p.PropertyType.IsGenericType &&
                                         p.PropertyType.GetGenericTypeDefinition() == typeof(List<>));

                try
                {
                    if (targetProperty != null && IsArrayResponse(responseContent))
                    {
                        var elementType = targetProperty.PropertyType.GetGenericArguments()[0];
                        var listType = typeof(List<>).MakeGenericType(elementType);
                        var list = JsonConvert.DeserializeObject(responseContent, listType);
                        targetProperty.SetValue(result, list);
                    }
                    else
                    {
                        JsonConvert.PopulateObject(responseContent, result);
                    }
                }
                catch (JsonException)
                {
                    /* Handle deserialization errors if needed */
                }

                
                
                return result;
            }
            catch (Exception ex)
            {
                return new TResponse { StatusCode = 500, StatusMessage = ex.Message };
            }
        }

        private static bool IsArrayResponse(string content) => content.StartsWith("[") && content.EndsWith("]");
        
    }
}