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

                                Console.WriteLine(" \n VIRUS DECTED! Your File is not being Uploaded");
                                Console.WriteLine(" \n Virus Dected : " + VirusName + " | Type Of Virus : " + type);
                            }
                            else
                            {
                                Console.WriteLine("\n There is no virus! Very Good!! ");
                                Console.WriteLine("\n File is now being uploaded......");
                            }

                        }
                        else
                        {
                            Console.WriteLine("\n There is no virus! Very Good!! ");
                            Console.WriteLine("\n File is now being uploaded......");
                        }
                        con.Close();
                    }



                    if (virus == 0)
                    {
                        ConnectionStringSettings conSettings1 = ConfigurationManager.ConnectionStrings["connString1"];
                        ConnectionStringSettings conSettings2 = ConfigurationManager.ConnectionStrings["connString2"];
                        ConnectionStringSettings conSettings3 = ConfigurationManager.ConnectionStrings["connString3"];

                        string connectionString1 = conSettings1.ConnectionString;
                        string connectionString2 = conSettings2.ConnectionString;
                        string connectionString3 = conSettings3.ConnectionString;

                        imgLocation = dlg.FileName.ToString();
                        con = new SqlConnection(connectionString);
                        con1 = new SqlConnection(connectionString1);
                        con2 = new SqlConnection(connectionString2);
                        con3 = new SqlConnection(connectionString3);
                        byte[] images = null;
                        byte[] images1 = null;
                        byte[] images2 = null;
                        byte[] images3 = null;

                        BitArray b1;

                        FileStream Stream = new FileStream(imgLocation, FileMode.Open, FileAccess.Read);
                        BinaryReader brs = new BinaryReader(Stream);
                        images = brs.ReadBytes((int)Stream.Length);

                        String strImage = System.Text.Encoding.UTF8.GetString(images);
                        // Console.Write(strImage);
                        int leng = images.Length / 3;
                        Console.Write("Length : " + leng);
                        images1 = images.Take(leng).ToArray();
                        // images1 = strImage.Substring(1,leng);

                    
                        images2 = images.Skip(leng).Take(leng).ToArray();
                        int leng2 = leng + leng;
                        images3 = images.Skip(leng2).Take(leng).ToArray();
                        con.Open();
                        con1.Open();
                        con2.Open();
                        con3.Open();

                        //BitArray bits1 = new BitArray(images1);
                        //  BitArray bits2 = new BitArray(images2);
                        // BitArray bits3 = new BitArray(images3);

                        string sqlQuery = "Insert into dbo.UserFiles(Username,Name,Image)Values( 'superman' , 'man' , @images )";
                        cmd = new SqlCommand(sqlQuery, con);
                        cmd.Parameters.Add(new SqlParameter("@images", images));
                        cmd.ExecuteNonQuery();

                        string sqlQuery1 = "Insert into dbo.UserFiles1(Username,Name,Image)Values( '123456' , 'man' , @images1 )";
                        cmd = new SqlCommand(sqlQuery1, con1);
                        cmd.Parameters.Add(new SqlParameter("@images1", images1));
                        cmd.ExecuteNonQuery();

                        string sqlQuery2 = "Insert into dbo.UserFiles3(Username,Name,Image)Values( '123456' , 'man' , @images2 )";
                        cmd = new SqlCommand(sqlQuery2, con2);
                        cmd.Parameters.Add(new SqlParameter("@images2", images3));
                        cmd.ExecuteNonQuery();

                        string sqlQuery3 = "Insert into dbo.UserFiles2(Username,Name,Image)Values( '123456' , 'man' , @images3 )";
                        cmd = new SqlCommand(sqlQuery3, con3);
                        cmd.Parameters.Add(new SqlParameter("@images3", images2));
                        cmd.ExecuteNonQuery();

                        // cmd.Parameters.Add(new SqlParameter("@images",images));

                        // int N = cmd.ExecuteNonQuery();
                        con.Close();
                        Console.WriteLine("\n Data Has Been Uploaded");
                        System.Windows.MessageBox.Show("Datas Saved Success");
                        NavigationService.Navigate(new Page2());
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


    }
}
