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

        /// <summary>
        ///     Creates or updates the coupon
        /// </summary>
        public void Store(Coupon coupon)
        {
            _session.Store(coupon);
        }

        /// <summary>
        ///     Removes the given coupon
        /// </summary>
        public void Delete(Coupon coupon)
        {
            _session.Delete(coupon);
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