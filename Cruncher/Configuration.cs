using Topshelf;

namespace Cruncher
{
    public class Configuration
    {
        private const string NAME = "Cruncher";
        private const string DESCRIPTION = "Demo Service";

        internal static void Configure()
        {

            HostFactory.Run(configure =>
            {
                configure.Service<Cruncher>(service =>
                {
                    service.ConstructUsing(s => new Cruncher());
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });
                configure.RunAsLocalSystem();
                configure.SetServiceName(NAME);
                configure.SetDisplayName(NAME);
                configure.SetDescription(DESCRIPTION);
            });
        }
    }
}
