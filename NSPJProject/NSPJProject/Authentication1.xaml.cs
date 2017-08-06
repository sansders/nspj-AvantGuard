using AlgorithmLibary;
using Cloud.StartupPage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
    /// Interaction logic for Authentication1.xaml
    /// </summary>
    public partial class Authentication1 : Page
    {

        ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
        string connectionString = null;
        static int counter = 0;

        public Authentication1()
        {
            MessageBox.Show("HI");
            InitializeComponent();
            connectionString = conSettings.ConnectionString;
        }

        //private void saveDateTimeOfUser(string userID, string connectionString, string loginTime, string date, string publicIP, string publicMAC)
        //{
        //    SqlConnection con;
        //    SqlCommand cmd;
        //    con = new SqlConnection(connectionString);
        //    string currentHostname = System.Environment.MachineName.ToString();
        //    con.Open();
        //    try
        //    {


        //        cmd = new SqlCommand("INSERT INTO [dbo].[LogAnalysis] (UserID, LoginTime, LoginDate, IpAddress , MacAddress , hostname) VALUES (@UserID, @LoginTime, @LoginDate , @IPAddress , @MACAddress , @HostName)", con);
        //        cmd.Parameters.AddWithValue("@UserID", userID);
        //        cmd.Parameters.AddWithValue("@LoginTime", loginTime);
        //        cmd.Parameters.AddWithValue("@LoginDate", date.ToString());
        //        cmd.Parameters.AddWithValue("@IPAddress", publicIP);
        //        cmd.Parameters.AddWithValue("@MACAddress", publicMAC);
        //        cmd.Parameters.AddWithValue("@HostName", currentHostname);
        //        cmd.ExecuteNonQuery();

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //}

        private void ForgotPassword3NextButton_Click(object sender, RoutedEventArgs e)
        {
            string selected_ForgotPasswordCode = UserModel.UserModel.twoFAcode;
            UserModel.UserModel cm = UserModel.UserModel._currentUserModel;
            string userID = cm.userID;
            if (ForgotPasswordCodeTextBox.Text == selected_ForgotPasswordCode)
            {
                MessageBox.Show("Correct!");
                string date = AlgorithmLibary.PredictionModel.getCurrentDate();
                string loginTime = DateTime.Now.ToString("HH.mm");
                string publicIP = PredictionModel.getCurrentPublicIP();
                string publicMAC = PredictionModel.getCurrentMAC();
                Console.WriteLine(publicMAC + "HELLO");

                //Use the same class for saveDateTime Method - Justin Changed at 1:20 am on 6/8/2017
                UserModel.UserModel.saveDateTimeOfUser(userID, connectionString, loginTime, date, publicIP, publicMAC);
                string exist = UserModel.UserModel.checkFollowUp(userID, connectionString);

                Page cloud = new StartupPage();
                this.NavigationService.Navigate(cloud);
            }

            else
            {
                MessageBox.Show("Invalid code! Please Try Again");
                counter++;
                if (counter > 2)
                {
                    MessageBox.Show("More than 3 attempts! Account will be locked now!");
                    string exist = UserModel.UserModel.checkFollowUp(userID, connectionString);
                    if (exist != null)
                    {
                        UserModel.UserModel.updateFollowUp(userID, connectionString, "True");
                    }
                    else
                    {
                        UserModel.UserModel.saveFollowUp(userID, connectionString, "True");
                    }
                    Page LoginPage = new LoginPage();
                    this.NavigationService.Navigate(LoginPage);
                }
            }
        }

    }
}
