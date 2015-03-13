using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdminView.Annotations;
using AdminView.ViewModel;
using Domain.Entities;
using Domain.ExtensionMethods;
using Domain.Repositories;
using AdminView.Controllers.Helpers;

namespace AdminView.Controllers
{
    [LoggedIn]
    public class UserController : Controller
    {
        private const int PageSize = 15;
        private readonly RoleRepository _roleRepository;
        private readonly CouponHelper _couponHelper;

        public UserController(RoleRepository roleRepository, CouponHelper couponHelper)
        {
            _roleRepository = roleRepository;
            _couponHelper = couponHelper;
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
        [ValidateInput(false)]
        [RequiredPermission(Permission.CanAddUsers)]
        public ActionResult Create(AuthorizationViewModel model)
        {
            try
            {
                // Email has to be unique
                if(_roleRepository.FindByEmail(model.Email) != null)
                {
                    TempData["error"] = "Det finns redan en användare med detta användarnamn";
                }

                //must choose a role.
                else if(model.Role == null)
                {
                    TempData["error"] = "Du måste välja en roll.";
                }

                else if (ModelState.IsValid)
                {
                    var user = new User
                    {
                        Id = _couponHelper.RandomString(20),
                        Email = model.Email,
                        IsActive = true,
                        CreatedAt = DateTime.Now,
                        Password = model.Password
                    };
                    var role = _roleRepository.FindByName(model.Role);

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
            catch (Exception)
            {
                TempData["error"] = "Det gick inte att skapa användaren";
            }

            model.Roles = _roleRepository.FindAllRoles();
            return View(model);
        }

        // GET: User/Edit/5
        [ValidateInput(false)]
        [RequiredPermission(Permission.CanChangeUsers)]
        public ActionResult Edit(String email)
        {
            var role = _roleRepository.FindByEmail(email);

            var user = role.Users.FirstOrDefault(u => u.Email == email);
            
            return View(new AuthorizationViewModel
            {
                Id = user.Id,
                Email = user.Email,
                CurrentRole = role,
                Roles = _roleRepository.FindAllRoles()
            });
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        [RequiredPermission(Permission.CanChangeUsers)]
        public ActionResult Edit(AuthorizationViewModel model)
        {
            try
            {
                var currentRole = _roleRepository.FindByName(model.CurrentRole.Name);
                var user = currentRole.Users.FirstOrDefault(u => u.Id == model.Id);

                if (user == null)
                {
                    TempData["error"] = "Användaren finns ej.";
                    return RedirectToAction("Index");
                }




                if (user.Email != model.Email)
                {
                    if (_roleRepository.FindByEmail(model.Email) != null)
                    {
                        TempData["error"] = "Det finns redan en användare med detta användarnamn";
                        model.Email = user.Email;
                        model.Roles = _roleRepository.FindAllRoles();
                        return View(model);
                    }
                    user.Email = model.Email;
                }

                //must choose a role.
                if (model.Role == null)
                {
                    TempData["error"] = "Du måste välja en roll.";
                    model.Email = user.Email;
                    model.Roles = _roleRepository.FindAllRoles();
                    return View(model);
                }

                if (user.Id != ((User) Session["user"]).Id)
                {
                    var role = _roleRepository.FindByName(model.Role);
                    currentRole.Users.Remove(user);
                    role.Users.Add(user);
                    _roleRepository.Store(role);
                    TempData["success"] = "Användare sparad.";
                }
                else
                {
                    TempData["success"] = "Användare sparad men roll kvarstår eftersom att du inte kan ändra din egen roll.";
                }

                if (!String.IsNullOrWhiteSpace(model.Password))
                {
                    user.Password = model.Password;
                }

                _roleRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }

            model.Roles = _roleRepository.FindAllRoles();
            return View(model);
        }

        // POST Delete: User/SetStatus/5
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
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