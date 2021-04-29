using Microsoft.AspNet.SignalR;

namespace Cruncher
{
    public class CruncherHub : Hub
    {
        public void Send(string cpuUsage, string memoryUsage)
        {
            Clients.All.addMessage(cpuUsage, memoryUsage);
        }
    }
}
