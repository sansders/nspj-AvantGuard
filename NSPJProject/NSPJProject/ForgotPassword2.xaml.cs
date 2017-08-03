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
    /// Interaction logic for ForgotPassword2.xaml
    /// </summary>
    public partial class ForgotPassword2 : Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;

        public ForgotPassword2()
        {
            InitializeComponent();
            string selected_SecurityQ1 = (App.Current as App).SecurityQ1;
            Q1Label.Content = selected_SecurityQ1;

            string selected_SecurityQ2 = (App.Current as App).SecurityQ2;
            Q2Label.Content = selected_SecurityQ2;
        }

        private void ForgotPassword2BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"ForgotPassword1.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ForgotPassword2NextButton_Click(object sender, RoutedEventArgs e)
        {
            string selected_ForgotPasswordEmail = (App.Current as App).ForgotPasswordEmail;

            try
            {
                ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
                string connectionString = conSettings.ConnectionString;

                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("select * from [dbo].[test] where Q1Ans = '" + SecurityQ1Ans.Text + "' and Q2Ans = '" + SecurityQ2Ans.Text + "'", con);
                reader = cmd.ExecuteReader();

                int count = 0;
                while (reader.Read())
                {
                    count += 1;
                }
                if (count == 1)
                {
                    Random rnd = new Random();
                    int code = rnd.Next(1000, 9999);
                    string ForgotPasswordCode = code.ToString();

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
                        mail.Subject = "Change Password Request";
                        mail.Body = "Your authentication code is " + ForgotPasswordCode;
                        client.Send(mail);
                        System.Windows.MessageBox.Show("An authentication code has been sent to your email.");
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show(ex.Message);
                    }

                    Console.WriteLine(ForgotPasswordCode);
                    (App.Current as App).ForgotPasswordCode = ForgotPasswordCode;
                    this.NavigationService.Navigate(new Uri(@"ForgotPassword3.xaml? key1=" + SecurityQ1Ans.Text, UriKind.RelativeOrAbsolute));
                }

                else
                {
                    MessageBox.Show("Please make sure that all answers are correct.");
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
}
