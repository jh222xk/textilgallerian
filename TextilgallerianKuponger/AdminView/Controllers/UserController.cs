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
        private const int PageSize = 15;
        private readonly RoleRepository _roleRepository;

        public UserController(RoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        // GET: User
        [RequiredPermission(Permission.CanListUsers)]
        public ActionResult Index(int page = 1)
        {
            var users =
                _roleRepository.FindAllRoles()
                    .ToList()
                    .SelectMany(r => r.Users.Select(u => new AuthorizationViewModel
                    {
                        User = u,
                        Role = r.Name
                    }))
                    .OrderByDescending(u => u.User.CreatedAt)
                    .ToList();
            var model = new PagedViewModel<AuthorizationViewModel>
            {
                PagedObjects = users.Page(page - 1, PageSize),
                CurrentPage = page,
                TotalPages = (int) Math.Ceiling(users.Count()/(double) PageSize)
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
                //Email has to be unique
                if(_roleRepository.FindByEmail(model.Email) != null)
                {
                    TempData["Error"] = "Det finns redan en användare med denna E-mailadress.";
                }

                else if (ModelState.IsValid)
                {
                    var user = new User
                    {
                        Email = model.Email,
                        IsActive = true,
                        CreatedAt = DateTime.Now,
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
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }

            model.Roles = _roleRepository.FindAllRoles();
            return View(model);
        }

        // GET: User/Edit/5
        [RequiredPermission(Permission.CanChangeUsers)]
        public ActionResult Edit(String email)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        [RequiredPermission(Permission.CanChangeUsers)]
        public ActionResult Edit(User model)
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

        // POST: User/SetStatus/5
        [ValidateAntiForgeryToken]
        [RequiredPermission(Permission.CanDeleteUsers)]
        public ActionResult SetStatus(String email)
        {
            if (email == ((User) Session["user"]).Email)
            {
                TempData["error"] = "Du kan inte inaktivera dig själv.";
                return RedirectToAction("index");
            }
            try
            {
                var user = _roleRepository.FindByEmail(email).Users.FirstOrDefault(e => e.Email == email);
                user.IsActive = !user.IsActive;
                _roleRepository.SaveChanges();

                TempData["success"] = "Status ändrad.";
            }
            catch
            {
                TempData["error"] = "Status ändringen misslyckades.";
            }
            return RedirectToAction("index");
        }
    }
}