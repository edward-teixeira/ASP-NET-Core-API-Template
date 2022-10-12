namespace WebApi.Exceptions
{
    using System;

    /// <inheritdoc />
    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        private NotFoundException()
        {
        }
    }
}