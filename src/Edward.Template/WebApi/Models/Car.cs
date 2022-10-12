namespace WebApi.Models
{
    using WebApi.ViewModels;

    public class Car : EntityBase
    {
        public int Cylinders { get; set; }

        public string Make { get; set; } = default!;

        public string Model { get; set; } = default!;

        public CarViewModel ToViewModel() => new (Id, Cylinders, Make, Model);
    }
}