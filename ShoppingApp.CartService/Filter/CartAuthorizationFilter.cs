using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShoppingApp.CartService.Repositories.Interfaces;
using ShoppingApp.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.CartService.Filter
{
    public class CartAuthorizationFilter : IActionFilter
    {
        public ICartRepository _cartRepository;

        public CartAuthorizationFilter(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string token = context.RouteData.Values["cartId"] as string;
            try
            {
                Guid cartId = Guid.Parse(token);
                Cart cart = _cartRepository.GetCartById(cartId);
                if (cart != null)
                {
                    context.HttpContext.Items.Add("cart", cart);
                    return;
                }
            }
            catch
            {
            }

            context.Result = new BadRequestResult();

        }
    }
}
