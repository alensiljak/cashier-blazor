using BlazorDexie.Database;
using BlazorDexie.JsModule;
using Cashier.Model;

namespace Cashier.DAL
{
    public class DexieDAL : Db
    {
        public Store<Setting, string> Settings { get; set; } = new(nameof(Setting.Key), nameof(Setting.Value));

        public DexieDAL(IModuleFactory moduleFactory)
            : base("Cashier", 1, new DbVersion[] {}, moduleFactory)
        { }
    }
}
