using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using BenLog;
using System.Diagnostics;

namespace BenLog
{
    public class LogReader
    {
        public string MyFileDir { get; set; }
        public List<LogItem> LogItems { get { return _MyLogItems; } }
        private List<LogItem> _MyLogItems;
        public int NoOfRecords { get { return _MyLogItems.Count(); } }
        public LogReader(string pDir)
        {
            DefaultLog.Info("Test");
            MyFileDir = pDir;
        }

        public bool DispLogUsingMsNotepad()
        {
            string TempDir = @"C:\WINDOWS\system32\notepad.exe";
            if (Directory.Exists(TempDir))
            {
                Process Notepad = new Process();
                Notepad.StartInfo.FileName = TempDir;
                Notepad.StartInfo.Arguments = String.Format("\"{0}\"", MyFileDir);
                Notepad.EnableRaisingEvents = true;
                
                Notepad.Start();
                //Notepad.WaitForExit();

                return true;

            }
            else
            {
                return false;
            }
        }

        public bool DispLogUsingLogReader()
        {
            string TempDir = String.Format(@"{0}BenLogParser.exe", AppDomain.CurrentDomain.BaseDirectory);
            if (Directory.Exists(TempDir)) {

                Process LogParser = new Process();
                /*
                LogParser.StartInfo.FileName = String.Format(@"{0}BenLogParser\bin\Debug\BenLogParser.exe", 
                    Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\")));
                    */

                LogParser.StartInfo.FileName = TempDir;

                /*Console.WriteLine("LogParser.StartInfo.FileName = " + LogParser.StartInfo.FileName);
                Console.ReadLine();*/
                //LogParser.StartInfo.FileName = @"C:\Users\Ben\Desktop\BenLog\BenLogParser\bin\Debug\BenLogParser.exe";
                LogParser.StartInfo.Arguments = String.Format("\"{0}\"", MyFileDir);
                LogParser.EnableRaisingEvents = true;

                LogParser.Start();

                //LogParser.WaitForExit();

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool LogIsAvailable { get {
                return FileIsAvailable(MyFileDir);
            }
        }

        private bool FileIsAvailable(string pDir)
        {
            FileStream stream = null;
            try
            {
                stream = new FileInfo(pDir).Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return false;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is available (not locked)
            return true;
        }

        public bool ExtractData()
        {
            try
            {
                _MyLogItems = new List<LogItem>();
                DefaultLog.Break();
                DefaultLog.Break();
                DefaultLog.Break();

                LogItem PreviousItem = null;

                using (StreamReader reader = new StreamReader(MyFileDir))
                {
                    while (!reader.EndOfStream)
                    {
                        var MyRecord = reader.ReadLine();
                        string MyRecordInfo = MyRecord.Split('-')[0];
                        DefaultLog.Debug(String.Format("MyRecordInfo = \"{0}\"", MyRecordInfo));

                        if (MyRecordInfo.Contains('<') && MyRecordInfo.Contains('>'))
                        {
                            string MyRecordTag = MyRecordInfo.Substring(1, MyRecordInfo.LastIndexOf('>') - 1);
                            DefaultLog.Debug(String.Format("MyRecordTag = \"{0}\"", MyRecordTag));
                            string MyRecordDateTimeString = MyRecordInfo.Substring(MyRecordInfo.LastIndexOf('>') + 2, MyRecordInfo.Length - 3 - MyRecordInfo.LastIndexOf('>'));
                            DefaultLog.Debug(String.Format("MyRecordDateTimeString = \"{0}\"", MyRecordDateTimeString));
                            DateTime MyRecordDateTime = Convert.ToDateTime(MyRecordDateTimeString);
                            DefaultLog.Debug(String.Format("MyRecordDateTime = \"{0}\"", MyRecordDateTime));
                            string MyRecordText = MyRecord.Substring(MyRecordInfo.Length + 2, MyRecord.Length - MyRecordInfo.Length - 2);
                            DefaultLog.Debug(String.Format("MyRecordText = \"{0}\"", MyRecordText));

                            LogItem Temp = new LogItem(MyRecordTag, MyRecordDateTime, MyRecordText);
                            PreviousItem = Temp;
                            _MyLogItems.Add(Temp);

                        }
                        else
                        {
                            if (PreviousItem != null)
                            {
                                PreviousItem.AppendContent(MyRecordInfo);
                            }
                            else
                            {
                                //Shouldn't get here!
                                throw new NullReferenceException("'LogItem' called \"Previous Item\" is null!");
                            }

                        }
                        DefaultLog.Break();
                        DefaultLog.Break();
                        DefaultLog.Break();
                    }
                }
                return true;
            }
            catch(Exception e)
            {
                
                return false;
            }
        }
    }

    public class LogItem
    {
        public LogItem(string pTag, DateTime pDateTimeCreated, List<string> pContent)
        {
            Tag = pTag;
            DateTimeCreated = pDateTimeCreated;
            _Content.AddRange(pContent);
        }

        public LogItem(string pTag, DateTime pDateTimeCreated, string pContent)
        {
            Tag = pTag;
            DateTimeCreated = pDateTimeCreated;
            Content.Add(pContent);
        }

        public void AppendContent(string pContent)
        {
            _Content.Add(pContent);
        }

        /*internal LogItem(List <string> pContent)
        {
            DateTimeCreated = DateTime.MinValue;
            Content = pContent;
        }*/

        public string Tag { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public List<string> Content { get { return _Content;  } }
        private List<string> _Content = new List<string>();
    }
}
