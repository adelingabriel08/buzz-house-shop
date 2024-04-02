using BuzzHouse.Processor.Host.Options;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace BuzzHouse.Processor.Host;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ChangeFeedProcessorOptions _processorOptions;

    public Worker(ILogger<Worker> logger, IOptions<ChangeFeedProcessorOptions> processorOptions)
    {
        _logger = logger;
        _processorOptions = processorOptions.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Attempting to start worker at time UTC: {time}", DateTimeOffset.UtcNow);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}