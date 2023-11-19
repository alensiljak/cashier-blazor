using Cashier.Data;
using Cashier.Model;
using System.Reflection.Emit;
using System.Text;
using Tomlyn;
using Tomlyn.Model;
using Tomlyn.Syntax;

namespace Cashier.Lib
{
    /// <summary>
    /// All Asset Allocation functionality. Migrated from Cashier.
    /// </summary>
    public class AssetAllocation
    {
        public List<AssetClass> classes;

        private Dictionary<string, AssetAllocation> _assetClassIndex = [];

        public AssetAllocation(string toml)
        {
            loadFullAssetAllocation(toml);
        }

        public string GetCalculation()
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
        private void loadFullAssetAllocation(string toml)
        {
            // load definition
            //await LoadAssetAllocation();
            this.classes = ParseDefinition(toml);

            // build asset class index
            // _assetClassIndex = new Dictionary<string, AssetAllocation>();

            // build stock index
            // load current values
            // sum group balances
            // calculate offsets

        }

        /// <summary>
        /// Loads the Asset Allocation definition by parsing the TOML string.
        /// </summary>
        /// <param name="toml">Definition. Usually read from a file.</param>
        public List<AssetClass> ParseDefinition(string toml)
        {
            // parse TOML
            var model = Toml.ToModel(toml);
            var root = model.First();

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

        //private async Task LoadAssetAllocation()
        //{
        //    var svc = new OpfsService(_storage);
        //    var file = await svc.OpenFile(Constants.AssetAllocationFilename, false);
        //    // todo handle null

        //    Console.WriteLine("aa file size: {0}", await file.GetSizeAsync());
        //}

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

        //private AssetClass RecursivelyParse(KeyValuePair<string, object> pair)
        //{
        //    var ac = new AssetClass();
        //    ac.FullName = pair.Key;

        //    var table = (TomlTable)pair.Value;
        //    // now parse the subsections
        //    var sections = GetSections(table);
        //    foreach (var section in sections)
        //    {
        //        var child = RecursivelyParse(section);
        //        if (child != null)
        //        {
        //            // ac.
        //        }
        //    }

        //    return ac;
        //}

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
            foreach(var section in subsections)
            {
                var child = new AssetClass();
                if (!string.IsNullOrEmpty(nameSpace))
                {
                    child.FullName = nameSpace + ':' + section.Key;
                } else
                {
                    child.FullName = section.Key;
                }

                var childTable = (TomlTable)section.Value;
                child.Allocation = Convert.ToDecimal(childTable["allocation"]);
                if( childTable.ContainsKey("symbols"))
                {
                    var symbolsArray = (TomlArray) childTable["symbols"];
                    child.Symbols = symbolsArray.Select(item => (string)item!).ToList();
                }

                result.Add(child);

                // parse recursively
                var childNamespace = nameSpace;
                if(!string.IsNullOrEmpty(childNamespace))
                {
                    childNamespace += ':';
                }
                childNamespace += section.Key;

                var grandchildren = LinearizeTable(childTable, childNamespace);
                result.AddRange(grandchildren);
            }

            return result;
        }
    }
}
