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
    public class CartProductRepository : ICartProductRepository
    {
        private readonly AppDbContext _context;

        public CartProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public void CreateCartProduct(CartProduct cartProduct)
        {
            _context.CartProducts.Add(cartProduct);
        }

        public CartProduct GetCartProductById(Guid id)
        {
            return _context.CartProducts.Include(obj => obj.Product).FirstOrDefault(obj => obj.Id == id);
        }

        public void RemoveCartProduct(CartProduct cartProduct)
        {
            _context.CartProducts.Remove(cartProduct);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
