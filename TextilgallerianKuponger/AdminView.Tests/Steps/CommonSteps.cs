using System;
using NSpec;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace AdminView.Tests.Steps
{
    [Binding]
    public class CommonSteps : Base
    {

        [BeforeScenario("editor")]
        public void BeforeEditingScenario()
        {
            Login("editor@admin.com", "password");
        }

        [BeforeScenario("admin")]
        public void BeforeAdminScenario()
        {
            Login("admin@admin.com", "password");
        }

        [AfterScenario]
        public void TearDown()
        {
            Dispose();
        }

        public void Login(String email, String password)
        {
            WhenINavigateTo("/");
            Driver.FindElement(By.Name("Email")).SendKeys(email);
            Driver.FindElement(By.Name("Password")).SendKeys(password);
            WhenIPress("Logga in");
        }

        [BeforeScenario("logout")]
        public void BeforeAuthenticationScenario()
        {
            try
            {
                WhenIPress("Logga ut");
            }
            catch
            { }
        }

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
                field = String.Format("Parameters[{0}]", field);
                Driver.PageSource.should_contain(field);
                Driver.FindElement(By.Name(field)).SendKeys(value);
            }
        }
    }
}