namespace WebApi.ViewModels
{
    /// <summary>
    ///     A make and model of car.
    /// </summary>
    public record CarViewModel
    {
        #region Ctor

        /// <summary>
        ///     CarViewModel constructor
        /// </summary>
        /// <param name="carId"></param>
        /// <param name="cylinders"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <param name="url"></param>
        public CarViewModel(Guid carId, int cylinders, string make, string model, Uri? url = null)
        {
            CarId = carId;
            Cylinders = cylinders;
            Make = make;
            Model = model;
            Url = url;
        }

        #endregion Ctor

        /// <summary>
        ///     Gets or sets the cars unique identifier.
        /// </summary>
        /// <example>1</example>
        public Guid CarId { get; init; }

        /// <summary>
        ///     Gets or sets the number of cylinders in the cars engine.
        /// </summary>
        /// <example>6</example>
        public int Cylinders { get; init; }

        /// <summary>
        ///     Gets or sets the make of the car.
        /// </summary>
        /// <example>Honda</example>
        public string Make { get; init; } = default!;

        /// <summary>
        ///     Gets or sets the model of the car.
        /// </summary>
        /// <example>Civic</example>
        public string Model { get; init; } = default!;

        /// <summary>
        ///     Gets or sets the URL used to retrieve the resource conforming to REST'ful JSON http://restfuljson.org/.
        /// </summary>
        /// <example>/cars/1</example>
        public Uri? Url { get; init; }
    }
}