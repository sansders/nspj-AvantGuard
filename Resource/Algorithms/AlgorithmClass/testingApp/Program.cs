
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System.Diagnostics;
using System.IO;
using AlgorithmLibary;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;

namespace testingApp
{
    class Program
    {


        static void Main(string[] args)
        {
            //String allText = System.IO.File.ReadAllText(@"../../TextFile2.txt");
            //string[][] logInCollection = PredictionModel.readFromFile(allText);
            //double testTime = 13;
            //double testDay = 1;
            //PredictionModel predictModel = new PredictionModel(testTime, testDay, logInCollection);
            //string riskLevel = predictModel.logInRisk;
            //string output = predictModel.logInOutput;

            //Console.WriteLine(output);
            //Console.WriteLine("The risk level is " + riskLevel);

            string currentPublicIP = PredictionModel.getCurrentPublicIP();
            string currentPublicLocation = PredictionModel.getCurrentPublicIPLocation(currentPublicIP);
            Console.WriteLine(currentPublicIP + " is at " + currentPublicLocation);

            string localIP = getCurrentPrivateIP();
            string macAddress = getCurrentMAC(localIP);
            Console.WriteLine(macAddress);


            string[][] ipAddressCollection =
            {
                new string [] {"131.23.244.105","C00008"} ,
                new string [] {"131.23.244.105", "C00008"} ,
                new string [] { "147.120.34.99", "C00008"} ,
                new string [] { "131.23.244.105", "D00008"},
                new string [] { localIP , "D8000"}
            };

            string[] query = new string[] { localIP, macAddress };
            Dictionary<string, int> count = getCountNumber(ipAddressCollection);
            string[][] keyData = getValueArray(ipAddressCollection);
            string[] queryKey = checkQueryData(query, ipAddressCollection);
            
            Console.WriteLine(queryKey[0] + " " +  queryKey[1]);
            
            foreach(var element in count)
            {
                Console.WriteLine(element.Key + " = " + element.Value);
            }

            foreach(var element in keyData)
            {
                Console.WriteLine(element[0] + element[1]);
            }
            
        }

        public static Dictionary<string, int> getCountNumber(string[][] ipAddressCollection)
        {
            Dictionary<string, int> count = new Dictionary<string, int>();
            for (int i = 0; i < ipAddressCollection.Length; i++)
            {
                string concat = ipAddressCollection[i][0] + "/" + ipAddressCollection[i][1];
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

        public static string[] checkQueryData(string[] query, string[][] ipAddressCollection)
        {
            Dictionary<string, int> count = new Dictionary<string, int>();
            for (int i = 0; i < ipAddressCollection.Length; i++)
            {
                string concat = ipAddressCollection[i][0] + "/" + ipAddressCollection[i][1];
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
            var noDupsList = new HashSet<string>(count.Keys).ToList();

            String[] filteredIPList = new String[noDupsList.Count];
            String[] filteredMACList = new String[noDupsList.Count];
            int counter = 0;
            foreach (string element in noDupsList)
            {
                String[] myElement = element.Split('/');
                filteredIPList[counter] = myElement[0];
                filteredMACList[counter] = myElement[1];
                counter++;
            }

            Dictionary<int, string> IPDictionary = new Dictionary<int, string>();
            Dictionary<int, string> MACDictionary = new Dictionary<int, string>();

            for (int i = 0; i < filteredIPList.Count(); i++)
            {
                IPDictionary.Add(i, filteredIPList[i]);
                MACDictionary.Add(i, filteredMACList[i]);
            }

            string[] returnKey = null;  
            for(int i = 0; i < IPDictionary.Count; i++)
            {
                string checkIP = IPDictionary.ElementAt(i).Value;
                string checkMAC = MACDictionary.ElementAt(i).Value;
                int IPkey = -1;
                int MACkey = -1;
                if(query[0] == checkIP && query[1] == checkMAC)
                {
                    IPkey = IPDictionary.ElementAt(i).Key;
                    MACkey = MACDictionary.ElementAt(i).Key;
                }
                if(query[0] == checkIP)
                {
                    IPkey = IPDictionary.ElementAt(i).Key;
                }
                if(query[1] == checkMAC)
                {
                    MACkey = MACDictionary.ElementAt(i).Key;
                }

                returnKey = new string[] { Convert.ToString(IPkey), Convert.ToString(MACkey) };
            }
            return returnKey;
        }

        public static string[][] getValueArray(string[][] ipAddressCollection)
        {
            Dictionary<string, int> count = new Dictionary<string, int>();
            for (int i = 0; i < ipAddressCollection.Length; i++)
            {
                string concat = ipAddressCollection[i][0] + "/" + ipAddressCollection[i][1];
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
            var noDupsList = new HashSet<string>(count.Keys).ToList();

            String[] filteredIPList = new String[noDupsList.Count];
            String[] filteredMACList = new String[noDupsList.Count];
            int counter = 0;
            foreach (string element in noDupsList)
            {
                String[] myElement = element.Split('/');
                filteredIPList[counter] = myElement[0];
                filteredMACList[counter] = myElement[1];
                counter++;
            }

            Dictionary<int, string> IPDictionary = new Dictionary<int, string>();
            Dictionary<int, string> MACDictionary = new Dictionary<int, string>();

            for (int i = 0; i < filteredIPList.Count(); i++)
            {
                IPDictionary.Add(i, filteredIPList[i]);
                MACDictionary.Add(i, filteredMACList[i]);
            }

            string[][] data = new string[IPDictionary.Count()][];
            foreach (var element in count)
            {
                for (int i = 0; i < IPDictionary.Count; i++)
                {
                    string IP = IPDictionary.ElementAt(i).Value;
                    string MAC = MACDictionary.ElementAt(i).Value;
                    string concat = IP + "/" + MAC;
                    if (concat.Equals(element.Key))
                    {
                        //Console.WriteLine("The IP Address of " + IP + " with the mac address " + MAC + " has appeared " + element.Value);
                        data[i] = new string[] { Convert.ToString(IPDictionary.ElementAt(i).Key), Convert.ToString(MACDictionary.ElementAt(i).Key) };
                    }
                }
            }

            return data;
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

        public static string getCurrentMAC(string localIP)
        {
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
            return macAdd;
        }


    }
}
