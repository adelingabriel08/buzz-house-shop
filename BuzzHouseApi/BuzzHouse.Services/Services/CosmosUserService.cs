using BuzzHouse.Processor.Host.Options;
using BuzzHouse.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Microsoft.Extensions.Options;
using User = BuzzHouse.Model.Models.User;

namespace BuzzHouse.Services.Services;

public class CosmosUserService: IUserService
{
    private readonly CosmosClient _cosmosClient;
    private readonly CosmosDbOptions _cosmosDbOptions;
    private readonly UsersOptions _usersOptions;

    public CosmosUserService(CosmosClient cosmosClient, IOptions<CosmosDbOptions> options, IOptions<UsersOptions> usersOptions)
    {
        _cosmosClient = cosmosClient;
        _cosmosDbOptions = options.Value;
        _usersOptions = usersOptions.Value;
    }

    public async Task<ServiceResult<User>> CreateUserAsync(User user)
    {
        if (user.FirstName.Length < 3)
        {
            return ServiceResult<User>.Failed("Cannot have first name shorter than 3 characters.");
        }
        
        try
        {
            var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName,_usersOptions.ContainerName);
            await container.CreateItemAsync(user, new PartitionKey(user.Id.ToString()));
        }
        catch (Exception ex)
        {
            return ServiceResult<User>.Failed(ex.Message);
        }

        return ServiceResult<User>.Succeded(user);
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        var users = new List<User>();
        
        try
        {
            var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName,_usersOptions.ContainerName);
            var query = container.GetItemQueryIterator<User>(new QueryDefinition(
                "SELECT u.id, u.email, u.firstName, u.lastName, u.shippingAddress FROM " + _usersOptions.ContainerName +
                " AS u"));

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                users.AddRange(response);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }

        return users;
    }

    public async Task<User> GetUserByIdAsync(Guid userId)
    {
        try
        {
            var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName,_usersOptions.ContainerName);
            var response = await container.ReadItemAsync<User>(userId.ToString(), new PartitionKey(userId.ToString()));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<User> UpdateUserByIdAsync(Guid userId, User user)
    {
        try
        {
            var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName,_usersOptions.ContainerName);
            var response = await container.UpsertItemAsync(user, new PartitionKey(userId.ToString()));
            return response.Resource;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
    
    public async Task DeleteUserAsync(Guid userId)
    {
        var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName,_usersOptions.ContainerName);
        await container.DeleteItemAsync<User>(userId.ToString(), new PartitionKey(userId.ToString()));
    }
}