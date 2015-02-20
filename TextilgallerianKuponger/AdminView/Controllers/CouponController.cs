﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
﻿using System.Text.RegularExpressions;
﻿using System.Web.Mvc;
using AdminView.Annotations;
using AdminView.ViewModel;
using Domain.Entities;
using Domain.Repositories;
using Domain.Tests.Helpers;
using Domain.ExtensionMethods;

namespace AdminView.Controllers
{
    [LoggedIn]
    public class CouponController : Controller
    {
        private readonly CouponRepository _couponRepository;

        private const int PageSize = 15;

        public CouponController(CouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        // GET: Coupon
        public ActionResult Index(int page = 1)
        {
            var model = new PagedViewModel<Coupon>
            {
                PagedObjects = _couponRepository.FindAllCoupons().OrderBy(c => c.CreatedAt).Page(page, 10),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(_couponRepository.FindAllCoupons().Count() / (double) PageSize)
            };

            //// TestData for now
            //            var tempCoupons = Testdata.RandomAmount(() => Testdata.RandomCoupon());
            //
            //            tempCoupons.ForEach(_couponRepository.Store);
            //            _couponRepository.SaveChanges();

            return View(model);
        }

        // GET: Coupon/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Coupon/Create
        public ActionResult Create()
        {
            return View(new CouponViewModel());
        }

        // POST: Coupon/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CouponViewModel model)
        {
            var type = Assembly.GetAssembly(typeof(Coupon)).GetType(model.Type);

            var lines = model.CustomerString.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var customers = GetCustomers(lines);

            if (!ModelState.IsValid) return View();
            try
            {
                // Magic super perfect code, do not touch!
                var constructor = type.GetConstructor(new[] { typeof(IReadOnlyDictionary<String, String>) });
                var coupon = constructor.Invoke(new object[] { model.Parameters }) as Coupon;
                var user = (User)Session["user"];
                coupon.CreatedBy = user.Email;
                coupon.CanBeCombined = model.CanBeCombined;
                coupon.CustomersValidFor = customers;
                coupon.IsActive = true;
                coupon.CreatedAt = DateTime.Now;
                _couponRepository.Store(coupon);
                _couponRepository.SaveChanges();
                TempData["success"] = "Rabatt sparad!";
                return RedirectToAction("Index");
            }
            catch (NullReferenceException)
            {
                throw new ArgumentException("Invalid coupon type");
            }
            catch
            {
                TempData["error"] = "Misslyckades att spara rabatten!";
            }

            return View();
        }

        // GET: Coupon/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Coupon/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: /Coupon/Delete/:code
        public ActionResult Delete(string code)
        {
            var coupon = _couponRepository.FindByCode(code);

            return View(coupon);
        }

        // POST: /Coupon/Delete/42
        /// <summary>
        ///     Not really removes the coupon
        ///     (because of statitics) https://github.com/Textilgallerian/textilgallerian/issues/53
        ///     We only set it to unactive
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string code)
        {
            try
            {
                var coupon = _couponRepository.FindByCode(code);
                // Sets the given coupon to unactive
                coupon.IsActive = false;
                _couponRepository.SaveChanges();
                TempData["success"] = "Rabatten togs bort.";
            }
            catch (DataException)
            {
                TempData["error"] = "Misslyckades att ta bort rabatten!";
                return RedirectToAction("Delete", new { id = code });
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        private static List<Customer> GetCustomers(string [] lines)
        {
            if (lines == null) throw new ArgumentNullException("lines");
            var customers = new List<Customer>();

            foreach (var line in lines)
            {
                var customer = new Customer { CouponUses = 0 };

                var mailRegex = new Regex(@"^.+?@.+?\.\w{2,8}$");
                var ssnRegex = new Regex(@"^[0-9]{6,8}-?[0-9]{4}$");

                // Match email
                if (mailRegex.Match(line).Success)
                {
                    customer.Email = line;
                    customers.Add(customer);
                }

                // Match social security number
                else if (ssnRegex.Match(line).Success)
                {
                    customer.SocialSecurityNumber = line;
                    customers.Add(customer);
                }
            }

            return customers.Count > 0 ? customers : null;
        }
    }
}