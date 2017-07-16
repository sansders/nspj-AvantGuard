using System;
using System.Collections.Generic;
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
                mail.Body = "This is the content.";
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
