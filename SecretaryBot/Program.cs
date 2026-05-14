using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SecretaryBot.Bll.Commands;
using SecretaryBot.Bll.Services;
using SecretaryBot.Dal;
using SecretaryBot.Dal.Repositories;
using SecretaryBot.Domain.Abstractions;
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
            services.AddScoped<ICustomLogger, DbLogger>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPurchaseService, PurchaseService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IReportService, ReportService>();

            services.AddScoped<CommandFactory>();
            var commandType = typeof(ICommand);
            var commandImplementations = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => commandType.IsAssignableFrom(x) && x.IsClass && !x.IsAbstract);

            foreach (var impl in commandImplementations)
            {
                services.AddScoped(commandType, impl);
                services.AddScoped(impl);
            }
                

            services.AddScoped<CommandsController>();
            return services;
        }

        private static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            var secretaryBotConnection = configuration.GetConnectionString("SecretaryBot");
            services.AddEntityFrameworkNpgsql().AddDbContext<PostgresContext>(options =>
                options.UseNpgsql(secretaryBotConnection));
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
