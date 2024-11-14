using serverProjectFinal.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace serverProjectFinal.DTO
{
    public class ProductDTO
    {
        [Key]
        [NotNull]
        public int ProductId { get; set; }
        public string Name { get; set; }
        [NotNull]
        public int DonorId { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; }
        [NotNull]

        public int CategoryId { get; set; }
        
        public string UrlImage { get; set; }
    }
}
