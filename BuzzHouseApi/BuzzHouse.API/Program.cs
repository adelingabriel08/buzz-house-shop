using BuzzHouse.Processor.Host.Options;
using BuzzHouse.Services.Contracts;
using BuzzHouse.Services.Services;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<IUserService, CosmosUserService>();
builder.Services.AddScoped<IProductService, ProductService>();

var userOptions = builder.Configuration.GetSection(nameof(UsersOptions)).Get<UsersOptions>();
var productOptions = builder.Configuration.GetSection(nameof(ProductsOptions)).Get<ProductsOptions>();
var cosmosDbOptions = builder.Configuration.GetSection(nameof(CosmosDbOptions)).Get<CosmosDbOptions>();

var serializerOptions = new CosmosSerializationOptions() { PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase };
var clientOptions = new CosmosClientOptions() { SerializerOptions = serializerOptions };
var cosmosClient = new CosmosClient(cosmosDbOptions.CosmosUrl, cosmosDbOptions.CosmosKey, clientOptions);

builder.Services.AddSingleton(cosmosClient);
builder.Services.AddOptions<CosmosDbOptions>()
    .Bind(builder.Configuration.GetSection(nameof(CosmosDbOptions)));
builder.Services.AddOptions<UsersOptions>()
    .Bind(builder.Configuration.GetSection(nameof(UsersOptions)));
builder.Services.AddOptions<ProductsOptions>()
    .Bind(builder.Configuration.GetSection(nameof(ProductsOptions)));

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
await databaseResponse.Database.CreateContainerIfNotExistsAsync(productOptions.ContainerName, productOptions.PartitionKey);

app.MapControllers();

app.Run();