using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OnlinePhotoManager.Domain.Abstract;
using OnlinePhotoManager.Domain.Entities;
using OnlinePhotoManager.Web.Models;
using WebMatrix.WebData;
    
namespace OnlinePhotoManager.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            var user = new LoginViewModel();
            return View(user);
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel request)
        {
            var user = _userRepository.Users.FirstOrDefault(u => u.UserTag == request.Name);
            if (user == null)
                ModelState.AddModelError("", "User name doesn't exist.");
            if (user.Password != request.Password)
                ModelState.AddModelError("", "Wrong Password.");

            if (ModelState.IsValid)
            {
                if(WebSecurity.Login(request.Name, request.Password))
                    return RedirectToRoute("MainScheme", new { action = "Index", controller = "Albums", userName = user.UserTag});
            }
            return View(request);
        }

        public ActionResult SignOut()
        {
            WebSecurity.Logout();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult CreateNewAccount()
        {
            var user = new UserViewModel();
            user.DateOfBirth = new DateTime(1990, 01, 01);
            return View(user);
        }
        [HttpPost]
        public ActionResult CreateNewAccount(UserViewModel user, 
            HttpPostedFileBase image)
        {
            if (_userRepository.Users.Any(u => u.UserTag == user.UserTag))
                ModelState.AddModelError("", "Such user name is already used.");
            if (_userRepository.Users.Any(u => u.Email == user.Email))
                ModelState.AddModelError("", "Such email is already used.");
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    user.IconMimeType = image.ContentType;
                    user.IconData = new byte[image.ContentLength];
                    image.InputStream.Read(user.IconData, 0, image.ContentLength);
                
                }
                UserProfile dbUser = InitializeUser(user);
                var r = _userRepository.Add(dbUser);
                WebSecurity.CreateAccount(user.UserTag, user.Password, false);
                if (r == null)
                    ; // TODO or not TODO
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }
        private UserProfile InitializeUser(UserViewModel user)
        {
            UserProfile dbUser = new FreeUser();
            if (user.IsPremium)
                dbUser = new PremiumUser();

            dbUser.IsPremium = user.IsPremium;
            dbUser.UserTag = user.UserTag;
            dbUser.Password = user.Password;
            dbUser.Email = user.Email;
            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;
            dbUser.DateOfBirth = user.DateOfBirth;
            dbUser.IconData = user.IconData;
            dbUser.IconMimeType = user.IconMimeType;

            var u = dbUser as FreeUser;
            if (u != null)
            {
                u.MaxAlbums = 5;
                u.MaxPictures = 30;
            }
            return dbUser;
        }

        // Layout header methods
        public ActionResult AccountInfo()
        {
            UserProfile curUser = null;
            if (User.Identity.IsAuthenticated)
            {
                curUser = _userRepository.Users
                    .FirstOrDefault(u => string.Compare(u.UserTag, User.Identity.Name, StringComparison.Ordinal) == 0);
            }
            return PartialView(curUser);
        }

        public ActionResult GetIcon(decimal userId = -1)
        {
            if (userId == -1)
                return File("~/Content/defaultUserIcon.png", "img/png");

            var user = _userRepository.Get(userId);
            if (user.IconData == null)
                return File("~/Content/defaultUserIcon.png", "img/png");
            return File(user.IconData, user.IconMimeType);
        }
    }

}
