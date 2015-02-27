using System.Web.Http;
using Domain.Entities;
using Domain.Services;

namespace Api.Controllers
{
    public class CartController : ApiController
    {
        private readonly CouponService _couponService;

        public CartController(CouponService couponService)
        {
            _couponService = couponService;
        }

        // POST api/cart
        public Cart Post([FromBody] Cart cart)
        {
            return _couponService.FindBestCouponsForCart(cart);
        }
    }
}