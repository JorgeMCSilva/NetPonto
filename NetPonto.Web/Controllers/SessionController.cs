using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetPonto.Infrastructure.Authentication;
using System.Web.Security;
using NetPonto.Infrastructure.Storage;

namespace NetPonto.Web.Controllers
{
    public class SessionController : Controller
    {

        IAuthenticationService _authService;
        ISession _session;
        UserActivity _log;
        public SessionController(
            IAuthenticationService authService,
            ISession session,
            I)
        {
            _session = session;
            _authService = authService;

        }

        //
        // GET: /Session/Create
        //Kind of like "Login"
        public ActionResult Create()
        {

            if (Request.QueryString["ReturnUrl"] != null)
            {
                Session["ReturnUrl"] = Request.QueryString["ReturnUrl"];
            }
            var user = new User();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(FormCollection collection)
        {
            var login = collection["username"];
            var password = collection["password"];
            var confirm = collection["confirm"];
            if (!String.IsNullOrEmpty(login) & !String.IsNullOrEmpty(password) & !String.IsNullOrEmpty(confirm))
            {
                try
                {
                    var registered = _authService.RegisterUser(login, password, confirm, "", "", "");
                    if (registered)
                    {
                        this.FlashInfo("Thank you for signing up!");
                        _log.LogIt(login, "Registered");

                        return AuthAndRedirect(login, login);
                    }
                    else
                    {
                        this.FlashWarning("There was a problem with your registration");
                    }
                }
                catch (Exception x)
                {
                    //the auth service should return a usable exception message
                    this.FlashError(x.Message);
                }
            }
            else
            {
                this.FlashError("Invalid user name or password");
            }
            return RedirectToAction("create");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection collection)
        {

            var login = collection["username"];
            var password = collection["password"];
            if (!String.IsNullOrEmpty(login) & !String.IsNullOrEmpty(password))
            {
                if (_authService.IsValidLogin(login, password))
                {
                    _log.LogIt(login, "Logged in");
                    return AuthAndRedirect(login, login);
                }
            }
            this.FlashWarning("Invalid login");

            return View(new User() { UserName = login });

        }

        void SynchUser(string userName, string friendly)
        {
            //see if a user exists
            var user = _session.Single<User>(x => x.UserName == userName);
            if (user == null)
            {
                user = new User() { UserName = userName, Friendly = friendly, OpenID = userName };
                _session.Add(user);
            }
            user.JustLoggedIn();
            _session.Update(user);
            _session.CommitChanges();
        }

        ActionResult AuthAndRedirect(string friendly, string userName)
        {

            SynchUser(userName, friendly);

            Response.Cookies["friendly"].Value = friendly;
            Response.Cookies["friendly"].Expires = DateTime.Now.AddDays(30);
            Response.Cookies["friendly"].HttpOnly = true;

            FormsAuthentication.SetAuthCookie(userName, true);
            if (Session["ReturnUrl"] != null)
            {
                return Redirect(Session["ReturnUrl"].ToString());
            }
            else
            {
                return Redirect("/");
            }
        }
        public ActionResult Authenticate(string returnUrl)
        {
            var openid = new OpenIdRelyingParty();
            var response = openid.GetResponse();



            if (response == null)
            {
                // Stage 2: user submitting Identifier
                Identifier id;
                if (Identifier.TryParse(Request.Form["openid_identifier"], out id))
                {
                    try
                    {
                        var req = openid.CreateRequest(id);
                        var fetch = new FetchRequest();
                        //ask for more info - the email address
                        //no guarantee you'll get it back :)
                        var email = new AttributeRequest(WellKnownAttributes.Contact.Email);
                        email.IsRequired = true;
                        fetch.Attributes.Add(email);
                        req.AddExtension(fetch);

                        return req.RedirectingResponse.AsActionResult();

                    }
                    catch (ProtocolException ex)
                    {
                        ViewBag.Message = ex.Message;
                        return View("Login");
                    }
                }
                else
                {
                    ViewBag.Message = "Invalid identifier";
                    return View("Login");
                }
            }
            else
            {
                // Stage 3: OpenID Provider sending assertion response
                switch (response.Status)
                {
                    case AuthenticationStatus.Authenticated:
                        //They're in there...

                        var fetch = response.GetExtension<FetchResponse>();
                        string email = null;
                        if (fetch != null)
                        {
                            IList<string> emailAddresses = fetch.Attributes[WellKnownAttributes.Contact.Email].Values;
                            email = emailAddresses.Count > 0 ? emailAddresses[0] : null;

                        }
                        var friendly = email ?? response.FriendlyIdentifierForDisplay;
                        return AuthAndRedirect(friendly, response.ClaimedIdentifier);
                    case AuthenticationStatus.Canceled:
                        ViewBag.Message = "Canceled at provider";
                        return View("Login");
                    case AuthenticationStatus.Failed:
                        ViewBag.Message = response.Exception.Message;
                        return View("Login");
                }
            }
            return new EmptyResult();
        }

        //Logout
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Delete()
        {
            Response.Cookies["friendly"].Value = null;
            _log.LogIt(User.Identity.Name, "Logged out");
            FormsAuthentication.SignOut();
            return RedirectToAction("index", "home");
        }

    }
}
