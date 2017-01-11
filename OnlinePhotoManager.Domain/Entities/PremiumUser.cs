using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePhotoManager.Domain.Entities
{
    public class PremiumUser : UserProfile
    {
        public PremiumUser()
        {
            IsPremium = true;
        }
    }
}
