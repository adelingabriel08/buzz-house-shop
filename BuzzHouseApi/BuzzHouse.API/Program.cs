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
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

var userOptions = builder.Configuration.GetSection(nameof(UsersOptions)).Get<UsersOptions>();
var productOptions = builder.Configuration.GetSection(nameof(ProductsOptions)).Get<ProductsOptions>();
var shoppingCartOptions = builder.Configuration.GetSection(nameof(ShoppingCartOptions)).Get<ShoppingCartOptions>();
var ordersOptions = builder.Configuration.GetSection(nameof(OrdersOptions)).Get<ShoppingCartOptions>();
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
builder.Services.AddOptions<ShoppingCartOptions>()
    .Bind(builder.Configuration.GetSection(nameof(ShoppingCartOptions)));
builder.Services.AddOptions<OrdersOptions>()
    .Bind(builder.Configuration.GetSection(nameof(ordersOptions)));

builder.Services.AddCors(x => x.AddDefaultPolicy(p => p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

DatabaseResponse databaseResponse = await cosmosClient.CreateDatabaseIfNotExistsAsync(cosmosDbOptions.DatabaseName);
await databaseResponse.Database.CreateContainerIfNotExistsAsync(userOptions.ContainerName, userOptions.PartitionKey);
await databaseResponse.Database.CreateContainerIfNotExistsAsync(productOptions.ContainerName, productOptions.PartitionKey);
await databaseResponse.Database.CreateContainerIfNotExistsAsync(shoppingCartOptions.ContainerName, shoppingCartOptions.PartitionKey);
await databaseResponse.Database.CreateContainerIfNotExistsAsync(ordersOptions.ContainerName, ordersOptions.PartitionKey);

app.UseCors();

app.MapControllers();

app.Run();