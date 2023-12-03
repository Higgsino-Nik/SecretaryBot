using Microsoft.Extensions.Configuration;

namespace SecretaryBot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            var config = configuration.Build();
            var connectionString = config.GetConnectionString("ConnectionString");
            Console.WriteLine("Hello, World!");
        }
    }
}
