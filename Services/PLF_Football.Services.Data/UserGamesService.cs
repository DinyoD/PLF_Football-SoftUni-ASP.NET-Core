namespace PLF_Football.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

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

        public async Task AddPlayerToUserGameAsync(int playerId, int userGameId)
        {
            var userGame = this.userGamesRepo.All().Where(x => x.Id == userGameId).FirstOrDefault();
            var player = this.playersRepo.All().Where(x => x.Id == playerId).FirstOrDefault();
            if (userGame != null && player != null)
            {
                userGame.MatchdayTeam.Add(new PlayerUserGame { PlayerId = playerId, UserGameId = userGameId });
            }

            await this.userGamesRepo.SaveChangesAsync();
        }

        public async Task<int> CreateUserGameAsync(string userid, int nextMatchday)
        {
            var userGame = new UserGame { UserId = userid, Matchday = nextMatchday };
            await this.userGamesRepo.AddAsync(userGame);
            await this.userGamesRepo.SaveChangesAsync();
            return userGame.Id;
        }

        public int GetUserGameIdByUserAndMatchday(string userId, int matchday)
        {
            var userGame = this.userGamesRepo
                .All()
                .Where(x => x.UserId == userId && x.Matchday == matchday)
                .FirstOrDefault();
            return userGame != null ? userGame.Id : 0;
        }

        public T GetUserGame<T>(string userId, int matchday)
        {
            return this.userGamesRepo
                .AllAsNoTracking()
                .Where(x => x.UserId == userId && x.Matchday == matchday)
                .To<T>()
                .FirstOrDefault();
        }

        public ICollection<Player> GetUserGameMatchTeamPlayers(int userGameId)
        {
            var team = this.userGamesRepo
                .AllAsNoTracking()
                .Where(x => x.Id == userGameId)
                .Select(x => x.MatchdayTeam)
                .FirstOrDefault();

            return team.SelectMany(x => this.playersRepo.All().Where(y => y.Id == x.PlayerId))
                .ToList();
        }
    }
}
