﻿namespace PLF_Football.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;
    using PLF_Football.Web.ViewModels.Players;

    public class PlayersPointsService : IPlayersPointsService
    {
        private readonly IDeletableEntityRepository<PlayerPointsByFixture> playersPointsRepo;

        public PlayersPointsService(IDeletableEntityRepository<PlayerPointsByFixture> playersPointsRepo)
        {
            this.playersPointsRepo = playersPointsRepo;
        }

        public async Task AddPlayersPointByFixtureAsync(PlayersPointsInMatchdayDto playersPointsDto)
        {
            var playersPoints = new PlayerPointsByFixture
            {
                Matchday = playersPointsDto.Matchday,
                PlayerId = playersPointsDto.PlayerId,
                Points = playersPointsDto.Points,
            };

            if (!this.playersPointsRepo.AllAsNoTracking().Any(x => x.PlayerId == playersPoints.PlayerId && x.Matchday == playersPoints.Matchday))
            {
                await this.playersPointsRepo.AddAsync(playersPoints);
                await this.playersPointsRepo.SaveChangesAsync();
            }
        }

        public ICollection<T> GetAllPointsByMatchdaysForPlayer<T>(int playerId)
        {
            return this.playersPointsRepo.AllAsNoTracking().Where(x => x.PlayerId == playerId).To<T>().ToList();
        }

        public int GetPointsByMatchdayAndPlayerIdCollection(int matchday, ICollection<int> playersId)
        {
            return this.playersPointsRepo.AllAsNoTracking()
               .Where(x => x.Matchday == matchday && playersId.Contains(x.PlayerId))
               .Sum(x => x.Points);
        }
    }
}
