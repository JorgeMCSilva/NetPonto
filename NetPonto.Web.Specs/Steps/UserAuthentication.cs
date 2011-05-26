using TechTalk.SpecFlow;
using Moq;
using NetPonto.Infrastructure.Authentication;
using NUnit.Framework;

namespace NetPonto.Web.Specs.Steps
{
    [Binding]
    public class StepDefinitions
    {
        private IUserAuthentication _auth;
        public StepDefinitions()
        {
            var mock = new Moq.Mock<IUserAuthentication>();
            mock.Expect(x => x.IsValidLogin("admin", "password")).Returns(true);
            //mock.Expect(x => x.IsValidLogin(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            _auth = mock.Object;
        }

        [Given(@"a user exists with username ""(.*)"" and password ""(.*)""")]
        public void GivenAUserExistsWithUsernameAdminAndPasswordPassword(string userName, string password)
        {
            //ScenarioContext.Current.Pending();
        }

        [Then(@"that user should be able to login using ""(.*)"" and ""(.*)""")]
        public void ThenThatUserShouldBeAbleToLoginUsingAdminAndPassowrd(string userName, string password)
        {
            Assert.True(_auth.IsValidLogin(userName,password));
        }

        [Then(@"that user should not be able to login using ""(.*)"" and ""(.*)""")]
        public void ThenThatUserShouldNotBeAbleToLoginUsingAdminAndBadguy(string userName, string password)
        {
            Assert.False(_auth.IsValidLogin(userName, password));
        }

    }
}
