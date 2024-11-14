using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace serverProjectFinal.Models
{
    public enum Type
    {
        Gift,
        Money
    }
    public class Donor
    {
        public int DonorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public Type MyType { get; set; }
        [JsonIgnore]

        public ICollection<Product> Product { get; set; }
        
    }
}
