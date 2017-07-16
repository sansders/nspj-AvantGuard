using System;
using System.Collections.Generic;
using System.Linq;
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

            string txtMessage = "Hi.";
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                try
                {
                    string url = "http://smsc.vianett.no/v3/send.ashx?" +
                        "src=" + txtContact.Text + "&" +
                        "dst=" + txtContact.Text + "&" +
                        "msg=" + System.Web.HttpUtility.UrlEncode(txtMessage, System.Text.Encoding.GetEncoding("ISO-8859-1"));
                    string result = client.DownloadString(url);
                    if (result.Contains("OK"))
                    {
                        System.Windows.Forms.MessageBox.Show("Your message has been successfully sent.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Message send failure.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                      
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
