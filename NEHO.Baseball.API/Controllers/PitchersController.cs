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
    public class PitchersController : ApiController
    {
        private readonly IPitcherRepository _pitcherRepository;
        readonly PitcherFactory _pitcherFactory = new PitcherFactory();

        public PitchersController()
        {
            _pitcherRepository = new PitcherRepository(new BaseballEntities());
        }

        public PitchersController(IPitcherRepository pitcherRepository)
        {
            _pitcherRepository = pitcherRepository;
        }

        public IHttpActionResult Get(string sort="id")
        {
            try
            {
                var pitchers = _pitcherRepository.GetPitchers();

                return Ok(pitchers.ToList().Select(p => _pitcherFactory.CreatePitcher(p)));
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
                var pitcher = _pitcherRepository.GetPitcher(MLBAM_ID);

                if (pitcher == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(_pitcherFactory.CreatePitcher(pitcher));
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] DTO.Pitcher pitcher)
        {
            try
            {
                if (pitcher == null)
                {
                    return BadRequest();
                }

                var createPitcher = _pitcherFactory.CreatePitcher(pitcher);
                var result = _pitcherRepository.InsertPitcher(createPitcher);

                if (result.Status == RepositoryActionStatus.Created)
                {
                    var newPitcher = _pitcherFactory.CreatePitcher(result.Entity);

                    return Created(Request.RequestUri + "/" + newPitcher.MLBAM_ID.ToString(), newPitcher);
                }

                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        public IHttpActionResult Put([FromBody] DTO.Pitcher pitcher)
        {
            try
            {
                if (pitcher == null)
                {
                    return BadRequest();
                }

                var createdPitcher = _pitcherFactory.CreatePitcher(pitcher);
                var result = _pitcherRepository.UpdatePitcher(createdPitcher);

                if (result.Status == RepositoryActionStatus.Updated)
                {
                    var updatedPitcher = _pitcherFactory.CreatePitcher(result.Entity);

                    return Created(Request.RequestUri + "/" + updatedPitcher.MLBAM_ID.ToString(), updatedPitcher);
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
                var result = _pitcherRepository.DeletePitcher(MLBAM_ID);

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
