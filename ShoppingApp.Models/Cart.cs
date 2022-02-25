using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.DataModel
{
    public class Cart
    {
        [Key]
        public Guid Id { get; set; }

        public List<CartProduct> CartProducts { get; set; } 

    }
}
