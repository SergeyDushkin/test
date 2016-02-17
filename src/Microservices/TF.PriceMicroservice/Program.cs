using Topshelf;
namespace TF.PriceMicroservice
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
                    s.WhenStarted(svc => svc.Start("http://localhost:5533/"));
                    s.WhenStopped(svc => svc.Stop());
                });

                x.RunAsLocalSystem();

                x.SetDescription("TechFactory PriceMicroservice WebAPI selfhosting Windows Service");
                x.SetDisplayName("TechFactory PriceMicroservice");
                x.SetServiceName("TechFactory.PriceMicroservice");
            });
        }
    }
}
