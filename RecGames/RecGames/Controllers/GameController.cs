﻿using System;
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

namespace RecGames.Controllers
{
    public class GameController : ApiController
    {
        private RecGameContext db = new RecGameContext();

        [HttpPost]
        [ActionName("RecommendedGames")]
        public IHttpActionResult PostRecommendedGames(JObject playerData)
        {

            //var playerNotOwnedGames = db.Games.Where(g => !playerOwnedGames.Contains(g));
            return Ok();
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
