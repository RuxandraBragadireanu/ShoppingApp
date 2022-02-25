
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.CartService.Data;
using ShoppingApp.CartService.DTO;
using ShoppingApp.CartService.Filter;
using ShoppingApp.CartService.Repositories.Interfaces;
using ShoppingApp.CartService.Services.Interfaces;
using ShoppingApp.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.CartService.Services
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartService : ControllerBase, ICartService
    {
        private readonly ICartProductRepository _cartProductRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IDiscountRepository _discountRepository;

        public CartService(ICartRepository cartRepository, ICartProductRepository cartProductRepository, IDiscountRepository discountRepository)
        {
            _cartProductRepository = cartProductRepository;
            _cartRepository = cartRepository;
            _discountRepository = discountRepository;
        }

        [HttpGet]
        public ActionResult<List<Cart>> GetAll(Cart cart)
        {
            return _cartRepository.GetAllCarts().ToList();
        }

        [HttpGet("{cartId}")]
        [ServiceFilter(typeof(CartAuthorizationFilter))]
        public ActionResult<Cart> Get()
        {
            Cart cart = this.HttpContext.Items["cart"] as Cart;
            return cart;
        }

        [HttpPost("{cartId}")]
        [ServiceFilter(typeof(CartAuthorizationFilter))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CartProductReadDTO> Add([FromBody] CartProductCreateDTO item)
        {
            Cart cart = this.HttpContext.Items["cart"] as Cart;

            //check discount available for current item
            IEnumerable<Discount> discountList = _discountRepository.GetDiscountsByProductId(item.ProductId);
            Discount selectedDiscount = null;

            if (discountList.Any())
            {
                //select discound based on existing items quantity
                var existingItems = cart.CartProducts.Where(obj => obj.ProductId == item.ProductId && obj.DiscountId == Guid.Empty);
                selectedDiscount = discountList.FirstOrDefault(obj => obj.Quantity == (existingItems.Count() + 1));
                   
                //if discount is applicable set it to the existing items
                if (selectedDiscount != null)
                {
                    foreach (CartProduct existingItem in existingItems)
                    {
                        existingItem.DiscountId = selectedDiscount.Id;
                    }
                }                
                              
            }

            //create new item
            CartProduct cartProduct = new CartProduct() { ProductId = item.ProductId, CartId = cart.Id };            
            //if discount exists set it
            if(selectedDiscount != null)
            {
                cartProduct.DiscountId = selectedDiscount.Id;
            }

            _cartProductRepository.CreateCartProduct(cartProduct);
            
            if(_cartProductRepository.SaveChanges())
            {
                CartProductReadDTO cartProductRead = new CartProductReadDTO() { CartProductId = cartProduct.Id };
                return cartProductRead;
            }

            return new BadRequestResult();
        }

        [HttpGet("total/{cartId}")]
        [ServiceFilter(typeof(CartAuthorizationFilter))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<double> GetTotal()
        {
            Cart cart = this.HttpContext.Items["cart"] as Cart;
            double total = 0;

            foreach(var cartProductsGroup in cart.CartProducts.GroupBy(obj => obj.DiscountId)) 
            {
                double groupTotal = 0;
                foreach(CartProduct cartProduct in cartProductsGroup)
                {
                    CartProduct completeCartProduct = _cartProductRepository.GetCartProductById(cartProduct.Id);
                    groupTotal += cartProduct.Product.Price;

                }

                total += groupTotal;

                if (cartProductsGroup.Key == Guid.Empty)
                {
                    continue;
                }

                Discount discount = _discountRepository.GetDiscountsById(cartProductsGroup.Key);
                if(discount != null)
                {
                    double productPrice = discount.Product.Price;
                    total -= productPrice * discount.Quantity * discount.Percentage * (cartProductsGroup.Count() / discount.Quantity);
                }

            }

            return total;
        }

        [HttpDelete("{cartId}")]
        [ServiceFilter(typeof(CartAuthorizationFilter))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Remove([FromBody] CartProductReadDTO item)
        {
            CartProduct cartProduct = _cartProductRepository.GetCartProductById(item.CartProductId);
            if(cartProduct == null)
            {
                return new BadRequestResult();
            }

            Cart cart = this.HttpContext.Items["cart"] as Cart;

            if (cartProduct.DiscountId != Guid.Empty)
            {
                Discount discount = _discountRepository.GetDiscountsById(cartProduct.DiscountId);
                List<CartProduct> existingCartProducts = cart.CartProducts.Where(obj => obj != cartProduct && obj.DiscountId == cartProduct.DiscountId).ToList();

                for(int i = 0; i < discount.Quantity - 1; i++)
                {
                    existingCartProducts[i].DiscountId = Guid.Empty;
                }                
            }

            _cartProductRepository.RemoveCartProduct(cartProduct);
          
            if (_cartProductRepository.SaveChanges())
            {
                return new OkResult();
            }

            return new BadRequestResult();
        }
    }
}
