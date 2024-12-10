using BlazorDexie.Database;
using Cashier.Data;
using Cashier.Services;
using System.Text;
using WebDav;

namespace Cashier.Lib
{
    public class CloudBackupService
    {
        public CloudBackupService(HttpClient httpClient, string serverUrl)
        {
            _httpClient = httpClient;
            _serverUrl = serverUrl;

            _client = new WebDavClient(httpClient);
        }

        HttpClient _httpClient;
        WebDavClient _client;
        private string _serverUrl;

        /// <summary>
        /// Used to avoid sending too many request.
        /// </summary>
        private List<WebDavResource>? _resourceCache;

        public async Task BackupJournal(IDexieDAL db)
        {
            // get the text data for export
            var output = await AppService.GetXactsForExport(db);
            if (output == null)
            {
                throw new Exception("Could not serialize Transactions");
            }

            // get filename
            var filename = GetFilenameForNewBackup(BackupType.Journal);

            // upload
            var url = GetUrl($"/{filename}");
            var fileContents = new MemoryStream(Encoding.UTF8.GetBytes(output));
            var result = await _client.PutFile(url, fileContents);

            if (!result.IsSuccessful)
            {
                throw new Exception($"Failed to backup Transactions");
            }

        }

        public async Task BackupScheduledXacts(IDexieDAL db)
        {
            // get the JSON data for export
            var output = await AppService.GetScheduledXactsForExport(db);
            if (output == null)
            {
                throw new Exception("Could not serialize Scheduled Transactions");
            }

            // get filename
            var filename = GetFilenameForNewBackup(BackupType.Scheduled);

            // upload
            var url = GetUrl($"/{filename}");
            var fileContents = new MemoryStream(Encoding.UTF8.GetBytes(output));
            var result = await _client.PutFile(url, fileContents);

            if (!result.IsSuccessful)
            {
                throw new Exception($"Failed to backup Scheduled Transactions");
            }
        }

        public async Task BackupSettings(IDexieDAL db)
        {
            // get the JSON data for export
            var output = await AppService.GetSettingsForExport(db);
            if (output == null)
            {
                throw new Exception("Could not serialize Settings");
            }

            // get filename
            var filename = GetFilenameForNewBackup(BackupType.Settings);

            // upload
            var url = GetUrl($"/{filename}");
            var fileContents = new MemoryStream(Encoding.UTF8.GetBytes(output));
            var result = await _client.PutFile(url, fileContents);

            if (!result.IsSuccessful)
            {
                throw new Exception($"Failed to backup Settings");
            }
        }

        public void ClearCache()
        {
            _resourceCache = null;
        }

        /// <summary>
        /// Retrieves the number of backups for the given backup type.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetRemoteBackupCount(BackupType backupType)
        {
            // Get all the files
            var files = await GetFileListing();

            // filter only the backups for Scheduled Xacts
            var result = files.Count(f => f.StartsWith(backupType.ToString().ToLowerInvariant()));

            return result;
        }

        public async Task<string> GetLatestFilename(BackupType backupType)
        {
            var files = await GetFileListing();

            // get only the scheduled xact backups
            var result = files
                .Where(f => f.StartsWith(backupType.ToString().ToLowerInvariant()))
                .OrderByDescending(f => f)
                .FirstOrDefault();

            return result ?? string.Empty;
        }

        public async Task<List<string>> GetFileListing()
        {
            // Fetch the file listing only if the cache is empty.
            if (_resourceCache == null)
            {
                var url = GetUrl("/");
                var response = await _client.Propfind(url);

                // Cache the listing.
                _resourceCache = response.Resources.ToList();
            }

            var result = _resourceCache.Select(r => r.DisplayName).ToList();
            return result;
        }

        public async Task<bool> HealthCheck()
        {
            var request = new HttpRequestMessage(HttpMethod.Options, _serverUrl);
            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        // private 

        private string GetFilenameForNewBackup(BackupType backupType)
        {
            var now = DateTime.Now;

            // file extension
            string extension = backupType switch
            {
                BackupType.Journal => "ledger",
                _ => "json",
            };

            var prefix = backupType.ToString().ToLowerInvariant();
            var date = now.Date.ToString(Constants.ISODateFormat);
            var time = now.ToString(Constants.LongTimeFormat);

            var filename = string.Format($"{prefix}_{date}_{time}.{extension}");

            return filename;
        }

        private string GetUrl(string path)
        {
            return $"{_serverUrl}/{path}";
        }
    }
}
