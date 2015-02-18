using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Raven.Client;

namespace Domain.Repositories
{
    /// <summary>
    ///     Repository for storing or reading coupons to and from RavenDB
    /// </summary>
    public class CouponRepository
    {
        private readonly IDocumentSession _session;

        public CouponRepository(IDocumentSession session)
        {
            _session = session;
        }

        /// <summary>
        ///     Get all the coupons from the database
        ///     TODO: Needs a limit (paging).
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Coupon> FindAllCoupons()
        {
            return _session.Query<Coupon>();
        }

        public IEnumerable<Coupon> FindActiveCoupons()
        {
            return _session.Query<Coupon>().Where(coupon => coupon.IsActive);
        }

        public IEnumerable<Coupon> FindNotActiveCoupons()
        {
            return _session.Query<Coupon>().Where(coupon => coupon.IsActive == false);
        }

        /// <summary>
        ///     Finds a coupon by the coupon code
        /// </summary>
        public Coupon FindByCode(String code)
        {
            return _session.Query<Coupon>()
                .FirstOrDefault(coupon => coupon.Code == code);
        }

        /// <summary>
        ///     Finds all coupons that is valid for a customer with the specified social security number
        /// </summary>
        public IQueryable<Coupon> FindBySocialSecurityNumber(String ssn)
        {
            return _session.Query<Coupon>()
                .Where(
                    coupon =>
                        coupon.CustomersValidFor.Any(
                            customer => customer.SocialSecurityNumber == ssn));
        }

        /// <summary>
        ///     Finds all coupons that is valid for a customer with the specified email
        /// </summary>
        public IQueryable<Coupon> FindByEmail(String email)
        {
            return _session.Query<Coupon>()
                .Where(
                    coupon =>
                        coupon.CustomersValidFor.Any(customer => customer.Email == email));
        }

        /// <summary>
        ///     Finds all coupons that is valid for the specified product
        /// </summary>
        public IQueryable<ProductCoupon> FindByProduct(Product product)
        {
            return _session.Query<ProductCoupon>()
                .Where(
                    coupon =>
                        coupon.Products.Any(p => p.ProductId == product.ProductId));
        }

        public IEnumerable<Coupon> FindCouponsByPage(int page, int pageSize)
        {
            return FindActiveCoupons().OrderBy(c => c.Start).Reverse().Skip(page * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        ///     Creates or updates the coupon
        /// </summary>
        public void Store(Coupon coupon)
        {
            _session.Store(coupon);
        }

        /// <summary>
        ///     Save changes specified with the Store method
        /// </summary>
        public void SaveChanges()
        {
            _session.SaveChanges();
        }
    }
}