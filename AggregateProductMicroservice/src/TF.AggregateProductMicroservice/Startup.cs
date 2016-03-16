using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Microsoft.Practices.Unity;
using NLog;
using Owin;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using TF.Data;

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

            /// Регистрация маршрутов odata
            Startup.RegisterOdataRoutes(config);

            ///// Подключаем модуль web api
            app.UseWebApi(config);
        }

        private static void RegisterProductDependency(HttpConfiguration config)
        {
            var container = new UnityContainer();

            container.RegisterType<IAggregateProductProductRepository, AggregateProductRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ICategoryTreeRepository, CategoryTreeRepository>(new InjectionConstructor(System.Configuration.ConfigurationManager.AppSettings["CategoryTreeServiceUri"]));
            container.RegisterType<IProductsCategoryRepository, ProductsCategoryRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ILogger, Logger>(new InjectionFactory(x => LogManager.GetCurrentClassLogger())); 

            config.DependencyResolver = new UnityResolver(container);
        }

        private static void RegisterOdataRoutes(HttpConfiguration config)
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();

            builder.EntitySet<AggregateProduct>("Products");
            builder.EntitySet<CategoryTree>("CategoryTrees");

            config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
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
