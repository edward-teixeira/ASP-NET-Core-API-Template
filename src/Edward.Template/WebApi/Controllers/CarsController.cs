namespace WebApi.Controllers
{
    using Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Services;

    [AllowAnonymous]
    public class CarsController : BaseApiController<Car>
    {
        private const string CarListCacheKey = "CarsList";

        public CarsController(AppDbContext context, ICacheService cacheService, ILogger<CarsController> logger)
            : base(cacheService, logger, context)
        {
        }

        /// <summary>
        ///     Get Cars
        /// </summary>
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars(CancellationToken cancellationToken)
        {
            var entities = await GetFromCacheStoreAsync(CarListCacheKey, cancellationToken).ConfigureAwait(false);
            return Ok(entities);
        }

        /// <summary>
        ///     Get Car
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Car>> GetCar(Guid id, CancellationToken cancellationToken)
        {
            var entity = await Context.Cars.FirstOrDefaultAsync(e => e.Id == id, cancellationToken)
                .ConfigureAwait(false);
            if (entity is null)
            {
                return NotFound();
            }

            return Ok(entity);
        }
    }
}