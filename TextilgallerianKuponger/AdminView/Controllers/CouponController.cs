using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminView.Annotations;
using AdminView.ViewModel;
using Domain.Entities;
using Domain.Repositories;
using Domain.Tests.Helpers;

namespace AdminView.Controllers
{
    [LoggedIn]
    public class CouponController : Controller
    {

        private readonly CouponRepository _couponRepository;

        public CouponController(CouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }


        // GET: Coupon
        public ActionResult Index()
        {
            var coupons = _couponRepository.FindAllCoupons().ToList();

            var tempCoupons = Testdata.RandomCoupon();

            _couponRepository.Store(tempCoupons);

            _couponRepository.SaveChanges();

            return View("Coupons", coupons);
        }

        // GET: Coupon/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Coupon/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Coupon/Create
        [HttpPost]
        public ActionResult Create(CouponViewModel model)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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
        /// TODO: NEEDS TYPE?
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string code)
        {
            try
            {
                var couponToDelete = new BuyProductXRecieveProductY {Code = code};
                _couponRepository.Delete(couponToDelete);
                _couponRepository.SaveChanges();
                TempData["success"] = "Rabatten togs bort.";
            }
            catch (DataException)
            {
                TempData["error"] = "Misslyckades att ta bort rabatten!";
                return RedirectToAction("Delete", new {id = code});
            }

            return RedirectToAction("Index");
        }
    }
}
