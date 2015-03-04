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
    public class RoleController : Controller
    {
        private const int PageSize = 15;
        private readonly RoleRepository _roleRepository;

        public RoleController(RoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        // GET: Role
        [RequiredPermission(Permission.CanListRoles)]
        [ValidateInput(false)]
        public ActionResult Index(int page = 1)
        {
            var model = new PagedViewModel<Role>
            {
                PagedObjects = _roleRepository.FindAllRoles().Page(page - 1, PageSize),
                CurrentPage = page,
                TotalPages =
                    (int) Math.Ceiling(_roleRepository.FindAllRoles().Count()/(double) PageSize)
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
        [ValidateInput(false)]
        [RequiredPermission(Permission.CanAddRoles)]
        public ActionResult Create(Role role)
        {
            if (String.IsNullOrWhiteSpace(role.Name))
            {
                TempData["error"] = "Du måste ange ett namn";
                return View(role);
            }
            if (role.Permissions == null || !role.Permissions.Any())
            {
                TempData["error"] = "Du måste ange minst en behörighet";
                return View(role);
            }
            if(_roleRepository.FindByName(role.Name) != null)
            {
                TempData["error"] = "En roll med detta namn finns redan";
                return View(role);
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
                return View(role);
            }
        }

        [RequiredPermission(Permission.CanChangeRoles)]
        public ActionResult Edit(String name)
        {
            var role = _roleRepository.FindByName(name);
            return View(role);
        }

        [HttpPost]
        [RequiredPermission(Permission.CanChangeRoles)]
        public ActionResult Edit(Role model)
        {
            if (model.Permissions == null || !model.Permissions.Any())
            {
                TempData["error"] = "Du måste ange minst en behörighet";
                return View(model);
            }
            if (((Role) Session["role"]).Name == model.Name &&
                !model.Permissions.Contains(Permission.CanChangeRoles))
            {
                TempData["error"] = "Du kan inte ta bort dina egna rättigheter att ändra roller";
                return View(model);
            }

            try
            {
                var role = _roleRepository.FindByName(model.Name);
                role.Permissions = model.Permissions;

                _roleRepository.Store(role);
                _roleRepository.SaveChanges();

                TempData["success"] = "Roll sparad!";
                return RedirectToAction("index");
            }
            catch
            {
                return View(model);
            }
        }
    }
}
