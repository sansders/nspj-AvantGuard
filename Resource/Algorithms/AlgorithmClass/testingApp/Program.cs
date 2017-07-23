
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

namespace testingApp
{
    class Program
    {
       

        static void Main(string[] args)
        {
            //String allText = System.IO.File.ReadAllText(@"../../TextFile1.txt");
            //string[][] logInCollection = PredictionModel.readFromFile(allText);
            //double testTime = 15;
            //double testDay = 3;
            //PredictionModel predictModel = new PredictionModel(testTime, testDay, logInCollection);
            //string riskLevel = predictModel.logInRisk;
            //string output = predictModel.logInOutput;
            //Console.WriteLine(output);
            //Console.WriteLine("The risk level is " + riskLevel);

            //string currentPublicIP = PredictionModel.getCurrentPublicIP();
            //string currentPublicLocation = PredictionModel.getCurrentPublicIPLocation(currentPublicIP);
            //Console.WriteLine(currentPublicIP + " is at " + currentPublicLocation);

           

            string[] ipAddressCollection =
            {
                "131.23.244.105" ,
                "131.23.244.105" ,
                "131.23.244.105" ,
                "131.23.244.105" ,
                "131.23.244.105" ,
                "116.86.120.223"
            };
            
            //Removes any duplicate IP addresses to get the Unique IP addresses//
            var noDupsList = new HashSet<string>(ipAddressCollection).ToList();
            Dictionary<int, string> myDictionary = new Dictionary<int, string>();
            //Creates a link between a int value to a certain ip address using dictionary method 
            for (int i = 0; i < noDupsList.Count(); i++)
            {
                myDictionary.Add(i, noDupsList[i]);
            }


            //string output = "lala";
            //myDictionary.TryGetValue(1, out output);

            foreach (KeyValuePair<int, string> entry in myDictionary)
            {
                Console.WriteLine(entry.Value);
            }
        }

        
    }
    
}
