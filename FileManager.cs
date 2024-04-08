using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Globalization;
using System.Buffers.Binary;
using System.Text.RegularExpressions;

namespace TestTask
{
    internal class FileManager
    {
        public string logPath;
        public string outputPath;

        //IPAddress endAdress;
        //public int startAdress;
        public IPAddress? adressStart;
        public IPAddress? inverseSubnetMask;
        public IPAddress? adressEnd;

        public int adressMask;
        //public Span<byte> endAdress;

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
                try
                {
                    timeStart = DateOnly.ParseExact(parameters[3], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    timeEnd = DateOnly.ParseExact(parameters[4], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Некорректный формат параметра");
                }


            }
            if (parameters.Length == 6)
            {
                logPath = parameters[1];
                outputPath = parameters[2];
                //startAdress = Int32.Parse(parameters[3]);
                //IPAddress.TryParse(parameters[3],out startAdress);


                //поправить позже
                try
                {
                    adressStart = IPAddress.Parse(parameters[3]);
                    //byte[] bytesOfStartAdress = startAdress.GetAddressBytes();
                    
                    //foreach (byte b in bytesOfStartAdress)
                    //{
                    //    Console.WriteLine(b);
                    //}

                    timeStart = DateOnly.ParseExact(parameters[4], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    timeEnd = DateOnly.ParseExact(parameters[5], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Некорректный формат параметра");
                }


            }
            if (parameters.Length == 7)
            {
                logPath = parameters[1];
                outputPath = parameters[2];
                //startAdress = Int32.Parse(parameters[3]);

                //endAdress = Int32.Parse(parameters[4]);
                //BinaryPrimitives.WriteInt32LittleEndian(endAdress, parameters[4]);

                try
                {
                    adressStart = IPAddress.Parse(parameters[3]);
                    //adressMask = IPAddress.Parse(parameters[4]);
                    adressMask = Int32.Parse(parameters[4]);
                    inverseSubnetMask = IPAddress.Parse(GetDecSubNetMask(GetBinInverseSubNetMask(adressMask)));
                    adressEnd = GetSubnetAddress(inverseSubnetMask, adressStart);
                    timeStart = DateOnly.ParseExact(parameters[5], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    timeEnd = DateOnly.ParseExact(parameters[6], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Некорректный формат параметра");
                }
                //var startAdressUint32 = BitConverter.ToUInt32(IPAddress.Parse(parameters[3]).GetAddressBytes(), 0);
                //Console.WriteLine(startAdressUint32);
                //Console.WriteLine(GetBinSubNetMask(27));
                //Console.WriteLine(GetDecSubNetMask(GetBinSubNetMask(27)));
                Console.WriteLine(adressStart);
                Console.WriteLine(inverseSubnetMask);
                Console.WriteLine(adressEnd);

            }

        }


        static string GetBinInverseSubNetMask(int numberOfOnes)
        {
            String mask = new String('0', numberOfOnes);

            mask += new string('1', 32 - numberOfOnes);

            int number = 0;
            mask = Regex.Replace(mask, ".{1}", (f) => (++number % 8 == 0 && number < 32) ? f.Value + "." : f.Value);

            return mask;
        }


        //static string GetBinSubNetMask(int numberOfOnes)
        //{
        //    String mask = new String('1', numberOfOnes);

        //    mask += new string('0', 32 - numberOfOnes);

        //    int number = 0;
        //    mask = Regex.Replace(mask, ".{1}", (f) => (++number % 8 == 0 && number < 32) ? f.Value + "." : f.Value);

        //    return mask;
        //}

        static string GetDecSubNetMask(string binSubNetMask)
        {
            string[] subNetMask = binSubNetMask.Split('.');

            for (int i = 0; i < subNetMask.Length; i++)
            {
                subNetMask[i] = Convert.ToInt32(subNetMask[i], 2).ToString();
            }

            return String.Join(".", subNetMask);
        }

        static IPAddress GetSubnetAddress(IPAddress subnetMask, IPAddress hostIp)
        {
            var ipBytes = hostIp.GetAddressBytes();
            var maskBytes = subnetMask.GetAddressBytes();

            //IpV4
            var subnetBytes = Enumerable.Range(0, 4).Select((index) => (byte)(ipBytes[index] ^ maskBytes[index])).ToArray();
            return new IPAddress(subnetBytes);
        }


        //static string GetHexSubNetMask(string binSubNetMask)
        //{
        //    string[] subNetMask = binSubNetMask.Split('.');

        //    for (int i = 0; i < subNetMask.Length; i++)
        //    {
        //        subNetMask[i] = Convert.ToInt32(subNetMask[i], 2).ToString("X2");
        //    }

        //    return String.Join(".", subNetMask);
        //}

        //static IPAddress GetSubnetAddress(IPAddress mask, IPAddress hostIp)
        //{
        //    var ipBytes = hostIp.GetAddressBytes();
        //    var maskBytes = mask.GetAddressBytes();

        //    //IpV4
        //    var subnetBytes = Enumerable.Range(0, 4).Select((index) => (byte)(ipBytes[index] & maskBytes[index])).ToArray();
        //    return new IPAddress(subnetBytes);
        //}


        //public Dictionary<byte[], DateTime> GetIpList(string[] strList)
        //{
        //    int index;
        //    string key = "";
        //    string value = "";
        //    Dictionary<byte[] , DateTime> ipList = new Dictionary<byte[], DateTime>();
        //    for (int i = 0; i < strList.Length; i++)
        //    {
        //        index = 0;
        //        index = strList[i].IndexOf(':');
        //        key = strList[i].Substring(0, index - 1);
        //        value = strList[i].Substring(index + 1, strList[i].Length);


        //        //добавить проверку
        //        try
        //        {
        //            ipList.Add(IPAddress.Parse(key).GetAddressBytes(), DateTime.Parse(value));
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine("Неверный формат данных внутри исходного файла");
        //        }
        //    }
        //    return ipList;
        //}








        //public void Sort(Dictionary<byte[], DateTime> dictionary, IPAddress adressStart, IPAddress adressMask)
        //{
        //    if (adressStart != null && adressMask != null)
        //    {
        //        foreach (var dict in dictionary.Where())
        //        {

        //        }
        //    }
        //    if (adressStart != null & adressMask == null)
        //    {

        //    }
        //    else
        //    {

        //    }

        //}


        public void CreateFile(string dst)
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

        //public string[] GetFile(string src)
        public void GetFile(string src)
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

                Console.WriteLine("Исходный файл не найден");
            }






            //return sourceFileArr;
        }

    }
}
