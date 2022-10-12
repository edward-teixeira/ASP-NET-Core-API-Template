namespace WebApi.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <inheritdoc />
    [Serializable]
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException()
        {
        }

        public UnauthorizedException(string? message) : base(message)
        {
        }

        public UnauthorizedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public UnauthorizedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}