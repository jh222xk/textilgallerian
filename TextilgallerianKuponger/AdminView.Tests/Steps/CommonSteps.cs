using System;
using System.Configuration;
using NSpec;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using TechTalk.SpecFlow;

namespace AdminView.Tests.Steps
{
    [Binding]
    public class CommonSteps : Base
    {

        [BeforeScenario("editor")]
        public void BeforeEditingScenario()
        {
            Driver = new PhantomJSDriver();
            Login("editor@admin.com", "password");
        }

        [BeforeScenario("admin")]
        public void BeforeAdminScenario()
        {
            Driver = new PhantomJSDriver();
            Login("admin@admin.com", "password");
        }

        [AfterScenario]
        public void TearDown()
        {
            Driver.Quit();
        }

        public void Login(String email, String password)
        {
//            try
//            {
                WhenINavigateTo("/");
                Driver.FindElement(By.Name("Email")).SendKeys(email);
                Driver.FindElement(By.Name("Password")).SendKeys(password);
                WhenIPress("Logga in");
//            }
//            catch
//            { }
        }

        [BeforeScenario("logout")]
        public void BeforeAuthenticationScenario()
        {
            Driver = new PhantomJSDriver();
            try
            {
                WhenIPress("Logga ut");
            }
            catch
            { }
        }

//        [Then(@"I would need to login")]
//        public void ThenIWouldNeedToLogin()
//        {
//            WhenINavigateTo("/");
//            Driver.FindElement(By.Name("username")).SendKeys("admin@admin.com");
//            Driver.FindElement(By.Name("password")).SendKeys("password");
//            WhenIPress("Login");
//        }

        [Then(@"I would need to login")]
        public void ThenIWouldNeedToLogin()
        {
            Driver.FindElement(By.Name("Email")).should_not_be_null();
            Driver.FindElement(By.Name("Password")).should_not_be_null();
        }

        [When(@"I navigate to (.*)")]
        public void WhenINavigateTo(String path)
        {
            var rootUrl = new Uri("http://localhost:8000");
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

        [When(@"I press ""(.*)""")]
        public void WhenIPress(String button)
        {
            try
            {
                Driver.FindElement(
                By.CssSelector(String.Format("input[type='submit'][value='{0}']", button)))
                .Click();
            }
            catch (NoSuchElementException)
            {
                Driver.FindElement(By.LinkText(button)).Click();
            }
        }

        [Given(@"I have entered ""(.*)"" in the ""(.*)"" field")]
        [When(@"I enter ""(.*)"" in the ""(.*)"" field")]
        public void WhenIEnterInTheField(string value, string field)
        {
            try
            {
                Driver.FindElement(By.Name(field)).SendKeys(value);
            }
            catch (NoSuchElementException)
            {
                Driver.FindElement(By.Name(String.Format("Parameters[{0}]", field))).SendKeys(value);
            }
        }

//        [Given(@"I have entered ""(.*)"" in the ""(.*)"" field")]
//        public void GivenIHaveEnteredInTheField(string value, string field)
//        {
//            try
//            {
//                Driver.FindElement(By.Name(field)).GetAttribute("value").should_be_same(value);
//            }
//            catch (NoSuchElementException)
//            {
//                Driver.FindElement(By.Name(String.Format("Parameters[{0}]", field))).GetAttribute("value").should_be_same(value);
//            }
//        }
    }
}