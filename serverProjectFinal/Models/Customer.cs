using System.ComponentModel.DataAnnotations;
namespace serverProjectFinal.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string PassWord { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public bool IsManager { get; set; }
        public ICollection<Order> Order { get; set; }
        public string? Roles { get; set; }

    }
}