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
            //_httpClient = httpClient;
            _serverUrl = serverUrl;

            _client = new WebDavClient(httpClient);
        }

        const string FILENAME_TEMPLATE = "{prefix}_{date}_{time}.json";

        WebDavClient _client;
        private string _serverUrl;

        /// <summary>
        /// Used to avoid sending too many request.
        /// </summary>
        private List<WebDavResource>? _resourceCache;

        public async Task BackupScheduled(IDexieDAL db)
        {
            // get the JSON data for export
            var output = await AppService.GetScheduledXactsForExport(db);
            if(output == null)
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

        public void ClearCache()
        {
            _resourceCache = null;
        }

        /// <summary>
        /// Retrieves the number of backups for Scheduled Transactions.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetRemoteBackupCountScheduled()
        {
            // Get all the files
            var files = await GetFileListing();

            // filter only the backups for Scheduled Xacts
            var result = files.Count(f => f.StartsWith(BackupType.Scheduled
                .ToString().ToLowerInvariant()));

            return result;
        }

        public async Task<string> GetLatestFilenameScheduled()
        {
            var files = await GetFileListing();

            // get only the scheduled xact backups
            var result = files
                .Where(f => f.StartsWith(BackupType.Scheduled.ToString().ToLowerInvariant()))
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

        private string GetFilenameForNewBackup(BackupType backupType)
        {
            var now = DateTime.Now;
            var template = FILENAME_TEMPLATE;
            var prefix = backupType.ToString().ToLowerInvariant();

            template = template.Replace("{prefix}", prefix);

            // replace time and date
            template = template.Replace("{date}", now.Date.ToString(Constants.ISODateFormat));

            var time = TimeOnly.FromDateTime(now);
            template = template.Replace("{time}", time.ToString(Constants.LongTimeFormat));

            return template;
        }

        private string GetUrl(string path)
        {
            return $"{_serverUrl}/{path}";
        }
    }
}
