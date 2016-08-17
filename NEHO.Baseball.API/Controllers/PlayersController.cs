using System;
using System.Linq;
using System.Web.Http;

using NEHO.Baseball.Repository;
using NEHO.Baseball.Repository.Factories;

namespace NEHO.Baseball.API.Controllers
{
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

        public IHttpActionResult Get(int id)
        {
            try
            {
                var player = _playerRepository.GetPlayer(id);

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
    }
}