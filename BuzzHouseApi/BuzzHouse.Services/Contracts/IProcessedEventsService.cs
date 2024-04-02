using BuzzHouse.Model.Models;

namespace BuzzHouse.Services.Contracts;

public interface IProcessedEventsService
{
    Task LogProcessedEventAsync(ProcessedEvent processedEvent);
}