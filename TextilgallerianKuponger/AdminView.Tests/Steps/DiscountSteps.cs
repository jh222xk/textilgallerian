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

        [Given(@"I have selected ""(.*)"" in the code type field")]
        public void GivenIHaveSelectedInTheCodeTypeField(String codeType)
        {
            driver.FindElement(By.Name("Code Type")).SendKeys(codeType);
        }

        [Given(@"And I have entered ""(.*)"" in the amount field")]
        public void GivenIHaveEnteredInTheAmountField(string amount)
        {
            driver.FindElement(By.Name("Amount")).SendKeys(amount);
        }

        [Given(@"I have entered ""(.*)"" in the customer email field")]
        public void GivenIHaveEnteredInTheCustomerEmailField(String email)
        {
            driver.FindElement(By.Name("Email")).SendKeys(email);
        }

        [Given(@"I have selected ""(.*)"" in the Start Date field")]
        public void GivenIHaveEnteredInTheStartDateField(String startDate)
        {
            driver.FindElement(By.Name("Start Date")).SendKeys(startDate);
        }

        [Given(@"I have selected ""(.*)"" in the End Date field")]
        public void GivenIHaveEnteredInTheEndDateField(String endDate)
        {
            driver.FindElement(By.Name("End Date")).SendKeys(endDate);
        }

        [Given(@"I have selected ""(.*)"" in the combinable checkbox")]
        public void GivenIHaveSelectedInTheCombinableCheckbox(String canBeCombined)
        {
            driver.FindElement(By.Name("Combinable")).SendKeys(canBeCombined);
        }

        [Given(@"I have entered ""(.*)"" in the ""(.*)"" field")]
        public void GivenIHaveEnteredInThePurchaseLimitField(String limit)
        {
            driver.FindElement(By.Name("limit")).SendKeys(limit);
        }

        [Given(@"I have entered ""(.*)"" in the take field")]
        public void GivenIHaveEnteredInTheTakeField(String take)
        {
            driver.FindElement(By.Name("Take")).SendKeys(take);
        }

        [Given(@"I have entered ""(.*)"" in the pay field")]
        public void GivenIHaveEnteredInThePayField(String pay)
        {
            driver.FindElement(By.Name("Pay")).SendKeys(pay);
        }

        [Given(@"And I have entered ""(.*)"" in the Buy Products field")]
        public void GivenIHaveEnteredInTheBuyProductsField(String buyProducts)
        {
            driver.FindElement(By.Name("Buy Products")).SendKeys(buyProducts);
        }

        [Given(@"And I have entered ""(.*)"" in the Free Products field")]
        public void GivenIHaveEnteredInTheFreeProductsField(String freeProducts)
        {
            driver.FindElement(By.Name("Free Products")).SendKeys(freeProducts);
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
