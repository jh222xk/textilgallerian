using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSpec;

namespace Api.Tests.E2E
{
    [TestClass]
    public class BasicCouponRules : ApiTest
    {
        [TestMethod]
        public void TestThatAnInactiveCouponIsNotReturned()
        {
            AssertCoupon("Inactive");
            var response = Check(new Cart
            {
                CouponCode = "Inactive",
                Rows = new List<Row>(),
            });
            
            response.CouponCode.should_be("Inactive");
            response.Customer.should_be_null();
            response.Rows.Count.should_be(0);
            response.Discounts.Count.should_be(0);
            response.TotalSum.should_be(0);
            response.DiscountedSum.should_be(0);
            response.Discount.should_be(0);
            response.NumberOfProducts.should_be(0);
        }

        [TestMethod]
        public void TestThatAnActiveCouponIsReturned()
        {
            var response = Check(new Cart
            {
                CouponCode = "Active",
                Rows = new List<Row>(),
            });

            response.CouponCode.should_be("Active");
            response.Customer.should_be_null();
            response.Rows.Count.should_be(0);
            response.Discounts.Count.should_be(1);
            response.Discounts[0].should_cast_to<TotalSumAmountDiscount>();
            response.Discounts[0].Name.should_be("TestCoupon");
            response.Discounts[0].Description.should_be("Active but empty coupon");
            response.Discounts[0].Type.should_be("TotalSumAmountDiscount");
            response.Discounts[0].CanBeCombined.should_be(false);
            response.Discounts[0].Code.should_be("Active");
            response.Discounts[0].CreatedAt.should_be_less_than(DateTime.Now);
            response.Discounts[0].CreatedAt.should_be_greater_than(DateTime.Now.AddMinutes(-5));
            response.Discounts[0].End.HasValue.should_be(false);
            response.Discounts[0].CreatedBy.should_be("some@test.com");
            response.Discounts[0].CustomersValidFor.should_be_null();
            response.Discounts[0].CustomersUsedBy.Count.should_be(0);
            response.Discounts[0].MinPurchase.should_be(0);
            response.TotalSum.should_be(0);
            response.DiscountedSum.should_be(0);
            response.Discount.should_be(0);
            response.NumberOfProducts.should_be(0);
        }

        [TestMethod]
        public void TestThatAnCouponWithANotReachedMinPurchaseIsNotReturned()
        {
            AssertCoupon("MinPurchase");
            var response = Check(new Cart
            {
                CouponCode = "MinPurchase",
                Rows = new List<Row>(),
            });

            response.CouponCode.should_be("MinPurchase");
            response.Customer.should_be_null();
            response.Rows.Count.should_be(0);
            response.Discounts.Count.should_be(0);
            response.TotalSum.should_be(0);
            response.DiscountedSum.should_be(0);
            response.Discount.should_be(0);
            response.NumberOfProducts.should_be(0);
        }

        [TestMethod]
        public void TestThatAnActiveCouponValidOneTimeUsedTwoTimesIsNotReturnd()
        {
            var cart = new Cart
            {
                CouponCode = "UseLimit",
                Rows = new List<Row>(),
            };
            var response1 = Check(cart);
            Purched(response1);
            var response2 = Check(cart);

            response1.Discounts.Count.should_be(1);
            response2.Discounts.Count.should_be(0);
        }

        [TestMethod]
        public void TestThatAnOutdatedCouponIsNotReturned()
        {
            AssertCoupon("Outdated");
            var response = Check(new Cart
            {
                CouponCode = "Outdated",
                Rows = new List<Row>(),
            });

            response.CouponCode.should_be("Outdated");
            response.Customer.should_be_null();
            response.Rows.Count.should_be(0);
            response.Discounts.Count.should_be(0);
            response.TotalSum.should_be(0);
            response.DiscountedSum.should_be(0);
            response.Discount.should_be(0);
            response.NumberOfProducts.should_be(0);
        }

        [TestMethod]
        public void TestThatAnCouponThatIsValidForTheShoppingCustomerUsingEmailIsReturned()
        {
            var response = Check(new Cart
            {
                CouponCode = "CustomerEmail",
                Customer = new Customer
                {
                    Email = "shopping@customer.com"
                },
                Rows = new List<Row>(),
            });

            response.CouponCode.should_be("CustomerEmail");
            response.Customer.Email.should_be("shopping@customer.com");
            response.Customer.CouponCode.should_be_null();
            response.Customer.SocialSecurityNumber.should_be_null();
            response.Rows.Count.should_be(0);
            response.Discounts.Count.should_be(1);
            response.TotalSum.should_be(0);
            response.DiscountedSum.should_be(0);
            response.Discount.should_be(0);
            response.NumberOfProducts.should_be(0);
        }

        [TestMethod]
        public void TestThatAnCouponThatIsNotValidForTheShoppingCustomerUsingEmailIsNotReturned()
        {
            AssertCoupon("CustomerEmail");
            var response = Check(new Cart
            {
                CouponCode = "CustomerEmail",
                Customer = new Customer
                {
                    Email = "invalid@customer.com"
                },
                Rows = new List<Row>(),
            });

            response.CouponCode.should_be("CustomerEmail");
            response.Customer.Email.should_be("invalid@customer.com");
            response.Customer.CouponCode.should_be_null();
            response.Customer.SocialSecurityNumber.should_be_null();
            response.Rows.Count.should_be(0);
            response.Discounts.Count.should_be(0);
            response.TotalSum.should_be(0);
            response.DiscountedSum.should_be(0);
            response.Discount.should_be(0);
            response.NumberOfProducts.should_be(0);
        }

        [TestMethod]
        public void TestThatAnCouponThatIsValidForTheShoppingCustomerUsingEmailOneTimeIsNotReturnedTwoTimes()
        {
            var cart = new Cart
            {
                CouponCode = "CustomerEmail",
                Customer = new Customer
                {
                    Email = "shopping@customer.com"
                },
                Rows = new List<Row>(),
            };
            var response1 = Check(cart);
            Purched(response1);
            var response2 = Check(cart);

            response1.Discounts.Count.should_be(1);
            response2.Discounts.Count.should_be(0);
        }

        [TestMethod]
        public void TestThatAnCouponThatIsValidForTheShoppingCustomerUsingSSNIsReturned()
        {
            var response = Check(new Cart
            {
                CouponCode = "CustomerSSN",
                Customer = new Customer
                {
                    SocialSecurityNumber = "600921-1234"
                },
                Rows = new List<Row>(),
            });

            response.Customer.Email.should_be_null();
            response.Customer.CouponCode.should_be_null();
            response.Customer.SocialSecurityNumber.should_be("600921-1234");
            response.Discounts.Count.should_be(1);
        }

        [TestMethod]
        public void TestThatAnCouponThatIsNotValidForTheShoppingCustomerUsingSSNIsNotReturned()
        {
            AssertCoupon("CustomerSSN");
            var response = Check(new Cart
            {
                CouponCode = "CustomerSSN",
                Customer = new Customer
                {
                    SocialSecurityNumber = "600921-9876"
                },
                Rows = new List<Row>(),
            });

            response.Customer.Email.should_be_null();
            response.Customer.CouponCode.should_be_null();
            response.Customer.SocialSecurityNumber.should_be("600921-9876");
            response.Discounts.Count.should_be(0);
        }

        [TestMethod]
        public void TestThatAnCouponThatIsValidForTheShoppingCustomerUsingSSNOneTimeIsNotReturnedTwoTimes()
        {
            var cart = new Cart
            {
                CouponCode = "CustomerSSN",
                Customer = new Customer
                {
                    SocialSecurityNumber = "600921-1234"
                },
                Rows = new List<Row>(),
            };
            var response1 = Check(cart);
            Purched(response1);
            var response2 = Check(cart);

            response1.Discounts.Count.should_be(1);
            response2.Discounts.Count.should_be(0);
        }

        [TestMethod]
        public void TestThatAnCouponThatIsValidForTheShoppingCustomerUsingCodeIsReturned()
        {
            var response = Check(new Cart
            {
                CouponCode = "CustomerCode",
                Rows = new List<Row>(),
            });

            response.Customer.should_be_null();
            response.Discounts.Count.should_be(1);
        }

        [TestMethod]
        public void TestThatAnCouponThatIsNotValidForTheShoppingCustomerUsingCodeIsNotReturned()
        {
            AssertCouponByCustomerCode("CustomerCode");
            var response = Check(new Cart
            {
                CouponCode = "CustomerCode2",
                Rows = new List<Row>(),
            });

            response.Customer.should_be_null();
            response.Discounts.Count.should_be(0);
        }

        [TestMethod]
        public void TestThatAnCouponThatIsValidForTheShoppingCustomerUsingCodeOneTimeIsNotReturnedTwoTimes()
        {
            var cart = new Cart
            {
                CouponCode = "CustomerCode",
                Rows = new List<Row>(),
            };
            var response1 = Check(cart);
            Purched(response1);
            var response2 = Check(cart);

            response1.Discounts.Count.should_be(1);
            response2.Discounts.Count.should_be(0);
        }

        [TestMethod]
        public void TestThatAnCouponThatIsUsedByTheShoppingCustomerUsingEmailIsNotReturned()
        {
            AssertCoupon("UsedCustomerEmail");
            var response = Check(new Cart
            {
                CouponCode = "UsedCustomerEmail",
                Customer = new Customer
                {
                    Email = "shopping@customer.com"
                },
                Rows = new List<Row>(),
            });

            response.Discounts.Count.should_be(0);
        }

        [TestMethod]
        public void TestThatAnCouponThatIsUsedByTheShoppingCustomerUsingSSNIsNotReturned()
        {
            AssertCoupon("UsedCustomerSSN");
            var response = Check(new Cart
            {
                CouponCode = "UsedCustomerSSN",
                Customer = new Customer
                {
                    SocialSecurityNumber = "600921-1234"
                },
                Rows = new List<Row>(),
            });

            response.Discounts.Count.should_be(0);
        }

        [TestMethod]
        public void TestThatAnCouponThatIsUsedByTheShoppingCustomerUsingCodeIsNotReturned()
        {
            AssertCouponByCustomerCode("UsedCustomerCode");
            var response = Check(new Cart
            {
                CouponCode = "UsedCustomerCode",
                Rows = new List<Row>(),
            });

            response.Discounts.Count.should_be(0);
        }

        [TestMethod]
        public void TestThatAnCouponThatIsUsedByTheShoppingCustomerUsingEmailOneOfTwoTimesIsReturned()
        {
            var response = Check(new Cart
            {
                CouponCode = "UsedCustomerEmail2",
                Customer = new Customer
                {
                    Email = "shopping@customer.com"
                },
                Rows = new List<Row>(),
            });

            response.Discounts.Count.should_be(1);
        }

        [TestMethod]
        public void TestThatAnCouponThatIsUsedByTheShoppingCustomerUsingSSNOneOfTwoTimesIsReturned()
        {
            var response = Check(new Cart
            {
                CouponCode = "UsedCustomerSSN2",
                Customer = new Customer
                {
                    SocialSecurityNumber = "600921-1234"
                },
                Rows = new List<Row>(),
            });

            response.Discounts.Count.should_be(1);
        }

        [TestMethod]
        public void TestThatAnCouponThatIsUsedByTheShoppingCustomerUsingCodeOneOfTwoTimesIsReturned()
        {
            var response = Check(new Cart
            {
                CouponCode = "UsedCustomerCode2",
                Rows = new List<Row>(),
            });

            response.Discounts.Count.should_be(1);
        }

        [TestMethod]
        public void TestThatAnCouponThatIsUsedByTheShoppingCustomerUsingEmailOneOfTwoTimesIsNotReturnedTwoTimes()
        {
            var cart = new Cart
            {
                CouponCode = "UsedCustomerEmail2",
                Customer = new Customer
                {
                    Email = "shopping@customer.com"
                },
                Rows = new List<Row>(),
            };
            var response1 = Check(cart);
            Purched(response1);
            var response2 = Check(cart);

            response1.Discounts.Count.should_be(1);
            response2.Discounts.Count.should_be(0);
        }

        [TestMethod]
        public void TestThatAnCouponThatIsUsedByTheShoppingCustomerUsingSSNOneOfTwoTimesIsNotReturnedTwoTimes()
        {
            var cart = new Cart
            {
                CouponCode = "UsedCustomerSSN2",
                Customer = new Customer
                {
                    SocialSecurityNumber = "600921-1234"
                },
                Rows = new List<Row>(),
            };
            var response1 = Check(cart);
            Purched(response1);
            var response2 = Check(cart);

            response1.Discounts.Count.should_be(1);
            response2.Discounts.Count.should_be(0);
        }

        [TestMethod]
        public void TestThatAnCouponThatIsUsedByTheShoppingCustomerUsingCodeOneOfTwoTimesIsNotReturnedTwoTimes()
        {
            var cart = new Cart
            {
                CouponCode = "UsedCustomerCode2",
                Rows = new List<Row>(),
            };
            var response1 = Check(cart);
            Purched(response1);
            var response2 = Check(cart);

            response1.Discounts.Count.should_be(1);
            response2.Discounts.Count.should_be(0);
        }
    }
}
