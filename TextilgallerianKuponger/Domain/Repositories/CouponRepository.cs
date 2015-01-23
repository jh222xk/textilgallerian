using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Raven.Client;

namespace Domain.Repositories {
    public class CouponRepository {
        private readonly IDocumentSession session;

        public CouponRepository(IDocumentSession session) {
            this.session = session;
        }

        public void Store(Coupon coupon) {
            session.Store(coupon);
        }

        public Coupon FindByCode(String code) {
            return session.Query<Coupon>()
                          .FirstOrDefault(coupon => coupon.Code == code);
        }

        public IQueryable<Coupon> FindBySocialSecurityNumber(String ssn) {
            return session.Query<Coupon>()
                          .Where(coupon => coupon.CustomersValidFor.Any(customer => customer.SocialSecurityNumber == ssn));
        }

        public IQueryable<Coupon> FindByEmail(String email) {
            return session.Query<Coupon>()
                          .Where(coupon => coupon.CustomersValidFor.Any(customer => customer.Email == email));
        }

        public void SaveChanges() {
            session.SaveChanges();
        }
    }
}
