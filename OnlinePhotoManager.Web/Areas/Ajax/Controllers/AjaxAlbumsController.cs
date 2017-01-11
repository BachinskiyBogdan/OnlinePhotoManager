using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlinePhotoManager.Domain.Abstract;
using OnlinePhotoManager.Domain.Concrete;
using OnlinePhotoManager.Domain.Entities;
using OnlinePhotoManager.Web.Models;

namespace OnlinePhotoManager.Web.Areas.Ajax.Controllers
{
    [Authorize]
    public class AjaxAlbumsController : Controller
    {

        private readonly IAlbumRepository _albumRepository;
        private readonly IPictureRepository _pictureRepository;
        private readonly IUserRepository _userRepository;
        private readonly int _elementsPerPage;

        public AjaxAlbumsController(IAlbumRepository album, IPictureRepository picture, IUserRepository user)
        {
            _albumRepository = album;
            _pictureRepository = picture;
            _userRepository = user;
            _elementsPerPage = 9;
        }

        // GET: Album
        public ActionResult Index(string userName, string searchRequest = "", int page = 1)
        {
            return View();
        }

        public ActionResult GetAlbums(string userName, string searchRequest = "", int page = 1)
        {
            userName = User.Identity.Name;//RouteData.Values["userName"].ToString();
            var user = _userRepository.Users.FirstOrDefault(u => u.UserTag == userName);
            ViewBag.searchRequest = searchRequest;
            ViewBag.userName = userName;

            var albums = _albumRepository.Albums
                .Where(a => a.UserId == user.Id)
                .Where(a => a.Name.Contains(searchRequest))
                .OrderBy(a => a.Id)
                .Skip((page - 1) * _elementsPerPage)
                .Take(_elementsPerPage).ToList();

            return PartialView(albums);
        }


        public ActionResult Album(string albumName, string userName = "", string searchRequest = "", int page = 1)
        {
            ViewBag.albumName = albumName;
            ViewBag.userName = userName;
            ViewBag.searchRequest = searchRequest;
            ViewBag.page = page;
            ViewBag.picturesPerPage = _elementsPerPage;

            return PartialView();
        }

        public ActionResult GetAlbumPictures(string albumName, string userName = "", string searchRequest = "",
            int page = 1, PictureSearchViewModel advSearch = null)
        {
            if (userName == "")
                userName = User.Identity.Name;
            var user = _userRepository.Users.FirstOrDefault(u => u.UserTag == userName);
            var album = _albumRepository.Albums
                .Where(a => a.UserId == user.Id).FirstOrDefault(a => a.Name == albumName);
            var result = album.Pictures
                .Where(p => p.Name.Contains(searchRequest));

            result = AdvanceSearch(advSearch, result.AsQueryable());

            result = result.OrderBy(a => a.Id)
                .Skip((page - 1) * _elementsPerPage)
                .Take(_elementsPerPage).ToList();

            ViewBag.index = (page - 1) * _elementsPerPage;
            ViewBag.albumName = albumName;
            ViewBag.userName = userName;
            ViewBag.searchRequest = searchRequest;
            return PartialView(result);
        }
        private static IEnumerable<Picture> AdvanceSearch(PictureSearchViewModel advSearch, IQueryable<Picture> result)
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


        #region Navigation

        [ChildActionOnly]
        public ActionResult AlbumsNavigation(string userName, string searchRequest = "")
        {
            var user = _userRepository.Users.FirstOrDefault(u => u.UserTag == User.Identity.Name);
            var result = _albumRepository.Albums
                .Where(a => a.UserId == user.Id)
                .Count(a => a.Name.Contains(searchRequest));

            ViewBag.searchRequest = searchRequest;
            ViewBag.userName = userName;
            ViewBag.albumsPerPage = _elementsPerPage;

            return PartialView(result);
        }

        [ChildActionOnly]
        public ActionResult PicturesNavigation(string userName, string albumName, string searchRequest, PictureSearchViewModel advSearch = null)
        {
            var user = _userRepository.Users.FirstOrDefault(u => u.UserTag == userName);
            var album = _albumRepository.Albums
                .Where(a => a.UserId == user.Id).FirstOrDefault(a => a.Name == albumName);

            var result = album.Pictures.Where(p => p.Name.Contains(searchRequest));
            result = AdvanceSearch(advSearch, result.AsQueryable());

            ViewBag.searchRequest = searchRequest;
            ViewBag.userName = userName;
            ViewBag.albumName = albumName;
            ViewBag.picturesPerPage = _elementsPerPage;

            return PartialView(result.Count());
        }

        [ChildActionOnly]
        public ActionResult AddPicturesNavigation(string userName, string albumName, string searchRequest)
        {
            var user = _userRepository.Users.FirstOrDefault(u => u.UserTag == userName);
            var album = _albumRepository.Albums.Where(a => a.UserId == user.Id).FirstOrDefault(a => a.Name == albumName);
            var result = _pictureRepository.Pictures
                .Where(p => p.UserId == user.Id)
                .Count(p => p.Name.Contains(searchRequest));

            ViewBag.searchRequest = searchRequest;
            ViewBag.userName = userName;
            ViewBag.albumName = albumName;
            ViewBag.picturesPerPage = _elementsPerPage;

            return PartialView(result);
        }

        public FileContentResult GetAlbumPicture(string albumName = "")
        {
            if (albumName == "")
                return null;
            var album = _albumRepository.Albums.FirstOrDefault(a => a.Name == albumName);
            if (album == null)
                return null;
            if (album.CoverData == null)
                return null;
            return File(album.CoverData, album.CoverMimeType);
        }

        [ChildActionOnly]
        public ActionResult AdvanceSearch(string albumName) 
        {
            ViewBag.albumName = albumName;
            var model = new PictureSearchViewModel();
            return PartialView(model);
        }

        #endregion Navigation

        #region Edit

        public ActionResult AddNewAlbum()
        {
            var album = new Album();
            return View(album);
        }
        [HttpPost]
        public ActionResult AddNewAlbum(Album album, HttpPostedFileBase image)
        {
            var user = _userRepository.Users.FirstOrDefault(u => u.UserTag == User.Identity.Name);
            if (_albumRepository.Albums.Where(a => a.UserId == user.Id).Any(a => a.Name == album.Name))
            {
                ModelState.AddModelError("", "Album name is already exist.");
            }
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    album.CoverMimeType = image.ContentType;
                    album.CoverData = new byte[image.ContentLength];
                    image.InputStream.Read(album.CoverData, 0, image.ContentLength);
                }

                album.UserId = user.Id;
                _albumRepository.Add(album);
                return RedirectToAction("Index");
            }
            return View(album);
        }

        public ActionResult EditAlbum(string albumName)
        {
            var album = _albumRepository.Albums.FirstOrDefault(a => a.Name == albumName);
            return View(album);
        }
        [HttpPost]
        public ActionResult EditAlbum(Album album, HttpPostedFileBase image)
        {
            var r = _albumRepository.Albums.FirstOrDefault(a => a.Id == album.Id);
            if (r.Name != album.Name && _albumRepository.Albums.Any(a => a.Name == album.Name))
                ModelState.AddModelError("", "Album with such name already exist. Use previous or another.");
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    album.CoverMimeType = image.ContentType;
                    album.CoverData = new byte[image.ContentLength];
                    image.InputStream.Read(album.CoverData, 0, image.ContentLength);
                }
                _albumRepository.Update(album);
                return RedirectToAction("Index");
            }
            return View(album);
        }

        public ActionResult DeleteAlbum(decimal albumId)
        {
            _albumRepository.Remove(albumId);
            return RedirectToAction("Index");
        }

        #endregion Edit 
        
    }
}