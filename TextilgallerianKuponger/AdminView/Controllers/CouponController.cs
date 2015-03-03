﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
﻿using System.Web.Mvc;
using AdminView.Annotations;
using AdminView.ViewModel;
using Domain.Entities;
using Domain.Repositories;
using Domain.ExtensionMethods;
using AdminView.Controllers.Helpers;

namespace AdminView.Controllers
{
   [LoggedIn]
    public class CouponController : Controller
    {
        private readonly CouponRepository _couponRepository;
        private readonly CouponHelper _couponHelper;

        private const int PageSize = 15;

        public CouponController(CouponRepository couponRepository, CouponHelper couponHelper)

        {
            _couponRepository = couponRepository;
            _couponHelper = couponHelper;
        }

        // GET: Coupon
        [RequiredPermission(Permission.CanListCoupons)]
        public ActionResult Index(int page = 1)
        {
            var model = new PagedViewModel<Coupon>
            {
                PagedObjects = _couponRepository.FindAllCoupons().OrderByDescending(c => c.CreatedAt).Page(page - 1, PageSize),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(_couponRepository.FindAllCoupons().Count() / (double) PageSize)
            };

            // TestData for now
            //var tempCoupons = Testdata.RandomAmount(() => Testdata.RandomCoupon());
            //tempCoupons.ForEach(_couponRepository.Store);
            //_couponRepository.SaveChanges();

            return View(model);
        }

        // GET: Coupon/Details/5
        [RequiredPermission(Permission.CanListCoupons)]
        public ActionResult Details(string uniqueKey)
        {
            var coupon = _couponRepository.FindByUniqueKey(uniqueKey);
            
            var cvm = new CouponViewModel
            {
                CanBeCombined = coupon.CanBeCombined,
                Parameters = coupon.GetProperties(),
                CustomerString =
                    coupon.CustomersValidFor != null ? _couponHelper.CreateCustomerString(coupon.CustomersValidFor) : "",
                ProductsString = coupon.Products != null ? _couponHelper.CreateProductsString(coupon.Products) : "",
                Type = ExtensionMethods.TypeExtension.Types[coupon.GetType().FullName],
                DisposableCodes = string.IsNullOrEmpty(coupon.Code),
                Customers = coupon.CustomersValidFor
            };

            return View(cvm);
        }

        // GET: Coupon/Create
        [RequiredPermission(Permission.CanAddCoupons)]
        public ActionResult Create()
        {
            return View(new CouponViewModel());
        }

        // POST: Coupon/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequiredPermission(Permission.CanAddCoupons)]
        public ActionResult Create(CouponViewModel model)
        {
            List<Customer> customers = null;
            
            if (model.DisposableCodes)
            {
                // There can't be both a campaign and disposable codes
                model.Parameters["Code"] = null;

                customers = _couponHelper.GenerateDisposableCodes(model.NumberOfCodes);
            }
            else
            {
                customers = _couponHelper.GetCustomers(model.CustomerString);
            }

            List<Product> products = _couponHelper.GetProducts(model.ProductsString);

            if (!ModelState.IsValid) return View(model);
            try
            {
                var coupon = _couponHelper.CreateCoupon(model.Type, model.Parameters);

                //user that created the coupon.
                var user = (User) Session["user"];   

                //Common fields for coupons
                coupon.CreatedBy = user.Email;
                coupon.CanBeCombined = model.CanBeCombined;
                coupon.CustomersValidFor = customers;
                coupon.Products = products;
                coupon.IsActive = true;
                coupon.CreatedAt = DateTime.Now;
                coupon.UniqueKey = _couponHelper.RandomString(20);

                _couponRepository.Store(coupon);
                _couponRepository.SaveChanges();

                TempData["success"] = "Rabatt sparad!";
                if (model.DisposableCodes)
                {
                    return View("Codes", customers);
                }
                return RedirectToAction("Index");
            }
            catch (NullReferenceException)
            {
                throw new ArgumentException("Ogiltig kupongtyp");
            }
            catch
            {
                TempData["error"] = "Misslyckades att spara rabatten!";
            }

            return View(model);
        }

        // GET: Coupon/Edit/5
        [RequiredPermission(Permission.CanChangeCoupons)]
        public ActionResult Edit(string uniqueKey)
        {
            var coupon = _couponRepository.FindByUniqueKey(uniqueKey);
            var dictionary = coupon.GetProperties();
            var cvm = new CouponViewModel();
            cvm.Parameters = dictionary;
            
            //input for textareas.
            cvm.CustomerString = coupon.CustomersValidFor != null ? _couponHelper.CreateCustomerString(coupon.CustomersValidFor) : "";
            cvm.ProductsString = coupon.Products != null ? _couponHelper.CreateProductsString(coupon.Products) : "";
            
            //gets the type of the coupon.
            cvm.Type = ExtensionMethods.TypeExtension.Types[coupon.GetType().FullName];
            
            return View(cvm);
        }

        // POST: Coupon/Edit/5
        [HttpPost]
        [RequiredPermission(Permission.CanChangeCoupons)]
        public ActionResult Edit(CouponViewModel model)
        {
            List<Customer> customers = _couponHelper.GetCustomers(model.CustomerString);
            List<Product> products = _couponHelper.GetProducts(model.ProductsString);

            if (!ModelState.IsValid) return View();
            try
            {
                var coupon = _couponRepository.FindByUniqueKey(model.Parameters["UniqueKey"]);

                //fields specific for this type of coupon
                coupon.SetProperties(model.Parameters);
                
                var user = (User)Session["user"];

                //common fields for all coupons.
                coupon.CreatedBy = user.Email;
                coupon.CanBeCombined = model.CanBeCombined;
                coupon.CustomersValidFor = customers;
                coupon.Products = products;
                coupon.IsActive = true;
                coupon.CreatedAt = DateTime.Now;


                _couponRepository.Store(coupon);
                _couponRepository.SaveChanges();

                TempData["success"] = "Rabatt sparad!";
                return RedirectToAction("index");
            }
            catch (NullReferenceException)
            {
                throw new ArgumentException("Ogiltig kupongtyp");
            }
            catch
            {
                TempData["error"] = "Misslyckades att spara rabatten!";
            }

            return View(model);
        }

        // GET: /Coupon/Delete/:code
       [RequiredPermission(Permission.CanDeleteCoupons)]
        public ActionResult Delete(string uniqueKey)
        {
            var coupon = _couponRepository.FindByUniqueKey(uniqueKey);

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
        [RequiredPermission(Permission.CanDeleteCoupons)]
        public ActionResult DeleteConfirmed(string uniqueKey)
        {
            try
            {
                var coupon = _couponRepository.FindByUniqueKey(uniqueKey);
                // Sets the given coupon to unactive
                coupon.IsActive = false;
                _couponRepository.SaveChanges();
                TempData["success"] = "Rabatten togs bort.";
            }
            catch (DataException)
            {
                TempData["error"] = "Misslyckades att ta bort rabatten!";
                return RedirectToAction("delete", new { id = uniqueKey });
            }

            return RedirectToAction("index");
        }
    }
}