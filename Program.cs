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
            string url = "ftp://updater:thisispassword@31.25.29.138/usb1_1/minecraft/DontTouchThisFolder/test.zip";
            string path = @"C:\Program Files\";
            string name = "Необходимо удалить.jar";
            Download(url, path, name);
            Console.ReadKey(true);
            /*
              ╔═══════════════════════ Скачивание ═══════════════════════╗
              ║ Файл: .                                                  ║
              ║ Прошло: 00:00:00. Осталось: 0000. Скачано: 00000Мб.      ║
              ║ //////////////////////////////////////////////////  000% ║
              ╚══════════════════════════════════════════════════════════╝  
            */
        }
        static void Download(string url, string path, string name)
        {
            Console.CursorVisible = false;
            int pos = Console.CursorTop;
            long totalBytes;
            int Time = 1;
            bool ccomp = true;
            bool DCompleted = false;
            int TExpired = 0;
            int TExpired1 = 0;
            int TExpired2 = 0;
            int TNeed = 0;
            int DSpeed = 0;
            int PER1 = 3;
            int PER2 = 2;
            var webClient = new WebClient();
            string nametowrite = name;
            if (name.Length > 50)
            {
                nametowrite = name.Remove(51);
            }
                    var request = (FtpWebRequest)WebRequest.Create(url);
                    request.Method = WebRequestMethods.Ftp.GetFileSize;
                    using (var response = request.GetResponse())
                    {
                        totalBytes = response.ContentLength;
                    }
            webClient.DownloadProgressChanged += (s, e) =>
            {
                int Per = (int)(e.BytesReceived * 100 / totalBytes);
                if (ccomp == true)
                {
                    ccomp = false;
                    int MbRecieved = (int)e.BytesReceived / 1048576;
                    long BytesRemaining = totalBytes - e.BytesReceived;
                    int pointwrited = Per / 2;
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
                        Console.SetCursorPosition(0, pos);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("╔═══════════════════════ Скачивание ═══════════════════════╗");
                    Console.Write(new string(' ', Console.WindowWidth - Console.CursorLeft));
                    Console.SetCursorPosition(0, pos + 1);
                    Console.Write("║ ");
                    Console.Write("Файл: " + nametowrite);
                    Console.Write(new string(' ', 51 - nametowrite.Length));
                    Console.Write("║");
                    Console.Write(new string(' ', Console.WindowWidth - Console.CursorLeft));
                    Console.SetCursorPosition(0, pos + 2);
                    Console.Write("║ ");
                    Console.Write("Прошло: ");
                    if (TExpired2 < 10)
                    {
                        Console.Write("0");
                    }
                    Console.Write(TExpired2);
                    Console.Write(":");
                    if (TExpired1 < 10)
                    {
                        Console.Write("0");
                    }
                    Console.Write(TExpired1);
                    Console.Write(":");
                    if (TExpired < 10)
                    {
                        Console.Write("0");
                    }
                    Console.Write(TExpired);
                    Console.Write(". Осталось: ");
                    Console.Write(TNeed + ". Скачано: " + MbRecieved + "Мб.");
                    Console.Write(new string(' ', 59 - Console.CursorLeft - 2 - Convert.ToString(DSpeed).Length - 4));
                    Console.Write("~" + DSpeed + "Kbps ");
                    Console.Write("║");
                    Console.Write(new string(' ', Console.WindowWidth - Console.CursorLeft));
                    Console.SetCursorPosition(0, pos + 3);
                    Console.Write("║ ");
                    Console.Write(new string('█', pointwrited));
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
                    Console.WriteLine(new string(' ', 400));
                    ccomp = true;
                }

            };
            webClient.DownloadFileCompleted += (s, e) =>
            {
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
            do
            {
                Thread.Sleep(1000);
                ++Time;
                ++TExpired;
            } while (DCompleted != true);
            Console.CursorVisible = true;
        }



    }

}
