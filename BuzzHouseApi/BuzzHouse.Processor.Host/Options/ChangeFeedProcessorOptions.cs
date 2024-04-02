namespace BuzzHouse.Processor.Host.Options;

public class ChangeFeedProcessorOptions
{
    public string DatabaseName { get; set; }
    public string SourceContainerName { get; set; }
    public string LeasesContainerName { get; set; }
}