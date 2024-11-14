
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using serverProjectFinal.Models;
using serverProjectFinal.BLL;
using AutoMapper;
using serverProjectFinal.DTO;
using Microsoft.AspNetCore.Authorization;


namespace serverProjectFinal.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _product;
        private readonly IMapper _mapper;

        public ProductController(IProductService product,IMapper mapper)
        {
            _product = product ?? throw new ArgumentNullException(nameof(product));
            _mapper = mapper;

        }
        //[Authorize(Roles = "manager")]
        [Route("GetProduct")]
        [HttpGet]
        public async Task<ActionResult<List<ProductDTO>>> GetProduct()
        {
            IEnumerable<Product> products = await _product.GetProductBll();
            IEnumerable<ProductDTO> productsDTO = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);
            if (productsDTO.Count() == 0)
            {
                return NoContent();
            }

            return Ok(productsDTO);

        }

        [Authorize(Roles = "manager")]
        [Route("AddProduct")]
        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] ProductDTO product)
        {
            Product newProduct = _mapper.Map<ProductDTO, Product>(product);
            ProductDTO newProductReturn = _mapper.Map<Product, ProductDTO>(await _product.AddProductBll(newProduct));
            if (newProductReturn == null)
            {
                return NoContent();
            }

            return Ok(newProductReturn);
        }

        [Authorize(Roles = "manager")]
        [Route("UpdateProduct/{id}")]
        [HttpPut]
        public async Task<string> UpdateProduct(int id, [FromBody] ProductDTO productDTO)
        {
            Product productToUpdate = _mapper.Map<Product>(productDTO);
            return await _product.UpdateProductBll(id, productToUpdate);
        }

        [Authorize(Roles = "manager")]
        [Route("DeleteProduct/{id}")]
        [HttpDelete]
        public async Task<ActionResult<string>> DeleteProduct(int id)
        {
            return await _product.DeleteProductBll(id);
        }

        [Authorize(Roles = "manager")]
        [Route("SearchGifts")]
        [HttpGet]
        public async Task<IEnumerable<Product>> SearchGifts(string? name, string? donorName, int? numOfPurcheses)
        {
            return await _product.SearchGiftsBll(name, donorName, numOfPurcheses);
        }


        //[Authorize(Roles = "manager")]
        [HttpGet("byCategory/{category}")]
        public async Task<List<Product>> GetByCategory(string category)
        {
            return await _product.GetPresentByCategory(category);
        }

        //[Authorize(Roles = "manager")]
        [HttpGet("byPrice")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get(int? minPrice, int? maxPrice)
        {
            IEnumerable<Product> products = await _product.getProducts( minPrice, maxPrice);
            IEnumerable<ProductDTO> productsDTO = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);
            if (productsDTO.Count() == 0)
            {
                return NoContent();
            }

            return Ok(productsDTO);

        }

    }

}