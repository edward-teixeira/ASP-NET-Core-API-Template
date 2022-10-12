namespace WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using WebApi.Data;
    using WebApi.Models;
    using WebApi.Services;

    [AllowAnonymous]
    public class CarsController : BaseApiController
    {
        private readonly AppDbContext _context;

        public CarsController(AppDbContext context, ICacheService cacheService, ILogger<CarsController> logger)
            : base(cacheService, logger)
        {
            _context = context;
        }

        /// <summary>
        /// Get Cars
        /// </summary>
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars(CancellationToken cancellationToken)
            => Ok(await _context.Cars.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false));

        /// <summary>
        /// Get Car
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Car>> GetCar(Guid id, CancellationToken cancellationToken)
        {
            var car = await _context.Cars.FindAsync(new object?[] { id, cancellationToken }, cancellationToken: cancellationToken).ConfigureAwait(false);
            if (car is null) { return NotFound(); }

            return Ok(car);
        }
    }
}