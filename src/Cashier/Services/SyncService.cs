using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Cashier.Services
{
    /// <summary>
    /// Synchronizes with the Cashier Server.
    /// </summary>
    public class SyncService
    {
        private const string AccountsCommand = "b --flat --empty --no-total";
        private const string PayeesCommand = "payees";

        private HttpClient _httpClient;
        private string _serverUrl;

        public SyncService(HttpClient client, string serverUrl) { 
            _httpClient = client;
            _serverUrl = serverUrl;
        }

        /// <summary>
        /// Demonstrates communicating with a remote server.
        /// </summary>
        /// <returns></returns>
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

        [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
        public async Task<string[]?> ReadAccounts()
        {
            var response = await this.ledger(AccountsCommand);
            var lines = JsonSerializer.Deserialize<string[]>(response);
            return lines;
        }

        /// <summary>
        /// Retrieve the list of Payees
        /// </summary>
        /// <returns></returns>
        [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
        public async Task<string[]?> ReadPayees()
        {
            var result = await ledger(PayeesCommand);
            var payees = JsonSerializer.Deserialize<string[]>(result);
            return payees;
        }

        // Private

        private string createUrl(string command)
        {
            var path = $"?command={command}";
            var url = $"{this._serverUrl}{path}";
            return url;
        }

        private async Task<string> ledger(string command)
        {
            var url = createUrl(command);

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error communicating with Cashier Server");
            }

            var content = await response.Content.ReadAsStringAsync();
            
            return content;
        }
    }
}
