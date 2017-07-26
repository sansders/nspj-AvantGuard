﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmLibary
{
    public class PredictionModel
    {
        private string _logInRisk, _logInOutput;
        public PredictionModel(double logInTime, double logInDay , string[][] logInCollection)
        {
            startLogInPrediction(logInTime, logInDay, logInCollection);
        }

        public string logInRisk
        {
            get { return _logInRisk; }
            set { _logInRisk = value; }
        }

        public string logInOutput
        {
            get { return _logInOutput;}
            set { _logInOutput = value; }
        }

        public void startLogInPrediction(double logInTime, double logInDay , string[][] logInCollection)
        {
            string fileURL = "../../../../testing/Prediction.py";
            string python = "../../../../../../../../Anaconda/python.exe";
            string logInTimeConverted = Convert.ToString(logInTime);
            string logInDayConverted = Convert.ToString(logInDay);
            string dataList = null;
            int start = 0; 
            if (logInCollection.Length > 100)
            {
                start = logInCollection.Length - 100; 
            }
            for(int i = start; i < logInCollection.Length; i++)
            {
                dataList = dataList + " " + String.Join(" ", logInCollection[i][0], logInCollection[i][1]);
            }
            string[] argsSet = { fileURL, logInTimeConverted, logInDayConverted , dataList};
            string output = startPythonProgramm(python, argsSet);
            _logInRisk = PredictionModel.getRiskLevel(output);
            _logInOutput = PredictionModel.getOutput(output);

           
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
            string url = "http://checkip.dyndns.org";
            WebRequest req = WebRequest.Create(url);
            WebResponse resp = req.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string response = sr.ReadToEnd().Trim();
            string[] a = response.Split(':');
            string a2 = a[1].Substring(1);
            string[] a3 = a2.Split('<');
            string a4 = a3[0];
            return a4;
           
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

    }
}
