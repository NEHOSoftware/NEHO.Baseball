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
    public class PlayersController : ApiController
    {
        private readonly IPlayerRepository _playerRepository;
        readonly PlayerFactory _playerFactory = new PlayerFactory();

        public PlayersController()
        {
            _playerRepository = new PlayerRepository(new BaseballEntities());
        }

        public PlayersController(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public IHttpActionResult Get()
        {
            try
            {
                var players = _playerRepository.GetPlayers();

                return Ok(players.ToList().Select(p => _playerFactory.CreatePlayer(p)));
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
                var player = _playerRepository.GetPlayer(MLBAM_ID);

                if (player == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(_playerFactory.CreatePlayer(player));
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] DTO.Player player)
        {
            try
            {
                if (player == null)
                {
                    return BadRequest();
                }

                var createPlayer = _playerFactory.CreatePlayer(player);
                var result = _playerRepository.InsertPlayer(createPlayer);

                if (result.Status == RepositoryActionStatus.Created)
                {
                    var newPlayer = _playerFactory.CreatePlayer(result.Entity);

                    return Created(Request.RequestUri + "/" + newPlayer.MLBAM_ID.ToString(), newPlayer);
                }

                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        public IHttpActionResult Put([FromBody] DTO.Player player)
        {
            try
            {
                if (player == null)
                {
                    return BadRequest();
                }

                var createdPlayer = _playerFactory.CreatePlayer(player);
                var result = _playerRepository.UpdatePlayer(createdPlayer);

                if (result.Status == RepositoryActionStatus.Updated)
                {
                    var updatedPlayer = _playerFactory.CreatePlayer(result.Entity);

                    return Created(Request.RequestUri + "/" + updatedPlayer.MLBAM_ID.ToString(), updatedPlayer);
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
                var result = _playerRepository.DeletePlayer(MLBAM_ID);

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