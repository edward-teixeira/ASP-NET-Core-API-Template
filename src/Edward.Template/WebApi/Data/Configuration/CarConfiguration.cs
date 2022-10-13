namespace WebApi.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class CarConfiguration : EntityConfigurationBase<Car>
    {
        public override void Configure(EntityTypeBuilder<Car> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);
            builder.ToTable("cars");
            builder.HasKey(c => c.Id);
        }
    }
}