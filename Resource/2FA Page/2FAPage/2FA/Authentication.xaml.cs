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
using NSPJProject;
using System.Configuration;
using AlgorithmLibary;
using System.Data.SqlClient;


namespace NSPJProject
{
    /// <summary>
    /// Interaction logic for Authentication.xaml
    /// </summary>
    public partial class Authentication : Page
    {
        ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
        string connectionString = null;
        static int counter = 0;
        public Authentication()
        {
            InitializeComponent();
            connectionString = conSettings.ConnectionString;

        }

        private void ForgotPassword3NextButton_Click(object sender, RoutedEventArgs e)
        {
            string selected_ForgotPasswordCode = UserModel.UserModel.twoFAcode;
            UserModel.UserModel cm = UserModel.UserModel._currentUserModel;
            string userID = cm.userID;
            Console.WriteLine(userID + "TEICJASMCA");
            if (ForgotPasswordCodeTextBox.Text == selected_ForgotPasswordCode)
            {
                MessageBox.Show("Correct!");
                string date = AlgorithmLibary.PredictionModel.getCurrentDate();
                string loginTime = DateTime.Now.ToString("HH.mm");
                string publicIP = PredictionModel.getCurrentPublicIP();
                string publicMAC = PredictionModel.getCurrentMAC();
                Console.WriteLine(publicMAC + "HELLO");
               
                UserModel.UserModel.saveDateTimeOfUser(userID, connectionString, loginTime, date, publicIP, publicMAC);
                string exist = UserModel.UserModel.checkFollowUp(userID, connectionString);
                SqlConnection con;
                SqlCommand cmd;
                string riskLevelStatement = null;
                riskLevelStatement = "Low";
                con = new SqlConnection(connectionString);
                try
                {
                    string connectionString = conSettings.ConnectionString;

                    con = new SqlConnection(connectionString);
                    con.Open();
                    cmd = new SqlCommand("DELETE FROM [dbo].[FailedAttempt] where UserID = '" + userID + "'", con);
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
                if(exist != null)
                {
                    UserModel.UserModel.updateFollowUp(userID, connectionString, "False");
                    
                   
                    //Navigate to Chester page
                    //Page cloud = new StartupPage();
                    //this.NavigationService.Navigate(cloud);

                    
                }
              
                else
                { 
                    UserModel.UserModel.saveFollowUp(userID, connectionString, "False");
                    //Navigate To chester page
                    //Page cloud = new StartupPage();
                    //this.NavigationService.Navigate(cloud);
                }
                PredictionModel.SessionRiskValue = riskLevelStatement;
            }
            else
            {
                MessageBox.Show("Invalid code! Please Try Again");
                counter++;
                if(counter > 3 )
                {
                    MessageBox.Show("More than 3  Failed attempts! Account will be locked now!");
                    string exist = UserModel.UserModel.checkFollowUp(userID, connectionString);
                    if (exist != null)
                    {
                        UserModel.UserModel.updateFollowUp(userID, connectionString, "True");
                    }
                    else { 
                        UserModel.UserModel.saveFollowUp(userID, connectionString , "True");
                    }
                    //Page LoginPage = new LoginPage();
                    //this.NavigationService.Navigate(LoginPage);
                    counter = 0; 
                }
            }
        }



        //private void ForgotPassword3BackButton_Click(object sender, RoutedEventArgs e)
        //{
        //    this.NavigationService.Navigate(new Uri(@"ForgotPassword2.xaml", UriKind.RelativeOrAbsolute));
        //}

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
    }
}
