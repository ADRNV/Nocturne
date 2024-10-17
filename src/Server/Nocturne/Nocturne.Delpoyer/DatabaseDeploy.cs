using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Redis.OM;
using Redis.OM.Contracts;

namespace Nocturne.Delpoyer
{
    public static class Deploy
    {
        public static IServiceCollection AddConfig(this IServiceCollection services, string config)
        {
            IConfiguration _configuration = new ConfigurationBuilder()
                .AddJsonFile(config)
                .Build();

           return services.AddSingleton(_configuration);
        }
        public static IServiceCollection AddDbFromEF<T>(this IServiceCollection services) where T : DbContext
        {
            return services.AddDbContext<T>((sp, options) =>
            {
                options.UseSqlServer();
            });         
        }

        public static IServiceCollection DropDbFromEF<T>(this IServiceCollection services, IServiceProvider serviceProvider) where T : DbContext
        {
            serviceProvider
                .GetRequiredService<T>()
                .Database
                .EnsureDeleted();

            return services;
        }

        public static IServiceCollection CreateDbFromEF<T>(this IServiceCollection services, IServiceProvider serviceProvider) where T : DbContext
        {
            serviceProvider
                .GetRequiredService<T>()
                .Database
                .EnsureCreated();

            return services;
        }

        public static IServiceCollection AddCache(this IServiceCollection services, IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var redisConnectionProvider = new RedisConnectionProvider(configuration.GetConnectionString("RedisConnection")!);

            services.AddSingleton<IRedisConnectionProvider>(redisConnectionProvider);

            return services;
        }

        public static IServiceCollection ClearCache<T>(this IServiceCollection services, IServiceProvider serviceProvider, Type[] collections) where T : IRedisConnectionProvider
        {
            var connection = serviceProvider.GetRequiredService<T>().Connection;
            
            collections.ToList().ForEach(c =>
            {
                connection.DropIndexAndAssociatedRecords(c);
            });
            
            return services;
        }
    }
}
