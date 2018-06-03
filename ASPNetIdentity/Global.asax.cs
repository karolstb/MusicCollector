using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MusicCollector
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //http://www.hackered.co.uk/articles/globalization-model-binding-datetimes-with-asp-net-mvc
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(TimeSpan), new Tools.DurationBinder());
        }
    }
}
