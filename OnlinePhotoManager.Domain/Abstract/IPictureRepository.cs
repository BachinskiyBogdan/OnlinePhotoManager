using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePhotoManager.Domain.Entities;

namespace OnlinePhotoManager.Domain.Abstract
{
    public interface IPictureRepository
    {
        IQueryable<Picture> Pictures { get; }

        IEnumerable<Picture> GetAll();
        Picture Get(decimal id);
        Picture Add(Picture item);
        void Attach(Picture item);
        void Remove(decimal id);
        bool Update(Picture item);
        void SaveChanges();
    }
}
