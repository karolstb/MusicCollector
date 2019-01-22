using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MusicCollector
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions-1/controllers-and-routing/creating-custom-routes-cs
        /// <summary>
        /// dodaje nowe ściezki do domyślnych urli
        /// </summary>
        /// <param name="routes"></param>
        //public static void RegisterRoutes(RouteCollection routes)
        //{
        //    routes.MapRoute("Index", "Index/{query}/{startIndex}",
        //                new
        //                {
        //                    controller = "Albums",
        //                    action = "Index",
        //                    startIndex = 0,
        //                    pageSize = 5
        //                });
        //}

        protected void Application_Start()
        {
            DisableApplicationInsightsOnDebug();
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //http://www.hackered.co.uk/articles/globalization-model-binding-datetimes-with-asp-net-mvc
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(TimeSpan), new Tools.DurationBinder());
            ModelBinders.Binders.Add(typeof(Tuple<MusicCollector.Models.Album, ICollection<MusicCollector.Models.Photo>>), new Tools.TupleAlbumPhotoBinder());
        }

        /// <summary>
        /// Disables the application insights locally.
        /// </summary>
        [Conditional("DEBUG")]
        private static void DisableApplicationInsightsOnDebug()
        {
            TelemetryConfiguration.Active.DisableTelemetry = true;
        }
    }
}
