using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace TestTask
{
    internal class FileManager
    {
        public string logPath;
        public string outputPath;
        public IPAddress? adressStart;
        public IPAddress? inverseSubnetMask;
        public IPAddress? adressBroadcast;

        public int adressMask;
        public DateOnly timeStart;
        public DateOnly timeEnd;
        public string[] parameters;

        public string[] sourceFileArr;

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
                try
                {
                    timeStart = DateOnly.ParseExact(parameters[4], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    timeEnd = DateOnly.ParseExact(parameters[5], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    adressStart = IPAddress.Parse(parameters[3]);
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
                try
                {
                    timeStart = DateOnly.ParseExact(parameters[5], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    timeEnd = DateOnly.ParseExact(parameters[6], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    adressStart = IPAddress.Parse(parameters[3]);
                    adressMask = Int32.Parse(parameters[4]);
                    inverseSubnetMask = IPAddress.Parse(GetDecSubNetMask(GetBinInverseSubNetMask(adressMask)));
                    adressBroadcast = GetBroadcastAddress(inverseSubnetMask, adressStart);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Некорректный формат параметра");
                }
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

        static string GetDecSubNetMask(string binSubNetMask)
        {
            string[] subNetMask = binSubNetMask.Split('.');
            for (int i = 0; i < subNetMask.Length; i++)
            {
                subNetMask[i] = Convert.ToInt32(subNetMask[i], 2).ToString();
            }
            return String.Join(".", subNetMask);
        }

        static IPAddress GetBroadcastAddress(IPAddress subnetMask, IPAddress hostIp)
        {
            var ipBytes = hostIp.GetAddressBytes();
            var maskBytes = subnetMask.GetAddressBytes();
            //IpV4
            var broadcastBytes = Enumerable.Range(0, 4).Select((index) => (byte)(ipBytes[index] ^ maskBytes[index])).ToArray();
            return new IPAddress(broadcastBytes);
        }


        static bool inRange(IPAddress startIP, IPAddress brcstIP, IPAddress curIP, DateOnly startDate, DateOnly endDate, DateTime curDateTime)
        {
            if (startDate != null && endDate != null && startIP != null && brcstIP != null && startDate < endDate)
            {
                byte[]? lowIP = startIP.GetAddressBytes();
                byte[]? broadcasdIP = brcstIP.GetAddressBytes();
                byte[] currentIP = curIP.GetAddressBytes();

                int startDateYear = 0, startDateMonth = 0, startDateDay = 0, endDateYear = 0, endDateMonth = 0, endDateDay = 0;

                startDateYear = startDate.Year;
                startDateMonth = startDate.Month;
                startDateDay = startDate.Day;
                endDateYear = endDate.Year;
                endDateMonth = endDate.Month;
                endDateDay = endDate.Day;
                DateTime? lowDate;
                DateTime? highDate;
                string lowDateString = "";
                string highDateString = "";

                lowDateString += startDateYear + "-";
                lowDateString += startDateMonth + "-";
                lowDateString += startDateDay + " " + "00:00:00";
                highDateString += endDateYear + "-";
                highDateString += endDateMonth + "-";
                highDateString += endDateDay + " " + "00:00:00";
                lowDate = DateTime.Parse(lowDateString, CultureInfo.InvariantCulture);
                highDate = DateTime.Parse(highDateString, CultureInfo.InvariantCulture);
                bool result = false;
                for (int i = 0; i < currentIP.Length; i++)
                {
                    if (lowIP != null & broadcasdIP != null)
                    {
                        if (currentIP[i] > lowIP[i] && currentIP[i] < broadcasdIP[i] && curDateTime >= lowDate && curDateTime <= highDate)
                        {
                            result = true;
                        }
                        else result = false;
                    }
                    if (lowIP != null & broadcasdIP == null)
                    {
                        if (currentIP[i] > lowIP[i] && curDateTime >= lowDate && curDateTime <= highDate)
                        {
                            result = true;
                        }
                        else result = false;
                    }
                }
                return result;
            }

            if (startDate != null && endDate != null && startIP != null && startDate < endDate)
            {
                byte[]? lowIP = startIP.GetAddressBytes();
                byte[] currentIP = curIP.GetAddressBytes();

                int startDateYear = 0, startDateMonth = 0, startDateDay = 0, endDateYear = 0, endDateMonth = 0, endDateDay = 0;

                startDateYear = startDate.Year;
                startDateMonth = startDate.Month;
                startDateDay = startDate.Day;
                endDateYear = endDate.Year;
                endDateMonth = endDate.Month;
                endDateDay = endDate.Day;
                DateTime? lowDate;
                DateTime? highDate;
                string lowDateString = "";
                string highDateString = "";

                lowDateString += startDateYear + "-";
                lowDateString += startDateMonth + "-";
                lowDateString += startDateDay + " " + "00:00:00";
                highDateString += endDateYear + "-";
                highDateString += endDateMonth + "-";
                highDateString += endDateDay + " " + "00:00:00";
                lowDate = DateTime.Parse(lowDateString, CultureInfo.InvariantCulture);
                highDate = DateTime.Parse(highDateString, CultureInfo.InvariantCulture);
                bool result = false;
                for (int i = 0; i < currentIP.Length; i++)
                {
                    if (lowIP != null)
                    {
                        if (currentIP[i] > lowIP[i] && curDateTime >= lowDate && curDateTime <= highDate)
                        {
                            result = true;
                        }
                        else result = false;
                    }
                }
                return result;
            }

            if (startDate != null && endDate != null && startDate < endDate)
            {
                byte[] currentIP = curIP.GetAddressBytes();

                int startDateYear = 0, startDateMonth = 0, startDateDay = 0, endDateYear = 0, endDateMonth = 0, endDateDay = 0;

                startDateYear = startDate.Year;
                startDateMonth = startDate.Month;
                startDateDay = startDate.Day;
                endDateYear = endDate.Year;
                endDateMonth = endDate.Month;
                endDateDay = endDate.Day;
                DateTime? lowDate;
                DateTime? highDate;
                string lowDateString = "";
                string highDateString = "";

                lowDateString += startDateYear + "-";
                lowDateString += startDateMonth + "-";
                lowDateString += startDateDay + " " + "00:00:00";
                highDateString += endDateYear + "-";
                highDateString += endDateMonth + "-";
                highDateString += endDateDay + " " + "00:00:00";
                lowDate = DateTime.Parse(lowDateString, CultureInfo.InvariantCulture);
                highDate = DateTime.Parse(highDateString, CultureInfo.InvariantCulture);
                bool result = false;
                for (int i = 0; i < currentIP.Length; i++)
                {
                        if (curDateTime >= lowDate && curDateTime <= highDate)
                        {
                            result = true;
                        }
                        else result = false;
                }
                return result;
            }


            else return false;
        }

        private List<string> GetIpList(string[] strList)
        {
            int index;
            IPAddress curIP;
            DateTime curDateTime;
            List<string> ipList = new List<string>();
            for (int i = 0; i < strList.Length; i++)
            {
                try
                {
                    string dateString, format;
                    index = 0;
                    index = strList[i].IndexOf(':');
                    curIP = IPAddress.Parse(strList[i].Substring(0, index));
                    dateString = strList[i].Substring(index + 1, strList[i].Length - (index + 1));
                    curDateTime = DateTime.ParseExact(dateString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    if (inRange(adressStart, adressBroadcast, curIP, timeStart, timeEnd, curDateTime))
                    {
                        ipList.Add(strList[i]);
                        Console.WriteLine("Добавлено " + strList[i]);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Неверный формат данных внутри исходного файла");
                }
            }
            return ipList;
        }

        public void CreateFile(string dst)
        {
            outputPath = dst;
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
            try
            {
                    File.AppendAllLines(outputPath, this.GetIpList(this.sourceFileArr));
                    Console.WriteLine("Файл успешно создан");
            }
            catch (Exception e)
            {
                Console.WriteLine("Некорректный путь к файлу назначения");
            }
        }

        public void GetFile(string src)
        {
            logPath = src;
            try
            {
                if (File.Exists(logPath))
                {
                    sourceFileArr = File.ReadAllLines(logPath);
                    Console.WriteLine("Файл успешно считан");
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
        }
    }
}
