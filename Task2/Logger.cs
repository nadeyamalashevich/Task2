using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    class Logger : IDisposable
    {
        private StreamWriter writer;

        public Logger(String filePath)
        {
            FileStream file = File.Create(filePath);
            writer = new StreamWriter(file);
        }

        public void Log(String log)
        {
            writer.WriteLine(log);
        }

        public void Dispose()
        {
            writer.Close();
        }
    }
}
