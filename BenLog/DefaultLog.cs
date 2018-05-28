using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenLog
{
    public static class DefaultLog
    {
        private static LogWriter MyDefaultLog = new LogWriter("log.txt", AppDomain.CurrentDomain.BaseDirectory);

        //Work
        //private static Log DefaultLog = new Log(@"C:\Users\Ben\Desktop\BTS Class Lib + Con Test (Friday 5PM)\log.txt");

        //Home
        //private static Log DefaultLog = new Log(@"M:\Users\benba\My Documents\Production\Code Projects\C#\Bug Tracking System\BTS Class Lib + Con Test\log.txt");

        public static string LogFileDir { get { return MyDefaultLog.LogFileDir; } }
        public static string LogFileName { get { return MyDefaultLog.LogFileName; } }

        public static void Break()
        {
            MyDefaultLog.Break();
        }

        public static void DeleteLog()
        {
            MyDefaultLog.DeleteLog();
        }

        /*public static void Input(string value)
        {
            DefaultLog.Input(value);
        }

        public static void Output(string value)
        {
            DefaultLog.Output(value);
        }

        public static void Func(string value)
        {
            DefaultLog.Func(value);
        }*/

        public static void Debug(string value)
        {
            MyDefaultLog.Debug(value);
        }

        public static void Info(string value)
        {
            MyDefaultLog.Info(value);
        }

        public static void Error(string value)
        {
            MyDefaultLog.Error(value);
        }

        public static void Warning(string value)
        {
            MyDefaultLog.Warning(value);
        }

        public static void ClearLog()
        {
            MyDefaultLog.ClearLog();
        }
    }
}
