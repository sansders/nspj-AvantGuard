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
        String value1;
        public page1viewmodel()
        {
            value1 = "Hello";
        }

        public String showOption
        {
            get { return "TEds"; }
            set
            {
                value1 = value;
            }
              

            //var radioButton = sender as RadioButton;
            //if (radioButton == null)
            //{

            //    CurrentPageModel.firstValidation = false;
            //}
            //else
            //{
            //    CurrentPageModel.firstValidation = true;
            //    String data = radioButton.Content as String;
            //    Console.WriteLine(data);
            //}
        }

    }
}
