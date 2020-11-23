namespace PLF_Football.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using PLF_Football.Services;

    public class ClubsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Clubs.Any())
            {
                return;
            }

            var clubsScraperService = serviceProvider.GetRequiredService<IPLClubsScraperService>();
            await clubsScraperService.ImportClubsInfoAsync();
        }
    }
}
