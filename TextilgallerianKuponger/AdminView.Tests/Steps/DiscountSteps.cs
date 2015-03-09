using System;
using System.Collections.Generic;
using System.Linq;
using AdminView.Tests.Helpers;
using AdminView.Tests.Steps;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSpec;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace AdminView.Tests
{
    [Binding]
    public class DiscountSteps : Base
    {
        private readonly CommonSteps _common = new CommonSteps();

        [TestInitialize]
        public override void CleanupCoupons()
        {
            base.CleanupCoupons();
        }

        [Given(@"I am on the add new discount page")]
        public void GivenIAmOnTheAddNewDiscountPage()
        {
            _common.WhenINavigateTo("/coupon/create");
        }

        [Given(@"I have selected the ""(.*)"" in the discount type field")]
        public void GivenIHaveSelectedTheInTheDiscountTypeField(String type)
        {
            Driver.PageSource.should_contain("Välj typ av kampanj");
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

        [Then(@"the Holiday Season API test should pass")]
        public void ThenTheHolidaySeasonAPITestShouldPass()
        {
            CallApi(new Cart
            {
                CouponCode = "XMAS15",
                Customer = new Customer
                {
                    SocialSecurityNumber = "900105-3001"
                },
            }).should_be_json_for(new Cart
            {
                CouponCode = "XMAS15",
                Customer = new Customer
                {
                    SocialSecurityNumber = "900105-3001"
                },
                Rows = new List<Row>(),
                Discounts = new List<Coupon>
                {
                    new TotalSumPercentageDiscount
                    {
                        Name = "Holiday Season",
                        Code = "XMAS15",
                        Description = "Test coupon",
                        CreatedBy = "editor@admin.com",
                        CustomersUsedBy = new List<Customer>(),
                        CustomersValidFor = new List<Customer>
                        {
                            new Customer
                            {
                                SocialSecurityNumber = "900105-3001"
                            }
                        },
                        DiscountOnlyOnSpecifiedProducts = false,
                        Start = new DateTime(2015, 01, 15),
                        End = new DateTime(2016, 04, 30),
                        Percentage = 0.3m,
                        CanBeCombined = true,
                        UseLimit = 2,
                        IsActive = true,
                    }
                },
            });
        }

        [Then(@"the Holiday Season 16 API test should pass")]
        public void ThenTheHolidaySeason16APITestShouldPass()
        {
            CallApi(new Cart
            {
                CouponCode = "XMAS16",
                Customer = new Customer
                {
                    Email = "customer@email.com"
                },
                Rows = new List<Row>
                {
                    new Row
                    {
                        Amount = 10,
                        Product = new Product
                        {
                            Name = "Egg",
                            ProductId = "Egg",
                        },
                        ProductPrice = 50,
                    }
                },
            }).should_be_json_for(new Cart
            {
                CouponCode = "XMAS16",
                Customer = new Customer
                {
                    Email = "customer@email.com"
                },
                Rows = new List<Row>
                {
                    new Row
                    {
                        Amount = 10,
                        Product = new Product
                        {
                            Name = "Egg",
                            ProductId = "Egg",
                        },
                        ProductPrice = 50,
                    }
                },
                Discounts = new List<Coupon>
                {
                    new TotalSumPercentageDiscount
                    {
                        Name = "Holiday Season 16",
                        Code = "XMAS16",
                        Description = "Test coupon",
                        CreatedBy = "editor@admin.com",
                        CustomersUsedBy = new List<Customer>(),
                        CustomersValidFor = new List<Customer>
                        {
                            new Customer
                            {
                                Email = "customer@email.com"
                            }
                        },
                        DiscountOnlyOnSpecifiedProducts = false,
                        Start = new DateTime(2014, 04, 15),
                        End = new DateTime(2016, 04, 30),
                        Percentage = 0.3m,
                        CanBeCombined = true,
                        UseLimit = 2,
                        IsActive = true,
                        MinPurchase = 500,
                    }
                },
            });
        }

        [Then(@"the Easter Season API test should pass")]
        public void ThenTheEasterSeasonAPITestShouldPass()
        {
            CallApi(new Cart
            {
                CouponCode = "Chicken",
                Rows = new List<Row>
                {
                    new Row
                    {
                        Amount = 10,
                        Product = new Product
                        {
                            Name = "Egg",
                            ProductId = "Egg",
                        },
                        ProductPrice = 50,
                    }
                },
            }).should_be_json_for(new Cart
            {
                CouponCode = "Chicken",
                Customer = new Customer(),
                Rows = new List<Row>
                {
                    new Row
                    {
                        Amount = 10,
                        Product = new Product
                        {
                            Name = "Egg",
                            ProductId = "Egg",
                        },
                        ProductPrice = 50,
                    }
                },
                Discounts = new List<Coupon>
                {
                    new TotalSumAmountDiscount
                    {
                        Name = "Easter Season",
                        Code = "Chicken",
                        Description = "TotalSumAmountDiscount",
                        CreatedBy = "editor@admin.com",
                        CustomersUsedBy = new List<Customer>(),
                        Start = new DateTime(2015, 01, 15),
                        Amount = 100m,
                        IsActive = true,
                        MinPurchase = 500m,
                    }
                },
            });
        }

        [Then(@"the Summer API test should pass")]
        public void ThenTheSummerAPITestShouldPass()
        {
            CallApi(new Cart
            {
                CouponCode = "Beach",
                Rows = new List<Row>
                {
                    new Row
                    {
                        Amount = 3,
                        Product = new Product
                        {
                            Name = "Egg",
                            ProductId = "Egg",
                        },
                        ProductPrice = 50,
                    }
                },
            }).should_be_json_for(new Cart
            {
                CouponCode = "Beach",
                Customer = new Customer(),
                Rows = new List<Row>
                {
                    new Row
                    {
                        Amount = 3,
                        Product = new Product
                        {
                            Name = "Egg",
                            ProductId = "Egg",
                        },
                        ProductPrice = 50,
                    }
                },
                Discounts = new List<Coupon>
                {
                    new BuyXProductsPayForYProducts
                    {
                        Name = "Summer",
                        Code = "Beach",
                        Description = "",
                        CreatedBy = "editor@admin.com",
                        CustomersUsedBy = new List<Customer>(),
                        Start = new DateTime(2014, 06, 01),
                        NumberOfProductsToBuy = 3,
                        PayFor = 2,
                        IsActive = true,
                    }
                },
            });
        }

        [Then(@"the Halloween API test should pass")]
        public void ThenTheHalloweenAPITestShouldPass()
        {
            var cart = new Cart
            {
                CouponCode = "pumpkin",
                Customer = new Customer(),
                Rows = new List<Row>
                {
                    new Row
                    {
                        Amount = 3,
                        Product = new Product
                        {
                            Name = "Egg",
                            ProductId = "Egg",
                        },
                        ProductPrice = 50,
                    }
                },
                Discounts = new List<Coupon>
                {
                    new BuyProductXRecieveProductY
                    {
                        Name = "Halloween",
                        Code = "pumpkin",
                        Description = "",
                        CreatedBy = "editor@admin.com",
                        CustomersUsedBy = new List<Customer>(),
                        Start = new DateTime(2014, 09, 01),
                        NumberOfProductsToBuy = 3,
                        IsActive = true,
                        FreeProduct = new Product
                        {
                            ProductId = "Pink Curtain",
                        },
                        AmountOfProducts = 3,
                        MinPurchase = 100
                    }
                },
            };
            cart.CalculateDiscount();

            CallApi(new Cart
            {
                CouponCode = "pumpkin",
                Rows = new List<Row>
                {
                    new Row
                    {
                        Amount = 3,
                        Product = new Product
                        {
                            Name = "Egg",
                            ProductId = "Egg",
                        },
                        ProductPrice = 50,
                    }
                },
            }).should_be_json_for(cart);
        }

    }
}