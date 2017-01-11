using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlinePhotoManager.Web.Models
{
    public class PictureSearchViewModel
    {
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public string Place { get; set; }
        public string Model { get; set; }
        public int? FocalLength { get; set; }
        public string Diaphragm { get; set; }
        public string ShutterSpeed { get; set; }
        public decimal? ISO { get; set; }
        public bool? IsFlash { get; set; }
    }
}