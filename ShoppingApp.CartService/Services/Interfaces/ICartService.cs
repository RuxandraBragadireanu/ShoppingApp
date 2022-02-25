using Microsoft.AspNetCore.Mvc;
using ShoppingApp.CartService.DTO;
using ShoppingApp.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.CartService.Services.Interfaces
{
    public interface ICartService
    {
        ActionResult<CartProductReadDTO> Add(CartProductCreateDTO item);
        ActionResult<double> GetTotal();
        ActionResult Remove(CartProductReadDTO item);
    }
}
