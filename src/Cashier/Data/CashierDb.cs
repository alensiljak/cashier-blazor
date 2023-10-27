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
        public IndexedSet<Xact> Xacts { get; set; }

    }
}
