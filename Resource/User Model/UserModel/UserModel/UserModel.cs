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
                cmd = new SqlCommand("INSERT INTO [dbo].[test] (UserID, Password, Name, Email, ContactNo, DOB, SecurityQ1, Q1Ans, SecurityQ2, Q2Ans) VALUES (@UserID, @Password, @Name, @Email, @ContactNo, @DOB, @SecurityQ1, @Q1Ans, @SecurityQ2, @Q2Ans)", con);
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
