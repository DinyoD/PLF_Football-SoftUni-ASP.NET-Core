namespace PLF_Football.Web
{
    using System.Reflection;

    using Hangfire;
    using Hangfire.Dashboard;
    using Hangfire.MemoryStorage;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using PLF_Football.Common;
    using PLF_Football.Data;
    using PLF_Football.Data.Common;
    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;
    using PLF_Football.Data.Repositories;
    using PLF_Football.Data.Seeding;
    using PLF_Football.Services;
    using PLF_Football.Services.Data;
    using PLF_Football.Services.Mapping;
    using PLF_Football.Services.Messaging;
    using PLF_Football.Web.Controllers;
    using PLF_Football.Web.Hubs;
    using PLF_Football.Web.ViewModels;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectonString = this.configuration.GetConnectionString("DefaultConnection");

            services.AddHangfire(config => config
                                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                                 .UseSimpleAssemblyNameTypeSerializer()
                                 .UseDefaultTypeSerializer()
                                 .UseMemoryStorage());

            services.AddHangfireServer();

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(connectonString));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });
            services.AddSignalR();
            services.AddControllersWithViews(
                options =>
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }).AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender, NullMessageSender>();

            services.AddTransient<IPLClubsScraperService, PLClubsScraperService>();
            services.AddTransient<IFixtureScraperService, FixtureScraperService>();
            services.AddTransient<IPlayersByClubScraperService, PlayersByClubScraperService>();
            services.AddTransient<ICountryScraperService, CountryScraperService>();

            services.AddTransient<IClubsService, ClubsService>();
            services.AddTransient<IPlayersService, PlayersService>();
            services.AddTransient<IUserGamesService, UserGamesService>();
            services.AddTransient<IFixtureService, FixtureService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IPlayerStatsService, PlayerStatsService>();
            services.AddTransient<IPlayersPointsService, PlayersPointsService>();
            services.AddTransient<IPositionService, PositionService>();
            services.AddTransient<IMessagesService, MessagesService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager recurringJobManager)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();

                dbContext.Database.Migrate();

                new ApplicationDbContextSeeder()
                    .SeedAsync(dbContext, serviceScope.ServiceProvider)
                    .GetAwaiter()
                    .GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireDashboard(
                   "/Hangfire",
                   new DashboardOptions { Authorization = new[] { new HangfireAuthorizationFilter() } });

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapHub<ChatHub>("/Chat");
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });

            recurringJobManager.AddOrUpdate<DataController>(
                                                         "DataUpdate",
                                                         x => x.UpdateFixturesAndPlayersStatsAndPointsAsync(),
                                                         "*/17 * * * *");
        }

        private class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                var httpContext = context.GetHttpContext();
                return httpContext.User.IsInRole(GlobalConstants.AdministratorRoleName);
            }
        }
    }
}
