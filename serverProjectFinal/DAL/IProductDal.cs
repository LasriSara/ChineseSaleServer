using Microsoft.AspNetCore.Mvc;
using serverProjectFinal.DTO;
using serverProjectFinal.Models;

namespace serverProjectFinal.DAL
{
    public interface IProductDal
    {
        Task<List<Product>> GetProductDal();
        Task<Product> AddProductDal(Product product);
        Task<string> UpdateProductDal(int id,Product product);
        Task<string> DeleteProductDal(int id);
        Task<IEnumerable<Product>> SearchGiftsDal(string? name, string? donorName, int? numOfPurcheses);
    
        Task<List<Product>> GetPresentByCategoryDal(string category);
        Task<IEnumerable<Product>> getProducts( int? minPrice, int? maxPrice);

    }
}
