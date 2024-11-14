using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using serverProjectFinal.BLL;
using serverProjectFinal.DTO;
using serverProjectFinal.Models;
using serverProjectFinal.DAL;
using Microsoft.Win32;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace serverProjectFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {

        private readonly PayingContext _PayingContext;

        private readonly IConfiguration _config;

        private readonly ICustomerService _customer;

        private readonly IMapper _mapper;

        public CustomerController(IConfiguration config, PayingContext payingContext, ICustomerService customer, IMapper mapper)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _PayingContext = payingContext ?? throw new ArgumentNullException(nameof(payingContext));
            _customer = customer ?? throw new ArgumentNullException(nameof(customer));
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginDTO loginDTO)
        {
            var user = await _customer.Authenticate(loginDTO);

            if (user != null)
            {
                object token = _customer.Generate(user);
                var jsonToken = JsonConvert.SerializeObject( token );
                return Ok(jsonToken );
                //return Ok(token);
            }
            return NotFound("User not found");
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<Customer>> Register([FromBody] CustomerDTO customerDTO)
        {
            if (customerDTO == null)
            {
                return BadRequest("Customer details are missing");
            }


            customerDTO.Roles ??= "Saller";

            var customer = _mapper.Map<Customer>(customerDTO);

            if (customer.PassWord == null || customer.Email == null 
                || customer.Phone == null || customer.LastName == null ||
                customer.FirstName == null )
            {
                return NotFound("Details are missing");
            }
            else
            {
                var registered = await _customer.AddUser(customer);
                return Ok(registered);
            }
        }


        
    }
}