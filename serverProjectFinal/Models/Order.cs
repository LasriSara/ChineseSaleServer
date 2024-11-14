using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace serverProjectFinal.Models
{
    public class Order
    {
     

        [Key]

        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        [ForeignKey("Customer")]

        public int CustomerId { get; set; }
        public string status { get; set; }

        public  Customer Customer { get; set; }
        public  ICollection<OrderItem> OrderItemList { get; set; }


    }



    
}