using System;
using System.Linq;
using AdminView.Tests.Steps;
using NSpec;
using OpenQA.Selenium;
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
            Driver.FindElement(By.LinkText("Rabatter")).Click();
            ItShouldHaveAOf("Typ av kampanj", type);
        }

        [Then(@"it should have a ""(.*)"" of ""(.*)""")]
        public void ItShouldHaveAOf(String key, String value)
        {
            var index = Driver.FindElement(By.TagName("thead")).FindElements(By.TagName("th")).TakeWhile(el => el.Text != key).Count();
            var element = Driver.FindElements(By.CssSelector("tbody > tr:first-child > td"))[index];
            element.Text.should_be(value);
        }

        [Then(@"the discount should not be combinable")]
        public void ThenTheDiscountShouldNotBeCombinable()
        {
            ScenarioContext.Current.Pending();
        }
    }
}