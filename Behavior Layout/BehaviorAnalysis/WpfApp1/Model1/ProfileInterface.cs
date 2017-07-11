using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model1
{
    class ProfileInterface
    {
        public interface ProfileCreation 
        {

            //How to use interface? Go to your model that you want to implement and beside the class do : ProfileInterface.ProfileCreation 

            //For my profile creation page 1
             void saveLogInPreference(string logInPreference);
             string getLogInPreference(string username);

            //For my profile creation page 2
            void saveLogOutPreference(string logOutPreference);
            string getLogOutPreference(string username);

            //For my profile creation page 3
            void savePrimaryWorkstationPreference(string primaryWorkstationPreference);
            string getPrimaryWorkstationPreference();

            //For my profile creation page 4
            void saveAccessApplicationPreference(string accessApplicationPreference);
            string getAccessApplicationPreference();

            //For my profile creation page 5
            //Still DK 


        }
    }
}
