using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client.Embedded;

namespace Domain.Tests.Repositories {
    [TestClass]
    public class CouponRepositoryTest {
        private CouponRepository couponRepository;

        [TestInitialize]
        public void SetUp() {
            // Creates a database in memory that only exists during the test
            var store = new EmbeddableDocumentStore {
                Configuration = {
                    RunInUnreliableYetFastModeThatIsNotSuitableForProduction = true,
                    RunInMemory = true,
                }
            };

            store.Initialize();
            var session = store.OpenSession();

            couponRepository = new CouponRepository(session);

            // Some demo data to help testing
            couponRepository.Store(new BuyProductXRecieveProductY {
                CanBeCombined = true,
                Code = "XMAS15",
                CustomersUsedBy = new List<Customer>(),
                CustomersValidFor = new List<Customer> {
                    new Customer {
                        Email = "email@example.com",
                        SocialSecurityNumber = "831017-2732",
                    },
                    new Customer {
                        Email = "some@email.com",
                        SocialSecurityNumber = "700131-2371",
                    }
                },
                Start = DateTime.Now,
                End = DateTime.Now
            });

            couponRepository.Store(new BuyXProductsPayForYProducts {
                CanBeCombined = true,
                Code = "XMAS14",
                CustomersUsedBy = new List<Customer>(),
                CustomersValidFor = new List<Customer> {
                    new Customer {
                        SocialSecurityNumber = "831017-2732",
                    },
                    new Customer {
                        Email = "some@email.com",
                        SocialSecurityNumber = "700131-2371",
                    }
                },
                Start = DateTime.Now,
                End = DateTime.Now
            });

            couponRepository.Store(new TotalSumAmountDiscount {
                CanBeCombined = true,
                Code = "ihafi7Hsda",
                CustomersUsedBy = new List<Customer>(),
                CustomersValidFor = new List<Customer> {
                    new Customer {
                        Email = "other@email.com",
                        SocialSecurityNumber = "831017-2732",
                    },
                    new Customer {
                        SocialSecurityNumber = "700131-2371",
                    },
                },
                Start = DateTime.Now,
                End = DateTime.Now
            });

            couponRepository.SaveChanges();
        }

        [TestMethod]
        public void TestGettingACouponByCode() {
            var coupon = couponRepository.FindByCode("XMAS15");
            Assert.IsInstanceOfType(coupon, typeof(BuyProductXRecieveProductY));
            Assert.AreEqual("XMAS15", coupon.Code);
            Assert.AreEqual(2, coupon.CustomersValidFor.Count);
        }

        [TestMethod]
        public void TestGettingCouponsByEmail() {
            var coupons = couponRepository.FindByEmail("some@email.com").ToList();
            Assert.AreEqual(2, coupons.Count);

            foreach (var coupon in coupons) {
                Assert.IsInstanceOfType(coupon, typeof(Coupon));
                Assert.IsTrue(coupon.CustomersValidFor.Any(customer => customer.Email == "some@email.com"));
            }

            Assert.AreEqual("XMAS15", coupons[0].Code);
            Assert.AreEqual("XMAS14", coupons[1].Code);
        }
    }
}
