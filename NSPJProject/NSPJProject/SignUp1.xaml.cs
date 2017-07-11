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
    /// Interaction logic for SignUp1.xaml
    /// </summary>
    public partial class SignUp1 : Page
    {
        public SignUp1()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextNumeric(e.Text);
        }

        private static bool IsTextNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9]");
            return reg.IsMatch(str);
        }

        private void SignUp1NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(SignUpUserIDTextBox.Text) || SignUpPasswordTextBox.SecurePassword.Length == 0 || SignUpPasswordTextBox.SecurePassword.Length < 8
                || String.IsNullOrEmpty(SignUpNameTextBox.Text) || String.IsNullOrEmpty(SignUpEmailTextBox.Text) ||
                !Regex.IsMatch(SignUpEmailTextBox.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$") ||
                String.IsNullOrEmpty(SignUpContactTextBox.Text) || SignUpDOBDatePicker.SelectedDate == null ||
                SignUpMaleRadioButton.IsChecked == false && SignUpFemaleRadioButton.IsChecked == false)
            {
                MessageBox.Show("Please make sure that all blanks are filled.");

                if (String.IsNullOrEmpty(SignUpUserIDTextBox.Text))
                {
                    UserIDImage.Visibility = Visibility.Visible;
                }

                else
                {
                    UserIDImage.Visibility = Visibility.Hidden;
                }

                if (SignUpPasswordTextBox.SecurePassword.Length == 0)
                {
                    PasswordImage.Visibility = Visibility.Visible;
                }

                else
                {
                    PasswordImage.Visibility = Visibility.Hidden;
                }

                if (String.IsNullOrEmpty(SignUpNameTextBox.Text))
                {
                    NameImage.Visibility = Visibility.Visible;
                }

                else
                {
                    NameImage.Visibility = Visibility.Hidden;
                }

                if (String.IsNullOrEmpty(SignUpEmailTextBox.Text))
                {
                    EmailImage.Visibility = Visibility.Visible;
                }

                else
                {
                    EmailImage.Visibility = Visibility.Hidden;
                }

                if (String.IsNullOrEmpty(SignUpContactTextBox.Text))
                {
                    ContactImage.Visibility = Visibility.Visible;
                }

                else
                {
                    ContactImage.Visibility = Visibility.Hidden;
                }

                if (SignUpDOBDatePicker.SelectedDate == null)
                {
                    DOBImage.Visibility = Visibility.Visible;
                }

                else
                {
                    DOBImage.Visibility = Visibility.Hidden;
                }

                if (SignUpMaleRadioButton.IsChecked == false && SignUpFemaleRadioButton.IsChecked == false)
                {
                    GenderImage.Visibility = Visibility.Visible;
                }

                else
                {
                    GenderImage.Visibility = Visibility.Hidden;
                }

                if (SignUpPasswordTextBox.SecurePassword.Length < 8)
                {
                    PasswordImage.Visibility = Visibility.Visible;
                    MessageBox.Show("Password must be at least 8 characters.");
                }

                else
                {
                    PasswordImage.Visibility = Visibility.Hidden;
                }

                if (!Regex.IsMatch(SignUpEmailTextBox.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
                {
                    EmailImage.Visibility = Visibility.Visible;
                    MessageBox.Show("Please enter a valid email.");
                }

                else
                {
                    EmailImage.Visibility = Visibility.Hidden;
                }
            }

            else
            {
                this.NavigationService.Navigate(new Uri(@"SignUp2.xaml", UriKind.RelativeOrAbsolute));
            }


        }

        private void SignUp1BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"LoginPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
