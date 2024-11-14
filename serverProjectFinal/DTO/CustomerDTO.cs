using System.ComponentModel.DataAnnotations;

namespace serverProjectFinal.DTO
{
    public class CustomerDTO
    {
        [Key]
        public int CustomerId { get; set; }
        public string PassWord { get; set; }
        [Required(ErrorMessage = "FirstName is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        public string? Roles { get; set; }


    }
}
