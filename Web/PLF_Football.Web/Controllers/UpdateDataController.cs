namespace PLF_Football.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using PLF_Football.Services;
    using PLF_Football.Services.Data;
    using PLF_Football.Services.Data.Models;
    using PLF_Football.Services.Model;

    public class UpdateDataController : BaseController
    {
        private readonly IFixtureScraperService fixtureScraperService;
        private readonly IFixtureService fixtureService;
        private readonly IPlayersByClubScraperService playersScraperService;
        private readonly IPlayersService playersService;

        public UpdateDataController(
            IFixtureScraperService fixtureScraperService,
            IFixtureService fixtureService,
            IPlayersByClubScraperService playersScraperService,
            IPlayersService playersService)
        {
            this.fixtureScraperService = fixtureScraperService;
            this.fixtureService = fixtureService;
            this.playersScraperService = playersScraperService;
            this.playersService = playersService;
        }

        public async Task<IActionResult> Update()
        {
            var nextNotStartedMatchday = await this.fixtureScraperService.GetFirstNotStartedMatchdayAsync();

            var allFixtureBeforeNextMatchday = await this.fixtureScraperService
                                                                    .GetFixturesAsync(nextNotStartedMatchday);

            var allFinishedOrStartedFixtureBeforeNextMatchday = allFixtureBeforeNextMatchday
                                                                    .Where(x => x.Finished == true
                                                                                || x.Started == true)
                                                                    .ToList();

            var allNotFinishedFixtureBeforeNextMatchdayInDb = this.fixtureService
                                                                    .GetFixtures<FixtureForUpdateDto>(nextNotStartedMatchday)
                                                                    .Where(x => x.Finished == false)
                                                                    .ToList();

            var allChangedStatusFixture = this.GetAllChangedFixture(
                                                      allFinishedOrStartedFixtureBeforeNextMatchday,
                                                      allNotFinishedFixtureBeforeNextMatchdayInDb);

            var newFinishedMatchesClubsId = await this.fixtureService.UpdateFixtureAsync(allChangedStatusFixture);

            var players = await this.playersScraperService.GetPlayersNewStatsAsync(newFinishedMatchesClubsId);
            var updatedPlayers = players.Where(x => x.PremierLeagueRecordTotal.Appearances > 0).ToList();

            var playersForUpdate = this.playersService.GetPlayersByClubsId<PlayerForUpdateDto>(newFinishedMatchesClubsId);

            this.UpdatePlayers(updatedPlayers, playersForUpdate);

            return this.Redirect("/");
        }

        private void UpdatePlayers(List<PlayerInfoForUpdateDto> updatedPlayers, ICollection<PlayerForUpdateDto> playersForUpdate)
        {
            throw new System.NotImplementedException();
        }

        private ICollection<FixtureForUpdateDto> GetAllChangedFixture(
                                 ICollection<FixtureDto> allFinishedOrStartedFixtureBeforeNextMatchday,                   ICollection<FixtureForUpdateDto> allNotFinishedFixtureBeforeNextMatchdayInDb)
        {
            var changedFixturesDto = new List<FixtureForUpdateDto>();
            foreach (var fixtureinDb in allNotFinishedFixtureBeforeNextMatchdayInDb)
            {
                foreach (var fixture in allFinishedOrStartedFixtureBeforeNextMatchday)
                {
                    if (fixture.AwayTeamId == fixtureinDb.AwayTeamId
                                               && fixture.HomeTeamId == fixtureinDb.HomeTeamId)
                    {
                        fixtureinDb.Started = fixture.Started;
                        fixtureinDb.Finished = fixture.Finished;
                    }

                    if (fixture.Finished)
                    {
                        fixtureinDb.Result = fixture.Result;
                    }
                }

                changedFixturesDto.Add(fixtureinDb);
            }

            return changedFixturesDto;
        }
    }
}
