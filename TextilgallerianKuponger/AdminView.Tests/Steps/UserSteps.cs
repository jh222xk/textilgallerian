using System;
using System.Configuration;
using NSpec;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using TechTalk.SpecFlow;
using AdminView.Tests.Steps;


namespace AdminView.Tests
{
    [Binding]
    class AddANewUserSteps : CommonSteps
    {
        //login
        [BeforeScenario("authentication")]
        public void BeforeAuthenticationScenario()
        {
            ThenIWouldNeedToLogin();
        }

        [Given(@"I am on the User Page")]
        public void GivenIAmOnTheUserPage()
        {
            WhenINavigateTo("/User");
        }

        [Given(@"And I have clicked Add new User")]
        public void GivenIHaveClickedAddNewUser()
        {
            WhenIPress("Add new User");
        }

        [Given(@"And I have entered ""(.*)""  in the Name Field")]
        public void GivenIHaveEnteredInTheNameField(string name)
        {
            driver.FindElement(By.Name("Name")).SendKeys(name);
        }

        [Given(@"And I have entered ""(.*)""  in the Email Field")]
        public void GivenIHaveEnteredInTheNameField(string email)
        {
            driver.FindElement(By.Name("Email")).SendKeys(email);
        }

        [Given(@"And I have entered ""(.*)""  in the Password Field")]
        public void GivenIHaveEnteredInTheNameField(string password)
        {
            driver.FindElement(By.Name("Password")).SendKeys(password);
        }

        [Given(@"And I have entered ""(.*)""  in the Password Confirmation Field")]
        public void GivenIHaveEnteredInTheNameField(string passwordConfirm)
        {
            driver.FindElement(By.Name("Password Confirmation")).SendKeys(passwordConfirm);
        }

        [Given(@"And I have entered ""(.*)""  in the Permission Field")]
        public void GivenIHaveEnteredInTheNameField(string permission)
        {
            driver.FindElement(By.Name("Permission")).SendKeys(permission);
        }

        [Then(@"a new User named ""(.*)"" should exist")]
        public void ThenANewUserNamedShouldExist(string name)
        {
            driver.FindElement(By.ClassName("name")).Text.should_be(name);
        }

        [Then(@"the ""(.*)"" of the new User should be ""(.*)""")]
        public void ThenAWithValueShouldExist(String name, String value)
        {
            driver.FindElement(By.ClassName(name)).Text.should_be(value);
        }
    }
}


