using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OnlinePhotoManager.Domain.Abstract;
using OnlinePhotoManager.Domain.Entities;
using OnlinePhotoManager.Web.Models;
using WebMatrix.WebData;

namespace OnlinePhotoManager.Web.Areas.Ajax.Controllers
{
    public class AjaxHomeController : Controller
    {
        private readonly IPictureRepository _pictureRepository;
        private readonly IUserRepository _userRepository;

        private readonly int _elementsPerPage;
        public AjaxHomeController(IUserRepository user, IPictureRepository picture)
        {
            _userRepository = user;
            _pictureRepository = picture;
            _elementsPerPage = 9;
        }


        public ActionResult Index()
        {
            if (this.Request.IsAuthenticated)
            {
                return RedirectToAction("UserHomePage");
            }
            return RedirectToAction("GuestHomePage");
        }


        public ActionResult UserHomePage(string searchRequest = "", int page = 1)
        {
            var user = _userRepository.Users.FirstOrDefault(u => string.Compare(u.UserTag, User.Identity.Name, StringComparison.Ordinal) == 0);
            if (user == null)
                return RedirectToAction("SignIn", "AjaxAccount");
            return RedirectToRoute("Ajax_MainScheme", new {action="Index", controller="AjaxAlbums", userName = user.UserTag });
        }
        public ActionResult GuestHomePage(string searchRequest = "", int page = 1, PictureSearchViewModel advSearch = null)
        {
            ViewBag.SearchRequest = searchRequest;
            var request = _pictureRepository.Pictures
                    .Where(a => a.IsPrivate == false)
                    .Where(a => a.Name.Contains(searchRequest));
            request = AdvanceSearch(advSearch, request);
            request.OrderBy(p => p.Id)
                    .Skip(_elementsPerPage * (page - 1))
                    .Take(_elementsPerPage);

            return View(request.ToList());
        }

        private static IQueryable<Picture> AdvanceSearch(PictureSearchViewModel advSearch, IQueryable<Picture> result)
        {
            if (advSearch.Name != null)
            {
                result = result.Where(p => p.Name.Contains(advSearch.Name));
            }
            if (advSearch.Date != null)
            {
                result = result.Where(p => p.Date == advSearch.Date);
            }
            if (advSearch.IsFlash != null)
            {
                result = result.Where(p => p.IsFlash == advSearch.IsFlash);
            }
            if (advSearch.Place != null)
            {
                result = result.Where(p => p.Place == advSearch.Place);
            }
            if (advSearch.Model != null)
            {
                result = result.Where(p => p.Model.Contains(advSearch.Model));
            }
            if (advSearch.ShutterSpeed != null)
            {
                result = result.Where(p => p.ShutterSpeed == advSearch.ShutterSpeed);
            }
            if (advSearch.FocalLength != null)
            {
                result = result.Where(p => p.FocalLength == advSearch.FocalLength);
            }
            if (advSearch.Diaphragm != null)
            {
                result = result.Where(p => p.Diaphragm.Contains(advSearch.Diaphragm));
            }
            if (advSearch.ISO != null)
            {
                result = result.Where(p => p.ISO == advSearch.ISO);
            }
            return result;
        }

        [ChildActionOnly]
        public ActionResult Navigation(string searchRequest = "")
        {
            ViewBag.SearchRequest = searchRequest;
            ViewBag.ElementsPerPage = _elementsPerPage;
            var request = _pictureRepository.Pictures.Where(p => p.IsPrivate == false).Count(p => p.Name.Contains(searchRequest));
            return PartialView(request);
        }

        [ChildActionOnly]
        public ActionResult AdvanceSearch(string albumName)
        {
            ViewBag.albumName = albumName;
            var model = new PictureSearchViewModel();
            return PartialView(model);
        }

        public FileContentResult GetPicture(decimal id = -1)
        {
            if (id == -1)
                return null;
            var picture = _pictureRepository.Get(id);
            if (picture.IconData == null)
                return null;
            return File(picture.IconData, picture.ImageMimeType);
        }

    }
}
