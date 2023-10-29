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

            await Dexie.Xacts().Add(new Xact("2023-10-29", "Oebb"));
            await Dexie.Xacts().Add(new Xact("2023-10-30", "Drakon"));

            await LoadData();
        }

        private async Task GoodTransaction()
        {
            LogMessage("Good Transaction");

            await Dexie.Transaction(async tx =>
            {
                var key = await Dexie.Xacts().Add(new Xact("2023-11-01", "Halloween"));
                var xact = await Dexie.Xacts().Get(key);
            });
        }

        private async Task FailedTransaction()
        {
            LogMessage("Failed Transaction");

            try
            {

            } catch (Exception ex)
            {
                LogMessage(ex.Message);
            }
        }

        private void LogMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
