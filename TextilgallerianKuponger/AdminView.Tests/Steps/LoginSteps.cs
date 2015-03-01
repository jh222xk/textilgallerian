using NSpec;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace AdminView.Tests.Steps
{
    [Binding]
    public class Login : Base
    {
        private readonly CommonSteps _common = new CommonSteps();

        [Then(@"I should be able to add a new discount")]
        public void ThenIShouldBeAbleToAddANewDiscount()
        {
            _common.ThenIWouldNeedToLogin();
            Driver.FindElement(By.Name("code"));
            Driver.FindElement(By.Name("combinable"));
        }

        [Given(@"I am on the login page")]
        public void GivenIAmOnTheLoginPage()
        {
            _common.WhenINavigateTo("/");
        }

        [Then(@"I should be logged in")]
        public void ThenIShouldBeLoggedIn()
        {
            ThenTheSystemShouldPresent("Logga ut");
            Driver.FindElement(By.LinkText("Logga ut"));
        }

        [Then(@"I shouldn't be logged in")]
        public void ThenIShouldntBeLoggedIn()
        {
            expect<NoSuchElementException>(() =>
                Driver.FindElement(By.LinkText("Logga ut")));
        }

        [Then(@"the system should present ""(.*)""")]
        public void ThenTheSystemShouldPresent(string message)
        {
            Driver.PageSource.should_contain(message);
        }
    }
}