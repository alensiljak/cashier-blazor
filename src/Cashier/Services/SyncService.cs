namespace Cashier.Services
{
    /// <summary>
    /// Synchronizes with the Cashier Server.
    /// </summary>
    public class SyncService
    {
        private HttpClient _httpClient;
        private string _serverUrl;

        public SyncService(HttpClient client, string serverUrl) { 
            _httpClient = client;
            _serverUrl = serverUrl;
        }

        public async Task test()
        {
            HttpResponseMessage? response;
            var url = $"{_serverUrl}?command=b gratis";
            try
            {
                response = await _httpClient.GetAsync(url);
            } catch (HttpRequestException rex)
            {
                // The server is not up?
                Console.WriteLine("Error: {0}", rex.Message);
                return;
            }

            var code = response.StatusCode;
            Console.WriteLine("Code: {0}", code);

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Content: {0}", content);
        }
    }
}
