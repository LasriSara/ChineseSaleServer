using Microsoft.AspNetCore.Mvc;
using serverProjectFinal.DTO;
using serverProjectFinal.Models;

namespace serverProjectFinal.DAL
{
    public interface IOrderDal
    {
        Task<IEnumerable<OrderItem>> GetGiftCardsAsync(string giftName);
        Task<IEnumerable<Product>> GetSortGiftsAsync(bool? price, bool? maxQuentity);
        Task<string> GetPurchesesDetailsAsync();
        Task<List<Product>> add(int giftId,int q, int customerId);

        Task<List<Product>> UpdateOrderaDl(int customerId, int giftId);

        Task<List<Order>> PayForOrderDal(int userId, List<int> order);

        Task<List<Product>> GetCartAsync(int userId);

 


    }
}
