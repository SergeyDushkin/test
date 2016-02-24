using Topshelf;

namespace TF.ProductMicroservice
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
                    s.WhenStarted(svc => svc.Start("http://localhost:5555/"));
                    s.WhenStopped(svc => svc.Stop());
                });

                x.RunAsLocalSystem();
                
                x.SetDescription("TechFactory ProductMicroservice WebAPI selfhosting Windows Service");
                x.SetDisplayName("TechFactory ProductMicroservice");
                x.SetServiceName("TechFactory.ProductMicroservice");
            });
        }
    }
}
