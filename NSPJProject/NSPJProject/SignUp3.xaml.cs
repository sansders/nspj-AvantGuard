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
                client.Credentials = new NetworkCredential("limxj90@gmail.com", "sistarbora19");
                MailMessage mail = new MailMessage();
                mail.To.Add(txtContact.Text);
                mail.From = new MailAddress("limxj90@gmail.com");
                mail.Subject = "This is an email";
                mail.Body = "This is the content.";
                client.Send(mail);
                System.Windows.MessageBox.Show("Successfully send email.");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }

            //string txtMessage = "Hi.";
            //using (System.Net.WebClient client = new System.Net.WebClient())
            //{
            //    try
            //    {
            //        string url = "http://smsc.vianett.no/v3/send.ashx?" +
            //            "src=" + txtContact.Text + "&" +
            //            "dst=" + txtContact.Text + "&" +
            //            "msg=" + System.Web.HttpUtility.UrlEncode(txtMessage, System.Text.Encoding.GetEncoding("ISO-8859-1"));
            //        string result = client.DownloadString(url);
            //        if (result.Contains("OK"))
            //        {
            //            System.Windows.Forms.MessageBox.Show("Your message has been successfully sent.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }

            //        else
            //        {
            //            System.Windows.Forms.MessageBox.Show("Message send failure.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
                      
            //    }
            //    catch (Exception ex)
            //    {
            //        System.Windows.Forms.MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
        }

        
        
    }
}
