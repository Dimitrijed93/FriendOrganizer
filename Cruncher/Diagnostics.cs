using Microsoft.AspNet.SignalR;
using System.Diagnostics;

namespace Cruncher
{
    public static class Diagnostics
    {
        public static string GetCpuUsage()
        {
            return $"{GetCpuCounter().NextValue()}%";
        }

        public static string GetMemoryUsage()
        {
            return $"{GetMemoryCounter().NextValue()}MB";
        }

        public static PerformanceCounter GetCpuCounter()
        {
            return new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        public static PerformanceCounter GetMemoryCounter()
        {
            return new PerformanceCounter("Memory", "Available MBytes");
        }

        public static void SendData()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<CruncherHub>();
            string cpu = GetCpuUsage();
            string memory = GetMemoryUsage();
            context.Clients.All.Send(cpu, memory);
        }
    }
}
