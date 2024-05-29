using BuzzHouse.Model.Models;
using BuzzHouse.Processor.Host.Options;
using BuzzHouse.Services.Contracts;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace BuzzHouse.Processor.Host;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ChangeFeedProcessorOptions _processorOptions;
    private readonly CosmosClient _cosmosClient;
    private readonly IProcessedEventsService _processedEventsService;
    private readonly IOrderService _orderService;

    public Worker(ILogger<Worker> logger, IOptions<ChangeFeedProcessorOptions> processorOptions, CosmosClient cosmosClient, 
        IProcessedEventsService processedEventsService, IOrderService orderService)
    {
        _logger = logger;
        _cosmosClient = cosmosClient;
        _processedEventsService = processedEventsService;
        _orderService = orderService;
        _processorOptions = processorOptions.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var processor = await StartChangeFeedProcessorAsync();
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
        await StopProcessorAsync(processor);
    }
    
    private async Task<ChangeFeedProcessor> StartChangeFeedProcessorAsync()
    { 
        await CreateLeasesContainerIfNotExistsAsync(_processorOptions.DatabaseName, _processorOptions.LeasesContainerName);
        await CreateProcessedEventsContainerIfNotExistsAsync(_processorOptions.DatabaseName, _processorOptions.ProcessedEventsContainerName);
        Container leaseContainer = _cosmosClient.GetContainer(_processorOptions.DatabaseName, _processorOptions.LeasesContainerName);
        ChangeFeedProcessor changeFeedProcessor = _cosmosClient.GetContainer(_processorOptions.DatabaseName, _processorOptions.SourceContainerName)
            .GetChangeFeedProcessorBuilder<Order>(processorName: typeof(Worker).Assembly.GetName().Name, onChangesDelegate: HandleChangesAsync)
            .WithInstanceName(Environment.MachineName)
            .WithLeaseContainer(leaseContainer)
            .Build();

        _logger.LogInformation("Starting Change Feed Processor...");
        await changeFeedProcessor.StartAsync();
        _logger.LogInformation("Change Feed Processor started.");
        return changeFeedProcessor;
    }
    
    private async Task StopProcessorAsync(ChangeFeedProcessor changeFeedProcessor)
    {
        _logger.LogInformation("Stopping Change Feed Processor...");
        await changeFeedProcessor.StopAsync();
        _logger.LogInformation("Change Feed Processor stopped.");
    }

    private async Task CreateLeasesContainerIfNotExistsAsync(string dbName, string container)
    {
        DatabaseResponse databaseResponse = await _cosmosClient.CreateDatabaseIfNotExistsAsync(dbName);
        await databaseResponse.Database.CreateContainerIfNotExistsAsync(container, "/id");
        _logger.LogInformation($"Container {container} created successfully.");
    }
    
    private async Task CreateProcessedEventsContainerIfNotExistsAsync(string dbName, string container)
    {
        DatabaseResponse databaseResponse = await _cosmosClient.CreateDatabaseIfNotExistsAsync(dbName);
        await databaseResponse.Database.CreateContainerIfNotExistsAsync(container, "/id");
        _logger.LogInformation($"Container {container} created successfully.");
    }
    
    private async Task HandleChangesAsync(
        ChangeFeedProcessorContext context,
        IReadOnlyCollection<Order> changes,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Started handling changes for lease {context.LeaseToken}...");
        _logger.LogInformation($"Change Feed request consumed {context.Headers.RequestCharge} RU.");
        // SessionToken if needed to enforce Session consistency on another client instance
        _logger.LogInformation($"SessionToken ${context.Headers.Session}");

        // We may want to track any operation's Diagnostics that took longer than some threshold
        if (context.Diagnostics.GetClientElapsedTime() > TimeSpan.FromSeconds(1))
        {
            _logger.LogInformation($"Change Feed request took longer than expected. Diagnostics:" + context.Diagnostics.ToString());
        }

        foreach (var item in changes)
        {
            var processedEvent = new ProcessedEvent
            {
                EntityId = item.Id.ToString(),
                ProcessingTime = DateTime.UtcNow.ToString(),
                ProcessorInstanceName = Environment.MachineName,
                ProcessorName = typeof(Worker).Assembly.GetName().Name,
                ContainerName = _processorOptions.SourceContainerName,
                IsSuccessfullyProcessed = false,
                Id = Guid.NewGuid()
            };
            try
            {
                _logger.LogInformation($"Detected operation for item with id {item.Id}, created at {item.CreatedDate}.");
                
                await _orderService.HandleOrderChange(item);

                processedEvent.IsSuccessfullyProcessed = true;
            } catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing item with id {item.Id} with exception: {ex.Message} : {ex.StackTrace}");
            }
            
            await _processedEventsService.LogProcessedEventAsync(processedEvent);
        }

        _logger.LogInformation("Finished handling changes.");
    }
}