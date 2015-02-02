using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Domain.Entities;
using Domain.Repositories;

namespace Api.Controllers
{
    public class CartController : ApiController
    {
        private readonly CouponRepository _couponRepository;

        public CartController(CouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        // GET api/cart/5
        public void Get(int id)
        {
            _couponRepository.Store(
                new TotalSumPercentageDiscount
                {
                    Percentage = 10,
                    Code = String.Format("EASTER15"),
                    CustomersValidFor =
                        new List<Customer>
                        {
                            new Customer {Email = String.Format("{0}@test.com", id)}
                        }
                });
            _couponRepository.SaveChanges();
        }

        // POST api/cart
        public IEnumerable<Coupon> Post([FromBody] Cart cart)
        {
            // List with all coupons that may be valid for this cart
            var coupons = new List<Coupon>();

            // If there is a CouponCode filled in
            if (cart.CouponCode != null)
            {
                var coupon = _couponRepository.FindByCode(cart.CouponCode);
                // If the code is valid
                if (coupon != null)
                {
                    coupons.Add(coupon);
                }
            }

            if (cart.Customer != null)
            {
                // The customer have an email
                if (cart.Customer.Email != null) 
                {
                    // Add all coupons that may be valid for this customer
                    var validCouponsByEmail = _couponRepository.FindByEmail(cart.Customer.Email);
                    coupons.AddRange(validCouponsByEmail);
                }

                // If customer have an socialsecurityNumber
                if (cart.Customer.SocialSecurityNumber != null)
                {
                    var validCouponsBySsn =
                        _couponRepository.FindBySocialSecurityNumber(
                            cart.Customer.SocialSecurityNumber);
                    coupons.AddRange(validCouponsBySsn);
                }
            }

            // Return the valid coupons
            return coupons.Distinct().Where(coupon => coupon.IsValidFor(cart));
        }
    }
}
