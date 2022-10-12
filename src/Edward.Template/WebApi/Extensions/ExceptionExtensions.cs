namespace WebApi.Extensions
{
    using System.Security;
    using WebApi.Helpers;

    /// <summary>
    ///     Exception Helpers utility methods
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        ///     Returns the hierarchy from the source. Validates that <paramref name="source" /> and
        ///     <paramref name="nextItem" /> is not null.
        /// </summary>
        /// <typeparam name="TSource">The type of the t source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="nextItem">The next item.</param>
        /// <returns>IEnumerable&lt;TSource&gt;.</returns>
        public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source,
            Func<TSource, TSource> nextItem)
            where TSource : Exception =>
            FromHierarchy(source, nextItem, s => s != null);

        /// <summary>
        ///     Returns the hierarchy from the source. Validates that <paramref name="source" />,
        ///     <paramref name="nextItem" /> and <paramref name="canContinue" /> is not null.
        /// </summary>
        /// <typeparam name="TSource">The type of the t source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="nextItem">The next item.</param>
        /// <param name="canContinue">The can continue.</param>
        /// <returns>IEnumerable&lt;TSource&gt;.</returns>
        public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source,
            Func<TSource, TSource> nextItem, Func<TSource, bool> canContinue)
            where TSource : Exception
        {
            for (var current = source; canContinue(current); current = nextItem(current))
            {
                yield return current;
            }
        }

        /// <summary>
        ///     Gets all messages from an <see cref="Exception" />. Validates that
        ///     <paramref
        ///         name="exception" />
        ///     is not null.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>System.String.</returns>
        public static string GetAllMessages(this Exception exception, char separator = ControlCharsHelper.Comma)
        {
            var messages = exception.FromHierarchy(ex => ex.InnerException)
                .Select(ex => ex.Message);

            return string.Join(separator, messages);
        }

        /// <summary>
        ///     Gets all messages from a <see cref="Exception" /> including the stack trace. Validates
        ///     that <paramref name="exception" /> is not null.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>System.String.</returns>
        public static IEnumerable<(string message, string StackTrace)> GetAllMessagesWithStackTrace(
            this Exception exception) =>
            exception.FromHierarchy(ex => ex.InnerException)
                .Select(ex => new
                {
                    ex.Message,
                    StackTrace = string.IsNullOrEmpty(ex.StackTrace) ? ex.StackTrace : "NONE"
                })
                .Select(c => (c.Message, c.StackTrace))
                .ToList();

        /// <summary>
        ///     Gets the most inner (deepest) exception of a given Exception object
        /// </summary>
        /// <param name="ex"></param>
        public static string GetInnerException(this Exception ex) =>
            ex.InnerException == null ? ex.Message : GetInnerException(ex.InnerException);

        /// <summary>
        ///     Determines whether the specified <see cref="Exception" /> is critical is a
        ///     <see
        ///         cref="NullReferenceException" />
        ///     , <see cref="StackOverflowException" />,
        ///     <see
        ///         cref="OutOfMemoryException" />
        ///     , <see cref="ThreadAbortException" />,
        ///     <see
        ///         cref="IndexOutOfRangeException" />
        ///     or <see cref="AccessViolationException" />. Validates
        ///     that <paramref name="exception" /> is not null.
        /// </summary>
        /// <param name="exception">The ex.</param>
        /// <returns><c>true</c> if the specified ex is critical; otherwise, <c>false</c>.</returns>
        public static bool IsCritical(this Exception exception) =>
            exception is NullReferenceException or
                StackOverflowException or
                OutOfMemoryException or
                ThreadAbortException or
                IndexOutOfRangeException or
                AccessViolationException;

        /// <summary>
        ///     Determines whether the specified <see cref="Exception" /> is fatal (
        ///     <see
        ///         cref="OutOfMemoryException" />
        ///     ). Validates that <paramref name="exception" /> is not null.
        /// </summary>
        /// <param name="exception">The ex.</param>
        /// <returns><c>true</c> if the specified ex is fatal; otherwise, <c>false</c>.</returns>
        public static bool IsFatal(this Exception exception) => exception is OutOfMemoryException;

        /// <summary>
        ///     Determines whether the <see cref="Exception" /> is <see cref="SecurityException" /> or is
        ///     critical]. Validates that <paramref name="exception" /> is not null.
        /// </summary>
        /// <param name="exception">The ex.</param>
        /// <returns>
        ///     <c>true</c> if [is security or critical] [the specified ex]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSecurityOrCritical(this Exception exception) =>
            exception is SecurityException || exception.IsCritical();

        /// <summary>
        ///     Turns any object to Exception.
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="Exception"></exception>
        public static void ToException(this object obj) => throw new Exception(nameof(obj));

        /// <summary>
        ///     Collect exception information in textual format.
        /// </summary>
        /// <param name="exception"></param>
        public static string ToStringFormatted(this Exception exception) =>
            string.Format
            ("Exception: {0}\nMessage: {1}\nStack Trace: {2}\nInner {3}", exception.GetType(),
                exception.Message, exception.StackTrace, exception.InnerException?.ToStringFormatted());

        /// <summary>
        ///     Traverses the <see cref="Exception" />. Validates that <paramref name="exception" /> is
        ///     not null.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="exception">The ex.</param>
        /// <returns>T.</returns>
        public static T? TraverseFor<T>(this Exception exception)
            where T : class =>
            ReferenceEquals(exception.GetType(), typeof(T))
                ? exception as T
                : exception.InnerException?.TraverseFor<T>();
    }
}