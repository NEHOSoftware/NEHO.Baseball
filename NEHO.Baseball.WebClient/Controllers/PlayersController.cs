using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

using Newtonsoft.Json;

using NEHO.Baseball.DTO;
using NEHO.Baseball.WebClient.Helpers;
using NEHO.Baseball.WebClient.Models;

namespace NEHO.Baseball.WebClient.Controllers
{
    public class PlayersController : Controller
    {
        // GET: Players
        public async Task<ActionResult> Index()
        {
            var httpClient = BaseballHttpClient.GetClient();

            var battersResponse = await httpClient.GetAsync("api/batters");

            if (battersResponse.IsSuccessStatusCode)
            {
                var battersContent = await battersResponse.Content.ReadAsStringAsync();
                var batters = JsonConvert.DeserializeObject<IEnumerable<Batter>>(battersContent);
            }
            else
            {
                return Content("An error ocurred.");
            }

            var playersModel = new PlayersViewModel();

            var httpResponseMessage = await httpClient.GetAsync("api/players");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var content = await httpResponseMessage.Content.ReadAsStringAsync();
                var players = JsonConvert.DeserializeObject<IEnumerable<Player>>(content);

                playersModel.Players = players; 
            }
            else
            {
                return Content("An error ocurred.");
            }

            return View(playersModel);
        }
    }
}