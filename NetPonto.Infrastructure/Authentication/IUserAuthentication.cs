using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetPonto.Infrastructure.Authentication
{
    public interface IUserAuthentication
    {
        bool IsValidLogin(string username, string password);
        bool IsLogedIn();
        bool IsInRole(string role);
        string[] GetRole();
        bool GetFeature(string feature);
        Guid GetResetToken(string email);
        //MembershipUser GetCurrentUser();
        bool ResetPassword();
        bool UpdateUser(string email);
        bool RegisterUser(string email, string passwd);
    }
}
