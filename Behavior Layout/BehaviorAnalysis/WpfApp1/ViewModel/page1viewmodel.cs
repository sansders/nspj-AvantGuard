using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Model1;
using System.ComponentModel;

namespace WpfApp1.ViewModel
{
    class page1viewmodel
    {
        public page1viewmodel()
        {

        }

        private String ToggleCheckOption
        {
            get
            {
                String hello = "123";
                return hello;
            }
            set
            {
                MessageBox.Show("Testing");

                //var radiobutton = sender as radiobutton;
                //if (radiobutton == null)
                //{
                //    currentpagemodel.firstvalidation = false;
                //}
                //else
                //{
                //    currentpagemodel.firstvalidation = true;
                //    string data = radiobutton.content as string;
                //    console.writeline(data);
                //}
            }

        }
    }
}
