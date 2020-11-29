namespace PLF_Football.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AngleSharp;
    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;

    public class CountryScraperService : ICountryScraperService
    {
        private readonly IRepository<Country> countryRepo;
        private readonly IRepository<Club> clubsRepo;
        private readonly IBrowsingContext context;

        public CountryScraperService(IRepository<Country> countryRepo, IRepository<Club> clubsRepo)
        {
            var config = Configuration.Default.WithDefaultLoader();
            this.context = BrowsingContext.New(config);
            this.countryRepo = countryRepo;
            this.clubsRepo = clubsRepo;
        }

        public async Task ImportPlayersCountry()
        {
            var countriesAll = new List<Country>();

            foreach (var club in this.clubsRepo.AllAsNoTracking())
            {
                var countriesByClub = await this.ImportPlayersCountry(club);

                foreach (var country in countriesByClub)
                {
                    if (!countriesAll.Any(x => x.Name == country.Name))
                    {
                        countriesAll.Add(country);
                    }
                }
            }

            foreach (var country in countriesAll)
            {
                await this.countryRepo.AddAsync(country);
            }
        }

        public async Task<ICollection<Country>> ImportPlayersCountry(Club club)
        {
            var clubSquadPage = await this.context.OpenAsync(club.PLSquadLink);
            var playersNationality = clubSquadPage.QuerySelectorAll(".squadPlayerStats");
            var counries = new List<Country>();
            foreach (var item in playersNationality)
            {
                var country = new Country();
                var name = item.QuerySelector(".playerCountry")?.TextContent;
                country.Name = name;
                var code = item.QuerySelector(".flag").GetAttribute("class").Replace("flag ", string.Empty);
                country.FlagCode = code;

                if (!counries.Any(x => x.Name == country.Name))
                {
                    counries.Add(country);
                }
            }

            return counries;
        }
    }
}
