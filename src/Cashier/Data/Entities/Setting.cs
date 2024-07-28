namespace Cashier.Data.Entities
{
    public class Setting(string key, string value)
    {
        public string Key { get; set; } = key;
        public string Value { get; set; } = value;
    }
}
