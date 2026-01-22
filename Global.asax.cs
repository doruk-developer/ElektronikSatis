using ElektronikSatis.Helpers;
using ElektronikSatis.Interfaces;
using ElektronikSatis.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Web.Mvc;
using System.Web.Routing;

namespace ElektronikSatis
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var services = new ServiceCollection();

            // Servisleri kaydet
            services.AddScoped<ILoginService, LoginService>();

            // Controller'larý kaydet (ÖNEMLÝ!)
            services.AddControllersAsServices(typeof(MvcApplication).Assembly);

            var serviceProvider = services.BuildServiceProvider();

            DependencyResolver.SetResolver(new DefaultDependencyResolver(serviceProvider));


            GlobalFilters.Filters.Add(new AuthorizeAttribute());
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
