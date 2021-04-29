using Microsoft.Owin.Hosting;
using System;
using System.Timers;

namespace Cruncher
{
    class Program
    {
        static void Main(string[] args)
        {
            Configuration.Configure();
            //string url = "http://localhost:9191";
            //using(WebApp.Start(url))
            //{
            //    schedule();
            //    Console.WriteLine("Server running on {0}", url);
            //    Console.ReadLine();
            //}
        }


        private static void schedule()
        {
            Timer timer1 = new Timer();
            timer1.Elapsed += (s, e) =>
            {
                Diagnostics.SendData();
            };
            timer1.Interval = 2000;
            timer1.Start();
        }
    }
}
