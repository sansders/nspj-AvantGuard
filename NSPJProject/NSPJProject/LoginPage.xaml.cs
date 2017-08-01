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

            ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
            string connectionString = conSettings.ConnectionString;

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
                    cmd = new SqlCommand("INSERT INTO [dbo].[LogAnalysis] (UserID, LoginTime, LoginDate) VALUES (@UserID, @LoginTime, @LoginDate", con);
                    cmd.Parameters.AddWithValue("@UserID", UserIDTextBox.Text);
                    cmd.Parameters.AddWithValue("@LoginTime", DateTime.Now.ToString("HH.mm"));
                    cmd.Parameters.AddWithValue("@LoginDate", DateTime.Now.ToShortDateString());

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
