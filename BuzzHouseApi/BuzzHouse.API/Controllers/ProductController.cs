using BuzzHouse.Model.Models;
using BuzzHouse.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BuzzHouse.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController: ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] Product product) 
    {
        var result  = await _productService.CreateProductAsync(product);

        if (result.HasErrors())
        {
            return BadRequest(result.Errors);
        }
        
        return Ok(result.Item);
    }

    [HttpGet]
    public async Task<IEnumerable<Product>> GetProducts()
    {
        var result = await _productService.GetProductsAsync();
        return result;
    }

    [HttpGet("{productId}")]
    //[Route("getUserById")]
    public async Task<ActionResult<Product>> GetProductById(Guid productId)
    {
        var result = await _productService.GetProductByIdAsync(productId);
       
        if (result == null)
            return NotFound();
        
        return result;
    }

    [HttpPut("{productId}")]
    public async Task<ActionResult<Product>> UpdateProductById(Guid productId, Product product)
    {
        if (product == null)
        {
            return BadRequest("Invalid product data");
        }
        
        var result = await _productService.UpdateProductByIdAsync(productId, product);

        if (result == null)
        {
            return NotFound();
        }

        return result;
    }

    [HttpDelete("{productId}")]
    public async Task<IActionResult> DeleteProduct(Guid productId)
    {
        await _productService.DeleteProductAsync(productId);
        return NoContent();
    }
}