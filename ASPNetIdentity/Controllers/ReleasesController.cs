using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MusicCollector.Models;

namespace MusicCollector.Controllers
{
    public class ReleasesController : Controller
    {
        private MyApplicationDbContext db = new MyApplicationDbContext();

        // GET: Releases
        public ActionResult Index()
        {
            var release = db.Release.Include(r => r.Album);
            return View(release.ToList());
        }

        //na potrezeby okna modalnego
        [HttpGet]
        public ActionResult ReleaseModal(int albumNo, string query, int startIndex, int pageSize)
        {
            ViewBag.StartIndex = startIndex;
            ViewBag.AlbumNo = albumNo;
            var releaseList = db.Release.Where(c => c.AlbumNo == albumNo).ToList().Skip(startIndex).Take(pageSize);//.ToList();
            return PartialView("ReleaseModal", releaseList);
        }

        [HttpPost]
        [ActionName("ReleaseModal")]
        public ActionResult ReleaseModalPost(int albumNo, string query, int startIndex, int pageSize)
        {
            ViewBag.StartIndex = startIndex;
            ViewBag.AlbumNo = albumNo;
            var releaseList = db.Release.Where(c=>c.AlbumNo == albumNo).ToList().Skip(startIndex).Take(pageSize);//.ToList();
            return PartialView("ReleaseModal", releaseList);
        }

        // GET: Releases/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Release release = db.Release.Find(id);
            if (release == null)
            {
                return HttpNotFound();
            }
            return View(release);
        }

        // GET: Releases/Create
        public ActionResult Create()
        {
            ViewBag.AlbumNo = new SelectList(db.Album, "EntryNo", "Author");
            return View();
        }

        // POST: Releases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EntryNo,AlbumNo,RecordCompany,ReleaseCode")] Release release)
        {
            if (ModelState.IsValid)
            {
                db.Release.Add(release);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AlbumNo = new SelectList(db.Album, "EntryNo", "Author", release.AlbumNo);
            return View(release);
        }

        // GET: Releases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Release release = db.Release.Find(id);
            if (release == null)
            {
                return HttpNotFound();
            }
            ViewBag.AlbumNo = new SelectList(db.Album, "EntryNo", "Author", release.AlbumNo);
            return View(release);
        }

        // POST: Releases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EntryNo,AlbumNo,RecordCompany,ReleaseCode")] Release release)
        {
            if (ModelState.IsValid)
            {
                db.Entry(release).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AlbumNo = new SelectList(db.Album, "EntryNo", "Author", release.AlbumNo);
            return View(release);
        }

        // GET: Releases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Release release = db.Release.Find(id);
            if (release == null)
            {
                return HttpNotFound();
            }
            return View(release);
        }

        // POST: Releases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Release release = db.Release.Find(id);
            db.Release.Remove(release);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetReleaseInfo(int id)
        {
            var release = db.Release.Where(c => c.EntryNo == id).FirstOrDefault();
            if (release != null)
            {
                var data = new { ReleaseID = release.EntryNo, ReleaseDesc = release.MetaDescription };
                return Json(data);
            }
            return Json("Nie znalazło wydania", JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
