using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
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
        public LoginPage()
        {
            InitializeComponent();
        }

        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;

        public string GetSha512FromString(string strData)
        {
            //strData.Insert(0, "XYZ");
            var message = Encoding.ASCII.GetBytes(strData.Insert(2, "026620758babadb008ee7b98e1bb07351f08d49228c15f6f31c4ee75cb9a26f5079b81c01f14f78cf5f9639e49d7319ee3c3fcc1f94e686b8d605c93f2ab9fb4"));
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
            MessageBox.Show(GetSha512FromString(PasswordTextBox.Password));

            ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
            string connectionString = conSettings.ConnectionString;

            try
            {
                con = new SqlConnection(connectionString);
                con.Open();
                //cmd = new SqlCommand("SELECT Username , Name , ContactNo , Password FROM [dbo].[UserAcc]", con);
                cmd = new SqlCommand("select * from dbo.test where UserID = '" + UserIDTextBox.Text + "' and Password = '" + PasswordTextBox.Password + "'", con);
                reader = cmd.ExecuteReader();


                int count = 0;
                while (reader.Read())
                {
                    count += 1;
                    Console.WriteLine(" | Username : " + reader.GetString(0) + " | Name : " + reader.GetString(1) + " | Contact No : " + reader.GetString(2) + " | Password : " + reader.GetString(3));
                }

                if (count == 1)
                {
                    MessageBox.Show("Successful Login.");

                }

                else if (count > 0)
                {
                    MessageBox.Show("Duplicate userid and password.");
                }

                else
                {
                    MessageBox.Show("Invalid userid or password");
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
