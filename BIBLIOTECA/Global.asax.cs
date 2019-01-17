using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BIBLIOTECA
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Cada que arrancas verifica si tienes que modificar la base de datos
            //hace que si cambio la tabla (quito campos agrego campos) se midifica en la bd
            Database.SetInitializer(new MigrateDatabaseToLatestVersion
                <Models.BIBLIOTECAContext,
               Migrations.Configuration>());

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
