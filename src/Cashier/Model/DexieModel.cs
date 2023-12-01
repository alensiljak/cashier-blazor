/*
 * Domain / Data model
 */
namespace Cashier.Model
{
    //public class AssetAllocation
    //{
    //    public string? FullName { get; set; }
    //}

    public class Payee
    {
        public string? Name { get; set; }
    }

    public class Setting(string key, string value)
    {
        public string Key { get; set; } = key;
        public string Value { get; set; } = value;
    }
}