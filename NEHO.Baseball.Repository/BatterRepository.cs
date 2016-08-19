using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace NEHO.Baseball.Repository
{
    public class BatterRepository : IBatterRepository
    {
        private readonly BaseballEntities _baseballEntities;

        public BatterRepository(BaseballEntities baseballEntities)
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

        public IQueryable<Batter> GetBatters()
        {
            return _baseballEntities.Batters;
        }

        public Batter GetBatter(int mlbamId)
        {
            return _baseballEntities.Batters.FirstOrDefault(b => b.MLBAM_ID == mlbamId);
        }

        public RepositoryActionResult<Batter> InsertBatter(Batter batter)
        {
            try
            {
                _baseballEntities.Batters.Add(batter);
                var result = _baseballEntities.SaveChanges();

                if (result > 0)
                {
                    return new RepositoryActionResult<Batter>(batter, RepositoryActionStatus.Created);
                }
                else
                {
                    return new RepositoryActionResult<Batter>(batter, RepositoryActionStatus.NothingModified, null);
                }
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<Batter>(null, RepositoryActionStatus.Error, ex);
            }
        }

        public RepositoryActionResult<Batter> UpdateBatter(Batter batter)
        {
            try
            {
                var existingBatter = _baseballEntities.Batters.FirstOrDefault(b => b.MLBAM_ID == batter.MLBAM_ID);

                if (existingBatter == null)
                {
                    return new RepositoryActionResult<Batter>(batter, RepositoryActionStatus.NotFound);
                }

                existingBatter.AtBats = batter.AtBats;
                existingBatter.Runs = batter.Runs;
                existingBatter.Hits = batter.Hits;
                existingBatter.RBI = batter.RBI;
                existingBatter.Singles = batter.Singles;
                existingBatter.Doubles = batter.Doubles;
                existingBatter.Triples = batter.Triples;
                existingBatter.Homeruns = batter.Homeruns;
                existingBatter.Walks = batter.Walks;
                existingBatter.Strikeouts = batter.Strikeouts;
                existingBatter.HitByPitch = batter.HitByPitch;
                existingBatter.StolenBases = batter.StolenBases;
                existingBatter.CaughtStealing = batter.CaughtStealing;
                existingBatter.Errors = batter.Errors;

                _baseballEntities.Batters.AddOrUpdate(existingBatter);

                var result = _baseballEntities.SaveChanges();
                if (result > 0)
                {
                    return new RepositoryActionResult<Batter>(batter, RepositoryActionStatus.Updated);
                }

                return new RepositoryActionResult<Batter>(batter, RepositoryActionStatus.NothingModified, null);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<Batter>(null, RepositoryActionStatus.Error, ex);
            }
        }

        public RepositoryActionResult<Batter> DeleteBatter(int id)
        {
            try
            {

                var batter = _baseballEntities.Batters.FirstOrDefault(b => b.MLBAM_ID == id);
                if (batter != null)
                {
                    // also remove all expenses linked to this expensegroup

                    _baseballEntities.Batters.Remove(batter);

                    _baseballEntities.SaveChanges();

                    return new RepositoryActionResult<Batter>(null, RepositoryActionStatus.Deleted);
                }
                return new RepositoryActionResult<Batter>(null, RepositoryActionStatus.NotFound);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<Batter>(null, RepositoryActionStatus.Error, ex);
            }
        }
    }
}