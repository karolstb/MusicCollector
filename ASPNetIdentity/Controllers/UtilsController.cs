using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using MusicCollector.Models;

namespace MusicCollector.Controllers
{
    public class UtilsController : Controller
    {
        private MyApplicationDbContext db = new MyApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        private string GetBase64Image(int photoEntryNo)
        {
            Photo photo = db.Photos.Where(c => c.EntryNo == photoEntryNo).FirstOrDefault();
            if (photo == null)
                return "";
            return GetBase64Image(photo.FilePath);
        }

        private string GetBase64Image(string photoPath)
        {
            FileStream fs = new FileStream(
                  //Server.MapPath((MusicCollector.Properties.Resources.PHOTO_PATH + " / " + photoPath).Replace("/","\\"))
                  Server.MapPath(@"~\App_Data\MusicPhotos\" + photoPath)
                   , FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            Byte[] bytes = br.ReadBytes((Int32)fs.Length);
            br.Close();
            fs.Close();
            return  Convert.ToBase64String(bytes);
        }

        //[ChildActionOnly]
        public PartialViewResult Utils(int photoId)//string photoPath)
        {
            string base64String = "";
            try
            {
                Photo photo = db.Photos.Where(c => c.EntryNo == photoId).FirstOrDefault();
                if (photo == null)
                    return PartialView();

                base64String = GetBase64Image(photo.FilePath);
            }
            catch (Exception ex)
            {

            }
            //var bytes = File.ReadAllBytes(HttpContext.Current.Server.MapPath(MusicCollector.Properties.Resources.PHOTO_PATH + " / " + photoPath));

            // Get some config settings etc here and make a view model
            var model = new UtilsModel
            {
                ImagesPath = base64String,
                PhotoEntryNo = photoId
                //ImagesPath = @"..\..\..\App_Data\MusicPhotos\4\Vader_ - _De_Profundis.jpg"
                //ImagesPath = Server.MapPath(@"~\App_Data\MusicPhotos\4\Vader_-_De_Profundis.jpg")
                //ImagesPath = Server.MapPath(MusicCollector.Properties.Resources.PHOTO_PATH +" / "+ photoPath)
                //ImagesPath = "../../../MusicPhotos"+photoPath
            };

            return PartialView(model);
        }


        public ActionResult Image(string id)
        {
            // Here is where we would take the id that was passed as
            // the argument and get the image from the database or 
            // filesystem.  In this example though, I am just going to 
            // return the same image from my local hard drive 
            // regardless of the id parameter.

            int photoId = 0;
            if (!Int32.TryParse(id, out photoId))
                return Redirect(Url.Content("~/"));

            var base64Image = GetBase64Image(photoId);
            byte[] image = Convert.FromBase64String(base64Image);
            string contentType = "image/jpeg";

            // Here we are calling the extension method and returning    
            // the ImageResult.
            return this.Image(image, contentType);
        }

    }
}