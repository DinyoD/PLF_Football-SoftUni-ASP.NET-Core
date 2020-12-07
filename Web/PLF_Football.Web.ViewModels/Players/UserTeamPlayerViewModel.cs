namespace PLF_Football.Web.ViewModels.Players
{
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class UserTeamPlayerViewModel : PlayerBasicViewModel, IMapFrom<Player>
    {
    }
}
