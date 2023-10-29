using Cashier.Data;
using DexieNET;

namespace Cashier.Data
{
    public interface ICashierDB : IDBStore { };
}

namespace Cashier.Domain
{
    public partial record AssetAllocation(
        [property: Index(IsPrimary = true)] string FullName
    ) : ICashierDB;

    public partial record Account
    (
        [property: Index(IsPrimary = true)] string Name
    ) : ICashierDB;

    public partial record Payee
    (
        [property: Index(IsPrimary = true)] string Name
    ) : ICashierDB;

    [Schema(PrimaryKeyGuid = false)]
    public partial record ScheduledXact(
        [property: Index] string NextDate
    ) : ICashierDB;

    public partial record Setting(
        [property: Index(IsPrimary = true)] string Key
    ) : ICashierDB;

    [Schema(PrimaryKeyGuid = false)]
    public partial record Xact
    (
        //[property: Index(IsAuto = true, IsPrimary = true, IsUnique = true)] long? Id,
        [property: Index] string? Date,
        string? Payee
    ) : ICashierDB;

    public partial record LastXact(
        [property: Index] string Payee
    ) : ICashierDB;
}