using Cashier.Domain;
using IndexedDB.Blazor;
using Microsoft.JSInterop;
using System;

namespace Cashier.Data
{
    public class CashierDb : IndexedDb
    {
        public CashierDb(IJSRuntime jSRuntime, string name, int version) : base(jSRuntime, name, version) { }

        public IndexedSet<Cashier.Domain.Account> Accounts { get; set; }
        public IndexedSet<Cashier.Domain.Payee> Payees { get; set; }

        public IndexedSet<Cashier.Domain.Xact> Xacts { get; set; } = default;

        public IndexedSet<Cashier.Domain.ScheduledXact> ScheduledXacts { get; set; }

        public IndexedSet<Cashier.Domain.AssetAllocation> AssetAllocation { get; set; }
        public IndexedSet<Cashier.Domain.LastXact> LastXacts { get; set; }

    }
}
