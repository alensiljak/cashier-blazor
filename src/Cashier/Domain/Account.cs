using System.ComponentModel.DataAnnotations;

namespace Cashier.Domain
{
    public class Account
    {
        //public int Id { get;set; }

        [Key]
        public string Name { get; set; } = string.Empty;
    }
}
