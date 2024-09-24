namespace Voxerra.Services
{
    public class ServiceProvider
    {
        private static ServiceProvider _instance;
        private string _serverRootUrl = "https://localhost:7264";
        public string _accesToken = "";
        private ServiceProvider() { }

        public static ServiceProvider GetInstance()
        {
            if (_instance == null)
                _instance = new ServiceProvider();

            return _instance;
        }

        public async Task <AuthenticateResponse> Authenticate ( AuthenticateRequest request)
        {
            using (HttpClient client = new HttpClient())
            {
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                httpRequestMessage.RequestUri = new Uri(_serverRootUrl + "/Authenticate/Authenticate");

                if (request != null)
                {
                    string jsonContent = JsonConvert.SerializeObject(request);
                    var httpContent = new StringContent(jsonContent, encoding: Encoding.UTF8, "application/json"); ;
                    httpRequestMessage.Content = httpContent;
                }
                try
                {
                    var response = await client.SendAsync(httpRequestMessage);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<AuthenticateResponse>(responseContent);  
                    result.StatusCode = (int)response.StatusCode;
                    
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
            }
    }
}
