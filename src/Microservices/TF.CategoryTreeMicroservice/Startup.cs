using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using System.Net.Http;
using System.Web.Http.Routing;

namespace TF.CategoryTreeMicroservice
{
    public class Startup
    {
        private IDisposable application;

        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            /*
            config.Routes.MapHttpRoute(
                name: "DefaultNestedApi",
                routeTemplate: "api/{controller}/{id}/{actions}/",
                defaults: new { id = RouteParameter.Optional }
            );
            */
            app.UseWebApi(config);
        }


        public void Start(string baseAddress)
        {
            application = WebApp.Start<Startup>(url: baseAddress);

            new TF.Business.CategoryTreeService().Init();
        }

        public void Stop()
        {
            application.Dispose();
        }
    }
}
