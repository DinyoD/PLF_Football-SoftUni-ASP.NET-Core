namespace PLF_Football.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using PLF_Football.Data.Models;
    using PLF_Football.Services;

    public class CountrySeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Countries.Any())
            {
                return;
            }

            var countryScraperService = serviceProvider
                .GetRequiredService<ICountryScraperService>();

            await countryScraperService.ImportPlayersCountry();
        }
    }
}
