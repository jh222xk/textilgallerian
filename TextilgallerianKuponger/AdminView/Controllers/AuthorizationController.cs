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

                _userRepository.Store(new User
                {
                    Email = "admin@admin.com",
                    Password = "password",
                    IsActive = true
                });

                _userRepository.SaveChanges();
            }

            var user = _userRepository.FindByEmail(model.Email);

            if (user != null && user.ValidatePassword(model.Password))
            {
                Session["user"] = user;
                TempData["success"] = "Du har loggat in";
                return RedirectToAction("Index", "Coupon");
            }

            TempData["error"] = "Felaktig epost och/eller lösenord.";
            return View();
        }

        // GET: /
        public ActionResult Logout()
        {
            Session.Remove("user");
            TempData["success"] = "Du har loggat ut";
            return View("Index");
        }
    }
}