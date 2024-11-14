

using AutoMapper;
using serverProjectFinal.DTO;
using serverProjectFinal.Models;

namespace serverProjectFinal
{
    public class Mapper: Profile
    {
        public Mapper()
        {

            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Donor, DonorDTO>().ReverseMap();
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<Customer, LotteryDTO>().ReverseMap();
            CreateMap<Lottery, LotteryDTO>().ReverseMap();

        }
    }
}
