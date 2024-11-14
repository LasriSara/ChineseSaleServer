using Microsoft.AspNetCore.Mvc;
using serverProjectFinal.DTO;
using serverProjectFinal.Models;

namespace serverProjectFinal.BLL
{
    public interface ICustomerService
    {
        public Task<List<CustomerDTO>> AddUser(Customer customer);
        public string Generate(CustomerDTO customer);
        public Task<CustomerDTO> Authenticate(LoginDTO loginDTO);

    }
}
