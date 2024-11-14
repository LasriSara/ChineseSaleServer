using Microsoft.AspNetCore.Mvc;
using serverProjectFinal.DAL;
using serverProjectFinal.Models;

namespace serverProjectFinal.BLL
{
    public class LotteryService : ILotteryService
    {
        private readonly ILotteryDal _lotteryDal;

        public LotteryService(ILotteryDal lotteryDal)
        {
            _lotteryDal = lotteryDal;
        }

        public async  Task<ActionResult<int>> GetAllMoneyBll()
        {
            return await _lotteryDal.GetAllMoneyDal();
        }

        public async Task<List<Lottery>> GetPresentsAsync()
        {
            return await _lotteryDal.GetPresentsAsync();
        }

        public async Task<Customer> LotteryUser(int productId)
        {
            return await _lotteryDal.LotteryUser(productId);
        }


    }
}
