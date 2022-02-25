using ShoppingApp.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.CartService.Repositories.Interfaces
{
    public interface ICartRepository
    {
        IEnumerable<Cart> GetAllCarts();
        Cart GetCartById(Guid id);
        void CreateCart(Cart cart);

        bool SaveChanges();
    }
}
