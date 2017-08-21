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
            Console.Title = "Для запуска загрузки нажмите ENTER";
            Console.ReadKey(true);
            string url = "ftp://updater:thisispassword@31.25.29.138/usb1_1/minecraft/DontTouchThisFolder/Client.zip";
            string path = @"C:\Program Files\";
            string name = "Необходимо удалить.jar";
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
        public static bool PrlongCompleted = false;
        public static bool AfterDWNEnd = false;
        public static long totalBytes;
        public static long Time = 1;
        public static long TExpired = 0;
        public static long TExpired1 = 0;
        public static long TExpired2 = 0;
        public static long TNeed = 0;
        public static long DSpeed = 0;
        public static long PER1 = 3;
        public static long PER2 = 2;
        public static string nametowrite;
        public static int Per = 0;
        public static long MbRecieved = 0;
        public static int pointwrited = 0;
        public static long MBTotal = 0;
        public static int FPS = 0;



        static void DWL(string url, string path, string name)
        {
            Console.Title = "Simle C# ProgressBar!";
            int pos = Console.CursorTop;
            long BytesRecieved = 0;
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
                    Per = Convert.ToInt32(e.BytesReceived * 100 / totalBytes);
                    BytesRemaining = totalBytes - e.BytesReceived;
                }
                else
                {
                    Per = e.ProgressPercentage;
                    BytesRemaining = e.TotalBytesToReceive - e.BytesReceived;
                }
                MbRecieved = e.BytesReceived / 1048576;
                if (isFTP != true)
                {
                    MBTotal = e.TotalBytesToReceive / 1048576;
                }

                else
                {
                    MBTotal = totalBytes / 1048576;
                }

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

                if (PER1 < Time)
                {
                    TNeed = BytesRemaining / e.BytesReceived * Time;
                    PER1 = Time;
                }
                
                BytesRecieved = e.BytesReceived;
            };
            webClient.DownloadFileCompleted += (s, e) =>
            {
                AfterDWNEnd = true;
                while (PrlongCompleted == true)
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

            long BRecieved1 = 0;
            long BRecieved2 = 0;
            long BRecieved3 = 0;
            
            do
            {
                Thread.Sleep(1000);
                ++Time;
                ++TExpired;
                Console.Title = "Simle C# ProgressBar! FPS: " + FPS;
                FPS = 0;

                long LastSec = BytesRecieved - BRecieved1;
                long LastSec2 = BRecieved1 - BRecieved2;
                long LastSec3 = BRecieved2 - BRecieved3;
                DSpeed = ((LastSec + LastSec2 + LastSec3) / 3) / 1000;

                BRecieved3 = BRecieved2;
                BRecieved2 = BRecieved1;
                BRecieved1 = BytesRecieved;
            } while (DCompleted != true);
            Console.CursorVisible = true;
        }

        static void DownLoadPrint()
        {
            Console.CursorVisible = false;

            int pos = Console.CursorTop;
            Console.SetCursorPosition(0, pos);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("╔═══════════════════════ Скачивание ═══════════════════════╗");
            Console.Write(new string(' ', Console.WindowWidth - Console.CursorLeft));

            Console.SetCursorPosition(0, pos + 1);
            Console.Write("║ ");
            Console.Write("Файл: " + nametowrite);
            long temppos1 = Console.CursorLeft;
            Console.SetCursorPosition(0, pos + 2);
            Console.Write("║ ");
            Console.Write("Прошло: ");
            Console.SetCursorPosition(59, pos + 2);
            Console.Write("║");
            Console.SetCursorPosition(0, pos + 3);
            Console.Write("║ ");

            Console.SetCursorPosition(0, pos + 4);
            Console.Write("╚══════════════════════════════════════════════════════════╝");

            long LastDSpeed = 999999999;
            string LastPBar = "";
            string LastTEXP = "";
            do
            {
                Console.SetCursorPosition((int)temppos1, pos + 1);
                if (LastDSpeed != DSpeed)
                {
                    Console.Write(new string(' ', 51 - nametowrite.Length - 6 - Convert.ToString(DSpeed).Length));
                    Console.Write("~" + DSpeed + "Kbps ");
                    Console.Write("║");
                    Console.Write(new string(' ', Console.WindowWidth - Console.CursorLeft));
                    LastDSpeed = DSpeed;
                }
                Console.SetCursorPosition(10, pos + 2);
                string TEXTToWrite = "";

                if (TExpired2 > 0)
                {
                    if (TExpired2 < 10)
                    {
                        TEXTToWrite = TEXTToWrite + "0";
                    }
                    TEXTToWrite = TEXTToWrite + TExpired2 + ":";
                }
                if (TExpired1 > 0)
                {
                    if (TExpired1 < 10)
                    {
                        TEXTToWrite = TEXTToWrite + "0";
                    }
                    TEXTToWrite = TEXTToWrite + TExpired1 + ":";
                }
                if (TExpired < 10)
                {
                    TEXTToWrite = TEXTToWrite + "0";
                }
                TEXTToWrite = TEXTToWrite + TExpired;
                if (TEXTToWrite != LastTEXP)
                {
                    Console.Write(TEXTToWrite);
                    Console.Write(". Осталось: ");
                    Console.Write(TNeed + ". Скачано: " + MbRecieved + " из " + MBTotal + "Mb.");
                    
                }
                

                Console.SetCursorPosition(2, pos + 3);
                Random rnd = new Random();
                string RandomPbarSymbols = WritedString();
                if (RandomPbarSymbols + new string ('▒', 50 - pointwrited) != LastPBar)
                {
                    Console.Write(RandomPbarSymbols);
                    Console.Write(new string('▒', 50 - pointwrited));
                    LastPBar = RandomPbarSymbols + new string('▒', 50 - pointwrited);
                }
                Console.SetCursorPosition(53, pos + 3);
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
                Console.WriteLine("║");
                //Thread.Sleep(100);
                ++FPS;
                LastTEXP = TEXTToWrite;
            } while (AfterDWNEnd != true);
            PrlongCompleted = true;
            Console.CursorVisible = true;
        }

        static string WritedString()
        {
            string WritedString = "";
            string Symbols = ".@#$%^&*=+-YUOPERWQASDFGHJKLZXCVBNM?[]{}<>";
            Random rnd = new Random();
            while (WritedString.Length != pointwrited)
            {
                long randomcount = rnd.Next(0, 100);
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
