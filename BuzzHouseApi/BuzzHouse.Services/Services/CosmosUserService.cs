using BuzzHouse.Processor.Host.Options;
using BuzzHouse.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
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
}