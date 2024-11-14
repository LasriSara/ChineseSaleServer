using Microsoft.AspNetCore.Mvc;
using serverProjectFinal.Models;

namespace serverProjectFinal.BLL
{
    public interface ILotteryService
    {
        Task<List<Lottery>> GetPresentsAsync();
        Task<Customer> LotteryUser(int productId);
        Task<ActionResult<int>> GetAllMoneyBll();

        

    }
}
