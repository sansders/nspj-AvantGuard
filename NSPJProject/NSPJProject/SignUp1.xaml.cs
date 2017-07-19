using NSPJProject.Model1;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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

        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;
        

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
                String.IsNullOrEmpty(SignUpContactTextBox.Text) || SignUpDOBDatePicker.SelectedDate == null)
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
                ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
                string connectionString = conSettings.ConnectionString;


                
                //UserModel user = new UserModel();
                //user.userID = SignUpUserIDTextBox.Text;
                //user.userPassword = SignUpPasswordTextBox.Password;
                //user.userName = SignUpNameTextBox.Text;
                //user.userEmail = SignUpEmailTextBox.Text;
                //user.userContact = SignUpContactTextBox.Text;
                //user.userDOB = SignUpDOBDatePicker.Text;

                try
                {
                    con = new SqlConnection(connectionString);
                    con.Open();
                    cmd = new SqlCommand("INSERT INTO dbo.test (UserID, Password, Name, Email, ContactNo, DOB) VALUES (@UserID, @Password, @Name, @Email, @ContactNo, @DOB)", con);
                    cmd.Parameters.AddWithValue("@UserID", SignUpUserIDTextBox.Text);
                    cmd.Parameters.AddWithValue("@Password", SignUpPasswordTextBox.Password);
                    cmd.Parameters.AddWithValue("@Name", SignUpNameTextBox.Text);
                    cmd.Parameters.AddWithValue("@Email", SignUpEmailTextBox.Text);
                    cmd.Parameters.AddWithValue("@ContactNo", SignUpContactTextBox.Text);
                    cmd.Parameters.AddWithValue("@DOB", SignUpDOBDatePicker.Text);

                    //cmd = new SqlCommand("INSERT INTO [dbo].[UserAcc] VALUES (user.userID, user.userPassword, user.userName, user.userEmail, user.userContact, user.userDOB); ", con);
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
                finally
                {

                    con.Close();
                }


                this.NavigationService.Navigate(new Uri(@"SignUp2.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        private void SignUp1BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"LoginPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
