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
        private readonly IDeletableEntityRepository<Fixture> fixtureRepo;
        private readonly IDeletableEntityRepository<Club> clubRepo;

        public FixtureScraperService(
            IDeletableEntityRepository<Fixture> fixtureRepo,
            IDeletableEntityRepository<Club> clubRepo)
        {
            var config = Configuration.Default.WithDefaultLoader();
            this.context = BrowsingContext.New(config);
            this.fixtureRepo = fixtureRepo;
            this.clubRepo = clubRepo;
        }

        public async Task ImportFixturesAsync()
        {
            var allFixtures = await this.GetFixturesOnAndBeforeMatchdayAsync(GlobalConstants.AllFixtureCount);
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

        public async Task<ICollection<FixtureDto>> GetFixturesOnAndBeforeMatchdayAsync(int matchday)
        {
            var document = await this.context.OpenAsync(GlobalConstants.FixtureSource);

            var allFixtureDto = new List<FixtureDto>();

            for (int i = 1; i <= matchday; i++)
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

        public async Task<ICollection<FixtureDto>> GetFixturesOnMatchdayAsync(int matchday)
        {
            var document = await this.context.OpenAsync(GlobalConstants.FixtureSource);

            var allFixtureDto = new List<FixtureDto>();

            var idName = "jornada-" + matchday;

            var gameWeekFixture = document.QuerySelectorAll($"#{idName} tbody tr");

            foreach (var item in gameWeekFixture)
            {
                var matchInfo = item.QuerySelectorAll($"td").Select(x => x.TextContent.Trim()).ToList();
                var homeTeamName = this.FixClubName(matchInfo[0]);
                var awayTeamname = this.FixClubName(matchInfo[2]);
                var currMatch = new FixtureDto
                {
                    Matchday = matchday,
                    Result = matchInfo[1],
                    HomeTeamId = this.clubRepo.All().FirstOrDefault(t => t.Name == homeTeamName).Id,
                    AwayTeamId = this.clubRepo.All().FirstOrDefault(t => t.Name == awayTeamname).Id,
                    Finished = item.QuerySelector(".col-resultado").GetAttribute("class").Contains("finalizado"),
                };
                currMatch.Started = currMatch.Finished == true ? true : !item.QuerySelector(".col-resultado").GetAttribute("class").Contains("no-comenzado");

                allFixtureDto.Add(currMatch);
            }

            return allFixtureDto;
        }

        public async Task<int> GetFirstNotStartedMatchdayAsync()
        {
            var document = await this.context.OpenAsync(GlobalConstants.FixtureSource);

            var matchdaysGames = new Dictionary<int, List<bool>>();

            for (int i = 1; i <= 38; i++)
            {
                var idName = "jornada-" + i;

                var gameWeekFixture = document.QuerySelectorAll($"#{idName} tbody tr");

                var matchsAreStarted = new List<bool>();

                foreach (var item in gameWeekFixture)
                {
                    var matchInfo = item.QuerySelectorAll($"td").Select(x => x.TextContent.Trim()).ToList();

                    var finished = item.QuerySelector(".col-resultado").GetAttribute("class").Contains("finalizado");
                    var started = finished == true || !item.QuerySelector(".col-resultado").GetAttribute("class").Contains("no-comenzado");

                    matchsAreStarted.Add(started);
                }

                matchdaysGames[i] = matchsAreStarted;
            }

            var matchday = 39;
            for (int i = 1; i <= 38; i++)
            {
                if (matchdaysGames[i].All(x => x == false))
                {
                    matchday = i;
                    break;
                }
            }

            return matchday;
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
