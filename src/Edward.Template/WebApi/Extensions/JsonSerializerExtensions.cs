namespace WebApi.Extensions
{
    using System.Text.Json;

    public static class JsonSerializerExtensions
    {
        /// <summary>
        ///     Parses the text representing a single JSON value
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="json"></param>
        /// <param name="options"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static TResult? JsonDeserialize<TResult>(this string json,
            JsonSerializerOptions? options = null)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException(nameof(json));
            }

            options ??= new JsonSerializerOptions { WriteIndented = true, AllowTrailingCommas = true };
            return JsonSerializer.Deserialize<TResult>(json, options);
        }

        /// <summary>
        ///     Convert an object into a JSON string
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="obj"></param>
        /// <param name="options"></param>
        public static string? JsonSerialize<TValue>(this TValue obj, JsonSerializerOptions? options = null)
        {
            ArgumentNullException.ThrowIfNull(obj);
            return options != null ? JsonSerializer.Serialize(obj, options) : JsonSerializer.Serialize(obj);
        }

        /// <summary>
        ///     Serializes to and object to a JSON file.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void SerializeToFile(this object obj, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            File.WriteAllText(fileName, obj.JsonSerialize());
        }
    }
}