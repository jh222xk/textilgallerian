using System;
using System.Collections.Generic;
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

        public CartController(CouponRepository couponRepository, CouponService couponService)
        {
            _couponRepository = couponRepository;
            _couponService = couponService;
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
        public Cart Post([FromBody] Cart cart)
        {
            return _couponService.FindBestCouponsForCart(cart);
        }
    }
}