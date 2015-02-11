using System;
using System.Configuration;
using NSpec;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using TechTalk.SpecFlow;

namespace AdminView.Tests.Steps
{
    [Binding]
    public class CommonSteps : nspec
    {
        protected IWebDriver Driver;

        [BeforeScenario]
        public void Setup()
        {
            Driver = new PhantomJSDriver();
        }

        [AfterScenario]
        public void TearDown()
        {
            Driver.Quit();
        }

        [BeforeScenario("authentication")]
        public void BeforeAuthenticationScenario()
        {
            ThenIWouldNeedToLogin();
        }

        [Then(@"I would need to login")]
        public void ThenIWouldNeedToLogin()
        {
            WhenINavigateTo("/");
            Driver.FindElement(By.Name("username")).SendKeys("username");
            Driver.FindElement(By.Name("password")).SendKeys("password");
            WhenIPress("Login");
        }

        [When(@"I navigate to (.*)")]
        public void WhenINavigateTo(String path)
        {
            var rootUrl = new Uri(ConfigurationManager.AppSettings["RootUrl"]);
            var absoluteUrl = new Uri(rootUrl, path);
            Driver.Navigate().GoToUrl(absoluteUrl);
        }

        [Then(@"the system should present success")]
        public void ThenTheSystemShouldPresentSuccess()
        {
            Driver.FindElement(By.CssSelector("success"));
            Driver.FindElement(By.CssSelector("error")).should_be_null();
        }

        [Then(@"the system should present success")]
        public void ThenTheSystemShouldPresentError()
        {
            Driver.FindElement(By.CssSelector("error"));
            Driver.FindElement(By.CssSelector("success")).should_be_null();
        }

        [When(@"I press (.*)")]
        public void WhenIPress(String button)
        {
            try
            {
                Driver.FindElement(By.LinkText(button)).Click();
            }
            catch (NoSuchElementException)
            {
                Driver.FindElement(
                    By.CssSelector(String.Format("input[type='submit'][value='{0}']", button)))
                    .Click();
            }
        }
    }
}
