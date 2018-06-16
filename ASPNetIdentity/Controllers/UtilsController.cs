using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace MusicCollector.Controllers
{
    public class UtilsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        

        //[ChildActionOnly]
        public PartialViewResult Utils(string photoPath)
        {
            string base64String = "";
            try
            {
                FileStream fs = new FileStream(
                   //Server.MapPath((MusicCollector.Properties.Resources.PHOTO_PATH + " / " + photoPath).Replace("/","\\"))
                   Server.MapPath(@"~\App_Data\MusicPhotos\"+photoPath)
                    , FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                br.Close();
                fs.Close();
                base64String = Convert.ToBase64String(bytes);
            }
            catch (Exception ex)
            {

            }
            //var bytes = File.ReadAllBytes(HttpContext.Current.Server.MapPath(MusicCollector.Properties.Resources.PHOTO_PATH + " / " + photoPath));

            // Get some config settings etc here and make a view model
            var model = new Tools.UtilsModel
            {
                ImagesPath = base64String
                //ImagesPath = @"..\..\..\App_Data\MusicPhotos\4\Vader_ - _De_Profundis.jpg"
                //ImagesPath = Server.MapPath(@"~\App_Data\MusicPhotos\4\Vader_-_De_Profundis.jpg")
                //ImagesPath = Server.MapPath(MusicCollector.Properties.Resources.PHOTO_PATH +" / "+ photoPath)
                //ImagesPath = "../../../MusicPhotos"+photoPath
            };

            return PartialView(model);
        }
    }
}