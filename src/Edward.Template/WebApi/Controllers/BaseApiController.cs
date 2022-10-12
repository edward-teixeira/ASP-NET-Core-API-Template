namespace WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using WebApi.Extensions;
    using WebApi.Models;
    using WebApi.Services;

    /// <summary>
    ///     Api Controller Base
    /// </summary>
    [ApiController]
    [ApiVersion("v1")]
    [Route("api/v1/[controller]")]
    [Produces("application/json", "application/xml")]
    [Consumes("application/json", "application/xml")]
    public abstract class BaseApiController : ControllerBase
    {
        private const string CarListCacheKey = "CarsList";
        private static readonly SemaphoreSlim s_sSemaphore = new(1, 1);
        private readonly ICacheService _cacheService;
        protected readonly ILogger<BaseApiController> Logger;

        protected BaseApiController(
            ICacheService cacheService,
            ILogger<BaseApiController> logger)
        {
            _cacheService = cacheService;
            Logger = logger;
        }

        private async Task<IActionResult> Index()
        {
            // MemoryCache example
            try
            {
                await s_sSemaphore.WaitAsync().ConfigureAwait(false);
                var value = _cacheService.Get<IEnumerable<Car>>(CarListCacheKey);

                if (value != null) { return Ok(value.ToList()); }

                // value = await _context!.DbSet.ToListAsync().ConfigureAwait(false) as IEnumerable<Product>;
                await _cacheService.SetAsync(CarListCacheKey, value, TimeSpan.FromSeconds(2400));

                return Ok(value);
            }
            catch (Exception ex)
            {
                Logger.LogError("{ErrorMessage}", ex.ToStringFormatted());
                throw;
            }
            finally
            {
                s_sSemaphore.Release();
            }
        }
    }
}