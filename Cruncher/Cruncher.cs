using Microsoft.Owin.Hosting;
using System;
using System.Timers;

namespace Cruncher
{
    public class Cruncher
    {
        private Timer timer1;
        protected IDisposable App;

        public void Start()
        {
            App = WebApp.Start<Startup>("http://localhost:9191");
        }

        public void Stop()
        {
            App.Dispose();
        }

        private void loadData()
        {
            string cpu = Diagnostics.GetCpuUsage();
            string memory = Diagnostics.GetMemoryUsage();
        }


        private void schedule()
        {
            timer1 = new Timer();
            timer1.Elapsed += (s, e) =>
            {
                loadData();
            };
            timer1.Interval = 2000; 
            timer1.Start();
        }

    }
}
