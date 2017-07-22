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
        public String _securityQ1;
        public String _securityQ1Ans;
        public String _securityQ2;
        public String _securityQ2Ans;

        public UserModel()
        {

        }

        public UserModel (String _userID, String _userPassword, String _userName, String _userEmail, String _userContact, String _userDOB, 
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

        public string getUserID()
        {
            return _userID;
        }

        public void setUserID(string _userID)
        {
            this._userID = _userID;
        }

        public string getUserPassword()
        {
            return _userPassword;
        }

        public void setUserPassword(string _userPassword)
        {
            this._userPassword = _userPassword;
        }

        public string getUserName()
        {
            return _userName;
        }

        public void setUserName(string _userName)
        {
            this._userName = _userName;
        }

        public string getUserEmail()
        {
            return _userEmail;
        }

        public void setUserEmail(string _userEmail)
        {
            this._userEmail = _userEmail;
        }

        public string getUserContact()
        {
            return _userContact;
        }

        public void setUserContact(string _userContact)
        {
            this._userContact = _userContact;
        }

        public string getUserDOB()
        {
            return _userDOB;
        }

        public void setUserDOB(string _userDOB)
        {
            this._userDOB = _userDOB;
        }

        public string getSecurityQ1()
        {
            return _securityQ1;
        }

        public void setSecurityQ1(string _securityQ1)
        {
            this._securityQ1 = _securityQ1;
        }

        public string getSecurityQ1Ans()
        {
            return _securityQ1Ans;
        }

        public void setSecurityQ1Ans(string _securityQ1Ans)
        {
            this._securityQ1Ans = _securityQ1Ans;
        }

        public string getSecurityQ2()
        {
            return _securityQ2;
        }

        public void setSecurityQ2(string _securityQ2)
        {
            this._securityQ2 = _securityQ2;
        }

        public string getSecurityQ2Ans()
        {
            return _securityQ2Ans;
        }

        public void setSecurityQ2Ans(string _securityQ2Ans)
        {
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
