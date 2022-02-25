using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.DataModel
{
    public class Discount
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        public double Percentage { get; set; }

        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }

        public List<CartProduct> CartProducts { get; set; }
    }
}
