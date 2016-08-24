using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.Http.Routing;

using Newtonsoft.Json;

using NEHO.Baseball.Constants;
using NEHO.Baseball.Repository;
using NEHO.Baseball.Repository.Factories;
using NEHO.Baseball.WebAPI.Helpers;

namespace NEHO.Baseball.WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PlayersController : ApiController
    {
        private readonly BaseballEntities _db = new BaseballEntities();
        readonly PlayerFactory _playerFactory = new PlayerFactory();
        const int MaxPageSize = 20;

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/players", Name = "PlayersList")]
        public IHttpActionResult GetPlayers(string sort = "mlbam_id", string lastName = null, int page = 1, int pageSize = 10)
        {
            var players = _db.Players.ApplySort(sort);

            if (lastName != null)
            {
                players = players.Where(p => p.LastName == lastName);
            }

            if (pageSize > MaxPageSize)
            {
                pageSize = MaxPageSize;
            }

            var totalPlayers = players.Count();
            var totalPages = (int)Math.Ceiling((double)totalPlayers / pageSize);

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

        // GET: api/Players/5
        [ResponseType(typeof(Player))]
        public async Task<IHttpActionResult> GetPlayer(int id)
        {
            var player = await _db.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        // PUT: api/Players/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPlayer(int id, Player player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != player.ID)
            {
                return BadRequest();
            }

            _db.Entry(player).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Players
        [ResponseType(typeof(Player))]
        public async Task<IHttpActionResult> PostPlayer([FromBody] Player player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Players.Add(player);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = player.ID }, player);
        }

        // DELETE: api/Players/5
        [ResponseType(typeof(Player))]
        public async Task<IHttpActionResult> DeletePlayer(int id)
        {
            var player = await _db.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            _db.Players.Remove(player);
            await _db.SaveChangesAsync();

            return Ok(player);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlayerExists(int id)
        {
            return _db.Players.Count(e => e.ID == id) > 0;
        }
    }
}