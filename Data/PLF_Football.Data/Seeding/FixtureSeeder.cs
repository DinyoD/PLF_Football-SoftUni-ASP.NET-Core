namespace PLF_Football.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using PLF_Football.Services;

    public class FixtureSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Fixtures.Any())
            {
                return;
            }

            var fixturexScraperService = serviceProvider.GetRequiredService<IFixtureScraperService>();
            await fixturexScraperService.ImportFixture();
        }
    }
}
