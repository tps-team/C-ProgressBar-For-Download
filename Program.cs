using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.ReadKey(true);
            string url = "PASTE YOUR URL HERE";
            string path = @"C:\Program Files\";
            string name = "Example.Ultimate";
            DWL(url, path, name);
            Console.ReadKey(true);
            /*
              ╔═══════════════════════ Скачивание ═══════════════════════╗
              ║ Файл: .                                                  ║
              ║ Прошло: 00:00:00. Осталось: 0000. Скачано: 00000Мб.      ║
              ║ //////////////////////////////////////////////////  000% ║
              ╚══════════════════════════════════════════════════════════╝  
            */
        }

        public static bool DCompleted = false;
        public static bool PrintCompleted = false;
        public static bool AfterDWNEnd = false;
        public static long totalBytes;
        public static int Time = 1;
        public static int TExpired = 0;
        public static int TExpired1 = 0;
        public static int TExpired2 = 0;
        public static int TNeed = 0;
        public static int DSpeed = 0;
        public static int PER1 = 3;
        public static int PER2 = 2;
        public static string nametowrite;
        public static int Per = 0;
        public static int MbRecieved = 0;
        public static int pointwrited = 0;

        static void DWL(string url, string path, string name)
        {
            int pos = Console.CursorTop;
            DCompleted = false;
            var webClient = new WebClient();
            nametowrite = name;
            if (name.Length > 50)
            {
                nametowrite = name.Remove(51);
            }

            bool isFTP = false;
            if (url.Remove(4) == "ftp:")
            {
                isFTP = true;
            }

            if (isFTP == true)
            {
                var request = (FtpWebRequest)WebRequest.Create(url);
                request.Method = WebRequestMethods.Ftp.GetFileSize;
                using (var response = request.GetResponse())
                {
                    totalBytes = response.ContentLength;
                }
            }
            webClient.DownloadProgressChanged += (s, e) =>
            {
                long BytesRemaining = 0;
                if (isFTP == true)
                {
                    Per = (int)(e.BytesReceived * 100 / totalBytes);
                    BytesRemaining = totalBytes - e.BytesReceived;
                }
                else
                {
                    Per = e.ProgressPercentage;
                    BytesRemaining = e.TotalBytesToReceive - e.BytesReceived;
                }
                MbRecieved = (int)e.BytesReceived / 1048576;
                pointwrited = Per / 2;
                if (TExpired == 60)
                {
                    TExpired = TExpired - 60;
                    ++TExpired1;
                }
                if (TExpired1 == 60)
                {
                    TExpired1 = TExpired1 - 60;
                    ++TExpired2;
                }
                if (PER1 + 2 < Time)
                {
                    TNeed = (int)BytesRemaining / (int)e.BytesReceived * Time;
                    PER1 = Time;
                }
                if (PER2 < Time)
                {
                    DSpeed = (int)e.BytesReceived / Time / 1000;
                    PER2 = Time;
                }
            };
            webClient.DownloadFileCompleted += (s, e) =>
            {
                AfterDWNEnd = true;
                while (PrintCompleted == true)
                {
                    Thread.Sleep(20);
                }
                Thread.Sleep(3000);
                Console.SetCursorPosition(0, pos);
                Console.Write(new string(' ', 800));
                Console.SetCursorPosition(0, pos);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("_-=<-(}[");
                Console.Write("Скачивание ");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(name);
                Console.Write(" завершено за " + TExpired1 + "мин. " + TExpired + "сек!");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("]{)->=-_");
                DCompleted = true;

            };
            webClient.DownloadFileAsync(new Uri(url), path + name);

            Thread Printing = new Thread(DownLoadPrint);
            Printing.IsBackground = true;
            Printing.Start();

            do
            {
                Thread.Sleep(1000);
                ++Time;
                ++TExpired;
            } while (DCompleted != true);
            Console.CursorVisible = true;
        }

        static void DownLoadPrint()
        {
            Console.CursorVisible = false;

            int pos = Console.CursorTop;
            do
            {
                Console.SetCursorPosition(0, pos);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("╔═══════════════════════ Скачивание ═══════════════════════╗");
                Console.Write(new string(' ', Console.WindowWidth - Console.CursorLeft));
                Console.SetCursorPosition(0, pos + 1);
                Console.Write("║ ");
                Console.Write("Файл: " + nametowrite);
                Console.Write(new string(' ', 51 - nametowrite.Length - 6 - Convert.ToString(DSpeed).Length));
                Console.Write("~" + DSpeed + "Kbps ");
                Console.Write("║");
                Console.Write(new string(' ', Console.WindowWidth - Console.CursorLeft));
                Console.SetCursorPosition(0, pos + 2);
                Console.Write("║ ");
                Console.Write("Прошло: ");
                if (TExpired2 > 0)
                {
                    if (TExpired2 < 10)
                    {
                        Console.Write("0");
                    }
                    Console.Write(TExpired2);
                    Console.Write(":");
                }
                if (TExpired1 > 0)
                {
                    if (TExpired1 < 10)
                    {
                        Console.Write("0");
                    }
                    Console.Write(TExpired1);
                    Console.Write(":");
                }
                if (TExpired < 10)
                {
                    Console.Write("0");
                }
                Console.Write(TExpired);
                Console.Write(". Осталось: ");
                Console.Write(TNeed + ". Скачано: " + MbRecieved + "Мб.");
                Console.Write(new string(' ', 59 - Console.CursorLeft));
                Console.Write("║");
                Console.Write(new string(' ', Console.WindowWidth - Console.CursorLeft));
                Console.SetCursorPosition(0, pos + 3);
                Console.Write("║ ");
                Random rnd = new Random();
                //Console.Write(new string('█', pointwrited));
                Console.Write(WritedString());
                Console.Write(new string('▒', 50 - pointwrited));
                Console.Write("  ");
                if (Per < 100)
                {
                    Console.Write(" ");
                }
                else if (Per < 10)
                {
                    Console.Write("  ");
                }
                Console.Write(Per + "%");
                Console.SetCursorPosition(59, pos + 3);
                Console.Write("║");
                Console.SetCursorPosition(0, pos + 4);
                Console.Write("╚══════════════════════════════════════════════════════════╝");
                Thread.Sleep(50);
            } while (AfterDWNEnd != true);
            PrintCompleted = true;
            Console.CursorVisible = true;
        }

        static string WritedString()
        {
            string WritedString = "";
            string Symbols = ".@#$%^&*=+-YUOPERWQASDFGHJKLZXCVBNM?[]{}<>";
            Random rnd = new Random();
            while (WritedString.Length != pointwrited)
            {
                int randomcount = rnd.Next(0, 100);
                if (randomcount != 99)
                {
                    
                    WritedString = WritedString + "█";
                }
                else
                {
                    string Temp1 = "";
                    int randomcount1 = rnd.Next(1, Symbols.Length);
                    if (randomcount1 + 1 != Symbols.Length)
                    {
                        Temp1 = Symbols.Remove(randomcount1 + 1);
                    }
                    else
                    {
                        Temp1 = Symbols;
                    }
                    string Temp2 = Temp1.Remove(0, Temp1.Length - 1);
                    WritedString = WritedString + Temp2;
                }
            }
            return WritedString;
        }
    }

}
