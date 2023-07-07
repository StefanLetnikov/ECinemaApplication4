using ECinema.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECinema.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        public IActionResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Tickets");
            }

            return View(_shoppingCartService.GetShoppingCartInfo(userId));
        }


        public IActionResult DeleteFromShoppingCart(Guid id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = _shoppingCartService.DeleteTicketFromShoppingCart(userId, id);

            if (result)
            {
                return RedirectToAction("Index", "ShoppingCart");
            }

            return RedirectToAction("Index", "ShoppingCart");
        }

        public async Task<IActionResult> Order(Guid? id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _shoppingCartService.OrderNowAsync(userId);

            if (result)
            {
                return RedirectToAction("Index", "ShoppingCart");
            }

            return RedirectToAction("Index", "ShoppingCart");
        }
    }
}