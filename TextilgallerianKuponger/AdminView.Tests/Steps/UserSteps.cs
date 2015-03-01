using System;
using System.Linq;
using AdminView.Tests.Steps;
using NSpec;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace AdminView.Tests
{
    [Binding]
    internal class UserSteps : Base
    {

        private readonly CommonSteps _common = new CommonSteps();

        [Given(@"I am on the add new user page")]
        public void GivenIAmOnTheUserPage()
        {
            _common.WhenINavigateTo("/user/create");
        }

        [Given(@"And I have clicked Add new User")]
        public void GivenIHaveClickedAddNewUser()
        {
            _common.WhenIPress("Skapa användare");
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
            _common.WhenINavigateTo("/user/");
        }

        [When(@"I visit the the user creation page")]
        public void WhenIVisitTheTheUserCreationPage()
        {
            _common.WhenINavigateTo("/user/create/");
        }


        [Then(@"I should be redirected to the user page")]
        public void ThenIShouldBeRedirectedToTheUserPage()
        {
            _common.WhenINavigateTo("/user/");
        }


        [Given(@"I have selected ""(.*)"" in the ""(.*)"" dropdown")]
        public void WhenIHaveSelectedInTheDropdown(String name, String value)
        {
            Driver.FindElement(By.XPath("//option[@value = '" + name + "']")).Click();
        }

        [Then(@"a user with email ""(.*)"" should exist")]
        public void ThenAUserWithEmailShouldExist(String value)
        {
            Driver.PageSource.should_contain(value);
        }

        [Then(@"the user ""(.*)"" should have the role ""(.*)""")]
        public void ThenTheUserShouldHaveTheRole(String user, String role)
        {
            var element =
                Driver.FindElements(By.TagName("td")).SkipWhile(e => !e.Text.Contains(user)).Skip(1).First();
            element.Text.should_contain(role);
        }

        [Given(@"I am signed in as an user with role ""(.*)""")]
        public void GivenIAmSignedInAsAnUserWithRole(String role)
        {
            _common.WhenINavigateTo("/user/create");
            Driver.PageSource.should_contain(role);
        }

    }
}