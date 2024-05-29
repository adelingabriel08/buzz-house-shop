using System.Security.Claims;
using BuzzHouse.Model.Models;
using BuzzHouse.Processor.Host.Options;
using BuzzHouse.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using User = BuzzHouse.Model.Models.User;

namespace BuzzHouse.Services.Services;

public class CosmosUserService: IUserService
{
    private readonly CosmosClient _cosmosClient;
    private readonly CosmosDbOptions _cosmosDbOptions;
    private readonly UsersOptions _usersOptions;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CosmosUserService(CosmosClient cosmosClient, IOptions<CosmosDbOptions> options, IOptions<UsersOptions> usersOptions, IHttpContextAccessor httpContextAccessor)
    {
        _cosmosClient = cosmosClient;
        _httpContextAccessor = httpContextAccessor;
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
                "SELECT u.id, u.firstName, u.lastName, u.shippingAddress FROM " + _usersOptions.ContainerName +
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

    public async Task<User> GetUserByIdAsync(string userId)
    {
        try
        {
            var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName,_usersOptions.ContainerName);
            var response = await container.ReadItemAsync<User>(userId, new PartitionKey(userId));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<User> UpdateUserByIdAsync(string userId, User user)
    {
        try
        {
            var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName,_usersOptions.ContainerName);
            var response = await container.UpsertItemAsync(user, new PartitionKey(userId));
            return response.Resource;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
    
    public async Task DeleteUserAsync(string userId)
    {
        var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName,_usersOptions.ContainerName);
        await container.DeleteItemAsync<User>(userId, new PartitionKey(userId));
    }

    public string GetCurrentUserEmailAddress()
    {
        return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)!.Value;
    }

    public async Task<User> GetOrCreateCurrentUserAsync()
    {
        var email = GetCurrentUserEmailAddress();

        User? user;
        
        try
        {
            var container = _cosmosClient.GetContainer(_cosmosDbOptions.DatabaseName,_usersOptions.ContainerName);
            var response = await container.ReadItemAsync<User>(email, new PartitionKey(email));
            user = response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            user = null;
        }

        if (user is null)
        {
            var firstname = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.GivenName)?.Value;
            var lastName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Surname)?.Value;
            user = new User(){ FirstName = firstname, LastName = lastName, Id = email};

            await CreateUserAsync(user);
        }

        return user;
    }
}