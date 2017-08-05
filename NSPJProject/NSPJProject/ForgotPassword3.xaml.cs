using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
    /// Interaction logic for ForgotPassword3.xaml
    /// </summary>
    public partial class ForgotPassword3 : Page
    {
        public ForgotPassword3()
        {
            InitializeComponent();

        }

        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;

        private static readonly string _allowedCharacters = "abcdfghjklmnpqrstvxz0123456789";

        private void ForgotPassword3NextButton_Click(object sender, RoutedEventArgs e)
        {
            string selected_ForgotPasswordCode = (App.Current as App).ForgotPasswordCode;
            if (ForgotPasswordCodeTextBox.Text == selected_ForgotPasswordCode)
            {

                const int from = 0;
                int to = _allowedCharacters.Length;
                Random r = new Random();

                StringBuilder qs = new StringBuilder();
                for (int i = 0; i < 8; i++)
                {
                    qs.Append(_allowedCharacters.Substring(r.Next(from, to), 1));
                }

                LoginPage LP = new LoginPage();

                string selected_ForgotPasswordEmail = (App.Current as App).ForgotPasswordEmail;

                try
                {
                    SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                    client.EnableSsl = true;
                    client.Timeout = 10000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("nspjproject1718@gmail.com", "avantguard");
                    MailMessage mail = new MailMessage();
                    mail.To.Add(selected_ForgotPasswordEmail);
                    mail.From = new MailAddress("nspjproject1718@gmail.com");
                    mail.Subject = "This is an email.";
                    mail.Body = "Your new password is " + qs.ToString();
                    client.Send(mail);
                    System.Windows.MessageBox.Show("Your new password has been sent to your email. You are encouraged to change your password.");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }

                ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
                string connectionString = conSettings.ConnectionString;

                try
                {
                    ConnectionStringSettings conSettings1 = ConfigurationManager.ConnectionStrings["connString"];
                    string connectionString1 = conSettings1.ConnectionString;

                    con = new SqlConnection(connectionString1);
                    con.Open();
                    cmd = new SqlCommand("UPDATE [dbo].[test] SET Password = '" + LP.GetSha512FromString(qs.ToString()) + "' WHERE Email = '" + selected_ForgotPasswordEmail + "'", con);
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

                this.NavigationService.Navigate(new Uri(@"LoginPage.xaml", UriKind.RelativeOrAbsolute));
            }
            else
            {
                MessageBox.Show("Invalid authentication code!");
            }
        }

        private void ForgotPassword3BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"ForgotPassword2.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
