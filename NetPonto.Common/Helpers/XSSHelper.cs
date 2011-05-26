﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Security.Application;
using System.Web.Mvc;

namespace System.Web.Mvc
{
    public static class XSSHelper
    {
        public static string h(this HtmlHelper helper, string input)
        {
            return AntiXss.HtmlEncode(input);
        }
        public static string Sanitize(this HtmlHelper helper, string input)
        {
            return AntiXss.GetSafeHtml(input);
        }
        /// <summary>
        /// Encodes Javascript
        /// </summary>
        public static string hscript(this HtmlHelper helper, string input)
        {
            return AntiXss.JavaScriptEncode(input);
        }
    }
}
