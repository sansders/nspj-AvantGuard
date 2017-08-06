using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace UserModel
{
    public class UserModel
    {

        public String _userID;
        public String _userPassword;
        public String _userName;
        public String _userEmail;
        public String _userContact;
        public String _userDOB;
        public String _securityQ1;
        public String _securityQ1Ans;
        public String _securityQ2;
        public String _securityQ2Ans;
        public String _profile1Answer;
        public String _profile2Answer;
        public String _profile3Answer;
        public String _profile4Answer;
        public static String _twoFACode;
        public static UserModel _currentUserModel;
        public static string _currentUserID;
        public static string _currentUserLogInTime;
        public static string _currentUserLogInDate;
        public static Boolean _twoFAStatus;
        public UserModel()
        {

        }

        public UserModel(String _userID, String _userPassword, String _userName, String _userEmail, String _userContact, String _userDOB,
            String _securityQ1, String _securityQ1Ans, String _securityQ2, String _securityQ2Ans)
        {
            this._userID = _userID;
            this._userPassword = _userPassword;
            this._userName = _userName;
            this._userEmail = _userEmail;
            this._userContact = _userContact;
            this._userDOB = _userDOB;
            this._securityQ1 = _securityQ1;
            this._securityQ1Ans = _securityQ1Ans;
            this._securityQ2 = _securityQ2;
            this._securityQ2Ans = _securityQ2Ans;
        }

        public UserModel(String _userID, String _userPassword, String _userName, String _userEmail, String _userContact, String _userDOB,
            String _securityQ1, String _securityQ1Ans, String _securityQ2, String _securityQ2Ans, String _Profile1, String _Profile2, String _Profile3, String _Profile4)
        {
            this._userID = _userID;
            this._userPassword = _userPassword;
            this._userName = _userName;
            this._userEmail = _userEmail;
            this._userContact = _userContact;
            this._userDOB = _userDOB;
            this._securityQ1 = _securityQ1;
            this._securityQ1Ans = _securityQ1Ans;
            this._securityQ2 = _securityQ2;
            this._securityQ2Ans = _securityQ2Ans;
            this.profile1 = _Profile1;
            this.profile2 = _Profile2;
            this.profile3 = _Profile3;
            this.profile4 = _Profile4;
        }

        public string userID
        {

            get { return _userID; }
            set { _userID = value; }
        }

        public string userPassword
        {
            get { return _userPassword; }
            set { _userPassword = value; }
        }

        public string userName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public string userEmail
        {
            get { return _userEmail; }
            set { _userEmail = value; }
        }

        public string userContact
        {
            get { return _userContact; }
            set { _userContact = value; }
        }

        public string userDOB
        {
            get { return _userDOB; }
            set { _userDOB = value; }
        }

        public string securityQ1
        {
            get { return _securityQ1; }
            set { _securityQ1 = value; }
        }

        public string securityQ1Ans
        {
            get { return _securityQ1Ans; }
            set { _securityQ1Ans = value; }
        }

        public string securityQ2
        {
            get { return _securityQ2; }
            set { _securityQ2 = value; }
        }

        public string securityQ2Ans
        {
            get { return _securityQ2Ans; }
            set { _securityQ2Ans = value; }
        }

        public string profile1
        {
            get { return _profile1Answer; }
            set { _profile1Answer = value; }
        }

        public string profile2
        {
            get { return _profile2Answer; }
            set { _profile2Answer = value; }
        }

        public string profile3
        {
            get { return _profile3Answer; }
            set { _profile3Answer = value; }
        }

        public string profile4
        {
            get { return _profile4Answer; }
            set { _profile4Answer = value; }
        }


        public static UserModel currentUserModel
        {
            get { return _currentUserModel; }
            set { _currentUserModel = value; }
        }

        public static string currentUserID
        {
            get { return _currentUserID; }
            set { _currentUserID = value; }
        }

        public static string currentUserTime
        {
            get { return _currentUserLogInTime; }
            set { _currentUserLogInTime = value; }
        }

        public static string currentUserDate
        {
            get { return _currentUserLogInDate; }
            set { _currentUserLogInDate = value; }
        }



        public static string twoFAcode
        {
            get { return _twoFACode; }
            set { _twoFACode = value; }
        }

        public static Boolean twoFASucceed
        {
            get { return _twoFAStatus; }
            set { _twoFAStatus = value; }
        }

    public static UserModel retrieveUserFromDatabase(string userID)
        {
            ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
            string connectionString = conSettings.ConnectionString;
            String _userID = null;
            String _userPassword = null;
            String _userName = null;
            String _userEmail = null;
            String _userContact = null;
            String _userDOB = null;
            String _securityQ1 = null;
            String _securityQ1Ans = null;
            String _securityQ2 = null;
            String _securityQ2Ans = null;
            String _profile1Answer = null;
            String _profile2Answer = null;
            String _profile3Answer = null;
            String _profile4Answer = null;
            UserModel currentUser = null;
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

                    _userID = reader.GetString(0);
                    _userPassword = reader.GetString(1);
                    _userName = reader.GetString(2);
                    _userEmail = reader.GetString(3);
                    _userContact = reader.GetString(4);
                    _userDOB = reader.GetString(5);
                    _securityQ1 = reader.GetString(6);
                    _securityQ1Ans = reader.GetString(7);
                    _securityQ2 = reader.GetString(8);
                    _securityQ2Ans = reader.GetString(9);
                    _profile1Answer = reader.GetString(10);
                    _profile2Answer = reader.GetString(11);
                    _profile3Answer = reader.GetString(12);
                    _profile4Answer = reader.GetString(13);
                }
                Console.WriteLine(_userID + "|" + _userPassword + "|" + _userName + "|" + _userEmail + "|" + _userContact + "|" + _userDOB + "|" + _securityQ1 + "|" + _securityQ1Ans + "|" + _securityQ2 + "|" + _securityQ2Ans + "|" + _profile1Answer + "|" + _profile2Answer + "|" + _profile3Answer + "|" + _profile4Answer);
                currentUserModel = new UserModel(
                    _userID, _userPassword, _userName, _userEmail, _userContact, _userDOB, _securityQ1, _securityQ1Ans, _securityQ2, _securityQ2Ans, _profile1Answer, _profile2Answer, _profile3Answer, _profile4Answer);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {

                con.Close();
            }

            return currentUserModel;
        }


        public void saveToDatabase()
        {
            ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
            string connectionString = conSettings.ConnectionString;
            SqlConnection con = new SqlConnection();
            SqlCommand cmd;
            SqlDataReader reader;
            try
            {
                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("INSERT INTO [dbo].[test] (UserID, Password, Name, Email, ContactNo, DOB, SecurityQ1, Q1Ans, SecurityQ2, Q2Ans, Profile1 , Profile2, Profile3, Profile4) VALUES (@UserID, @Password, @Name, @Email, @ContactNo, @DOB, @SecurityQ1, @Q1Ans, @SecurityQ2, @Q2Ans, @Profile1 , @Profile2 , @Profile3 , @Profile4)", con);
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.Parameters.AddWithValue("@Password", userPassword);
                cmd.Parameters.AddWithValue("@Name", userName);
                cmd.Parameters.AddWithValue("@Email", userEmail);
                cmd.Parameters.AddWithValue("@ContactNo", userContact);
                cmd.Parameters.AddWithValue("@DOB", userDOB);
                cmd.Parameters.AddWithValue("@SecurityQ1", securityQ1);
                cmd.Parameters.AddWithValue("@Q1Ans", securityQ1Ans);
                cmd.Parameters.AddWithValue("@SecurityQ2", securityQ2);
                cmd.Parameters.AddWithValue("@Q2Ans", securityQ2Ans);
                cmd.Parameters.AddWithValue("@Profile1", profile1);
                cmd.Parameters.AddWithValue("@Profile2", profile2);
                cmd.Parameters.AddWithValue("@Profile3", profile3);
                cmd.Parameters.AddWithValue("@Profile4", profile4);



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

       


        public static void do2fa(string subject , string subjectBody , string destinationEmail)
        {
            Random rnd = new Random();
            int randomNumber = rnd.Next(1000, 9999);
            string code = randomNumber.ToString();
            UserModel.twoFAcode = code;
            Console.WriteLine(code);
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("nspjproject1718@gmail.com", "avantguard");
                MailMessage mail = new MailMessage();
                mail.To.Add(destinationEmail);
                mail.From = new MailAddress("nspjproject1718@gmail.com");
                mail.Subject = subject;
                mail.Body = subjectBody + code;
                client.Send(mail);
                //System.Windows.MessageBox.Show("An authentication code has been sent to your email.");
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public string getAccessApplicationPreference()
        {
            throw new NotImplementedException();
        }

        public string getLogInPreference(string username)
        {
            throw new NotImplementedException();
        }

        public string getLogOutPreference(string username)
        {
            throw new NotImplementedException();
        }

        public string getPrimaryWorkstationPreference()
        {
            throw new NotImplementedException();
        }

        public void saveAccessApplicationPreference(string accessApplicationPreference)
        {
            throw new NotImplementedException();
        }

        public void saveLogInPreference(string logInPreference)
        {
            throw new NotImplementedException();
        }

        public void saveLogOutPreference(string logOutPreference)
        {
            throw new NotImplementedException();
        }

        public void savePrimaryWorkstationPreference(string primaryWorkstationPreference)
        {
            throw new NotImplementedException();
        }

        public static void saveDateTimeOfUser(string userID, string connectionString, string loginTime, string date, string publicIP, string publicMAC)
        {

            Boolean canSave = checkLastLogin(userID, connectionString, loginTime, date);
            if (canSave == true)
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
            else
            {
                Console.WriteLine("Cannot save until 30 minutes have passed");
            }


        }

        private static Boolean checkLastLogin(string userID, string connectionString, string loginTime, string loginDate)
        {
            SqlConnection con;
            SqlCommand cmd;
            SqlDataReader reader;
            Boolean canSave = true;
            Console.WriteLine(connectionString);
            List<String> loginTimeList = new List<string>();
            con = new SqlConnection(connectionString);
            // string currentHostname = System.Environment.MachineName.ToString();
            con.Open();
            try
            {
                cmd = new SqlCommand("SELECT LoginTime FROM [dbo].[LogAnalysis] where UserID = '" + userID + "' and LoginDate = '" + loginDate + "' Order By LoginTime DESC", con);
                reader = cmd.ExecuteReader();
                if (reader == null)
                {

                }
                else
                {
                    while (reader.Read())
                    {
                        loginTimeList.Add(reader.GetString(0));
                    }
                }
                int counter = 0;
                double currentLoginTime = Convert.ToDouble(loginTime);
                if (loginTimeList.Count < 2)
                {
                    Console.WriteLine("List size is less than 2");
                    for (int i = 0; i < loginTimeList.Count(); i++)
                    {
                        double pastLoginTime = Convert.ToDouble(loginTimeList[i]);
                        string pastLoginTimeStr = convertTime(pastLoginTime);
                        pastLoginTime = Convert.ToDouble(pastLoginTimeStr);
                        Console.WriteLine(pastLoginTime);
                        if (currentLoginTime < pastLoginTime)
                        {
                            counter++;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("List size is more than 2");
                    for (int i = loginTimeList.Count - 2; i < loginTimeList.Count(); i++)
                    {
                        double pastLoginTime = Convert.ToDouble(loginTimeList[i]);
                        string pastLoginTimeStr = convertTime(pastLoginTime);
                        pastLoginTime = Convert.ToDouble(pastLoginTimeStr);
                        Console.WriteLine(pastLoginTime);
                        if (currentLoginTime < pastLoginTime)
                        {
                            counter++;
                        }
                    }
                }

                if (counter == 2)
                {
                    Console.WriteLine("Maximum amount of data entries in 30 minute span has been reached");
                    canSave = false;
                }
                else
                {
                    Console.WriteLine("Proceeding with data storage");
                    canSave = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                con.Close();


            }
            return canSave;
        }

        private static string convertTime(double pastLoginTime)
        {
            double endResult = pastLoginTime + 0.30;
            String time = endResult.ToString();
            var data = time.Split('.');
            string finaltime = null;
            Console.WriteLine(endResult);
            Console.WriteLine(data[1] + "Last");
            if(Convert.ToDouble(data[1]) >= 60)
            {
                double firstSet = Convert.ToDouble(data[0]) + 1;
                int secondSet = Convert.ToInt32(data[1]) - 60;
                string secondSetStr = null;
                if (secondSet < 10)
                {
                    secondSetStr = "0" + secondSet;
                }
                finaltime = firstSet + "." + secondSetStr;
            }
            else
            {
                pastLoginTime += 0.30;
                finaltime = pastLoginTime.ToString();
            }

            return finaltime;

        }

        public static void saveFollowUp(string userID, string connectionString , string status)
        {
            SqlConnection con;
            SqlCommand cmd;
            con = new SqlConnection(connectionString);
            // string currentHostname = System.Environment.MachineName.ToString();
            con.Open();
            try
            {
                cmd = new SqlCommand("INSERT INTO [dbo].[checkFollowUp] (UserID, followUp) VALUES (@UserID, @followUp)", con);
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.Parameters.AddWithValue("@followUp", status);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }

        public static void updateFollowUp(string userID , string connectionString , string status)
        {
            SqlConnection con;
            SqlCommand cmd;
            con = new SqlConnection(connectionString);
            // string currentHostname = System.Environment.MachineName.ToString();
            con.Open();
            try
            {
                cmd = new SqlCommand("UPDATE [dbo].[checkFollowUp] SET followUp = '" + status + "' where UserID = '" + userID + "'", con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                con.Close();
            }
          
        }

        public static void deleteDateTimeOfUser(string userID , string connectionString , string loginTime , string date)
        {
            SqlConnection con;
            SqlCommand cmd;
            con = new SqlConnection(connectionString);
            // string currentHostname = System.Environment.MachineName.ToString();
            con.Open();
            try
            {
               
               cmd = new SqlCommand("DELETE FROM [dbo].[LogAnalysis] where UserID = '" + userID + "' and LoginTime = '" + loginTime + "' and LoginDate = '" + date + "'", con);
               cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }

        public static string checkFollowUp(string userID, string connectionString)
        {
            SqlConnection con;
            SqlCommand cmd;
            SqlDataReader reader;
            con = new SqlConnection(connectionString);
            string value = null;
            //string currentHostname = System.Environment.MachineName.ToString();
            con.Open();
            try
            {
                cmd = new SqlCommand("SELECT *  FROM [dbo].[checkFollowUp] where UserID = '" + userID + "'", con);
                reader = cmd.ExecuteReader();
                if (reader == null)
                {
                    value = null;
                }
                else
                {

                  
                        while (reader.Read())
                        {
                            value = reader.GetString(1);
                        }
                    
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                con.Close();

            }
            return value;
        }
    }

   
}
