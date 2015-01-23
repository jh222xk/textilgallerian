using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            driver.FindElement(By.Name("code"));
            driver.FindElement(By.Name("combinable"));
        }
        
    }
}
