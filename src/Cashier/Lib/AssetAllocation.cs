using MudBlazor.Utilities;
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
        /// <summary>
        /// Loads the Asset Allocation definition by parsing the TOML string.
        /// </summary>
        /// <param name="toml">Definition. Usually read from a file.</param>
        public void ParseDefinition(string toml)
        {
            // parse TOML
            var model = Toml.ToModel( toml );
            var root = model.First();

            RecursivelyDisplay(root, 0);
        }

        private IEnumerable<KeyValuePair<string, object>> GetSections(TomlTable item)
        {
            var subsections = item.Where(pair => pair.Value is TomlTable)
                .Select(pair => pair);
            return subsections;
        }

        private void RecursivelyDisplay(KeyValuePair<string, object> pair, int level)
        {
            // The indentation is 2 spaces per level.
            var indent = new string(' ', level*2);

            TomlTable table = (TomlTable) pair.Value;
            Console.WriteLine($"{indent}{pair.Key}");

            // now display the subsections
            var sections = GetSections(table);
            foreach( var section in sections )
            {
                RecursivelyDisplay(section, level+1);
            }
        }
    }
}
