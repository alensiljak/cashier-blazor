namespace Cashier.Model
{
    public class AssetClass
    {
        public string FullName { get; set; } = string.Empty;
        public decimal Allocation { get; set; }
        public decimal AllocatedValue { get; set; }
        public decimal CurrentAllocation { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal Diff { get; set; }
        public decimal DiffAmount { get; set; }
        public decimal DiffPerc { get; set; }
        public string Currency { get; set; } = string.Empty;
        public List<string> Symbols { get; set; } = new List<string>();

        /// <summary>
        /// Represents the class depth in the allocation tree.
        /// The root element (Allocation) is 0. This is effectively the number of parents.
        /// </summary>
        public int Depth
        {
            get
            {
                if (this.FullName == string.Empty)
                {
                    return 0;
                }

                var parts = this.FullName.Split(':');
                return parts.Length;
            }
        }

        public string Name
        {
            get
            {
                var parts = this.FullName.Split(':');
                var lastIndex = parts.Length - 1;
                var name = parts[lastIndex];
                return name;
            }
        }

        public string ParentName
        {
            get
            {
                var parts = this.FullName.Split(':');
                var lastIndex = parts.Length - 1;
                var parentNameParts = parts.Take(lastIndex).ToArray();
                var result = string.Join(':', parentNameParts);
                return result;
            }
        }
    }
}
