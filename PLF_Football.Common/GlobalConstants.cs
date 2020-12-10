namespace PLF_Football.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "Premier.League.Fan.Football";

        public const string AdministratorRoleName = "Administrator";

        public const string PremierLeagueLink = "https://www.premierleague.com/clubs";

        // owner consts
        public const string OwnerName = "Dinyo";

        public const string OwnerEmail = "dinyo517@gmail.com";

        public const string OwnerPassword = "Dinjo517";

        // club scraper consts
        public const string ClubOverview = "overview";

        public const string ClubStadium = "stadium";

        public const string ClubSquad = "squad";

        // player scraper consts
        public const string PlayerOverview = "overview";

        public const string PlayerTotalStats = "stats";

        public const string PlayerSeasonStatsFilter = "?co=1&se=363";

        public const string PlayerImgLink =
            "https://resources.premierleague.com/premierleague/photos/players/110x140/";

        // fixture scraper consts
        public const string FixtureSource = "https://en.as.com/resultados/futbol/inglaterra/calendario/";

        public const int AllFixtureCount = 38;

        // game consts
        public const int UserBudget = 40_000_000;
    }
}
