using System;
using Owin;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using Microsoft.Practices.Unity;

namespace TF.ProductMicroservice
{
    public class Startup
    {
        private IDisposable application;

        public void Configuration(IAppBuilder app)
        {            
            HttpConfiguration config = new HttpConfiguration();
            Startup.RegisterProductDependency(config);
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            app.UseWebApi(config); 
        }

        private static void RegisterProductDependency(HttpConfiguration config)
        {
            var container = new UnityContainer();
            container.RegisterType<IProductRepository, ProductRepository>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);
        }

        public void Start(string baseAddress)
        {
            application = WebApp.Start<Startup>(url: baseAddress);
        }

        public void Stop()
        {
            application.Dispose();
        }
    }
}
