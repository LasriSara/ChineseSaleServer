using Microsoft.AspNetCore.Mvc;
using serverProjectFinal.DTO;
using serverProjectFinal.Models;

namespace serverProjectFinal.BLL
{
    public interface IProductService
    {
        Task<List<Product>> GetProductBll();
        Task<Product> AddProductBll(Product product);
        Task<string> UpdateProductBll(int id, [FromBody] Product product);
        Task<string> DeleteProductBll(int id);
        Task<IEnumerable<Product>> SearchGiftsBll(string? name , string? donorName, int? numOfPurcheses);

        public Task<List<Product>> GetPresentByCategory(string category);
        Task<IEnumerable<Product>> getProducts(int? minPrice, int? maxPrice);

    }
}
