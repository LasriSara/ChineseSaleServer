using serverProjectFinal.DTO;
using serverProjectFinal.Models;

namespace serverProjectFinal.BLL
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderItem>> GetGiftCards(string giftName);
        Task<IEnumerable<Product>> GetSortGifts(bool? price, bool? maxQuentity);
        Task<string> GetPurchesesDetails();
        public Task<List<Product>> UpdateOrder(int customerId, int giftId);
        public Task<List<Order>> PayForOrderDal(int userId, List<int> orderIds);
        Task<List<Product>> GetCart(int userId);

        Task<List<Product>> add(int giftId, int q,int customerId);
       

    }
}
