using System.Web.Mvc;

namespace OnlinePhotoManager.Web.Areas.Ajax
{
    public class AjaxAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Ajax";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "Ajax_PictureScheme",
                url: "Ajax/{controller}/{userName}/{albumName}/{pictureName}/{action}",
                namespaces: new string[] { "OnlinePhotoManager.Web.Areas.Ajax.Controllers" }
                );

            context.MapRoute(
                name: "Ajax_AlbumScheme",
                url: "Ajax/{controller}/{userName}/{albumName}/{action}",
                namespaces: new string[] { "OnlinePhotoManager.Web.Areas.Ajax.Controllers" }
                );

            context.MapRoute(
                name: "Ajax_MainScheme",
                url: "Ajax/{controller}/{userName}/{action}",
                namespaces: new string[] { "OnlinePhotoManager.Web.Areas.Ajax.Controllers" }
                );
            context.MapRoute(
                name: "Ajax_default",
                url: "Ajax/{controller}/{action}/{id}",
                defaults: new { action = "Index", controller="AjaxHome", id = UrlParameter.Optional },
                namespaces: new string[] { "OnlinePhotoManager.Web.Areas.Ajax.Controllers" }
            );
        }
    }
}