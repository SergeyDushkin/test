﻿using System;
using Owin;
using System.Web.Http;
using Microsoft.Owin.Hosting;

namespace TF.PersonMicroservice
{
    public class Startup
    {
        private IDisposable application;

        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

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

            new TF.Business.PersonService().Init();
        }

        public void Stop()
        {
            application.Dispose();
        }
    }
}
