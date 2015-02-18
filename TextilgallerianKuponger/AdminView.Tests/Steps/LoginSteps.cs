using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace AdminView.Tests.Steps
{
    [Binding]
    public class LoginSteps : CommonSteps
    {
        [Then(@"I should be able to add a new discount")]
        public void ThenIShouldBeAbleToAddANewDiscount()
        {
            ThenIWouldNeedToLogin();
            Driver.FindElement(By.Name("code"));
            Driver.FindElement(By.Name("combinable"));
        }

        [Given(@"I am on the login page")]
        public void GivenIAmOnTheLoginPage()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I should be logged in")]
        public void ThenIShouldBeLoggedIn()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I shouldn't be logged in")]
        public void ThenIShouldntBeLoggedIn()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the system should present ""(.*)""")]
        public void ThenTheSystemShouldPresent(string p0)
        {
            ScenarioContext.Current.Pending();
        }
    }
}