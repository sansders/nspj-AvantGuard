using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
    /// Interaction logic for ForgotPassword3.xaml
    /// </summary>
    public partial class ForgotPassword3 : Page
    {
        public ForgotPassword3()
        {
            InitializeComponent();

        }

        private void ForgotPassword3NextButton_Click(object sender, RoutedEventArgs e)
        {
            string selected_ForgotPasswordCode = (App.Current as App).ForgotPasswordCode;
            if (ForgotPasswordCodeTextBox.Text == selected_ForgotPasswordCode)
            {
                MessageBox.Show("Correct!");
                this.NavigationService.Navigate(new Uri(@"ForgotPassword4.xaml", UriKind.RelativeOrAbsolute));
            }
            else
            {
                MessageBox.Show("Invalid code!");
            }
        }

        private void ForgotPassword3BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"ForgotPassword2.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
