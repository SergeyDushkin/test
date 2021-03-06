﻿using Topshelf;
namespace TF.AggregateProductMicroservice
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<Startup>(s =>
                {
                    s.ConstructUsing(name => new Startup());
                    s.WhenStarted(svc => svc.Start("http://localhost:5588/"));
                    s.WhenStopped(svc => svc.Stop());
                });

                x.RunAsLocalSystem();

                x.SetDescription("TechFactory AggregateProductMicroservice WebAPI selfhosting Windows Service");
                x.SetDisplayName("TechFactory AggregateProductMicroservice");
                x.SetServiceName("TechFactory.AggregateProductMicroservice");
            });
        }
    }
}
