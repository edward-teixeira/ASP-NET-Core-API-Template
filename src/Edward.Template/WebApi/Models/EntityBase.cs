namespace WebApi.Models
{
    public abstract class EntityBase : IEquatable<EntityBase?>
    {
        protected EntityBase()
        {
            Id = Guid.NewGuid();
        }

        public DateTime CreatedAtUtc { get; set; }
        public Guid Id { get; set; }
        public DateTime UpdatedAtUtc { get; set; }

        public bool Equals(EntityBase? other) =>
            other is not null &&
            Id.Equals(other.Id) &&
            CreatedAtUtc == other.CreatedAtUtc &&
            UpdatedAtUtc == other.UpdatedAtUtc;

        public static bool operator !=(EntityBase? left, EntityBase? right) => !(left == right);

        public static bool operator ==(EntityBase? left, EntityBase? right) =>
            EqualityComparer<EntityBase>.Default.Equals(left, right);

        public override bool Equals(object? obj) => Equals(obj as EntityBase);

        public override int GetHashCode() => HashCode.Combine(Id, CreatedAtUtc, UpdatedAtUtc);
    }
}