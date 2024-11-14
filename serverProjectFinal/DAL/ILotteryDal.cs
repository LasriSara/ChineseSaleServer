using Microsoft.AspNetCore.Mvc;
using serverProjectFinal.Models;

namespace serverProjectFinal.DAL
{
    public interface ILotteryDal
    {
        Task<List<Lottery>> GetPresentsAsync();
        Task<Customer> LotteryUser(int productId);

        Task<ActionResult<int>> GetAllMoneyDal();


    }
}
