namespace Cashier.Data
{
    /// <summary>
    /// Used to set the selection mode (Payee, Account, etc.) when editing a Transaction.
    /// Contains all the values required for the selection mode to function.
    /// When an object of this type exists in the state store, we are in the Selection mode. The pages
    /// like Accounts and Payees act like selectors and not pages with lists. Tapping an item will select 
    /// it and return to the previous page.
    /// </summary>
    public class SelectionModeMetadata
    {
        /// <summary>
        /// The selection requestor. Can be used to explicitly name the origin and
        /// avoid confusion in unexpected navigation routes.
        /// </summary>
        public string Origin { get; set; } = string.Empty;
        public int? PostingIndex { get; set; }
        /// <summary>
        /// The type of item being selected. Useful on return to the original entity.
        /// </summary>
        public SelectionType? SelectionType { get; set; }

        /// <summary>
        /// The id of the selected item.
        /// </summary>
        public string? SelectedId { get;set; }

        public SelectionModeMetadata() { }

        //public override string ToString()
        //{
        //    // return string.Format($"");
        //}
    }
}
