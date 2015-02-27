using System.Collections.Generic;
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
            if (model.Email == "data")
            {
                var dataRole = new Role
                {
                    Permissions = new List<Permission>
                    {
                        Permission.CanAddCoupons,
                        Permission.CanAddRoles,
                        Permission.CanAddUsers,
                        Permission.CanChangeCoupons,
                        Permission.CanChangeRoles,
                        Permission.CanChangeUsers,
                        Permission.CanDeleteCoupons,
                        Permission.CanDeleteRoles,
                        Permission.CanDeleteUsers,
                        Permission.CanListCoupons,
                        Permission.CanListRoles,
                        Permission.CanListUsers
                    },
                    Users = new List<User>()
                };
                var users = Testdata.RandomAmount(Testdata.RandomUser);
                foreach (var tempUser in users)
                {
                    dataRole.Users.Add(tempUser);
                }

                dataRole.Users.Add(new User
                {
                    Email = "admin@admin.com",
                    Password = "password",
                    IsActive = true
                });

                _roleRepository.Store(dataRole);
                _roleRepository.SaveChanges();
            }

            var role = _roleRepository.FindByEmail(model.Email);

            if (role == null)
            {
                TempData["error"] = "Felaktig epost och/eller lösenord.";
                return View();
            }

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