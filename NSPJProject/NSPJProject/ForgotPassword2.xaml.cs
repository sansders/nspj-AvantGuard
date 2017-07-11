using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for ForgotPassword2.xaml
    /// </summary>
    public partial class ForgotPassword2 : Page
    {
        public ForgotPassword2()
        {
            InitializeComponent();
        }

        private void ForgotPassword2BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"LoginPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ForgotPassword2NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(SecurityQ1Ans.Text) || String.IsNullOrEmpty(SecurityQ2Ans.Text))
            {
                MessageBox.Show("Please fill in all blanks!");

                if (String.IsNullOrEmpty(SecurityQ1Ans.Text))
                {
                    ForgotPassword2Image1.Visibility = Visibility.Visible;
                }

                else
                {
                    ForgotPassword2Image1.Visibility = Visibility.Hidden;
                }

                if (String.IsNullOrEmpty(SecurityQ2Ans.Text))
                {
                    ForgotPassword2Image2.Visibility = Visibility.Visible;
                }

                else
                {
                    ForgotPassword2Image2.Visibility = Visibility.Hidden;
                }
            }

            else
            {
                ForgotPassword2Image1.Visibility = Visibility.Hidden;
                ForgotPassword2Image2.Visibility = Visibility.Hidden;
                MessageBox.Show("(Still Incomplete.");
            }
        }
    }
}
