using BuzzHouse.Model.Models;
using BuzzHouse.Services.Services;

namespace BuzzHouse.Services.Contracts;

public interface IProductService
{
    Task<ServiceResult<Product>> CreateProductAsync(Product product);
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<Product> GetProductByIdAsync(Guid productId);
    Task<Product> UpdateProductByIdAsync(Guid productId, Product product);
    Task DeleteProductAsync(Guid productId);
}