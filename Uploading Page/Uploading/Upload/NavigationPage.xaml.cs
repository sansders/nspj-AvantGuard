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
                cmd = new SqlCommand("SELECT Username , Name , ContactNo FROM [dbo].[UserAcc]", con);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(" | Username : " + reader.GetString(0) + " | Name : " + reader.GetString(1) + " | Contact No : " + reader.GetString(2));
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

            String imgLocation = "";
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fileName;
                fileName = dlg.FileName; //Will be useful in the future when selecting files
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

                FileStream Stream = new FileStream(imgLocation,FileMode.Open,FileAccess.Read);
                BinaryReader brs = new BinaryReader(Stream);
                images = brs.ReadBytes((int)Stream.Length);
                // Console.Write(images);
                for(int i = 0; i < images.Length; i ++)
  {
                    Console.WriteLine(images[i]);
                }


                String strImage = System.Text.Encoding.UTF8.GetString(images);
               // Console.Write(strImage);
                int leng = images.Length / 3;
                Console.Write("Length : "+ leng );
                 images1 = images.Take(leng).ToArray() ;
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

                string sqlQuery = "Insert into dbo.UserFiles(Username,Name,Image)Values( 'Random123' , 'man' , @images )";
                cmd = new SqlCommand(sqlQuery, con);
                cmd.Parameters.Add(new SqlParameter("@images", images));
                cmd.ExecuteNonQuery();

                string sqlQuery1 = "Insert into dbo.UserFiles1(Username,Name,Image)Values( 'Random123' , 'man' , @images1 )";
                cmd = new SqlCommand(sqlQuery1, con1);
                cmd.Parameters.Add(new SqlParameter("@images1", images1));
                cmd.ExecuteNonQuery();

                string sqlQuery2 = "Insert into dbo.UserFiles3(Username,Name,Image)Values( 'Random123' , 'man' , @images2 )";
                cmd = new SqlCommand(sqlQuery2, con2);
                cmd.Parameters.Add(new SqlParameter("@images2", images3));
                cmd.ExecuteNonQuery();

                string sqlQuery3 = "Insert into dbo.UserFiles2(Username,Name,Image)Values( 'Random123' , 'man' , @images3 )";
                cmd = new SqlCommand(sqlQuery3, con3);
                cmd.Parameters.Add(new SqlParameter("@images3", images2));
                cmd.ExecuteNonQuery();

                // cmd.Parameters.Add(new SqlParameter("@images",images));

                // int N = cmd.ExecuteNonQuery();
                con.Close();
                Console.Write("Data Has Been Uploaded");
                System.Windows.MessageBox.Show( "Datas Saved Success");
            

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
            }

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

            string sqlQuery1 = "select Image FROM [dbo].[UserFiles1] where Username='Random123'";
            cmd = new SqlCommand(sqlQuery1, con1);
            SqlDataReader DataRead1 = cmd.ExecuteReader();

            string sqlQuery2 = "select Image FROM [dbo].[UserFiles3] where Username='Random123'";
            cmd = new SqlCommand(sqlQuery2, con2);
            SqlDataReader DataRead2 = cmd.ExecuteReader();

            string sqlQuery3 = "select Image FROM [dbo].[UserFiles2] where Username='Random123'";
            cmd = new SqlCommand(sqlQuery3, con3);
            SqlDataReader DataRead3 = cmd.ExecuteReader();

            DataRead1.Read();
            DataRead2.Read();
            DataRead3.Read();

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
                MemoryStream mstreem = new MemoryStream(images);
                imageViewer.Source = BitmapFrame.Create(mstreem);
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



        string _enText;

        private void encryptBtn(object sender, RoutedEventArgs e)
        {
            /*
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

                KeyController ks = new KeyController();
                _enText = ks.asymmetricEncryption(stringFormatOfFile);
            }*/
        }

        private void decryptBtn(object sender, RoutedEventArgs e)
        {
            /*KeyController ks = new KeyController();
            ks.asymmetricDecryption(_enText);*/
            
        }
    }
}
