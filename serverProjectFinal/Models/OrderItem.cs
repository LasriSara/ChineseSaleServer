﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace serverProjectFinal.Models
{
    public class OrderItem
    {

        [Key]

        public int OrderItemId { get; set; }
        [ForeignKey("Order")]

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        [ForeignKey("Product")]

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int Quantity { get; set; }
      
    }
}
