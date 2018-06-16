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
    public class PhotosController : Controller
    {
        private MyApplicationDbContext db = new MyApplicationDbContext();

        // GET: Photos
        public ActionResult Index()
        {
            var photos = db.Photos.Include(p => p.Album).Include(p => p.Release);
            return View(photos.ToList());
        }

        // GET: Photos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // GET: Photos/Create
        public ActionResult Create(int albumNo)
        {
            //ViewBag.AlbumNo = new SelectList(db.Album.Where(c=>c.EntryNo==albumNo).ToList() , "EntryNo", "MetaDescription");
            //wersja ddl bez filtrowania
            ViewBag.AlbumNo = new SelectList(db.Album, "EntryNo", "MetaDescription");
            ViewBag.ReleaseNo = new SelectList(db.Release.Where(c => c.AlbumNo == albumNo).ToList(), "EntryNo", "MetaDescription");
            //test
            //var rList = new List<Release>();
            //rList.Add(new Release() { EntryNo = 2343, ReleaseCode = "test", AlbumNo=3 });
            //rList.Add(new Release() { EntryNo = 54, ReleaseCode = "tfdfdsfest", AlbumNo=4 });
            //ViewBag.ReleaseNo = new SelectList(rList.Where(c => c.AlbumNo == albumNo).ToList(), "EntryNo", "ReleaseCode");
            //ViewBag.UserUploaded = User.Identity.Name;
            return View();
        }

        // POST: Photos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EntryNo,AlbumNo,ReleaseNo,FilePath,UserUploaded,PostedFile,IsMain")] Photo photo)
        {
            if (ModelState.IsValid)
            {
                if (photo.PostedFile != null && photo.AlbumNo != null)
                    photo.FilePath = Upload(photo.PostedFile, (int)photo.AlbumNo);
                //photo.UserUploaded = User.Identity.Name;
                db.Photos.Add(photo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AlbumNo = new SelectList(db.Album, "EntryNo", "MetaDescription", photo.AlbumNo);
            ViewBag.ReleaseNo = new SelectList(db.Release, "EntryNo", "MetaDescription", photo.ReleaseNo);
            //ViewBag.UserUploaded = User.Identity.Name;
            return View(photo);
        }

        // GET: Photos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            ViewBag.AlbumNo = new SelectList(db.Album, "EntryNo", "MetaDescription");
            ViewBag.ReleaseNo = new SelectList(db.Release.Where(c => c.AlbumNo == photo.AlbumNo).ToList(), "EntryNo", "MetaDescription");

            //ViewBag.AlbumNo = new SelectList(db.Album, "EntryNo", "Author", photo.AlbumNo);
            //ViewBag.ReleaseNo = new SelectList(db.Release, "EntryNo", "EntryNo", photo.ReleaseNo);
            return View(photo);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EntryNo,AlbumNo,ReleaseNo,FilePath,UserUploaded")] Photo photo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AlbumNo = new SelectList(db.Album, "EntryNo", "MetaDescription");
            ViewBag.ReleaseNo = new SelectList(db.Release.Where(c => c.AlbumNo == photo.AlbumNo).ToList(), "EntryNo", "MetaDescription");

            //ViewBag.AlbumNo = new SelectList(db.Album, "EntryNo", "Author", photo.AlbumNo);
            //ViewBag.ReleaseNo = new SelectList(db.Release, "EntryNo", "EntryNo", photo.ReleaseNo);
            return View(photo);
        }

        // GET: Photos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Photo photo = db.Photos.Find(id);

            DeleteFile(Path.Combine(MusicCollector.Properties.Resources.PHOTO_PATH + "/", photo.FilePath));

            db.Photos.Remove(photo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateAlbumNo(Photo model)
        {
            //ViewBag.ReleaseNo = new SelectList(db.Release, "EntryNo", "ReleaseCode");
            //test
            //var rList = new List<Release>();
            //rList.Add(new Release() { EntryNo = 55, ReleaseCode = "test" });
            //rList.Add(new Release() { EntryNo = 535, ReleaseCode = "tfdfdsfest" });
            //ViewBag.ReleaseNo = new SelectList(rList, "EntryNo", "ReleaseCode", 535);

            //return PartialView(model);
            return RedirectToAction("Create", new { albumNo = model.AlbumNo });
            //return View(); 
        }

        [HttpPost]
        public JsonResult UpdateAlbumNo2(int albumNo)
        {
            var Releases = db.Release.Where(c => c.AlbumNo == albumNo).ToList();
            if (!Releases.Any())
                return Json("", JsonRequestBehavior.DenyGet);
            else
            {
                var RelSelectList = Releases.Select(c => new
                {
                    Text = c.MetaDescription,
                    ID = c.EntryNo
                });
                return Json(RelSelectList, JsonRequestBehavior.DenyGet);
            }

            return Json(albumNo + " trtrtr", JsonRequestBehavior.AllowGet);
            //return RedirectToAction("Create", new { albumNo = albumNo });
        }

        [HttpPost]
        public JsonResult UpdateAlbumNo3()
        {
            return Json(" trtrtr", JsonRequestBehavior.AllowGet);
            //return RedirectToAction("Create", new { albumNo = albumNo });
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                if (fileName.ToLower().EndsWith("png") || fileName.ToLower().EndsWith("jpg") || fileName.ToLower().EndsWith("jpeg"))
                {
                    //var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    var path = Path.Combine(MusicCollector.Properties.Resources.PHOTO_PATH, fileName);
                    file.SaveAs(path);
                }
            }
            var no = Request.QueryString["albumNo"];
            if (no != null)
                return RedirectToAction("Create", new { albumNo = no.ToString() });
            else
                return RedirectToAction("Index");
        }

        /// <summary>
        /// zapisuje fotke na dysku
        /// </summary>
        /// <param name="file"></param>
        /// <param name="albumNo"></param>
        /// <returns></returns>
        public string Upload(HttpPostedFileBase file, int albumNo)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                if (fileName.ToLower().EndsWith("png") || fileName.ToLower().EndsWith("jpg") || fileName.ToLower().EndsWith("jpeg"))
                {
                    if (!Directory.Exists(MusicCollector.Properties.Resources.PHOTO_PATH + "/" + albumNo))
                        Directory.CreateDirectory(MusicCollector.Properties.Resources.PHOTO_PATH + "/" + albumNo);

                    //var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    var path = Path.Combine(MusicCollector.Properties.Resources.PHOTO_PATH + "/" + albumNo, fileName);
                    path = Tools.Utils.NextAvailableFilename(path);
                    path = path.Replace(@"\", "/");
                    file.SaveAs(path);
                    return albumNo + "/" + path.Substring(path.LastIndexOf("/") + 1);
                }
            }

            return "";
        }

        public void DeleteFile(string path)
        {
            FileInfo fi = new FileInfo(path);
            if (fi.Exists)
                fi.Delete();
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
