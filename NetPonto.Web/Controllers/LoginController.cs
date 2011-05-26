using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetPonto.Infrastructure.Storage;
using NetPonto.Web.Model;

//Controller for a Login
namespace NetPonto.Web.Controllers
{
    public class LoginController : Controller
    {
        [HttpPost]
        public PartialViewResult ForgotPassword(ForgotPasswordModel model)
        {

            if (String.IsNullOrEmpty(model.Username))
            {
                ModelState.AddModelError("Username", ForgotPasswordStrings.USER_NAME_REQUIRED);
            }
            else
            {

            }

            //PartialViewResult retVal = null;
            //if (ModelState.IsValid)
            //{

            //    retVal = PartialView("ForgotPasswordAnswerAjax", model);
            //}
            //else
            //{
            //    retVal = PartialView("_ForgotPasswordUserNameAjax", model);
            //}

            //return retVal;

            return PartialView("");
        }

    }
}
