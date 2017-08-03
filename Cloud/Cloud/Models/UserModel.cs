using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
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
        public static UserModel _currentUserModel;
        public static string _currentUserID;
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
            String _securityQ1, String _securityQ1Ans, String _securityQ2, String _securityQ2Ans , String _Profile1 , String _Profile2 , String _Profile3 , String _Profile4)
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
            set { _currentUserModel = value;  }
        }

        public static string currentUserID
        {
            get { return _currentUserID; }
            set { _currentUserID = value; }
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
    }
}
