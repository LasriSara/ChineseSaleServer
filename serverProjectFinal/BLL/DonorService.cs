using Microsoft.AspNetCore.Mvc;
using serverProjectFinal.DAL;
using serverProjectFinal.Models;
using System.Drawing;
using System.Text.RegularExpressions;

namespace serverProjectFinal.BLL
{
    public class DonorService : IDonorService
    {
        private readonly IDonorDal _donorDal;

        public DonorService(IDonorDal donorDal)
        {
            this._donorDal = donorDal ?? throw new ArgumentNullException(nameof(donorDal));
        }

        public async Task<List<Donor>> GetDonorBll()
        {
            return await _donorDal.GetDonorDal();
        }
        public async Task<Donor> AddDonorBll(Donor donor)
        {
            // Email validation
            if (!IsValidEmail(donor.Email))
            {
                throw new ArgumentException("Invalid email format");
            }

            return await _donorDal.AddDonorDal(donor);
        }
        
        private bool IsValidEmail(string email)
        {
            string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            Regex regex = new Regex(emailPattern);
            return regex.IsMatch(email);
        }



        public async Task<string> UpdateDonorBll(int id, [FromBody] Donor donor)
        {
            return await _donorDal.UpdateDonorDal(id,donor);
        }

        public async Task<List<Donor>>  DeleteDonorBll(int id)
        {
            return await _donorDal.DeleteDonorDal(id);
        }

        public async Task<IEnumerable<Donor>> SelectDonorByNameEmailProductBll(string? name, string? email ,string? productName)
        {
            return await _donorDal.SelectDonorByNameEmailProductDal(name, email, productName);
        }

        public async Task<List<string>> GetDonorDetailsBll(int donorId)
        {
            return await _donorDal.GetDonorDetailsDal(donorId);
        }

       
    }
}
