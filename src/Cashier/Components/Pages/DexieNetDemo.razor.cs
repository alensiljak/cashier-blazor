using Cashier.Domain;
using DexieNET;

namespace Cashier.Components.Pages
{
    public partial class DexieNetDemo
    {
        private IEnumerable<Account>? accounts;
        private IEnumerable<Xact>? xacts;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await Dexie.Version(1).Stores();

            await LoadData();
        }

        private async Task LoadData()
        {
            accounts = await Dexie.Accounts().ToArray();
            xacts = await Dexie.Xacts().ToArray();
            
            await InvokeAsync(StateHasChanged);
        }

        private async Task ClearDatabase()
        {
            LogMessage("Clearing database...");

            await Dexie.Accounts().Clear();
            await Dexie.Xacts().Clear();

            await LoadData();
        }

        private async Task PopulateDatabase()
        {
            LogMessage("populating database");

            await Dexie.Accounts().Add(new Account("Assets"));
            await Dexie.Accounts().Add(new Account("Equity"));
            await Dexie.Accounts().Add(new Account("Expenses"));
            await Dexie.Accounts().Add(new Account("Income"));

            await Dexie.Xacts().Add(new Xact("2023-10-29", "Oebb", null, null));

            var postings = new Posting[]
            {
                new Posting("Expenses:Groceries", new Money(20, "EUR")),
                new Posting("Assets:Checking", null)
            };
            await Dexie.Xacts().Add(new Xact("2023-10-30", "Drakon", null, postings));

            await LoadData();
        }

        private async Task GoodTransaction()
        {
            LogMessage("Good transaction example");

            await Dexie.Transaction(async tx =>
            {
                var key = await Dexie.Xacts().Add(new Xact("2023-11-01", "Halloween", null, null));
                var xact = await Dexie.Xacts().Get(key);
            });

            await LoadData();
        }

        private async Task FailedTransaction()
        {
            LogMessage("Failed transaction");

            try
            {
                await Dexie.Transaction(async tx =>
                {
                    await Dexie.Xacts().Clear();
                    var key = await Dexie.Xacts().Add(new Xact("2023-10-18", "Supermarket", null, null));
                    var xact = await Dexie.Xacts().Get(key);
                    // fail
                    await Dexie.Xacts().Add(xact);
                });
            } catch (Exception ex)
            {
                LogMessage(ex.Message);
            }

            await LoadData();
        }

        private void LogMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
