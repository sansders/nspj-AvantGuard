using AlgorithmLibary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;
        string connectionString = null;

        public LoginPage()
        {
            InitializeComponent();
            connectionString = conSettings.ConnectionString;
           
            String allText = System.IO.File.ReadAllText(@"../../../../Resource/Algorithms/AlgorithmClass/ClassLibrary1/TextFile1.txt");
            string[][] myList = PredictionModel.readFromFile(allText);
            string userID = "trying";
            string currentPublicIP = PredictionModel.getCurrentPublicIP();
            string macAddress = PredictionModel.getCurrentMAC();
            string date = PredictionModel.getCurrentDate();
            string[][] ipAddressCollection =
           {
                new string[] { "131.23.244.105", "C00008", "4" } ,
                new string[] { "131.23.244.105", "C00008", "4" } ,
                new string[] { "147.120.34.99", "C00008", "1" } ,
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { currentPublicIP, "D8000", "4" },
                new string[] { currentPublicIP, macAddress, "4" },
                new string[] { currentPublicIP, macAddress, "4" },
                new string[] { "151.23.244.105", "C000324", "4" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "151.23.244.105", "C000324", "4" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "151.23.244.105", "C000324", "4" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "151.23.244.105", "C000324", "4" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "151.23.244.105", "C000324", "4"},
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" },
                new string[] { "131.23.244.105", "D00008", "3" }
            };

           
            int counter = 0;
            Console.WriteLine(myList.Count());
            Console.WriteLine(ipAddressCollection.Count());
            foreach (var element in myList)
            {
                string loginTime = element[0];
                date = element[1];
                string ipAddress = ipAddressCollection[counter][0];
                macAddress = ipAddressCollection[counter][1];
                counter++;
                //saveDateTimeOfUser(userID, connectionString, loginTime, date , ipAddress , macAddress);
                //deleteDateTimeOfUser(userID, connectionString, loginTime, date);
            }
        }

        

        public string GetSha512FromString(string strData)
        {
            //strData.Insert(0, "XYZ");
            var message = Encoding.ASCII.GetBytes(strData.Insert(2, "02662028c15f6f31c4758babadb008ee7b98e1bb07351f08d492ee75cb9a26f5079b81c01f14f78cf5f9639e49d7319ee3c3fcc1f94e686b8d605c93f2ab9fb4"));
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";


            byte[] currentHash = hashString.ComputeHash(message);
            for (int i = 0; i < 100; i++)
            {
                if (i != 0)
                {
                    //var message = Encoding.ASCII.GetBytes(strData.Insert(0, "026620758babadb008ee7b98e1bb07351f08d49228c15f6f31c4ee75cb9a26f5079b81c01f14f78cf5f9639e49d7319ee3c3fcc1f94e686b8d605c93f2ab9fb4"));
                    //byte[] currentHash = hashString.ComputeHash(message);

                    currentHash = hashString.ComputeHash(currentHash);
                    Console.WriteLine(i + "round");
                }
            }


            foreach (byte x in currentHash)
            {
                hex += String.Format("{0:x2}", x);
            }


            return hex;
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            PasswordTextBox.Password = GetSha512FromString(PasswordTextBox.Password);
            MessageBox.Show(PasswordTextBox.Password);

            
           

            try
            {
                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("select * from [dbo].[test] where UserID = '" + UserIDTextBox.Text + "' and Password = '" + PasswordTextBox.Password + "'", con);
                reader = cmd.ExecuteReader();

                int count = 0;
                while (reader.Read())
                {
                    count += 1;
                    //Bryan's code
                    Console.WriteLine(" | UserID : " + reader.GetString(0) + " | Password : " + reader.GetString(1) + " | Name : " + reader.GetString(2) + " | Email : " + reader.GetString(3) + " | ContactNo : " + reader.GetString(4));
                   
                }

                if (count == 1)
                {
                    string userID = UserIDTextBox.Text;
                    string[][] userList = checkUserEligibility(userID , connectionString);

                    if(userList.Count() < 30 )
                    {
                        
                        string date = AlgorithmLibary.PredictionModel.getCurrentDate();
                        string loginTime = DateTime.Now.ToString("HH.mm");
                        string publicIP = PredictionModel.getCurrentPublicIP();
                        string publicMAC = PredictionModel.getCurrentMAC();
                        string userLogInPreference = getUserLogInPreference(userID, connectionString);
                        Boolean consideredNormal = evaulateUserLogInString(userLogInPreference , loginTime);
                        if(consideredNormal == true)
                        { 
                            saveDateTimeOfUser(userID, connectionString , loginTime , date , publicIP , publicMAC);
                        }
                        else
                        {
                            //Do 2fa and if authenticated then save date time 
                        }
                    }
                    
                    else if (userList.Count() >= 30)
                    {
                        //Run the login prediction  
                        string date = AlgorithmLibary.PredictionModel.getCurrentDate();
                        string loginTime = DateTime.Now.ToString("HH.mm");
                        string publicIP = PredictionModel.getCurrentPublicIP();
                        string publicMAC = PredictionModel.getCurrentMAC();
                        string[][] logInCollection = getUserLogInData(userID, connectionString);
                        double testTime = Convert.ToDouble(loginTime);
                        double testDay = Convert.ToDouble(date);

                        PredictionModel logInPredictionModel = new PredictionModel(testTime, testDay, logInCollection);
                        string logInRiskLevel = logInPredictionModel.logInRisk;
                        string logInOutput = logInPredictionModel.logInOutput;
                        Console.WriteLine(logInOutput);
                        Console.WriteLine("The risk level is " + logInRiskLevel);

                        //Left with intergration of the IP MAC prediction and total risk factor 
                        //If Risk is low , login 
                        //Else if risk is medium disable access list 
                        //Else if risk is high, do 2fa and set a counter into a new database called 

                    }



              


                    MessageBox.Show("Successful Login.");

                }

                //else if (count > 0)
                //{
                //    cmd = new SqlCommand("INSERT INTO dbo.test (DateOfLogin, TimeOfLogin, SuccessfulLogin, AccountLocked) VALUES (@DateOfLogin, @TimeOfLogin, @SuccessfulLogin, @AccountLocked", con);
                //    cmd.Parameters.AddWithValue("@DateOfLogin", DateTime.Now.ToShortDateString());
                //    cmd.Parameters.AddWithValue("@TimeOfLogin", DateTime.Now.ToString("HH.mm"));
                //    cmd.Parameters.AddWithValue("@SuccessfulLogin", 'N');
                //    cmd.Parameters.AddWithValue("@AccountLocked", null);

                //    MessageBox.Show("Duplicate userid and password.");
                //}

                else
                {
                    //cmd = new SqlCommand("INSERT INTO dbo.test (DateOfLogin, TimeOfLogin, SuccessfulLogin, AccountLocked) VALUES (@DateOfLogin, @TimeOfLogin, @SuccessfulLogin, @AccountLocked", con);
                    //cmd.Parameters.AddWithValue("@DateOfLogin", DateTime.Now.ToShortDateString());
                    //cmd.Parameters.AddWithValue("@TimeOfLogin", DateTime.Now.ToString("HH.mm"));
                    //cmd.Parameters.AddWithValue("@SuccessfulLogin", 'N');
                    //cmd.Parameters.AddWithValue("@AccountLocked", null);
                    MessageBox.Show("Invalid user id or password.");


                    
                }   

                UserIDTextBox.Clear();
                PasswordTextBox.Clear();               

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            finally
            {

                con.Close();
            }
        }

        private string[][] getUserLogInData(string userID, string connectionString)
        {
            SqlConnection con;
            SqlCommand cmd;
            SqlDataReader reader;
            con = new SqlConnection(connectionString);
            con.Open();
            string[][] myList = null;
            try
            {
                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("SELECT * FROM [dbo].[LogAnalysis] where UserID = '" + userID + "'", con);
                reader = cmd.ExecuteReader();
                List<String[]> myCollection = new List<string[]>();
                while (reader.Read())
                {
                   myCollection.Add(new string[] { reader.GetString(1), reader.GetString(2) });
                }

                int counter = 0;
                myList = new string[myCollection.Count()][];
                foreach(var element in myCollection)
                {
                    myList[counter] = element;
                    counter++;
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

            return myList ;

        }

        private bool evaulateUserLogInString(string userLogInPreference , string logInTime)
        {
            int start = 0;
            int end = 0; 
            if(userLogInPreference.Contains("a"))
            {
                end += 6;
               
            }
            if(userLogInPreference.Contains("b"))
            {
                end += 6;
            }
            if (userLogInPreference.Contains("c"))
            {
                end += 6;
            }
            if (userLogInPreference.Contains("d"))
            {
                end += 6;
            }
            if(userLogInPreference.Contains("e"))
            {
                start = 0;
                end = 24;
            }

            double logInTimeDbl = Convert.ToDouble(logInTime);
            if(start <= logInTimeDbl && logInTimeDbl <= end)
            {
                Console.WriteLine(Convert.ToString(start) + " is the start and the end is " + Convert.ToString(end));
                Console.WriteLine("Within the range");
                return true;
            }
            else
            {
                Console.WriteLine("Not within the range");
                return false;
            }


        }

        private string getUserLogInPreference(string userID, string connectionString)
        {
            //10 - 13
            SqlConnection con;
            SqlCommand cmd;
            SqlDataReader reader;
            con = new SqlConnection(connectionString);
            con.Open();
            string data = null;
            try
            {
                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("SELECT * FROM [dbo].[test] where UserID = '" + userID +"'", con);
                reader = cmd.ExecuteReader();
                
                if(reader == null)
                {
                    Console.WriteLine("Blahblah");
                }
                else { 
                    while (reader.Read())
                    {
                        data =  reader.GetString(13);
                        Console.WriteLine(reader.GetString(0) + "|" + reader.GetString(13));
                    }
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


            return data;
        }

        private void deleteDateTimeOfUser(string userID, string connectionString, string loginTime, string date)
        {

            SqlConnection con;
            SqlCommand cmd;
            con = new SqlConnection(connectionString);
            con.Open();
            try
            {
                cmd = new SqlCommand("DELETE FROM [dbo].[LogAnalysis] where UserID = '" + userID  + "'", con);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void saveDateTimeOfUser(string userID, string connectionString , string loginTime, string date , string publicIP , string publicMAC )
        {
            SqlConnection con;
            SqlCommand cmd;
            con = new SqlConnection(connectionString);
            con.Open();
            try
            {

               
                cmd = new SqlCommand("INSERT INTO [dbo].[LogAnalysis] (UserID, LoginTime, LoginDate, IpAddress , MacAddress) VALUES (@UserID, @LoginTime, @LoginDate , @IPAddress , @MACAddress)", con);
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.Parameters.AddWithValue("@LoginTime", loginTime);
                cmd.Parameters.AddWithValue("@LoginDate", date.ToString());
                cmd.Parameters.AddWithValue("@IPAddress", publicIP);
                cmd.Parameters.AddWithValue("@MACAddress", publicMAC);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private string[][] checkUserEligibility(string userID , string connectionString)
        {
            SqlConnection con;
            SqlCommand cmd;
            SqlDataReader reader;
            con = new SqlConnection(connectionString);
            con.Open();
            MessageBox.Show(userID);
            cmd = new SqlCommand("select * from [dbo].[LogAnalysis] where UserID = '" + userID + "'", con);
            reader = cmd.ExecuteReader();
            List<String[]> userList = new List<String[]>();
            if(reader == null)
            {
                MessageBox.Show("JACSIcnm");
            }
            else { 
            while(reader.Read())
            {
               userList.Add(new string[] { reader.GetString(1), reader.GetString(2) });
            }
            }
           
            string[][] myList = new string[userList.Count()][];
            int counter = 0;
            foreach (var element in userList)
            {
                myList[counter] = element;
                Console.WriteLine(element[0] + "|" + element[1]);
                counter++;
            }
            
            con.Close();
            return myList;
        }

        private void ButtonSignUp_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"SignUp1.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ButtonForgetPass_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"ForgotPassword1.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
