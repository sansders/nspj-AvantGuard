using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NSPJProject
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        public static string GetSha512FromString(string strData)
        {
            //strData.Insert(0, "XYZ");
            var message = Encoding.ASCII.GetBytes(strData.Insert(2, "026620758babadb008ee7b98e1bb07351f08d49228c15f6f31c4ee75cb9a26f5079b81c01f14f78cf5f9639e49d7319ee3c3fcc1f94e686b8d605c93f2ab9fb4"));
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";


            byte[] currentHash = hashString.ComputeHash(message);
            for (int i = 0; i < 100; i++)
            {
                if (i != 0)
                {
                    //var message = Encoding.ASCII.GetBytes(strData.Insert(0, "026620758babadb008ee7b98e1bb07351f08d49228c15f6f31c4ee75cb9a26f5079b81c01f14f78cf5f9639e49d7319ee3c3fcc1f94e686b8d605c93f2ab9fb4"));
                    //byte[] currentHash = hashString.ComputeHash(message);

                    currentHash = hashString.ComputeHash(currentHash);
                    Console.WriteLine(i + "round");
                }
            }


            foreach (byte x in currentHash)
            {
                hex += String.Format("{0:x2}", x);
            }


            return hex;
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            PasswordTextBox.Password = GetSha512FromString(PasswordTextBox.Password);
            MessageBox.Show(GetSha512FromString(PasswordTextBox.Password));
        }

        private void ButtonSignUp_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"SignUp1.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ButtonForgetPass_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"ForgotPassword1.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
