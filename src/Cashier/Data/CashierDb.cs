using Cashier.Data;
using DexieNET;

namespace Cashier.Data
{
    public interface ICashierDB : IDBStore { };
}

namespace Cashier.Domain
{
    [Schema(StoreName = "assetAllocation")]
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
        [property: Index(IsPrimary = true)] string Key,
        string Value
    ) : ICashierDB;

    public partial record Posting(
        string Account,
        Money? Money
    );

    [Schema(PrimaryKeyGuid = false)]
    public partial record Xact
    (
        [property: Index] string? Date,
        string? Payee,
        string? Note,
        Posting[]? postings
    ) : ICashierDB;

    public partial record LastXact(
        [property: Index] string Payee,
        Xact? Xact
    ) : ICashierDB;
}