using Api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client;
using Raven.Client.Embedded;
using NSpec;

namespace Api.Tests.Controllers
{
    [TestClass]
   public class CartControllerTest
   {
       private CouponRepository couponRepository;
       private CartController cartController;
        private Cart cart;

         [TestInitialize]
         public void SetUp()
         {
             // Creates a database in memory that only exists during the test
             var store = new EmbeddableDocumentStore
             {
                 Configuration =
                 {
                     RunInUnreliableYetFastModeThatIsNotSuitableForProduction = true,
                     RunInMemory = true,
                 },
                 Conventions =
                 {
                     FindTypeTagName =
                         type => typeof(Coupon).IsAssignableFrom(type) ? "coupons" : null
                 }
             };

             store.Initialize();
             var session = store.OpenSession();

             couponRepository = new CouponRepository(session);

             cartController = new CartController(couponRepository);

             couponRepository.Store(new BuyProductXRecieveProductY {
                 Code = "XMAS15"
             });
             couponRepository.Store(new TotalSumAmountDiscount {
                 CustomersValidFor = new List<Customer> {
                     new Customer {
                         Email = "test@testmail.com"
                     }
                 }
             });
             couponRepository.Store(new BuyXProductsPayForYProducts
             {
                 CustomersValidFor = new List<Customer> {
                     new Customer {
                         SocialSecurityNumber = "8888"
                     }
                 }
             });
             couponRepository.Store(new TotalSumAmountDiscount
             {
                 CustomersValidFor = new List<Customer> {
                     new Customer {
                         Email = "test@testmail.com"
                     }
                 },
                 End = DateTime.Now.AddDays(-1)
             });
             couponRepository.Store(new BuyXProductsPayForYProducts
             {
                 CustomersValidFor = new List<Customer> {
                     new Customer {
                         SocialSecurityNumber = "8888"
                     }
                 }
             });
         }

        [TestMethod]
        public void TestThatCouponsAreQueriedByCode()
         {
             cart = new Cart
             {
                 CouponCode = "XMAS15"
             };

            var result = cartController.Post(cart).ToList();

            result.Count.should_be(1);
            result[0].Code.should_be("XMAS15");
        }
        [TestMethod]
        public void TestThatCouponsAreQueriedByEmail()
        {

            cart = new Cart
            {
                CouponCode = "XMAS15",
                Customer = new Customer
                {
                    Email = "test@testmail.com"
                }
            };

            var result = cartController.Post(cart).ToList();
            result.Count.should_be(1);
            result[0].Code.should_be("XMAS15");

            cart = new Cart
            {
                CouponCode = "XMAS15",
                Customer = new Customer
                {
                    Email = "wrongmail@testmail.com"
                }
            };

            var result2 = cartController.Post(cart).ToList();
            result2.Count.should_be(0);
            result2[0].Code.should_be("XMAS15");

        }

        [TestMethod]
        public void TestThatCouponsAreQueriedBySocialSecurityNumber()
        {

            cart = new Cart
            {
                CouponCode = "XMAS15",
                Customer = new Customer
                {
                    SocialSecurityNumber = "8888"
                }
            };

            var result = cartController.Post(cart).ToList();
            result.Count.should_be(1);
            result[0].Code.should_be("XMAS15");

            cart = new Cart
            {
                CouponCode = "XMAS15",
                Customer = new Customer
                {
                    SocialSecurityNumber = "8889"
                }
            };

            var result2 = cartController.Post(cart).ToList();
            result2.Count.should_be(0);
            result2[0].Code.should_be("XMAS15");
        }
     
    }

}
