using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePhotoManager.Domain.Entities
{
    [MetadataType(typeof(PictureMetaData))]
    [Table("Picture")]
    public partial class Picture
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime Date { get; set; }
        public string Place { get; set; }
        public string Model { get; set; }
        public int? FocalLength { get; set; }
        public string Diaphragm { get; set; }
        public string ShutterSpeed { get; set; }
        public decimal? ISO { get; set; }
        public bool? IsFlash { get; set; }
        public byte[] ImageData { get; set; }
        public byte[] IconData { get; set; }
        public string ImageMimeType { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Album> Albums { get; set; }

        public string LinkName()
        {
            var name = Name.Replace(" ", "-");
            return Id + "-" + name;
        }
        public void GetInfoFromLink(out int id, out string pictureName, string link)
        {
            if (link == "")
                throw new ArgumentException("link is empty");
            var index = link.Split('-')[0];
            pictureName = link.Remove(0, index.Length).Replace('-', ' ');
            if (!int.TryParse(index, out id))
                throw new ArgumentException("link contains invalid id parameter");
            
        }

        public Picture(Picture entity)
        {
            Id = entity.Id;
            UserId = entity.UserId;
            Name = entity.Name;
            IsPrivate = entity.IsPrivate;
            Date = entity.Date;
            Place = entity.Place;
            Model = entity.Model;
            FocalLength = entity.FocalLength;
            Diaphragm = entity.Diaphragm;
            ISO = entity.ISO;
            ShutterSpeed = entity.ShutterSpeed;
            IsFlash = entity.IsFlash;
            ImageData = entity.ImageData;
            IconData = entity.IconData;
            ImageMimeType = entity.ImageMimeType;
            Description = entity.Description;
            Albums = entity.Albums;
        }
        public Picture()
        {
            Id = 0;
            UserId = 0;
            Name = "";
            IsPrivate = false;
            Date = DateTime.Now;
            Place = "";
            Model = "";
            FocalLength = 0;
            Diaphragm = "";
            ISO = 0;
            ShutterSpeed = "";
            IsFlash = false;
            ImageData = null;
            IconData = null;
            ImageMimeType = "";
            Description = "";
            Albums = new Collection<Album>();
        }
    }
}
