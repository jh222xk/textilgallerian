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
        protected IWebDriver driver;

        [BeforeScenario]
        public void Setup()
        {
            driver = new PhantomJSDriver();
        }

        [AfterScenario]
        public void TearDown()
        {
            driver.Quit();
        }

        [Then(@"I would need to login")]
        public void ThenIWouldNeedToLogin()
        {
            WhenINavigateTo("/");
            driver.FindElement(By.Name("username")).SendKeys("username");
            driver.FindElement(By.Name("password")).SendKeys("password");
            WhenIPress("Login");
        }

        [When(@"I navigate to (.*)")]
        public void WhenINavigateTo(String path)
        {
            var rootUrl = new Uri(ConfigurationManager.AppSettings["RootUrl"]);
            var absoluteUrl = new Uri(rootUrl, path);
            driver.Navigate().GoToUrl(absoluteUrl);
        }

        [Then(@"the system should present success")]
        public void ThenTheSystemShouldPresentSuccess()
        {
            driver.FindElement(By.CssSelector("success"));
            driver.FindElement(By.CssSelector("error")).should_be_null();
        }

        [Then(@"the system should present success")]
        public void ThenTheSystemShouldPresentError()
        {
            driver.FindElement(By.CssSelector("error"));
            driver.FindElement(By.CssSelector("success")).should_be_null();
        }

        [When(@"I press (.*)")]
        public void WhenIPress(String button)
        {
            try
            {
                driver.FindElement(By.LinkText(button)).Click();
            }
            catch (NoSuchElementException)
            {
                driver.FindElement(
                    By.CssSelector(String.Format("input[type='submit'][value='{0}']", button)))
                    .Click();
            }
        }
    }
}
