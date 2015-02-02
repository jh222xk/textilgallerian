using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Domain.Entities;
using Domain.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Api.Controllers
{
    public class CartController : ApiController
    {
        private readonly CouponRepository couponRepository;

        public CartController(CouponRepository couponRepository)
        {
            this.couponRepository = couponRepository;
        }

        // GET api/cart/5
        public void Get(int id)
        {
            couponRepository.Store(new TotalSumPercentageDiscount
            {
                Percentage = 10,
                Code = String.Format("EASTER15"),
                CustomersValidFor = new List<Customer> { new Customer { Email = String.Format("{0}@test.com", id)} }
            });
            couponRepository.SaveChanges();
        }

        // POST api/cart
        public IEnumerable<Coupon> Post([FromBody] Cart cart)
        {
            // List with all coupons that may be valid for this cart
            var coupons = new List<Coupon>();

            // If there is a CouponCode filled in
            if (cart.CouponCode != null)
            {
                var coupon = couponRepository.FindByCode(cart.CouponCode);
                // If the code is valid
                if (coupon != null)
                {
                    coupons.Add(coupon);
                }
            }

            if (cart.Customer != null) {

                // Question: is there a risk to store same coupon mutiple times if saving by both email and SSN?

                // The customer have an email
                if (cart.Customer.Email != null) {
                    // Add all coupons that may be valid for this customer
                    var validCouponsByEmail = couponRepository.FindByEmail(cart.Customer.Email);
                    coupons.AddRange(validCouponsByEmail);
                }
                // If customer have an socialsecurityNumber
                if (cart.Customer.SocialSecurityNumber != null) {
                    var validCouponsBySSN =
                        couponRepository.FindBySocialSecurityNumber(cart.Customer.SocialSecurityNumber);
                    coupons.AddRange(validCouponsBySSN);
                }
            }


            // Return the valid coupons
            return coupons.Where(coupon => coupon.IsValidFor(cart));
        }
    }
}
