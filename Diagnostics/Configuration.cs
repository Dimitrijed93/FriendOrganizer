using Topshelf;

namespace Diagnostics
{
    public class Configuration
    {
        private const string NAME = "Diagnostics";
        private const string DESCRIPTION = "Diagnostics Demo Service";

        internal static void Configure()
        {
            HostFactory.Run(configure =>
            {
                configure.Service<Diagnostics>(service => {

                    service.ConstructUsing(s => new Diagnostics());
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });
                configure.RunAsLocalSystem();
                configure.SetDisplayName(NAME);
                configure.SetDescription(DESCRIPTION);
                configure.SetServiceName(NAME);
            });
        }
    }
}
