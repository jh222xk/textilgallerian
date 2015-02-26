using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdminView.Annotations;
using AdminView.ViewModel;
using Domain.Entities;
using Domain.ExtensionMethods;
using Domain.Repositories;

namespace AdminView.Controllers
{
   [LoggedIn]
    public class UserController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly RoleRepository _roleRepository;

        private const int PageSize = 15;

        public UserController(UserRepository userRepository, RoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        // GET: User
        [RequiredPermission(Permission.CanListUsers)]
        public ActionResult Index(int page = 1)
        {
            var model = new PagedViewModel<User>
            {
                PagedObjects = _userRepository.FindAllUsers().Page(page - 1, PageSize),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(_userRepository.FindAllUsers().Count() / (double) PageSize)
            };

            return View(model);
        }

        // GET: User/Details/5
        [RequiredPermission(Permission.CanListUsers)]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        [RequiredPermission(Permission.CanAddUsers)]
        public ActionResult Create()
        {

            return View(new AuthorizationViewModel
            {
                Roles = _roleRepository.FindAllRoles()
            });
        }


        // POST: User/Create
        [HttpPost]
        [RequiredPermission(Permission.CanAddUsers)]
        public ActionResult Create(AuthorizationViewModel model)
        {
            try
            {
                var user = new User
                {
                    Email = model.Email,
                    IsActive = true,
                    Password = model.Password
                };
                var role = _roleRepository.FindByName(model.Role);
                //_userRepository.Store(user);
                //_userRepository.SaveChanges();
                if (role.Users == null)
                {
                    role.Users = new List<User>();
                }
                role.Users.Add(user);
                _roleRepository.Store(role);
                _roleRepository.SaveChanges();

                TempData["success"] = "Användare sparad!";
                return RedirectToAction("index");
            }
            catch
            {
                return View(model);
            }
        }

        // GET: User/Edit/5
        [RequiredPermission(Permission.CanChangeUsers)]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        [RequiredPermission(Permission.CanChangeUsers)]
        public ActionResult Edit(int id, FormCollection collection)
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

        // GET: User/SetStatus/5
       [RequiredPermission(Permission.CanDeleteUsers)]
        public ActionResult SetStatus(string email)
        {
            var user = _userRepository.FindByEmail(email);
            return View(user);
        }

        // POST: User/SetStatus/5
        [HttpPost, ActionName("SetStatus")]
        [ValidateAntiForgeryToken]
        [RequiredPermission(Permission.CanDeleteUsers)]
        public ActionResult SetStatusConfirmed(string email)
        {
            try
            {
                var user = _userRepository.FindByEmail(email);
                user.IsActive = !user.IsActive;
                _userRepository.SaveChanges();

                TempData["success"] = "Status ändrad.";
                return RedirectToAction("index");
            }
            catch
            {
                TempData["error"] = "Status ändringen misslyckades.";
                return View();
            }
        }
    }
}