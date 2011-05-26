using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace NetPonto.Infrastructure.Filters
{
    public class TransactionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //commit the ISession unit of work
            //System.Web.Application.Session.CommitChanges();

        }

    }
}
