using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePhotoManager.Domain.Entities
{
    public class FreeUser : UserProfile
    {
        public decimal MaxPictures { get; set; } // 30
        public decimal MaxAlbums { get; set; } // 5
    }
}
