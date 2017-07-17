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
                SHA1Managed sha1 = new SHA1Managed();
                byte[] byteComputedHash = sha1.ComputeHash(byteFormatOfFile);
                Controllers.HashController hashController = new Controllers.HashController();
                stringComputedHash = hashController.HexStringFromBytes(byteComputedHash).ToUpper();
                Console.WriteLine(stringComputedHash);
                Console.WriteLine(hash);

                //Actions to take if hash not equals to computed hash
                if (!hash.Equals(stringComputedHash))
                {
                    Console.WriteLine(stringComputedHash);
                    Console.WriteLine(hash);
                    //Do stuff:
                    //Stuff like an error message box pop up
                    //Clear the userInputHash box
                    //Break from the current method

                    string promptValue = Controllers.Prompt.ShowDialog("Hash does not match!", "Error");

                    // Update 17.7.17
                    // A very weird error is occuring
                    // !hash.Equals(stringComputedHash) keeps returning false when the condition is supposed to return true
                    // Therefore the 'else' statement keeps running even when the two hashes do not match
                }

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
                }
            }
        }
    }
}
