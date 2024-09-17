using BTL_LTWebNC.Data;
using BTL_LTWebNC.Models.EF;
using BTL_LTWebNC.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace BTL_LTWebNC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public AccountController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterViewModel user)
        {
            var userNameExist = _dbContext.DbUser.SingleOrDefault(x => x.UserName == user.UserName);

            if (userNameExist == null)
            {
                var newUser = new User()
                {
                    //add data to newUser
                    UserName = user.UserName.Trim(),
                    UserPassword = user.Password.Trim(),
                    RePassWord = user.RePassword,
                    Email = user.Email,
                    FullName = user.FullName,
                    UserRoleID = 2,
                    VerifyKey = user.VerifyKey,
                };

                _dbContext.DbUser.Add(newUser);
                _dbContext.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("UserName","Tên đăng nhập này đã được sử dụng");
                return View(user);
            }
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginViewModel model)
        {
            var user = _dbContext.DbUser.Where(x => x.UserName == model.UserName && x.UserPassword == model.UserPassword).SingleOrDefault();

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.UserID);
                HttpContext.Session.SetString("UserName", user.UserName.ToString());
                HttpContext.Session.SetInt32("Role", user.UserRoleID);
                HttpContext.Session.SetString("Email", user.Email);
                HttpContext.Session.SetString("FullName", user.FullName);

                if (user.UserRoleID == 1)
                {
                    return RedirectToAction("IndexAdmin", "Account");
                }
                else
                {
                    return RedirectToAction("IndexPost", "Post");
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Tài khoản hoặc mật khẩu sai";
            }

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("Role");
            return RedirectToAction("Index", "Home");
        }

        //Admin
        public IActionResult IndexAdmin()
        {
            var listUser = _dbContext.DbUser
                .Include(a => a.Role)
                .ToList();
            return View(listUser);
        }

        public IActionResult ActionThi()
        {
            var listUser = _dbContext.DbUser
                .Include(a => a.Role)
                .ToList();
            return View(listUser);
        }

        [HttpGet]
        public IActionResult AddAccount()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddAccount(RegisterViewModel account)
        {
            if(account == null)
            {
                return View("Error");
            }
            else
            {
                var user = new User()
                {
                    UserName = account.UserName,
                    UserPassword = account.Password,
                    Email = account.Email,
                    FullName = account.FullName,
                    UserRoleID = account.RoleID,
                };

                _dbContext.DbUser.Add(user);
                _dbContext.SaveChanges();

                return RedirectToAction("IndexAdmin");
            }
        }

        [HttpGet]
        public IActionResult EditAccount(int id)
        {
            var editUser = _dbContext.DbUser.Find(id);
            if (editUser == null)
            {
                return NotFound();
            }

            return View(editUser);
        }

        [HttpPost]
        public IActionResult EditAccount(User user)
        {
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                var editUser = _dbContext.DbUser.Find(user.UserID);
                if (editUser == null)
                {
                    return NotFound();
                }

                editUser.UserName = user.UserName;
                editUser.UserPassword = user.UserPassword;
                editUser.Email = user.Email;
                editUser.FullName = user.FullName;
                editUser.UserRoleID = user.UserRoleID;

                _dbContext.SaveChanges();

                return RedirectToAction("IndexAdmin");
            }
        }

        [HttpGet]
        public IActionResult DeleteAccount(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _dbContext.DbUser.FirstOrDefault(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult DeleteAccount(int id)
        {
            var user = _dbContext.DbUser.Find(id);
            if (user != null)
            {
                _dbContext.DbUser.Remove(user);
                _dbContext.SaveChanges();
            }

            return RedirectToAction("IndexAdmin");
        }

        //////////////
    }
}
