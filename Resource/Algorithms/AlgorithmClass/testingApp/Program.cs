
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

            //string currentPublicIP = PredictionModel.getCurrentPublicIP();
            //string currentPublicLocation = PredictionModel.getCurrentPublicIPLocation(currentPublicIP);
            //Console.WriteLine(currentPublicIP + " is at " + currentPublicLocation);



            //string[] ipAddressCollection =
            //{
            //    "131.23.244.105" ,
            //    "131.23.244.105" ,
            //    "147.120.34.99",
            //    "131.23.244.105" ,
            //    "131.23.244.105" ,
            //    "147.120.34.99",
            //    "131.23.244.105" ,
            //    "116.86.120.223"
            //};


            //Dictionary<string, int> count = new Dictionary<string, int>();

            //foreach (string element in ipAddressCollection)
            //{
            //    if (count.ContainsKey(element))
            //    {
            //        count[element]++;
            //    }
            //    else
            //    {
            //        count[element] = 1;
            //    }
            //}

            //foreach (var pair in count)
            //    Console.WriteLine("Value {0} occurred {1} times.", pair.Key, pair.Value);


            ////Removes any duplicate IP addresses to get the Unique IP addresses//
            //var noDupsList = new HashSet<string>(ipAddressCollection).ToList();
            //Dictionary<int, string> myDictionary = new Dictionary<int, string>();
            ////Creates a link between a int value to a certain ip address using dictionary method 
            //for (int i = 0; i < noDupsList.Count(); i++)
            //{
            //    myDictionary.Add(i, noDupsList[i]);

            //}

            //string[][] passingList = new string[myDictionary.Count][];
            //int counter = 0;
            //foreach (KeyValuePair<int, string> entry in myDictionary)
            //{

            //    foreach (KeyValuePair<string, int> totCount in count)
            //    {
            //        if (entry.Value == totCount.Key)
            //        {
            //            Console.WriteLine("The IP address " + entry.Value + " has appeared " + totCount.Value + " times and has a index of " + entry.Key);
            //            string[] newArray = new string[] { Convert.ToString(entry.Key), Convert.ToString(totCount.Value) };
            //            passingList[counter] = newArray;

            //        }
            //    }
            //    counter++;
            //}

            //for (int i = 0; i < passingList.Length; i++)
            //{
            //    Console.WriteLine(passingList[i][0] + " " + passingList[i][1]);
            //}

            List<PhysicalAddress> macAddress = new List<PhysicalAddress>();
            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();

            }
            //Console.WriteLine(localIP);
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces
                if(nic.OperationalStatus == OperationalStatus.Up)
                { 
                    //Console.WriteLine(nic.NetworkInterfaceType.ToString());
                    var test = nic.GetIPProperties();
                    if(test.UnicastAddresses.Count != 0 )
                    {
                        var testing = test.UnicastAddresses[1];
                       
                        if (testing.Address.ToString() == localIP)
                        {
                            macAddress.Add(nic.GetPhysicalAddress());
                        }
                    }
                }

            }
         
            foreach (var element in macAddress)
            {
                Console.WriteLine(element);
            }

        }


    }
}
