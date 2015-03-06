using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services;

namespace Api.Controllers
{
    public class CartController : ApiController
    {
        private readonly CouponRepository _couponRepository;
        private readonly CouponService _couponService;

        public CartController(CouponService couponService, CouponRepository couponRepository)
        {
            _couponService = couponService;
            _couponRepository = couponRepository;
        }

        public Cart Post([FromBody] Cart cart)
        {
            return _couponService.FindBestCouponsForCart(cart);
        }

        [Route("purchased")]
        public Boolean Purchased([FromBody] Cart cart)
        {
            var coupons = _couponRepository.FindByUniqueKeys(cart.Discounts.Select(c => c.UniqueKey));

            foreach (var coupon in coupons)
            {
                if (coupon.CustomersValidFor == null)
                {
                    if (coupon.UseLimit.HasValue)
                    {
                        coupon.UseLimit = coupon.UseLimit.Value - 1;
                    }

                    if (cart.Customer != null)
                    {
                        var customer = FindCustomer(cart.Customer, coupon.CustomersUsedBy);
                        if (customer == null)
                        {
                            customer = cart.Customer;
                            customer.CouponUses = 1;
                            coupon.CustomersUsedBy.Add(customer);
                        }
                        else
                        {
                            customer.CouponUses++;
                        }
                    }
                }
                else
                {
                    var customerValid = FindCustomer(cart.Customer, coupon.CustomersValidFor, cart.CouponCode);
                    var customerUsed = FindCustomer(cart.Customer, coupon.CustomersUsedBy, cart.CouponCode);

                    customerValid.CouponUses++;
                    if (customerUsed == null)
                    {
                        customerUsed = new Customer
                        {
                            CouponCode = customerValid.CouponCode,
                            Email = customerValid.Email,
                            SocialSecurityNumber = customerValid.SocialSecurityNumber,
                            CouponUses = 1,
                        };
                        coupon.CustomersUsedBy.Add(customerUsed);
                    }
                    else
                    {
                        customerUsed.CouponUses++;
                    }
                }
                _couponRepository.Store(coupon);
            }

            _couponRepository.SaveChanges();
            return true;
        }

        private static Customer FindCustomer(Customer customer, List<Customer> customers, String code = null)
        {

            if (customer == null)
            {
                return customers.Find(cust => (cust.CouponCode == code && cust.CouponCode != null));
            }

            return customers.Find(cust => (cust.Email == customer.Email && cust.Email != null) ||
                                          (cust.SocialSecurityNumber == customer.SocialSecurityNumber && cust.SocialSecurityNumber != null));
        }
    }
}