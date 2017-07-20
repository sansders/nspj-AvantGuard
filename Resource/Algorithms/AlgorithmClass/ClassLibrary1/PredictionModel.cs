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
        public static string startLogInPrediction()
        {
            string fileURL = "../../../../testing/Prediction.py";
            string python = "../../../../../../../../Anaconda/python.exe";
            string output = startPythonProgramm(fileURL , python);
            return output;
        }

        private static string startPythonProgramm(string fileURL , string python)
        {
            //string fileName = @"../../../../testing/Prediction.py";
            string fileName = @fileURL;
            string pythonValue = @python;

            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(@python, fileName)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            p.Start();

            string output = p.StandardOutput.ReadToEnd();
            return output;
        }
    }
}
