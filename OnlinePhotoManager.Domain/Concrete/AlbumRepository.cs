using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePhotoManager.Domain.Abstract;
using OnlinePhotoManager.Domain.Entities;

namespace OnlinePhotoManager.Domain.Concrete
{
    public class AlbumRepository : RepositoryBase, IAlbumRepository
    {
        public AlbumRepository(EFDbContext context) : base(context)
        {
        }

        public IQueryable<Album> Albums
        {
            get { return DataContext.Albums; }
        }

        public IEnumerable<Album> GetAll()
        {
            return GetList<Album>();
        }

        public Album Get(decimal id)
        {
            return Get<Album>(k => k.Id == id);
        }

        public Album Add(Album item)
        {
            if (item.Id == 0)
                DataContext.Albums.Add(item);
            if (Save<Album>(item)) return item;
            return null;
        }

        public void Attach(Album item)
        {
            Update<Album>(item);
        }

        public void Remove(decimal id)
        {
            Delete<Album>(Get<Album>(k => k.Id == id));
        }

        public bool Update(Album item)
        {
            var r = Albums.FirstOrDefault(a => a.Id == item.Id);
            if (r != null)
            {
                r.Name = item.Name;
                r.IsPrivate = item.IsPrivate;
                r.Size = item.Size;
                r.CoverData = item.CoverData;
                r.CoverMimeType = item.CoverMimeType;

                return Save<Album>(item);
            }
            return false;
        }

        public void SaveChanges()
        {
            DataContext.SaveChanges();
        }
    }
}
