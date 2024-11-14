//using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace serverProjectFinal.Models
{
    public class Product
    {
        [Key]

        public int ProductId { get; set; } 
        public string Name { get; set; }
       [JsonIgnore]
        public virtual Donor Donor { get; set; }
        [NotNull]

        public int DonorId { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; } = 10;

        [ForeignKey("Category")]

        public int CategoryId { get; set; }
        [NotNull]
        public int Quantity { get; set; }

        public virtual Category Category { get; set; }

        public string UrlImage { get; set; } 
    }
}
