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
using System.Security.Cryptography;

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

        public string hash;
        public string stringComputedHash;

        private void userHashInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Obtains hash value from the text box
            hash = new TextRange(userHashInput.Document.ContentStart, userHashInput.Document.ContentEnd).Text.ToUpper();
        }

        private string stringUserHashInput(System.Windows.Controls.RichTextBox rtb)
        {
            TextRange textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            string thisHash = textRange.Text.ToUpper();
            try
            {
                thisHash = thisHash.Substring(0, 40);
            }
            catch (Exception e)
            {
                
            }
            return thisHash;
        }

        private void uploadButton_Click(object sender, RoutedEventArgs e)
        {
        
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Controllers.KeyController kc = new Controllers.KeyController();
                             
                string fileName;
                string stringFormatOfFile;
                fileName = dlg.FileName; //Will be useful in the future when selecting files

                using (StreamReader streamReader = new StreamReader(fileName, Encoding.Unicode))
                {
                    stringFormatOfFile = streamReader.ReadToEnd();
                }
           
                byte[] byteFormatOfFile = Encoding.Unicode.GetBytes(stringFormatOfFile);
                Console.WriteLine("Gets bytes of file");

         

                //Hash Computation
                SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
                byte[] byteComputedHash;

                using (FileStream stream = File.OpenRead(fileName))
                {
                    SHA1Managed sha = new SHA1Managed();
                    byteComputedHash = sha.ComputeHash(stream);
                }
                Controllers.HashController hashController = new Controllers.HashController();
                stringComputedHash = hashController.HexStringFromBytes(byteComputedHash).ToUpper();

                //Important line to ensure user inputted hash stays at no more than 40 chars
                hash = stringUserHashInput(userHashInput);
                Console.WriteLine(hash);



                //Actions to take if hash not equals to computed hash
                if (!(hash.Equals(stringComputedHash)))
                {
                    //Do stuff:
                    //Stuff like an error message box pop up (done)
                    //Clear the userInputHash box (done)
                    //Break from the current method (done)
                    Controllers.Prompt.ShowDialog("Hash does not match!", "Error");
                    userHashInput.Document.Blocks.Clear();
                }
                /*
                else
                {
                    byte[] IV = System.IO.File.ReadAllBytes(@"C:\\Users\\SengokuMedaru\\Desktop\\keys\\IV.txt");
                    Console.WriteLine("Gets bytes of IV");
                    byte[] encryptedSymmetricKey = System.IO.File.ReadAllBytes(@"C:\\Users\\SengokuMedaru\\Desktop\\keys\\encryptedSymmetricKey.txt");

                    //Gets the symmetric key by decrypting the encrypted symmetric key with the decryption (private) key
                    byte[] decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                    //Encrypts plaintext with symmetric key
                    byte[] cipherText = kc.symmetricEncryption(stringFormatOfFile, decryptedSymmetricKey, IV);

                    //For debugging purposes
                    fileName = System.IO.Path.GetFileNameWithoutExtension(fileName);
                    byte[] testOutput = cipherText;
                    System.IO.File.WriteAllBytes(@"C:\\Users\\SengokuMedaru\\Desktop\\EncryptedText\\encrypted_" + fileName, testOutput);
                    Console.WriteLine(fileName + " has successfully been encrypted!");
                    Console.WriteLine("");
                }*/

                // 25.7.17 UPDATE
                //
                // 1) Hash is not consistent. It is correct for some files,  but wrong for some other files. (SOLVED)
                //
                // 2) Implement password in stego function. This password will be the input to generate a symmetric key to decrypt a file inside the carrier.
                //    File will be encrypted with this password(key), then stego-ed into the carrier.
                //    If key entered is correct, then the correct file will be extracted.
                //    If key entered is incorrect, a corrupted file will be extracted.
                //
                // 3) File decryption not complete
            }
        }
    }
}
