using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Xml.Linq;

namespace NEHO.Baseball.Repository
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly BaseballEntities _baseballEntities;

        public PlayerRepository(BaseballEntities baseballEntities)
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

        public IQueryable<Player> GetPlayers()
        {

            return _baseballEntities.Players;
        }

        public Player GetPlayer(int mlbamId)
        {
            return _baseballEntities.Players.FirstOrDefault(p => p.MLBAM_ID == mlbamId);
        }

        public RepositoryActionResult<Player> InsertPlayer(Player player)
        {
            try
            {
                _baseballEntities.Players.Add(player);
                var result = _baseballEntities.SaveChanges();

                if (result > 0)
                {
                    return new RepositoryActionResult<Player>(player, RepositoryActionStatus.Created);
                }
                else
                {
                    return new RepositoryActionResult<Player>(player, RepositoryActionStatus.NothingModified, null);
                }
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<Player>(null, RepositoryActionStatus.Error, ex);
            }
        }

        public RepositoryActionResult<Player> UpdatePlayer(Player player)
        {
            try
            {
                var existingPlayer = _baseballEntities.Players.FirstOrDefault(p => p.MLBAM_ID == player.MLBAM_ID);

                if (existingPlayer == null)
                {
                    return new RepositoryActionResult<Player>(player, RepositoryActionStatus.NotFound);
                }

                existingPlayer.FirstName = player.FirstName;
                existingPlayer.LastName = player.LastName;

                _baseballEntities.Players.AddOrUpdate(existingPlayer);

                var result = _baseballEntities.SaveChanges();
                if (result > 0)
                {
                    return new RepositoryActionResult<Player>(player, RepositoryActionStatus.Updated);
                }

                return new RepositoryActionResult<Player>(player, RepositoryActionStatus.NothingModified, null);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<Player>(null, RepositoryActionStatus.Error, ex);
            }
        }

        public RepositoryActionResult<Player> DeletePlayer(int id)
        {
            try
            {

                var player = _baseballEntities.Players.FirstOrDefault(p => p.MLBAM_ID == id);
                if (player != null)
                {
                    // also remove all expenses linked to this expensegroup

                    _baseballEntities.Players.Remove(player);

                    _baseballEntities.SaveChanges();

                    return new RepositoryActionResult<Player>(null, RepositoryActionStatus.Deleted);
                }
                return new RepositoryActionResult<Player>(null, RepositoryActionStatus.NotFound);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<Player>(null, RepositoryActionStatus.Error, ex);
            }
        }
    }
}