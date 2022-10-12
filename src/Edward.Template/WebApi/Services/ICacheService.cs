namespace WebApi.Services
{
    public interface ICacheService
    {
        /// <summary>
        ///     Get Data using key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        T? Get<T>(string key);

        Task<T?> GetAsync<T>(string key);

        /// <summary>
        ///     Remove Data
        /// </summary>
        /// <param name="key"></param>
        bool Remove(string key);

        Task<bool> RemoveAsync(string key);

        /// <summary>
        ///     Set Data with Key, Value and Expiration Time
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expirationTime"></param>
        bool Set<T>(string key, T value, TimeSpan expirationTime);

        Task<bool> SetAsync<T>(string key, T value, TimeSpan expirationTime);
    }
}