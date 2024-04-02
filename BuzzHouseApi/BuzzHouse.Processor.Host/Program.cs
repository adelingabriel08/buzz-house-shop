using BuzzHouse.Processor.Host;
using BuzzHouse.Processor.Host.Options;
using Microsoft.Azure.Cosmos;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var cosmosClient = new CosmosClient(builder.Configuration["CosmosCredentials:Endpoint"], builder.Configuration["CosmosCredentials:Key"]);

builder.Services.AddSingleton(cosmosClient);
builder.Services.AddOptions<ChangeFeedProcessorOptions>()
    .Bind(builder.Configuration.GetSection(nameof(ChangeFeedProcessorOptions)));
    
var host = builder.Build();
host.Run();