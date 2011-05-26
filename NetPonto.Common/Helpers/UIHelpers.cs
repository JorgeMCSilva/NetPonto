﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace NetPonto.Common.Helpers
{
    /// <summary>
    /// jQuery "snipets" go here
    /// </summary>
    public static class UIHelpers
    {
        #region FLASH
        public static void FlashInfo(this Controller controller, string message)
        {
            controller.TempData["info"] = message;
        }
        public static void FlashWarning(this Controller controller, string message)
        {
            controller.TempData["warning"] = message;
        }
        public static void FlashError(this Controller controller, string message)
        {
            controller.TempData["error"] = message;
        }
        public static string Flash(this HtmlHelper helper)
        {

            var message = "";
            var className = "";
            if (helper.ViewContext.TempData["info"] != null)
            {
                message = helper.ViewContext.TempData["info"].ToString();
                className = "info";
            }
            else if (helper.ViewContext.TempData["warning"] != null)
            {
                message = helper.ViewContext.TempData["warning"].ToString();
                className = "warning";
            }
            else if (helper.ViewContext.TempData["error"] != null)
            {
                message = helper.ViewContext.TempData["error"].ToString();
                className = "error";
            }
            var sb = new StringBuilder();
            if (!String.IsNullOrEmpty(message))
            {
                sb.AppendLine("<script>");
                sb.AppendLine("$(document).ready(function() {");
                sb.AppendFormat("$('#flash').html('{0}');", message);
                sb.AppendFormat("$('#flash').toggleClass('{0}');", className);
                sb.AppendLine("$('#flash').slideDown('slow');");
                sb.AppendLine("$('#flash').click(function(){$('#flash').toggle('highlight')});");
                sb.AppendLine("});");
                sb.AppendLine("</script>");
            }
            return sb.ToString();
        }
        #endregion FLASH

    }
}
