using ECommerce.SharedLibrary.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.Conversions;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;

namespace ProductApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProduct productInterface) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            //Get all products from repo
            var products = await productInterface.GetAllAsync();
            if(products.Any())
                return NotFound("No products found");
            //convert data from entity to DTO and return
            var(_,list) = ProductConversion.FromEntity(null!, products);
            return list!.Any() ? Ok(list) : NotFound("No products found");
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            //Get single product from the repo
            var product = await productInterface.FindByIdAsync(id);
            if (product is null) 
                return NotFound("Product not found");
            // convert from entity to DTO and return
            var(_product,_) = ProductConversion.FromEntity(product, null!);
            return _product is not null ? Ok(_product) : NotFound("No products found");
        }
        [HttpPost]
        public async Task<ActionResult<Response>> CreateProduct(ProductDTO product)
        {
            //check if model state is all data annotations are valid
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);
            //convery to entity
            var getEntity = ProductConversion.ToEntity(product);
            var response = await productInterface.CreateAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }
        [HttpPut]
        public async Task<ActionResult<Response>> UpdateProduct(ProductDTO product)
        {
            //check if model state is all data annotations are valid
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);
            //convery to entity
            var getEntity = ProductConversion.ToEntity(product);
            var response = await productInterface.UpdateAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }
        [HttpDelete]
        public async Task<ActionResult<Response>> DeleteProduct(ProductDTO product)
        {
            //convery to entity
            var getEntity = ProductConversion.ToEntity(product);
            var response = await productInterface.DeleteAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }
    }
}
