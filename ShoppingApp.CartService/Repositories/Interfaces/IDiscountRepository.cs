using ShoppingApp.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.CartService.Repositories.Interfaces
{
    public interface IDiscountRepository
    {
        IEnumerable<Discount> GetDiscountsByProductId(Guid productId);
        Discount GetDiscountsById(Guid discountId);
    }
}
