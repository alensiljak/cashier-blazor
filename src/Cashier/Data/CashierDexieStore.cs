using DexieNET;

namespace Cashier.Data
{
    public interface ICashierDB : IDBStore { };

    [Schema(PrimaryKeyGuid = false)]
    public partial record Xact
        (
        //[property: Index(IsAuto = true, IsPrimary = true, IsUnique = true)] long? Id,
        string? Date,
        [property: Index] string? Payee
        ) : ICashierDB;

    public partial record Account
        (
        [property: Index(IsPrimary = true)] string Name
        ) : ICashierDB;
}
