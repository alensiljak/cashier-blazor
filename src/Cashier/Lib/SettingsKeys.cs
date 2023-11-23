namespace Cashier.Lib
{
    public class SettingsKeys
    {
        public static string assetAllocationDefinition = "aa.definition";
        public static string backupServerUrl = "backupServerUrl";
        public static string currency = "currency";
        public static string favouriteAccounts = "favouriteAccounts";
        public static string dbInitialized = "dbInitialized"; // Marks that the db has been initialized
        public static string pCloudToken = "pCloudToken";
        public static string syncServerUrl = "syncServerUrl";
        // path to the prices repository for CashierSync.
        public static string pricesRepositoryPath = "pricesRepositoryPath";
        // path to the book repository for CashierSync.
        public static string repositoryPath = "repositoryPath";
        public static string rootInvestmentAccount = "aa.rootAccount";
        public static string rememberLastTransaction = "rememberLastTransaction";
        public static string writeableJournalFilePath = "writeableJournalFilePath";
        // synchronization choices
        public static string syncAccounts = "syncAccounts";
        public static string syncAaValues = "syncAaValues";
        public static string syncPayees = "syncPayees";
    }
}
