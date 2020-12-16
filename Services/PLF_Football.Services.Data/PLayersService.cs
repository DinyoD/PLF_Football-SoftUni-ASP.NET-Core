﻿namespace PLF_Football.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class PlayersService : IPlayersService
    {
        private readonly IDeletableEntityRepository<Player> playersRepo;

        public PlayersService(IDeletableEntityRepository<Player> playersRepo)
        {
            this.playersRepo = playersRepo;
        }

        public ICollection<T> GetAll<T>()
        {
            return this.playersRepo
                .All()
                .To<T>()
                .ToList();
        }

        public int GetCount()
        {
            return this.playersRepo.All().Count();
        }

        public T GetPlayerById<T>(int? playerId)
        {
            return this.playersRepo.AllAsNoTracking().Where(x => x.Id == playerId).To<T>().FirstOrDefault();
        }

        public Player GetPlayerById(int playerId)
        {
            return this.playersRepo.All().Where(x => x.Id == playerId).FirstOrDefault();
        }

        public ICollection<T> GetPlayersByClubsId<T>(ICollection<int> clubsIds)
        {
            return this.playersRepo.All().Where(x => clubsIds.Contains(x.ClubId)).To<T>().ToList();
        }

        public ICollection<T> GetPlayersBySearchingString<T>(string searchString)
        {
            return this.playersRepo.All()
                                   .Where(s => (s.FirstName.ToLower() + s.LastName.ToLower()).Contains(searchString)
                                     || s.Club.Name.ToLower().Contains(searchString))
                                   .To<T>()
                                   .ToList();
        }

        public T GetPlayerStatsbyId<T>(int playerId)
        {
            return this.playersRepo.AllAsNoTracking()
                .Where(x => x.Id == playerId)
                .Select(x => x.PlayerStats)
                .To<T>()
                .FirstOrDefault();
        }

        public async Task UpdatePlayerPrice(int id, int price)
        {
            var player = this.playersRepo.All().Where(x => x.Id == id).FirstOrDefault();
            if (player != null)
            {
                player.Price = price;
                await this.playersRepo.SaveChangesAsync();
            }
        }
    }
}
