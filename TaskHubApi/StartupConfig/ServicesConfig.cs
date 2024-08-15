using AspNetCoreRateLimit;

namespace TaskHubApi.StartupConfig
{
    public static class ServicesConfig
    {
        public static void AddRateLimitServices(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection(key: "IpRateLimiting"));
            builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            builder.Services.AddInMemoryRateLimiting();
        }
    }
}
