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
    public class AlbumsController : Controller
    {
        private MyApplicationDbContext db = new MyApplicationDbContext();

        // GET: Albums
        public ActionResult Index()
        {
            return View(db.Album.ToList());
        }

        // GET: Albums/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Album.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // GET: Albums/Create
        public ActionResult Create(int defaultYearOfProduction, int minutes)
        {
            Album newAlbum = new Album();
            newAlbum.YearOfProduction = defaultYearOfProduction;
            newAlbum.Duration = new TimeSpan(0, minutes, 0);
            return View( newAlbum);
        }

        //// GET: Albums/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: Albums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "EntryNo,Author,Title,Duration")] Album album)   
        //public ActionResult Create([Bind(Include = "Author,Title,Duration,YearOfProduction", Prefix ="Duration")] Album album)
        public ActionResult Create(Album album)
        {
            //bind include (albo exclude) definiuje kolumny, jakie można ustawić daną funkcją. 
            //dzięki temu zaboiega sie próbom atakowania np. przez fidller, gdzię można przypisać wartości do propertisów, 
            //którym nie chcemy na to pozwolic
            if (ModelState.IsValid)
            {
                db.Album.Add(album);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(album);
        }

        // GET: Albums/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Album.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }

            List<Photo> photoList = db.Photos.Where(c => c.AlbumNo == album.EntryNo).ToList();
            return View(new Tuple<Album, ICollection<Photo>>(album, photoList));
            //return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "EntryNo,Author,Title,Duration,YearOfProduction")] Album album)
        public ActionResult Edit(Tuple<Album, ICollection<Photo>> albumPlusPhotos)
        {
            if (ModelState.IsValid)
            {
                var album = albumPlusPhotos.Item1;
                db.Entry(album).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(albumPlusPhotos);
            //return View(album);
        }

        // GET: Albums/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Album.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Album album = db.Album.Find(id);
            db.Album.Remove(album);
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
