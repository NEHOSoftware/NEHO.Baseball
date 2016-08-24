using System.Collections.Generic;

using NEHO.Baseball.API.Helpers;
using NEHO.Baseball.DTO;

using PagedList;

namespace NEHO.Baseball.WebClient.Models
{
    public class PlayersViewModel
    {
        public IPagedList<Player> Players { get; set; }
        //public IEnumerable<Player> Players { get; set; }
        public IEnumerable<Player> Player { get; set; }
        public IPagedList<Batter> Batters { get; set; }
        public IEnumerable<Batter> Batter { get; set; }
        public IPagedList<Pitcher> Pitchers { get; set; }
        public IEnumerable<Pitcher> Pitcher { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}