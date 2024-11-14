using System.ComponentModel.DataAnnotations;

namespace serverProjectFinal.Models
{
    public class Lottery
    {
       
        public int LotteryId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public Customer Customer { get; set; }
        public Product Product { get; set; }
    }
}
