using NSPJProject.Model1;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for SignUp2.xaml
    /// </summary>
    public partial class SignUp2 : Page
    {
        public SignUp2()
        {
            InitializeComponent();
        }

        private void SignUp2NextButton_Click(object sender, RoutedEventArgs e)
        {



            if (String.IsNullOrEmpty(SecurityQuestion1TextBox.Text) || String.IsNullOrEmpty(SecurityQuestion2TextBox.Text)
                || String.IsNullOrEmpty(Answer1TextBox.Text) || String.IsNullOrEmpty(Answer2TextBox.Text))
            {

                MessageBox.Show("Please fill in all blanks!");

                if (String.IsNullOrEmpty(SecurityQuestion1TextBox.Text))
                {
                    Q1Image.Visibility = Visibility.Visible;
                }

                else
                {
                    Q1Image.Visibility = Visibility.Hidden;
                }

                if (String.IsNullOrEmpty(SecurityQuestion2TextBox.Text))
                {
                    Q2Image.Visibility = Visibility.Visible;
                }

                else
                {
                    Q2Image.Visibility = Visibility.Hidden;
                }

                if (String.IsNullOrEmpty(Answer1TextBox.Text))
                {
                    A1Image.Visibility = Visibility.Visible;
                }

                else
                {
                    A1Image.Visibility = Visibility.Hidden;
                }

                if (String.IsNullOrEmpty(Answer2TextBox.Text))
                {
                    A2Image.Visibility = Visibility.Visible;
                }

                else
                {
                    A2Image.Visibility = Visibility.Hidden;
                }
            }

            else
            {
                MessageBox.Show("You're almost done. An email has been sent to your email address. Please verify your email address, thank you!");
                this.NavigationService.Navigate(new Uri(@"SignUp3.xaml", UriKind.RelativeOrAbsolute));
            }

        }

        private void SignUp2BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"LoginPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
