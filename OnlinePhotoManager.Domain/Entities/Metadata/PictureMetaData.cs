using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace OnlinePhotoManager.Domain.Entities
{
    public partial class PictureMetaData
    {
        [Required]
        [StringLength(32, MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Private")]
        public bool IsPrivate { get; set; }
        [Required]
        [Display(Name="Creation Date")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        [StringLength(100)]
        public string Place { get; set; }
        [StringLength(50)]
        public string Model { get; set; }
        public int? FocalLength { get; set; }
        [StringLength(50)]
        public string Diaphragm { get; set; }
        [StringLength(5)]
        public string ShutterSpeed { get; set; }
        public decimal? ISO { get; set; }
        [Display(Name="Flash")]
        public bool IsFlash { get; set; }
        public byte[] ImageData { get; set; }
        public byte[] IconData { get; set; }
        public string ImageMimeType { get; set; }
        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}
