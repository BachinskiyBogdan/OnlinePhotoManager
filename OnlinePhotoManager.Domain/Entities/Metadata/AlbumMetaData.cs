using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePhotoManager.Domain.Entities.Metadata
{
    public partial class AlbumMetaData
    {
        [Required]
        [StringLength(16, MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Private")]
        public bool IsPrivate { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(500)]
        public string Description { get; set; }
    }
}
