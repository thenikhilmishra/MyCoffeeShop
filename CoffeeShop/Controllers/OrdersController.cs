using CoffeeShop.Models;
using CoffeeShop.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private IOrderRepository orderRepository;
        private IShoppingCartRepository shopCartRepository;
        public OrdersController(IOrderRepository orderRepository, IShoppingCartRepository shopCartRepository)
        {
            this.orderRepository = orderRepository;
            this.shopCartRepository = shopCartRepository;
        }

        public IActionResult Checkout()
        {
            // Fetch cart items and pass to the view for summary display
            var cartItems = shopCartRepository.GetShoppingCartItems();
            ViewBag.CartItems = cartItems;
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            // Fetch cart items for order summary in case of validation error
            var cartItems = shopCartRepository.GetShoppingCartItems();
            ViewBag.CartItems = cartItems;

            if (!ModelState.IsValid || cartItems == null || !cartItems.Any())
            {
                ModelState.AddModelError("", "Please fill all required fields and ensure your cart is not empty.");
                return View(order);
            }

            order.UserId = User.Identity.Name; // Or use User.FindFirst(ClaimTypes.NameIdentifier)?.Value if using Identity
            orderRepository.PlaceOrder(order);
            shopCartRepository.ClearCart();
            HttpContext.Session.SetInt32("CartCount", 0);
            return RedirectToAction("CheckoutComplete");
        }
        
        public IActionResult CheckoutComplete()
        {
            return View();
        }
    }
}
