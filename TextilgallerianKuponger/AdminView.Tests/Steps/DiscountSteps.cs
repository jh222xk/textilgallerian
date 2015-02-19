using System;
using System.Runtime.InteropServices;
using AdminView.Tests.Steps;
using NSpec;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using TechTalk.SpecFlow;

namespace AdminView.Tests
{
    [Binding]
    public class DiscountSteps : Base
    {
        private readonly CommonSteps _common = new CommonSteps();

        [Given(@"I am on the add new discount page")]
        public void GivenIAmOnTheAddNewDiscountPage()
        {
            _common.WhenINavigateTo("/coupon/create");
        }

        [Given(@"I have selected the ""(.*)"" in the discount type field")]
        public void GivenIHaveSelectedTheInTheDiscountTypeField(String type)
        {
            Driver.FindElement(By.Name("Type")).Click();
            Driver.FindElement(By.CssSelector(String.Format("option[value='Domain.Entities.{0}']", type))).Click();
        }

        [Given(@"I have checked the coupon can be combined checkbox")]
        public void GivenIHaveEnteredInThePercentageField()
        {
            Driver.FindElement(By.Name("CanBeCombined")).Click();
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