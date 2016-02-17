using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Web.Http;

namespace TF.PriceMicroservice
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

            app.UseWebApi(config);
        }


        public void Start(string baseAddress)
        {
            application = WebApp.Start<Startup>(url: baseAddress);

            new TF.Business.WMS.PriceService();
        }

        public void Stop()
        {
            application.Dispose();
        }
    }
}
