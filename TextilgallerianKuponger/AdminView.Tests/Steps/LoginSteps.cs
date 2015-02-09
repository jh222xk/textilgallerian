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
        
    }
}
