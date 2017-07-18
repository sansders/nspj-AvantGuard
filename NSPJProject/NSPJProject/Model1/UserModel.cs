using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model1;

namespace NSPJProject.Model1 
{
    public class UserModel : ProfileInterface.ProfileCreation
    {
        public String _userID;
        public String _userPassword;
        public String _userName;
        public String _userEmail;
        public String _userContact;
        public String _userDOB;
        public String _userGender;
        public String _securityQ1;
        public String _securityQ1Ans;
        public String _securityQ2;
        public String _securityQ2Ans;
       

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

        public string userGender
        {
            get { return _userGender; }
            set { _userGender = value; }
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
