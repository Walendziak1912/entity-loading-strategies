using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.Persistance.Common.Options;
using Shared.Persistance.Common.Providers;

namespace Shared.Persistance.Common.Extensions
{
    public static class DatabaseConfigurationExtensions
    {
        public static IServiceCollection AddDatabaseConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<DatabaseOptions>(configuration.GetSection("Database"));
            services.AddSingleton<IDatabaseOptionsProvider, DatabaseOptionsProvider>();
            services.AddScoped<ITestDataProvider, TestDataProvider>();
            return services;
        }

        public static IServiceCollection AddDatabaseConfiguration(
            this IServiceCollection services,
            IConfiguration configuration,
            string sectionName)
        {
            services.Configure<DatabaseOptions>(configuration.GetSection(sectionName));
            services.AddSingleton<IDatabaseOptionsProvider, DatabaseOptionsProvider>();
            return services;
        }
    }
}
