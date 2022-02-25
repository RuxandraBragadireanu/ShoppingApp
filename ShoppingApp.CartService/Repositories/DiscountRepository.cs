using Microsoft.EntityFrameworkCore;
using ShoppingApp.CartService.Data;
using ShoppingApp.CartService.Repositories.Interfaces;
using ShoppingApp.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.CartService.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly AppDbContext _context;

        public DiscountRepository(AppDbContext context)
        {
            _context = context;
        }

        public Discount GetDiscountsById(Guid discountId)
        {
            return _context.Discounts.Include(obj => obj.Product).FirstOrDefault(obj => obj.Id == discountId);
        }

        public IEnumerable<Discount> GetDiscountsByProductId(Guid productId)
        {
            return _context.Discounts.Where(obj => obj.ProductId == productId);
        }
    }
}
