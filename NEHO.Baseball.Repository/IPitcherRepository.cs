using System.Linq;

namespace NEHO.Baseball.Repository
{
    public interface IPitcherRepository
    {
        IQueryable<Pitcher> GetPitchers();
        Pitcher GetPitcher(int mlbamid);
        RepositoryActionResult<Pitcher> InsertPitcher(Pitcher pitcher);
        RepositoryActionResult<Pitcher> UpdatePitcher(Pitcher pitcher);
        RepositoryActionResult<Pitcher> DeletePitcher(int mlbamid);
    }
}