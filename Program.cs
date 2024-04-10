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
            Console.WriteLine("Введите исходные данные в формате: --путь к файлу-источнику --путь к файлу назначения --начальный IP-адрес(опционально) --маска подсети(опционально, целое число) --начало интервала времени --окончание интервала времени");
            string[] envArgsList = Environment.CommandLine.Split("--").ToArray();
            string[] consoleArgsList = Console.ReadLine().Split("--").ToArray();

            if (envArgsList.Length > consoleArgsList.Length)
            {
                FileManager fm = new FileManager(envArgsList);
                fm.GetFile(fm.logPath);
                if (fm.sourceFileArr != null)
                {
                    fm.CreateFile(fm.outputPath);
                }
            }
            if (envArgsList.Length < consoleArgsList.Length)
            {
                FileManager fm = new FileManager(consoleArgsList);
                fm.GetFile(fm.logPath);
                if (fm.sourceFileArr != null)
                {
                    fm.CreateFile(fm.outputPath);
                }

            }
            if (envArgsList.Length == consoleArgsList.Length)
            {
                FileManager fm = new FileManager(consoleArgsList);
                fm.GetFile(fm.logPath);
                if (fm.sourceFileArr != null)
                {
                    fm.CreateFile(fm.outputPath);
                }
            }

            Console.WriteLine("Нажмите любую клавишу для выхода");
            Console.ReadKey();
        }
    }
}
