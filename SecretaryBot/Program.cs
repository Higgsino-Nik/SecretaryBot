using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using SecretaryBot.Bll.Services;
using SecretaryBot.Dal;
using SecretaryBot.Dal.Repositories;
using SecretaryBot.Domain.Abstractions.Repositories;
using SecretaryBot.Domain.Abstractions.Services;

namespace SecretaryBot
{
    public static class Program
    {
        static void Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args)
                .UseWindowsService(options =>
                {
                    options.ServiceName = "SecretaryBotWorker";
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMapper();
                    services.AddMemoryCache();
                    services.ConfigureDbContext(hostContext.Configuration);
                    services.ConfigureDI();
                    services.AddHostedService<SecretaryBot>();
                })
                .Build();

            host.Run();
        }

        private static IServiceCollection ConfigureDI(this IServiceCollection services)
        {
            services.AddTransient<IDbLogger, DbLogger>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IPurchaseRepository, PurchaseRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ICacheService, CacheService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IPurchaseService, PurchaseService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<CommandsController>();
           return services;
        }

        private static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            var secretaryBotConnection = configuration.GetConnectionString("SecretaryBot");
            services.AddDbContext<IUserRepository, UserRepository>(options => options.UseNpgsql(secretaryBotConnection), optionsLifetime: ServiceLifetime.Transient, contextLifetime: ServiceLifetime.Transient);
            services.AddDbContext<IDbLogger, DbLogger>(options => options.UseNpgsql(secretaryBotConnection), optionsLifetime: ServiceLifetime.Transient, contextLifetime: ServiceLifetime.Transient);
            services.AddDbContext<ICategoryRepository, CategoryRepository>(options => options.UseNpgsql(secretaryBotConnection), optionsLifetime: ServiceLifetime.Transient, contextLifetime: ServiceLifetime.Transient);
            services.AddDbContext<IPurchaseRepository, PurchaseRepository>(options => options.UseNpgsql(secretaryBotConnection), optionsLifetime: ServiceLifetime.Transient, contextLifetime: ServiceLifetime.Transient);
            return services;
        }

        private static IServiceCollection AddMapper(this IServiceCollection services)
        {
            var mapper = new MapperConfiguration(cfg => new DalMapperConfigurator(cfg).AddConfiguration());
            services.AddSingleton(mapper.CreateMapper());
            return services;
        }
    }
}
