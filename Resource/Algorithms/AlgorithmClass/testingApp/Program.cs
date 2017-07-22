
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
            String allText = System.IO.File.ReadAllText(@"../../TextFile1.txt");
            string[][] logInCollection = PredictionModel.readFromFile(allText);
            double testTime = 15;
            double testDay = 3;
            PredictionModel predictModel = new PredictionModel(testTime, testDay, logInCollection);
            string riskLevel = predictModel.logInRisk;
            string output = predictModel.logInOutput;
            Console.WriteLine(output);
            Console.WriteLine("The risk level is " + riskLevel);

           
            

        }

        
    }
    
}
