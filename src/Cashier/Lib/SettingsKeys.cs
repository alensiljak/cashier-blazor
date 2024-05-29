namespace Cashier.Lib
{
    public class SettingsKeys
    {
        public const string assetAllocationDefinition = "aa.definition";
        public const string backupServerUrl = "backupServerUrl";
        public const string currency = "currency";
        public const string favouriteAccounts = "favouriteAccounts";
        public const string dbInitialized = "dbInitialized"; // Marks that the db has been initialized
        public const string pCloudToken = "pCloudToken";
        public const string syncServerUrl = "syncServerUrl";
        // path to the prices repository for CashierSync.
        public const string pricesRepositoryPath = "pricesRepositoryPath";
        // path to the book repository for CashierSync.
        public const string repositoryPath = "repositoryPath";
        public const string rootInvestmentAccount = "aa.rootAccount";
        public const string rememberLastTransaction = "rememberLastTransaction";
        public const string writeableJournalFilePath = "writeableJournalFilePath";
        // synchronization choices
        public const string syncAccounts = "syncAccounts";
        public const string syncAaValues = "syncAaValues";
        public const string syncPayees = "syncPayees";
        // home
        public const string visibleCards = "home.visibleCards";
        // forecast
        public const string ForecastAccounts = "forecast.accounts";
    }
}
