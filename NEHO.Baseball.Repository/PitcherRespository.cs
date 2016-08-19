using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace NEHO.Baseball.Repository
{
    public class PitcherRepository : IPitcherRepository
    {
        private readonly BaseballEntities _baseballEntities;

        public PitcherRepository(BaseballEntities baseballEntities)
        {
            _baseballEntities = baseballEntities;

            // Disable lazy loading - if not, related properties are auto-loaded when
            // they are accessed for the first time, which means they'll be included when
            // we serialize (b/c the serialization process accesses those properties).  
            // 
            // We don't want that, so we turn it off.  We want to eagerly load them (using Include)
            // manually.

            _baseballEntities.Configuration.LazyLoadingEnabled = false;
        }

        public IQueryable<Pitcher> GetPitchers()
        {
            return _baseballEntities.Pitchers;
        }

        public Pitcher GetPitcher(int mlbamId)
        {
            return _baseballEntities.Pitchers.FirstOrDefault(p => p.MLBAM_ID == mlbamId);
        }

        public RepositoryActionResult<Pitcher> InsertPitcher(Pitcher pitcher)
        {
            try
            {
                _baseballEntities.Pitchers.Add(pitcher);

                var result = _baseballEntities.SaveChanges();

                if (result > 0)
                {
                    return new RepositoryActionResult<Pitcher>(pitcher, RepositoryActionStatus.Created);
                }

                return new RepositoryActionResult<Pitcher>(pitcher, RepositoryActionStatus.NothingModified, null);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<Pitcher>(null, RepositoryActionStatus.Error, ex);
            }
        }

        public RepositoryActionResult<Pitcher> UpdatePitcher(Pitcher pitcher)
        {
            try
            {
                var existingPitcher = _baseballEntities.Pitchers.FirstOrDefault(p => p.MLBAM_ID == pitcher.MLBAM_ID && p.Year == pitcher.Year);

                if (existingPitcher == null)
                {
                    return new RepositoryActionResult<Pitcher>(pitcher, RepositoryActionStatus.NotFound);
                }

                existingPitcher.Games = pitcher.Games;
                existingPitcher.GamesStarted = pitcher.GamesStarted;
                existingPitcher.Wins = pitcher.Wins;
                existingPitcher.Losses = pitcher.Losses;
                existingPitcher.Saves = pitcher.Saves;
                existingPitcher.InningsPitched = pitcher.InningsPitched;
                existingPitcher.Hits = pitcher.Hits;
                existingPitcher.HomerunsAllowed = pitcher.HomerunsAllowed;
                existingPitcher.Walks = pitcher.Walks;
                existingPitcher.Strikeouts = pitcher.Strikeouts;
                existingPitcher.HitBatters = pitcher.HitBatters;
                existingPitcher.Runs = pitcher.Runs;
                existingPitcher.EarnedRuns = pitcher.EarnedRuns;

                _baseballEntities.Pitchers.AddOrUpdate(existingPitcher);

                var result = _baseballEntities.SaveChanges();
                if (result > 0)
                {
                    return new RepositoryActionResult<Pitcher>(pitcher, RepositoryActionStatus.Updated);
                }

                return new RepositoryActionResult<Pitcher>(pitcher, RepositoryActionStatus.NothingModified, null);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<Pitcher>(null, RepositoryActionStatus.Error, ex);
            }
        }

        public RepositoryActionResult<Pitcher> DeletePitcher(int id)
        {
            try
            {

                var pitcher = _baseballEntities.Pitchers.FirstOrDefault(p => p.MLBAM_ID == id);
                if (pitcher != null)
                {
                    // also remove all expenses linked to this expensegroup

                    _baseballEntities.Pitchers.Remove(pitcher);

                    _baseballEntities.SaveChanges();

                    return new RepositoryActionResult<Pitcher>(null, RepositoryActionStatus.Deleted);
                }
                return new RepositoryActionResult<Pitcher>(null, RepositoryActionStatus.NotFound);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<Pitcher>(null, RepositoryActionStatus.Error, ex);
            }
        }
    }
}