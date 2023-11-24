using BlazorDexie.Database;
using Cashier.Model;

namespace Cashier.Data
{
    public interface IDexieDAL
    {
        Store<Account, string> Accounts { get; set; }
        Store<AssetAllocation, string> AssetAllocations { get; set; }
        Store<LastXact, long> LastTransactions { get; set; }
        Store<Payee, string> Payees { get; set; }
        //Store<Posting, string> Postings { get; set; }
        Store<ScheduledXact, string> ScheduledXacts { get; set; }
        Store<Setting, string> Settings { get; set; }
        Store<Xact, long> Xacts { get; set; }
    }
}
