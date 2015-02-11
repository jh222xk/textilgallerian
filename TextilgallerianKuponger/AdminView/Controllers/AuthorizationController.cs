using System;
using System.IO;
using System.Web.Mvc;
using AdminView.ViewModel;
using Domain.Entities;
using Domain.Repositories;
using Domain.Tests.Helpers;

namespace AdminView.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly UserRepository _userRepository;

        public AuthorizationController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        // GET: /
        public ActionResult Index()
        {
            return View();
        }

        // POST: /
        [HttpPost]
        public ActionResult Index(AuthorizationViewModel model)
        {
            if (model.Email == "data")
            {
                var users = Testdata.RandomAmount(Testdata.RandomUser);
                foreach (var tempUser in users)
                {
                    _userRepository.Store(tempUser);   
                }
                _userRepository.SaveChanges();
            }

            var user = _userRepository.FindByEmail(model.Email);

            if (user != null && user.ValidatePassword(model.Password))
            {
                Session["user"] = user;

                return RedirectToAction("Index", "Coupon");
            }

            ViewBag.errorMessage = "Wrong email or password";
            return View();
        }
    }
}