using BuzzHouse.Services.Interfaces;
using BuzzHouse.Services.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    
    var cosmosUrl = "https://ada.documents.azure.com:443/";
    var cosmosKey = "";
    var databaseName = "ada";

    services.AddScoped<IUserService, CosmosUserService>(provider =>
        new CosmosUserService(cosmosUrl, cosmosKey, databaseName));

    // Add Swagger configuration
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    });
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();