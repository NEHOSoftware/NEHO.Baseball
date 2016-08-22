using System.Collections.Generic;

using NEHO.Baseball.DTO;

namespace NEHO.Baseball.WebClient.Models
{
    public class PlayersViewModel
    {
        public IEnumerable<Player> Players { get; set; }
        public IEnumerable<Batter> Batters { get; set; }
        public IEnumerable<Pitcher> Pitchers { get; set; }
    }
}