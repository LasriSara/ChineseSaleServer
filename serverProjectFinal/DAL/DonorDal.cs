using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using serverProjectFinal.Models;
using System.Drawing;

namespace serverProjectFinal.DAL
{
    public class DonorDal : IDonorDal
    {
        private readonly PayingContext _payingContext;
        private readonly ILogger<Donor> _logger;
        public DonorDal(PayingContext payingContext, ILogger<Donor> logger)
        {
               _payingContext = payingContext ?? throw new ArgumentNullException(nameof(payingContext));
               _logger = logger;
        }
        public async Task<List<Donor>> GetDonorDal()
        {
            try
            {
                return await _payingContext.Donor.ToListAsync();
            }           
            catch(Exception ex)
            {
                _logger.LogError("Logging from GetDonor, the exception: " + ex.Message, 1);
                throw new Exception("Logging from GetDonor, the exception: " + ex.Message);
            }
        }
        public async Task<Donor> AddDonorDal(Donor donor)
        {
            try
            {
                _payingContext.Donor.Add(donor);
                await this._payingContext.SaveChangesAsync();
                return donor;
            }
            catch (Exception ex)
            {
                _logger.LogError("Logging from AddDonor, the exception: " + ex.Message, 1);
                throw new Exception("Logging from AddDonor, the exception: " + ex.Message);
            }
        }


        public async Task<string> UpdateDonorDal(int id,Donor donor)
        {
            try
            {
                var existingDonor = await _payingContext.Donor.FindAsync(id);

                if (existingDonor == null)
                {
                    return $"Product with ID {id} not found.";
                }

                existingDonor.FirstName = donor.FirstName;
                existingDonor.LastName = donor.LastName;
                existingDonor.Phone = donor.Phone;
                existingDonor.MyType = donor.MyType;
                existingDonor.Email = donor.Email;




                _payingContext.Donor.Update(existingDonor);
                await _payingContext.SaveChangesAsync();

                return $"Product {existingDonor.FirstName} updated successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError("Logging from UpdateDonor, the exception: " + ex.Message, 1);
                throw new Exception("Logging from UpdateDonor, the exception: " + ex.Message);
            }
        }

    public async Task<List<Donor>> DeleteDonorDal(int id)
    {
        var a = _payingContext.Donor.Find(id);
        var buy = _payingContext.Products.Where(z => z.DonorId == id).ToList();
        if (buy != null)
        {
            for (int i = 0; i < buy.Count; i++)
            {
                var c = _payingContext.Products.Where(b => b.ProductId == buy[i].ProductId).ToList();
                if (c != null)
                {
                    return await _payingContext.Donor.ToListAsync();
                }
            }

        }
        _payingContext.Donor.Remove(a);
        _payingContext.SaveChanges();
        return await _payingContext.Donor.ToListAsync();

    }

    public async Task<IEnumerable<Donor>> SelectDonorByNameEmailProductDal(string? name, string? email,  string? productName)
        {
            try
            {
                var query = _payingContext.Donor
                    .Include(p => p.Product)
                    .Where(p =>
                        (name == null ? (true) : (p.FirstName.Contains(name)))
                        && (email == null ? (true) : (p.Email.Contains(email)))
                        //&& (giftId == null || p.Product.Any(p => p.ProductId.ToString() == giftId))
                        && (productName == null || p.Product.Any(p => p.Name.Contains(productName)))
                    );

                return await query.ToListAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError("Logging from SelectDonor, the exception: " + ex.Message, 1);
                throw new Exception("Logging from SelectDonor, the exception: " + ex.Message);
            }
        }


        public async Task<List<string>> GetDonorDetailsDal(int donorId)
        {
            try
            {
                var donor = await _payingContext.Donor
                    .Include(d => d.Product)
                    .FirstOrDefaultAsync(d => d.DonorId == donorId);

                if (donor != null)
                {
                    var contributions = donor.Product.Select(p => p.Name).ToList();
                    return contributions;
                }

                return new List<string>(); // אם לא נמצא תורם
            }
            catch (Exception ex)
            {
                _logger.LogError("Logging from GetDonorDetailsDal, the exception: " + ex.Message, 1);
                throw new Exception("Logging from GetDonorDetailsDal, the exception: " + ex.Message);
            }
        }
    }
}
