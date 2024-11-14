using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using serverProjectFinal.DAL;
using serverProjectFinal.DTO;
using serverProjectFinal.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace serverProjectFinal.BLL
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerDal _customerDal;
        private readonly IConfiguration _config;
        private readonly ILogger<CustomerService> _logger;  // Corrected here

        public CustomerService(ICustomerDal customerDal, IConfiguration config, ILogger<CustomerService> logger)
        {
            this._customerDal = customerDal;
            this._config = config;
            _logger = logger;
        }
        public async Task<List<CustomerDTO>> AddUser(Customer customer)
        {
            return await _customerDal.AddUserDal(customer);
        }

        
        public async Task<CustomerDTO> Authenticate(LoginDTO loginDTO)
        {
            return await _customerDal.AuthenticateDal(loginDTO);

        }

        public string Generate(CustomerDTO customerDTO)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
 {
new Claim(ClaimTypes.NameIdentifier, customerDTO.CustomerId.ToString()),
                    new Claim(ClaimTypes.Name, customerDTO.PassWord),

    new Claim(ClaimTypes.Surname, customerDTO.FirstName),
    new Claim(ClaimTypes.Surname, customerDTO.LastName),
    new Claim(ClaimTypes.HomePhone, customerDTO.Phone),
    new Claim(ClaimTypes.Email, customerDTO.Email),
    new Claim(ClaimTypes.Role, customerDTO.Roles),
};

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],

                //issuer: "http://localhost:4200/",
                //audience: "http://localhost:4200/",
                //_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
