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

        public EditUserInfo()
        {
            InitializeComponent();
            string selected_UserID = (App.Current as App).LoginUserID;

            ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
            string connectionString = conSettings.ConnectionString;

            try
            {
                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("select * from [dbo].[test] where UserID = '" + selected_UserID + "'", con);
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

                    ChangeInfoName.Text = reader.GetString(2);
                    ChangeInfoEmail.Text = reader.GetString(3);
                    ChangeInfoContact.Text = reader.GetString(4);
                    ChangeInfoDOB.Text = reader.GetString(5);
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

        private void ChangeInfoButton_Click(object sender, RoutedEventArgs e)
        {
            string selected_UserID = (App.Current as App).LoginUserID;

            try
            {
                ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
                string connectionString = conSettings.ConnectionString;

                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("UPDATE [dbo].[test] SET Name = '" + ChangeInfoName.Text + "', Email = '" + ChangeInfoEmail.Text + "',  ContactNo = '" + ChangeInfoContact.Text + "', DOB = '" + ChangeInfoDOB.Text + "', SecurityQ1 = '" + ChangeInfoQ1.Text + "', Q1Ans = '" + ChangeInfoQ1Ans.Text + "', SecurityQ2 = '" + ChangeInfoQ2.Text + "', Q2Ans = '" + ChangeInfoQ2Ans.Text + "' WHERE UserID = '" + selected_UserID + "'", con);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Successfully save changes.");

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
}
