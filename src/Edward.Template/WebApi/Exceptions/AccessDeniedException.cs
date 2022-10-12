namespace WebApi.Exceptions
{
    using System;

    /// <inheritdoc />
    [Serializable]
    public class AccessDeniedException : Exception
    {
        public AccessDeniedException()
        {
        }

        public AccessDeniedException(string? message) : base(message)
        {
        }

        public AccessDeniedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}