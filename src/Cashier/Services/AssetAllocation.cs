using Cashier.Data;
using Cashier.Model;
using Microsoft.JSInterop;
using System.Reflection.Emit;
using System.Security.Principal;
using System.Text;
using Tomlyn;
using Tomlyn.Model;
using Tomlyn.Syntax;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Cashier.Services
{
    /// <summary>
    /// All Asset Allocation functionality. Migrated from Cashier.
    /// </summary>
    public class AssetAllocationService
    {
        public List<AssetClass> classes = [];

        private Dictionary<string, AssetClass> _assetClassIndex = [];
        private Dictionary<string, string> _stockIndex = []; // symbol / asset class
        //private IJSRuntime _jsRuntime;
        private ISettingsService _settings;
        private IDexieDAL _db;
        private IAccountService _accountService;

        public AssetAllocationService(ISettingsService settings, IDexieDAL dal, IAccountService accountService)
        {
            _settings = settings;
            _db = dal;
            _accountService = accountService;
        }

        public string GetTextReport()
        {
            var output = new StringBuilder();

            foreach (var asset in classes)
            {
                // Indentation
                //output.Append(' ', asset.Depth * 2);
                var indent = new string(' ', asset.Depth * 2);
                var nameWithIndent = indent + asset.Name;

                output.Append(nameWithIndent.PadRight(20, ' '));
                output.Append(' ');

                output.Append(asset.Allocation);
                output.AppendLine();
            }
            return output.ToString();
        }

        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="toml">Asset Allocation definition in TOML</param>
        /// <returns></returns>
        public async Task loadFullAssetAllocation(string toml)
        {
            // load definition
            //await LoadAssetAllocation();
            classes = ParseDefinition(toml);

            // build asset class index
            _assetClassIndex = BuildAssetClassIndex();

            // build stock index
            BuildStockIndex();

            // load current values
            await LoadCurrentValues();

            // sum group balances
            SumGroupBalances();

            // calculate offsets
            //calculateOffsets();

        }

        private Dictionary<string, AssetClass> BuildAssetClassIndex()
        {
            var index = new Dictionary<string, AssetClass>();
            foreach (var item in classes)
            {
                index.Add(item.FullName, item);
            }
            return index;
        }

        private void BuildStockIndex()
        {
            foreach (var assetClass in classes)
            {
                if (assetClass.Symbols.Count == 0) continue;

                foreach (var symbol in assetClass.Symbols)
                {
                    _stockIndex.Add(symbol, assetClass.FullName);
                }
            }
        }

        /// <summary>
        /// Loads the Asset Allocation definition by parsing the TOML string.
        /// </summary>
        /// <param name="toml">Definition. Usually read from a file.</param>
        public List<AssetClass> ParseDefinition(string toml)
        {
            // parse TOML
            var model = Toml.ToModel(toml);
            //var root = model.First();

            //RecursivelyDisplay(root, 0);
            //RecursivelyParse(root);
            var aa = LinearizeTable(model);
            return aa;
        }

        // Private

        private IEnumerable<KeyValuePair<string, object>> GetSections(TomlTable item)
        {
            var subsections = item.Where(pair => pair.Value is TomlTable)
                .Select(pair => pair);
            return subsections;
        }

        /// <summary>
        /// Load current balances from accounts.
        /// </summary>
        /// <returns></returns>
        private async Task LoadCurrentValues()
        {
            var currency = await _settings.GetDefaultCurrency();
            var invAccounts = await _accountService.LoadInvestmentAccounts(_settings, _db);

            foreach (var account in invAccounts)
            {
                var amount = Decimal.Zero;
                var commodity = currency;

                // Current Value is populated from Ledger. Only the active accounts will have a value.
                if (!string.IsNullOrEmpty(account.CurrentValue))
                {
                    amount = Decimal.Parse(account.CurrentValue);
                }
                else
                {
                    continue;
                }

                var acctBalance = _accountService.GetAccountBalance(account, currency);
                // the the account balance.
                account.AccountBalance = acctBalance;

                if (account.AccountBalance.Amount == 0) continue;

                commodity = account.AccountBalance.Currency;

                // Now get the asset class for this commodity.
                var assetClassName = _stockIndex[commodity!];
                if (assetClassName == null)
                {
                    throw new Exception($"Asset class name not found for commodity {commodity}");
                }

                var assetClass = _assetClassIndex[assetClassName];
                assetClass.CurrentValue.Amount += amount;
                assetClass.CurrentValue.Currency = currency;
            }
        }

        private void RecursivelyDisplay(KeyValuePair<string, object> pair, int level)
        {
            // The indentation is 2 spaces per level.
            var indent = new string(' ', level * 2);

            TomlTable table = (TomlTable)pair.Value;
            Console.WriteLine($"{indent}{pair.Key}");

            // now display the subsections
            var sections = GetSections(table);
            foreach (var section in sections)
            {
                RecursivelyDisplay(section, level + 1);
            }
        }

        /// <summary>
        /// Creates a linear representation of the Asset Allocation table.
        /// It is not a tree but a list of all asset classes.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        private List<AssetClass> LinearizeTable(TomlTable table, string nameSpace = "")
        {
            var result = new List<AssetClass>();

            var subsections = GetSections(table);
            foreach (var section in subsections)
            {
                var child = new AssetClass();
                if (!string.IsNullOrEmpty(nameSpace))
                {
                    child.FullName = nameSpace + ':' + section.Key;
                }
                else
                {
                    child.FullName = section.Key;
                }

                var childTable = (TomlTable)section.Value;
                child.Allocation = Convert.ToDecimal(childTable["allocation"]);
                if (childTable.ContainsKey("symbols"))
                {
                    var symbolsArray = (TomlArray)childTable["symbols"];
                    child.Symbols = symbolsArray.Select(item => (string)item!).ToList();
                }

                result.Add(child);

                // parse recursively
                var childNamespace = nameSpace;
                if (!string.IsNullOrEmpty(childNamespace))
                {
                    childNamespace += ':';
                }
                childNamespace += section.Key;

                var grandchildren = LinearizeTable(childTable, childNamespace);
                result.AddRange(grandchildren);
            }

            return result;
        }

        private void SumGroupBalances()
        {
            var root = _assetClassIndex["Allocation"];

            var sum = SumChildren(root);
            
            root.CurrentValue.Amount = sum;
        }

        private Decimal SumChildren(AssetClass item)
        {
            var children = FindChildren(item);
            if (children.Count == 0) {
                return item.CurrentValue?.Amount ?? 0;
            }

            var sum = 0m;
            foreach (var child in children)
            {
                //var sum = children.Sum(child => child.CurrentValue.Amount);
                child.CurrentValue.Amount = SumChildren(child);
                sum += child.CurrentValue.Amount ?? 0;
            }
            return sum;
        }

        private List<AssetClass> FindChildren(AssetClass parent)
        {
            return _assetClassIndex
                .Where(x => x.Value.ParentName == parent.FullName)
                .Select(x => x.Value)
                .ToList();
        }
    }
}
