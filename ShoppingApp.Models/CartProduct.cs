using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.DataModel
{
    public class CartProduct
    {
        [Key]
        public Guid Id { get; set; }
                
        public Guid DiscountId { get; set; }
        public virtual Discount Discount { get; set; }

        public Guid CartId { get; set; }
        public virtual Cart Cart { get; set; }

        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
