using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePhotoManager.Domain.Entities;

namespace OnlinePhotoManager.Domain.Abstract
{
    public interface IAlbumRepository
    {
        IQueryable<Album> Albums { get; }

        IEnumerable<Album> GetAll();
        Album Get(decimal id);
        Album Add(Album item);
        void Attach(Album item);
        void Remove(decimal id);
        bool Update(Album item);
        void SaveChanges();
    }
}
