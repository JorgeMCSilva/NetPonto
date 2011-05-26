using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace NetPonto.Infrastructure.Authentication
{
    public class AspMembershipAuthentication : IUserAuthentication
    {
        bool IUserAuthentication.IsValidLogin(string username, string password)
        {
            return Membership.ValidateUser(username, password);
        }

        bool IUserAuthentication.IsLogedIn()
        {
            return Membership.GetUser(true) != null ? true : false;
        }

        bool IUserAuthentication.IsInRole(string role)
        {
            return Roles.IsUserInRole(role);
        }

        string[] IUserAuthentication.GetRole()
        {
            return Roles.GetRolesForUser();
        }

        bool IUserAuthentication.GetFeature(string feature)
        {
            throw new NotImplementedException();
        }

        //TODO: Implementar reset de password via token com membership
        Guid IUserAuthentication.GetResetToken(string email)
        {
            throw new NotImplementedException();
        }

        bool IUserAuthentication.ResetPassword()
        {
            throw new NotImplementedException();
        }

        //TODO: Adicionar campos que são actualizáveis no membership
        bool IUserAuthentication.UpdateUser(string email)
        {
            throw new NotImplementedException();
        }


        public bool RegisterUser(string email, string passwd)
        {
            bool result = false;
            MembershipCreateStatus status;
            var reminderQuestion = Guid.NewGuid().ToString();
            var reminderAnswer = Guid.NewGuid().ToString();
            Membership.CreateUser(email, passwd, email, reminderQuestion, reminderAnswer, true, out status);

            if (status == MembershipCreateStatus.Success)
            {
                result = true;

            }
            else
            {

                switch (status)
                {
                    case MembershipCreateStatus.DuplicateEmail:
                        throw new InvalidOperationException("This email is already in our system");
                    case MembershipCreateStatus.DuplicateProviderUserKey:
                        throw new InvalidOperationException("There's a problem saving your information");
                    case MembershipCreateStatus.DuplicateUserName:
                        throw new InvalidOperationException("Need to use another login - this one's taken");
                    case MembershipCreateStatus.InvalidAnswer:
                        throw new InvalidOperationException("The reminder answer is not valid");
                    case MembershipCreateStatus.InvalidEmail:
                        throw new InvalidOperationException("Invalid email address");
                    case MembershipCreateStatus.InvalidPassword:
                        throw new InvalidOperationException("Invalid password - please be sure to use some numbers and uppercase letters.");
                    case MembershipCreateStatus.InvalidProviderUserKey:
                        throw new InvalidOperationException("There's a problem saving your information");
                    //case MembershipCreateStatus.InvalidQuestion:
                    //    throw new InvalidOperationException("The reminder question is not valid.");
                    case MembershipCreateStatus.InvalidUserName:
                        throw new InvalidOperationException("The username you've chosen is not valid.");
                    case MembershipCreateStatus.ProviderError:
                        throw new InvalidOperationException("There's a problem saving your information");
                    case MembershipCreateStatus.UserRejected:
                        throw new InvalidOperationException("Cannot register at this time");
                }
            }

            return result;
        }
    }
}
