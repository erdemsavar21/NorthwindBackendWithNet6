using Microsoft.AspNetCore.Mvc;
using Northwind.Business.Abstract;
using Northwind.Entities.Concrete;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace Northwind.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController : ControllerBase
    {
        private IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetList()
        {
            var result = await _productService.GetList();

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpGet("{productId:int}")]
        public async Task<IActionResult> GetProduct(int productId)
        {
            var result = await _productService.GetById(productId);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpGet("getlistwithname")]
        public async Task<IActionResult> GetListWithName()
        {
            var result = await _productService.GetProductsWithName();

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            var result = await _productService.Add(product);

            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

        [HttpPatch("{productId:int}")]
        public async Task<IActionResult> Update(int productId, [FromBody] JsonPatchDocument<Product> productPatch)
        {
            var productResult = await _productService.GetById(productId);

            if (productResult.Data == null)
            {
                return NotFound(productResult.Message);
            }

            Product product = productResult.Data;

            productPatch.ApplyTo(product, ModelState);

            var result = await _productService.Update(product);

            if (result.Success)
            {
                return Ok(result.Success);
            }

            return BadRequest(result.Message);

            //   if (productPatch.Operations[0].path.ToString() == "/prepaidAmount")


        }
    }
}