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
   // [LoggedIn]
    public class RoleController : Controller
    {
        private readonly RoleRepository _roleRepository;

        private const int PageSize = 15;

        public RoleController(RoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        // GET: Role
        [RequiredPermission(Permission.CanListRoles)]
        public ActionResult Index(int page = 1)
        {
            var model = new PagedViewModel<Role>
            {
                PagedObjects = _roleRepository.FindAllRoles().Page(page - 1, PageSize),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(_roleRepository.FindAllRoles().Count() / (double) PageSize)
            };

            return View(model);
        }

        // GET: Role/Details/5
        [RequiredPermission(Permission.CanListRoles)]
        public ActionResult Details(int id)
        {
            return View();
        }

        [RequiredPermission(Permission.CanAddRoles)]
        public ActionResult Create()
        {
            return View(new Role());
        }

        [HttpPost]
        [RequiredPermission(Permission.CanAddRoles)]
        public ActionResult Create(Role role)
        {
            if (String.IsNullOrWhiteSpace(role.Name))
            {
                TempData["error"] = "Du måste ange ett namn";
                return View(new Role());
            }
            if (role.Permissions == null || !role.Permissions.Any())
            {
                TempData["error"] = "Du måste ange minst en behörighet";
                return View(new Role());
            }
            try
            {
                role.Users = new List<User>();
                _roleRepository.Store(role);
                _roleRepository.SaveChanges();

                TempData["success"] = "Roll sparad!";
                return RedirectToAction("index");
            }
            catch
            {
                return View(new Role());
            }
        }
    }
}