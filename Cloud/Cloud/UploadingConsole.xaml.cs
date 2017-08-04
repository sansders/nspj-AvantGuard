using Layout.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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

namespace Layout.Upload
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        private bool isToggled = false;

        static ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
        static ConnectionStringSettings conSettings1 = ConfigurationManager.ConnectionStrings["connString1"];
        static ConnectionStringSettings conSettings2 = ConfigurationManager.ConnectionStrings["connString2"];
        static ConnectionStringSettings conSettings3 = ConfigurationManager.ConnectionStrings["connString3"];

        static string connectionString = conSettings.ConnectionString;
        static string connectionString1 = conSettings1.ConnectionString;
        static string connectionString2 = conSettings2.ConnectionString;
        static string connectionString3 = conSettings3.ConnectionString;

        SqlConnection con = new SqlConnection(connectionString);
        SqlConnection con1 = new SqlConnection(connectionString1);
        SqlConnection con2 = new SqlConnection(connectionString2);
        SqlConnection con3 = new SqlConnection(connectionString3);


        SqlDataReader reader;
        SqlCommand cmd1;
        SqlCommand cmd2;
        SqlCommand cmd3;


        public Page2()
        {



            InitializeComponent();
            //Added this

            FileModel fm = FileModel.getFileModel();

            string id = fm.ReturnUserID();
            byte[] fileBytes = fm.ReturnFileBytes();
            string fileName = fm.ReturnfileName();
            string fileSize = fm.ReturnFileSize();
            string lastModified = fm.ReturnLastModified();
            string isFavorite = fm.ReturnIsFavorite();
            string isDeleted = fm.ReturnIsDeleted();
            string fileType = fm.ReturnFileType();
            string sharedBy = fm.ReturnSharedBy();



            int len = fileBytes.Length / 3;
            byte[] toSend1 = fileBytes.Take(len).ToArray();
            byte[] toSend2 = fileBytes.Skip(len).Take(len).ToArray();
            int len2 = len + len;
            byte[] toSend3 = fileBytes.Skip(len2).Take(len).ToArray();

            openAllConnections();

            string sqlQuery1 = ("insert into [dbo].[UserFiles1] values(@id,@fileName,@toSend1,@fileSize,@lastModified,@isFavorite,@isDeleted,@fileType,@sharedBy)");
            cmd1 = new SqlCommand(sqlQuery1, con1);
            
            cmd1.Parameters.Add(new SqlParameter("@id",id));
            cmd1.Parameters.Add(new SqlParameter("@fileName", fileName));
            SqlParameter para1 = new SqlParameter("@toSend1", toSend1);
            cmd1.Parameters.Add(para1);
            cmd1.Parameters.Add(new SqlParameter("@fileSize", fileSize));
            cmd1.Parameters.Add(new SqlParameter("@lastModified", lastModified));
            cmd1.Parameters.Add(new SqlParameter("@isFavorite", isFavorite));
            cmd1.Parameters.Add(new SqlParameter("@isDeleted", isDeleted));
            cmd1.Parameters.Add(new SqlParameter("@fileType", fileType));
            cmd1.Parameters.Add(new SqlParameter("@sharedBy", sharedBy));

            string sqlQuery2 = ("insert into [dbo].[UserFiles3] values(@id2,@fileName2,@toSend2,@fileSize2,@lastModified2,@isFavorite2,@isDeleted2,@fileType2,@sharedBy2)");
            cmd2 = new SqlCommand(sqlQuery2, con2);

            cmd2.Parameters.Add(new SqlParameter("@id2", id));
            cmd2.Parameters.Add(new SqlParameter("@fileName2", fileName));
            SqlParameter para2 = new SqlParameter("@toSend2", toSend2);
            cmd2.Parameters.Add(para2);
            cmd2.Parameters.Add(new SqlParameter("@fileSize2", fileSize));
            cmd2.Parameters.Add(new SqlParameter("@lastModified2", lastModified));
            cmd2.Parameters.Add(new SqlParameter("@isFavorite2", isFavorite));
            cmd2.Parameters.Add(new SqlParameter("@isDeleted2", isDeleted));
            cmd2.Parameters.Add(new SqlParameter("@fileType2", fileType));
            cmd2.Parameters.Add(new SqlParameter("@sharedBy2", sharedBy));


            string sqlQuery3 = ("insert into [dbo].[UserFiles2] values(@id3,@fileName3,@toSend3,@fileSize3,@lastModified3,@isFavorite3,@isDeleted3,@fileType3,@sharedBy3)");
            cmd3 = new SqlCommand(sqlQuery3, con3);

            cmd3.Parameters.Add(new SqlParameter("@id3", id));
            cmd3.Parameters.Add(new SqlParameter("@fileName3", fileName));
            SqlParameter para3 = new SqlParameter("@toSend3", toSend3);
            cmd3.Parameters.Add(para3);
            cmd3.Parameters.Add(new SqlParameter("@fileSize3", fileSize));
            cmd3.Parameters.Add(new SqlParameter("@lastModified3", lastModified));
            cmd3.Parameters.Add(new SqlParameter("@isFavorite3", isFavorite));
            cmd3.Parameters.Add(new SqlParameter("@isDeleted3", isDeleted));
            cmd3.Parameters.Add(new SqlParameter("@fileType3", fileType));
            cmd3.Parameters.Add(new SqlParameter("@sharedBy3", sharedBy));

            cmd1.ExecuteNonQuery();
            cmd2.ExecuteNonQuery();
            cmd3.ExecuteNonQuery();

            closeAllConnections();
        }

        private void openAllConnections()
        {
            con.Open();
            con1.Open();
            con2.Open();
            con3.Open();
        }

        private void closeAllConnections()
        {
            con.Close();
            con1.Close();
            con2.Close();
            con3.Close();
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {

            if (console.Visibility == Visibility.Hidden && isToggled == false)
            {

                Console.Write(toggle.IsChecked);
                console.Visibility = Visibility.Visible;
                isToggled = true;

            }

        }

        private void ToggleButton_unChecked(object sender, RoutedEventArgs e)
        {

            if (console.Visibility == Visibility.Visible && isToggled == true)
            {
                console.Visibility = Visibility.Hidden;
                Console.Write(toggle.IsChecked);
                isToggled = false;

            }

        }

        private void consoleClicked(object sender, EventArgs e)
        {
            if (console.Visibility == Visibility.Hidden && isToggled == false)
            {

                toggle.IsChecked = true;
                console.Visibility = Visibility.Visible;
                isToggled = true;

            }
            else if (console.Visibility == Visibility.Visible && isToggled == true)
            {
                console.Visibility = Visibility.Hidden;
                toggle.IsChecked = false;
                isToggled = false;

            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NavigationPage());

        }

        private void MyFoldersButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RecentButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SharedButton_Click(object sender, RoutedEventArgs e)
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
    }
}

