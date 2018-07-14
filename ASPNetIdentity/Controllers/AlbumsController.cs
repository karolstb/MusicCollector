using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MusicCollector.Models;
using System.IO;

namespace MusicCollector.Controllers
{
    public class AlbumsController : Controller
    {
        //string coverImgBase64 = "";
        private MyApplicationDbContext db = new MyApplicationDbContext();

        // GET: Albums
        public ActionResult Index(string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToUpper();
                var filteredList = db.Album.Where(c => c.Author.ToUpper().Contains(searchString) 
                    || c.Title.ToUpper().Contains(searchString)).ToList();
                return View(filteredList);
            }
            return View(db.Album.ToList());
        }

        //na potrezeby okna modalnego
        [HttpGet]
        public ActionResult AlbumModal(string query, int startIndex, int pageSize)
        {
            ViewBag.StartIndex = startIndex;
            var albumList = db.Album.ToList().Skip(startIndex).Take(pageSize);//.ToList();
            //var albumList = db.Album.ToList();
            return PartialView("AlbumModal", albumList);
        }

        [HttpPost]
        [ActionName("AlbumModal")]
        public ActionResult AlbumModalPost(string query, int startIndex, int pageSize)
        {
            ViewBag.StartIndex = startIndex;
            var albumList = db.Album.ToList().Skip(startIndex).Take(pageSize);//.ToList();
            //var albumList = db.Album.ToList();
            return PartialView("AlbumModal", albumList);
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

            List<Photo> photoList = db.Photos.Where(c => c.AlbumNo == album.EntryNo).ToList();
            return View(new Tuple<Album, ICollection<Photo>>(album, photoList));

            //return View(album);
        }

        // GET: Albums/Create
        public ActionResult Create(int defaultYearOfProduction, int minutes)
        {
            //ViewBag.OrderNo = 1;
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
        public ActionResult Create(Album album, string CoverArtHidden)
        {
            //bind include (albo exclude) definiuje kolumny, jakie można ustawić daną funkcją. 
            //dzięki temu zaboiega sie próbom atakowania np. przez fidller, gdzię można przypisać wartości do propertisów, 
            //którym nie chcemy na to pozwolic
            if (ModelState.IsValid)
            {
                db.Album.Add(album);
                db.SaveChanges();

                if (!String.IsNullOrEmpty(CoverArtHidden))
                {
                    Photo photo = new Photo();
                    photo.AlbumNo = album.EntryNo;
                    photo.IsMain = true;
                    photo.UserUploaded = HttpContext.User.Identity.Name;

                    //zapis obrazka na dysku do pliku
                    if (!Directory.Exists(MusicCollector.Properties.Resources.PHOTO_PATH + "/" + album.EntryNo))
                        Directory.CreateDirectory(MusicCollector.Properties.Resources.PHOTO_PATH + "/" + album.EntryNo);
                    string fileName = DateTime.Now.Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds + ".jpg";
                    string filePath = Path.Combine(MusicCollector.Properties.Resources.PHOTO_PATH + "/" + album.EntryNo, fileName);
                    filePath = filePath.Replace(@"\", "/");

                    var bytes = Convert.FromBase64String(CoverArtHidden);
                    using (var imageFile = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                    }

                    photo.FilePath = album.EntryNo + "/" + fileName;
                    db.Photos.Add(photo);
                    db.SaveChanges();
                }

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
            //usun zdjęcia
            List<Photo> photoList = db.Photos.Where(c => c.AlbumNo == id).ToList();
            foreach(Photo photo in photoList)
            {
                if (!String.IsNullOrEmpty(photo.FilePath))
                {
                    PhotosController photoController = new PhotosController();
                    photoController.DeleteFile(Path.Combine(MusicCollector.Properties.Resources.PHOTO_PATH + "/", photo.FilePath));
                }
                db.Photos.Remove(photo);
            }
            //usun folder na zdjecia
            if (Directory.Exists(MusicCollector.Properties.Resources.PHOTO_PATH + "/" + id))
                Directory.Delete(MusicCollector.Properties.Resources.PHOTO_PATH + "/" + id);

            Album album = db.Album.Find(id);
            db.Album.Remove(album);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //// GET: Photos/Create
        //public ActionResult AddPhoto()
        //{
        //    ViewBag.AlbumNo = new SelectList(db.Album, "EntryNo", "Author");
        //    ViewBag.ReleaseNo = new SelectList(db.Release, "EntryNo", "EntryNo");
        //    return View();
        //}

        /// <summary>
        /// fucnkaj próbuje wyszukać w sieci zdjęcie okładki dodwanego albumu i pokazac na formatce (ajax)
        /// </summary>
        /// <param name="author"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SearchForCoverImage(string author, string title, int order)
        {
            if (String.IsNullOrEmpty(author) || String.IsNullOrEmpty(title))
            {
                var emptyData = new { ImageBase64 = "" };
                return Json(emptyData, JsonRequestBehavior.AllowGet);
            }
            
            //ViewBag.OrderNo = order;
            
            string photo = "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";
            var imageBytes = Utils.SearchForGoogleImage(author + " " + title, order);
            photo = Convert.ToBase64String(imageBytes);
            var photoData = new { ImageBase64 = photo };
            //ViewBag.CoverImgBase64 = photo;
            return Json(photoData, JsonRequestBehavior.DenyGet);
            //var Releases = db.Release.Where(c => c.AlbumNo == albumNo).ToList();
            //if (!Releases.Any())
            //    return Json("", JsonRequestBehavior.DenyGet);
            //else
            //{
            //    var RelSelectList = Releases.Select(c => new
            //    {
            //        Text = c.MetaDescription,
            //        ID = c.EntryNo
            //    });
            //    return Json(RelSelectList, JsonRequestBehavior.DenyGet);
            //}

            return Json(author+" "+title + " trtrtr", JsonRequestBehavior.AllowGet);
            //return RedirectToAction("Create", new { albumNo = albumNo });
        }

        [HttpPost]
        public JsonResult GetAlbumInfo(int id)
        {
            var album = db.Album.Where(c => c.EntryNo == id).FirstOrDefault();
            if(album!=null)
            {
                var data = new { AlbumID = album.EntryNo, AlbumDesc = album.MetaDescription };
                return Json(data);
            }
            return Json("Nie znalazło albumu", JsonRequestBehavior.AllowGet);
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
