namespace WebApi.Services
{
    using Extensions;
    using Microsoft.Extensions.Caching.Memory;

    public sealed class CacheService : ICacheService
    {
        public CacheService(IMemoryCache memoryCacheStore, ILogger<CacheService> logger)
        {
            MemoryCacheStore = memoryCacheStore;
            Logger = logger;
        }

        private ILogger<CacheService> Logger { get; }

        private MemoryCacheEntryOptions MemoryCacheOptions { get; set; } = new() { Size = 1024 };

        private IMemoryCache MemoryCacheStore { get; }

        public T? Get<T>(string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentNullException(nameof(key));
                }

                var sucess = MemoryCacheStore.TryGetValue(key, out T value);
                if (!sucess)
                {
                    return default;
                }

                return value;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                Logger.LogDebug(ex.StackTrace);
            }

            return default;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentNullException(nameof(key));
                }

                return (T)await Task.Run(() => MemoryCacheStore.Get(key)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                Logger.LogDebug(ex.StackTrace);
            }

            return default;
        }

        public bool Remove(string key)
        {
            const bool Success = true;
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    return !Success;
                }

                MemoryCacheStore.Remove(key);
                return Success;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                Logger.LogDebug(ex.StackTrace);
            }

            return !Success;
        }

        public async Task<bool> RemoveAsync(string key)
        {
            const bool Success = true;
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    return !Success;
                }

                await Task.Run(() => MemoryCacheStore.Remove(key)).ConfigureAwait(false);
                return Success;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                Logger.LogDebug(ex.StackTrace);
            }

            return !Success;
        }

        public bool Set<T>(string key, T value, TimeSpan expirationTime)
        {
            const bool Success = true;
            MemoryCacheOptions = new MemoryCacheEntryOptions()
                .SetSize(2)
                .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                .SetPriority(CacheItemPriority.Normal);
            try
            {
                ArgumentNullException.ThrowIfNull(expirationTime);
                if (string.IsNullOrEmpty(key))
                {
                    return default;
                }

                MemoryCacheOptions.SetAbsoluteExpiration(expirationTime);
                MemoryCacheStore.Set(key, value, expirationTime);

                return Success;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                Logger.LogDebug(ex.StackTrace);
            }

            return !Success;
        }

        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan expirationTime)
        {
            const bool Success = true;
            MemoryCacheOptions = new MemoryCacheEntryOptions()
                .SetSize(2)
                .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                .SetPriority(CacheItemPriority.Normal);
            try
            {
                ArgumentNullException.ThrowIfNull(expirationTime);
                if (string.IsNullOrEmpty(key))
                {
                    return !Success;
                }

                MemoryCacheOptions.SetAbsoluteExpiration(expirationTime);
                _ = await Task.Run(() => MemoryCacheStore.Set(key, value, expirationTime)).ConfigureAwait(false);

                return Success;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToStringFormatted());
                throw;
            }
        }
    }
}