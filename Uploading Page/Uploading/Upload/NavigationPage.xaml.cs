using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Configuration;
using System.Windows.Forms;
using System.IO;
using Layout.Controllers;
using Layout.Models;
using static System.Environment;
using System.Drawing;
using System.Collections;
using System.Security.Cryptography;
using System.IO.Compression;
using System.Drawing.Imaging;

namespace Layout.Upload
{
    /// <summary>
    /// Interaction logic for NavigationPage.xaml
    /// </summary>
    public partial class NavigationPage : Page
    {

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
        public NavigationPage()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page1());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page2());
        }

        private void BtnDB(object sender, RoutedEventArgs e)
        {

            ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
            string connectionString = conSettings.ConnectionString;

            try
            {
                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand("SELECT Username , Name , ContactNo , Password FROM [dbo].[UserAcc]", con);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(" | Username : " + reader.GetString(0) + " | Name : " + reader.GetString(1) + " | Contact No : " + reader.GetString(2) + " | Password : " + reader.GetString(3)) ;
                }
            } catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            } finally
            {
                
                con.Close();
            }

            //SqlConnection conn = new SqlConnection("Server=WIN-P4IU8GMGT91\\SQLEXPRESS;Database=User;Integrated Security=True");
            //SqlConnection conn = new SqlConnection("Server=192.168.205.138,1433\\SQLEXPRESS;Network Library=DBMSSOCN;Initial Catalog = User; User ID = root; Password = ; ");
           // SqlConnection conn = new SqlConnection("Data Source=192.168.205.138,1433;Network Library=DBMSSOCN;Initial Catalog = User; User ID = root; Password = ...;Trusted_Connection = no;Integrated Security=False; ");
            //WHY CANNOT CONNECT

            

           // conn.Open();
           // Console.Write(conn.State);
         //  SqlCommand cmd = new SqlCommand("SELECT Username , Name , ContactNo FROM [dbo].[UserAcc]" , conn);
           // SqlCommand cmd = new SqlCommand("SELECT * FROM UserAcc" , conn); 
         //   SqlDataReader reader = cmd.ExecuteReader();
            //Console.WriteLine(reader.GetString(0));
         /*   while (reader.Read()) {
                
                Console.WriteLine(" | Username : " + reader.GetString(0) + " | Name : " + reader.GetString(1) + " | Contact No : " + reader.GetString(2));
            }
            }
            reader.Close();
            conn.Close();
            */
          /*  if (Debugger.IsAttached)
            {
                Console.ReadLine();
            } */

        }

        private void uploadBtn(object sender, RoutedEventArgs e)
        {

            ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
            string connectionString = conSettings.ConnectionString;

            string VirusName;
            string type;
            SqlCommand cmd5;

            String imgLocation = "";
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();

          
            int virus = 0;

            byte[] checkMD5;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fileName;
                fileName = dlg.FileName; //Will be useful in the future when selecting files
                path = fileName;

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
                        if(strImage.Contains(reader2.GetString(3)))
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

                    } else
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

                    }

                    /*

                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "C:\\Program Files\\Windows Defender\\MpCmdRun.exe";
                    startInfo.Arguments = "-Scan -ScanType 3 -File " + fileName;
                    process.StartInfo = startInfo;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    var result = process.Start();
                    Console.WriteLine(fileName);
                    Console.WriteLine("Scanned with result: " + result.ToString());
                    Console.WriteLine("Scanning...");
                    string output = process.StandardOutput.ReadToEnd();
                    Console.WriteLine(output);

                    if (output.ToLowerInvariant().Contains(("cleaning")))
                    {
                        Console.Write("Got Virus SIA");
                    } else
                    {
                        Console.Write("Okay no Virus , Very Safe ");

                        Console.Write("File is not being uploaded");







                        ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
                        ConnectionStringSettings conSettings1 = ConfigurationManager.ConnectionStrings["connString1"];
                        ConnectionStringSettings conSettings2 = ConfigurationManager.ConnectionStrings["connString2"];
                        ConnectionStringSettings conSettings3 = ConfigurationManager.ConnectionStrings["connString3"];
                        string connectionString = conSettings.ConnectionString;
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
                        Console.WriteLine("1/3 image");
                        for (int i = 0; i < images1.Length; i++)
                        {
                            Console.Write("1/3 image");
                            Console.Write(images1[i]);
                        }




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

                        string sqlQuery1 = "Insert into dbo.UserFiles1(Username,Name,Image)Values( '123' , 'man' , @images1 )";
                        cmd = new SqlCommand(sqlQuery1, con1);
                        cmd.Parameters.Add(new SqlParameter("@images1", images1));
                        cmd.ExecuteNonQuery();

                        string sqlQuery2 = "Insert into dbo.UserFiles3(Username,Name,Image)Values( '123' , 'man' , @images2 )";
                        cmd = new SqlCommand(sqlQuery2, con2);
                        cmd.Parameters.Add(new SqlParameter("@images2", images3));
                        cmd.ExecuteNonQuery();

                        string sqlQuery3 = "Insert into dbo.UserFiles2(Username,Name,Image)Values( '123' , 'man' , @images3 )";
                        cmd = new SqlCommand(sqlQuery3, con3);
                        cmd.Parameters.Add(new SqlParameter("@images3", images2));
                        cmd.ExecuteNonQuery();

                        // cmd.Parameters.Add(new SqlParameter("@images",images));

                        // int N = cmd.ExecuteNonQuery();
                        con.Close();
                        Console.Write("Data Has Been Uploaded");
                        System.Windows.MessageBox.Show("Datas Saved Success");

                    }

                    process.WaitForExit();

                  */

                    // while (!process.StandardOutput.EndOfStream)
                    // {
                    //  string line = process.StandardOutput.ReadLine();
                    //     Console.WriteLine(process.StandardOutput.ReadLine());
                    //  }



                    //  FileStream FS = new FileStream(fileName, FileMode.Open, FileAccess.Read); //create a file stream object associate to user selected file 
                    //    byte[] img = new byte[FS.Length]; //create a byte array with size of user select file stream length
                    //   FS.Read(img, 0, Convert.ToInt32(FS.Length));//read user selected file stream in to byte array



                    /*
                                    try
                                    {
                                        con = new SqlConnection(connectionString);
                                        con.Open();
                                        cmd = new SqlCommand("", con);

                                    }
                                    catch (Exception ex)
                                    {
                                        System.Windows.MessageBox.Show(ex.Message);
                                    }
                                    finally
                                    {

                                        con.Close();
                                    }

                                    */



                    /*
           for (int counter = 0; counter < bits.Length; counter++)
           {
               Console.Write(bits[counter] ? "1" : "0");
               if ((counter + 1) % 8 == 0)
                   Console.WriteLine();
           }
                    */
                

            }
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

        private void SetfullPath(string updatedAdd)
        {
            this.updatedAdd = updatedAdd;
        }

        private string fullPath()
        {
            return updatedAdd;
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

                                    } else
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

        private byte[] ConvertImageToByteArray(System.Drawing.Image imageToConvert,System.Drawing.Imaging.ImageFormat formatOfImage)
        {
            byte[] Ret;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    imageToConvert.Save(ms, formatOfImage);
                    Ret = ms.ToArray();
                }
            }
            catch (Exception) { throw; }
            return Ret;
        }

     

        private void ShowBtn(object sender, RoutedEventArgs e)
        {
            ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];


            ConnectionStringSettings conSettings1 = ConfigurationManager.ConnectionStrings["connString1"];
            ConnectionStringSettings conSettings2 = ConfigurationManager.ConnectionStrings["connString2"];
            ConnectionStringSettings conSettings3 = ConfigurationManager.ConnectionStrings["connString3"];
         
            string connectionString1 = conSettings1.ConnectionString;
            string connectionString2 = conSettings2.ConnectionString;
            string connectionString3 = conSettings3.ConnectionString;
         
          
            con1 = new SqlConnection(connectionString1);
            con2 = new SqlConnection(connectionString2);
            con3 = new SqlConnection(connectionString3);


            string connectionString = conSettings.ConnectionString;
            con = new SqlConnection(connectionString);
            con.Open();
            con1.Open();
            con2.Open();
            con3.Open();

            //  string sqlQuery = "select Image FROM [dbo].[UserFiles] where Username='Random'";
            //  cmd = new SqlCommand(sqlQuery, con);

            string sqlQuery1 = "select Image FROM [dbo].[UserFiles1] where Username='123456'";
            cmd = new SqlCommand(sqlQuery1, con1);
            SqlDataReader DataRead1 = cmd.ExecuteReader();

            string sqlQuery2 = "select Image FROM [dbo].[UserFiles3] where Username='123456'";
            cmd = new SqlCommand(sqlQuery2, con2);
            SqlDataReader DataRead2 = cmd.ExecuteReader();

            string sqlQuery3 = "select Image FROM [dbo].[UserFiles2] where Username='123456'";
            cmd = new SqlCommand(sqlQuery3, con3);
            SqlDataReader DataRead3 = cmd.ExecuteReader();

            DataRead1.Read();
            DataRead2.Read();
            DataRead3.Read();


         /*   BitArray fullBits;
            BitArray bits1 = new BitArray((BitArray)DataRead1[0]);
            BitArray bits2 = new BitArray((BitArray)DataRead2[0]);
            BitArray bits3 = new BitArray((BitArray)DataRead3[0]);



            fullBits = new BitArray(bits1.Count + bits2.Count + bits3.Count);

            Console.Write(fullBits); */

            byte[] images;
            byte[] images1 = ((byte[])DataRead1[0]);
            byte[] images2 = ((byte[])DataRead2[0]);
            byte[] images3 = ((byte[])DataRead3[0]);

            images = images1.Concat(images3).Concat(images2).ToArray();

            if (images == null)
            {
                imageViewer.Source = null;
            }
            else
            {

              //  SaveFileDialog saveFileDialog = new SaveFileDialog();

                MemoryStream mstreem = new MemoryStream(images);
                imageViewer.Source = BitmapFrame.Create(mstreem);
                System.Drawing.Image img = System.Drawing.Image.FromStream(mstreem);

               

             //   byte[] iBytes = new byte[images.Length + 1];
             //   String path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
             //   path = System.IO.Path.Combine(path, "Downloads");

           //     img.Save( @path);

          //      MemoryStream ms = new MemoryStream();
               




              //  using (FileStream file = new FileStream(@path, FileMode.Create))
             //   {
              //     file.Write(iBytes, 0, iBytes.Count());
              //   }

               
            }

            //SqlDataReader DataRead = cmd.ExecuteReader();

            /*
                DataRead.Read();

                if (DataRead.HasRows)
                {
                    //textName.Text = DataRead[0].ToString();
                    byte[] images = ((byte[])DataRead[0]);


                    if (images == null)
                    {
                        imageViewer.Source= null;
                    } else {
                        MemoryStream mstreem = new MemoryStream(images);
                        imageViewer.Source = BitmapFrame.Create(mstreem);
                    }
                }
                */

            con.Close();
        }


        private void encryptBtn(object sender, RoutedEventArgs e)
        {

            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Controllers.KeyController kc = new Controllers.KeyController();

                string fileName;
                byte[] plainBytes;
                fileName = dlg.FileName; //Will be useful in the future when selecting files

                plainBytes = File.ReadAllBytes(@fileName);
                Console.WriteLine("Getting bytes of file");
                byte[] IV = File.ReadAllBytes(@"C:\\Users\\SengokuMedaru\\Desktop\\keys\\IV.txt");
                Console.WriteLine("Getting bytes of IV");
                byte[] encryptedSymmetricKey = File.ReadAllBytes(@"C:\\Users\\SengokuMedaru\\Desktop\\keys\\encryptedSymmetricKey.txt");
                Console.WriteLine("Getting bytes of encrypted symmetric key");

                //Gets the symmetric key by decrypting the encrypted symmetric key with the decryption (private) key
                byte[] decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                //Encrypts plaintext with symmetric key
                byte[] cipherText = kc.symmetricEncryption(plainBytes, decryptedSymmetricKey, IV);

                //For debugging purposes
                fileName = System.IO.Path.GetFileNameWithoutExtension(fileName);
                byte[] testOutput = cipherText;
                System.IO.File.WriteAllBytes(@"C:\\Users\\SengokuMedaru\\Desktop\\EncryptedText\\encrypted_" + fileName, testOutput);
                Console.WriteLine(fileName + " has successfully been encrypted!");
                Console.WriteLine("");

            }
        }
        private void decryptBtn(object sender, RoutedEventArgs e)
        {
            //Creates an instance of the KeyController Object
            KeyController kc = new KeyController();

            //Gets IV & Encrypted Symmetric Key
            byte[] IV = System.IO.File.ReadAllBytes(@"C:\\Users\\SengokuMedaru\\Desktop\\keys\\IV.txt");
            byte[] encryptedSymmetricKey = File.ReadAllBytes(@"C:\\Users\\SengokuMedaru\\Desktop\\keys\\encryptedSymmetricKey.txt");
            
            //Gets the symmetric key by decrypting the encrypted symmetric key with the decryption (private) key
            byte[] decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);

            string fileName;

            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                fileName = dlg.FileName;
                byte[] fileData = System.IO.File.ReadAllBytes(@fileName);
                byte[] plainText = kc.symmetricDecryption(fileData, decryptedSymmetricKey, IV);
                Console.WriteLine("Selected file is successfully decrypted!");


                //Bryan, I'll need you for this
                //I'll have to add the file's name (which will be stored in your database I assume) at the last part of the string below
                System.IO.File.WriteAllBytes(@"C:\\Users\\SengokuMedaru\\Desktop\\DecryptedText\\decrypted_file.docx", plainText);
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("No file selected :(");
                Console.WriteLine("");
            }

        }

        private void stegEncrypt(object sender, RoutedEventArgs e)
        {
            string fileName;
            string hideThis;
            Bitmap bmp;

            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                fileName = dlg.FileName;
                string fileExtention = fileName.Substring(fileName.Length-3);
                if (!fileExtention.Equals("bmp"))
                {
                    bmp = Steganography.ConvertToBitmap(fileName);
                }
                else
                {
                    byte[] imageData = System.IO.File.ReadAllBytes(@fileName);
                    using (var ms = new MemoryStream(imageData))
                    {
                        bmp = new Bitmap(ms);
                    }
                }
                OpenFileDialog dlg1 = new OpenFileDialog();
                if (dlg1.ShowDialog() == DialogResult.OK)
                {
                    hideThis = File.ReadAllText(dlg1.FileName);
                    bmp = Steganography.embedText(hideThis, bmp);
                    fileName = System.IO.Path.GetFileNameWithoutExtension(fileName);

                    bmp.Save(@"C:\\Users\\SengokuMedaru\\Desktop\\EncryptedText\\Harmless " + fileName + ".bmp");
                    Console.WriteLine("A txt file has successfully been hidden in " +fileName+"!");
                    Console.WriteLine("");

                }
            }
            
        }

        private void stegDecrypt(object sender, RoutedEventArgs e)
        {
            string extractThis;
            string fileName;
            Bitmap bmp;

            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                fileName = dlg.FileName;
                byte[] imageData = System.IO.File.ReadAllBytes(@fileName);
                using (var ms = new MemoryStream(imageData))
                {
                    bmp = new Bitmap(ms);
                }
                fileName = System.IO.Path.GetFileNameWithoutExtension(fileName);
                extractThis = Steganography.extractText(bmp);
                System.IO.File.WriteAllText(@"C:\\Users\\SengokuMedaru\\Desktop\\DecryptedText\\SECRET MESSAGE.txt", extractThis);
                Console.WriteLine("!!! A SECRET MESSAGE HAS BEEN REVEALED FROM " + fileName+" !!!");
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("Error: File not specified :(");
                Console.WriteLine("");
            }
            
        }
    }
}
