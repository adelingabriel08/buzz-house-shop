using BuzzHouse.Model.Models;
using BuzzHouse.Processor.Host.Options;
using BuzzHouse.Services.Contracts;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace BuzzHouse.Services.Services;

public class ProductService: IProductService
{
    private readonly CosmosClient _cosmosClient;
    private readonly CosmosDbOptions _cosmosDbOptions;
    private readonly ProductsOptions _productsOptions;

    public ProductService(CosmosClient cosmosClient, IOptions<CosmosDbOptions> options, IOptions<ProductsOptions> productOptions)
    {
        _cosmosClient = cosmosClient;
        _cosmosDbOptions = options.Value;
        _productsOptions = productOptions.Value;
    }
    
    public async Task<ServiceResult<Product>> CreateProductAsync(Product product)
    {
        try
        {
            var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName,_productsOptions.ContainerName);
            await container.CreateItemAsync(product, new PartitionKey(product.Id.ToString()));
        }
        catch (Exception ex)
        {
            return ServiceResult<Product>.Failed(ex.Message);
        }

        return ServiceResult<Product>.Succeded(product);
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        var products = new List<Product>();
        
        try
        {
            var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName,_productsOptions.ContainerName);
            var query = container.GetItemQueryIterator<Product>(new QueryDefinition(
                "SELECT p.id, p.type, p.name, p.description, p.price, p.stock, p.imageUrl, p.custom FROM " + _productsOptions.ContainerName +
                " AS p"));

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                products.AddRange(response);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }

        return products;
    }

    public async Task<Product> GetProductByIdAsync(Guid productId)
    {
        try
        {
            var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName,_productsOptions.ContainerName);
            var response = await container.ReadItemAsync<Product>(productId.ToString(), new PartitionKey(productId.ToString()));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<Product> UpdateProductByIdAsync(Guid productId, Product product)
    {
        try
        {
            var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName,_productsOptions.ContainerName);
            var response = await container.UpsertItemAsync(product, new PartitionKey(productId.ToString()));
            return response.Resource;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
    
    public async Task DeleteProductAsync(Guid productId)
    {
        var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName,_productsOptions.ContainerName);
        await container.DeleteItemAsync<Product>(productId.ToString(), new PartitionKey(productId.ToString()));
    }

}