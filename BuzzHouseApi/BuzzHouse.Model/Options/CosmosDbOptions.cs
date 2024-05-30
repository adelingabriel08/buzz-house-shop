namespace BuzzHouse.Processor.Host.Options;

public class CosmosDbOptions
{
    public string CosmosUrl { get; set; }
    public string CosmosKey { get; set; }
    public string DatabaseName { get; set; }
    public string DeploymentRegion { get; set; }
}