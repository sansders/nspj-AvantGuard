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
        SqlDataReader reader;
        SqlCommand cmd;

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
                string connectionString = conSettings.ConnectionString;
                imgLocation = dlg.FileName.ToString();

                con = new SqlConnection(connectionString);

                byte[] images = null;
                byte[] images1 = null;
                byte[] images2 = null;
                byte[] images3 = null;

                FileStream Stream = new FileStream(imgLocation,FileMode.Open,FileAccess.Read);
                BinaryReader brs = new BinaryReader(Stream);
                images = brs.ReadBytes((int)Stream.Length);
                

                con.Open();

                string sqlQuery = "Insert into dbo.UserFiles(Username,Name,Image)Values( 'Random' , 'man' , @images )";
                cmd = new SqlCommand(sqlQuery, con);
                cmd.Parameters.Add(new SqlParameter("@images",images));
                int N = cmd.ExecuteNonQuery();
                con.Close();
                Console.Write("Data Has Been Uploaded");
                System.Windows.MessageBox.Show(N.ToString() + "Datas Saved Success");
            

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
            string connectionString = conSettings.ConnectionString;
            con = new SqlConnection(connectionString);
            con.Open();
            string sqlQuery = "select Image FROM [dbo].[UserFiles] where Username='Random'";
            cmd = new SqlCommand(sqlQuery, con);
            SqlDataReader DataRead = cmd.ExecuteReader();
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

            con.Close();
        }

        string enText;

        private void encryptBtn(object sender, RoutedEventArgs e)
        {
           KeyController ks = new KeyController();
           enText = ks.encrypt();

        }

        private void decryptBtn(object sender, RoutedEventArgs e)
        {
            KeyController ks = new KeyController();
            ks.decrypt(enText);
            
        }
    }
}
