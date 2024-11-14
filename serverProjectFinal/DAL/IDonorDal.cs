using serverProjectFinal.Models;

namespace serverProjectFinal.DAL
{
    public interface IDonorDal
    {
        Task<List<Donor>> GetDonorDal();
        Task<Donor> AddDonorDal(Donor donor);
        Task<string> UpdateDonorDal(int id, Donor donor);
        Task<List<Donor>> DeleteDonorDal(int id);
        Task<IEnumerable<Donor>> SelectDonorByNameEmailProductDal(string? name, string? email, string? productName);
        Task<List<string>> GetDonorDetailsDal(int donorId);

    }
}
