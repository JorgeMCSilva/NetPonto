﻿using System.Web.Mvc;
using NetPonto.Infrastructure.Log;

//Controller for a Error
namespace NetPonto.Web.Controllers
{
    public class ErrorController : Controller
    {
        ILogger _logger;

        public ErrorController(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// This is fired when the site hits a 500
        /// </summary>
        /// <returns></returns>
        public ActionResult Problem()
        {
            //no logging here - let the app do it 
            //we don't get reliable error traps here
            return View();
        }
        /// <summary>
        /// This is fired when the site gets a bad URL
        /// </summary>
        /// <returns></returns>
        public ActionResult NotFound()
        {
            //you should probably log this - if you're getting 
            //bad links you'll want to know...
            _logger.Warn(string.Format("404 - {0}", Request.UrlReferrer));
            return View();
        }

    }
}
