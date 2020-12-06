namespace PLF_Football.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AngleSharp;
    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Model;

    public class FixtureScraperService : IFixtureScraperService
    {
        private const string FixtureSource = "https://en.as.com/resultados/futbol/inglaterra/calendario/";
        private readonly IBrowsingContext context;
        private readonly IRepository<Fixture> fixtureRepo;
        private readonly IRepository<Club> clubRepo;

        public FixtureScraperService(
            IRepository<Fixture> fixtureRepo,
            IRepository<Club> clubRepo)
        {
            var config = Configuration.Default.WithDefaultLoader();
            this.context = BrowsingContext.New(config);
            this.fixtureRepo = fixtureRepo;
            this.clubRepo = clubRepo;
        }

        public async Task ImportFixture()
        {
            var allFixtures = await this.GetFixture();
            foreach (var fixtureDto in allFixtures)
            {
                var fixture = new Fixture
                {
                    Matchday = fixtureDto.Matchday,
                    AwayTeamId = fixtureDto.AwayTeamId,
                    HomeTeamId = fixtureDto.HomeTeamId,
                    Started = fixtureDto.Result.Contains(" - "),
                    Result = fixtureDto.Result,
                };

                await this.fixtureRepo.AddAsync(fixture);
            }
        }

        public async Task<ICollection<FixtureDto>> GetFixture()
        {
            var document = await this.context.OpenAsync(FixtureSource);
            var fixture = new Dictionary<int, List<string>>();
            for (int i = 1; i <= 38; i++)
            {
                var idName = "jornada-" + i;

                var gameWeekFixture = document
                    .QuerySelectorAll($"#{idName} tbody tr td")
                    .Select(x => x.TextContent
                    .Trim())
                    .ToList();
                fixture[i] = gameWeekFixture;
            }

            var allFixtureDto = new List<FixtureDto>();

            foreach (var matchday in fixture)
            {
                for (int i = 0; i < matchday.Value.Count; i += 3)
                {
                    var homeTeamName = this.FixClubName(matchday.Value[i]);
                    var awayTeamname = this.FixClubName(matchday.Value[i + 2]);

                    var currMatch = new FixtureDto
                    {
                        Matchday = matchday.Key,
                        HomeTeamId = this.clubRepo.All().FirstOrDefault(t => t.Name == homeTeamName).Id,
                        Result = matchday.Value[i + 1],
                        AwayTeamId = this.clubRepo.All().FirstOrDefault(t => t.Name == awayTeamname).Id,
                    };

                    allFixtureDto.Add(currMatch);
                }
            }

            return allFixtureDto;
        }

        private string FixClubName(string v)
        {
            if (v == "M. United")
            {
                return "Manchester United";
            }
            else if (v == "M. City")
            {
                return "Manchester City";
            }
            else if (v == "Brighton")
            {
                return "Brighton and Hove Albion";
            }
            else if (v == "Leeds")
            {
                return "Leeds United";
            }
            else if (v == "Leicester")
            {
                return "Leicester City";
            }
            else if (v == "Newcastle")
            {
                return "Newcastle United";
            }
            else if (v == "Sheffield Utd")
            {
                return "Sheffield United";
            }
            else if (v == "Wolves")
            {
                return "Wolverhampton Wanderers";
            }
            else if (v == "Tottenham")
            {
                return "Tottenham Hotspur";
            }
            else if (v == "WBA")
            {
                return "West Bromwich Albion";
            }
            else if (v == "West Ham")
            {
                return "West Ham United";
            }
            else
            {
                return v;
            }
        }
    }
}
