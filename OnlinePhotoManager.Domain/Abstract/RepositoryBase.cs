using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using OnlinePhotoManager.Domain.Concrete;


namespace OnlinePhotoManager.Domain.Abstract
{
    public class RepositoryBase : IBaseRepository
    {
        private EFDbContext _context;

        public RepositoryBase(EFDbContext context)
        {
            _context = context;
        }

        public virtual EFDbContext DataContext
        {
            get { return _context; }
        }

        public virtual TEntity Get<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            predicate.CheckNotNull("Predicate value must be passed to Get<TResult>.");

            return DataContext.Set<TEntity>().Where(predicate).SingleOrDefault();
        }

        public virtual IQueryable<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            predicate.CheckNotNull("Predicate value must be passed to GetList<TResult>.");
            try
            {
                return DataContext.Set<TEntity>().Where(predicate);
            }
            catch (Exception ex)
            {
                DataContext.Database.Log.Invoke("Error saving " + typeof(TEntity) + ". " + ex.Message);
                //LoggerHelper.Logger.ErrorException(ex.Message, ex);
            }
            return null;
        }

        public virtual IQueryable<TEntity> GetList<TEntity, TKey>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TKey>> orderBy) where TEntity : class
        {
            try
            {
                return GetList(predicate).OrderBy(orderBy);
            }
            catch (Exception ex)
            {
                DataContext.Database.Log.Invoke("Error saving " + typeof(TEntity) + ". " + ex.Message);
                //LoggerHelper.Logger.ErrorException(ex.Message, ex);
            }
            return null;
        }

        public virtual IQueryable<TEntity> GetList<TEntity, TKey>(Expression<Func<TEntity, TKey>> orderBy) where TEntity : class
        {
            try
            {
                return GetList<TEntity>().OrderBy(orderBy);
            }
            catch (Exception ex)
            {
                DataContext.Database.Log.Invoke("Error saving " + typeof(TEntity) + ". " + ex.Message);
                //Log error
                //LoggerHelper.Logger.ErrorException(ex.Message, ex);
            }
            return null;
        }

        public virtual IQueryable<TEntity> GetList<TEntity>() where TEntity : class
        {
            try
            {
                return DataContext.Set<TEntity>();
            }
            catch (Exception ex)
            {
                DataContext.Database.Log.Invoke("Error saving " + typeof(TEntity) + ". " + ex.Message);
                //Log error
                //LoggerHelper.Logger.ErrorException(ex.Message, ex);
            }
            return null;
        }

        public virtual bool Save<TEntity>(TEntity entity) where TEntity : class
        {
            //try
            //{
            return DataContext.SaveChanges() > 0;
            //}
            //catch (Exception ex)
            //{
            //    DataContext.Database.Log.Invoke("Error saving " + typeof(TEntity) + ". " + ex.Message);
            //    //LoggerHelper.Logger.ErrorException("Error saving " + typeof(TEntity) + ".", ex);
            //    throw;
            //}
        }
        public virtual void Update<TEntity>(TEntity entity, params string[] propsToUpdate) where TEntity : class
        {
            //try
            //{
                DataContext.Set<TEntity>().Attach(entity);
                //return DataContext.SaveChanges() > 0;
            //}
            //catch (Exception ex)
            //{
            //    DataContext.Database.Log.Invoke("Error saving " + typeof(TEntity) + ". " + ex.Message);
            //    //LoggerHelper.Logger.ErrorException("Error saving " + typeof(TEntity) + ".", ex);
            //    throw;
            //}
        }

        public virtual bool Delete<TEntity>(TEntity entity) where TEntity : class
        {
            //try
            //{
                ObjectSet<TEntity> objectSet = ((IObjectContextAdapter)DataContext).ObjectContext.CreateObjectSet<TEntity>();
                objectSet.Attach(entity);
                objectSet.DeleteObject(entity);
                return DataContext.SaveChanges() > 0;
            //}
            //catch (Exception ex)
            //{
            //    DataContext.Database.Log.Invoke(ex.Message);
            //    //LoggerHelper.Logger.ErrorException("Error deleting " + typeof(TEntity), ex);
            //    throw;
            //}

        }

        public void Dispose()
        {
            if (DataContext != null) DataContext.Dispose();
        }
    }


    public static class ObjectExtensions
    {
        public static void CheckNotNull(this object value, string error)
        {
            if (value == null)
                throw new Exception(error);
        }
    }
}
