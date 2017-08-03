using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using UserModel;
namespace Cloud.StartupPage
{
    /// <summary>
    /// Interaction logic for StartupPage.xaml
    /// </summary>
    public partial class StartupPage : Page
    {
        private List<String> newList = new List<String>();

        static ConnectionStringSettings conSettings1 = ConfigurationManager.ConnectionStrings["connString1"];
        static ConnectionStringSettings conSettings2 = ConfigurationManager.ConnectionStrings["connString2"];
        static ConnectionStringSettings conSettings3 = ConfigurationManager.ConnectionStrings["connString3"];

        static string connectionString1 = conSettings1.ConnectionString;
        static string connectionString2 = conSettings2.ConnectionString;
        static string connectionString3 = conSettings3.ConnectionString;

        SqlConnection con1 = new SqlConnection(connectionString1);
        SqlConnection con2 = new SqlConnection(connectionString2);
        SqlConnection con3 = new SqlConnection(connectionString3);

        SqlDataReader reader;
        SqlCommand cmd1;
        SqlCommand cmd2;
        SqlCommand cmd3;

        string dtformat = "yyyy-MM-dd HH:mm:ss";
        DataTable dt = new DataTable();
        String currentPage = "";

        static String currentUserName = UserModel.UserModel.currentUserID;
        UserModel.UserModel currentUser = UserModel.UserModel.retrieveUserFromDatabase(currentUserName);

        //DEFAULT CONSTRUCTOR
        public StartupPage()
        {
            InitializeComponent();

            /*ImageBrush imgBrush = new ImageBrush();
             * get image byte from database
             * convert byte to image
             * imgBrush.ImageSource = image
             * ellipse.Fill = imgBrush;
            */
            
            nameBox.Text = currentUserName;

            currentPage = "MyFolders";
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

            openAllConnections();
             
            string sqlQuery = "select Name, sharedBy, lastModified, fileSize from [dbo].[UserFiles1] where Username = '" + currentUserName + "'";
            cmd1 = new SqlCommand(sqlQuery, con1);
            cmd1.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            da.Fill(dt);
            listView.ItemsSource = dt.DefaultView;

            closeAllConnections();
        }

        private void openAllConnections()
        {
            con1.Open();
            con2.Open();
            con3.Open();
        }

        private void closeAllConnections()
        {
            con1.Close();
            con2.Close();
            con3.Close();
        }

        //SEARCH FUNCTION
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            dt.DefaultView.RowFilter = String.Format("Name LIKE '%{0}%'", searchBar.Text);
        }


        //RIGHTCLICK -> OPEN
        private void openClick(object sender, RoutedEventArgs e)
        {
            openFile(((DataRowView)((ListView)sender).SelectedItem)["Name"].ToString());
        }


        //RIGHTCLICK -> FAVORITE
        private void favoriteClick(object sender, RoutedEventArgs e)
        {
            String selectedText = ((DataRowView)listView.SelectedItem)["Name"].ToString();

            openAllConnections();

            string sqlQuery1 = "update [dbo].[UserFiles1] set isFavorite = 'yes' where Name = '" + selectedText + "'";
            SqlCommand cmd1 = new SqlCommand(sqlQuery1, con1);
            string sqlQuery2 = "update [dbo].[UserFiles3] set isFavorite = 'yes' where Name = '" + selectedText + "'";
            SqlCommand cmd2 = new SqlCommand(sqlQuery2, con2);
            string sqlQuery3 = "update [dbo].[UserFiles2] set isFavorite = 'yes' where Name = '" + selectedText + "'";
            SqlCommand cmd3 = new SqlCommand(sqlQuery3, con3);

            cmd1.ExecuteNonQuery();
            cmd2.ExecuteNonQuery();
            cmd3.ExecuteNonQuery();

            closeAllConnections();

            sortNormally();
        }

        private void unfavoriteClick(object sender, RoutedEventArgs e)
        {
            String selectedText = ((DataRowView)listView.SelectedItem)["docName"].ToString();

            openAllConnections();

            string sqlQuery1 = "update [dbo].[UserFiles1] set isFavorite = 'no' where Name = '" + selectedText + "'";
            SqlCommand cmd1 = new SqlCommand(sqlQuery1, con1);
            string sqlQuery2 = "update [dbo].[UserFiles3] set isFavorite = 'no' where Name = '" + selectedText + "'";
            SqlCommand cmd2 = new SqlCommand(sqlQuery2, con2);
            string sqlQuery3 = "update [dbo].[UserFiles2] set isFavorite = 'no' where Name = '" + selectedText + "'";
            SqlCommand cmd3 = new SqlCommand(sqlQuery3, con3);

            cmd1.ExecuteNonQuery();
            cmd2.ExecuteNonQuery();
            cmd3.ExecuteNonQuery();

            closeAllConnections();

            sortFavorites();
        }

        //RIGHTCLICK -> DELETE
        private void deleteClick(object sender, RoutedEventArgs e)
        {
            String selectedText = ((DataRowView)listView.SelectedItem)["docName"].ToString();

            openAllConnections();

            if (currentPage == "Bin")
            {
                string sqlQuery1 = "delete from [dbo].[UserFiles1] where Name = '" + selectedText + "'";
                SqlCommand cmd1 = new SqlCommand(sqlQuery1, con1);
                string sqlQuery2 = "delete from [dbo].[UserFiles3] where Name = '" + selectedText + "'";
                SqlCommand cmd2 = new SqlCommand(sqlQuery2, con2);
                string sqlQuery3 = "delete from [dbo].[UserFiles2] where Name = '" + selectedText + "'";
                SqlCommand cmd3 = new SqlCommand(sqlQuery3, con3);

                cmd1.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                cmd3.ExecuteNonQuery();

                sortBin();
            }

            else {
                string sqlQuery1 = "update [dbo].[UserFiles1] set isDeleted = 'yes' where Name = '" + selectedText + "'";
                SqlCommand cmd1 = new SqlCommand(sqlQuery1, con1);
                string sqlQuery2 = "update [dbo].[UserFiles3] set isDeleted = 'yes' where Name = '" + selectedText + "'";
                SqlCommand cmd2 = new SqlCommand(sqlQuery2, con2);
                string sqlQuery3 = "update [dbo].[UserFiles2] set isDeleted = 'yes' where Name = '" + selectedText + "'";
                SqlCommand cmd3 = new SqlCommand(sqlQuery3, con3);

                cmd1.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                cmd3.ExecuteNonQuery();

                if (currentPage == "MyFolders" || currentPage == "Shared")
                {
                    sortNormally();
                }

                else if (currentPage == "Favorites")
                {
                    sortFavorites();
                }
            }

            closeAllConnections();
        }

        private void recoverClick(object sender, RoutedEventArgs e)
        {
            String selectedText = ((DataRowView)listView.SelectedItem)["Name"].ToString();

            openAllConnections();

            string sqlQuery1 = "update [dbo].[UserFiles1] set isDeleted = 'no' where Name = '" + selectedText + "'";
            SqlCommand cmd1 = new SqlCommand(sqlQuery1, con1);
            string sqlQuery2 = "update [dbo].[UserFiles3] set isDeleted = 'no' where Name = '" + selectedText + "'";
            SqlCommand cmd2 = new SqlCommand(sqlQuery2, con2);
            string sqlQuery3 = "update [dbo].[UserFiles2] set isDeleted = 'no' where Name = '" + selectedText + "'";
            SqlCommand cmd3 = new SqlCommand(sqlQuery3, con3);

            closeAllConnections();

            sortBin();
        }

        private void openFile(String selectedText)
        {
            bool doThis = false;
            String theText = "";
            String ext = "";

            openAllConnections();

            String sqlQuery = "select fileType from [dbo].[UserFiles1] where Name = '" + selectedText + "'";
            SqlCommand cmd = new SqlCommand(sqlQuery, con1);

            reader = cmd.ExecuteReader();
            reader.Read();

            ext = (reader[0].ToString());


            if (ext == ".doc" || ext == ".docx")
            {
                fileName.Content = selectedText;

                string sqlQuery1 = "select [File] from [dbo].[UserFiles1] where Name = '" + selectedText + "'";
                SqlCommand cmd1 = new SqlCommand(sqlQuery1, con1);
                string sqlQuery2 = "select [File] from [dbo].[UserFiles3] where Name = '" + selectedText + "'";
                SqlCommand cmd2 = new SqlCommand(sqlQuery2, con2);
                string sqlQuery3 = "select [File] from [dbo].[UserFiles2] where Name = '" + selectedText + "'";
                SqlCommand cmd3 = new SqlCommand(sqlQuery3, con3);

                SqlDataReader Reader1 = cmd1.ExecuteReader();
                SqlDataReader Reader2 = cmd2.ExecuteReader();
                SqlDataReader Reader3 = cmd3.ExecuteReader();

                try
                {
                    byte[] retrieve1 = ((byte[])Reader1[0]);
                    byte[] retrieve2 = ((byte[])Reader2[0]);
                    byte[] retrieve3 = ((byte[])Reader3[0]);

                    byte[] retrieve = retrieve1.Concat(retrieve3).Concat(retrieve2).ToArray();

                    theText = System.Text.Encoding.ASCII.GetString(retrieve);
                    doThis = true;
                }


                catch (InvalidCastException)
                {
                    theText = "";
                    doThis = false;
                }


                if (doThis == true)
                {
                    byte[] byteArray = Encoding.ASCII.GetBytes(theText);

                    using (MemoryStream ms = new MemoryStream(byteArray))
                    {
                        TextRange tr = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                        tr.Load(ms, DataFormats.Rtf);

                    }
                }


                listView.Visibility = System.Windows.Visibility.Collapsed;
                mainToolBar.Visibility = System.Windows.Visibility.Collapsed;
                rtbEditor.Visibility = System.Windows.Visibility.Visible;
                secondToolBar.Visibility = System.Windows.Visibility.Visible;
            }

            else if (ext == ".ppt" || ext == ".pptx")
            {
                string sqlQuery1 = "select [File] from [dbo].[UserFiles1] where Name = '" + selectedText + "'";
                SqlCommand cmd1 = new SqlCommand(sqlQuery1, con1);
                string sqlQuery2 = "select [File] from [dbo].[UserFiles3] where Name = '" + selectedText + "'";
                SqlCommand cmd2 = new SqlCommand(sqlQuery2, con2);
                string sqlQuery3 = "select [File] from [dbo].[UserFiles2] where Name = '" + selectedText + "'";
                SqlCommand cmd3 = new SqlCommand(sqlQuery3, con3);

                SqlDataReader Reader1 = cmd1.ExecuteReader();
                SqlDataReader Reader2 = cmd2.ExecuteReader();
                SqlDataReader Reader3 = cmd3.ExecuteReader();
               
                byte[] retrieve1 = ((byte[])Reader1[0]);
                byte[] retrieve2 = ((byte[])Reader2[0]);
                byte[] retrieve3 = ((byte[])Reader3[0]);

                byte[] retrieve = retrieve1.Concat(retrieve3).Concat(retrieve2).ToArray();

                File.WriteAllBytes("temp.ppt", retrieve);
                Process process = new Process();
                process.StartInfo.FileName = "temp.ppt";
                process.StartInfo.UseShellExecute = true;
                process.Start();
                process.EnableRaisingEvents = true;
                process.Exited += (sender2, eventArgs) =>
                {
                    byte[] file;
                    using (var stream = new FileStream("temp.ppt", FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = new BinaryReader(stream))
                        {
                            file = reader.ReadBytes((int)stream.Length);
                        }
                    }

                    int len = file.Length / 3;
                    byte[] toSend1 = file.Take(len).ToArray();
                    byte[] toSend2 = file.Skip(len).Take(len).ToArray();
                    int len2 = len + len;
                    byte[] toSend3 = file.Skip(len2).Take(len).ToArray();

                    sqlQuery1 = "update [dbo].[UserFiles1] set [File] = @toSend1, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "'";
                    cmd1 = new SqlCommand(sqlQuery1, con1);
                    SqlParameter para1 = new SqlParameter("@toSend1", toSend1);
                    cmd1.Parameters.Add(para1);
                    sqlQuery2 = "update [dbo].[UserFiles2] set [File] = @toSend2, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "'";
                    cmd2 = new SqlCommand(sqlQuery2, con2);
                    SqlParameter para2 = new SqlParameter("@toSend1", toSend1);
                    cmd2.Parameters.Add(para2);
                    sqlQuery3 = "update [dbo].[UserFiles3] set [File] = @toSend3, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "'";
                    cmd3 = new SqlCommand(sqlQuery3, con3);
                    SqlParameter para3 = new SqlParameter("@toSend1", toSend1);
                    cmd3.Parameters.Add(para3);

                    cmd1.ExecuteNonQuery();
                    cmd2.ExecuteNonQuery();
                    cmd3.ExecuteNonQuery();

                    File.Delete("temp.ppt");
                };
            }   

            else if (ext == ".xlsx")
            {
                string sqlQuery1 = "select [File] from [dbo].[UserFiles1] where Name = '" + selectedText + "'";
                SqlCommand cmd1 = new SqlCommand(sqlQuery1, con1);
                string sqlQuery2 = "select [File] from [dbo].[UserFiles3] where Name = '" + selectedText + "'";
                SqlCommand cmd2 = new SqlCommand(sqlQuery2, con2);
                string sqlQuery3 = "select [File] from [dbo].[UserFiles2] where Name = '" + selectedText + "'";
                SqlCommand cmd3 = new SqlCommand(sqlQuery3, con3);

                SqlDataReader Reader1 = cmd1.ExecuteReader();
                SqlDataReader Reader2 = cmd2.ExecuteReader();
                SqlDataReader Reader3 = cmd3.ExecuteReader();

                byte[] retrieve1 = ((byte[])Reader1[0]);
                byte[] retrieve2 = ((byte[])Reader2[0]);
                byte[] retrieve3 = ((byte[])Reader3[0]);

                byte[] retrieve = retrieve1.Concat(retrieve3).Concat(retrieve2).ToArray();

                File.WriteAllBytes("temp.ppt", retrieve);
                Process process = new Process();
                process.StartInfo.FileName = "temp.ppt";
                process.StartInfo.UseShellExecute = true;
                process.Start();
                process.EnableRaisingEvents = true;
                process.Exited += (sender2, eventArgs) =>
                {
                    byte[] file;
                    using (var stream = new FileStream("temp.ppt", FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = new BinaryReader(stream))
                        {
                            file = reader.ReadBytes((int)stream.Length);
                        }
                    }

                    int len = file.Length / 3;
                    byte[] toSend1 = file.Take(len).ToArray();
                    byte[] toSend2 = file.Skip(len).Take(len).ToArray();
                    int len2 = len + len;
                    byte[] toSend3 = file.Skip(len2).Take(len).ToArray();

                    sqlQuery1 = "update [dbo].[UserFiles1] set [File] = @toSend1, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "'";
                    cmd1 = new SqlCommand(sqlQuery1, con1);
                    SqlParameter para1 = new SqlParameter("@toSend1", toSend1);
                    cmd1.Parameters.Add(para1);
                    sqlQuery2 = "update [dbo].[UserFiles2] set [File] = @toSend2, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "'";
                    cmd2 = new SqlCommand(sqlQuery2, con2);
                    SqlParameter para2 = new SqlParameter("@toSend2", toSend2);
                    cmd2.Parameters.Add(para2);
                    sqlQuery3 = "update [dbo].[UserFiles3] set [File] = @toSend3, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "'";
                    cmd3 = new SqlCommand(sqlQuery3, con3);
                    SqlParameter para3 = new SqlParameter("@toSend3", toSend3);
                    cmd3.Parameters.Add(para3);

                    cmd1.ExecuteNonQuery();
                    cmd2.ExecuteNonQuery();
                    cmd3.ExecuteNonQuery();

                    File.Delete("temp.ppt");
                };
            }
            closeAllConnections();
        }
        
        //CLICK ON LIST VIEW ITEM TO OPEN
        private void listView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            openFile(((DataRowView)((ListView)sender).SelectedItem)["Name"].ToString());
        }


        //CREATE NEW FILE
        private void newTextBtn_Click(object sender, RoutedEventArgs e)
        {
            NewFile.Visibility = System.Windows.Visibility.Visible;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            NewFile.Visibility = System.Windows.Visibility.Collapsed;

            String input = textbox1.Text;
            fileName.Content = input;
            openAllConnections();

            string sqlQuery1 = ("insert into [dbo].[UserFiles1] values('" + textbox1.Text + "', @Null, '', '" + getCurrent() + "', 'no', 'no', '.doc')");
            cmd1 = new SqlCommand(sqlQuery1, con1);
            cmd1.Parameters.Add("@Null", SqlDbType.VarBinary, -1);
            cmd1.Parameters["@Null"].Value = DBNull.Value;
            string sqlQuery2 = ("insert into [dbo].[UserFiles2] values('" + textbox1.Text + "', @Null, '', '" + getCurrent() + "', 'no', 'no', '.doc')");
            cmd2 = new SqlCommand(sqlQuery2, con2);
            cmd2.Parameters.Add("@Null", SqlDbType.VarBinary, -1);
            cmd2.Parameters["@Null"].Value = DBNull.Value;
            string sqlQuery3 = ("insert into [dbo].[UserFiles3] values('" + textbox1.Text + "', @Null, '', '" + getCurrent() + "', 'no', 'no', '.doc')");
            cmd3 = new SqlCommand(sqlQuery3, con3);
            cmd3.Parameters.Add("@Null", SqlDbType.VarBinary, -1);
            cmd3.Parameters["@Null"].Value = DBNull.Value;

            cmd1.ExecuteNonQuery();
            cmd2.ExecuteNonQuery();
            cmd3.ExecuteNonQuery();

            textbox1.Text = String.Empty;

            closeAllConnections();

            listView.Visibility = System.Windows.Visibility.Collapsed;
            mainToolBar.Visibility = System.Windows.Visibility.Collapsed;
            rtbEditor.Visibility = System.Windows.Visibility.Visible;
            secondToolBar.Visibility = System.Windows.Visibility.Visible;
        }


        //CANCEL 
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NewFile.Visibility = System.Windows.Visibility.Collapsed;
            textbox1.Text = String.Empty;
        }
        

        //SET FONT FAMILY THINGS
        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null)
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
        }


        //SET FONT SIZE THINGS
        private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            rtbEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);
        }


        //TEXT EDITOR FUNCTIONS
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


        //COLORPICKER
        private void colorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            rtbEditor.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, (SolidColorBrush)(new BrushConverter().ConvertFrom(colorPicker.SelectedColor.ToString())));
        }


        //SAVE ANY CHANGES MADE TO DOCUMENT
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
            double fileSize = byteArray.Length;

            openAllConnections();

            int len = byteArray.Length / 3;
            byte[] toSend1 = byteArray.Take(len).ToArray();
            byte[] toSend2 = byteArray.Skip(len).Take(len).ToArray();
            int len2 = len + len;
            byte[] toSend3 = byteArray.Skip(len2).Take(len).ToArray();

            string sqlQuery1 = "update [dbo].[UserFiles1] set [File] = @toSend1, fileSize = '" + getFileSize(byteArray.Length) + "', lastModified = '" + getCurrent() + "'";
            cmd1 = new SqlCommand(sqlQuery1, con1);
            SqlParameter para1 = new SqlParameter("@toSend1", toSend1);
            cmd1.Parameters.Add(para1);
            string sqlQuery2 = "update [dbo].[UserFiles2] set [File] = @toSend2, fileSize = '" + getFileSize(byteArray.Length) + "', lastModified = '" + getCurrent() + "'";
            cmd2 = new SqlCommand(sqlQuery2, con2);
            SqlParameter para2 = new SqlParameter("@toSend2", toSend2);
            cmd2.Parameters.Add(para2);
            string sqlQuery3 = "update [dbo].[UserFiles3] set [File] = @toSend3, fileSize = '" + getFileSize(byteArray.Length) + "', lastModified = '" + getCurrent() + "'";
            cmd3 = new SqlCommand(sqlQuery3, con3);
            SqlParameter para3 = new SqlParameter("@toSend3", toSend3);
            cmd3.Parameters.Add(para3);

            cmd1.ExecuteNonQuery();
            cmd2.ExecuteNonQuery();
            cmd3.ExecuteNonQuery();

            closeAllConnections();

            MessageBox.Show("Save was done.");
        }


        //DOWNLOAD FILE AS MS WORD DOC
        public void TestDownload(object sender, RoutedEventArgs e)
        {
            String theText = "";
            String selectedText = ((DataRowView)listView.SelectedItem)["Name"].ToString();

            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            object nullobject = System.Reflection.Missing.Value;
            object start = 0;
            Microsoft.Office.Interop.Word.Document doc = wordApp.Documents.Add(ref nullobject, ref nullobject, ref nullobject, ref nullobject);

            openAllConnections();

            string sqlQuery1 = "select [File] from [dbo].[UserFiles1] where Name = '" + selectedText + "'";
            cmd1 = new SqlCommand(sqlQuery1, con1);
            string sqlQuery2 = "select [File] from [dbo].[UserFiles3] where Name = '" + selectedText + "'";
            cmd2 = new SqlCommand(sqlQuery2, con2);
            string sqlQuery3 = "select [File] from [dbo].[UserFiles2] where Name = '" + selectedText + "'";
            cmd3 = new SqlCommand(sqlQuery3, con3);

            SqlDataReader Reader1 = cmd1.ExecuteReader();
            SqlDataReader Reader2 = cmd2.ExecuteReader();
            SqlDataReader Reader3 = cmd3.ExecuteReader();

            byte[] retrieve1 = ((byte[])Reader1[0]);
            byte[] retrieve2 = ((byte[])Reader2[0]);
            byte[] retrieve3 = ((byte[])Reader3[0]);

            byte[] retrieve = retrieve1.Concat(retrieve3).Concat(retrieve2).ToArray();

            theText = Encoding.ASCII.GetString(retrieve);

            closeAllConnections();

            Clipboard.SetText(theText, TextDataFormat.Rtf);
            Microsoft.Office.Interop.Word.Range rng = doc.Range(ref start, ref nullobject);

            wordApp.Selection.Paste();
            String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            object filename = path + @"\" + selectedText + ".doc";
            doc.SaveAs2(filename);

            MessageBox.Show("Downloaded.");
        }


        //UPLOAD AND OPEN PPT FROM FILE EXPLORER
        public void openPPTBtn_Click(Object sender, RoutedEventArgs e)
        {
            if (Layout.Controllers.Prompt.ShowDialog2("Do you want to ensure file integrity? (Includes extra steps)", "Prompt") == true)
            {
                NavigationService.Navigate(new Uri("Page1.xaml", UriKind.RelativeOrAbsolute));
            }

            else {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.RestoreDirectory = true;
                openFileDialog.DefaultExt = ".ppt";
                openFileDialog.Filter = "PowerPoint Presentations(*.ppt;*.pptx)|*.ppt;*.pptx";

                Nullable<bool> result = openFileDialog.ShowDialog();

                if (result == true)
                {
                    String filename = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                    String fullfilename = System.IO.Path.GetFullPath(openFileDialog.FileName);
                    String extension = System.IO.Path.GetExtension(openFileDialog.FileName);

                    byte[] file;
                    using (var stream = new FileStream(fullfilename, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = new BinaryReader(stream))
                        {
                            file = reader.ReadBytes((int)stream.Length);
                        }
                    }

                    //encrypt file with seans method
                    //send encrypted file to model
                    //do navigation here
                    
                    File.WriteAllBytes("temp.ppt", file);

                    Process process = new Process();
                    process.StartInfo.FileName = "temp.ppt";
                    process.StartInfo.UseShellExecute = true;
                    process.Start();
                    process.EnableRaisingEvents = true;

                    int len = file.Length / 3;
                    byte[] toSend1 = file.Take(len).ToArray();
                    byte[] toSend2 = file.Skip(len).Take(len).ToArray();
                    int len2 = len + len;
                    byte[] toSend3 = file.Skip(len2).Take(len).ToArray();

                    openAllConnections();

                    string sqlQuery1 = ("insert into [dbo].[UserFiles1] values('" + filename + "', @toSend1, '" + getFileSize(file.Length) + "', '" + getCurrent() + "', 'no', 'no', '" + extension + "')");
                    cmd1 = new SqlCommand(sqlQuery1, con1);
                    SqlParameter para1 = new SqlParameter("@toSend1", toSend1);
                    cmd1.Parameters.Add(para1);
                    string sqlQuery2 = ("insert into [dbo].[UserFiles2] values('" + filename + "', @toSend2, '" + getFileSize(file.Length) + "', '" + getCurrent() + "', 'no', 'no', '" + extension + "')");
                    cmd2 = new SqlCommand(sqlQuery2, con2);
                    SqlParameter para2 = new SqlParameter("@toSend2", toSend2);
                    cmd2.Parameters.Add(para2);
                    string sqlQuery3 = ("insert into [dbo].[UserFiles3] values('" + filename + "', @toSend3, '" + getFileSize(file.Length) + "', '" + getCurrent() + "', 'no', 'no', '" + extension + "')");
                    cmd3 = new SqlCommand(sqlQuery3, con3);
                    SqlParameter para3 = new SqlParameter("@toSend3", toSend3);
                    cmd3.Parameters.Add(para3);

                    cmd1.ExecuteNonQuery();
                    cmd2.ExecuteNonQuery();
                    cmd3.ExecuteNonQuery();

                    process.Exited += (sender2, eventArgs) =>
                    {
                        using (var stream = new FileStream("temp.ppt", FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = new BinaryReader(stream))
                            {
                                file = reader.ReadBytes((int)stream.Length);
                            }
                        }

                        len = file.Length / 3;
                        toSend1 = file.Take(len).ToArray();
                        toSend2 = file.Skip(len).Take(len).ToArray();
                        len2 = len + len;
                        toSend3 = file.Skip(len2).Take(len).ToArray();

                        sqlQuery1 = "update [dbo].[UserFiles1] set [File] = @toSend1, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "'";
                        cmd1 = new SqlCommand(sqlQuery1, con1);
                        para1 = new SqlParameter("@toSend1", toSend1);
                        cmd1.Parameters.Add(para1);
                        sqlQuery2 = "update [dbo].[UserFiles2] set [File] = @toSend2, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "'";
                        cmd2 = new SqlCommand(sqlQuery2, con2);
                        para2 = new SqlParameter("@toSend2", toSend2);
                        cmd2.Parameters.Add(para2);
                        sqlQuery3 = "update [dbo].[UserFiles3] set [File] = @toSend3, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "'";
                        cmd3 = new SqlCommand(sqlQuery3, con3);
                        para3 = new SqlParameter("@toSend3", toSend3);
                        cmd3.Parameters.Add(para3);

                        cmd1.ExecuteNonQuery();
                        cmd2.ExecuteNonQuery();
                        cmd3.ExecuteNonQuery();

                        File.Delete("temp.ppt");
                    };
                    closeAllConnections();
                }
            }
        }


        //UPLOAD AND OPEN MS WORD DOC FROM FILE EXPLORER
        public void openMSBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Layout.Controllers.Prompt.ShowDialog2("Do you want to ensure file integrity? (Includes extra steps)", "Prompt") == true)
            {
                NavigationService.Navigate(new Uri("Page1.xaml", UriKind.RelativeOrAbsolute));
            }

            else
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
                    String extension = System.IO.Path.GetExtension(openFileDialog.FileName);

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

                    //encrypt file with seans method
                    //send encrypted file to model
                    //do navigation here

                    byte[] byteArray = Encoding.ASCII.GetBytes(rtfText);
                    double fileSize = byteArray.Length;

                    int len = byteArray.Length / 3;
                    byte[] toSend1 = byteArray.Take(len).ToArray();
                    byte[] toSend2 = byteArray.Skip(len).Take(len).ToArray();
                    int len2 = len + len;
                    byte[] toSend3 = byteArray.Skip(len2).Take(len).ToArray();

                    openAllConnections();

                    string sqlQuery1 = ("insert into [dbo].[UserFiles1] values('" + filename + "', @toSend1, '" + getFileSize(byteArray.Length) + "', '" + getCurrent() + "', 'no', 'no', '" + extension + "')");
                    cmd1 = new SqlCommand(sqlQuery1, con1);
                    SqlParameter para1 = new SqlParameter("@toSend1", toSend1);
                    cmd1.Parameters.Add(para1);
                    string sqlQuery2 = ("insert into [dbo].[UserFiles2] values('" + filename + "', @toSend2, '" + getFileSize(byteArray.Length) + "', '" + getCurrent() + "', 'no', 'no', '" + extension + "')");
                    cmd2 = new SqlCommand(sqlQuery2, con2);
                    SqlParameter para2 = new SqlParameter("@toSend2", toSend2);
                    cmd2.Parameters.Add(para2);
                    string sqlQuery3 = ("insert into [dbo].[UserFiles3] values('" + filename + "', @toSend3, '" + getFileSize(byteArray.Length) + "', '" + getCurrent() + "', 'no', 'no', '" + extension + "')");
                    cmd3 = new SqlCommand(sqlQuery3, con3);
                    SqlParameter para3 = new SqlParameter("@toSend3", toSend3);
                    cmd3.Parameters.Add(para3);

                    cmd1.ExecuteNonQuery();
                    cmd2.ExecuteNonQuery();
                    cmd3.ExecuteNonQuery();

                    closeAllConnections();

                    listView.Visibility = System.Windows.Visibility.Collapsed;
                    mainToolBar.Visibility = System.Windows.Visibility.Collapsed;
                    rtbEditor.Visibility = System.Windows.Visibility.Visible;
                    secondToolBar.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }


        //UPLOAD AND OPEN EXCEL FILE FROM FILE EXPLORER
        private void openEXLBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Layout.Controllers.Prompt.ShowDialog2("Do you want to ensure file integrity? (Includes extra steps)", "Prompt") == true)
            {
                NavigationService.Navigate(new Uri("Page1.xaml", UriKind.RelativeOrAbsolute));
            }

            else
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.RestoreDirectory = true;
                openFileDialog.DefaultExt = ".xlsx";
                openFileDialog.Filter = "Excel Files(*.xls;*.xlsx;*.xlsm)|*.xls;*.xlsx;*.xlsm";

                Nullable<bool> result = openFileDialog.ShowDialog();

                if (result == true)
                {
                    String filename = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                    String fullfilename = System.IO.Path.GetFullPath(openFileDialog.FileName);
                    String extension = System.IO.Path.GetExtension(openFileDialog.FileName);

                    byte[] file;
                    using (var stream = new FileStream(fullfilename, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = new BinaryReader(stream))
                        {
                            file = reader.ReadBytes((int)stream.Length);
                        }
                    }

                    //encrypt file with seans method
                    //send encrypted file to model
                    //do navigation here

                    File.WriteAllBytes("temp.xlsx", file);

                    Process process = new Process();
                    process.StartInfo.FileName = "temp.xlsx";
                    process.StartInfo.UseShellExecute = true;
                    process.Start();
                    process.EnableRaisingEvents = true;

                    int len = file.Length / 3;
                    byte[] toSend1 = file.Take(len).ToArray();
                    byte[] toSend2 = file.Skip(len).Take(len).ToArray();
                    int len2 = len + len;
                    byte[] toSend3 = file.Skip(len2).Take(len).ToArray();

                    openAllConnections();

                    string sqlQuery1 = ("insert into [dbo].[UserFiles1] values('" + filename + "', @toSend1, '" + getFileSize(file.Length) + "', '" + getCurrent() + "', 'no', 'no', '" + extension + "')");
                    cmd1 = new SqlCommand(sqlQuery1, con1);
                    SqlParameter para1 = new SqlParameter("@toSend1", toSend1);
                    cmd1.Parameters.Add(para1);
                    string sqlQuery2 = ("insert into [dbo].[UserFiles2] values('" + filename + "', @toSend2, '" + getFileSize(file.Length) + "', '" + getCurrent() + "', 'no', 'no', '" + extension + "')");
                    cmd2 = new SqlCommand(sqlQuery2, con2);
                    SqlParameter para2 = new SqlParameter("@toSend2", toSend2);
                    cmd2.Parameters.Add(para2);
                    string sqlQuery3 = ("insert into [dbo].[UserFiles3] values('" + filename + "', @toSend3, '" + getFileSize(file.Length) + "', '" + getCurrent() + "', 'no', 'no', '" + extension + "')");
                    cmd3 = new SqlCommand(sqlQuery3, con3);
                    SqlParameter para3 = new SqlParameter("@toSend3", toSend3);
                    cmd3.Parameters.Add(para3);

                    cmd1.ExecuteNonQuery();
                    cmd2.ExecuteNonQuery();
                    cmd3.ExecuteNonQuery();

                    process.Exited += (sender2, eventArgs) =>
                    {
                        using (var stream = new FileStream("temp.xlsx", FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = new BinaryReader(stream))
                            {
                                file = reader.ReadBytes((int)stream.Length);
                            }
                        }

                        len = file.Length / 3;
                        toSend1 = file.Take(len).ToArray();
                        toSend2 = file.Skip(len).Take(len).ToArray();
                        len2 = len + len;
                        toSend3 = file.Skip(len2).Take(len).ToArray();

                        sqlQuery1 = "update [dbo].[UserFiles1] set [File] = @toSend1, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "'";
                        cmd1 = new SqlCommand(sqlQuery1, con1);
                        para1 = new SqlParameter("@toSend1", toSend1);
                        cmd1.Parameters.Add(para1);
                        sqlQuery2 = "update [dbo].[UserFiles2] set [File] = @toSend2, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "'";
                        cmd2 = new SqlCommand(sqlQuery2, con2);
                        para2 = new SqlParameter("@toSend2", toSend2);
                        cmd2.Parameters.Add(para2);
                        sqlQuery3 = "update [dbo].[UserFiles3] set [File] = @toSend3, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "'";
                        cmd3 = new SqlCommand(sqlQuery3, con3);
                        para3 = new SqlParameter("@toSend3", toSend3);
                        cmd3.Parameters.Add(para3);

                        cmd1.ExecuteNonQuery();
                        cmd2.ExecuteNonQuery();
                        cmd3.ExecuteNonQuery();

                        File.Delete("temp.xlsx");
                    };
                    closeAllConnections();
                }
            }
        }


        private void MyFoldersButton_Click(object sender, RoutedEventArgs e)
        {
            unfavoriteHeader.Visibility = Visibility.Collapsed;
            recoverHeader.Visibility = Visibility.Collapsed;
            deleteHeader.Visibility = Visibility.Visible;

            currentPage = "MyFolders";
            sortNormally();

            MyFoldersButton.Background = (Brush)(new BrushConverter().ConvertFrom("#23aeff"));
            RecentButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            SharedButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            FavoritesButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            BinButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));

        }


        private void RecentButton_Click(object sender, RoutedEventArgs e)
        {
            unfavoriteHeader.Visibility = Visibility.Collapsed;
            recoverHeader.Visibility = Visibility.Collapsed;
            deleteHeader.Visibility = Visibility.Visible;

            currentPage = "Recent";
            sortByLastModified();

            MyFoldersButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            RecentButton.Background = (Brush)(new BrushConverter().ConvertFrom("#23aeff"));
            SharedButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            FavoritesButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            BinButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
        }


        private void SharedButton_Click(object sender, RoutedEventArgs e)
        {
            unfavoriteHeader.Visibility = Visibility.Collapsed;
            recoverHeader.Visibility = Visibility.Collapsed;
            deleteHeader.Visibility = Visibility.Visible;

            currentPage = "Shared";
            sortNormally();

            MyFoldersButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            RecentButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            SharedButton.Background = (Brush)(new BrushConverter().ConvertFrom("#23aeff"));
            FavoritesButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            BinButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
        }


        private void FavoritesButton_Click(object sender, RoutedEventArgs e)
        {
            unfavoriteHeader.Visibility = Visibility.Visible;
            recoverHeader.Visibility = Visibility.Collapsed;
            deleteHeader.Visibility = Visibility.Visible;

            currentPage = "Favorites";
            sortFavorites();

            MyFoldersButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            RecentButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            SharedButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            FavoritesButton.Background = (Brush)(new BrushConverter().ConvertFrom("#23aeff"));
            BinButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
        }


        private void BinButton_Click(object sender, RoutedEventArgs e)
        {
            unfavoriteHeader.Visibility = Visibility.Collapsed;
            recoverHeader.Visibility = Visibility.Visible;
            deleteHeader.Visibility = Visibility.Collapsed;

            currentPage = "Bin";
            sortBin();

            MyFoldersButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            RecentButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            SharedButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            FavoritesButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            BinButton.Background = (Brush)(new BrushConverter().ConvertFrom("#23aeff"));
        }


        private void sortByLastModified()
        {
            openAllConnections();

            string sqlQuery = "select Name, sharedBy, lastModified, fileSize from [dbo].[UserFiles1] where Username = '" + currentUserName + "' and isDeleted = 'no'";
            cmd1 = new SqlCommand(sqlQuery, con1);
            cmd1.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            dt.Clear();
            da.Fill(dt);
            listView.ItemsSource = dt.DefaultView;

            closeAllConnections();

            ICollectionView view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("lastModified", ListSortDirection.Descending));
        }


        private void sortNormally()
        {
            openAllConnections();

            string sqlQuery = "select Name, sharedBy, lastModified, fileSize from [dbo].[UserFiles1] where Username = '" + currentUserName + "' and isDeleted = 'no'";
            cmd1 = new SqlCommand(sqlQuery, con1);
            cmd1.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            dt.Clear();
            da.Fill(dt);
            listView.ItemsSource = dt.DefaultView;
            
            closeAllConnections();

            ICollectionView view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            view.SortDescriptions.Clear();
        }


        private void sortFavorites()
        {
            openAllConnections();

            string sqlQuery = "select Name, sharedBy, lastModified, fileSize from [dbo].[UserFiles1] where Username = '" + currentUserName + "' and isDeleted = 'no' and isFavorite = 'yes'";
            cmd1 = new SqlCommand(sqlQuery, con1);
            cmd1.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            dt.Clear();
            da.Fill(dt);
            listView.ItemsSource = dt.DefaultView;

            closeAllConnections();

            ICollectionView view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            view.SortDescriptions.Clear();
        }


        private void sortBin()
        {
            openAllConnections();

            string sqlQuery = "select Name, sharedBy, lastModified, fileSize from [dbo].[UserFiles1] where Username = '" + currentUserName + "' and isDeleted = 'yes'";
            cmd1 = new SqlCommand(sqlQuery, con1);
            cmd1.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            dt.Clear();
            da.Fill(dt);
            listView.ItemsSource = dt.DefaultView;

            closeAllConnections();

            ICollectionView view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            view.SortDescriptions.Clear();
        }


        private String getCurrent()
        {
            DateTime current = new DateTime();
            current = DateTime.Now;
            return current.ToString(dtformat);
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

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {

        }
    }
}

