using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmLibary
{
    public class PredictionModel
    {
        public static string startLogInPrediction(double logInTime, double logInDay)
        {
            string fileURL = "../../../../testing/Prediction.py";
            string python = "../../../../../../../../Anaconda/python.exe";
            string logInTimeConverted = Convert.ToString(logInTime);
            string logInDayConverted = Convert.ToString(logInDay);
            string[] argsSet = { fileURL, logInTimeConverted, logInDayConverted };
            string output = startPythonProgramm(python, argsSet);
            return output;
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
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = String.Join(" ", argsSet)
            };
            p.Start();

            string output = p.StandardOutput.ReadToEnd();
            return output;


        }
    }
}
