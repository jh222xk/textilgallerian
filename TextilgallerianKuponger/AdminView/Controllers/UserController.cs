using System;
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

        private const int PageSize = 15;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: User
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
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Role()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/SetStatus/5
        public ActionResult SetStatus(string email)
        {
            var user = _userRepository.FindByEmail(email);
            return View(user);
        }

        // POST: User/SetStatus/5
        [HttpPost, ActionName("SetStatus")]
        [ValidateAntiForgeryToken]
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