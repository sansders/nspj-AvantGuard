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
using UserModel;

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
            Console.WriteLine(macAddress);
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
            //string username = "username";
            //deleteACertainUser(username, connectionString);
            //string test = "0";
            //deleteDateTimeOfUser(username, connectionString, test, date);
        }

        private void deleteACertainUser(string userID, string connectionString)
        {
            SqlConnection con;
            SqlCommand cmd;
            con = new SqlConnection(connectionString);
            con.Open();
            try
            {
                cmd = new SqlCommand("DELETE FROM [dbo].[test] where UserID = '" + userID + "'", con);
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
                    Console.WriteLine(" | UserID : " + reader.GetString(0) + " | Password : " + reader.GetString(1) + " | Name : " + reader.GetString(2) + " | Email : " + reader.GetString(3) + " | ContactNo : " + reader.GetString(4));
                  
                }

                if (count == 1)
                {
                    string userID = UserIDTextBox.Text;
                    string[][] userList = checkUserEligibility(userID , connectionString);
                    UserModel.UserModel.currentUserID = userID;
                    string currentUser = UserModel.UserModel.currentUserID;
                    MessageBox.Show(currentUser + "is thios");
                    UserModel.UserModel um = UserModel.UserModel.retrieveUserFromDatabase(currentUser);
                    
                    Console.WriteLine(um.userPassword);
                    if (userList.Count() < 30 )
                    {
                        
                        string date = AlgorithmLibary.PredictionModel.getCurrentDate();
                        string loginTime = DateTime.Now.ToString("HH.mm");
                        string publicIP = PredictionModel.getCurrentPublicIP();
                        string publicMAC = PredictionModel.getCurrentMAC();
                        string userLogInPreference = getUserLogInPreference(userID, connectionString);
                        string userComputerPreference = getUserComputerPreference(userID, connectionString);
                        //The method below is supposed to read from the database all the entries of hostname for this specific user

                        string[] currentHostnameSet = getUserHostNameSet(userID, connectionString); 
                        foreach(var element in currentHostnameSet)
                        {
                            Console.WriteLine(element + "JADSjc"); 
                        }
                        //string[] currentHostnameSet =
                        //{
                        //    "JUSTINSOH-PC",
                        //    "JUSTINSOH-PC",
                        //    "JUSTINSOH-PC",
                        //    "JUSTINSOH-PCC",
                        //    "JUSTINSOH-PCC",
                        //    "JUSTINSOH-PCC",

                        //};
                        double logInRisk = evaulateUserLogInString(userLogInPreference , loginTime);

                        double userHostRisk = evaulateUserComputerPreference(userComputerPreference, currentHostnameSet);

                        logInRisk = logInRisk * 0.3;
                        userHostRisk = userHostRisk * 0.7;

                        double totalRisk = logInRisk + userHostRisk;
                        Console.WriteLine(userHostRisk + " HOSTNAME");
                        Console.WriteLine(logInRisk + "LOG IN RISK");
                        Console.WriteLine(totalRisk);
                        string riskStatement = null;
                        if (totalRisk <= 0.4)
                        {
                            riskStatement = "The risk level is low";
                            saveDateTimeOfUser(userID, connectionString, loginTime, date, publicIP, publicMAC);
                            //Navigate To Chester
                        }

                        // Removing access control and giving access control
                        else if (totalRisk <= 0.70)
                        {
                            riskStatement = "The risk level is medium";
                            //Remove Access Control 

                        }

                        //Instantly Re authenticate
                        else if (totalRisk > 0.70)
                        {
                            riskStatement = "The risk level is high";
                            string subject = "Authentication Message";
                            string subjectBody = "Authentication Code is ";
                            UserModel.UserModel.do2fa(subject, subjectBody, "hoggersoh@gmail.com");

                        }
                        Console.WriteLine(riskStatement);
                     
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


                        string[][] ipAddressCollection = getUserIPAddressCollection(userID, connectionString);
                        Console.Write(ipAddressCollection.Count());
                        string[] query = new string[] { publicIP , publicMAC, date };
                        PredictionModel ipPredictionModel = new PredictionModel(ipAddressCollection, query);
                        string ipRisk = ipPredictionModel.ipRisk;
                        string ipOutput = ipPredictionModel.ipOutput;
                        Console.WriteLine(ipOutput);

                        double logInPercentage = Convert.ToDouble(logInRiskLevel) / 5.0;
                        double ipPercentage = Convert.ToDouble(ipRisk);

                        logInPercentage = (logInPercentage / 100) * 30;
                        ipPercentage = (ipPercentage / 100) * 70;
                        double riskLevel = logInPercentage + ipPercentage;
                        Console.WriteLine(logInPercentage);
                        Console.WriteLine(ipRisk);
                        Console.WriteLine(riskLevel);
                        string riskStatement = null;
                        //Can do anything 
                        if (riskLevel <= 0.4)
                        {
                            riskStatement = "The risk level is low";
                            saveDateTimeOfUser(userID, connectionString, loginTime, date, publicIP, publicMAC);
                            //Navigate To Chester
                        }

                        // Removing access control and giving access control
                        else if (riskLevel <= 0.70)
                        {
                            riskStatement = "The risk level is medium";
                            //Remove Access Control 

                        }

                        //Instantly Re authenticate
                        else if (riskLevel > 0.70)
                        {
                            riskStatement = "The risk level is high";
                            //Do 2FA
                        }
                        Console.WriteLine(riskStatement);
                     

                    }

                    (App.Current as App).LoginUserID = UserIDTextBox.Text;
                    MessageBox.Show("Successful Login.");
                    this.NavigationService.Navigate(new Uri(@"EditUserInfo.xaml", UriKind.RelativeOrAbsolute));

                }

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

        private string[] getUserHostNameSet(string userID, string connectionString)
        {
            SqlConnection con;
            SqlCommand cmd;
            SqlDataReader reader;
            con = new SqlConnection(connectionString);
            con.Open();
            string hostName = null;
            string[] fullList = null;
            List<string> fullHostName = new List<string>();
            try
            {
                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("SELECT * FROM [dbo].[LogAnalysis] where UserID = '" + userID + "'", con);
                reader = cmd.ExecuteReader();
                List<String[]> myCollection = new List<string[]>();

                while (reader.Read())
                {
                   
                    hostName = reader.GetString(5);
                    fullHostName.Add(hostName);
                }
                int counter = 0;
                fullList = new string[fullHostName.Count()];
                foreach(var element in fullHostName)
                {
                    fullList[counter] = element;
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

            return fullList;
        }

        private double evaulateUserComputerPreference(string userComputerPreference, string[] currentHostnameSet)
        {
            double riskLevel = 0;
            double likelihood = 0;
            if(userComputerPreference.Contains("a"))
            {
                riskLevel = 0.7;
            }
            if(userComputerPreference.Contains("b"))
            {
                riskLevel = 0.3; 
            }
            if(userComputerPreference.Contains("c"))
            {
                riskLevel = 0.5;
            }
            string currentHostname = System.Environment.MachineName.ToString();
     
            double totalMatch = 0;
            foreach (var element in currentHostnameSet)
            {
                if(currentHostname.Equals(element))
                {
                    totalMatch++;
                   
                }
            }

            if(currentHostnameSet.Count() == 0)
            {
                likelihood = 0;
            }
            else
            {
                likelihood = (totalMatch / currentHostnameSet.Count()) ;
            }

            riskLevel = 1 - likelihood;
            Console.WriteLine(riskLevel + "risk level");

            return riskLevel;
        }

      


        private string getUserComputerPreference(string userID, string connectionString)
        {
            SqlConnection con;
            SqlCommand cmd;
            SqlDataReader reader;
            con = new SqlConnection(connectionString);
            con.Open();
            string choice = null;
            try
            {
                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("SELECT * FROM [dbo].[test] where UserID = '" + userID + "'", con);
                reader = cmd.ExecuteReader();
                List<String[]> myCollection = new List<string[]>();
               
                while (reader.Read())
                {
                   choice = reader.GetString(12);
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

            return choice;
        }

        private string[][] getUserIPAddressCollection(string userID, string connectionString)
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
                    myCollection.Add(new string[] { reader.GetString(3), reader.GetString(4) , reader.GetString(2) });
                }

                int counter = 0;
                myList = new string[myCollection.Count()][];
                foreach (var element in myCollection)
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

            return myList;
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

        private double evaulateUserLogInString(string userLogInPreference , string logInTime)
        {
            double start = 0;
            double end = 0;
            Console.WriteLine(userLogInPreference);
            List<double[]> userList = new List<double[]>();
            if (userLogInPreference.Contains("a"))
            {
                start = 0;
                end = start + 6;
                double[] newSet = new double[] { start, end };
                userList.Add(newSet);
            }
            else if (userLogInPreference.Contains("b"))
            {
                start = 6;
                end = start + 6;
                double[] newSet = new double[] { start, end };
                userList.Add(newSet);
            }
            if (userLogInPreference.Contains("c"))
            {
                start = 12;
                end = start + 6;
                double[] newSet = new double[] { start, end };
                userList.Add(newSet);
            }
            if (userLogInPreference.Contains("d"))
            {
                start = 18;
                end = start + 6;
                double[] newSet = new double[] { start, end };
                userList.Add(newSet);
            }
            if (userLogInPreference.Contains("e"))
            {
                start = 0;
                end = start + 24;
                double[] newSet = new double[] { start, end };
                userList.Add(newSet);
            }
            double riskPercent = 0;
            double logInTimeDbl = Convert.ToDouble(logInTime);
            int counter = 0;
            foreach(var element in userList)
            {
                do
                {
                    Console.WriteLine("Matching Exactly");
                    riskPercent = matchExactly(element[0], element[1], logInTimeDbl);
                    if (riskPercent == 0)
                    {
                        Console.WriteLine("Matching 3 Hours Buffer");
                        riskPercent = match3Buffer(element[0], element[1], logInTimeDbl);
                        if (riskPercent == 0)
                        {
                            Console.WriteLine("Matching 6 Hours Buffer");
                            riskPercent = match6Buffer(element[0], element[1], logInTimeDbl);
                            if (riskPercent == 0)
                            {
                                Console.WriteLine("Not in any range");
                                riskPercent = 0.8;
                                counter++;
                                break;
                            }
                           

                        }
                        else
                        {
                            counter++;
                            break;
                        }

                    }
                    else
                    {
                        counter++;
                        break;
                    }
                }
                while (counter == 0);
                if(counter != 0)
                {
                    break;
                }

            }

          

         

            return riskPercent;

            //double riskPercent = 0;
            //double logInTimeDbl = Convert.ToDouble(logInTime);
            //Console.WriteLine(start.ToString() + " to " + end.ToString());
            //if(start <= logInTimeDbl && logInTimeDbl <= end)
            //{
            //    Console.WriteLine(Convert.ToString(start) + " is the start and the end is " + Convert.ToString(end));
            //    Console.WriteLine("Within the exact range");
            //    riskPercent = 0.1;
            //}
            //else if(start - 3<= logInTimeDbl && logInTimeDbl <= end + 3)
            //{
            //    Console.WriteLine("Not within the exact range but with 3 hours buffer");
            //    riskPercent = 0.4;
            //}
            //else if (start - 6 <= logInTimeDbl && logInTimeDbl <= end + 6)
            //{
            //    Console.WriteLine("Not within the exact range but with 6 hours buffer");
            //    riskPercent = 0.6;
            //}
            //else
            //{
            //    Console.WriteLine("Not in any range");
            //    riskPercent = 0.8;
            //}
            //Console.WriteLine(riskPercent);
            //return riskPercent;


        }

        private double matchExactly(double start , double end, double logInTimeDbl)
        {
            double riskPercent = 0;
            if(start <= logInTimeDbl && logInTimeDbl <= end)
            {
                Console.WriteLine(Convert.ToString(start) + " is the start and the end is " + Convert.ToString(end));
                Console.WriteLine("Within the exact range");
                riskPercent = 0.1;
            }
            return riskPercent;
        }

        private double match3Buffer(double start, double end, double logInTimeDbl)
        {
            double riskPercent = 0;
            if (start -3 <= logInTimeDbl && logInTimeDbl <= end + 3)
            {
                Console.WriteLine(Convert.ToString(start) + " is the start and the end is " + Convert.ToString(end));
                Console.WriteLine("Not within the exact range but with 3 hours buffer");
                riskPercent = 0.4;
            }
            return riskPercent;
        }

        private double match6Buffer(double start, double end, double logInTimeDbl)
        {
            double riskPercent = 0;
            if (start - 6 <= logInTimeDbl && logInTimeDbl <= end + 6)
            {
                Console.WriteLine(Convert.ToString(start) + " is the start and the end is " + Convert.ToString(end));
                Console.WriteLine("Not within the exact range but with 6 hours buffer");
                riskPercent = 0.6;
            }
            return riskPercent;
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
            string currentHostname = System.Environment.MachineName.ToString();
            con.Open();
            try
            {

               
                cmd = new SqlCommand("INSERT INTO [dbo].[LogAnalysis] (UserID, LoginTime, LoginDate, IpAddress , MacAddress , hostname) VALUES (@UserID, @LoginTime, @LoginDate , @IPAddress , @MACAddress , @HostName)", con);
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.Parameters.AddWithValue("@LoginTime", loginTime);
                cmd.Parameters.AddWithValue("@LoginDate", date.ToString());
                cmd.Parameters.AddWithValue("@IPAddress", publicIP);
                cmd.Parameters.AddWithValue("@MACAddress", publicMAC);
                cmd.Parameters.AddWithValue("@HostName", currentHostname);
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

        private void ButtonChangePass_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"ForgotPassword1.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
