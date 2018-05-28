using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace BenLog
{
    public class LogWriter
    {
        //protected string version = FileVersionInfo.GetVersionInfo(AppDomain.CurrentDomain.BaseDirectory + @"bin\BenLog.dll").ProductVersion; //Directory changes when this a referenced dll

        internal string version = "1.3";

        internal string _LogFileName;
        internal string _LogFileDir;

        public string LogFileDir { get { return _LogFileDir; } }
        public string LogFileName { get { return _LogFileName; } }

        protected bool _DebugMode = false;
        public bool DebugMode { get { return _DebugMode; } }

        protected LogWriter()
        {

        }

        public LogWriter(string fileName)
        {
            _LogFileName = fileName;
        }

        public LogWriter(string fileName, string fileDirectory)
        {
            _LogFileName = fileName;
            _LogFileDir = fileDirectory;
        }
        
        public void DebugModeOff()
        {
            _DebugMode = false;
        }

        public void DebugModeOn()
        {
            _DebugMode = true;
        }
        
        public void ExceptionAsError(string ExceptionType, string CurrentAction, string Exception)
        {
            Error(FormatExceptionDescription(ExceptionType, CurrentAction));
            WriteLog(Exception);
        }

        public void ExceptionAsWarning(string ExceptionType, string CurrentAction, string Exception)
        {
            Error(FormatExceptionDescription(ExceptionType, CurrentAction));
            WriteLog(Exception);
        }

        private string FormatExceptionDescription(string ExceptionType, string CurrentAction)
        {
            return String.Format("There was an {0} thrown whilst {1}:", ExceptionType, CurrentAction);
        }

        public void Debug(string value)
        {
            if (!DebugMode)
            {
                WriteLogWithPrefix("debug", value);
            }
        }

        public void Info(string value)
        {
            WriteLogWithPrefix("info", value);
        }

        public void Error(string value)
        {
            WriteLogWithPrefix("error", value);
        }

        public void Warning(string value)
        {
            WriteLogWithPrefix("warning", value);
        }

        public void WriteLog(string pString)
        {
            if (_LogFileDir != null)
            {
                if (!System.IO.Directory.Exists(_LogFileDir))
                {
                    System.IO.Directory.CreateDirectory(_LogFileDir);
                }

                if (_LogFileDir.Substring(_LogFileDir.Length - 1, 1) != @"\")
                {
                    _LogFileDir += @"\";
                }
            }

            if (File.Exists(_LogFileDir + _LogFileName))
            {
                //Console.WriteLine("Log exists!");
                using (StreamWriter log = File.AppendText(_LogFileDir + _LogFileName))
                {
                    log.WriteLine(pString);
                }
            }
            else
            {
                try
                {
                    using (FileStream fileStream = new FileStream(_LogFileDir + _LogFileName, FileMode.Create))
                    {

                        using (StreamWriter LogWriter = new StreamWriter(fileStream))
                        {
                            //ShowVers(); 1.2 - This was causing crashes
                            LogWriter.WriteLine(String.Format("<info> {0} - You are using BenLog {1}!", GetDateTime(), version));
                            LogWriter.WriteLine("<info> {0} - Log \"{1}\" did not exist. Log has been created", GetDateTime(), _LogFileName);
                            LogWriter.WriteLine(pString);
                        }
                    }
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine("Error while creating log. Directory does not exist!");
                    Console.ReadLine();
                }

            }
        }

        private void WriteLogWithPrefix(string prefix, string value)
        {
            WriteLog(string.Format("<{0}> {1} - {2}", prefix, GetDateTime(), value));
        }

        private void WriteLogBlank()
        {
            WriteLog("");
        }

        public void ClearLog()
        {
            if (_LogFileDir != null)
            {
                if (!System.IO.Directory.Exists(_LogFileDir))
                {
                    System.IO.Directory.CreateDirectory(_LogFileDir);
                }

                if (_LogFileDir.Substring(_LogFileDir.Length - 1, 1) != @"\")
                {
                    _LogFileDir += @"\";
                }
            }

            if (File.Exists(_LogFileDir + _LogFileName))
            {
                //Console.WriteLine("Log exists!");
                //ShowVers(); 1.2 - This was causing crashes
                //Info(String.Format("You are using BenLog {0}!", version));
                File.WriteAllText(_LogFileDir + _LogFileName, String.Format("<info> {0} - Log \"{1}\" was reset!", GetDateTime(), _LogFileName));


                WriteLogBlank(); //1.1 This is to stop the next line of the log being put on the same line as above

                Info(String.Format("You are using BenLog {0}!", version));
            }
            else
            {
                /*using (FileStream fileStream = new FileStream(_filePath + _fileName, FileMode.OpenOrCreate))
                {

                    using (StreamWriter log = new StreamWriter(fileStream))
                    {
                        log.WriteLine("<info> {0} - Log \"{1}\" did not exist. Log has been created", GetDateTime(), _fileName);
                        log.WriteLine("");
                    }
                }*/
            }
        }

        public void DeleteLog()
        {
            File.Delete(_LogFileDir);
        }

        public void Break()
        {
            WriteLogBlank();
        }

        private static string GetDateTime()
        {
            return System.DateTime.Now.ToString();
        }
    }
}
