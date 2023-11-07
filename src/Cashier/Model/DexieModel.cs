/*
 */
namespace Cashier.Model
{
    public partial record SettingRecord(
        string Key,
        string Value
    );

    public class Setting
    {
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
