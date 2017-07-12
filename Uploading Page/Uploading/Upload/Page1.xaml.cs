using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Layout.Upload
{
    /// <summary>
    /// Interaction logic for UploadPage.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void uploadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fileName;
                string stringFormatOfFile;
                fileName = dlg.FileName; //Will be useful in the future when selecting files
                
                using (StreamReader streamReader = new StreamReader(fileName, Encoding.UTF8))
                {
                    stringFormatOfFile = streamReader.ReadToEnd();
                }

                Controllers.KeyController kc = new Controllers.KeyController();
                //kc.encrypt(stringFormatOfTheFile);
                

                //For debugging purposes
                fileName = fileName.Replace("C:\\Users\\SengokuMedaru\\Desktop\\", "");
                String testOutput = kc.encrypt(stringFormatOfFile);
                System.IO.File.WriteAllText(@"C:\\Users\\SengokuMedaru\\Desktop\\EncryptedText\\encrypted_" + fileName, testOutput);
                

                // 11.7.2017 Update
                // RSA has been used to encrypt files
                //
                // However, new problem has arose
                // RSA can only encrypt tiny files
                // Need to use symmetric algorithm to encrypt files instead
                // RSA can be used to encrypt the symmetric keys instead, if that makes any sense
                // - Sean
            }
        }
    }
}
