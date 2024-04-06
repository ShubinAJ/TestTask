using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Globalization;

namespace TestTask
{
    internal class FileManager
    {
        public string logPath;
        public string outputPath;
        //IPAddress startAdress;
        //IPAddress endAdress;
        public int startAdress;
        public int endAdress;
        public DateOnly timeStart;
        public DateOnly timeEnd;
        public string[] parameters;

        string[] sourceFileArr;
        public int stage = 0;

        //string[] source;
        //StringBuilder source = new StringBuilder();
        List<string> cmdLineArgs = new List<string>();
        //public FileManager(string str)
        public FileManager(string[] str)

        {
            ////source = str;
            ////parameters = source.Split("--");
            //parameters = str.Split("--").ToArray();
            parameters = str;


            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = parameters[i].Trim();

                if (parameters[i].StartsWith(','))
                {
                    parameters[i] = parameters[i].Substring(1);
                }
                if (parameters[i].EndsWith(','))
                {
                    parameters[i] = parameters[i].Substring(0, parameters[i].Length - 1);
                }

            }


            if (parameters.Length == 5)
            {
                logPath = parameters[1];
                outputPath = parameters[2];
                timeStart = DateOnly.ParseExact(parameters[3], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                timeEnd = DateOnly.ParseExact(parameters[4], "dd.MM.yyyy", CultureInfo.InvariantCulture);
            }
            if (parameters.Length == 6)
            {
                logPath = parameters[1];
                outputPath = parameters[2];
                startAdress = Int32.Parse(parameters[3]);
                timeStart = DateOnly.ParseExact(parameters[4], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                timeEnd = DateOnly.ParseExact(parameters[5], "dd.MM.yyyy", CultureInfo.InvariantCulture);
            }
            if (parameters.Length == 7)
            {
                logPath = parameters[1];
                outputPath = parameters[2];
                startAdress = Int32.Parse(parameters[3]);
                endAdress = Int32.Parse(parameters[4]);
                timeStart = DateOnly.ParseExact(parameters[5], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                timeEnd = DateOnly.ParseExact(parameters[6], "dd.MM.yyyy", CultureInfo.InvariantCulture);
            }

        }


        //public FileManager(StringBuilder sb)
        //{
        //    source = sb;
        //    parameters = source.ToString().Split("--");
        //    if (parameters.Length == 4)
        //    {
        //        logPath = parameters[0];
        //        outputPath = parameters[1];
        //        timeStart = DateOnly.ParseExact(parameters[2], "dd.MM.yyyy", CultureInfo.InvariantCulture); 
        //        timeEnd = DateOnly.ParseExact(parameters[3], "dd.MM.yyyy", CultureInfo.InvariantCulture); 
        //    }
        //    if (parameters.Length == 5)
        //    {
        //        logPath = parameters[0];
        //        outputPath = parameters[1];
        //        //startAdress = parameters[2];
        //        startAdress = Int32.Parse(parameters[2]);
        //        timeStart = DateOnly.ParseExact(parameters[3], "dd.MM.yyyy", CultureInfo.InvariantCulture);
        //        timeEnd = DateOnly.ParseExact(parameters[4], "dd.MM.yyyy", CultureInfo.InvariantCulture);
        //    }
        //    if (parameters.Length == 6)
        //    {
        //        logPath = parameters[0];
        //        outputPath = parameters[1];
        //        startAdress = Int32.Parse(parameters[2]);
        //        endAdress = Int32.Parse(parameters[3]);
        //        timeStart = DateOnly.ParseExact(parameters[4], "dd.MM.yyyy", CultureInfo.InvariantCulture);
        //        timeEnd = DateOnly.ParseExact(parameters[5], "dd.MM.yyyy", CultureInfo.InvariantCulture);
        //    }

        //}



        


        public bool Check (string str)
        {
            string source = str;
            if (source != null) 
            {
                logPath = source;
                return true;

            }
            else return false;
        }

        public string[] GetFile(string src)
        {
            logPath = src;
            try
            {
                if (File.Exists(logPath))
                {
                    sourceFileArr = File.ReadAllLines(logPath);
                    Console.WriteLine("Файл успешно считан");
                    stage++;
                    
                }
                else
                {
                    Console.WriteLine("Исходный файл не найден");
                    
                }

            }
            catch (Exception e)
            {

                //Console.WriteLine("Исходный файл не найден");
            }
            return sourceFileArr;
        }
        public string[] Sort(string[] src)
        {
            //this.startAdress = 
            string[] files = new string[1];
            return files;
        }

        //public void GetFile (string src)
        //{
        //    logPath = src;
        //    try
        //    {
        //        if (File.Exists(logPath))
        //        {
        //            sourceFileArr = File.ReadAllLines(logPath);
        //            Console.WriteLine("Файл успешно считан");
        //            stage++;
        //        }
        //        else
        //        {
        //            Console.WriteLine("Исходный файл не найден");
        //        }

        //    }
        //    catch (Exception e)
        //    {

        //        //Console.WriteLine("Исходный файл не найден");
        //    }

        //}

        public void CreateFile (string dst)
        {
            outputPath = dst;
            try
            {
                //sourceFileArr = File.ReadAllLines(logPath);
                File.AppendAllLines(outputPath, sourceFileArr);
                Console.WriteLine("Файл успешно создан");
                stage++;
            }
            catch (Exception e)
            {
                Console.WriteLine("Некорректный путь к файлу назначения");
            }
        }
        public void CreateStartDate (string str)
        {
            try
            {
                timeStart = DateOnly.ParseExact(str, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                stage++;
            }
            catch (Exception e) 
            {
                Console.WriteLine("Дата введена некорректно");
            }
        }
        public void CreateEndDate(string str)
        {
            try
            {
                timeEnd = DateOnly.ParseExact(str, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                stage++;
            }
            catch (Exception e)
            {
                Console.WriteLine("Дата введена некорректно");
            }

        }
    }
}
