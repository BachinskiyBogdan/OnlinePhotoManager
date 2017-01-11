using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlinePhotoManager.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: null,
                url: "Ajax/",
                defaults: new { controller="AjaxHome", action="Index"},
                namespaces: new string[] { "OnlinePhotoManager.Web.Areas.Ajax.Controllers" }
                );

            routes.MapRoute(
                name: "PictureScheme",
                url: "{controller}/{userName}/{albumName}/{pictureName}/{action}",
                namespaces: new string[]{"OnlinePhotoManager.Web.Controllers"}
                );

            routes.MapRoute(
                name: "AlbumScheme",
                url: "{controller}/{userName}/{albumName}/{action}",
                namespaces: new string[] { "OnlinePhotoManager.Web.Controllers" }
                );

            routes.MapRoute(
                name: "MainScheme",
                url: "{controller}/{userName}/{action}",
                namespaces: new string[] { "OnlinePhotoManager.Web.Controllers" }
                );
      
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index"},
                namespaces: new string[] { "OnlinePhotoManager.Web.Controllers" }
                );

            
        }
    }
}