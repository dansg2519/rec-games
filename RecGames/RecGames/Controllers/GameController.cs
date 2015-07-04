using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using RecGames.DAL;
using RecGames.Models;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using RecGames.Helpers;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.IO;

namespace RecGames.Controllers
{
    public class GameController : ApiController
    {
        private RecGameContext db = new RecGameContext();

        [HttpPost]
        [ActionName("RecommendedGames")]
        public IHttpActionResult PostRecommendedGames(JObject playerData)
        {
            var ownedGamesIds = playerData.GetValue("owned_games").Select(o => o.SelectToken("appid").ToObject<int>()).ToList();
            var playerPortrait = playerData.GetValue("player_portrait").Select(p => p.ToString()).ToList();

            //ideia:pegar todos os jogos que o jogador não tem; pegar todos os jogos que não tem nenhuma tag que descrevem o jogador;
            //remover de todos os jogos que não tem, os que não tem nenhuma tag que descrevem o jogador, para assim obter 
            //jogos que o jogador não tem mas tem alguma das tags que o descrevem
            var playerNotOwnedGamesIds = db.Games.Where(g => !ownedGamesIds.Contains(g.GameID)).Select(g => g.GameID).ToList();
            var gamesIdsNotMatchingPortrait = db.Games.Where(g => g.Tags.All(t => !playerPortrait.Contains(t.TagName))).Select(g => g.GameID).ToList();

            //linha abaixo usada pra avaliar comportamento correto da query
            //var tags = db.Games.Where(g => gamesIdsNotMatchingPortrait.Contains(g.GameID)).SelectMany(g => g.Tags.Select(t => t.TagName)).ToList();

            playerNotOwnedGamesIds.RemoveAll(g => gamesIdsNotMatchingPortrait.Contains(g));

            var playerNotOwnedGames = db.Games.Where(g => playerNotOwnedGamesIds.Contains(g.GameID)).ToList();
            var gamesIdsToRecommend = GameHelpers.CalculateRecommendationScore(playerPortrait, playerNotOwnedGames);
            var gamesToRecommend = db.Games.Where(g => gamesIdsToRecommend.Contains(g.GameID)).ToList();
            var gamesToRecommendJson = GameHelpers.SetUpGamesToRecommendJson(gamesToRecommend);

            return Ok(gamesToRecommendJson);
        }
    }

    //public class GameController : Controller
    //{
    //    private RecGameContext db = new RecGameContext();

    //    // GET: Game
    //    public ActionResult Index()
    //    {
    //        return View(db.Games.ToList());
    //    }

    //    // GET: Game/Details/5
    //    public ActionResult Details(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        Game game = db.Games.Find(id);
    //        if (game == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        return View(game);
    //    }

    //    // GET: Game/Create
    //    public ActionResult Create()
    //    {
    //        return View();
    //    }

    //    // POST: Game/Create
    //    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    //    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult Create([Bind(Include = "GameID,Name,ControllersSupported,Platforms,Developers,Publishers,Genre,LaunchDate,TotalAchievements,Recommendations,MetacriticScore")] Game game)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            db.Games.Add(game);
    //            db.SaveChanges();
    //            return RedirectToAction("Index");
    //        }

    //        return View(game);
    //    }

    //    // GET: Game/Edit/5
    //    public ActionResult Edit(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        Game game = db.Games.Find(id);
    //        if (game == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        return View(game);
    //    }

    //    // POST: Game/Edit/5
    //    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    //    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult Edit([Bind(Include = "GameID,Name,ControllersSupported,Platforms,Developers,Publishers,Genre,LaunchDate,TotalAchievements,Recommendations,MetacriticScore")] Game game)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            db.Entry(game).State = EntityState.Modified;
    //            db.SaveChanges();
    //            return RedirectToAction("Index");
    //        }
    //        return View(game);
    //    }

    //    // GET: Game/Delete/5
    //    public ActionResult Delete(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        Game game = db.Games.Find(id);
    //        if (game == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        return View(game);
    //    }

    //    // POST: Game/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult DeleteConfirmed(int id)
    //    {
    //        Game game = db.Games.Find(id);
    //        db.Games.Remove(game);
    //        db.SaveChanges();
    //        return RedirectToAction("Index");
    //    }

    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            db.Dispose();
    //        }
    //        base.Dispose(disposing);
    //    }

    //    public IHttpActionResult postRecommendedGames()
    //    {
    //        return this.Ok();
    //    }
    //}
}
