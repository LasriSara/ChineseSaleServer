using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using serverProjectFinal.DTO;
using serverProjectFinal.Models;

namespace serverProjectFinal.DAL
{
    public class OrderDal : IOrderDal
    {
        private readonly PayingContext _payingContext;
        private readonly IProductDal _productDal;
        private readonly ILogger<Order> _logger;
        private readonly IMapper _mapper;


        public OrderDal(PayingContext hsContext, IProductDal productDal, IMapper mapper)
        {
            _payingContext = hsContext ?? throw new ArgumentNullException(nameof(hsContext));
            _productDal = productDal ?? throw new ArgumentNullException(nameof(productDal));
            _mapper = mapper;
        }
        //רכישת כרטיסים עבור כל מתנה
        public async Task<IEnumerable<OrderItem>> GetGiftCardsAsync(string giftName)
        {
            try
            {
                var giftId = _productDal.SearchGiftsDal(giftName, null, null).Result.First().ProductId;
                return await _payingContext.OrderItem.Where(o => o.ProductId == giftId).ToListAsync();
            }
          
                 catch (Exception ex)
            {
                _logger.LogError("Logging from GetGiftCardsAsync, the exception: " + ex.Message, 1);
                throw new Exception("Logging from GetGiftCardsAsync, the exception: " + ex.Message);
            }
        
        }

        public async Task<IEnumerable<Product>> GetSortGiftsAsync(bool? price, bool? maxQuentity)
        {
            try
            {
                if (price.HasValue)
                {
                    return await _payingContext.Products.OrderBy(g => g.Price).ToListAsync();
                }
                else if (maxQuentity.HasValue)
                {
                    return await _payingContext.Products
                        .OrderByDescending(product => _payingContext.OrderItem.Count(orderItem => orderItem.ProductId == product.ProductId))
                        .ToListAsync();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Logging from GetSortGiftsAsync, the exception: " + ex.Message, 1);
                throw new Exception("Logging from GetSortGiftsAsync, the exception: " + ex.Message);
            }
        }
        //מביא את כל מי שרכש
        public async Task<string> GetPurchesesDetailsAsync()
        {
            try
            {
                var customersWithOrders = await _payingContext.Customer
                .Where(p => p.Order.Count > 0)
                .Select(p => $"{p.FirstName} {p.LastName}")
                .ToListAsync();
                //משלבת למחרוזת אחת
                return string.Join(", ", customersWithOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError("Logging from GetPurchesesDetailsAsync, the exception: " + ex.Message, 1);
                throw new Exception("Logging from GetPurchesesDetailsAsync, the exception: " + ex.Message);
            }

        }




        //הוספה לסל

        public async Task<List<Product>> add(int giftId, int q, int customerId)
        {
            try
            {
                //בדיקה אם קיים כבר כזה משתמש
                var existingOrder = await _payingContext.Order
                    .Include(o => o.OrderItemList)
                    .ThenInclude(oi => oi.Product)
                    .Where(o => o.CustomerId == customerId && o.status == "chart")
                    .FirstOrDefaultAsync();

                if (existingOrder != null)
                {
                    // אם יש הזמנה פתוחה אז תוסיף לשם את המוצר
                    var orderItem = existingOrder.OrderItemList.FirstOrDefault(oi => oi.ProductId == giftId);
                    if (orderItem != null)
                    {
                        // If the item already exists in the order, update its quantity
                        orderItem.Quantity += q;
                    }
                    else
                    {
                        // If the item is not in the order, add it
                        orderItem = new OrderItem
                        {
                            ProductId = giftId,
                            Quantity = q,
                            OrderId = existingOrder.OrderId
                        };

                        existingOrder.OrderItemList.Add(orderItem);
                    }
                }
                else
                {
                    // If no open order exists, create a new one
                    var newOrder = new Order
                    {
                        CustomerId = customerId,
                        OrderDate = DateTime.Now,
                        status = "chart",
                        OrderItemList = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId = giftId,
                        Quantity = q
                    }
                }
                    };

                    await _payingContext.Order.AddAsync(newOrder);
                }

                // Save changes to the database
                await _payingContext.SaveChangesAsync();

                // Retrieve products in the cart
                var productsInCart = await _payingContext.Order
                    .Where(o => existingOrder != null && o.OrderItemList.Any(oi => oi.OrderId == existingOrder.OrderId))
                    .SelectMany(o => o.OrderItemList)
                    .Select(oi => oi.Product)
                    .ToListAsync();

                Console.WriteLine($"Customer ID received in 'AddToCart' function: {customerId}");

                // Return the list of products in the cart
                return productsInCart;
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                Console.WriteLine($"An error occurred while saving changes: {innerException.Message}");
                throw;
            }
        }


        
        

        //מחיקה
        public async Task<List<Product>> UpdateOrderaDl(int customerId, int giftId)
        {
            List<Product> userPresents = new List<Product>();

            var toDelete = await _payingContext.OrderItem
                .Where(oi => oi.ProductId == giftId && oi.Order.CustomerId == customerId && oi.Order.status != "שולם")
                .ToListAsync();

            foreach (var item in toDelete)
            {
                _payingContext.OrderItem.Remove(item);
            }

            await _payingContext.SaveChangesAsync();

            var userOrders = await _payingContext.Order
                .Where(o => o.CustomerId == customerId)
                .ToListAsync();

            foreach (var ord in userOrders)
            {
                var p = await _payingContext.Products
                    .Where(o => o.ProductId == o.ProductId && ord.status == "עגלה")
                    .ToListAsync();

                userPresents.AddRange(p);
            }

            Console.WriteLine($"Product {giftId} deleted successfully from the cart.");

            return userPresents;
        }


        public async Task<List<Product>> GetCartAsync(int userId)
        {
            try
            {
                //Check if an open order already exists
                var order = await _payingContext.Order
                    .FirstOrDefaultAsync(o => o.CustomerId == userId);

                if (order == null)
                {
                    Console.WriteLine("Order not found for user with Id: " + userId);
                    return null; // Handle the case where no order is found
                }

                //Retrieve order items with product information and quantity
                var orderItems = await _payingContext.OrderItem
                    .Where(oi => oi.OrderId == order.OrderId)
                    .Include(oi => oi.Product)
                    .ToListAsync();

                if (orderItems == null || orderItems.Count == 0)
                {
                    Console.WriteLine("No order items found for order Id: " + order.OrderId);
                    return new List<Product>();
                }

                //Extract products from order items with quantity
               var orderProducts = orderItems.Select(oi => new Product
               {
                   ProductId = oi.Product.ProductId,
                   Name = oi.Product.Name,
                   Donor = oi.Product.Donor,
                   DonorId = oi.Product.DonorId,
                   Price = oi.Product.Price,
                   CategoryId = oi.Product.CategoryId,
                   Category = oi.Product.Category,
                   UrlImage = oi.Product.UrlImage,
                   Quantity = oi.Quantity // Include quantity in the Product model
               }).ToList();

                //Return the list of unique products
                return orderProducts.Distinct().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetCartAsync: " + ex.Message);
                return null;
            }
        }




        public async Task<List<Order>> PayForOrderDal(int userId, List<int> orderIds)
        {
            // שאילתה חד פעמית למשתמש וההזמנות שלו
            var userOrders = await _payingContext.Order
                .Include(o => o.OrderItemList)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.CustomerId == userId)
                .ToListAsync();

            foreach (var orderId in orderIds)
            {
                // מצא את ההזמנה המתאימה מתוך userOrders
                var order = userOrders.FirstOrDefault(o => o.OrderId == orderId);

                if (order != null)
                {
                    // עדכון סטטוס ותאריך בהזמנה
                    order.OrderDate = DateTime.Now;
                    order.status = "שולם";

                    // עדכון סטטוס בפריטי ההזמנה

                }
            }

            await _payingContext.SaveChangesAsync();
            return userOrders;
        }

  
    }
}



