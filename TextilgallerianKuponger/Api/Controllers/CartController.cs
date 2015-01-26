using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Domain.Entities;
using Domain.Repositories;

namespace Api.Controllers
{
    public class CartController : ApiController
    {
        private readonly CouponRepository couponRepository;

        public CartController(CouponRepository couponRepository)
        {
            this.couponRepository = couponRepository;
        }

        // GET api/cart
        public IEnumerable<Coupon> Get()
        {
            return Post();
        }

        // GET api/cart/5
        public void Get(int id)
        {
            couponRepository.Store(new BuyXProductsPayForYProducts
            {
                Code = String.Format("Id{0}", id),
                CustomersValidFor = new List<Customer> { new Customer { Email = String.Format("{0}@test.com", id)} }
            });
            couponRepository.SaveChanges();
        }

        // POST api/cart
        public IEnumerable<Coupon> Post()
        {
            return couponRepository.FindByEmail("2@test.com");
        }
    }
}
