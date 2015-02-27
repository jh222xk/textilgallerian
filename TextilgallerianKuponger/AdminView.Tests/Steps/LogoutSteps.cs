using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace AdminView.Tests.Steps
{
    [Binding]
    public class LogoutSteps : Base
    {
        private readonly CommonSteps _common = new CommonSteps();

        [Given(@"I am on the root path")]
        public void GivenIAmOnTheRootPath()
        {
            _common.WhenINavigateTo("/");
        }

        [Then(@"I should be logged in")]
        public void ThenIShouldBeLoggedIn()
        {
            Driver.FindElement(By.LinkText("Logga ut"));
        }

        [Then(@"I should be logged out")]
        public void ThenIShouldBeLoggedOut()
        {
            expect<NoSuchElementException>(() =>
                Driver.FindElement(By.LinkText("Logga ut")));
        }
    }
}
