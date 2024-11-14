using Microsoft.AspNetCore.Mvc;
using serverProjectFinal.DTO;
using serverProjectFinal.Models;

namespace serverProjectFinal.DAL
{
    public interface ICustomerDal
    {

        Task<List<CustomerDTO>> AddUserDal(Customer customer);
        public Task<CustomerDTO> AuthenticateDal(LoginDTO loginDTO);


    }
}
