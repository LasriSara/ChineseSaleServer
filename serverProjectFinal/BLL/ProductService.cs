using Microsoft.AspNetCore.Mvc;
using serverProjectFinal.DAL;
using serverProjectFinal.DTO;
using serverProjectFinal.Models;

namespace serverProjectFinal.BLL
{
    public class ProductService : IProductService
    {
        private readonly IProductDal _productDal;
        public ProductService(IProductDal productDal)
        {
                this._productDal = productDal ?? throw new ArgumentNullException(nameof(productDal));
        }
        
        public async Task<List<Product>> GetProductBll()
        {
            return await _productDal.GetProductDal();
        }
        public async Task<Product> AddProductBll(Product product)
        {
            return await _productDal.AddProductDal(product);
        }

        public async Task<string> UpdateProductBll(int id, [FromBody] Product product)
        {
            return await _productDal.UpdateProductDal(id, product);
        }

      

        public async Task<string> DeleteProductBll(int id)
        {
            return await _productDal.DeleteProductDal(id);
        }

        public async Task<IEnumerable<Product>> SearchGiftsBll(string? name, string? donorName, int? numOfPurcheses)
        {
            return await _productDal.SearchGiftsDal(name, donorName, numOfPurcheses);
        }

        public async Task<List<Product>> GetPresentByCategory(string category)
        {
            return await _productDal.GetPresentByCategoryDal(category);
        }

        public async Task<IEnumerable<Product>> getProducts(int? minPrice,
           int? maxPrice)
        {
            return await _productDal.getProducts( minPrice, maxPrice);
        }

     
    }
}
