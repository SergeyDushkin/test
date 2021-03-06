﻿using Topshelf;

namespace TF.CategoryTreeMicroservice
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
                    s.WhenStarted(svc => svc.Start("http://localhost:5544/"));
                    s.WhenStopped(svc => svc.Stop());
                });

                x.RunAsLocalSystem();

                x.SetDescription("TechFactory CategoryTreeMicroservice WebAPI selfhosting Windows Service");
                x.SetDisplayName("TechFactory CategoryTreeMicroservice");
                x.SetServiceName("TechFactory.CategoryTreeMicroservice");
            });
        }
    }
}
