﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Security;

namespace MusicCollector
{
    public static class Utils
    {
        private static string numberPattern = " ({0})";

        public static string NextAvailableFilename(string path)
        {
            // Short-cut if already available
            if (!File.Exists(path))
                return path;

            // If path has extension then insert the number pattern just before the extension and return next filename
            if (Path.HasExtension(path))
                return GetNextFilename(path.Insert(path.LastIndexOf(Path.GetExtension(path)), numberPattern));

            // Otherwise just append the pattern to the path and return next filename
            return GetNextFilename(path + numberPattern);
        }

        private static string GetNextFilename(string pattern)
        {
            string tmp = string.Format(pattern, 1);
            if (tmp == pattern)
                throw new ArgumentException("The pattern must include an index place-holder", "pattern");

            if (!File.Exists(tmp))
                return tmp; // short-circuit if no matches

            int min = 1, max = 2; // min is inclusive, max is exclusive/untested

            while (File.Exists(string.Format(pattern, max)))
            {
                min = max;
                max *= 2;
            }

            while (max != min + 1)
            {
                int pivot = (max + min) / 2;
                if (File.Exists(string.Format(pattern, pivot)))
                    min = pivot;
                else
                    max = pivot;
            }

            return string.Format(pattern, max);
        }

        public static string GetStringSha256Hash(string text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }

        //https://stackoverflow.com/questions/27846337/select-and-download-random-image-from-google#
        #region pobierania obrazka z google
        /// <summary>
        /// zwraca zdjęcie z wynikow google na podstawie podanego hasła i które tam z kolejnosci
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public static byte[] SearchForGoogleImage(string topic, int order = 1)
        {
            string html = GetHtmlCode(topic);
            string url = GetUrl(html, order);
            byte[] image = GetImage(url);
            return image;
        }

        private static string GetHtmlCode(string topic)
        {
            string url = "https://www.google.com/search?q=" + topic + "&tbm=isch";
            string data = "";

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";

            var response = (HttpWebResponse)request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                if (dataStream == null)
                    return "";
                using (var sr = new StreamReader(dataStream))
                {
                    data = sr.ReadToEnd();
                }
            }
            return data;
        }

        private static string GetUrl(string html, int order)
        {
            string url = "";
            int i = 1;
            int ndx = html.IndexOf("\"ou\"", StringComparison.Ordinal);

            while (ndx >= 0 && i<=order)
            {
                ndx = html.IndexOf("\"", ndx + 4, StringComparison.Ordinal);
                ndx++;
                int ndx2 = html.IndexOf("\"", ndx, StringComparison.Ordinal);
                url = html.Substring(ndx, ndx2 - ndx);
                ndx = html.IndexOf("\"ou\"", ndx2, StringComparison.Ordinal);
                i++;
            }
            return url;
        }

        private static byte[] GetImage(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                if (dataStream == null)
                    return null;
                using (var sr = new BinaryReader(dataStream))
                {
                    byte[] bytes = sr.ReadBytes(100000000);

                    return bytes;
                }
            }

            return null;
        }
        #endregion
    }
    
    public class UtilsModel
    {
        public string ImagesPath { get; set; }
        public int PhotoEntryNo { get; set; }
    }

    /// <summary>
    /// klasa dziedziczy po action result i obiekt tej klasy moze być zwrócony przez akcje z kontrolera
    /// jako link na przykłąd
    /// https://blogs.msdn.microsoft.com/miah/2008/11/13/extending-mvc-returning-an-image-from-a-controller-action/
    /// </summary>
    public class ImageResult : ActionResult
    {
        public ImageResult(Stream imageStream, string contentType)
        {
            if (imageStream == null)
                throw new ArgumentNullException("imageStream");
            if (contentType == null)
                throw new ArgumentNullException("contentType");

            this.ImageStream = imageStream;
            this.ContentType = contentType;
        }

        public Stream ImageStream { get; private set; }
        public string ContentType { get; private set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            HttpResponseBase response = context.HttpContext.Response;

            response.ContentType = this.ContentType;

            byte[] buffer = new byte[4096];
            while (true)
            {
                int read = this.ImageStream.Read(buffer, 0, buffer.Length);
                if (read == 0)
                    break;

                response.OutputStream.Write(buffer, 0, read);
            }

            response.End();
        }
    }
}

namespace MusicCollector
{
    //rozszerza HtmlHelper
    public static class LinkExtensions
    {
        public static IHtmlString ActionLinkIfInRole(
            this HtmlHelper htmlHelper,
            string roles,
            string linkText,
            string action
        )
        {
            if (!Roles.IsUserInRole(roles))
            {
                return MvcHtmlString.Empty;
            }
            return htmlHelper.ActionLink(linkText, action);
        }

        public static IHtmlString ActionLinkIfAuthorize(
            this HtmlHelper htmlHelper,
            string linkText,
            string action,
            string controller,
            object routeValues,
            object htmlAttributes
        )
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return MvcHtmlString.Empty;
            }
            return htmlHelper.ActionLink(linkText, action, controller, routeValues, htmlAttributes);
        }
    }

    //rozszerza Controller
    public static class ControllerExtensions
    {
        public static ImageResult Image(this Controller controller, Stream imageStream, string contentType)
        {
            return new ImageResult(imageStream, contentType);
        }

        public static ImageResult Image(this Controller controller, byte[] imageBytes, string contentType)
        {
            return new ImageResult(new MemoryStream(imageBytes), contentType);
        }
    }
}