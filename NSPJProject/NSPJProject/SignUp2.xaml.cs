
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
using WpfApp1.Model1;

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

        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;

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
                string selected_userID = (App.Current as App).UserID;

                string selected_userPassword = (App.Current as App).UserPassword;

                string selected_userName = (App.Current as App).UserName;

                string selected_userEmail = (App.Current as App).UserEmail;

                string selected_userContact = (App.Current as App).UserContact;

                string selected_userDOB = (App.Current as App).UserDOB;

                string selected_securityQ1 = SecurityQuestion1TextBox.Text;

                string selected_securityQ1Ans = Answer1TextBox.Text;

                string selected_securityQ2 = SecurityQuestion2TextBox.Text;

                string selected_securityQ2Ans = Answer2TextBox.Text;

                //Comment out the method to save to database 
                UserModel.UserModel newModel = new UserModel.UserModel(selected_userID, selected_userPassword, selected_userName, selected_userEmail, selected_userContact, selected_userDOB,
                selected_securityQ1 , selected_securityQ1Ans, selected_securityQ2, selected_securityQ2Ans);

                UserModel.UserModel.currentUserModel = newModel;

                //newModel.saveToDatabase();
                
                //ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
                //string connectionString = conSettings.ConnectionString;

                //try
                //{
                //    con = new SqlConnection(connectionString);
                //    con.Open();
                //    cmd = new SqlCommand("INSERT INTO [dbo].[test] (UserID, Password, Name, Email, ContactNo, DOB, SecurityQ1, Q1Ans, SecurityQ2, Q2Ans) VALUES (@UserID, @Password, @Name, @Email, @ContactNo, @DOB, @SecurityQ1, @Q1Ans, @SecurityQ2, @Q2Ans)", con);
                //    cmd.Parameters.AddWithValue("@UserID", selected_userID);
                //    cmd.Parameters.AddWithValue("@Password", selected_userPassword);
                //    cmd.Parameters.AddWithValue("@Name", selected_userName);
                //    cmd.Parameters.AddWithValue("@Email", selected_userEmail);
                //    cmd.Parameters.AddWithValue("@ContactNo", selected_userContact);
                //    cmd.Parameters.AddWithValue("@DOB", selected_userDOB);
                //    cmd.Parameters.AddWithValue("@SecurityQ1", SecurityQuestion1TextBox.Text);
                //    cmd.Parameters.AddWithValue("@Q1Ans", Answer1TextBox.Text);
                //    cmd.Parameters.AddWithValue("@SecurityQ2", SecurityQuestion2TextBox.Text);
                //    cmd.Parameters.AddWithValue("@Q2Ans", Answer2TextBox.Text);

                //    cmd.ExecuteNonQuery();

                //}
                //catch (Exception ex)
                //{
                //    System.Windows.MessageBox.Show(ex.Message);
                //}
                //finally
                //{

                //    con.Close();
                //}

                //MessageBox.Show("You're almost done. An email has been sent to your email address. Please verify your email address, thank you!");
                CurrentPageModel currentModel = new CurrentPageModel();
                Page currentPage = new WpfApp1.ProfilePages.Page1();
                this.NavigationService.Navigate(currentPage);
                //rootBox.NavigationService.Navigate(currentPage);
                //this.NavigationService.Navigate(new Uri(@"SignUp3.xaml", UriKind.RelativeOrAbsolute));
            }

        }

        private void SignUp2BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"SignUp1.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
