namespace WebApi.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class AppDbInitializer
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AppDbInitializer> _logger;

        public AppDbInitializer(AppDbContext context, ILogger<AppDbInitializer> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        ///     Recreate the database on application startup,existing data will not be preserved. Should
        ///     be used during early development.
        /// </summary>
        public Task EarlyDevStageAsync()
        {
            try
            {
                _context.Database.EnsureDeleted();
                _context.Database.EnsureCreated();
                return SeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        ///     Initialises the database on application startup while preserving existing data. Should
        ///     be used once initial development has been completed.
        /// </summary>
        public Task LateDevStage()
        {
            _context.Database.Migrate();
            return SeedAsync();
        }

        public Task SeedAsync()
        {
            // Default data Seed, if necessary
            if (_context.Cars.Any())
            {
                var productList = new List<Car>();
                productList.Add(new Car { Id = Guid.NewGuid(), Model = "Civic", Make = "Honda" });

                _context.AddRange(productList);
            }

            return _context.SaveChangesAsync();
        }
    }
}