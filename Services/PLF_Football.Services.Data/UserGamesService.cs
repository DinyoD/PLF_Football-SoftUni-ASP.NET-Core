namespace PLF_Football.Services.Data
{
    using System;
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

        public async Task<bool> AddPlayerToUserGame(int playerId, int userGameId)
        {
            bool sucsess = false;
            var userGame = this.userGamesRepo.All().Where(x => x.Id == userGameId).FirstOrDefault();
            var player = this.playersRepo.All().Where(x => x.Id == playerId).FirstOrDefault();

            if (userGame != null && userGame.MatchdayTeam.Count < 11 && player != null)
            {
                userGame.MatchdayTeam.Add(player);
                await this.userGamesRepo.SaveChangesAsync();
                sucsess = true;
            }

            return sucsess;
        }
    }
}
