using BuzzHouse.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using User = BuzzHouse.Model.Models.User;

namespace BuzzHouse.Services.Services;

public class CosmosUserService: IUserService
{
    private readonly string _cosmosUrl;
    private readonly string _cosmosKey;
    private readonly string _databaseName;

    public CosmosUserService(string cosmosUrl, string cosmosKey, string databaseName)
    {
        _cosmosUrl = cosmosUrl;
        _cosmosKey = cosmosKey;
        _databaseName = databaseName;
    }

    public async Task<IActionResult> CreateUserAsync(User user)
    {
        try
        {
            using (var client = new CosmosClient(_cosmosUrl, _cosmosKey))
            {
                Database database = await client.CreateDatabaseIfNotExistsAsync(_databaseName);
                Container container = await database.CreateContainerIfNotExistsAsync("User", "/Id", 400);

                user.Id = Guid.NewGuid();

                var response = await container.CreateItemAsync(user);
                return new CreatedResult($"/user/{user.Id}", user);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating user: {ex.Message}");
            return Index(); 
        }
    }

    public IActionResult Index()
    {
        return new BadRequestResult();
    }
}