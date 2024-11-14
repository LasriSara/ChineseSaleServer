using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using serverProjectFinal.DTO;
using serverProjectFinal.Models;
using System.Drawing;
using Newtonsoft.Json;
using System.Xml.Linq;


namespace serverProjectFinal.DAL
{
    public class ProductDal : IProductDal
    {
        private readonly PayingContext _payingContext;
        private readonly ILogger<Product> _logger;
        private readonly IMapper _mapper;

        public ProductDal(PayingContext payingContext, ILogger<Product> logger)
        {
            _payingContext = payingContext ?? throw new ArgumentNullException(nameof(payingContext));
            _logger = logger;
        }

        public async Task<List<Product>> GetProductDal()
        {
            try
            {
                return await _payingContext.Products.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Logging from GetProduct, the exception: " + ex.Message, 1);
                throw new Exception("Logging from GetProduct, the exception: " + ex.Message);
            }
        }

        public async Task<Product> AddProductDal(Product product)
        {
            try
            {
                await _payingContext.Products.AddAsync(product);
                await _payingContext.SaveChangesAsync();
                return product;

              
            }
            catch (Exception ex)
            {
                _logger.LogError("Logging from AddProduct, the exception: " + ex.Message, 1);
                throw new Exception("Logging from AddProduct, the exception: " + ex.Message);
            }
            
        }

   


        public async Task<string> UpdateProductDal(int id, Product product)
        {
            try
            {
                var existingProduct = await _payingContext.Products.FindAsync(id);

                if (existingProduct == null)
                {
                    return $"Product with ID {id} not found.";
                }

                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.UrlImage = product.UrlImage;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.DonorId = product.DonorId;


                _payingContext.Products.Update(existingProduct);
                await _payingContext.SaveChangesAsync();

                return $"Product {existingProduct.Name} updated successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating product with ID {id}. Exception: {ex.Message}", 1);
                throw new Exception($"Error updating product with ID {id}. Exception: {ex.Message}");
            }
        }

        public async Task<string> DeleteProductDal(int id )
        {
            try
            {
                var existingProduct = await _payingContext.Products.FindAsync(id);
                if (existingProduct != null)
                {
                    _payingContext.Products.Remove(existingProduct);
                    await _payingContext.SaveChangesAsync();
                    return $"Product {id} deleted";
                }
                return $"Product {id} not found";
              
            }
            catch (Exception ex)
            {
                _logger.LogError("Logging from DeleteProduct, the exception: " + ex.Message, 1);
                throw new Exception("Logging from DeleteProduct, the exception: " + ex.Message);
            }
        }



        public async Task<IEnumerable<Product>> SearchGiftsDal(string? name, string? donorName, int? numOfPurcheses)
        {
           

            try
            {
                var query = _payingContext.Products
                    .Include(g => g.Donor)
                    .Where(g =>
                        (name == null ? (true) : (g.Name.Contains(name)))&&
                        (donorName == null || g.Donor.FirstName.Contains(donorName))&&
                        (numOfPurcheses == null ? true : _payingContext.OrderItem.Count(oi => oi.ProductId == g.ProductId) == numOfPurcheses)

                        );


                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Logging from SearchGifts, the exception: {ex.Message}", 1);
                throw new DbUpdateException("Logging from SearchGifts, the exception: " + ex.Message, ex);
            }
        }



        public async Task<List<Product>> GetPresentByCategoryDal(string categoryName)
        {
            try
            {
                  var p = await _payingContext.Products
                        .Where(product => product.Category.Name == categoryName)
                        .ToListAsync();
                         return p;
            }

            catch (Exception ex)
            {
                _logger.LogError("Logging from GetPresentByCategoryDal, the exception: " + ex.Message, 1);
                throw new Exception("Logging from GetPresentByCategoryDal, the exception: " + ex.Message);
            }


        }


        public async Task<IEnumerable<Product>> getProducts(int? minPrice,int? maxPrice)
        {
            try
            {
                var query = _payingContext.Products.Where(p =>

                ((minPrice == null) ? (true) : (p.Price >= minPrice))
                && ((maxPrice == null) ? (true) : (p.Price <= maxPrice))
                );
                List<Product> products = await query.ToListAsync();
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError("Logging from getProducts, the exception: " + ex.Message, 1);
                throw new Exception("Logging from getProducts, the exception: " + ex.Message);
            }



        }

       
    }
}