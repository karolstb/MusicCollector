using MusicCollector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicCollector.Tools
{
    public class DurationBinder : DefaultModelBinder//IModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //var model = (Album)base.BindModel(controllerContext, bindingContext);
            var minutes = controllerContext.HttpContext.Request["Item1.Duration.Minutes"];
            var seconds = controllerContext.HttpContext.Request["Item1.Duration.Seconds"];
            var time = new TimeSpan(0, int.Parse(minutes), int.Parse(seconds));
            //model.Duration = time;
            //return model;
            return time;

            //var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            //if (value == null)
            //    return null;
            //TimeSpan ts = new TimeSpan();
            //TimeSpan.TryParse(value.AttemptedValue, out ts);
            //return ts;
        }
    }

    public class TupleAlbumPhotoBinder : DefaultModelBinder//IModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            return new Tuple<Album, ICollection<Photo>>(new Album(), new List<Photo>());

            //return base.CreateModel(controllerContext, bindingContext, modelType);
        }
    }


            //public class NullableDateTimeModelBinder : IModelBinder
            //{
            //    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            //    {
            //        var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            //        if (string.IsNullOrWhiteSpace(value.AttemptedValue))
            //        {
            //            return null;
            //        }

            //        DateTime dateTime;

            //        var isDate = DateTime.TryParse(value.AttemptedValue, Thread.CurrentThread.CurrentUICulture, DateTimeStyles.None, out dateTime);

            //        if (!isDate)
            //        {
            //            bindingContext.ModelState.AddModelError(bindingContext.ModelName, ModelValidationResources.InvalidDateTime);
            //            return DateTime.UtcNow;
            //        }

            //        return dateTime;
            //    }
            //}
        }