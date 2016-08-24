using NEHO.Baseball.API.Helpers;
using NEHO.Baseball.DTO;

using PagedList;

namespace NEHO.Baseball.WebClient.Models
{
    public class PlayersViewModel
    {
        public IPagedList<Player> Players { get; set; }
        public IPagedList<Batter> Batters { get; set; }
        public IPagedList<Pitcher> Pitchers { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}