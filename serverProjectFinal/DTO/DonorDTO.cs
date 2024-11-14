using System.ComponentModel.DataAnnotations;

namespace serverProjectFinal.DTO
{
    public enum Type
    {
        Gift,
        Money
    }
    public class DonorDTO
    {
        public int DonorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public Type MyType { get; set; }
    }
}
