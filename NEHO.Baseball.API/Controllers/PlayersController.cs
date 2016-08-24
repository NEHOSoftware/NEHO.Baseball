using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Routing;

using Newtonsoft.Json;

using NEHO.Baseball.API.Helpers;
using NEHO.Baseball.Repository;
using NEHO.Baseball.Repository.Factories;

namespace NEHO.Baseball.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PlayersController : ApiController
    {
        private readonly IPlayerRepository _playerRepository;
        readonly PlayerFactory _playerFactory = new PlayerFactory();
        const int MaxPageSize = 20;

        public PlayersController()
        {
            _playerRepository = new PlayerRepository(new BaseballEntities());
        }

        public PlayersController(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        [Route("api/players", Name = "PlayersList")]
        public IHttpActionResult Get(string sort = "mlbam_id", string lastName = null, int page = 1, int pageSize = 10)
        {
            try
            {
                var players = _playerRepository.GetPlayers().ApplySort(sort);

                if (lastName != null)
                {
                    players = players.Where(p => p.LastName == lastName);
                }

                if (pageSize > MaxPageSize)
                {
                    pageSize = MaxPageSize;
                }

                var totalPlayers = players.Count();
                var totalPages = (int) Math.Ceiling((double) totalPlayers/pageSize);

                var urlHelper = new UrlHelper(Request);
                var previousLink = page > 1
                    ? urlHelper.Link("PlayersList",
                        new
                        {
                            page = page - 1,
                            pageSize = pageSize,
                            sort = sort,
                            lastName = lastName
                        })
                    : "";

                var nextLink = page < totalPages
                    ? urlHelper.Link("PlayersList",
                        new
                        {
                            page = page + 1,
                            pageSize = pageSize,
                            sort = sort,
                            lastName = lastName
                        })
                    : "";

                var paginationHeader = new
                {
                    currentPage = page,
                    pageSize = pageSize,
                    totalPlayers = totalPlayers,
                    totalPages = totalPages,
                    previousPageLink = previousLink,
                    nextPageLink = nextLink
                };

                HttpContext.Current.Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationHeader));

                return Ok(players.Skip(pageSize * (page - 1)).Take(pageSize).ToList().Select(p => _playerFactory.CreatePlayer(p)));
            }
            catch (Exception ex)
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