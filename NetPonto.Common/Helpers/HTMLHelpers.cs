using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Web.Routing;
using System.Web;

namespace NetPonto.Common.Helpers
{
    /// <summary>
    /// Only HTML helpers here
    /// </summary>
    public static class HTMLHelpers
    {
        #region vars
        const string PUB_DIR = "/Public/";
        const string CSS_DIR = "css/";
        const string IMAGE_DIR = "images/";
        const string SCRIPT_DIR = "javascript/";

        //private static readonly Utilities Utilities;

        public enum InputType
        {
            Text,
            Color,
            Email,
            Search,
            Time,
            Tel,
            File,
            Date,
            Datetime,
            Month,
            Week,
            Url
        }

        public enum RangeNumberType
        {
            Range,
            Number
        }


        #endregion vars

        #region
        public static string Truncate(this HtmlHelper helper, string input, int length)
        {
            if (input.Length <= length)
            {
                return input;
            }
            return input.Substring(0, length) + "...";
        }

        #endregion

        #region Simple Image
        public static MvcHtmlString Image(this HtmlHelper helper,
         string src, string altText)
        {
            return Image(helper, src, altText, PUB_DIR, IMAGE_DIR);
        }

        public static MvcHtmlString Image(this HtmlHelper helper,
           string src, string altText, string pubDir, string cssDir)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", src);
            builder.MergeAttribute("alt", altText);
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }
        #endregion Simple Image

        #region JS Script Tag
        public static MvcHtmlString Script(this HtmlHelper helper,
           string src)
        {
            return Script(helper, src, PUB_DIR, SCRIPT_DIR);
        }

        public static MvcHtmlString Script(this HtmlHelper helper,
           string src, string pubDir, string cssDir)
        {
            var builder = new TagBuilder("script");
            builder.MergeAttribute("src", string.Format("{0}{1}{2}", pubDir, SCRIPT_DIR, src));
            builder.MergeAttribute("type", "text/javascript");
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }
        #endregion JS Script Tag

        #region CSS Script Tag
// ReSharper disable InconsistentNaming
        public static MvcHtmlString CSS(this HtmlHelper helper,
// ReSharper restore InconsistentNaming
           string src, string media)
        {
            return CSS(helper, src, media, PUB_DIR, CSS_DIR);
        }

// ReSharper disable InconsistentNaming
        public static MvcHtmlString CSS(this HtmlHelper helper,
// ReSharper restore InconsistentNaming
           string src)
        {
            return CSS(helper, src, "", PUB_DIR, CSS_DIR);
        }

// ReSharper disable InconsistentNaming
        public static MvcHtmlString CSS(this HtmlHelper helper,
// ReSharper restore InconsistentNaming
           string src, string media, string pubDir, string cssDir)
        {
            var builder = new TagBuilder("link");
            builder.MergeAttribute("href", string.Format("{0}{1}{2}", pubDir, cssDir, src));
            builder.MergeAttribute("rel", "stylesheet");
            builder.MergeAttribute("type", "text/css");
            if (media.Length > 0) builder.MergeAttribute("media", media);

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }
        #endregion CSS Script Tag

        // TODO: receber só o nome da imagem e nao o path todo
        // TODO: alterar a ordem dos parameteros para ficar coerente com o que vem de base do MVC3

        // Action Image is a linkbutton with an image inside
        #region Action Image
        public static MvcHtmlString ActionImage(this HtmlHelper htmlHelper, string imageUrl, string linkText, string actionName)
        {
            return htmlHelper.ActionImage(imageUrl, linkText, actionName, null, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public static MvcHtmlString ActionImage(this HtmlHelper htmlHelper, string imageUrl, string linkText, string actionName, object routeValues)
        {
            return htmlHelper.ActionImage(imageUrl, linkText, actionName, null, new RouteValueDictionary(routeValues), new RouteValueDictionary());
        }

        public static MvcHtmlString ActionImage(this HtmlHelper htmlHelper, string imageUrl, string linkText, string actionName, string controllerName)
        {
            return htmlHelper.ActionImage(imageUrl, linkText, actionName, controllerName, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public static MvcHtmlString ActionImage(this HtmlHelper htmlHelper, string imageUrl, string linkText, string actionName, RouteValueDictionary routeValues)
        {
            return htmlHelper.ActionImage(imageUrl, linkText, actionName, null, routeValues, new RouteValueDictionary());
        }

        public static MvcHtmlString ActionImage(this HtmlHelper htmlHelper, string imageUrl, string linkText, string actionName, object routeValues, object htmlanchorAttributes)
        {
            return htmlHelper.ActionImage(imageUrl, linkText, actionName, null, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlanchorAttributes), null);
        }

        public static MvcHtmlString ActionImage(this HtmlHelper htmlHelper, string imageUrl, string linkText, string actionName, RouteValueDictionary routeValues, IDictionary<string, object> htmlanchorAttributes, IDictionary<string, object> htmlImageAttributes)
        {
            return htmlHelper.ActionImage(imageUrl, linkText, actionName, null, routeValues, htmlanchorAttributes, htmlImageAttributes);
        }

        public static MvcHtmlString ActionImage(this HtmlHelper htmlHelper, string imageUrl, string linkText, string actionName, string controllerName, object routeValues, object htmlanchorAttributes)
        {
            //var route = (RouteValueDictionary)routeValues;
            //var html = (RouteValueDictionary)htmlanchorAttributes;

            //return htmlHelper.ActionImage(imageUrl, linkText, actionName, controllerName, route, html, null);

            return htmlHelper.ActionImage(imageUrl, linkText, actionName, controllerName, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlanchorAttributes), null);
        }

        public static MvcHtmlString ActionImage(this HtmlHelper htmlHelper, string imageUrl, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, object routeValues, object htmlanchorAttributes, object htmlImageAttributes)
        {
            return htmlHelper.ActionImage(imageUrl, linkText, actionName, controllerName, protocol, hostName, fragment, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlanchorAttributes), new RouteValueDictionary(htmlImageAttributes));
        }

        public static MvcHtmlString ActionImage<TController>(this HtmlHelper htmlHelper, Expression<Action<TController>> action, string imageUrl, string linkText) where TController : Controller
        {
            return htmlHelper.ActionImage(action, imageUrl, linkText, null, null, null);
        }

        public static MvcHtmlString ActionImage<TController>(this HtmlHelper htmlHelper, Expression<Action<TController>> action, string imageUrl, string linkText, object routeValues) where TController : Controller
        {
            return htmlHelper.ActionImage(action, imageUrl, linkText, routeValues, null, null);
        }

        public static MvcHtmlString ActionImage<TController>(this HtmlHelper htmlHelper, Expression<Action<TController>> action, string imageUrl, string linkText, object routeValues, object htmlanchorAttributes, object htmlImageAttributes) where TController : Controller
        {
            return htmlHelper.ActionImage(action, imageUrl, linkText, routeValues, new RouteValueDictionary(htmlanchorAttributes), new RouteValueDictionary(htmlImageAttributes));
        }

        //public static string ActionImage<TController>(this HtmlHelper htmlHelper, Expression<Action<TController>> action, string imageUrl, string linkText, object routeValues, IDictionary<string, object> htmlanchorAttributes, IDictionary<string, object> htmlImageAttributes) where TController : Controller
        //{

        //    var _routeValuesFromExpression = ExpressionHelper.GetRouteValuesFromExpression(action);
        //    var _mergedRouteValues = MergeRouteValueDictionaries(_routeValuesFromExpression, new RouteValueDictionary(routeValues));

        //    // get the action name
        //    //
        //    var _actionName = ((MethodCallExpression)action.Body).Method.Name;

        //    // get the bare url for the Action using the current
        //    // request context
        //    //     
        //    var _url = new UrlHelper(htmlHelper.ViewContext.RequestContext).Action(_actionName, _mergedRouteValues);

        //    return GetImageLink(_url, linkText, imageUrl, htmlanchorAttributes, htmlImageAttributes);

        //}

        public static MvcHtmlString ActionImage(this HtmlHelper htmlHelper, string imageUrl, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlanchorAttributes, IDictionary<string, object> htmlImageAttributes)
        {
            // get the bare url for the Action using the current
            // request context
            //
            var url = new UrlHelper(htmlHelper.ViewContext.RequestContext).Action(actionName, controllerName, routeValues);

            return GetImageLink(url, linkText, imageUrl, htmlanchorAttributes, htmlImageAttributes);

        }

        public static MvcHtmlString ActionImage(this HtmlHelper htmlHelper, string imageUrl, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlanchorAttributes, IDictionary<string, object> htmlImageAttributes)
        {

            // get the bare url for the Action using the current
            // request context
            //
            var url = new UrlHelper(htmlHelper.ViewContext.RequestContext).Action(actionName, controllerName, routeValues, protocol, hostName);

            return GetImageLink(url, linkText, imageUrl, htmlanchorAttributes, htmlImageAttributes);

        }

        /// <summary>
        /// Build up the anchor and image tag.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="imageUrl">The image URL.</param>
        /// <param name="htmlanchorAttributes">The HTML anchor attributes.</param>
        /// <param name="htmlImageAttributes">The HTML image attributes.</param>
        /// <returns></returns>
        internal static MvcHtmlString GetImageLink(string url, string linkText, string imageUrl, IDictionary<string, object> htmlanchorAttributes, IDictionary<string, object> htmlImageAttributes)
        {
            // build up the image link.
            // <a href=\"ActionUrl\"><img src=\"ImageUrl\" alt=\"Your Link Text\" /> linkText </a>
            //

            var linkTxt = !string.IsNullOrEmpty(linkText) ? HttpUtility.HtmlEncode(linkText) : string.Empty;

            // build the img tag
            //
            var img = new TagBuilder("img");
            img.MergeAttributes(htmlImageAttributes);
            img.MergeAttribute("src", imageUrl);
            img.MergeAttribute("alt", linkTxt);

            // build the anchor tag
            //
            var lnk = new TagBuilder("a");
            lnk.MergeAttributes(htmlanchorAttributes);
            lnk.MergeAttribute("href", url);

            // place the img tag inside the anchor tag.
            //
            lnk.InnerHtml = img.ToString(TagRenderMode.Normal) + linkText;

            // render the image link.
            //
            return MvcHtmlString.Create(lnk.ToString(TagRenderMode.Normal));

        }

        /// <summary>
        /// Merges the 2 source route value dictionaries.
        /// </summary>
        /// <param name="routeValueDictionary1">RouteValueDictionary 1.</param>
        /// <param name="routeValueDictionary2">RouteValueDictionary 2.</param>
        /// <returns></returns>
        internal static RouteValueDictionary MergeRouteValueDictionaries(RouteValueDictionary routeValueDictionary1, RouteValueDictionary routeValueDictionary2)
        {
            var mergedRouteValues = new RouteValueDictionary();

            if ((routeValueDictionary1 != null) & (routeValueDictionary2 != null))
            {
                foreach (KeyValuePair<string, object> routeElement in routeValueDictionary1)
                {
                    mergedRouteValues[routeElement.Key] = routeElement.Value;
                }

                foreach (KeyValuePair<string, object> routeElement in routeValueDictionary2)
                {
                    mergedRouteValues[routeElement.Key] = routeElement.Value;
                }

                return mergedRouteValues;
            }

            return null;
        }
        #endregion Action Image

        // TODO: alterar para ter o source como o Action Image
        #region Audio
        public static MvcHtmlString Audio(this HtmlHelper helper, string source, bool controls, bool autoplay)
        {
            var builder = new TagBuilder("Audio");

            if (controls) builder.MergeAttribute("controls", "controls");
            if (autoplay) builder.MergeAttribute("autoplay", "autoplay");

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }



        #endregion Audio

        // TODO: alterar para ter o source como o Action Image
        #region Video
        private static StringBuilder BuildVideo(string source)
        {
            var builder = new StringBuilder("<video src='");
            builder.Append(source);
            builder.Append("' ");
            return builder;
        }

        public static string Video(this HtmlHelper helper, string source, bool controls, bool autoplay)
        {
            StringBuilder builder = BuildVideo(source);
            if (controls)
            {
                builder.Append("controls ");
            }
            if (autoplay)
            {
                builder.Append("autoplay ");
            }
            builder.Append(" />");
            return builder.ToString();
        }

        #endregion Video
    }
}
