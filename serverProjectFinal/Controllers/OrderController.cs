using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using serverProjectFinal.BLL;
using serverProjectFinal.DAL;
using serverProjectFinal.DTO;
using serverProjectFinal.Models;

namespace serverProjectFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {

         

        private readonly IOrderService _order;
        private readonly IOrderDal _orderDal;

        
        public OrderController(IOrderService order,IOrderDal orderDal)
        {
            _order = order ?? throw new ArgumentNullException(nameof(order));
            _orderDal = orderDal ?? throw new ArgumentNullException(nameof(orderDal));

        }
     
        [HttpGet("{giftName}")]
        //[Authorize(Roles = "Saller")]

        public async Task<IEnumerable<OrderItem>> Get(string giftName)
        {
            var middlewareUser = HttpContext.Items["User"] as Customer;
            if (middlewareUser == null)
            {
                return null;
            }
            return await _order.GetGiftCards(giftName);
        }



        [HttpGet]
        public async Task<IEnumerable<Product>> Get(bool? price, bool? maxQuentity)
        {
            return await _order.GetSortGifts(price, maxQuentity);
        }
        [HttpGet("purchases")]
        public async Task<string> Get()
        {
            return await _order.GetPurchesesDetails();
        }


        [HttpPost]
        public async Task<ActionResult<List<Product>>> add(int giftId, int q)
        {
            Customer middlewareUser = HttpContext.Items["User"] as Customer;

            if (middlewareUser == null)
            {
                Console.WriteLine("User is not authenticated.");
                return Unauthorized();
            }

            Console.WriteLine($"Customer ID from HttpContext: {middlewareUser.CustomerId }");
            Console.WriteLine($"Customer Email from HttpContext: {middlewareUser.Email}");

            var productsInCart = await _orderDal.add(giftId, q, middlewareUser.CustomerId/* middlewareUser.CustomerId+1*/);

            if (productsInCart == null || productsInCart.Count == 0)
            {
                return Ok(new List<Product>());
            }

            return Ok(productsInCart);
        }


        [HttpPut("deleteFromCartById")]
        public async Task<ActionResult<List<Product>>> DeleteFromCart( int productId)
        {
            Customer middlewareUser = ControllerContext.HttpContext.Items["User"] as Customer;
            if (middlewareUser == null)
            {
                Console.WriteLine("User is not authenticated.");
                return Unauthorized();
            }
            return await _order.UpdateOrder(middlewareUser.CustomerId, productId);
        }


        [HttpPut("makePayment")]

        public async Task<List<Order>> MakePayment( List<int> order)
        {
            Customer middlewareUser = ControllerContext.HttpContext.Items["User"] as Customer;
            return await _order.PayForOrderDal(middlewareUser.CustomerId, order);
        }


        [HttpGet]
        [Route("GetCart")]
        public async Task<ActionResult<List<Product>>> GetCart()
        {
            Customer user = (Customer)HttpContext.Request.HttpContext.Items["User"];

            try
            {
                var cart = await _order.GetCart(user.CustomerId);

                if (cart == null || cart.Count == 0)
                {
                    return NotFound("Cart is empty");
                }

                return Ok(cart);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }




        
    }
}
