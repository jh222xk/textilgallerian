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
    public class DiscountSteps : CommonSteps
    {
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

        [Given(@"I have selected ""(.*)"" in the code type field")]
        public void GivenIHaveSelectedInTheCodeTypeField(String codeType)
        {
            Driver.FindElement(By.Name("Code Type")).SendKeys(codeType);
        }

        [Given(@"And I have entered ""(.*)"" in the amount field")]
        public void GivenIHaveEnteredInTheAmountField(string amount)
        {
            Driver.FindElement(By.Name("Amount")).SendKeys(amount);
        }

        [Given(@"I have entered ""(.*)"" in the customer email field")]
        public void GivenIHaveEnteredInTheCustomerEmailField(String email)
        {
            Driver.FindElement(By.Name("Email")).SendKeys(email);
        }

        [Given(@"I have selected ""(.*)"" in the Start Date field")]
        public void GivenIHaveEnteredInTheStartDateField(String startDate)
        {
            Driver.FindElement(By.Name("Start Date")).SendKeys(startDate);
        }

        [Given(@"I have selected ""(.*)"" in the End Date field")]
        public void GivenIHaveEnteredInTheEndDateField(String endDate)
        {
            Driver.FindElement(By.Name("End Date")).SendKeys(endDate);
        }

        [Given(@"I have selected ""(.*)"" in the combinable checkbox")]
        public void GivenIHaveSelectedInTheCombinableCheckbox(String canBeCombined)
        {
            Driver.FindElement(By.Name("Combinable")).SendKeys(canBeCombined);
        }

        [Given(@"I have entered ""(.*)"" in the ""(.*)"" field")]
        public void GivenIHaveEnteredInThePurchaseLimitField(String limit)
        {
            Driver.FindElement(By.Name("limit")).SendKeys(limit);
        }

        [Given(@"I have entered ""(.*)"" in the take field")]
        public void GivenIHaveEnteredInTheTakeField(String take)
        {
            Driver.FindElement(By.Name("Take")).SendKeys(take);
        }

        [Given(@"I have entered ""(.*)"" in the pay field")]
        public void GivenIHaveEnteredInThePayField(String pay)
        {
            Driver.FindElement(By.Name("Pay")).SendKeys(pay);
        }

        [Given(@"And I have entered ""(.*)"" in the Buy Products field")]
        public void GivenIHaveEnteredInTheBuyProductsField(String buyProducts)
        {
            Driver.FindElement(By.Name("Buy Products")).SendKeys(buyProducts);
        }

        [Given(@"And I have entered ""(.*)"" in the Free Products field")]
        public void GivenIHaveEnteredInTheFreeProductsField(String freeProducts)
        {
            Driver.FindElement(By.Name("Free Products")).SendKeys(freeProducts);
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

        [Given(@"I have entered ""(.*)"" in the amount field")]
        public void GivenIHaveEnteredInTheAmountField(int p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I have entered (.*) in the take field")]
        public void GivenIHaveEnteredInTheTakeField(int p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I have entered (.*) in the pay field")]
        public void GivenIHaveEnteredInThePayField(int p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"a ""(.*)"" with value April (.*), (.*)"" should exist")]
        public void ThenAWithValueAprilShouldExist(string p0, int p1, int p2)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the discount should not be combinable")]
        public void ThenTheDiscountShouldNotBeCombinable()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
