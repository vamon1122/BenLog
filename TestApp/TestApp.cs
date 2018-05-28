using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using BenLog;

namespace LaunchReader
{
    class Launch
    {
        static void Main(string[] args)
        {
            /*LogReader MyReader = new LogReader(@"C:\Users\benba\Documents\Visual Studio 2017\Projects\BenLog\log.txt");
            //MyReader.DisplayLog();
            MyReader.DisplayParsedLog();*/
            
            
            
            
            //DefaultLog.Break();
            DefaultLog.Info("Hello world!");

            LogReader MyLogReader = new LogReader(DefaultLog.LogFileDir);

            if (MyLogReader.)
            Console.ReadLine();
        }
    }
}
