using System.Text.Json;
using BuzzHouse.Processor.Host;
using BuzzHouse.Processor.Host.Options;
using BuzzHouse.Services.Contracts;
using BuzzHouse.Services.Services;
using Microsoft.Azure.Cosmos;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var serializerOptions = new CosmosSerializationOptions() { PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase };
var clientOptions = new CosmosClientOptions() { SerializerOptions = serializerOptions };
var cosmosClient = new CosmosClient(builder.Configuration["CosmosCredentials:Endpoint"], builder.Configuration["CosmosCredentials:Key"], clientOptions);

builder.Services.AddSingleton(cosmosClient);
builder.Services.AddOptions<ChangeFeedProcessorOptions>()
    .Bind(builder.Configuration.GetSection(nameof(ChangeFeedProcessorOptions)));
builder.Services.AddTransient<IProcessedEventsService, ProcessedEventsService>();
    
var host = builder.Build();
host.Run();