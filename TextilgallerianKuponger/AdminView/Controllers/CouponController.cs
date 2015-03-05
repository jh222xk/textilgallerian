﻿using System;
using System.Collections.Generic;
﻿using System.Data;
using System.Linq;
﻿using System.Web.Mvc;
﻿using AdminView.Annotations;
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
        [ValidateInput(false)]
        [RequiredPermission(Permission.CanAddCoupons)]
        public ActionResult Create(CouponViewModel model)
        {
            List<Customer> customers = null;

            
            // Search DB for coupon code 
            var couponCode = _couponRepository.FindByCode(model.Parameters["Code"]);

            //Validates if code already exists 
            if (couponCode != null)
            {
                TempData["error"] = "Finns redan en rabattkod med den koden";
                return View(model);
            }
            
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

            var products = _couponHelper.GetProducts(model.ProductsString);
            var brands = _couponHelper.GetBrands(model.BrandsString);
            var categories = _couponHelper.GetCategories(model.CategoriesString);

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
                coupon.Brands = brands;
                coupon.Categories = categories;
                coupon.IsActive = true;
                coupon.CreatedAt = DateTime.Now;
                coupon.UniqueKey = _couponHelper.RandomString(20);

                if (coupon is TotalSumPercentageDiscount)
                {
                    var c = coupon as TotalSumPercentageDiscount;
                    c.DiscountOnlyOnSpecifiedProducts =
                        model.PercentageDiscountOnlyOnSpecifiedProducts;
                }

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
        public ActionResult Edit(String uniqueKey)
        {
            var coupon = _couponRepository.FindByUniqueKey(uniqueKey);
            var cvm = new CouponViewModel
            {
                Parameters = coupon.GetProperties()
            };
            
            //input for textareas.
            cvm.CustomerString = coupon.CustomersValidFor != null ? _couponHelper.CreateCustomerString(coupon.CustomersValidFor) : "";
            cvm.ProductsString = coupon.Products != null ? _couponHelper.CreateProductsString(coupon.Products) : "";
            cvm.BrandsString = coupon.Brands != null ? _couponHelper.CreateBrandString(coupon.Brands) : "";
            cvm.CategoriesString = coupon.Categories != null ? _couponHelper.CreateCategoryString(coupon.Categories) : "";
            
            //gets the type of the coupon.
            cvm.Type = ExtensionMethods.TypeExtension.Types[coupon.GetType().FullName];

            if (coupon is TotalSumPercentageDiscount)
            {
                var c = coupon as TotalSumPercentageDiscount;
                cvm.PercentageDiscountOnlyOnSpecifiedProducts =
                    c.DiscountOnlyOnSpecifiedProducts;
            }
            
            return View(cvm);
        }

        // POST: Coupon/Edit/5
        [HttpPost]
        [RequiredPermission(Permission.CanChangeCoupons)]
        public ActionResult Edit(CouponViewModel model)
        {
            var customers = _couponHelper.GetCustomers(model.CustomerString);
            var products = _couponHelper.GetProducts(model.ProductsString);
            var brands = _couponHelper.GetBrands(model.BrandsString);
            var categories = _couponHelper.GetCategories(model.CategoriesString);


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
                coupon.Brands = brands;
                coupon.Categories = categories;
                coupon.CreatedAt = DateTime.Now;

                if (coupon is TotalSumPercentageDiscount)
                {
                    var c = coupon as TotalSumPercentageDiscount;
                    c.DiscountOnlyOnSpecifiedProducts =
                        model.PercentageDiscountOnlyOnSpecifiedProducts;
                }

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
        public ActionResult Delete(String uniqueKey)
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