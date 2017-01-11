using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePhotoManager.Domain.Abstract;
using OnlinePhotoManager.Domain.Entities;

namespace OnlinePhotoManager.Domain.Concrete
{
    public class PictureRepository : RepositoryBase, IPictureRepository
    {
        public PictureRepository(EFDbContext context) : base(context)
        {
        }

        public IQueryable<Picture> Pictures
        {
            get { return DataContext.Pictures; }
        }

        public IEnumerable<Picture> GetAll()
        {
            return GetList<Picture>();
        }

        public Picture Get(decimal id)
        {
            return Get<Picture>(k => k.Id == id);
        }

        public Picture Add(Picture item)
        {
            if (item.Id == 0)
                DataContext.Pictures.Add(item);
            if (Save<Picture>(item))
                return item;
            return null;
        }

        public void Attach(Picture item)
        {
            Update<Picture>(item);
        }
        public void Remove(decimal id)
        {
            Delete<Picture>(Get<Picture>(k => k.Id == id));
        }

        public bool Update(Picture item)
        {
            var r = Pictures.FirstOrDefault(p => p.Id == item.Id);
            Update<Picture>(r);
            if (r != null)
            {
                r.Name = item.Name;
                r.Date = item.Date;
                r.Description = item.Description;
                r.ImageData = item.ImageData;
                r.IconData = item.IconData;
                r.ImageMimeType = item.ImageMimeType;
                r.IsPrivate = item.IsPrivate;

                r.Place = item.Place;
                r.Model = item.Model;
                r.FocalLength = item.FocalLength;
                r.Diaphragm = item.Diaphragm;
                r.ShutterSpeed = item.ShutterSpeed;
                r.ISO = item.ISO;
                r.IsFlash = item.IsFlash;

                return Save<Picture>(item);
            }
            return false;
        }

        public void SaveChanges()
        {
            DataContext.SaveChanges();
        }
    }
}
