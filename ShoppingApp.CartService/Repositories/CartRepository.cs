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
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Cart> GetAllCarts()
        {
            return _context.Carts.ToList();
        }

        public void CreateCart(Cart cart)
        {
            _context.Carts.Add(cart);
        }

        public Cart GetCartById(Guid id)
        {
            return _context.Carts.Include(cart => cart.CartProducts ).FirstOrDefault(obj => obj.Id == id);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
