using Microsoft.AspNetCore.Mvc;
using serverProjectFinal.Models;

namespace serverProjectFinal.BLL
{
    public interface IDonorService
    {
        Task<List<Donor>> GetDonorBll();
        Task<Donor> AddDonorBll(Donor donor);
        Task<string> UpdateDonorBll(int id, [FromBody] Donor donor);
        Task<List<Donor>> DeleteDonorBll(int id);
        Task<IEnumerable<Donor>> SelectDonorByNameEmailProductBll(string? name, string? email, string? productName);
        Task<List<string>> GetDonorDetailsBll(int donorId);

    }
}
