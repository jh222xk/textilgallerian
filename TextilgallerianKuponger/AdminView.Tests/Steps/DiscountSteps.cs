using System;
using NSpec;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace AdminView.Tests.Steps
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
            Driver.FindElement(By.Name("Type")).SendKeys(type);
        }

        [Given(@"I have entered ""(.*)"" in the percentage field")]
        public void GivenIHaveEnteredInThePercentageField(String percentage)
        {
            Driver.FindElement(By.Name("Percentage")).SendKeys(percentage);
        }

        [Given(@"I have entered ""(.*)"" in the customer email field")]
        public void GivenIHaveEnteredInTheCustomerEmailField(String email)
        {
            Driver.FindElement(By.Name("Email")).SendKeys(email);
        }

        [Given(@"I have entered ""(.*)"" in the ""(.*)"" field")]
        public void GivenIHaveEnteredInThePurchaseLimitField(String limit)
        {
            Driver.FindElement(By.Name("limit")).SendKeys(limit);
        }

        [Given(@"I have entered (.*) in the take field")]
        public void GivenIHaveEnteredInTheTakeField(String take)
        {
            Driver.FindElement(By.Name("Take")).SendKeys(take);
        }

        [Given(@"I have entered (.*) in the pay field")]
        public void GivenIHaveEnteredInThePayField(String pay)
        {
            Driver.FindElement(By.Name("Pay")).SendKeys(pay);
        }

        [Then(@"a discount of type ""(.*)"" should exist")]
        public void ThenADiscountOfTypeShouldExist(String type)
        {
            Driver.FindElement(By.ClassName("Type")).Text.should_be(type);
        }

        [Then(@"a ""(.*)"" with value ""(.*)"" should exist")]
        public void ThenAWithValueShouldExist(String name, String value)
        {
            Driver.FindElement(By.ClassName(name)).Text.should_be(value);
        }
    }
}
