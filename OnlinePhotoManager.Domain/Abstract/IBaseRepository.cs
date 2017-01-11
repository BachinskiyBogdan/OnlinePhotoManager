using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using OnlinePhotoManager.Domain.Concrete;

namespace OnlinePhotoManager.Domain.Abstract
{
    public interface IBaseRepository : IDisposable
    {
        EFDbContext DataContext { get; }

        TEntity Get<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        IQueryable<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        IQueryable<TEntity> GetList<TEntity, TKey>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TKey>> orderBy) where TEntity : class;
        IQueryable<TEntity> GetList<TEntity, TKey>(Expression<Func<TEntity, TKey>> orderBy) where TEntity : class;
        IQueryable<TEntity> GetList<TEntity>() where TEntity : class;
        bool Save<TEntity>(TEntity entity) where TEntity : class;
        void Update<TEntity>(TEntity entity, params string[] propsToUpdate) where TEntity : class;
        bool Delete<TEntity>(TEntity entity) where TEntity : class;
    }
}
