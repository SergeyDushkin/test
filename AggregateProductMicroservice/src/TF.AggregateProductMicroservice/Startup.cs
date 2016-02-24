using Microsoft.Owin.Hosting;
using Microsoft.Practices.Unity;
using NLog;
using Owin;
using System;
using System.Web.Http;

namespace TF.AggregateProductMicroservice
{
    public class Startup
    {
        private IDisposable application;

        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            /// Регистрация зависимостей
            Startup.RegisterProductDependency(config);

            /// Регистрация маршрутов
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            /// Подключаем модуль web api
            app.UseWebApi(config);
        }

        private static void RegisterProductDependency(HttpConfiguration config)
        {
            var container = new UnityContainer();

            container.RegisterType<IAggregateProductProductRepository, AggregateProductRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ILogger, Logger>(new HierarchicalLifetimeManager());
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
