using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using OnlinePhotoManager.Domain.Abstract;
using OnlinePhotoManager.Domain.Entities;

namespace OnlinePhotoManager.Web.Areas.Ajax.Controllers
{
    public class AjaxPicturesController : Controller
    {
        #region Parameters

        private readonly IUserRepository _userRepository;
        private readonly IAlbumRepository _albumRepository;
        private readonly IPictureRepository _pictureRepository;

        public AjaxPicturesController(IAlbumRepository albums, IPictureRepository pictures, IUserRepository user)
        {
            _albumRepository = albums;
            _pictureRepository = pictures;
            _userRepository = user;
        }

        #endregion Parameters

        [Authorize]
        public ActionResult Index(string albumName, int index, string searchRequest = "")
        {
            ViewBag.index = index;
            ViewBag.albumName = albumName;
            ViewBag.searchRequest = searchRequest;
            
            return PartialView();
        }
        [Authorize]
        public ActionResult GetPictureData(string albumName, int index, string searchRequest = "")
        {
            var album = _albumRepository.Albums.FirstOrDefault(a => a.Name == albumName);
            if (album == null)
                return View("Error", "_Layout", "Invalid album name.");
            var pictures = album.Pictures.Where(p => p.Name.Contains(searchRequest));

            var picture = pictures.Skip(index - 1).Take(3).ToList();
            ViewBag.Position = 0;
            var cnt = pictures.Count();
            if (index >= cnt - 1)
            {
                ViewBag.Position = 1;
                picture = pictures.Skip(cnt - 2).Take(2).ToList();
                index = cnt - 1;
            }
            if (index <= 0)
            {
                ViewBag.Position = -1;
                index = 0;
                picture = pictures.Take(2).ToList();
            }


            ViewBag.index = index;
            ViewBag.albumName = albumName;
            ViewBag.searchRequest = searchRequest;
            return PartialView(picture);
        }

        public FileContentResult GetPicture(decimal pictureId = -1)
        {
            if (pictureId == -1)
                return null;
            var picture = _pictureRepository.Pictures.FirstOrDefault(p => p.Id == pictureId);
            if (picture == null)
                return null;
            if (picture.ImageData == null)
                return null;
            return File(picture.ImageData, picture.ImageMimeType);
        }
        public FileContentResult GetPictureIcon(decimal pictureId = -1)
        {
            if (pictureId == -1)
                return null;
            var picture = _pictureRepository.Pictures.FirstOrDefault(p => p.Id == pictureId);
            if (picture == null)
                return null;
            if (picture.IconData == null)
                return null;
            return File(picture.IconData, picture.ImageMimeType);
        }

        #region EDIT

        [Authorize]
        public ActionResult AddNewPicture()
        {
            var picture = new Picture();
            picture.Date = DateTime.Now;
            return View(picture);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken()] 
        public ActionResult AddNewPicture(Picture picture, HttpPostedFileBase image)
        {
            if (image == null)
            {
                ModelState.AddModelError("", "Image field is empty. Choose an image!");
            }
            var albumName = RouteData.Values["albumName"];
            var album = _albumRepository.Albums.FirstOrDefault(a => a.Name == albumName);
            if (album == null)
                return View("Error", "_Layout", "Album name isn't exist.");

            if (album.Pictures.Any(p => p.Name == picture.Name))
            {
                ModelState.AddModelError("", "Current album has picture with such name. Choose another.");
            }
            if (ModelState.IsValid)
            {
                var user = _userRepository.Users.FirstOrDefault(u => u.UserTag == User.Identity.Name);
                picture.UserId = user.Id;

                picture.ImageMimeType = image.ContentType;
                picture.ImageData = new byte[image.ContentLength];
                image.InputStream.Read(picture.ImageData, 0, image.ContentLength);

                var img = new WebImage(image.InputStream);
                img.Resize(800, 450);
                var stream = img.GetBytes();
                picture.IconData = new byte[stream.Length];
                for (var i = 0; i < stream.Length; i++)
                {
                    picture.IconData[i] = stream[i];
                }
                if (string.IsNullOrEmpty(picture.Name))
                    picture.Name = image.FileName;
                //_pictureRepository.Add(picture);
                album.Pictures.Add(picture);
                _albumRepository.SaveChanges();
                
                return RedirectToAction("Album","AjaxAlbums", new {albumName});
            }
            return View(picture);
        }

        [Authorize]
        public ActionResult EditPicture(string pictureName, string albumName, int index, string searchRequest = "")
        {
            var album = _albumRepository.Albums.FirstOrDefault(a => a.Name == albumName);
            if (album == null)
                return View("Error", "_Layout", "Invalid album name.");
            var picture = album.Pictures.FirstOrDefault(p => p.Name == pictureName);
            if (picture == null)
                return View("Error", "_Layout", "Invalid picture name.");

            ViewBag.index = index;
            ViewBag.albumName = albumName;
            ViewBag.searchRequest = searchRequest;
            return View(picture);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken()] 
        public ActionResult EditPicture(Picture picture, HttpPostedFileBase image)
        {
            var n = RouteData.Values["albumName"].ToString();
            var album = _albumRepository.Albums.FirstOrDefault(a => a.Name == n);
            if (album == null)
                return View("Error","_Layout", "Unexpected error. Album name isn't exist.");
            var pic = album.Pictures.FirstOrDefault(p => p.Id == picture.Id);
            if (pic.Name != picture.Name && album.Pictures.Any(a => a.Name == picture.Name))
                ModelState.AddModelError("", "Picture with such name already exist. Use previous or another.");
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    picture.ImageMimeType = image.ContentType;
                    picture.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(picture.ImageData, 0, image.ContentLength);

                    WebImage img = new WebImage(image.InputStream);
                    img.Resize(800, 450);
                    var stream = img.GetBytes();
                    picture.IconData = new byte[stream.Length];
                    for (var i = 0; i < stream.Length; i++)
                    {
                        picture.IconData[i] = stream[i];
                    }
                }
                _pictureRepository.Update(picture);
                return RedirectToRoute("Ajax_PictureScheme",
                    new
                    {
                        action = "Index",
                        controller = "AjaxPictures",
                        index = Request.QueryString["index"],
                        albumName = RouteData.Values["albumName"],
                        searchRequest = Request.QueryString["searchRequest"]
                    });
            }
            return View(picture);
        }

        [Authorize]
        public ActionResult DeletePicture(string pictureName, string albumName)
        {
            var album = _albumRepository.Albums.FirstOrDefault(a => a.Name == albumName);
            if (album == null)
                return View("Error", "_Layout", "Invalid album name.");
            var picture = album.Pictures.FirstOrDefault(p => p.Name == pictureName);
            if (picture == null)
            {
                return View("Error","_Layout","Invalid picture name.");
            }
            album.Pictures.Remove(picture);
            _pictureRepository.Remove(picture.Id);
            return RedirectToRoute("Ajax_PictureScheme",
                    new
                    {
                        action = "Index",
                        controller = "AjaxPictures",
                        index = RouteData.Values["index"],
                        albumName = RouteData.Values["albumName"],
                        searchRequest = RouteData.Values["searchRequest"]
                    });
        }

        #endregion EDIT
    }
}