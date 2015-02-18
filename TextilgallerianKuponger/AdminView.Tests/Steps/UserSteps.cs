using System;
using AdminView.Tests.Steps;
using NSpec;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace AdminView.Tests
{
    [Binding]
    internal class UserSteps : CommonSteps
    {
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

        [Then(@"a new User named ""(.*)"" should exist")]
        public void ThenANewUserNamedShouldExist(string name)
        {
            Driver.FindElement(By.ClassName("name")).Text.should_be(name);
        }

        [Then(@"the ""(.*)"" of the new User should be ""(.*)""")]
        public void ThenAWithValueShouldExist(String name, String value)
        {
            Driver.FindElement(By.ClassName(name)).Text.should_be(value);
        }

        [Given(@"I am on the users page")]
        public void GivenIAmOnTheUsersPage()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I enter ""(.*)"" in the ""(.*)"" field")]
        public void WhenIEnterInTheField(string p0, string p1)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I have entered ""(.*)"" in the ""(.*)"" field")]
        public void WhenIHaveEnteredInTheField(string p0, string p1)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I have selected ""(.*)"" in the ""(.*)"" dropdown")]
        public void WhenIHaveSelectedInTheDropdown(string p0, string p1)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"a user with email ""(.*)"" and password ""(.*)"" should exist")]
        public void ThenAUserWithEmailAndPasswordShouldExist(string p0, string p1)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the user ""(.*)"" should have the role ""(.*)""")]
        public void ThenTheUserShouldHaveTheRole(string p0, string p1)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"no user with email ""(.*)"" should exist")]
        public void ThenNoUserWithEmailShouldExist(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I am signed in as an user with role ""(.*)""")]
        public void GivenIAmSignedInAsAnUserWithRole(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I am on the users page")]
        public void WhenIAmOnTheUsersPage()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the link ""(.*)"" should be missing")]
        public void ThenTheLinkShouldBeMissing(string p0)
        {
            ScenarioContext.Current.Pending();
        }
    }
}