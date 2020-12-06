namespace PLF_Football.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;

    public class UserGamesService : IUserGamesService
    {
        private readonly IDeletableEntityRepository<UserGame> userGamesRepo;
        private readonly IDeletableEntityRepository<Player> playersRepo;

        public UserGamesService(
            IDeletableEntityRepository<UserGame> userGamesRepo,
            IDeletableEntityRepository<Player> playersRepo)
        {
            this.userGamesRepo = userGamesRepo;
            this.playersRepo = playersRepo;
        }

        public async Task<bool> AddPlayerToUserGameAsync(int playerId)
        {
            bool success = false;
            var userGame = this.userGamesRepo.All().OrderByDescending(x => x.Id).FirstOrDefault();
            var player = this.playersRepo.All().Where(x => x.Id == playerId).FirstOrDefault();

            if (userGame != null && userGame.MatchdayTeam.Count < 11 && player != null)
            {
                userGame.MatchdayTeam.Add(player);
                await this.userGamesRepo.SaveChangesAsync();
                success = true;
            }

            return success;
        }

        public async Task CreateUserGameAsync(string userid, int nextMatchday)
        {
            await this.userGamesRepo.AddAsync(new UserGame { UserId = userid, Matchday = nextMatchday });
            await this.userGamesRepo.SaveChangesAsync();
        }

        public int GetUserLastMatchdayInUserGames(string userId)
        {
            return this.userGamesRepo
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .Max(x => x.Matchday);
        }

        public ICollection<Player> GetPlayersInLastTeam(string userId)
        {
            return this.userGamesRepo
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Matchday)
                .Select(x => x.MatchdayTeam)
                .FirstOrDefault();
        }
    }
}
