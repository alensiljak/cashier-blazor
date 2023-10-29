using Cashier.Domain;
using IndexedDB.Blazor;
using Microsoft.JSInterop;
using System;

namespace Cashier.Data
{
    public class CashierDb : IndexedDb
    {
        public CashierDb(IJSRuntime jSRuntime, string name, int version) : base(jSRuntime, name, version) { }

        public IndexedSet<Account> Accounts { get; set; }
        public IndexedSet<Payee> Payees { get; set; }

        public IndexedSet<Xact> Xacts { get; set; } = default;

        public IndexedSet<ScheduledXact> ScheduledXacts { get; set; }

        public IndexedSet<AssetAllocation> AssetAllocation { get; set; }
        public IndexedSet<LastXact> LastXacts { get; set; }

    }
}
