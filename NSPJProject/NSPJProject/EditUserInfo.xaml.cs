using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
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
using WpfApp1.Model1;
using WpfApp1.ProfilePages;

namespace NSPJProject
{
    /// <summary>
    /// Interaction logic for EditUserInfo.xaml
    /// </summary>
    public partial class EditUserInfo : Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;
        string profile1;
        string profile2;
        string profile3;
        string profile4;
        string userpassword;
        string connectionString;
        string userID; 
        public EditUserInfo()
        {
            InitializeComponent();
            string selected_UserID = (App.Current as App).LoginUserID;
            
            ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
            connectionString = conSettings.ConnectionString;
            try
            {
                con = new SqlConnection(connectionString);
                con.Open();

                UserModel.UserModel cm = UserModel.UserModel.retrieveUserFromDatabase("Demo1");
                //cmd = new SqlCommand("select * from [dbo].[test] where UserID = '" + selected_UserID + "'", con);
                cmd = new SqlCommand("select * from [dbo].[test] where UserID = 'Demo1'", con);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine(" | UserID : " + reader.GetString(0) + " | Password : " + reader.GetString(1) + " | Name : " + reader.GetString(2) + " | Email : " + reader.GetString(3) + " | ContactNo : " + reader.GetString(4) + " | DOB : " + reader.GetString(5) + " | SecurityQ1 : " + reader.GetString(6) + " | Q1Ans : " + reader.GetString(7) + " | SecurityQ2 : " + reader.GetString(8) + " | Q2Ans : " + reader.GetString(9));
                    (App.Current as App).UserID = reader.GetString(0);
                    (App.Current as App).UserPassword = reader.GetString(1);
                    (App.Current as App).UserName = reader.GetString(2);
                    (App.Current as App).UserEmail = reader.GetString(3);
                    (App.Current as App).UserContact = reader.GetString(4);
                    (App.Current as App).UserDOB = reader.GetString(5);
                    (App.Current as App).SecurityQ1 = reader.GetString(6);
                    (App.Current as App).Q1Ans = reader.GetString(7);
                    (App.Current as App).SecurityQ2 = reader.GetString(8);
                    (App.Current as App).Q2Ans = reader.GetString(9);
                    userpassword = reader.GetString(1);
                    Console.WriteLine(cm.profile1);
                    Console.WriteLine(cm.profile2);
                     userID = reader.GetString(0);
                    string preference1 = cm.profile1;
                    string preference2 = cm.profile2;
                    string preference3 = cm.profile3;
                    string preference4 = cm.profile4;
                    setProfile1Preference(preference1);
                    setProfile2Preference(preference2);
                    setProfile3Preference(preference3);
                    setProfile4Preference(preference4);
                    ChangeInfoName.Text = reader.GetString(2);
                    ChangeInfoQ1.Text = reader.GetString(6);
                    ChangeInfoQ1Ans.Text = reader.GetString(7);
                    ChangeInfoQ2.Text = reader.GetString(8);
                    ChangeInfoQ2Ans.Text = reader.GetString(9);
                   
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
        }

        private void setProfile4Preference(string preference)
        {
            if (preference.Contains("a"))
            {
                option1d.IsChecked = true;
            }
            if (preference.Contains("b"))
            {
                option2d.IsChecked = true;
            }
            if (preference.Contains("c"))
            {
                option3d.IsChecked = true;
            }
            if(preference.Contains("d"))
            {
                option4d.IsChecked = true;
            }
            if(preference.Contains("e"))
            {
                option5d.IsChecked = true;
            }
        }

        private void setProfile3Preference(string preference)
        {
            if (preference.Equals("a"))
            {
                option1c.IsChecked = true;
            }
            if (preference.Equals("b"))
            {
                option2c.IsChecked = true;
            }
            if (preference.Equals("c"))
            {
                option3c.IsChecked = true;
            }
        }

        private void setProfile2Preference(string preference)
        {
            
            if (preference.Equals("a"))
            {
                option1b.IsChecked = true;
            }
            if (preference.Equals("b"))
            {
                option2b.IsChecked = true;
            }
            if (preference.Equals("c"))
            {
                option3b.IsChecked = true;
            }
            if (preference.Equals("d"))
            {
                option4b.IsChecked = true;
            }
        }

        private void setProfile1Preference(string preference)
        {
           if(preference.Equals("a"))
            {
                option1a.IsChecked = true;
            }
            if (preference.Equals("b"))
            {
                option2a.IsChecked = true;
            }
            if (preference.Equals("c"))
            {
                option3a.IsChecked = true;
            }
            if (preference.Equals("d"))
            {
                option4a.IsChecked = true;
            }

        }

        private void ChangeInfoConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            //string selected_UserID = (App.Current as App).LoginUserID;

            //try
            //{
            //    ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
            //    string connectionString = conSettings.ConnectionString;

            //    con = new SqlConnection(connectionString);
            //    con.Open();
            //    cmd = new SqlCommand("UPDATE [dbo].[test] SET Name = '" + ChangeInfoName.Text + "', Email = '"  + "',  ContactNo = '" + "', DOB = '"  + "', SecurityQ1 = '" + ChangeInfoQ1.Text + "', Q1Ans = '" + ChangeInfoQ1Ans.Text + "', SecurityQ2 = '" + ChangeInfoQ2.Text + "', Q2Ans = '" + ChangeInfoQ2Ans.Text + "' WHERE UserID = '" + selected_UserID + "'", con);
            //    cmd.ExecuteNonQuery();

            //    MessageBox.Show("Successfully save changes.");

            //}
            //catch (Exception ex)
            //{
            //    System.Windows.MessageBox.Show(ex.Message);
            //}
            //finally
            //{

            //    con.Close();
            //}
        }

        private void updateDatebaseWithoutPassword(string userID, string question1Text, string question1TextAns, string question2Text, string question2TextAns, string profile1, string profile2, string profile3, string profile4, string connectionString)
        {

            con = new SqlConnection(connectionString);
            con.Open();
            cmd = new SqlCommand("UPDATE [dbo].[test] SET Name = '" + ChangeInfoName.Text + "' , Email = '" + "',  ContactNo = '" + "', DOB = '" + "', SecurityQ1 = '" + ChangeInfoQ1.Text + "', Q1Ans = '" + ChangeInfoQ1Ans.Text + "', SecurityQ2 = '" + ChangeInfoQ2.Text + "', Q2Ans = '" + ChangeInfoQ2Ans.Text + "' WHERE UserID = '" + userID + "'", con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

       

        private void ChangeNameButton_Click(object sender, RoutedEventArgs e)
        {
            string selected_UserID = (App.Current as App).LoginUserID;

            try
            {
                ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
                string connectionString = conSettings.ConnectionString;

                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("UPDATE [dbo].[test] SET Name = '" + ChangeInfoName.Text + "'  WHERE UserID = '" + selected_UserID + "'", con);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Successfully change name.");

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

        private void ChangeQ1Button_Click(object sender, RoutedEventArgs e)
        {
            string selected_UserID = (App.Current as App).LoginUserID;

            try
            {
                ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
                string connectionString = conSettings.ConnectionString;

                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("UPDATE [dbo].[test] SET SecurityQ1 = '" + ChangeInfoQ1.Text + "'  WHERE UserID = '" + selected_UserID + "'", con);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Successfully change security question.");

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

        private void ChangeQ1AnsButton_Click(object sender, RoutedEventArgs e)
        {
            string selected_UserID = (App.Current as App).LoginUserID;

            try
            {
                ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
                string connectionString = conSettings.ConnectionString;

                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("UPDATE [dbo].[test] SET Q1Ans = '" + ChangeInfoQ1Ans.Text + "'  WHERE UserID = '" + selected_UserID + "'", con);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Successfully change security question answer.");

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

        private void ChangeQ2Button_Click(object sender, RoutedEventArgs e)
        {
            string selected_UserID = (App.Current as App).LoginUserID;

            try
            {
                ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
                string connectionString = conSettings.ConnectionString;

                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("UPDATE [dbo].[test] SET SecurityQ2 = '" + ChangeInfoQ2.Text + "'  WHERE UserID = '" + selected_UserID + "'", con);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Successfully change security question.");

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

        private void ChangeQ2AnsButton_Click(object sender, RoutedEventArgs e)
        {
            string selected_UserID = (App.Current as App).LoginUserID;

            try
            {
                ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
                string connectionString = conSettings.ConnectionString;

                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("UPDATE [dbo].[test] SET Q2Ans = '" + ChangeInfoQ1.Text + "'  WHERE UserID = '" + selected_UserID + "'", con);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Successfully change security question answer.");

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

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            runSavePassword();
        }

        private void runSavePassword()
        {
            if (ChangeInfoNewPassword.Password != ChangeInfoReNewPassword.Password)
            {
                MessageBox.Show("New password do not match.");
            }
            else
            {
                LoginPage LP = new LoginPage();
                ChangeInfoCurrentPassword.Password = LP.GetSha512FromString(ChangeInfoCurrentPassword.Password);
                ChangeInfoNewPassword.Password = LP.GetSha512FromString(ChangeInfoNewPassword.Password);

                try
                {
                    string selected_UserID = (App.Current as App).LoginUserID;

                    ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
                    string connectionString = conSettings.ConnectionString;

                    con = new SqlConnection(connectionString);
                    con.Open();
                    cmd = new SqlCommand("select * from [dbo].[test] where Password = '" + ChangeInfoCurrentPassword.Password + "' and UserID = '" + selected_UserID + "'", con);
                    reader = cmd.ExecuteReader();

                    int count = 0;
                    while (reader.Read())
                    {
                        count += 1;
                    }
                    if (count == 1)
                    {
                        try
                        {
                            ConnectionStringSettings conSettings1 = ConfigurationManager.ConnectionStrings["connString"];
                            string connectionString1 = conSettings1.ConnectionString;

                            con = new SqlConnection(connectionString1);
                            con.Open();
                            cmd = new SqlCommand("UPDATE [dbo].[test] SET Password = '" + ChangeInfoNewPassword.Password + "' WHERE UserID = '" + selected_UserID + "'", con);
                            cmd.ExecuteNonQuery();

                        }
                        catch (Exception ex)
                        {
                            System.Windows.MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            con.Close();
                        }

                        MessageBox.Show("You have changed your password.");
                        ChangeInfoCurrentPassword.Clear();
                        ChangeInfoNewPassword.Clear();
                        ChangeInfoReNewPassword.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect current password.");
                        ChangeInfoCurrentPassword.Clear();
                        ChangeInfoNewPassword.Clear();
                        ChangeInfoReNewPassword.Clear();
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
            }
        }

        private void PopUpPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPasswordLabel.Visibility == Visibility.Visible)
            {
                CurrentPasswordLabel.Visibility = Visibility.Collapsed;
                NewPasswordLabel.Visibility = Visibility.Collapsed;
                ReNewPasswordLabel.Visibility = Visibility.Collapsed;

                ChangeInfoCurrentPassword.Visibility = Visibility.Collapsed;
                ChangeInfoNewPassword.Visibility = Visibility.Collapsed;
                ChangeInfoReNewPassword.Visibility = Visibility.Collapsed;

                ChangePasswordButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                CurrentPasswordLabel.Visibility = Visibility.Visible;
                NewPasswordLabel.Visibility = Visibility.Visible;
                ReNewPasswordLabel.Visibility = Visibility.Visible;

                ChangeInfoCurrentPassword.Visibility = Visibility.Visible;
                ChangeInfoNewPassword.Visibility = Visibility.Visible;
                ChangeInfoReNewPassword.Visibility = Visibility.Visible;


                ChangePasswordButton.Visibility = Visibility.Visible;
            }
        }

        private void EditProfileCreation_Click(object sender, RoutedEventArgs e)
        {
            if(option1a.Visibility == Visibility.Collapsed)
            {
                Profile1Question.Visibility = Visibility.Visible;
                profilePanel.Visibility = Visibility.Visible;
                option1a.Visibility = Visibility.Visible;
                option2a.Visibility = Visibility.Visible;
                option3a.Visibility = Visibility.Visible;
                option4a.Visibility = Visibility.Visible;

                Profile2Question.Visibility = Visibility.Visible;
                option1b.Visibility = Visibility.Visible;
                option2b.Visibility = Visibility.Visible;
                option3b.Visibility = Visibility.Visible;
                option4b.Visibility = Visibility.Visible;

                Profile3Question.Visibility = Visibility.Visible;
                option1c.Visibility = Visibility.Visible;
                option2c.Visibility = Visibility.Visible;
                option3c.Visibility = Visibility.Visible;

                Profile4Question.Visibility = Visibility.Visible;
                option1d.Visibility = Visibility.Visible;
                option2d.Visibility = Visibility.Visible;
                option3d.Visibility = Visibility.Visible;
                option4d.Visibility = Visibility.Visible;
                option5d.Visibility = Visibility.Visible;

            }
            else
            {
                Profile1Question.Visibility = Visibility.Collapsed;
                profilePanel.Visibility = Visibility.Collapsed;

                option1a.Visibility = Visibility.Collapsed;
                option2a.Visibility = Visibility.Collapsed;
                option3a.Visibility = Visibility.Collapsed;
                option4a.Visibility = Visibility.Collapsed;

                Profile2Question.Visibility = Visibility.Collapsed;
                option1b.Visibility = Visibility.Collapsed;
                option2b.Visibility = Visibility.Collapsed;
                option3b.Visibility = Visibility.Collapsed;
                option4b.Visibility = Visibility.Collapsed;

                Profile3Question.Visibility = Visibility.Collapsed;
                option1c.Visibility = Visibility.Collapsed;
                option2c.Visibility = Visibility.Collapsed;
                option3c.Visibility = Visibility.Collapsed;

                Profile4Question.Visibility = Visibility.Collapsed;
                option1d.Visibility = Visibility.Collapsed;
                option2d.Visibility = Visibility.Collapsed;
                option3d.Visibility = Visibility.Collapsed;
                option4d.Visibility = Visibility.Collapsed;
                option5d.Visibility = Visibility.Collapsed;
            }
        }

        private void ToggleCheckOption1(object sender, RoutedEventArgs e)
        {
        }
        private void ToggleCheckOption2(object sender, RoutedEventArgs e)
        {
        }
        private void ToggleCheckOption3(object sender, RoutedEventArgs e)
        {
        }
        private void ToggleCheckOption4(object sender, RoutedEventArgs e)
        {
        }

        private void ConfirmEditProfile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ConfirmProfile(object sender, RoutedEventArgs e)
        {


            if (option1a.IsChecked == true)
            {
                profile1 = "a";
            }
            if (option2a.IsChecked == true)
            {
                profile1 = "b";
            }
            if (option3a.IsChecked == true)
            {
                profile1 = "c";
            }
            if (option4a.IsChecked == true)
            {
                profile1 = "d";
            }

            if (option1b.IsChecked == true)
            {
                profile2 = "a";
            }
            if (option2b.IsChecked == true)
            {
                profile2 = "b";
            }
            if (option3b.IsChecked == true)
            {
                profile2 = "c";
            }
            if (option4b.IsChecked == true)
            {
                profile2 = "d";
            }

            if (option1c.IsChecked == true)
            {
                profile3 = "a";
            }
            if (option2c.IsChecked == true)
            {
                profile3 = "b";
            }
            if (option3c.IsChecked == true)
            {
                profile3 = "c";
            }



            if (option1d.IsChecked == true)
            {
                profile4 = profile4 + "a";
            }
            if (option2d.IsChecked == true)
            {
                profile4 = profile4 + "b";
            }
            if (option3d.IsChecked == true)
            {
                profile4 = profile4 + "c";
            }
            if (option4d.IsChecked == true)
            {
                profile4 = profile4 + "d";
            }
            if (option5d.IsChecked == true)
            {
                profile4 = profile4 + "e";
            }

            MessageBox.Show(profile4);
            updateFullDatabase(userID, profile1, profile2, profile3, profile4);
            profile1 = null;
            profile2 = null;
            profile3 = null;
            profile4 = null;


        }

        private void updateFullDatabase(string userID, string Profile1, string Profile2 , string Profile3 , string Profile4)
        {

            con = new SqlConnection(connectionString);
            con.Open();
            try { 
            cmd = new SqlCommand("UPDATE [dbo].[test] SET Profile1 = '" + Profile1 + "' , Profile2 = '" + Profile2 + "' , Profile3 = '" + Profile3 + "' , Profile4 = '" + Profile4 + "' WHERE UserID = '" + userID + "'", con);
            cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            con.Close();
        }
    }
}
