using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using OnlinePhotoManager.Domain.Abstract;
using OnlinePhotoManager.Domain.Concrete;

namespace OnlinePhotoManager.Web.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel _ninject;
        public NinjectControllerFactory()
        {
            _ninject = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
                return null;
            else
                return (IController) _ninject.Get(controllerType);
        }

        private void AddBindings()
        {
            _ninject.Bind<EFDbContext>().To<EFDbContext>();
            _ninject.Bind<IAlbumRepository>().To<AlbumRepository>();
            _ninject.Bind<IUserRepository>().To<UserRepository>();
            _ninject.Bind<IPictureRepository>().To<PictureRepository>();
        }
    }
}