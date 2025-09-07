using CoffeeShop.Data;
using CoffeeShop.Models.Interfaces;
using CoffeeShop.Models;

namespace CoffeeShop.Models.Services
{
    public class OrderRepository : IOrderRepository
    {
        private CoffeeShopDbContext dbContext;
        private IShoppingCartRepository shoppingCartRepository;
        public OrderRepository(CoffeeShopDbContext dbContext, IShoppingCartRepository shoppingCartRepository)
        {
            this.dbContext = dbContext;
            this.shoppingCartRepository = shoppingCartRepository;
        }
        public void PlaceOrder(Order order)
        {
            var shoppingCartItems = shoppingCartRepository.GetShoppingCartItems();
            order.OrderDetails = new List<OrderDetail>();
            foreach (var item in shoppingCartItems)
            {
                var orderDetail = new OrderDetail
                {
                    Quantity = item.Qty,
                    ProductId = item.Product.Id,
                    Price = item.Product.Price,
                    Product = item.Product, // Use the tracked instance
                    Order = order
                };
                order.OrderDetails.Add(orderDetail);
            }
            // Set order time to UTC
            order.OrderPlaced = DateTime.UtcNow;
            order.OrderTotal = shoppingCartRepository.GetShoppingCartTotal();
            dbContext.Orders.Add(order);
            dbContext.SaveChanges();    
        }
    }
}
