using BlazorDexie.Database;
using BlazorDexie.JsModule;
using Cashier.Domain;

namespace Cashier.Data
{
    public class CashierDb : Db
    {
        public Store<Xact, int> Xacts { get; set; } = new("++id", "date");
        public Store<Account, int> Accounts { get; set; } = new("name");
        public Store<Payee, int> Payees { get; set; } = new("name");
        public Store<AssetAllocation, int> AssetAllocation { get; set; } = new("fullname");
        public Store<ScheduledXact, int> ScheduledXacts { get; set; } = new("++id", "nextDate");
        public Store<LastXact, int> LastXacts { get; set; } = new("payee");

        public CashierDb(IModuleFactory moduleFactory)
            : base("Cashier", 1, new DbVersion[0], moduleFactory)
        {

        }
    }
}
