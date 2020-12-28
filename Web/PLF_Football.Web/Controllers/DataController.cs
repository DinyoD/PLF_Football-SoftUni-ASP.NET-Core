namespace PLF_Football.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PLF_Football.Common;
    using PLF_Football.Services;
    using PLF_Football.Services.Data;
    using PLF_Football.Services.Model;
    using PLF_Football.Web.ViewModels.Administration;
    using PLF_Football.Web.ViewModels.Fixtures;
    using PLF_Football.Web.ViewModels.Players;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class DataController : BaseController
    {
        private readonly IFixtureScraperService fixtureScraperService;
        private readonly IFixtureService fixtureService;
        private readonly IPlayersByClubScraperService playersScraperService;
        private readonly IPlayersService playersService;
        private readonly IPlayerStatsService statsService;
        private readonly IPlayersPointsService pointsService;

        public DataController(
            IFixtureScraperService fixtureScraperService,
            IFixtureService fixtureService,
            IPlayersByClubScraperService playersScraperService,
            IPlayersService playersService,
            IPlayerStatsService statsService,
            IPlayersPointsService pointsService)
        {
            this.fixtureScraperService = fixtureScraperService;
            this.fixtureService = fixtureService;
            this.playersScraperService = playersScraperService;
            this.playersService = playersService;
            this.statsService = statsService;
            this.pointsService = pointsService;
        }

        public async Task<IActionResult> Update()
        {
            var viewModel = await this.UpdateFixturesAndPlayersStatsAndPointsAsync();

            return this.View(viewModel);
        }

        public async Task<UpdateCountViewModel> UpdateFixturesAndPlayersStatsAndPointsAsync()
        {
            var nextMatchday = await this.fixtureScraperService.GetFirstNotStartedMatchdayAsync();
            var currMatchday = nextMatchday - 1;

            var pastOrCurrentMatchdayFixtures = await this.fixtureScraperService
                                                                    .GetFixturesOnAndBeforeMatchdayAsync(currMatchday);

            var pastOrCurrentMatchdayNotFinishedFixtureInDb =
                this.fixtureService
                       .GetFixturesOnAndBeforeSpecificMatchday<FixtureForUpdateDto>(currMatchday)
                       .Where(x => x.Finished == false)
                       .ToList();

            var updatedFixtureDto = this.GetAllChangedFixture(
                                                      pastOrCurrentMatchdayFixtures,
                                                      pastOrCurrentMatchdayNotFinishedFixtureInDb);
            var clubsIdAndMatchdayPairsInFixtureForUpdate = this.GetClubsIdAndMatchdayPairs(updatedFixtureDto);
            var clubsId = clubsIdAndMatchdayPairsInFixtureForUpdate.Keys;

            var totalUpdatedPlayers = 0;
            foreach (var clubId in clubsId)
            {
                var players = await this.playersScraperService.GetPlayersInClubNewStatsAsync(clubId);
                var updatedPlayers = players.Where(x => x.PremierLeagueRecordTotal.Appearances > 0).ToList();

                var playersForUpdate = this.playersService
                    .GetPlayersByClubId<PlayerForUpdateDto>(clubId);

                var updatedPlayersInClubCount = await this.UpdatePlayersAsync(updatedPlayers, playersForUpdate, clubsIdAndMatchdayPairsInFixtureForUpdate);

                totalUpdatedPlayers += updatedPlayersInClubCount;
            }

            if (updatedFixtureDto.Count > 0)
            {
                await this.fixtureService.UpdateFixtureAsync(updatedFixtureDto);
            }

            var nextMatcdayFixture = await this.fixtureScraperService.GetFixturesOnMatchdayAsync(nextMatchday);
            var nextMatcdayFixtureInDb = this.fixtureService
                .GetFixturesAfterSpecificAndBeforeOrOnNextMatchday<FixtureForUpdateDto>(nextMatchday, nextMatchday);

            var updatedFixtureTimeDtoList = this.UpdateNextMatchdayFixtureTime(nextMatcdayFixture, nextMatcdayFixtureInDb);
            await this.fixtureService.UpdateFixtureAsync(updatedFixtureTimeDtoList);

            var viewModel = new UpdateCountViewModel
            {
                UpdatedFixturesCount = updatedFixtureDto.Count,
                UpdatedPlayersCount = totalUpdatedPlayers,
            };

            return viewModel;
        }

        private ICollection<FixtureForUpdateDto> UpdateNextMatchdayFixtureTime(
            ICollection<FixtureDto> nextMatcdayFixture,
            ICollection<FixtureForUpdateDto> nextMatcdayFixtureInDb)
        {
            var updatedFixDto = new List<FixtureForUpdateDto>();

            foreach (var dbFix in nextMatcdayFixtureInDb)
            {
                var fixture = nextMatcdayFixture.Where(x => x.HomeTeamId == dbFix.HomeTeamId && x.AwayTeamId == dbFix.AwayTeamId).FirstOrDefault();
                dbFix.Result = fixture.Result;
                updatedFixDto.Add(dbFix);
            }

            return updatedFixDto;
        }

        private async Task<int> UpdatePlayersAsync(
            List<UpdatedPlayerDto> updatedPlayers,
            ICollection<PlayerForUpdateDto> playersForUpdate,
            IDictionary<int, int> clubsIdAndMatchdayPairsInFixtureForUpdate)
        {
            var playersInClubCount = 0;
            var playersWithPoints = new List<PlayersPointsInMatchdayDto>();
            foreach (var updatedPlayer in updatedPlayers)
            {
                var playerInDb = playersForUpdate
                    .Where(x => x.ClubId == updatedPlayer.ClubId && x.SquadNumber == updatedPlayer.SquadNumber)
                    .FirstOrDefault();

                if (playerInDb != null
                    && playerInDb.PlayerStats.Appearances < updatedPlayer.PremierLeagueRecordTotal.Appearances)
                {
                    var playerPoints = this.GetPlayerPoints(playerInDb, updatedPlayer);
                    playerInDb.Points = playerPoints;

                    await this.UpdatePlayerPointsAsync(playerInDb, clubsIdAndMatchdayPairsInFixtureForUpdate);
                    await this.UpdatePlayerStatsAsync(playerInDb, updatedPlayer);

                    playersInClubCount++;
                }
            }

            return playersInClubCount;
        }

        private int GetPlayerPoints(PlayerForUpdateDto playerInDb, UpdatedPlayerDto updatedPlayer)
        {
            var points = 0;

            if (updatedPlayer.PremierLeagueRecordTotal.Appearances > playerInDb.PlayerStats.Appearances)
            {
                points += 2;
            }

            if (updatedPlayer.PremierLeagueRecordTotal.Wins > playerInDb.PlayerStats.Wins)
            {
                points += 3;
            }
            else if (updatedPlayer.PremierLeagueRecordTotal.Losses == playerInDb.PlayerStats.Losses)
            {
                points += 1;
            }

            if (updatedPlayer.PremierLeagueRecordTotal.CleanSheets != null
                && updatedPlayer.PremierLeagueRecordTotal.CleanSheets > playerInDb.PlayerStats.CleanSheets)
            {
                points += 2;
            }

            if (updatedPlayer.PremierLeagueRecordTotal.Goals > playerInDb.PlayerStats.Goals)
            {
                points += (updatedPlayer.PremierLeagueRecordTotal.Goals - playerInDb.PlayerStats.Goals) * 3;
            }

            if (updatedPlayer.PremierLeagueRecordTotal.Assists > playerInDb.PlayerStats.Assists)
            {
                points += (updatedPlayer.PremierLeagueRecordTotal.Assists - playerInDb.PlayerStats.Assists) * 2;
            }

            if (updatedPlayer.PremierLeagueRecordTotal.YellowCards > playerInDb.PlayerStats.YellowCards)
            {
                points -= (updatedPlayer.PremierLeagueRecordTotal.YellowCards - playerInDb.PlayerStats.YellowCards) * 1;
            }

            if (updatedPlayer.PremierLeagueRecordTotal.RedCards > playerInDb.PlayerStats.RedCards)
            {
                points -= (updatedPlayer.PremierLeagueRecordTotal.RedCards - playerInDb.PlayerStats.RedCards) * 2;
            }

            return points;
        }

        private async Task UpdatePlayerStatsAsync(PlayerForUpdateDto playerInDb, UpdatedPlayerDto updatedPlayer)
        {
            playerInDb.PlayerStats.Appearances = updatedPlayer.PremierLeagueRecordTotal.Appearances;
            playerInDb.PlayerStats.Wins = updatedPlayer.PremierLeagueRecordTotal.Wins;
            playerInDb.PlayerStats.Losses = updatedPlayer.PremierLeagueRecordTotal.Losses;
            playerInDb.PlayerStats.CleanSheets = updatedPlayer.PremierLeagueRecordTotal.CleanSheets;
            playerInDb.PlayerStats.GoalsConceded = updatedPlayer.PremierLeagueRecordTotal.GoalsConceded;
            playerInDb.PlayerStats.Clearences = updatedPlayer.PremierLeagueRecordTotal.Clearences;
            playerInDb.PlayerStats.Tackles = updatedPlayer.PremierLeagueRecordTotal.Tackles;
            playerInDb.PlayerStats.Goals = updatedPlayer.PremierLeagueRecordTotal.Goals;
            playerInDb.PlayerStats.Assists = updatedPlayer.PremierLeagueRecordTotal.Assists;
            playerInDb.PlayerStats.YellowCards = updatedPlayer.PremierLeagueRecordTotal.YellowCards;
            playerInDb.PlayerStats.RedCards = updatedPlayer.PremierLeagueRecordTotal.RedCards;
            playerInDb.PlayerStats.Shots = updatedPlayer.PremierLeagueRecordTotal.Shots;
            playerInDb.PlayerStats.ShotsOnTarget = updatedPlayer.PremierLeagueRecordTotal.ShotsOnTarget;
            playerInDb.PlayerStats.BigChanceCreated = updatedPlayer.PremierLeagueRecordTotal.BigChanceCreated;
            playerInDb.PlayerStats.Fouls = updatedPlayer.PremierLeagueRecordTotal.Fouls;
            playerInDb.PlayerStats.Passes = updatedPlayer.PremierLeagueRecordTotal.Passes;

            await this.statsService.UpdateStatsAsync(playerInDb.PlayerStats);
        }

        private async Task UpdatePlayerPointsAsync(
            PlayerForUpdateDto playerInDb,
            IDictionary<int, int> clubsIdAndMatchdayPairsInFixtureForUpdate)
        {
            var clubId = playerInDb.ClubId;
            var matchday = clubsIdAndMatchdayPairsInFixtureForUpdate[clubId];

            var playersPointsInMatchdayDto = new PlayersPointsInMatchdayDto
            {
                Matchday = matchday,
                PlayerId = playerInDb.Id,
                Points = playerInDb.Points,
            };

            await this.pointsService.AddPlayersPointByFixtureAsync(playersPointsInMatchdayDto);
        }

        private ICollection<FixtureForUpdateDto> GetAllChangedFixture(
                                 ICollection<FixtureDto> pastOrCurrentMatchdayFixtures,
                                 ICollection<FixtureForUpdateDto> pastOrCurrentMatchdayNotFinishedFixtureInDb)
        {
            var updatedFixtureDto = new List<FixtureForUpdateDto>();
            foreach (var fixtureinDb in pastOrCurrentMatchdayNotFinishedFixtureInDb)
            {
                var fixture = pastOrCurrentMatchdayFixtures
                    .Where(x => x.HomeTeamId == fixtureinDb.HomeTeamId && x.AwayTeamId == fixtureinDb.AwayTeamId)
                    .FirstOrDefault();
                if (fixture.Result != fixtureinDb.Result || fixture.Started != fixtureinDb.Started || fixture.Finished != fixtureinDb.Finished)
                {
                    var fixtureForUpdateDto = new FixtureForUpdateDto
                    {
                        Id = fixtureinDb.Id,
                        AwayTeamId = fixtureinDb.AwayTeamId,
                        HomeTeamId = fixtureinDb.HomeTeamId,
                        Matchday = fixtureinDb.Matchday,
                        Result = fixture.Result,
                        Finished = fixture.Finished,
                        Started = fixture.Started,
                    };

                    updatedFixtureDto.Add(fixtureForUpdateDto);
                }
            }

            return updatedFixtureDto;
        }

        private IDictionary<int, int> GetClubsIdAndMatchdayPairs(ICollection<FixtureForUpdateDto> fixtureForUpdateDtoList)
        {
            var clubsIdAndMatchdayPairs = new Dictionary<int, int>();
            foreach (var fixture in fixtureForUpdateDtoList)
            {
                if (fixture.Finished == true)
                {
                    clubsIdAndMatchdayPairs[fixture.HomeTeamId] = fixture.Matchday;
                    clubsIdAndMatchdayPairs[fixture.AwayTeamId] = fixture.Matchday;
                }
            }

            return clubsIdAndMatchdayPairs;
        }
    }
}
