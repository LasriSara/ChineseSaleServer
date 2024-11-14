using serverProjectFinal.DAL;
using serverProjectFinal.DTO;
using serverProjectFinal.Models;

namespace serverProjectFinal.BLL
{
    public class OrderService : IOrderService
    {
        private readonly IOrderDal _orderDal;

        public OrderService(IOrderDal orderDal)
        {
            _orderDal = orderDal ?? throw new ArgumentNullException(nameof(orderDal));
        }

        public async Task<IEnumerable<OrderItem>> GetGiftCards(string giftName)
        {
            return await _orderDal.GetGiftCardsAsync(giftName);
        }

   

        public async Task<IEnumerable<Product>> GetSortGifts(bool? price, bool? maxQuentity)
        {
            return await _orderDal.GetSortGiftsAsync(price, maxQuentity);
        }
        public async Task<string> GetPurchesesDetails()
        {
            return await _orderDal.GetPurchesesDetailsAsync();
        }

        public async Task<List<Product>> add(int giftId,int q, int customerId)
        {

            return await _orderDal.add(giftId, q,customerId);
        }


        public async Task<List<Product>> UpdateOrder(int customerId, int giftId)
        {
            return await _orderDal.UpdateOrderaDl(customerId, giftId);
        }

    
        public async Task<List<Order>> PayForOrderDal(int userId, List<int> orderIds)
        {
            return await _orderDal.PayForOrderDal(userId, orderIds); 
        }


        public async Task<List<Product>> GetCart(int userId)
        {
            return await _orderDal.GetCartAsync(userId);
        }

      
    }
}
