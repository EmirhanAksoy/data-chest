using Alachisoft.NCache.Web.SessionState;
using StackExchange.Redis;

namespace data_chest_service.Configurations
{
    enum CacheType
    {
        None,
        RedisCache,
        NCache,
        MemoryCache,
        SqlServerCache,
    }

    public static class CacheConfig
    {
        /// <summary>
        /// App cache configuration.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configurationManager"></param>
        public static void AddCacheConfiguration(this IServiceCollection serviceCollection,ConfigurationManager? configurationManager)
        {
            string? cacheOption = configurationManager?.GetSection("Cache:Value")?.Value;

            CacheType cacheType = (CacheType)Enum.Parse(typeof(CacheType), cacheOption ?? CacheType.None.ToString());
            switch (cacheType)
            {
                case CacheType.RedisCache:
                    {
                        string? endPoint = configurationManager?.GetSection("Cache:RedisCache:Endpoint")?.Value;
                        string? portValue = configurationManager?.GetSection("Cache:RedisCache:Port")?.Value;
                        int port = int.TryParse(portValue, out var portVal) ? portVal : 8080;

                        serviceCollection.AddStackExchangeRedisCache(options =>
                        {
                            options.ConfigurationOptions = new ConfigurationOptions
                            {
                                EndPoints = { { endPoint, port } },
                                User = configurationManager?.GetSection("Cache:RedisCache:Username")?.Value,
                                Password = configurationManager?.GetSection("Cache:RedisCache:Password")?.Value
                            };
                            options.InstanceName = string.Empty;
                        });

                        break;
                    }
                case CacheType.NCache:
                    {
                        serviceCollection.AddNCacheSession(configurationManager?.GetSection("Cache:NCache"));
                        break;
                    }

                case CacheType.SqlServerCache:
                    {
                        serviceCollection.AddDistributedSqlServerCache(options =>
                        {
                            options.ConnectionString = configurationManager?.GetConnectionString("DBConnectionString");
                            options.SchemaName = configurationManager?.GetSection("Cache:SqlServerCache:Schema").Value;
                            options.TableName = configurationManager?.GetSection("Cache:SqlServerCache:Table").Value;
                        });
                        break;
                    }
                case CacheType.None:
                case CacheType.MemoryCache:
                    {
                        serviceCollection.AddDistributedMemoryCache();
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
