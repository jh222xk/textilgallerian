using NSpec;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using TechTalk.SpecFlow;

namespace AdminView.Tests.Steps
{
    public class Base : nspec
    {

        protected static IWebDriver Driver;
    }
}
