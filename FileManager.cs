using System.Globalization;
using System.Net;
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
        public IPAddress? adressBroadcast;

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
                    //timeStart = DateOnly.ParseExact(parameters[3], "yyyy.MM.dd", CultureInfo.InvariantCulture);
                    //timeEnd = DateOnly.ParseExact(parameters[4], "yyyy.MM.dd", CultureInfo.InvariantCulture);
                    //timeStart = DateOnly.Parse(parameters[3], "yyyy.MM.dd");
                    //timeEnd = DateOnly.Parse(parameters[4], "yyyy.MM.dd", CultureInfo.InvariantCulture);
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

                    //timeStart = DateOnly.ParseExact(parameters[4], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    //timeEnd = DateOnly.ParseExact(parameters[5], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    timeStart = DateOnly.ParseExact(parameters[3], "yyyy.MM.dd", CultureInfo.InvariantCulture);
                    timeEnd = DateOnly.ParseExact(parameters[4], "yyyy.MM.dd", CultureInfo.InvariantCulture);
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
                    adressBroadcast = GetBroadcastAddress(inverseSubnetMask, adressStart);
                    timeStart = DateOnly.ParseExact(parameters[5], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    timeEnd = DateOnly.ParseExact(parameters[6], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    //timeStart = DateOnly.ParseExact(parameters[5], "yyyy.MM.dd", CultureInfo.InvariantCulture);
                    //timeEnd = DateOnly.ParseExact(parameters[6], "yyyy.MM.dd", CultureInfo.InvariantCulture);
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
                Console.WriteLine(adressBroadcast);

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
            if (startDate != null && endDate != null)
            {
                byte[]? lowIP = startIP.GetAddressBytes();
                byte[]? broadcasdIP = brcstIP.GetAddressBytes();
                byte[] currentIP = curIP.GetAddressBytes();
                //DateOnly bufStartDate;
                //DateOnly bufEndDate;
                int startDateYear = 0, startDateMonth = 0, startDateDay = 0, endDateYear = 0, endDateMonth = 0, endDateDay = 0;
                startDateYear = startDate.Year;
                startDateMonth = startDate.Month;
                startDateDay = startDate.Day;
                endDateYear = endDate.Year;
                endDateMonth = endDate.Month;
                endDateDay = endDate.Day;

                DateTime? lowDate;
                DateTime? highDate;

                //DateTime lowDate = new DateTime(startDate, new TimeOnly(0, 0, 0));
                //DateTime highDate = new DateTime(endDate, new TimeOnly(0, 0, 0));

                //DateTime lowDate = new DateTime(startDate, 0, 0, 0);
                //DateTime highDate = new DateTime(endDate, 0, 0, 0);
                string lowDateString = "";
                string highDateString = "";

                lowDateString += startDateYear + "-";
                lowDateString += startDateMonth + "-";
                lowDateString += startDateDay + " " + "00:00:00";
                highDateString += endDateYear + "-";
                highDateString += endDateMonth + "-";
                highDateString += endDateDay + " " + "00:00:00";


                //lowDate = DateTime.ParseExact(lowDateString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                //highDate = DateTime.ParseExact(highDateString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
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
            else return false;


        }

        private List<string> GetIpList(string[] strList)
        //public Dictionary<IPAddress, DateTime> GetIpList(string[] strList)
        {
            int index;
            //IPAddress key;
            //DateTime value;
            IPAddress curIP;
            DateTime curDateTime;
            //Dictionary<IPAddress, DateTime> ipList = new Dictionary<IPAddress, DateTime>();
            List<string> ipList = new List<string>();
            for (int i = 0; i < strList.Length; i++)
            {
                try
                {
                    //string str = "";
                    string dateString, format;
                    //CultureInfo provider = CultureInfo.InvariantCulture;
                    index = 0;
                    index = strList[i].IndexOf(':');
                    //str = strList[i].Substring(0, index - 1);
                    //key = IPAddress.Parse(strList[i].Substring(0, index));
                    curIP = IPAddress.Parse(strList[i].Substring(0, index));
                    //Console.WriteLine("Остаток строки " + strList[i]);
                    //key = IPAddress.Parse(str);


                    //dateString = strList[i].Substring(index + 1, strList[i].Length);
                    dateString = strList[i].Substring(index + 1, strList[i].Length - (index + 1));




                    //format = "yyyy-MM-dd HH:mm:ss";
                    //value = DateTime.ParseExact(dateString, format, provider);
                    //value = DateTime.ParseExact(dateString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
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

        public void CreateFile(string dst)
        {
            outputPath = dst;
            try
            {
                //sourceFileArr = File.ReadAllLines(logPath);
                //File.AppendAllLines(outputPath, sourceFileArr);
                File.AppendAllLines(outputPath, this.GetIpList(this.sourceFileArr));
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
