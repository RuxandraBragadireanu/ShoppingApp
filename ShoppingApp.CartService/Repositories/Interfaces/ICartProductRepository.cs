using ShoppingApp.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.CartService.Repositories.Interfaces
{
    public interface ICartProductRepository
    {
        CartProduct GetCartProductById(Guid id);
        void CreateCartProduct(CartProduct cartProduct);
        void RemoveCartProduct(CartProduct cartProduct);

        bool SaveChanges();
    }
}
