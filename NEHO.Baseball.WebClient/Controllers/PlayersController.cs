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
            var playersModel = new PlayersViewModel();

            var playersResponseMessage = await httpClient.GetAsync("api/players");
            var battersResponseMessage = await httpClient.GetAsync("api/batters");
            var pitchersResponseMessage = await httpClient.GetAsync("api/pitchers");

            if (playersResponseMessage.IsSuccessStatusCode && battersResponseMessage.IsSuccessStatusCode && pitchersResponseMessage.IsSuccessStatusCode)
            {
                var playersContent = await playersResponseMessage.Content.ReadAsStringAsync();
                var battersContent = await battersResponseMessage.Content.ReadAsStringAsync();
                var pitchersContent = await pitchersResponseMessage.Content.ReadAsStringAsync();

                var players = JsonConvert.DeserializeObject<IEnumerable<Player>>(playersContent);
                var batters = JsonConvert.DeserializeObject<IEnumerable<Batter>>(battersContent);
                var pitchers = JsonConvert.DeserializeObject<IEnumerable<Pitcher>>(pitchersContent);

                playersModel.Players = players;
                playersModel.Batters = batters;
                playersModel.Pitchers = pitchers;
            }
            else
            {
                return Content("An error ocurred while retrieving data.");
            }

            return View(playersModel);
        }
    }
}