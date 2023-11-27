using BlazorDexie.Database;
using BlazorDexie.JsModule;
using Cashier.Model;
using Microsoft.JSInterop;

namespace Cashier.Data
{
    public class DexieDAL : Db, IDexieDAL
    {
        public Store<Setting, string> Settings { get; set; } = new(nameof(Setting.Key));

        public Store<Account, string> Accounts { get; set; } = new(nameof(Account.Name));
        
        public Store<AssetAllocation, string> AssetAllocations { get; set; } = new(nameof(AssetAllocation.FullName));
        
        public Store<LastXact, string> LastTransactions { get; set; } = new(nameof(LastXact.Payee));
        
        public Store<Payee, string> Payees { get; set; } = new(nameof(Payee.Name));
        
        //public Store<Posting, string> Postings { get; set; } = new(nameof(Posting.Account));
        
        public Store<ScheduledXact, string> ScheduledXacts { get; set; } =
            new("++" + nameof(ScheduledXact.Id), nameof(ScheduledXact.NextDate));
        
        public Store<Xact, long> Xacts { get; set; } = new("++" + nameof(Xact.Id), nameof(Xact.Date));

        public DexieDAL(IJSRuntime jsRuntime)
            : base("Cashier", 1, new DbVersion[] { }, CreateModuleFactory(jsRuntime))
        { }

        private static IModuleFactory CreateModuleFactory(IJSRuntime jsRuntime)
        {
            return new EsModuleFactory(jsRuntime);
        }
    }
}
