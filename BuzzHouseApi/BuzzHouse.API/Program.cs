using BuzzHouse.Processor.Host.Options;
using BuzzHouse.Services.Interfaces;
using BuzzHouse.Services.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<IUserService, CosmosUserService>();

var userOptions = builder.Configuration.GetSection(nameof(UsersOptions)).Get<UsersOptions>();
var cosmosDbOptions = builder.Configuration.GetSection(nameof(CosmosDbOptions)).Get<CosmosDbOptions>();

var serializerOptions = new CosmosSerializationOptions() { PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase };
var clientOptions = new CosmosClientOptions() { SerializerOptions = serializerOptions };
var cosmosClient = new CosmosClient(cosmosDbOptions.CosmosUrl, cosmosDbOptions.CosmosKey, clientOptions);

builder.Services.AddSingleton(cosmosClient);
builder.Services.AddOptions<CosmosDbOptions>()
    .Bind(builder.Configuration.GetSection(nameof(CosmosDbOptions)));
builder.Services.AddOptions<UsersOptions>()
    .Bind(builder.Configuration.GetSection(nameof(UsersOptions)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

DatabaseResponse databaseResponse = await cosmosClient.CreateDatabaseIfNotExistsAsync(cosmosDbOptions.DatabaseName);
await databaseResponse.Database.CreateContainerIfNotExistsAsync(userOptions.ContainerName, userOptions.PartitionKey);

app.MapControllers();

app.Run();