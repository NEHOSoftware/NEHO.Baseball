using System.Linq;

namespace NEHO.Baseball.Repository
{
    public interface IBatterRepository
    {
        IQueryable<Batter> GetBatters();
        Batter GetBatter(int mlbamid);
        RepositoryActionResult<Batter> InsertBatter(Batter batter);
        RepositoryActionResult<Batter> UpdateBatter(Batter batter);
        RepositoryActionResult<Batter> DeleteBatter(int mlbamid);
    }
}