using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
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

namespace Cloud.MyFoldersPage
{
    /// <summary>
    /// Interaction logic for MyFoldersPage.xaml
    /// </summary>
    public partial class MyFoldersPage : Page
    {
        private List<String> newList = new List<String>();
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Chester\Documents\test_db.mdf;Integrated Security=True;Connect Timeout=30");
        

        public MyFoldersPage()
        {
            InitializeComponent();

            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select docName, fileSize from [table]";
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            listView.ItemsSource = dt.DefaultView;
            con.Close();

            foreach (DataRowView item in listView.Items)
            {
                newList.Add(item.ToString());
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(searchBar.Text))
            {

                listView.Items.Clear();

                for (int i = 0; i < newList.Count; i++)
                {
                    var item = newList[i];
                    if (item.ToLower().Contains(searchBar.Text.ToLower()))
                    {
                        listView.Items.Add(item);
                    }
                }

            }


            else
            {
                listView.Items.Clear();

                for (int i = 0; i < newList.Count; i++)
                {
                    var item = newList[i];
                    listView.Items.Add(item);
                }
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= TextBox_GotFocus;
        }

        private void listView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            String theText = "";
            String selectedText = ((DataRowView)((ListView)sender).SelectedItem)["docName"].ToString();
            fileName.Content = selectedText;
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select text from [table] where docName = '" + selectedText + "'";
            using (SqlDataReader read = cmd.ExecuteReader())
            {
                while (read.Read())
                {
                    theText = (read["text"].ToString());
                }
            }
            
            con.Close();

            byte[] byteArray = Encoding.ASCII.GetBytes(theText);
            
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                TextRange tr = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                tr.Load(ms, DataFormats.Rtf);
                
            }

            listView.Visibility = System.Windows.Visibility.Hidden;
            mainToolBar.Visibility = System.Windows.Visibility.Hidden;
            rtbEditor.Visibility = System.Windows.Visibility.Visible;
            secondToolBar.Visibility = System.Windows.Visibility.Visible;
        }

        private void newTextBtn_Click(object sender, RoutedEventArgs e)
        {
            NewFile.Visibility = System.Windows.Visibility.Visible;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            NewFile.Visibility = System.Windows.Visibility.Collapsed;

            String input = textbox1.Text;
            fileName.Content = input;
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into [table] values('" + textbox1.Text + "', '', '')";
            cmd.ExecuteNonQuery();
            con.Close();
            textbox1.Text = String.Empty;

            listView.Visibility = System.Windows.Visibility.Hidden;
            mainToolBar.Visibility = System.Windows.Visibility.Hidden;
            rtbEditor.Visibility = System.Windows.Visibility.Visible;
            secondToolBar.Visibility = System.Windows.Visibility.Visible;
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

        private void colorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            rtbEditor.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, (SolidColorBrush)(new BrushConverter().ConvertFrom(colorPicker.SelectedColor.ToString())));
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            String rtfText;
            TextRange tr = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            using (MemoryStream ms = new MemoryStream())
            {
                tr.Save(ms, DataFormats.Rtf);
                rtfText = Encoding.ASCII.GetString(ms.ToArray());
            }

            byte[] byteArray = Encoding.ASCII.GetBytes(rtfText);
            double fileSize = System.Text.ASCIIEncoding.ASCII.GetByteCount(rtfText);

            var culture = CultureInfo.CurrentUICulture;
            const String format = "#,0.0";
            String fileSizeDisplayed = "";

            if (fileSize < 1024)
            {
                fileSizeDisplayed = fileSize.ToString("#,0", culture);
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

            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update [table] set text = '" + rtfText + "', fileSize = '" + fileSizeDisplayed + "' where docName = '" + fileName.Content + "'";
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Save was done.");
        }

        public void openMSBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.RestoreDirectory = true;
            openFileDialog.DefaultExt = ".doc";
            openFileDialog.Filter = "Word Documents(*.doc;*.docx)|*.doc;*.docx";

            Nullable<bool> result = openFileDialog.ShowDialog();

            if (result == true)
            {
                String filename = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                String fullfilename = System.IO.Path.GetFullPath(openFileDialog.FileName);
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();

                object File = fullfilename;
                object nullobject = System.Reflection.Missing.Value;

                Microsoft.Office.Interop.Word.Application wordApp2 = new Microsoft.Office.Interop.Word.Application();
                wordApp2.DisplayAlerts = Microsoft.Office.Interop.Word.WdAlertLevel.wdAlertsNone;
                Microsoft.Office.Interop.Word.Document docs = wordApp.Documents.Open(ref File, ref nullobject, ref nullobject, ref nullobject, ref nullobject, ref nullobject, ref nullobject, ref nullobject, ref nullobject, ref nullobject, ref nullobject, ref nullobject, ref nullobject, ref nullobject, ref nullobject, ref nullobject);
                docs.ActiveWindow.Selection.WholeStory();
                docs.ActiveWindow.Selection.Copy();
                docs.Close();
                wordApp.Quit();

                fileName.Content = filename;
                rtbEditor.Document.Blocks.Clear();
                rtbEditor.Paste();

                String rtfText;
                TextRange tr = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                using (MemoryStream ms = new MemoryStream())
                {
                    tr.Save(ms, DataFormats.Rtf);
                    rtfText = Encoding.ASCII.GetString(ms.ToArray());
                }

                byte[] byteArray = Encoding.ASCII.GetBytes(rtfText);
                double fileSize = System.Text.ASCIIEncoding.ASCII.GetByteCount(rtfText);

                var culture = CultureInfo.CurrentUICulture;
                const String format = "#,0.0";
                String fileSizeDisplayed = "";

                if (fileSize < 1024)
                {
                    fileSizeDisplayed = fileSize.ToString("#,0", culture);
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
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into [table] values('" + fileName.Content + "', '" + rtfText + "', '" + fileSizeDisplayed + "')";
                cmd.ExecuteNonQuery();
                con.Close();

                listView.Visibility = System.Windows.Visibility.Collapsed;
                mainToolBar.Visibility = System.Windows.Visibility.Collapsed;
                rtbEditor.Visibility = System.Windows.Visibility.Visible;
                secondToolBar.Visibility = System.Windows.Visibility.Visible;
            }
        }
    }
}
