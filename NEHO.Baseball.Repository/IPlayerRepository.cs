using System.Linq;

namespace NEHO.Baseball.Repository
{
    public interface IPlayerRepository
    {
        IQueryable<Player> GetPlayers();
        Player GetPlayer(int mlbamid);
        RepositoryActionResult<Player> InsertPlayer(Player player);
        RepositoryActionResult<Player> UpdatePlayer(Player player);
    }
}