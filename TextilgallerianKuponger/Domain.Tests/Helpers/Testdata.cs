using System;
using System.Collections.Generic;
using Domain.Entities;
using Faker;

namespace Domain.Tests.Helpers
{
    public static class Testdata
    {
        private static readonly Random Random = new Random();

        /// <summary>
        ///     Generates an empty cart object (no rows) optionally with a code and/or customer
        /// </summary>
        /// <param name="providedCode">An optional code that have been provided by the customer on checkout</param>
        /// <param name="customerCheckingOut">Optionally the customer checking out</param>
        /// <returns>A valid but empty Cart object</returns>
        public static Cart EmptyCart(
            String providedCode = null,
            Customer customerCheckingOut = null)
        {
            return new Cart
            {
                CouponCode = providedCode,
                Customer = customerCheckingOut ?? new Customer(),
                Rows = new List<Row>(),
                Discounts = new List<Coupon>()
            };
        }

        /// <summary>
        ///     Generates a cart with randmozed amoutn of rows and customer data
        /// </summary>
        /// <param name="providedCode">Optional for overriding the random behaviour</param>
        /// <param name="customerCheckingOut">Optional for overriding the random behaviour</param>
        /// <param name="rows">Optional for overriding the random behaviour</param>
        public static Cart RandomCart(
            String providedCode = null,
            Customer customerCheckingOut = null,
            List<Row> rows = null)
        {
            if (rows == null)
            {
                rows = RandomAmount(
                    () => new Row
                    {
                        Amount = Random.Next(1, 20),
                        Product = RandomProduct()
                    });
            }

            return new Cart
            {
                CouponCode = providedCode ?? Internet.UserName(),
                Customer = customerCheckingOut ?? RandomCustomer(),
                Rows = rows,
                Discounts = new List<Coupon>()
            };
        }

        /// <summary>
        ///     Generates a coupon of random type with random data
        /// </summary>
        /// <param name="code">Optional for overriding the random code</param>
        /// <param name="customer">A customer the coupon should be valid for</param>
        /// <param name="customers">A list of customers the coupon should be valid for</param>
        public static Coupon RandomCoupon(String code = null, Customer customer = null, List<Customer> customers = null)
        {
            if (customer != null)
            {
                customers = RandomAmount(() => RandomCustomer());
                customers.Add(customer);
            }
            switch (Random.Next(4))
            {
                case 0:
                    return RandomCoupon(
                        new BuyProductXRecieveProductY
                        {
                            Code = code,
                            CustomersValidFor = customers
                        });
                case 1:
                    return RandomCoupon(
                        new BuyXProductsPayForYProducts
                        {
                            Code = code,
                            CustomersValidFor = customers
                        });
                case 2:
                    return RandomCoupon(
                        new TotalSumAmountDiscount
                        {
                            Code = code,
                            CustomersValidFor = customers
                        });
                case 3:
                    return RandomCoupon(
                        new TotalSumPercentageDiscount
                        {
                            Code = code,
                            CustomersValidFor = customers
                        });
                default:
                    // Should never happen
                    throw new Exception("Switch case not correct");
            }
        }

        /// <summary>
        ///     Generates a coupon of specified type with random data
        /// </summary>
        /// <param name="template">Optional template, specified values will not be overwritten</param>
        /// <param name="canBeCombined">Optional flag if the genereted coupon can be combined</param>
        public static T RandomCoupon<T>(T template = null, bool? canBeCombined = null, bool validForEveryone = false)
            where T : Coupon, new()
        {
            var coupon = template ?? new T();

            if (coupon is BuyProductXRecieveProductY)
            {
                var c = coupon as BuyProductXRecieveProductY;
                c.FreeProduct = c.FreeProduct ?? RandomProduct();
            }
            else if (coupon is BuyXProductsPayForYProducts)
            {
                var c = coupon as BuyXProductsPayForYProducts;
                c.Buy = (c.Buy > 0) ? c.Buy : Random.Next(10);
                c.PayFor = (c.PayFor > 0)
                    ? c.PayFor
                    : Decimal.Round(
                        Decimal.Parse(
                            (Random.NextDouble()*Double.Parse(c.Buy.ToString())).ToString()));
            }
            else if (coupon is TotalSumAmountDiscount)
            {
                var c = coupon as TotalSumAmountDiscount;
                c.Amount = (c.Amount > 0)
                    ? c.Amount
                    : Decimal.Round(Decimal.Parse((Random.NextDouble()*5000).ToString()));
            }
            else if (coupon is TotalSumPercentageDiscount)
            {
                var c = coupon as TotalSumPercentageDiscount;
                c.Percentage = (c.Percentage > 0)
                    ? c.Percentage
                    : Decimal.Round(
                        Decimal.Parse(
                            Random.NextDouble()
                                .ToString()),
                        2);
            }

            if (coupon is ProductCoupon)
            {
                var c = coupon as ProductCoupon;
                c.Products = c.Products ?? RandomAmount(RandomProduct);
            }

            coupon.Code = coupon.Code ?? Internet.UserName();
            coupon.CanBeCombined = canBeCombined ?? Random.Next(2) > 0;
            coupon.CustomersValidFor = coupon.CustomersValidFor ??
                                       (validForEveryone ? null : RandomAmount(() => RandomCustomer()));
            coupon.CustomersUsedBy = coupon.CustomersUsedBy ?? RandomAmount(() => RandomCustomer());
            coupon.Start = (coupon.Start > new DateTime())
                ? coupon.Start
                : DateTime.Now.AddDays(Random.Next(-10, 10));
            coupon.End = coupon.End ?? ((Random.Next(2) > 0)
                ? coupon.Start.AddDays(Random.Next(1, 20))
                : (DateTime?) null);
            coupon.IsActive = true;

            return coupon;
        }

        /// <summary>
        ///     Generates a customer filled with random data
        /// </summary>
        public static Customer RandomCustomer(String email = null, String socialSecurityNumber = null)
        {
            return new Customer
            {
                CouponUses = Random.Next(10),
                Email = email ?? Internet.Email(),
                SocialSecurityNumber = socialSecurityNumber ?? Phone.Number()
            };
        }

        /// <summary>
        ///     Generates product filled with random data
        /// </summary>
        public static Product RandomProduct()
        {
            return new Product
            {
                Name = Internet.UserName(),
                ProductId = Phone.Number()
            };
        }

        /// <summary>
        ///     Calls factory a random number of times, stores the rsult in a list and returns it
        /// </summary>
        /// <param name="factory">Should return an object that is stored in the returned list</param>
        /// <param name="min">Optionally the minimal times factory will be called</param>
        /// <param name="max">Optionally the maximal amount of times factory will be called</param>
        public static List<T> RandomAmount<T>(Func<T> factory, int min = 0, int max = 20)
        {
            var amount = Random.Next(min, max);
            var data = new List<T>(amount);
            for (var i = 0; i < amount; i++)
            {
                data.Add(factory());
            }
            return data;
        }

        /// <summary>
        ///     Generates faked users with the same password and isactive to true
        /// </summary>
        /// <returns></returns>
        public static User RandomUser()
        {
            return new User
            {
                Email = Internet.Email(),
                IsActive = true,
                Password = "password"
            };
        }
    }
}