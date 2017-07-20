
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
            //ScriptEngine engine = Python.CreateEngine();
            //var paths = engine.GetSearchPaths();
            //paths.Add(@"../../../../Anaconda/Lib/site-packages/pandas");
            //engine.SetSearchPaths(paths);
            //engine.ExecuteFile(@"../../../../testing/Prediction.py");



            //string fileName = @"../../../../testing/Prediction.py";

            //Process p = new Process();
            //p.StartInfo = new ProcessStartInfo(@"../../../../Anaconda/python.exe", fileName)
            //{
            //    RedirectStandardOutput = true,
            //    UseShellExecute = false,
            //    CreateNoWindow = true
            //};
            //p.Start();

            //string output = p.StandardOutput.ReadToEnd();
            //p.WaitForExit();


            string output = PredictionModel.startLogInPrediction();
            Console.WriteLine(output);

            
        }
    }
    
}
