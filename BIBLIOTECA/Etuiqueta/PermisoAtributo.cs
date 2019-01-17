using BIBLIOTECA.Models.Acciones;
using BIBLIOTECA.Models.Permiso;
using System.Web.Mvc;
using System.Web.Routing;

namespace BIBLIOTECA.Etuiqueta
{
    public class PermisoAtributo : ActionFilterAttribute
    {
        public AccionesEnum IdAccion { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (!Permisos.TienePermiso(this.IdAccion))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Home",
                    action = "Index"
                }));
            }
        }
    }
}