namespace WebApi.Services
{
    /// <summary>
    ///     DateTime Service
    /// </summary>
    public interface IDateTimeService
    {
        DateTime UtcNow { get; }
    }
}