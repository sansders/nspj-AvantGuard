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
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.IO.Compression;
using Layout.Models;
using System.Globalization;

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
        SqlConnection con;
        SqlConnection con1;
        SqlConnection con2;
        SqlConnection con3;
        SqlDataReader reader;
        SqlCommand cmd;
        SqlCommand cmd1;
        SqlCommand cmd2;
        SqlCommand cmd3;
        string path = "";
        String fullAdd = "";
        String updatedAdd = "";
        public string hash;
        public string stringComputedHash;
        string filename1;
        static String currentUserName = UserModel.UserModel.currentUserID;
        UserModel.UserModel currentUser = UserModel.UserModel.retrieveUserFromDatabase(currentUserName);


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
                fileName = dlg.FileName; //Will be useful in the future when selecting files
                filename1 = fileName;
                byte[] byteFormatOfFile = File.ReadAllBytes(@fileName);



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

                String filename = System.IO.Path.GetFileNameWithoutExtension(dlg.FileName);
                String fullfilename = System.IO.Path.GetFullPath(dlg.FileName);
                String extension = System.IO.Path.GetExtension(dlg.FileName);


                //Actions to take if hash not equals to computed hash
                if (!(hash.Equals(stringComputedHash)))
                {
                    //Do stuff:
                    //Stuff like an error message box pop up (done)
                    //Clear the userInputHash box (done)
                    //Break from the current method (done)
                    Controllers.Prompt.ShowDialog1("Hash does not match!", "Error");
                    userHashInput.Document.Blocks.Clear();
                }
                
                else
                {
                    byte[] IV = System.IO.File.ReadAllBytes(@"C:\\Users\\SengokuMedaru\\Desktop\\keys\\IV.txt");
                    Console.WriteLine("Gets bytes of IV");
                    byte[] encryptedSymmetricKey = System.IO.File.ReadAllBytes(@"C:\\Users\\SengokuMedaru\\Desktop\\keys\\encryptedSymmetricKey.txt");

                    //Gets the symmetric key by decrypting the encrypted symmetric key with the decryption (private) key
                    byte[] decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                    //Encrypts plaintext with symmetric key
                    byte[] cipherText = kc.symmetricEncryption(byteFormatOfFile, decryptedSymmetricKey, IV);

                    //For debugging purposes
                    fileName = System.IO.Path.GetFileNameWithoutExtension(fileName);
                    byte[] testOutput = cipherText;
                    System.IO.File.WriteAllBytes(@"C:\\Users\\SengokuMedaru\\Desktop\\EncryptedText\\encrypted_" + fileName, testOutput);
                    Console.WriteLine(fileName + " has successfully been encrypted!");
                    Console.WriteLine("");

                    //Bryan code below

                    ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
                    string connectionString = conSettings.ConnectionString;
                    string VirusName;
                    string type;
                    SqlCommand cmd5;

                    String imgLocation = "";
                    int virus = 0;
                    byte[] checkMD5;

                    path = filename1;
                    Console.Write(path);

                    byte[] md5HashBytes = ComputeMd5Hash(path);

                    String strMd5 = ToHexadecimal(md5HashBytes);

                    Console.Write(strMd5);


                    con = new SqlConnection(connectionString);
                    con.Open();
                    // cmd5 = new SqlCommand("SELECT vxVirusName, vxType FROM [User].[dbo].[vx] WHERE vxMD5 = " + checkMD5 + " ; ", con);
                    cmd5 = new SqlCommand("SELECT vxVirusName, vxType,vxMD5,BYTE FROM [User].[dbo].[vx] WHERE vxMD5 =  @checkMD5 ", con);

                    cmd5.Parameters.Add(new SqlParameter("@checkMD5", strMd5));



                    Console.Write(strMd5);

                    SqlDataReader reader1 = cmd5.ExecuteReader();
                    if (reader1.Read() == true)
                    {

                        VirusName = reader1.GetString(0);
                        type = reader1.GetString(1);
                        System.Windows.MessageBox.Show(" \n Virus Dected : " + VirusName + " | Type Of Virus : " + type);
                        Console.WriteLine(" \n Virus Dected : " + VirusName + " | Type Of Virus : " + type);
                        con.Close();
                        virus = virus + 1;


                    }
                    else
                    {
                        con.Close();

                        con.Open();

                        string finalPath = fullPath();

                        FileStream Stream = new FileStream(finalPath, FileMode.Open, FileAccess.Read);
                        BinaryReader brs = new BinaryReader(Stream);
                        byte[] images = brs.ReadBytes((int)Stream.Length);

                        String strImage = System.Text.Encoding.UTF8.GetString(images);


                        cmd2 = new SqlCommand("SELECT vxVirusName, vxType,vxMD5,BYTE FROM [User].[dbo].[vx] WHERE tID = 5002 ", con);

                        //  cmd2.Parameters.Add(new SqlParameter("@BYTE", strImage));

                        SqlDataReader reader2 = cmd2.ExecuteReader();

                        if (reader2.Read() == true)
                        {
                            if (strImage.Contains(reader2.GetString(3)))
                            {
                                virus = virus + 1;
                                VirusName = reader2.GetString(0);
                                type = reader2.GetString(1);
                               
                                System.Windows.MessageBox.Show(" \n VIRUS DECTED! Your File is not being Uploaded");
                                System.Windows.MessageBox.Show(" \n Virus Dected : " + VirusName + " | Type Of Virus : " + type);
                            }
                            else
                            {
                                System.Windows.MessageBox.Show("\n There is no virus! Very Good!! ");
                                Console.WriteLine("\n File is now being uploaded......");
                                System.Windows.MessageBox.Show("\n File is now being uploaded......");
                            }

                        }
                        else
                        {
                            System.Windows.MessageBox.Show("\n There is no virus! Very Good!! ");
                            System.Windows.MessageBox.Show("\n File is now being uploaded......");
                        }
                        con.Close();
                    }



                    if (virus == 0)
                    {

                        FileModel fm = new FileModel(currentUserName, filename, cipherText, getFileSize(cipherText.Length), getCurrent(), "no", "no", extension, "");
                        fm.setShow(true);
                        FileModel.setFileModel(fm);
                        //FileModel fm = FileModel.getFileModel();
                        NavigationService.Navigate(new Uri("UploadingConsole.xaml"), UriKind.RelativeOrAbsolute);
                    }

                }

                // 25.7.17 UPDATE
                //
                // 1) Hash is not consistent. It is correct for some files,  but wrong for some other files. (SOLVED)
                //
                // 2) Implement password in stego function. This password will be the input to generate a symmetric key to decrypt a file inside the carrier.
                //    File will be encrypted with this password(key), then stego-ed into the carrier.
                //    If key entered is correct, then the correct file will be extracted.
                //    If key entered is incorrect, a corrupted file will be extracted.
                //
                // 3) File decryption not complete (SOLVED)
            }
        }

        private String getFileSize(double fileSize)
        {
            var culture = CultureInfo.CurrentUICulture;
            const String format = "#,0.0";
            String fileSizeDisplayed = "";

            if (fileSize < 1024)
            {
                fileSizeDisplayed = fileSize.ToString("#,0", culture) + " bytes";
            }

            else
            {
                fileSize /= 1024;

                if (fileSize < 1024)
                {
                    fileSizeDisplayed = fileSize.ToString(format, culture) + " KB";
                }

                else
                {
                    fileSize /= 1024;

                    if (fileSize < 1024)
                    {
                        fileSizeDisplayed = fileSize.ToString(format, culture) + " MB";
                    }

                    else
                    {
                        fileSize /= 1024;

                        if (fileSize < 1024)
                        {
                            fileSizeDisplayed = fileSize.ToString(format, culture) + " GB";
                        }

                        else
                        {
                            fileSize /= 1024;

                            if (fileSize < 1024)
                            {
                                fileSizeDisplayed = fileSize.ToString(format, culture) + " TB";
                            }
                        }
                    }
                }
            }
            return fileSizeDisplayed;
        }

        private void SetfullPath(string updatedAdd)
        {
            this.updatedAdd = updatedAdd;
        }

        private string fullPath()
        {
            return updatedAdd;
        }

        static string ToHexadecimal(byte[] source)
        {
            if (source == null) return string.Empty;

            StringBuilder sb = new StringBuilder();

            foreach (byte b in source)
            {
                sb.Append(b.ToString("X2")); // print byte as Hexadecimal string
            }

            return sb.ToString();
        }

        string dtformat = "yyyy-MM-dd HH:mm:ss";
        

        private String getCurrent()
        {
            DateTime current = new DateTime();
            current = DateTime.Now;
            return current.ToString(dtformat);
        }

        private byte[] ComputeMd5Hash(string fileName)
        {
            byte[] result = null;

            Boolean zips = true;
            using (MD5 md5 = MD5.Create())
            {
                int bufferSize = 10 * 1024 * 1024; // 10MB

                if (System.IO.Path.GetExtension(fileName).Equals(".zip"))
                {

                    do
                    {
                        using (ZipArchive zipFile = ZipFile.OpenRead(fileName))
                        {



                            foreach (ZipArchiveEntry zip in zipFile.Entries)
                            {
                                System.Console.WriteLine("Zipfile: {0}", zip.FullName);
                                System.Console.WriteLine("Zipfile: {0}", zip.Length);
                                string dirpath = fileName;
                                path = System.IO.Path.Combine(dirpath, zip.FullName);
                                Console.Write(path);
                                String SavePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                                if (zip.FullName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                                {
                                    zips = true;
                                    // fileName = path;
                                    // extract to document then later read from there again and extract again
                                    fileName = System.IO.Path.Combine(SavePath, zip.FullName);

                                    if (File.Exists(fileName))
                                    {
                                        File.Delete(fileName);
                                        zip.ExtractToFile(System.IO.Path.Combine(SavePath, zip.FullName));

                                    }
                                    else
                                    {
                                        zip.ExtractToFile(System.IO.Path.Combine(SavePath, zip.FullName));
                                    }

                                }
                                else
                                {
                                    fullAdd = System.IO.Path.Combine(SavePath, zip.FullName);
                                    if (File.Exists(fullAdd))
                                    {
                                        File.Delete(fullAdd);
                                        zip.ExtractToFile(System.IO.Path.Combine(SavePath, zip.FullName));

                                    }
                                    else
                                    {
                                        zip.ExtractToFile(System.IO.Path.Combine(SavePath, zip.FullName));
                                    }
                                    zips = false;
                                }
                            }
                        }



                    } while (zips == true);

                    using (var stream = new BufferedStream(File.OpenRead(fullAdd), bufferSize))
                    {
                        result = md5.ComputeHash(stream);
                        SetfullPath(fullAdd);
                    }
                }
                else
                {
                    using (var stream = new BufferedStream(File.OpenRead(fileName), bufferSize))
                    {
                        result = md5.ComputeHash(stream);
                        SetfullPath(fileName);
                    }
                }


            }

            return result;
        }

        private void MyFoldersButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RecentButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FavoritesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BinButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {

        }

        private void changeSettings(object sender, RoutedEventArgs e)
        {

        }

        private void SharedButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
