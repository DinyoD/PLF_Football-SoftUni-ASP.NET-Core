namespace PLF_Football.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using PLF_Football.Services;

    public class PlayersSeasonStatsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var seasonStatsScraperSerice = serviceProvider.GetRequiredService<IPlayerUpdatedStatsScraperService>();
            foreach (var player in dbContext.Players)
            {
                await seasonStatsScraperSerice.GetPlayerUpdatedStats(player.Id);
            }
        }
    }
}
