using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePhotoManager.Domain.Entities;

namespace OnlinePhotoManager.Domain.Abstract
{
    public interface IUserRepository
    {
        IQueryable<UserProfile> Users { get; }

        IEnumerable<UserProfile> GetAll();
        UserProfile Get(decimal id);
        UserProfile Add(UserProfile item);
        void Remove(decimal id);
        UserProfile ChangeToPremium(UserProfile item);
        bool Update(UserProfile item);
    }
}
