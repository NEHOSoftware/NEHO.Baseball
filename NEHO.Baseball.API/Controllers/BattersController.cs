using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;

using NEHO.Baseball.Repository;
using NEHO.Baseball.Repository.Factories;

namespace NEHO.Baseball.API.Controllers
{
    [EnableCors("*", "*", "GET,POST,PUT,DELETE")]
    public class BattersController : ApiController
    {
        private readonly IBatterRepository _batterRepository;
        readonly BatterFactory _batterFactory = new BatterFactory();

        public BattersController()
        {
            _batterRepository = new BatterRepository(new BaseballEntities());
        }

        public BattersController(IBatterRepository batterRepository)
        {
            _batterRepository = batterRepository;
        }

        public IHttpActionResult Get()
        {
            try
            {
                var batters = _batterRepository.GetBatters();

                return Ok(batters.ToList().Select(b => _batterFactory.CreateBatter(b)));
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        public IHttpActionResult Get(int MLBAM_ID)
        {
            try
            {
                var batter = _batterRepository.GetBatter(MLBAM_ID);

                if (batter == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(_batterFactory.CreateBatter(batter));
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] DTO.Batter batter)
        {
            try
            {
                if (batter == null)
                {
                    return BadRequest();
                }

                var createBatter = _batterFactory.CreateBatter(batter);
                var result = _batterRepository.InsertBatter(createBatter);

                if (result.Status == RepositoryActionStatus.Created)
                {
                    var newBatter = _batterFactory.CreateBatter(result.Entity);

                    return Created(Request.RequestUri + "/" + newBatter.MLBAM_ID.ToString(), newBatter);
                }

                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        public IHttpActionResult Put([FromBody] DTO.Batter batter)
        {
            try
            {
                if (batter == null)
                {
                    return BadRequest();
                }

                var createdBatter = _batterFactory.CreateBatter(batter);
                var result = _batterRepository.UpdateBatter(createdBatter);

                if (result.Status == RepositoryActionStatus.Updated)
                {
                    var updatedBatter = _batterFactory.CreateBatter(result.Entity);

                    return Created(Request.RequestUri + "/" + updatedBatter.MLBAM_ID.ToString(), updatedBatter);
                }

                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        //[HttpPatch]
        //public IHttpActionResult Patch(int MLBAM_ID,
        //    [FromBody]JsonPatchDocument<DTO.Player> playerPatchDocument)
        //{
        //    try
        //    {
        //        if (playerPatchDocument == null)
        //        {
        //            return BadRequest();
        //        }

        //        var player = _playerRepository.GetPlayer(MLBAM_ID);
        //        if (player == null)
        //        {
        //            return NotFound();
        //        }

        //        // map
        //        var createPlayer = _playerFactory.CreatePlayer(player);

        //        // apply changes to the DTO
        //        playerPatchDocument.ApplyTo(createPlayer);

        //        // map the DTO with applied changes to the entity, & update
        //        var result = _playerRepository.UpdatePlayer(_playerFactory.CreatePlayer(createPlayer));

        //        if (result.Status == RepositoryActionStatus.Updated)
        //        {
        //            // map to dto
        //            var patchedPlayer = _playerFactory.CreatePlayer(result.Entity);

        //            return Ok(patchedPlayer);
        //        }

        //        return BadRequest();
        //    }
        //    catch (Exception)
        //    {
        //        return InternalServerError();
        //    }
        //}

        public IHttpActionResult Delete(int MLBAM_ID)
        {
            try
            {
                var result = _batterRepository.DeleteBatter(MLBAM_ID);

                switch (result.Status)
                {
                    case RepositoryActionStatus.Deleted:
                        return StatusCode(HttpStatusCode.NoContent);
                    case RepositoryActionStatus.NotFound:
                        return NotFound();
                }

                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}