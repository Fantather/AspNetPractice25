using AspNetPractice25.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AspNetPractice25.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUser _users;

        public HomeController(IUser users)
        {
            _users = users;
        }

        public IActionResult Index()
        {
            return View(_users.GetAllUsers());
        }

        public IActionResult GetUser(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }
            User user = _users.GetUser(id.Value);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            if (ModelState.IsValid)
            {
                _users.AddUser(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult UpdateUser(User user)
        {
            if (ModelState.IsValid)
            {
                if (user.Id <= 0)
                {
                    return BadRequest();
                }
                _users.UpdateUser(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult DeleteUser(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }
            User user = _users.GetUser(id.Value);
            if (user == null)
            {
                return NotFound();
            }
            _users.DeleteUser(user);
            return View(user);
        }
    }
}
