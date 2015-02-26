using System.Linq;
using System.Web.Mvc;
using AdminView.ViewModel;
using Domain.Entities;
using Domain.Repositories;
using Domain.Tests.Helpers;

namespace AdminView.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly RoleRepository _roleRepository;

        public AuthorizationController(RoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
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
            //if (model.Email == "data")
            //{
            //    var users = Testdata.RandomAmount(Testdata.RandomUser);
            //    foreach (var tempUser in users)
            //    {
            //        _userRepository.Store(tempUser);
            //    }

            //    _userRepository.Store(new User
            //    {
            //        Email = "admin@admin.com",
            //        Password = "password",
            //        IsActive = true
            //    });

            //    _userRepository.SaveChanges();
            //}

            var role = _roleRepository.FindByEmail(model.Email);
            var user = role.Users.FirstOrDefault(u => u.Email == model.Email);

            if (user != null && user.ValidatePassword(model.Password))
            {
                Session["user"] = user;
                Session["role"] = role;
                TempData["success"] = "Du har loggat in";
                return RedirectToAction("index", "coupon");
            }

            TempData["error"] = "Felaktig epost och/eller lösenord.";
            return View();
        }

        // GET: /
        public ActionResult Logout()
        {
            Session.Remove("user");
            TempData["success"] = "Du har loggat ut";
            return RedirectToAction("index");
        }
    }
}