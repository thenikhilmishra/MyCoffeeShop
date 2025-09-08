using CoffeeShop.Models;
using CoffeeShop.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace CoffeeShop.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private IOrderRepository orderRepository;
        private IShoppingCartRepository shopCartRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrdersController(IOrderRepository orderRepository, IShoppingCartRepository shopCartRepository, UserManager<ApplicationUser> userManager)
        {
            this.orderRepository = orderRepository;
            this.shopCartRepository = shopCartRepository;
            _userManager = userManager;
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

            // Use Identity GUID for UserId
            var userId = _userManager.GetUserId(User);
            order.UserId = userId;
            // Always set order email to logged-in user's email
            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                order.Email = user.Email.ToLowerInvariant();
            }
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
