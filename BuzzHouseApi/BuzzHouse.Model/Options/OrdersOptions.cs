namespace BuzzHouse.Processor.Host.Options;

public class OrdersOptions
{
    public string DatabaseName { get; set; }
    public string ContainerName { set; get; }
    public string PartitionKey { set; get; }
}