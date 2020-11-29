namespace Sandbox
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;

    using CommandLine;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using PLF_Football.Data;
    using PLF_Football.Data.Common;
    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;
    using PLF_Football.Data.Repositories;
    using PLF_Football.Data.Seeding;
    using PLF_Football.Services;
    using PLF_Football.Services.Messaging;

    public static class Program
    {
        public static int Main(string[] args)
        {
            //Console.WriteLine($"{typeof(Program).Namespace} ({string.Join(" ", args)}) starts working...");
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider(true);

            // Seed data on application startup
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                //dbContext.Database.Migrate();
                //new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();

                var players = dbContext.Players.Select(x => new
                {
                    Name = x.FirstName + " " + x.LastName,
                    Price = GetPlayerPrice(x.PlayerStats),
                }).ToList();

                Console.WriteLine(JsonSerializer.Serialize(players));
            }

            using (var serviceScope = serviceProvider.CreateScope())
            {
                serviceProvider = serviceScope.ServiceProvider;

                return Parser.Default.ParseArguments<SandboxOptions>(args).MapResult(
                    opts => SandboxCode(opts, serviceProvider).GetAwaiter().GetResult(),
                    _ => 255);
            }
        }

        private static int GetPlayerPrice(PlayerStats playerStats)
        {
            var price = 200_000;

            if (playerStats.Appearances > 6)
            {
                var bonus = (playerStats.Wins * 100 / playerStats.Appearances) * 40_000;
                price += bonus;
            }

            if (playerStats.CleanSheets == null && playerStats.Appearances > 0)
            {
                var bonus =
                    (playerStats.Goals * 100 / playerStats.Appearances * 60_000)
                    + (playerStats.Assists * 100 / playerStats.Appearances * 40_000);

                price += bonus;
            }

            if (playerStats.CleanSheets != null && playerStats.Appearances > 0)
            {
                var bonus =
                    (playerStats.CleanSheets * 100 / playerStats.Appearances * 30_000)
                    - (playerStats.GoalsConceded / playerStats.Appearances * 10_000);

                price += (int)bonus;
            }

            if (playerStats.Clearences != null && playerStats.Appearances > 5)
            {
                var bonus =
                    (playerStats.Clearences * 10 / playerStats.Appearances * 10_000)
                    + (playerStats.Tackles * 10 / playerStats.Appearances * 10_000);

                price += (int)bonus;
            }

            return price;
        }

        private static async Task<int> SandboxCode(SandboxOptions options, IServiceProvider serviceProvider)
        {
            var sw = Stopwatch.StartNew();

            Console.WriteLine(sw.Elapsed);
            return await Task.FromResult(0);
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                    .UseLoggerFactory(new LoggerFactory()));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender, NullMessageSender>();

        }
    }
}
