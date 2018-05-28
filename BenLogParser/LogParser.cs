using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenLog;
using System.IO;

namespace BenLogParser
{
    class LogParser
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            if(args.Count() > 0)
            {
                Console.WriteLine("I've already been given the directory of your log!");
            }
            else
            {
                Console.WriteLine("First, I need the log directory. The directory must include the filename AND extension.");
            }
            
            Console.ResetColor();


            string MyLogFileDir = "";

            string MyLogFileExtension = "";

            if (args.Count() > 0)
            {
                if (args[0] == "")
                {
                    GetDir();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("Log directory: ");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(args[0]);
                    Console.ResetColor();
                    if (!CheckDir(args[0]))
                    {
                        GetDir();
                    }
                }
            }
            else
            {
                GetDir();
            }
            

            void GetDir()
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Log directory: ");
                Console.ResetColor();
                string Input = Console.ReadLine();
                if (CheckDir(Input))
                {
                    MyLogFileDir = Input;
                    
                }
                else
                {
                    GetDir();
                }
                
            }

            bool CheckDir(string pDir)
            {
                if (pDir.Contains("."))
                {
                    string TempFileExtension = pDir.Substring(pDir.LastIndexOf('.') + 1, pDir.Length - 1 - pDir.LastIndexOf('.'));



                    if (TempFileExtension.ToLower() == "txt")
                    {

                        string TempFileDir = pDir;
                        if (File.Exists(TempFileDir))
                        {
                            MyLogFileExtension = TempFileExtension;
                            MyLogFileDir = TempFileDir;
                            //Console.WriteLine();
                            return true;
                        }
                        else
                        {
                            WriteError(String.Format("Error - The directory or file \"{0}\" does not exist!. File could not be parsed.", TempFileDir));
                            return false;
                        }
                    }
                    else
                    {
                        WriteError(String.Format("Error - \"{0}\" is not a valid file extension. File could not be parsed.", TempFileExtension));
                        return false;
                    }
                }
                else
                {
                    WriteError(String.Format("Error - The directory \"{0}\" does not have a file extension (eg \".txt\"). File could not be parsed.", pDir));
                    return false;
                }
            }

            void WriteError(string pError)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(pError);
                Console.ResetColor();
            }


            

            bool YesNo(string pQuestion)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(String.Format("{0} (y/n): ", pQuestion));
                Console.ResetColor();
                string Response = Console.ReadLine();
                if (Response.ToLower() == "y")
                {
                    return true;
                }
                else if (Response.ToLower() == "n")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine(String.Format("Input \"{0}\" was not recognised as either a \"y\" or a \"n\". Please try again.", Response));
                    return YesNo(pQuestion);
                }
            }

            Console.WriteLine();

            LogReader MyReader = new LogReader(MyLogFileDir);
            DateTime StartRead = DateTime.Now;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Reading file...");

            int FileReadAttempts = 0;

            CheckFile();
             void CheckFile() {
                FileReadAttempts++;
                if (MyReader.LogIsAvailable)
                {
                    MyReader.ExtractData();
                    DateTime EndRead = DateTime.Now;
                    Console.WriteLine(String.Format("\r{0} records were found in in {1}second(s). File was read successfully!", MyReader.NoOfRecords, Math.Round(((EndRead - StartRead).TotalMilliseconds * 0.001), 2)));
                    Console.ResetColor();
                    Console.WriteLine();
                }
                else
                {
                    //Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Could not access file. Trying again in 3...");
                    System.Threading.Thread.Sleep(1000);
                    Console.Write("\rCould not access file. Trying again in 2...");
                    System.Threading.Thread.Sleep(1000);
                    Console.Write("\rCould not access file. Trying again in 1...");
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine("\rCould not access file. Trying again...     ");
                    CheckFile();
                }
            }

            bool UseDefaults = false;
            bool Debug;
            bool Info;
            bool Error;
            bool Spacing;
            bool Warning;

            if (UseDefaults)
            {
                Console.WriteLine("Nearly ready! I've been told to use the default settings so we'll skip this section.");
                Debug = true;
                Console.WriteLine("  -  Show debug messages");
                Info = true;
                Console.WriteLine("  -  Show info messages");
                Error = true;
                Console.WriteLine("  -  Show error messages");
                Spacing = true;
                Console.WriteLine("  -  Show warning messages");
                Warning = true;
                Console.WriteLine("  -  Using spacing");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Nearly ready! Just let me know which records you would like to show.");
                Console.ResetColor();


                Info = YesNo("Show info records?");
                Error = YesNo("Show error records?");
                Debug = YesNo("Show debug records?");
                Warning = YesNo("Show warning records?");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Spacing can make a file easier to read by leaving a space between each record from the log.");
                Console.ResetColor();
                Spacing = YesNo("Use spacing?");
                Console.WriteLine();
            }

            foreach (LogItem Record in MyReader.LogItems)
            {
                if (Record.DateTimeCreated.ToString() != DateTime.MinValue.ToString())
                {
                    Console.ResetColor();
                }

                switch (Record.Tag)
                {
                    case "info":
                        if (Info)
                        {
                            Console.Write("<info> ");
                            Write();
                        }
                        break;
                    case "debug":
                        if (Debug)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("<debug> ");
                            //Console.ResetColor();
                            Write();
                        }
                        break;
                    case "error":
                        if (Error)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("<error> ");
                            //Console.ResetColor();
                            Write();
                        }
                        break;
                    case "warning":
                        if (Warning)
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write("<warning> ");
                            //Console.ResetColor();
                            Write();
                        }
                        break;
                    default:
                        Write();
                            break;
                       
                }
                void Write()
                {
                        Console.Write(Record.DateTimeCreated);
                        Console.Write(" - ");

                        foreach(string Line in Record.Content)
                        {
                            Console.WriteLine(Line);
                        }

                        
                    
                    
                    if (Spacing)
                    {
                        Console.WriteLine();
                    }
                }
            }
            Console.ReadLine();
        }
    }
}
