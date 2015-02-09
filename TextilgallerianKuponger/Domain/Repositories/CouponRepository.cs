using System;
using System.Linq;
using Domain.Entities;
using Raven.Client;

namespace Domain.Repositories
{
    /// <summary>
    /// Repository for storing or reading coupons to and from RavenDB
    /// </summary>
    public class CouponRepository
    {
        private readonly IDocumentSession session;

        public CouponRepository(IDocumentSession session)
        {
            this.session = session;
        }

        /// <summary>
        /// Finds a coupon by the coupon code
        /// </summary>
        public Coupon FindByCode(String code)
        {
            return session.Query<Coupon>()
                          .FirstOrDefault(coupon => coupon.Code == code);
        }

        /// <summary>
        /// Finds all coupons that is valid for a customer with the specified social security number
        /// </summary>
        public IQueryable<Coupon> FindBySocialSecurityNumber(String ssn)
        {
            return session.Query<Coupon>()
                          .Where(
                              coupon =>
                              coupon.CustomersValidFor.Any(
                                  customer => customer.SocialSecurityNumber == ssn));
        }

        /// <summary>
        /// Finds all coupons that is valid for a customer with the specified email
        /// </summary>
        public IQueryable<Coupon> FindByEmail(String email)
        {
            return session.Query<Coupon>()
                          .Where(
                              coupon =>
                              coupon.CustomersValidFor.Any(customer => customer.Email == email));
        }

        /// <summary>
        /// Finds all coupons that is valid for the specified product
        /// </summary>
        public IQueryable<ProductCoupon> FindByProduct(Product product)
        {
            return session.Query<ProductCoupon>()
                          .Where(
                              coupon =>
                              coupon.Products.Any(p => p.ProductId == product.ProductId));
        }

        /// <summary>
        /// Creates or updates the coupon
        /// </summary>
        public void Store(Coupon coupon)
        {
            session.Store(coupon);
        }

        /// <summary>
        /// Save changes specified with the Store method
        /// </summary>
        public void SaveChanges()
        {
            session.SaveChanges();
        }
    }
}
