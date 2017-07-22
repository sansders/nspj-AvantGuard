using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NSPJProject
{
    /// <summary>
    /// Interaction logic for SignUp3.xaml
    /// </summary>
    public partial class SignUp3 : Page
    {
        public SignUp3()
        {
            InitializeComponent();
        }


        private void VerifyButton_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    MailMessage message = new MailMessage();
            //    message.To.Add(txtContact.Text + "@m1.com.sg");
            //    message.From = new MailAddress("nspjproject1718@gmail.com", "App"); //See the note afterwards...
            //    message.Body = "This is your cell phone. How was your day?";

            //    SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            //    smtp.EnableSsl = true;
            //    smtp.Port = 587;
            //    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //    smtp.Credentials = new NetworkCredential("nspjproject1718@gmail.com", "avantguard");



            //    smtp.Send(message);
            //    System.Windows.MessageBox.Show("Message successfully send.");
            //}

            //catch (Exception ex)
            //{
            //    System.Windows.MessageBox.Show(ex.Message);
            //}


            Random rnd = new Random();
            int code = rnd.Next(1000, 9999);
            string codeString = code.ToString();

            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("nspjproject1718@gmail.com", "avantguard");
                MailMessage mail = new MailMessage();
                mail.To.Add(txtContact.Text);
                mail.From = new MailAddress("nspjproject1718@gmail.com");
                mail.Subject = "This is an email.";
                mail.Body = "Your verification code is " + codeString;
                client.Send(mail);
                System.Windows.MessageBox.Show("Successfully send email.");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
    }
}
