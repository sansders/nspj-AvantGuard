using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NSPJProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public string UserID { get; internal set; }
        public string UserPassword { get; internal set; }
        public string UserName { get; internal set; }
        public string UserEmail { get; internal set; }
        public string UserContact { get; internal set; }
        public string UserDOB { get; internal set; }
        public string SecurityQ1 { get; internal set; }
        public string Q1Ans { get; internal set; }
        public string SecurityQ2 { get; internal set; }
        public string Q2Ans { get; internal set; }
    }
}
