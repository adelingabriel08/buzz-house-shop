namespace BuzzHouse.Services.Services;

public class ServiceResult
{
    public List<string> Errors { get; } = new List<string>();

    public ServiceResult(string error)
    {
        Errors.Add(error);
    }

    public ServiceResult(string[] errors)
    {
        Errors.AddRange(errors);
    }

    protected ServiceResult() {}

    public bool HasErrors()
        => Errors.Any();

    public static ServiceResult Succeded() => new ServiceResult();
    public static ServiceResult Failed(string error) => new ServiceResult(error);
    public static ServiceResult Failed(string[] errors) => new ServiceResult(errors);
}
public class ServiceResult<TViewModel>: ServiceResult
{
    public TViewModel? Item;
    public ServiceResult(string error) : base(error)
    {
        Errors.Add(error);
    }

    public ServiceResult(string[] errors) : base(errors)
    {
        Errors.AddRange(errors);
    }
    public ServiceResult(TViewModel item)
    {
        Item = item;
    }
    public static ServiceResult<TViewModel> Succeded(TViewModel item) => new ServiceResult<TViewModel>(item);
    public static ServiceResult<TViewModel> Failed(string error) => new ServiceResult<TViewModel>(error);
    public static ServiceResult<TViewModel> Failed(string[] errors) => new ServiceResult<TViewModel>(errors);
}