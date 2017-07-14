using System;
using System.Collections.Generic;
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
using System.Data;
using System.Data.SqlClient;

namespace Cloud
{
    /// <summary>
    /// Interaction logic for testingdatabase.xaml
    /// </summary>
    public partial class testingdatabase : Page
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Chester\Documents\test_db.mdf;Integrated Security=True;Connect Timeout=30");

        public testingdatabase()
        {
            InitializeComponent();
            InputBox.Visibility = System.Windows.Visibility.Visible;      
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = System.Windows.Visibility.Collapsed;

            String input = textbox1.Text;
            fileName.Content = input;
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into [table] values('" + textbox1.Text + "', 'hi')";
            cmd.ExecuteNonQuery();
            con.Close();
            textbox1.Text = String.Empty;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = System.Windows.Visibility.Collapsed;
            textbox1.Text = String.Empty;
        }

        private void textbox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update [table] set text = '" + textbox2.Text + "' where docName = '" + fileName.Content + "'";
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}
