using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Repositories;

namespace Domain.Services
{
    public class CouponService
    {
        private readonly CouponRepository _couponRepository;

        public CouponService(CouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        /// <summary>
        ///     Finds the best combination of coupons avalible for the cart
        /// </summary>
        public Cart FindBestCouponsForCart(Cart cart)
        {
            var coupons = FindByCart(cart).ToList();
            var possibleCarts = new List<Cart>();
            var cartWithCombinableCoupons = (Cart) cart.Clone();

            while (coupons.Count > 0)
            {
                var coupon = coupons[0];
                coupons.RemoveAt(0);

                if (coupon.CanBeCombined)
                {
                    cartWithCombinableCoupons.Discounts.Add(coupon);
                }
                else
                {
                    var notCombinableCart = (Cart) cart.Clone();
                    notCombinableCart.Discounts = new List<Coupon> {coupon};
                    possibleCarts.Add(notCombinableCart);
                }
            }

            possibleCarts.Add(cartWithCombinableCoupons);

            return possibleCarts.OrderBy(c => c.DiscountedSum).First();
        }

        /// <summary>
        ///     Finds all coupons that is valid for the provided cart
        /// </summary>
        private IEnumerable<Coupon> FindByCart(Cart cart)
        {
            var coupons = new List<Coupon>();

            if (cart.CouponCode != null)
            {
                var coupon = _couponRepository.FindByCode(cart.CouponCode);
                if (coupon != null)
                {
                    coupons.Add(coupon);
                }
                coupon = _couponRepository.FindByCustomerCode(cart.CouponCode);
                if (coupon != null)
                {
                    coupons.Add(coupon);
                }
            }

            if (cart.Customer != null)
            {
                coupons.AddRange(FindByCustomer(cart.Customer));
            }

            // Return distinct coupons so that we don't get doublettes for coupons that have both the provided code
            // or matches the current customer
            return coupons.Distinct().Where(coupon => coupon.IsValidFor(cart));
        }

        /// <summary>
        ///     Finds all coupons that may be valid for a customer
        /// </summary>
        private IEnumerable<Coupon> FindByCustomer(Customer customer)
        {
            var coupons = new List<Coupon>();

            // If the  customer have an email
            if (customer.Email != null)
            {
                // Add all coupons that may be valid for this customer
                var validCouponsByEmail = _couponRepository.FindByEmail(customer.Email);
                coupons.AddRange(validCouponsByEmail);
            }

            // If the customer have an socialsecurityNumber
            if (customer.SocialSecurityNumber != null)
            {
                // Add all coupons that may be valid for this customer
                var validCouponsBySsn =
                    _couponRepository.FindBySocialSecurityNumber(
                        customer.SocialSecurityNumber);
                coupons.AddRange(validCouponsBySsn);
            }

            // Return distinct coupons so that we don't get doublettes for coupons that have both the customers
            // email and SSN
            return coupons.Distinct();
        }
    }
}