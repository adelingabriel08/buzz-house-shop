using BuzzHouse.Model.Models;
using BuzzHouse.Processor.Host.Options;
using BuzzHouse.Services.Contracts;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BuzzHouse.Services.Services;

public class ProcessedEventsService : IProcessedEventsService
{
    private readonly CosmosClient _cosmosClient;
    private readonly ChangeFeedProcessorOptions _processorOptions;
    private readonly ILogger<ProcessedEventsService> _logger;

    public ProcessedEventsService(CosmosClient cosmosClient, IOptions<ChangeFeedProcessorOptions> processorOptions, 
        ILogger<ProcessedEventsService> logger)
    {
        _cosmosClient = cosmosClient;
        _processorOptions = processorOptions.Value;
        _logger = logger;
    }

    public async Task LogProcessedEventAsync(ProcessedEvent processedEvent)
    {
        var container = _cosmosClient.GetContainer(_processorOptions.DatabaseName, _processorOptions.ProcessedEventsContainerName);
        await container.CreateItemAsync(processedEvent, new PartitionKey(processedEvent.Id.ToString()));
        _logger.LogInformation($"Processed event logged for entity {processedEvent.EntityId}");
    }
}