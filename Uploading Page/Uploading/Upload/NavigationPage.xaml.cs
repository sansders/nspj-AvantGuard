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

namespace Layout.Upload
{
    /// <summary>
    /// Interaction logic for NavigationPage.xaml
    /// </summary>
    public partial class NavigationPage : Page
    {
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
            //SqlConnection conn = new SqlConnection("Server=WIN-P4IU8GMGT91\\SQLEXPRESS;Database=User;Integrated Security=True");
            //SqlConnection conn = new SqlConnection("Server=192.168.205.138,1433\\SQLEXPRESS;Network Library=DBMSSOCN;Initial Catalog = User; User ID = root; Password = ; ");
            SqlConnection conn = new SqlConnection("Data Source=192.168.205.138,1433;Network Library=DBMSSOCN;Initial Catalog = User; User ID = root; Password = 123;Trusted_Connection = no;Integrated Security=False; ");
            //WHY CANNOT CONNECT

            conn.Open();
            Console.Write(conn.State);
           SqlCommand cmd = new SqlCommand("SELECT Username , Name , ContactNo FROM [dbo].[UserAcc]" , conn);
           // SqlCommand cmd = new SqlCommand("SELECT * FROM UserAcc" , conn);
            SqlDataReader reader = cmd.ExecuteReader();
            //Console.WriteLine(reader.GetString(0));
            while (reader.Read()) {
                
                Console.WriteLine(" | Username : " + reader.GetString(0) + " | Name : " + reader.GetString(1) + " | Contact No : " + reader.GetString(2));
            }
            reader.Close();
            conn.Close();

          /*  if (Debugger.IsAttached)
            {
                Console.ReadLine();
            } */

        }
    }
}
