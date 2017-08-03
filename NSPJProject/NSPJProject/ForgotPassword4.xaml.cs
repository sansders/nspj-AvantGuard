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
    /// Interaction logic for ForgotPassword4.xaml
    /// </summary>
    public partial class ForgotPassword4 : Page
    {
        public ForgotPassword4()
        {
            InitializeComponent();
        }

        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;

        private void ForgotPassword4BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"LoginPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ForgotPassword4NextButton_Click(object sender, RoutedEventArgs e)
        {
            LoginPage LP = new LoginPage();
            OldPasswordTextBox.Password = LP.GetSha512FromString(OldPasswordTextBox.Password);
            NewPasswordTextBox.Password = LP.GetSha512FromString(NewPasswordTextBox.Password);

            string selected_ForgotPasswordEmail = (App.Current as App).ForgotPasswordEmail;

            try
            {
                ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
                string connectionString = conSettings.ConnectionString;

                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("select * from [dbo].[test] where Password = '" + OldPasswordTextBox.Password + "' and Email = '" + selected_ForgotPasswordEmail + "'", con);
                reader = cmd.ExecuteReader();

                int count = 0;
                while (reader.Read())
                {
                    count += 1;
                }
                if (count == 1)
                {
                    MessageBox.Show("Correct old password.");

                    try
                    {
                        ConnectionStringSettings conSettings1 = ConfigurationManager.ConnectionStrings["connString"];
                        string connectionString1 = conSettings.ConnectionString;

                        con = new SqlConnection(connectionString);
                        con.Open();
                        cmd = new SqlCommand("UPDATE [dbo].[test] SET Password = '" + NewPasswordTextBox.Password + "' WHERE Email = '" + selected_ForgotPasswordEmail + "'" , con);
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
                }
                else
                {
                    MessageBox.Show("Incorrect old password.");
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

            OldPasswordTextBox.Clear();
        }
    }
}
