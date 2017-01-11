using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePhotoManager.Domain.Entities.Metadata;

namespace OnlinePhotoManager.Domain.Entities
{
    [MetadataType(typeof(AlbumMetaData))]
    [Table("Album")]
    public partial class Album
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }
        public string Name { get; set; }
        public decimal UserId { get; set; }
        public decimal Size { get; set; } // count of pictures in album
        public bool IsPrivate { get; set; }
        public byte[] CoverData { get; set; }
        public string CoverMimeType { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Picture> Pictures { get; set; }

        public string LinkName()
        {
            var name = Name.Replace(" ", "-");
            return Id + "-" + name;
        }
        public void GetInfoFromLink(out int id, out string albumName, string link)
        {
            if (link == "")
                throw new ArgumentException("link is empty");
            var index = link.Split('-')[0];
            albumName = link.Remove(0, index.Length).Replace('-', ' ');
            if (!int.TryParse(index, out id))
                throw new ArgumentException("link contains invalid id parameter");

        }
    }
}
