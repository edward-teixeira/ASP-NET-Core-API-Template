namespace WebApi.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using WebApi.Models;

    public class CarConfiguration : EntityConfigurationBase<Car>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Car> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);
            builder.ToTable("cars");
            builder.HasKey(c => c.Id);
        }
    }
}