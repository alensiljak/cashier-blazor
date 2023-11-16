using Tomlyn;
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
            // todo: parse TOML
            var model = Toml.Parse( toml );
            //Console.WriteLine(model);

            Console.WriteLine("kind: ", model.Kind);
            Console.WriteLine("child count: {0}", model.ChildrenCount);
            //for (int i = 0; i < model.ChildrenCount; i++)
            //{
            //    Console.WriteLine("child: {0}", model.GetChild(i));
            //}
            //Console.WriteLine(model.Descendants());
            foreach (var descendant in model.Descendants())
            {
                Console.WriteLine("d: {0}", descendant);
            }
        }
    }
}
