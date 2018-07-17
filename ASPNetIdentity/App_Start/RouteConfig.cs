using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MusicCollector
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //dodaje nowe ściezki do domyślnych urli
            //https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions-1/controllers-and-routing/creating-custom-routes-cs
            //routes.MapRoute("Index", "Albums/{searchString}/{startIndex}",
            //            new
            //            {
            //                controller = "Albums",
            //                action = "Index",
            //                startIndex = 0,
            //                pageSize = 5
            //            });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
