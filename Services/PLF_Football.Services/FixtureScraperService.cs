namespace PLF_Football.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AngleSharp;
    using PLF_Football.Common;
    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Model;

    public class FixtureScraperService : IFixtureScraperService
    {
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
            var allFixtures = await this.GetFixture(GlobalConstants.AllFixtureCount);
            foreach (var fixtureDto in allFixtures)
            {
                var fixture = new Fixture
                {
                    Matchday = fixtureDto.Matchday,
                    AwayTeamId = fixtureDto.AwayTeamId,
                    HomeTeamId = fixtureDto.HomeTeamId,
                    Result = fixtureDto.Result,
                    Started = fixtureDto.Started,
                    Finished = fixtureDto.Finished,
                };

                await this.fixtureRepo.AddAsync(fixture);
            }
        }

        public async Task<ICollection<FixtureDto>> GetFixture(int nextNotStartedMatchday)
        {
            var document = await this.context.OpenAsync(GlobalConstants.FixtureSource);

            var allFixtureDto = new List<FixtureDto>();

            for (int i = 1; i <= nextNotStartedMatchday; i++)
            {
                var idName = "jornada-" + i;

                var gameWeekFixture = document.QuerySelectorAll($"#{idName} tbody tr");

                foreach (var item in gameWeekFixture)
                {
                    var matchInfo = item.QuerySelectorAll($"td").Select(x => x.TextContent.Trim()).ToList();
                    var homeTeamName = this.FixClubName(matchInfo[0]);
                    var awayTeamname = this.FixClubName(matchInfo[2]);
                    var currMatch = new FixtureDto
                    {
                        Matchday = i,
                        Result = matchInfo[1],
                        HomeTeamId = this.clubRepo.All().FirstOrDefault(t => t.Name == homeTeamName).Id,
                        AwayTeamId = this.clubRepo.All().FirstOrDefault(t => t.Name == awayTeamname).Id,
                        Finished = item.QuerySelector(".col-resultado").GetAttribute("class").Contains("finalizado"),
                    };
                    currMatch.Started = currMatch.Finished == true ? true : !item.QuerySelector(".col-resultado").GetAttribute("class").Contains("no-comenzado");

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
