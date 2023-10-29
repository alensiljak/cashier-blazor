using Cashier.Data;
using DexieNET;

namespace Cashier.Data
{
    public interface ICashierDB : IDBStore { };
}

namespace Cashier.Domain
{
    public partial record AssetAllocation(
        string FullName
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

    ) : ICashierDB;

    [Schema(PrimaryKeyGuid = false)]
    public partial record Xact
    (
        //[property: Index(IsAuto = true, IsPrimary = true, IsUnique = true)] long? Id,
        string? Date,
        [property: Index] string? Payee
    ) : ICashierDB;

    public partial record LastXact(
        string Payee
    ) : ICashierDB;
}