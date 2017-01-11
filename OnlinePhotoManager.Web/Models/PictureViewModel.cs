using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlinePhotoManager.Domain.Entities;

namespace OnlinePhotoManager.Web.Models
{
    public class PictureViewModel
    {
        public Picture Picture { get; set; }
        public bool IsAdded { get; set; }
    }
}