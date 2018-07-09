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
    [Authorize]
    public class CollectionEntriesController : Controller
    {
        private MyApplicationDbContext db = new MyApplicationDbContext();

        // GET: CollectionEntries
        public ActionResult Index()
        {
            var collectionEntry = db.CollectionEntry.Include(c => c.Album).Include(c => c.Release).Include(c => c.User);
            return View(collectionEntry.ToList());
        }

        // GET: CollectionEntries/Details/5
        public ActionResult Details(string hashControlValue)
        {
            if (hashControlValue == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CollectionEntry collectionEntry = db.CollectionEntry.Where(c => c.HashControlValue == hashControlValue).FirstOrDefault();//.Find(id);
            if (collectionEntry == null)
            {
                return HttpNotFound();
            }
            return View(collectionEntry);
        }

        // GET: CollectionEntries/Create
        public ActionResult Create()
        {
            ViewBag.AlbumNo = new SelectList(db.Album, "EntryNo", "Author");
            ViewBag.ReleaseNo = new SelectList(db.Release, "EntryNo", "RecordCompany");
            ViewBag.UserNo = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: CollectionEntries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserNo,AlbumNo,ReleaseNo,Comment")] CollectionEntry collectionEntry)
        {
            if (ModelState.IsValid)
            {
                collectionEntry.HashControlValue = Utils.GetStringSha256Hash(collectionEntry.UserNo + collectionEntry.AlbumNo + collectionEntry.ReleaseNo);
                db.CollectionEntry.Add(collectionEntry);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AlbumNo = new SelectList(db.Album, "EntryNo", "Author", collectionEntry.AlbumNo);
            ViewBag.ReleaseNo = new SelectList(db.Release, "EntryNo", "RecordCompany", collectionEntry.ReleaseNo);
            ViewBag.UserNo = new SelectList(db.Users, "Id", "Email", collectionEntry.UserNo);
            return View(collectionEntry);
        }

        // GET: CollectionEntries/Edit/5
        public ActionResult Edit(string id)
        {
            //wyłąono możliwosć edycji 
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Brak możliwości edycji");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CollectionEntry collectionEntry = db.CollectionEntry.Find(id);
            if (collectionEntry == null)
            {
                return HttpNotFound();
            }
            ViewBag.AlbumNo = new SelectList(db.Album, "EntryNo", "Author", collectionEntry.AlbumNo);
            ViewBag.ReleaseNo = new SelectList(db.Release, "EntryNo", "RecordCompany", collectionEntry.ReleaseNo);
            ViewBag.UserNo = new SelectList(db.Users, "Id", "Email", collectionEntry.UserNo);
            return View(collectionEntry);
        }

        // POST: CollectionEntries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserNo,AlbumNo,ReleaseNo,Comment")] CollectionEntry collectionEntry)
        { 
            //wyłąono możliwosć edycji 
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Brak możliwości edycji");
            if (ModelState.IsValid)
            {
                db.Entry(collectionEntry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AlbumNo = new SelectList(db.Album, "EntryNo", "Author", collectionEntry.AlbumNo);
            ViewBag.ReleaseNo = new SelectList(db.Release, "EntryNo", "RecordCompany", collectionEntry.ReleaseNo);
            ViewBag.UserNo = new SelectList(db.Users, "Id", "Email", collectionEntry.UserNo);
            return View(collectionEntry);
        }

        // GET: CollectionEntries/Delete/5
        public ActionResult Delete(string hashControlValue)
        {
            if (hashControlValue == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CollectionEntry collectionEntry = db.CollectionEntry.Where(c => c.HashControlValue == hashControlValue).FirstOrDefault();//.Find(id);
            if (collectionEntry == null)
            {
                return HttpNotFound();
            }
            return View(collectionEntry);
        }

        // POST: CollectionEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string hashControlValue)
        {
            CollectionEntry collectionEntry = db.CollectionEntry.Where(c => c.HashControlValue == hashControlValue).FirstOrDefault();//.Find(id);
            db.CollectionEntry.Remove(collectionEntry);
            db.SaveChanges();
            return RedirectToAction("Index");
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
