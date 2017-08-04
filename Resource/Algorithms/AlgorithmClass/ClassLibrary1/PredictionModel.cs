using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmLibary
{
    public class PredictionModel
    {
        private string _logInRisk, _logInOutput;
        private string _ipRisk, _ipOutput;
        public PredictionModel(double logInTime, double logInDay, string[][] logInCollection)
        {
           
            startLogInPrediction(logInTime, logInDay, logInCollection);
        }

        public PredictionModel(string[][] ipAddressCollection, string[] query)
        {
            startIPMACPrediction(ipAddressCollection, query);
        }

        public string logInRisk
        {
            get { return _logInRisk; }
            set { _logInRisk = value; }
        }

        public string logInOutput
        {
            get { return _logInOutput; }
            set { _logInOutput = value; }
        }

        public string ipRisk
        {
            get { return _ipRisk; }
            set { _ipRisk = value; }
        }

        public string ipOutput
        {
            get { return _ipOutput; }
            set { _ipOutput = value; }
        }


        public void startLogInPrediction(double logInTime, double logInDay , string[][] logInCollection)
        {

            string fileURL = "../../../../Resource/Algorithms/testing/Prediction.py";
          
            string python = "../../../../../Anaconda/python.exe";

            
            string logInTimeConverted = Convert.ToString(logInTime);
            string logInDayConverted = Convert.ToString(logInDay);
            string dataList = null;

            
            int start = 0;

            ;
          
            if (logInCollection.Length >= 100)
            {
                start = logInCollection.Length - 100; 
            }
            for (int i = start; i < logInCollection.Length; i++)
            {
                dataList = dataList + " " + String.Join(" ", logInCollection[i][0], logInCollection[i][1]);
            }
            string[] argsSet = { fileURL, logInTimeConverted, logInDayConverted , dataList};
               
            string output = startPythonProgramm(python, argsSet);
         
            Console.WriteLine(output);
            _logInRisk = PredictionModel.getRiskLevel(output);
            _logInOutput = PredictionModel.getOutput(output);
            




        }

        public void startIPMACPrediction(string[][] ipAddressCollection, string[] query)
        {
            try { 
            string[][] passList = convertIPPredictionData(ipAddressCollection, query);
            string fileURL = @"../../../../Resource/Algorithms/testing/IPPrediction.py";
            string python = @"../../../../../Anaconda/python.exe";
            Console.WriteLine("Is this working");
            query = checkQueryData(query, ipAddressCollection);
            string queryIP = query[0];
            string queryMAC = query[1];
            string queryDAY = query[2];
            string queryCombined = null;
            int start = 0;
            string datalist = null;
            
            if (passList.Count() >= 100)
            {
                start = passList.Count() - 100;
            }
            for (int i = start; i < passList.Count(); i++)
            {
                datalist = datalist + " " + String.Join(" ", passList[i][0], passList[i][1], passList[i][2], passList[i][3]);
            }
            Console.WriteLine(datalist);
            string[] args = { fileURL, queryIP, queryMAC, queryDAY, datalist };
         
            args[0] = fileURL;
            string output = startPythonProgramm(python, args);
            Console.WriteLine("end");            
            string[] result = output.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
           
            Console.WriteLine(output);
            this.ipRisk = result[result.Length - 2];
            this.ipOutput = result[result.Length - 1];
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }



        }

        private static string[][] convertIPPredictionData(string[][] ipAddressCollection, string[] query)
        {
          
            string[][] keyData = getValueArray(ipAddressCollection);
         
            string[] queryKey = checkQueryData(query, ipAddressCollection);
            Dictionary<string, int> count = getCountNumber(ipAddressCollection);
            string[] retrieveValue = new string[] { queryKey[0], queryKey[1], queryKey[2] };
            string[] retrievedDataValue = getKeyInformation(queryKey, ipAddressCollection);

            int counter = 0;
            string[][] testingList = new string[keyData.Count()][];
            string[][] passList = new string[keyData.Count()][];
            foreach (var element in keyData)
            {
                passList[counter] = new string[] { element[0], element[1], element[2], Convert.ToString(count.ElementAt(counter).Value) };
                string[] question = new string[] { element[0], element[1], element[2] };
                string[] data = getKeyInformation(question, ipAddressCollection);
                testingList[counter] = new string[] { data[0], data[1], data[2], Convert.ToString(count.ElementAt(counter).Value) };
                counter++;
            }

            foreach(var element in testingList)
            {
                Console.WriteLine(Convert.ToString(element[0]) + Convert.ToString(element[1]) + Convert.ToString(element[2]));
            }


            return passList;
        }

        private static string startPythonProgramm(string python , string [] argsSet)
        {
            //string fileName = @"../../../../testing/Prediction.py";
            string fileName = @argsSet[0];
            string pythonValue = @python;

            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(@python)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = String.Join(" ", argsSet)
            };
            p.Start();

            string output = p.StandardOutput.ReadToEnd();
            //string output = p.StandardError.ReadToEnd();
            return output;


        }


        private static string getRiskLevel(string input)
        {
            string[] outputData = input.Split('/');
            string riskLevel = outputData[0];
            return riskLevel;
            //return input;
        }

        private static string getOutput(string input)
        {
            string[] outputData = input.Split('/');
            string output = outputData[1];
            return output;
            //return input;
        }

        public static string[][] readFromFile(String allText)
        {
            
            //Console.WriteLine(allText);
            int i = 0;
            String[] all = allText.Split('/');
            List<String> time = new List<string>();
            List<String> date = new List<string>();
            foreach (String element in all)
            {
                if (i == 0)
                {
                    time.Add(element);
                }
                else if (i == 1)
                {
                    date.Add(element);
                }
                i++;
            }

            String[] indTime = null;
            String[] indDay = null;
            foreach (String timeInd in time)
            {
                indTime = timeInd.Split(',');

            }
            foreach (String DateInd in date)
            {
                indDay = DateInd.Split(',');
            }

            List<string[]> myList = new List<string[]>();
            for (int a = 0; a < indTime.Length; a++)
            {
                string first = Convert.ToString(indTime[a]);
                string second = Convert.ToString(indDay[a]);
                string[] myNew = { first, second };
                myList.Add(myNew);
            }
            Console.WriteLine(myList[0].Count());
            Console.WriteLine(myList[1].Count());
            string[][] finalList = new string[myList.Count()][];
            for (int a = 0; a < myList.Count(); a++)
            {
                for (int b = 0; b < myList[a].Count(); b++)
                {
                    finalList[a] = myList[a];
                }

            }
            return finalList;
        }












        public static string getCurrentPublicIP()
        {
            Console.WriteLine("Getting Public IP Now...");
            //string url = "http://checkip.dyndns.org";
            //WebRequest req = WebRequest.Create(url);
            //WebResponse resp = req.GetResponse();
            //StreamReader sr = new StreamReader(resp.GetResponseStream());
            //string response = sr.ReadToEnd().Trim();
            //string[] a = response.Split(':');
            //string a2 = a[1].Substring(1);
            //string[] a3 = a2.Split('<');
            //string a4 = a3[0];

            //string ip = new WebClient().DownloadString(@"http://icanhazip.com").Trim();
            //Console.WriteLine(ip);

            //IPHostEntry IPHost = Dns.GetHostByName(Dns.GetHostName());
            //string ip = IPHost.AddressList[0].ToString();
            //Console.WriteLine(ip);
            return "10.10.10.1";
        }

        public static string getCurrentPublicIPLocation(string a4)
        {
            Console.WriteLine("Getting Public IP Location Now...");
            var strFile = "hello";
            using (var objClient = new System.Net.WebClient())
            {
                strFile = objClient.DownloadString("http://freegeoip.net/xml/" + a4);
            }

            StringReader strReader = new StringReader(strFile);
            string response = strReader.ReadToEnd().Trim();
            string[] a = response.Split('<');
            string countryNoFilter = " ";
           
            for(int i = 0; i < a.Length; i++)
            {
                if (a[i].Contains("CountryName>"))
                {
                    countryNoFilter = a[i];
                    break;
                }
            }
            string country = countryNoFilter.Remove(0, 12);
            return country;
        }

        private static string getCurrentPrivateIP()
        {
            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();

            }
            return localIP;
        }


        public static string getCurrentMAC()
        {
            string localIP = getCurrentPrivateIP();
            Console.WriteLine(localIP);
            string macAdd = null;
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    //Console.WriteLine(nic.NetworkInterfaceType.ToString());
                    var test = nic.GetIPProperties();
                    if (test.UnicastAddresses.Count != 0)
                    {
                        var testing = test.UnicastAddresses[1];

                        if (testing.Address.ToString() == localIP)
                        {
                            macAdd = nic.GetPhysicalAddress().ToString();
                        }
                    }
                }
            }
            Console.WriteLine(macAdd);
            return macAdd;
        }

        public static string getCurrentDate()
        {

            Console.WriteLine("Getting Current Date");
            int d = (int)DateTime.Now.DayOfWeek;
            if (d == 0)
            {
                d = 7;
            }
            return d.ToString();
        }

        private static string[] getKeyInformation(string[] retrieveKey, string[][] ipAddressCollection)
        {
            Dictionary<string, int> count = new Dictionary<string, int>();
            for (int i = 0; i < ipAddressCollection.Length; i++)
            {
                string concat = ipAddressCollection[i][0] + "/" + ipAddressCollection[i][1] + "/" + ipAddressCollection[i][2];
                if (count.ContainsKey(concat))
                {
                    count[concat]++;
                }
                else
                {
                    count[concat] = 1;
                }
            }

            List<String> IPlist = new List<string>();
            List<String> MAClist = new List<string>();
            List<String> DAYlist = new List<string>();
            foreach (string element in count.Keys)
            {
                String[] myElement = element.Split('/');
                IPlist.Add(myElement[0]);
                MAClist.Add(myElement[1]);
                DAYlist.Add(myElement[2]);
            }

            var noIPDupsList = removeDuplicate(IPlist);
            var noMACDupsList = removeDuplicate(MAClist);

            Dictionary<int, string> IP1Dictionary = linkData(noIPDupsList);
            Dictionary<int, string> MAC2Dictionary = linkData(noMACDupsList);

            int counter = 0;
            int ipkey = Convert.ToInt32(retrieveKey[0]);
            int mackey = Convert.ToInt32(retrieveKey[1]);

            string ipValue = IP1Dictionary.FirstOrDefault(x => x.Key == ipkey).Value;
            string macValue = MAC2Dictionary.FirstOrDefault(x => x.Key == mackey).Value;
            string day = retrieveKey[2];
            string value = checkCountForValue(ipValue, macValue, day, count);

            string[] data = new string[] { ipValue, macValue, day, Convert.ToString(value) };
            return data;

        }

        private static Dictionary<string, int> getCountNumber(string[][] ipAddressCollection)
        {
            Dictionary<string, int> count = new Dictionary<string, int>();
            for (int i = 0; i < ipAddressCollection.Length; i++)
            {
                string concat = ipAddressCollection[i][0] + "/" + ipAddressCollection[i][1] + "/" + ipAddressCollection[i][2];
                if (count.ContainsKey(concat))
                {
                    count[concat]++;
                }
                else
                {
                    count[concat] = 1;
                }
            }
            return count;
        }

        private static string[] checkQueryData(string[] query, string[][] ipAddressCollection)
        {
            Dictionary<string, int> count = new Dictionary<string, int>();
            for (int i = 0; i < ipAddressCollection.Length; i++)
            {
                string concat = ipAddressCollection[i][0] + "/" + ipAddressCollection[i][1] + "/" + ipAddressCollection[i][2];
                if (count.ContainsKey(concat))
                {
                    count[concat]++;
                }
                else
                {
                    count[concat] = 1;
                }
            }

            List<String> IPlist = new List<string>();
            List<String> MAClist = new List<string>();
            List<String> DAYlist = new List<string>();
            foreach (string element in count.Keys)
            {
                String[] myElement = element.Split('/');
                IPlist.Add(myElement[0]);
                MAClist.Add(myElement[1]);
                DAYlist.Add(myElement[2]);
            }

            var noIPDupsList = removeDuplicate(IPlist);
            var noMACDupsList = removeDuplicate(MAClist);

            Dictionary<int, string> IP1Dictionary = linkData(noIPDupsList);
            Dictionary<int, string> MAC2Dictionary = linkData(noMACDupsList);

            int counter = 0;

            int ipkey = -1,
                mackey = -1;
            string daykey = query[2];

            if (IP1Dictionary.ContainsValue(query[0]))
            {
                ipkey = IP1Dictionary.FirstOrDefault(x => x.Value == query[0]).Key;
            }

            if (MAC2Dictionary.ContainsValue(query[1]))
            {
                mackey = MAC2Dictionary.FirstOrDefault(x => x.Value == query[1]).Key;

            }

            string ipValue = IP1Dictionary.FirstOrDefault(x => x.Key == ipkey).Value;

            string macValue = MAC2Dictionary.FirstOrDefault(x => x.Key == mackey).Value;
            string value = checkCountForValue(ipValue, macValue, daykey, count);

            string[] data = new string[] { Convert.ToString(ipkey), Convert.ToString(mackey), daykey, value };
            return data;

        }

        private static string[][] getValueArray(string[][] ipAddressCollection)
        {
            string[][] data = null;
            try { 
            Dictionary<string, int> count = new Dictionary<string, int>();
            for (int i = 0; i < ipAddressCollection.Length; i++)
            {
                string concat = ipAddressCollection[i][0] + "/" + ipAddressCollection[i][1] + "/" + ipAddressCollection[i][2];
                if (count.ContainsKey(concat))
                {
                    count[concat]++;
                }
                else
                {
                    count[concat] = 1;
                }
            }
           
            //foreach (var pair in count)
            //    Console.WriteLine("Value {0} occurred {1} times.", pair.Key, pair.Value);

            //Removes any duplicate IP addresses/MAC address pair to get the Unique IP/ MAC address pair //
            List<String> IPlist = new List<string>();
            List<String> MAClist = new List<string>();
            List<String> DAYlist = new List<string>();
            foreach (string element in count.Keys)
            {
                String[] myElement = element.Split('/');
                IPlist.Add(myElement[0]);
                MAClist.Add(myElement[1]);
                DAYlist.Add(myElement[2]);
            }

            var noIPDupsList = removeDuplicate(IPlist);
            var noMACDupsList = removeDuplicate(MAClist);
            
            Dictionary<int, string> IP1Dictionary = linkData(noIPDupsList);
            Dictionary<int, string> MAC2Dictionary = linkData(noMACDupsList);
            data = new string[count.Count][];
            int counter = 0;
            foreach (string element in count.Keys)
            {
                int ipkey = -1,
                    mackey = -1;
                string daykey = DAYlist[counter];
                String[] myElement = element.Split('/');
                if (IP1Dictionary.ContainsValue(myElement[0]))
                {
                    ipkey = IP1Dictionary.FirstOrDefault(x => x.Value == myElement[0]).Key;

                }

                if (MAC2Dictionary.ContainsValue(myElement[1]))
                {
                    mackey = MAC2Dictionary.FirstOrDefault(x => x.Value == myElement[1]).Key;

                }
                data[counter] = new string[] { Convert.ToString(ipkey), Convert.ToString(mackey), daykey, Convert.ToString(count.ElementAt(counter).Value) };
                counter++;
            }
               
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return data;

        }

        private static List<String> removeDuplicate(List<string> dataset)
        {
            var hashSet = new HashSet<string>(dataset).ToList();
            return hashSet;
        }

        private static Dictionary<int, string> linkData(List<string> dataset)
        {
            Dictionary<int, string> linkDictionary = new Dictionary<int, string>();
            int counter = 0;
            foreach (var element in dataset)
            {
                linkDictionary.Add(counter, element);
                counter++;
            }
            return linkDictionary;
        }

        private static string checkCountForValue(string ipValue, string macValue, string day, Dictionary<string, int> count)
        {
            string check = ipValue + "/" + macValue + "/" + day;
            int value = 0;
            //Console.WriteLine(check);
            if (count.ContainsKey(check))
            {
                value = count.FirstOrDefault(x => x.Key == check).Value;
            }
            else
            {
                value = 1;
            }
            return Convert.ToString(value);
        }


    }
}
