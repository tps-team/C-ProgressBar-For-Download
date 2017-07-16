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
            int TNeed = 0;
            int DSpeed = 1;
            int PER1 = 3;
            var webClient = new WebClient();
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
                    DSpeed = (int)e.BytesReceived / Time / 1024;
                    int KbitRecieved = (int)e.BytesReceived / 8 / 1024;
                    int KbitTotal = (int)totalBytes / 8 / 1024;
                    long BytesRemaining = totalBytes - e.BytesReceived;
                    int pointwrited = Per / 2;
                        if (TExpired == 60)
                        {
                        TExpired = TExpired - 60;
                        ++TExpired1;
                        }
                        if (PER1 + 2 < Time)
                        {
                        TNeed = (int)BytesRemaining / (int)e.BytesReceived * Time;          
                        PER1 = Time;
                        }
                    int pointwritedE = pointwrited;
                        Console.SetCursorPosition(0, pos);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("Загружаем " + name + "...");
                    Console.Write(new string(' ', 80 - Console.CursorLeft));
                        Console.SetCursorPosition(0, pos + 1);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("◄");
                    if (Per < 30)
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    else if (Per < 66)
                        Console.ForegroundColor = ConsoleColor.Red;
                    else if (Per < 90)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    else
                        Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(new string('█', pointwrited));
                    if (pointwrited < 50)
                    {
                        Random rnd = new Random();
                        int value = rnd.Next(0, 13);
                        if (value == 0) { Console.ForegroundColor = ConsoleColor.Blue; }
                        if (value == 1) { Console.ForegroundColor = ConsoleColor.Cyan; }
                        if (value == 2) { Console.ForegroundColor = ConsoleColor.DarkBlue; }
                        if (value == 3) { Console.ForegroundColor = ConsoleColor.DarkCyan; }
                        if (value == 4) { Console.ForegroundColor = ConsoleColor.DarkGray; }
                        if (value == 5) { Console.ForegroundColor = ConsoleColor.DarkGreen; }
                        if (value == 6) { Console.ForegroundColor = ConsoleColor.DarkMagenta; }
                        if (value == 7) { Console.ForegroundColor = ConsoleColor.DarkRed; }
                        if (value == 8) { Console.ForegroundColor = ConsoleColor.DarkYellow; }
                        if (value == 9) { Console.ForegroundColor = ConsoleColor.Green; }
                        if (value == 10) { Console.ForegroundColor = ConsoleColor.Magenta; }
                        if (value == 11) { Console.ForegroundColor = ConsoleColor.Red; }
                        if (value == 12) { Console.ForegroundColor = ConsoleColor.White; }
                        if (value == 13) { Console.ForegroundColor = ConsoleColor.Yellow; }
                        Console.Write(">");
                        --pointwritedE;
                    }
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(new string('—', 50 - pointwritedE));
                    Console.Write("► ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(Per + "% Завершено!");
                    Console.Write(new string(' ', 80 - Console.CursorLeft));
                    Console.SetCursorPosition(0, pos + 2);
                    Console.Write("Скачано " + KbitRecieved + " Кбайт из " + KbitTotal + " Кбайт. Средняя скорость - " + DSpeed + "КБайт/С");
                    Console.Write(new string(' ', 80 - Console.CursorLeft));
                    Console.SetCursorPosition(0, pos + 3);
                    Console.Write("Прошло времени - " + TExpired1 + " мин. " + TExpired + " сек. Осталось - " + TNeed +  " сек.");
                    Console.WriteLine(new string(' ', 80 - Console.CursorLeft));
                    Console.SetCursorPosition(0, pos + 4);
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
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Скачивание ");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(name);
                Console.ForegroundColor = ConsoleColor.Cyan;
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
