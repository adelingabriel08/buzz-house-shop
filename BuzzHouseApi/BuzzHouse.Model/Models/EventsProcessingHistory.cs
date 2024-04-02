namespace BuzzHouse.Model.Models;

public class ProcessedEvent
{
    public string ProcessingTime { get; set; }
    public string ProcessorInstanceName { get; set; }
    public string ProcessorName { get; set; }
    public string EntityId { get; set; }
    public string ContainerName { get; set; }
    public bool IsSuccessfullyProcessed { get; set; }
    public Guid Id { get; set; }
}