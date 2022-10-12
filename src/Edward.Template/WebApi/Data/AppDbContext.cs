namespace WebApi.Data
{
    using System.Reflection;
    using Microsoft.EntityFrameworkCore;
    using WebApi.Models;

    public class AppDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            _ = builder.Model
                .GetEntityTypes()
                .SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string)))
                .Select(prop =>
                {
                    prop.SetColumnType("varchar(100)");
                    return prop;
                });
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entityEntry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entityEntry.State)
                {
                    case EntityState.Modified:
                        entityEntry.Entity.UpdatedAtUtc = DateTime.UtcNow;
                        break;

                    case EntityState.Added:
                        entityEntry.Entity.CreatedAtUtc = DateTime.UtcNow;
                        break;
                        // case EntityState.Detached: break; case EntityState.Unchanged: break; case
                        // EntityState.Deleted: break; default: throw new NotImplementedException();
                }
            }

            return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}