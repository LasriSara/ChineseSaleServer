using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using serverProjectFinal.BLL;
using serverProjectFinal.DTO;
using serverProjectFinal.Models;

namespace serverProjectFinal.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LotteryController : ControllerBase
    {
        private readonly ILotteryService _lotteryService;
        private readonly IMapper _mapper;

        public LotteryController(ILotteryService lotteryService, IMapper mapper)
        {
            _lotteryService = lotteryService;
            _mapper = mapper;
        }
        [Authorize(Roles = "manager")]
        [HttpGet]
        public async Task<List<LotteryDTO>> Get()
        {
            var lottries = await _lotteryService.GetPresentsAsync();
            return _mapper.Map<List<LotteryDTO>>(lottries);

        }


        [Authorize(Roles = "manager")]
        [HttpPost("{productId}")]
        //הגרלת מוצר לפי ה ID
        public async Task<ActionResult<LotteryDTO>> Post(int productId)
        {
            var user = await _lotteryService.LotteryUser(productId);
            if (user != null)
            {
                var lotteryDTO = _mapper.Map<LotteryDTO>(user);
                return Ok(lotteryDTO); 
            }
            return NotFound(); 
        }




        [Authorize(Roles = "manager")]
        [HttpGet("total")]
        public async Task<ActionResult<int>> GetAllMoney()
        {
            var lottries = await _lotteryService.GetAllMoneyBll();
            return lottries;

        }


    }
}