
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using serverProjectFinal.BLL;
using serverProjectFinal.DTO;
using serverProjectFinal.Models;
using System.Security.Cryptography;
using System.Text;

namespace serverProjectFinal.DAL
{
    public class CustomerDal : ICustomerDal
    {
        private readonly PayingContext _payingContext;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerDal> _logger;  // Corrected here



        public CustomerDal(PayingContext payingContext, ILogger<CustomerDal> logger, IMapper mapper)
        {
            _payingContext = payingContext;
            _logger = logger;
            _mapper = mapper;
        }

       

        //הוספת משתמש
        public async Task<List<CustomerDTO>> AddUserDal(Customer customer)
        {
            await _payingContext.Customer.AddAsync(customer);
            await _payingContext.SaveChangesAsync();

            var customers = await _payingContext.Customer.ToListAsync();
            var customersDTO = _mapper.Map<List<CustomerDTO>>(customers);

            return customersDTO;
        }

        //חיבור משתמש
        public async Task<CustomerDTO> AuthenticateDal(LoginDTO loginDTO)
        {
            var currentUser = await _payingContext.Customer.FirstOrDefaultAsync(o =>
                o.Email.ToLower() == loginDTO.Email.ToLower() && o.PassWord == loginDTO.PassWord);

            if (currentUser != null)
            {
                var customerDTO = _mapper.Map<CustomerDTO>(currentUser);
                return customerDTO;
            }

            return null;
        }

    }
}




