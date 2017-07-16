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
using System.IO;

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
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            OpenOrNew.Visibility = System.Windows.Visibility.Visible;      
        }

        private void NewFileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenOrNew.Visibility = System.Windows.Visibility.Collapsed;
            NewFile.Visibility = System.Windows.Visibility.Visible;
        }

        private void OpenExistingBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenOrNew.Visibility = System.Windows.Visibility.Collapsed;
            OpenExisting.Visibility = System.Windows.Visibility.Visible;
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select docName from [table]";
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataTable.ItemsSource = dt.DefaultView;
            con.Close();
        }

        private void dataTable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            String theText = "";
            String selectedText = ((DataRowView)((ListView)sender).SelectedItem)["docName"].ToString();
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select text from [table] where docName = '" + selectedText + "'";
            using (SqlDataReader read = cmd.ExecuteReader())
            {
                while(read.Read())
                {
                    theText = (read["text"].ToString());
                }
            }
            con.Close();

            byte[] byteArray = Encoding.ASCII.GetBytes(theText);
            using(MemoryStream ms = new MemoryStream(byteArray))
            {
                TextRange tr = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                tr.Load(ms, DataFormats.Rtf);
            }
            OpenExisting.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            NewFile.Visibility = System.Windows.Visibility.Collapsed;

            String input = textbox1.Text;
            fileName.Content = input;
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into [table] values('" + textbox1.Text + "', '')";
            cmd.ExecuteNonQuery();
            con.Close();
            textbox1.Text = String.Empty;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NewFile.Visibility = System.Windows.Visibility.Collapsed;
            textbox1.Text = String.Empty;
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null)
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
        }

        private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            rtbEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);
        }

        private void rtbEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            String rtfText;
            TextRange tr = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            using (MemoryStream ms = new MemoryStream())
            {
                tr.Save(ms, DataFormats.Rtf);
                rtfText = Encoding.ASCII.GetString(ms.ToArray());
            }
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update [table] set text = '" + rtfText + "' where docName = '" + fileName.Content + "'";
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = rtbEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp;
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
            cmbFontSize.Text = temp.ToString();
        }
    }
}
