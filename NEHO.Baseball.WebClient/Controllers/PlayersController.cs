﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

using Newtonsoft.Json;

using NEHO.Baseball.API.Helpers;
using NEHO.Baseball.DTO;
using NEHO.Baseball.WebClient.Helpers;
using NEHO.Baseball.WebClient.Models;

using PagedList;

namespace NEHO.Baseball.WebClient.Controllers
{
    public class PlayersController : Controller
    {
        // GET: Players
        public async Task<ActionResult> Index(int? page = 1)
        {
            var httpClient = BaseballHttpClient.GetClient();
            var playersModel = new PlayersViewModel();

            var playersResponseMessage = await httpClient.GetAsync("api/players?sort=LastName, FirstName&page=" + page + "&pagesize = 10");
            var battersResponseMessage = await httpClient.GetAsync("api/batters");
            var pitchersResponseMessage = await httpClient.GetAsync("api/pitchers");

            if (playersResponseMessage.IsSuccessStatusCode)
            {
                var playersContent = await playersResponseMessage.Content.ReadAsStringAsync();
                //var battersContent = await battersResponseMessage.Content.ReadAsStringAsync();
                //var pitchersContent = await pitchersResponseMessage.Content.ReadAsStringAsync();

                var pagingInfo = HeaderParser.FindAndParsePagingInfo(playersResponseMessage.Headers);

                var players = JsonConvert.DeserializeObject<IEnumerable<Player>>(playersContent);
                //var batters = JsonConvert.DeserializeObject<IEnumerable<Batter>>(battersContent);
                //var pitchers = JsonConvert.DeserializeObject<IEnumerable<Pitcher>>(pitchersContent);

                var pagedPlayersList = new StaticPagedList<Player>(players, pagingInfo.CurrentPage, pagingInfo.PageSize, pagingInfo.TotalPlayers);
                //var pagedBattersList = new StaticPagedList<Batter>(batters, pagingInfo.CurrentPage, pagingInfo.PageSize, pagingInfo.TotalPlayers);
                //var pagedPitchersList = new StaticPagedList<Pitcher>(pitchers, pagingInfo.CurrentPage, pagingInfo.PageSize, pagingInfo.TotalPlayers);

                playersModel.Players = pagedPlayersList;
                //playersModel.Batters = pagedBattersList;
                //playersModel.Pitchers = pagedPitchersList;
                playersModel.PagingInfo = pagingInfo;
            }
            else
            {
                return Content("An error ocurred while retrieving data.");
            }

            return View(playersModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: Players/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Player player)
        {
            try
            {
                var client = BaseballHttpClient.GetClient();

                var serializedPlayer = JsonConvert.SerializeObject(player);

                var response =
                    await
                        client.PostAsync("api/players",
                            new StringContent(serializedPlayer, Encoding.Unicode, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                return Content("An error ocurred while creating this player.");
            }
            catch (Exception ex)
            {

                return Content("An error ocurred while creating this player.");
            }
        }

        public async Task<ActionResult> Edit(int mlbamid)
        {
            var client = BaseballHttpClient.GetClient();

            var httpResponseMessage = await client.GetAsync("api/players/" + mlbamid);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var content = await httpResponseMessage.Content.ReadAsStringAsync();
                var playerModel = JsonConvert.DeserializeObject<Player>(content);

                return View(playerModel);
            }

            return Content("An error ocurred while retrieving this player.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int mlbamid, Player player)
        {
            try
            {
                var client = BaseballHttpClient.GetClient();

                var serializedPlayer = JsonConvert.SerializeObject(player);

                var httpResponseMessage =
                    await client.PutAsync("api/players/" + mlbamid,
                        new StringContent(serializedPlayer, Encoding.Unicode, "application/json"));

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                return Content("An error ocurred while editing this player.");
            }
            catch
            {
                return Content("An error ocurred while editing this player.");
            }
        }

        public async Task<ActionResult> Delete(int mlbamid)
        {
            try
            {
                var client = BaseballHttpClient.GetClient();
                var httpResponseMessage = await client.DeleteAsync("api/players/" + mlbamid);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                return Content("An error ocurred while trying to delete this player.");
            }
            catch (Exception)
            {
                return Content("An error ocurred while trying to delete this player.");
            }
        }

        public async Task<ActionResult> Details(int mlbamid)
        {
            var client = BaseballHttpClient.GetClient();
            var playersModel = new PlayersViewModel();

            var playersResponseMessage = await client.GetAsync("api/players/" + mlbamid);
            var battersResponseMessage = await client.GetAsync("api/batters" + mlbamid);
            var pitchersResponseMessage = await client.GetAsync("api/pitchers" + mlbamid);

            if (playersResponseMessage.IsSuccessStatusCode)
            {
                var playersContent = await playersResponseMessage.Content.ReadAsStringAsync();


                var player = JsonConvert.DeserializeObject<IEnumerable<Player>>(playersContent);


                //var player = JsonConvert.DeserializeObject<IEnumerable<Player>>(playersResponseMessage.Content.ReadAsStringAsync());
                playersModel.Player = player;


            }
            else
            {
                return Content("An error ocurred while retrieving data for this player.");
            }

            if (battersResponseMessage.IsSuccessStatusCode)
            {
                var battersContent = await battersResponseMessage.Content.ReadAsStringAsync();
                var batter = JsonConvert.DeserializeObject<IEnumerable<Batter>>(battersContent);
                playersModel.Batter = batter;
            }

            if (pitchersResponseMessage.IsSuccessStatusCode)
            {
                var pitchersContent = await pitchersResponseMessage.Content.ReadAsStringAsync();
                var pitcher = JsonConvert.DeserializeObject<IEnumerable<Pitcher>>(pitchersContent);
                playersModel.Pitcher = pitcher;
            }
            
            return View(playersModel);            
        }
    }
}