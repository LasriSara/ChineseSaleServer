using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using serverProjectFinal.DTO;
using serverProjectFinal.Models;

namespace serverProjectFinal.DAL
{
    public class LotteryDal : ILotteryDal
    {
        private readonly PayingContext _PayingContext;
        private readonly ILogger<Lottery> _logger;


        public LotteryDal(PayingContext payingContext, ILogger<Lottery> logger)
        {
            _PayingContext = payingContext ?? throw new ArgumentNullException(nameof(PayingContext));
            _logger = logger;
          
        }

       

        public async Task<List<Lottery>> GetPresentsAsync()
        {
            try
            {
                return await _PayingContext.Lottery.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Logging from Donation , the exception" + ex.Message, 1);
                throw new Exception("Logging from Donation , the exception" + ex.Message);
            }
        }

        public async Task<Customer> LotteryUser(int productId)
        {
            // בדיקה האם המתנה כבר הוגרלה
            if (await _PayingContext.Lottery.FirstOrDefaultAsync(l => l.ProductId == productId) != null)
            {


                throw new Exception("מתנה זו כבר הוגרלה");

            }

            List<OrderItem> tmplist = new List<OrderItem>();

            var cartItemsListTmp = await _PayingContext.OrderItem.Where(c => c.ProductId == productId).ToListAsync();
            cartItemsListTmp.ForEach(cart =>
            {
                if (cart != null)
                {
                    for (int i = 0; i < cart.Quantity; i++) tmplist.Add(cart);
                }
            });

            // בדיקה האם יש פריטים להגרלה
            if (tmplist.Count != 0)
            {
                Random r = new Random();
                int index = r.Next(tmplist.Count);
                var cartId = tmplist[index].OrderId;

                // בדיקה נוספת לפני ההוספה לטבלת Lottery
                if (await _PayingContext.Lottery.FirstOrDefaultAsync(l => l.ProductId == productId) != null)
                {
                    return null;
                }

                var mycart = await _PayingContext.Order.FirstOrDefaultAsync(c => c.OrderId == cartId);
                var user = await _PayingContext.Customer.FirstOrDefaultAsync(c => c.CustomerId == mycart.CustomerId);
                await _PayingContext.Lottery.AddAsync(new Lottery() { ProductId = productId, CustomerId = user.CustomerId });
                await _PayingContext.SaveChangesAsync();
                return user;
            }

            return null;
        }






        public async Task<ActionResult<int>> GetAllMoneyDal()
        {

            var totalRevenue = await _PayingContext.OrderItem
           .Include(oi => oi.Product)
           .SumAsync(oi => oi.Quantity * oi.Product.Price);

            return (ActionResult<int>)totalRevenue;

        }


    }
}















 