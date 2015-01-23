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
    public class AddNewDiscountCouponSteps : CommonSteps
    {
        [BeforeScenario("authentication")]
        public void BeforeAuthenticationScenario()
        {
            ThenIWouldNeedToLogin();
        }

        [Given(@"I am on the add new discount page")]
        public void GivenIAmOnTheAddNewDiscountPage()
        {
            WhenINavigateTo("/add");
        }

        [Given(@"I have selected the ""(.*)"" in the discount type field")]
        public void GivenIHaveSelectedTheInTheDiscountTypeField(String type)
        {
            driver.FindElement(By.Name("Type")).SendKeys(type);
        }

        [Given(@"I have entered ""(.*)"" in the percentage field")]
        public void GivenIHaveEnteredInThePercentageField(String percentage)
        {
            driver.FindElement(By.Name("Percentage")).SendKeys(percentage);
        }

        [Given(@"I have entered ""(.*)"" in the customer email field")]
        public void GivenIHaveEnteredInTheCustomerEmailField(String email)
        {
            driver.FindElement(By.Name("Email")).SendKeys(email);
        }

        [Given(@"I have entered ""(.*)"" in the ""(.*)"" field")]
        public void GivenIHaveEnteredInThePurchaseLimitField(String limit)
        {
            driver.FindElement(By.Name("limit")).SendKeys(limit);
        }

        [Given(@"I have entered (.*) in the take field")]
        public void GivenIHaveEnteredInTheTakeField(String take)
        {
            driver.FindElement(By.Name("Take")).SendKeys(take);
        }

        [Given(@"I have entered (.*) in the pay field")]
        public void GivenIHaveEnteredInThePayField(String pay)
        {
            driver.FindElement(By.Name("Pay")).SendKeys(pay);
        }

        [Then(@"a discount of type ""(.*)"" should exist")]
        public void ThenADiscountOfTypeShouldExist(String type)
        {
            driver.FindElement(By.ClassName("Type")).Text.should_be(type);
        }

        [Then(@"a ""(.*)"" with value ""(.*)"" should exist")]
        public void ThenAWithValueShouldExist(String name, String value)
        {
            driver.FindElement(By.ClassName(name)).Text.should_be(value);
        }
    }
}
