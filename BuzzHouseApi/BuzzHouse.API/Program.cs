using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

using BuzzHouse.Processor.Host.Options;
using BuzzHouse.Services.Contracts;
using BuzzHouse.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "Buzz House API",
        Version = "v1" //this links to ApiExplorerSettings.GroupName
    });

    options.DescribeAllParametersInCamelCase();
    
    options.AddSecurityDefinition("483940487682-6r62jodqsnep33dert0otauhkera0gvc.apps.googleusercontent.com", new OpenApiSecurityScheme
    {
        Scheme = "483940487682-6r62jodqsnep33dert0otauhkera0gvc.apps.googleusercontent.com",
        In = ParameterLocation.Header,
        Name = HeaderNames.Authorization,
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"https://accounts.google.com/o/oauth2/v2/auth", UriKind.Absolute),
                TokenUrl = new Uri($"https://oauth2.googleapis.com/token", UriKind.Absolute),
                Scopes = new Dictionary<string, string>
                {
                    { "openid" , "See your user identifier" },
                    { "email" , "See your email address" },
                    { "profile" , "See your user profile information" },
                    { "policy" , "See your security policy" },
                }
            }
        }
    });
    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });
});
builder.Services.AddControllers();
builder.Services.AddScoped<IUserService, CosmosUserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddHttpContextAccessor();

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

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt => jwt.UseGoogle(
    clientId: builder.Configuration["GoogleAuthentication:ClientId"]));

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