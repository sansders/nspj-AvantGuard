using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for ForgotPassword1.xaml
    /// </summary>
    public partial class ForgotPassword1 : Page
    {
        public ForgotPassword1()
        {
            InitializeComponent();
        }

        private void ForgotPassword1BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"LoginPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ForgotPassword1NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (ForgotPasswordEmailTextBox.Text.Length == 0)
            {
                MessageBox.Show("Enter an email.");
                ForgotPasswordEmailTextBox.Focus();
                ForgotPassword1Image.Visibility = Visibility.Visible;
            }
            else if (!Regex.IsMatch(ForgotPasswordEmailTextBox.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                MessageBox.Show("Please enter a valid email.");
                ForgotPasswordEmailTextBox.Select(0, ForgotPasswordEmailTextBox.Text.Length);
                ForgotPasswordEmailTextBox.Focus();
                ForgotPassword1Image.Visibility = Visibility.Visible;
            }

            else
            {
                ForgotPassword1Image.Visibility = Visibility.Hidden;

                this.NavigationService.Navigate(new Uri(@"ForgotPassword2.xaml", UriKind.RelativeOrAbsolute));
            }
        }
    }
}
