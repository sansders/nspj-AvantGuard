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
                Controllers.KeyController kc = new Controllers.KeyController();


                System.Text.UTF8Encoding UTF = new System.Text.UTF8Encoding();
                string ivReader = System.IO.File.ReadAllText(@"C:\\Users\\SengokuMedaru\\Desktop\\keys\\IV.txt");
                string encryptedSymmetricKeyReader = System.IO.File.ReadAllText(@"C:\\Users\\SengokuMedaru\\Desktop\\keys\\encryptedSymmetricKey.txt");
                string fileName;
                string stringFormatOfFile;
                fileName = dlg.FileName; //Will be useful in the future when selecting files
                
                using (StreamReader streamReader = new StreamReader(fileName, Encoding.UTF8))
                {
                    stringFormatOfFile = streamReader.ReadToEnd();
                }

                
                
                byte[] byteFormatOfFile = UTF.GetBytes(stringFormatOfFile);
                byte[] byteFormatOfIV = UTF.GetBytes(ivReader);
                byte[] encryptedSymmetricKey = UTF.GetBytes(encryptedSymmetricKeyReader);
                byte[] decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                byte[] cipherText = kc.symmetricEncryption(stringFormatOfFile, decryptedSymmetricKey, byteFormatOfIV);
                
                
                //For debugging purposes
                fileName = fileName.Replace("C:\\Users\\SengokuMedaru\\Desktop\\", "");
                String testOutput = UTF.GetString(cipherText);
                System.IO.File.WriteAllText(@"C:\\Users\\SengokuMedaru\\Desktop\\EncryptedText\\encrypted_" + fileName, testOutput);
                
            }
        }
    }
}
