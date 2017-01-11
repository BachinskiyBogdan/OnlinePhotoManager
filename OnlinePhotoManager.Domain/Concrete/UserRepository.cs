using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePhotoManager.Domain.Abstract;
using OnlinePhotoManager.Domain.Entities;

namespace OnlinePhotoManager.Domain.Concrete
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public UserRepository(EFDbContext context) : base(context)
        {
        }

        public IQueryable<UserProfile> Users
        {
            get { return DataContext.Users; }
        }

        public IEnumerable<UserProfile> GetAll()
        {
            return GetList<UserProfile>();
        }

        public UserProfile Get(decimal id)
        {
            return Get<UserProfile>(k => k.Id == id);
        }

        public UserProfile Add(UserProfile item)
        {
            if (item.Id == 0)
                DataContext.Users.Add(item);
            if (Save<UserProfile>(item)) return item;
            return null;
        }

        public void Remove(decimal id)
        {
            Delete<UserProfile>(Get<UserProfile>(k => k.Id == id));
        }

        public UserProfile ChangeToPremium(UserProfile item)
        {
            if (item.IsPremium)
                return item;
            var premUser = new PremiumUser()
            {
                DateOfBirth = item.DateOfBirth,
                IsPremium = true,
                FirstName = item.FirstName,
                LastName = item.LastName,
                UserTag = item.UserTag,
                Email = item.Email,
                Password = item.Password
            };
            Remove(item.Id);
            return Add(premUser);
        }

        public bool Update(UserProfile item)
        {
            var r = Users.FirstOrDefault(a => a.Id == item.Id);
            Update<UserProfile>(r);
            if (r != null)
            {
                CheckHierarchy(item, r);
                r.FirstName = item.FirstName;
                r.LastName = item.LastName;
                r.UserTag = item.UserTag;
                r.Password = item.Password;
                r.Email = item.Email;
                r.IsPremium = item.IsPremium;
                r.DateOfBirth = item.DateOfBirth;
                r.Description = item.Description;
                r.IconData = item.IconData;
                r.IconMimeType = item.IconMimeType;

                return Save<UserProfile>(item);
            }
            return false;
        }

        private void CheckHierarchy(UserProfile item, UserProfile user)
        {
            if (!item.IsPremium)
            {
                if (user as FreeUser == null)
                    user = new FreeUser();
                ((FreeUser) user).MaxAlbums = ((FreeUser) item).MaxAlbums;
                ((FreeUser)user).MaxPictures = ((FreeUser)item).MaxPictures;
            }
            else
            {
                if (user as PremiumUser == null)
                    user = new PremiumUser();
            }
        }

        private void UpdateFreeUser(UserProfile user, FreeUser item)
        {
            
        }
        private void UpdatePremiumUser(UserProfile user, PremiumUser item)
        {

        }
    }
}
