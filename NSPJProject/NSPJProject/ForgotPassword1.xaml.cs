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
    /// Interaction logic for ForgotPassword1.xaml
    /// </summary>
    public partial class ForgotPassword1 : Page
    {
        public ForgotPassword1()
        {
            InitializeComponent();
        }

        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;

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

                ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
                string connectionString = conSettings.ConnectionString;

                try
                {
                    con = new SqlConnection(connectionString);
                    con.Open();
                    cmd = new SqlCommand("select * from [dbo].[test] where Email = '" + ForgotPasswordEmailTextBox.Text + "'", con);
                    reader = cmd.ExecuteReader();

                    int count = 0;
                    while (reader.Read())
                    {
                        count += 1;
                        //Bryan's code
                        Console.WriteLine(" | UserID : " + reader.GetString(0) + " | Password : " + reader.GetString(1) + " | Name : " + reader.GetString(2) + " | Email : " + reader.GetString(3) + " | ContactNo : " + reader.GetString(4) + " | DOB : " + reader.GetString(5) + " | SecurityQ1 : " + reader.GetString(6) + " | Q1Ans : " + reader.GetString(7) + " | SecurityQ2 : " + reader.GetString(8) + " | Q2Ans : " + reader.GetString(9));
                        (App.Current as App).SecurityQ1 = reader.GetString(6);
                        (App.Current as App).Q1Ans = reader.GetString(7);
                        (App.Current as App).SecurityQ2 = reader.GetString(8);
                        (App.Current as App).Q2Ans = reader.GetString(9);

                    }

                    if (count == 1)
                    {
                        MessageBox.Show("Successful Login.");

                    }

                    else
                    {
                        MessageBox.Show("Invalid user id or password.");
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
                finally
                {

                    con.Close();
                }

                this.NavigationService.Navigate(new Uri(@"ForgotPassword2.xaml? key1=" + ForgotPasswordEmailTextBox.Text, UriKind.RelativeOrAbsolute));
            }
        }
    }
}
