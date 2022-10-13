namespace WebApi.Controllers
{
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Services;

    /// <summary>
    ///     Api Controller Base
    /// </summary>
    [ApiController]
    [ApiVersion("v1")]
    [Route("api/v1/[controller]")]
    [Produces("application/json", "application/xml")]
    [Consumes("application/json", "application/xml")]
    public abstract class BaseApiController<TEntity> : ControllerBase where TEntity : class, new()
    {
        private static readonly SemaphoreSlim s_sSemaphore = new(1, 1);
        private readonly ICacheService _cacheService;
        protected readonly AppDbContext Context;
        protected readonly ILogger<BaseApiController<TEntity>> Logger;

        protected BaseApiController(
            ICacheService cacheService,
            ILogger<BaseApiController<TEntity>> logger, AppDbContext context)
        {
            _cacheService = cacheService;
            Logger = logger;
            Context = context;
        }

        /// <summary>
        ///     Try to retrieve data from the cacheService.
        ///     If the value is not present, it queries the database and add to the cacheStore before returning.
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="cancellationToken"></param>
        protected async Task<IEnumerable<TEntity>> GetFromCacheStoreAsync(string cacheKey,
            CancellationToken cancellationToken)
        {
            try
            {
                var data = Enumerable.Empty<TEntity>();
                await s_sSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);

                data = await _cacheService.GetAsync<IEnumerable<TEntity>>(cacheKey).ConfigureAwait(false);
                if (data != null) { return data; }

                data = await Context.Set<TEntity>().AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);
                _ = await _cacheService.SetAsync(cacheKey, data, TimeSpan.FromSeconds(2400)).ConfigureAwait(false);

                return data;
            }
            finally
            {
                s_sSemaphore.Release();
            }
        }
    }
}