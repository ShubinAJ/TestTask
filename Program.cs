using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace TestTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //для отладки
            Console.WriteLine("--C:\\Users\\Александр\\Desktop\\source.txt");
            Console.WriteLine("--C:\\Users\\Александр\\Desktop\\output.txt");
            Console.WriteLine("--23.02.2023");
            Console.WriteLine("--12.05.2024");
            Console.WriteLine("--C:\\Users\\Александр\\Desktop\\source.txt--C:\\Users\\Александр\\Desktop\\output.txt--23.02.2023--12.05.2024");
            Console.WriteLine("--C:\\Users\\Александр\\Desktop\\source.txt_--C:\\Users\\Александр\\Desktop\\output.txt --23.02.2023 --12.05.2024");
            Console.WriteLine("--C:\\Users\\Александр\\Desktop\\source.txt--C:\\Users\\Александр\\Desktop\\output.txt --196.68.1.96--23.02.2023 --12.05.2024");
            Console.WriteLine("--C:\\Users\\Александр\\Desktop\\source.txt--C:\\Users\\Александр\\Desktop\\output.txt --192.164.223.69 --27 --20.03.2024 --20.05.2024");
            //Console.WriteLine("--C:\\Users\\Александр\\Desktop\\source.txt--C:\\Users\\Александр\\Desktop\\output.txt --196.68.1.96 --192.168.223.141 --23.02.2023 --12.05.2024");



            string[] envArgsList = Environment.CommandLine.Split("--").ToArray();
            string[] consoleArgsList = Console.ReadLine().Split("--").ToArray();

            //FileManager fm = new FileManager(envArgsList);
            //FileManager fm1 = new FileManager(consoleArgsList);

            //string envArgsList = Environment.CommandLine;
            //string consoleArgsList = Console.ReadLine();
            if (envArgsList.Length > consoleArgsList.Length)
            {
                FileManager fm = new FileManager(envArgsList);
                Show(fm);
                fm.GetFile(fm.logPath);
                fm.CreateFile(fm.outputPath);
            }
            if (envArgsList.Length < consoleArgsList.Length)
            {
                FileManager fm = new FileManager(consoleArgsList);
                Show(fm);
                fm.GetFile(fm.logPath);
                fm.CreateFile(fm.outputPath);
            }
            if (envArgsList.Length == consoleArgsList.Length)
            {
                FileManager fm = new FileManager(consoleArgsList);
                Show(fm);
                fm.GetFile(fm.logPath);
                fm.CreateFile(fm.outputPath);
            }

            void Show(FileManager f)
            {
                Console.WriteLine(f.logPath);
                Console.WriteLine(f.outputPath);
                Console.WriteLine(f.adressStart);
                Console.WriteLine(f.inverseSubnetMask);
                Console.WriteLine(f.timeStart);
                Console.WriteLine(f.timeEnd);
            }




            Console.WriteLine("Нажмите любую клавишу для выхода");
            Console.ReadKey();
        }
    }
}
