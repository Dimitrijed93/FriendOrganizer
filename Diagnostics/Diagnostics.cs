
using System;
using System.Diagnostics;
using System.IO;
using System.Timers;

namespace Diagnostics
{
    public class Diagnostics
    {
        private string fileName = "DiagnosticsFile.txt";

        public void Start()
        {
            InitTimer();
        }

        public void Stop()
        {
            string path = GetFilePath();
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public void InitTimer()
        {
            Timer timer = new Timer();
            timer.Elapsed += (s, e) =>
            {
                write();
            };
            timer.Interval = 2000; 
            timer.Start();
        }

        private void write()
        {
            string path = GetFilePath();

            using (StreamWriter sw = new StreamWriter(path, append: true))
            {
                sw.WriteLine($"It is {DateTime.Now}");
                sw.WriteLine($"Used memory: {new PerformanceCounter("Memory", "Available MBytes").NextValue()}");
            }
            
        }

        private string GetFilePath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName); 
        }

    }
}
