namespace PLF_Football.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using PLF_Football.Services;

    public class PlayersByClubSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            foreach (var club in dbContext.Clubs)
            {
                if (dbContext.Players.Where(x => x.ClubId == club.Id).Any())
                {
                    return;
                }

                var playersScraperService = serviceProvider
                    .GetRequiredService<IPlayersByClubScraperService>();

                await playersScraperService.ImportPlayersInfoAsync(club);
            }
        }
    }
}
