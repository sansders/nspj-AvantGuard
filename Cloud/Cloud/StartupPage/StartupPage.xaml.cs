using Layout.Models;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Cloud.StartupPage
{
    /// <summary>
    /// Interaction logic for StartupPage.xaml
    /// </summary>
    public partial class StartupPage : Page
    {
        private List<String> newList = new List<String>();

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

        String path = "";
        String fullAdd = "";
        String updatedAdd = "";

        string parent;
        string storage;
        string storage2;
        string dtformat = "yyyy-MM-dd HH:mm:ss";
        DataTable dt = new DataTable();
        String currentPage = "";

        static String currentUserName = UserModel.UserModel.currentUserID;
        UserModel.UserModel currentUser = UserModel.UserModel.retrieveUserFromDatabase(currentUserName);

        Layout.Controllers.KeyController kc = new Layout.Controllers.KeyController();

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

            inWhere.Text = currentPage;

            openAllConnections();

            string sqlQuery = "select Name, sharedBy, lastModified, fileSize from [dbo].[UserFiles1] where Username = '" + currentUserName + "'";
            cmd1 = new SqlCommand(sqlQuery, con1);
            cmd1.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            da.Fill(dt);
            listView.ItemsSource = dt.DefaultView;

            FileModel fm = FileModel.getFileModel();

            if(fm == null)
            {

            }
            else {

                if (fm.getShow() == true)
                {
                    byte[] file = fm.ReturnFileBytes();
                    File.WriteAllBytes("temp.ppt", file);

                    Process process = new Process();
                    process.StartInfo.FileName = "temp.ppt";
                    process.StartInfo.UseShellExecute = true;
                    process.Start();
                    process.EnableRaisingEvents = true;

                    process.Exited += (sender2, eventArgs) =>
                    {
                        using (var stream = new FileStream("temp.ppt", FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = new BinaryReader(stream))
                            {
                                file = reader.ReadBytes((int)stream.Length);
                            }
                        }

                    //Sean's Decryption Codes

                    sqlQuery = "SELECT keyPath FROM dbo.test WHERE UserID='" + currentUserName + "'";
                        SqlCommand cmd = new SqlCommand(sqlQuery, con);
                        SqlDataReader DataRead1 = cmd.ExecuteReader();
                        String bigPath = DataRead1.GetString(0);

                    //Gets IV & Encrypted Symmetric Key
                    byte[] IV = System.IO.File.ReadAllBytes(@bigPath + "\\IV.txt");
                        byte[] encryptedSymmetricKey = File.ReadAllBytes(@bigPath + "\\encryptedSymmetricKey.txt");
                        byte[] decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                        byte[] plainText = kc.symmetricDecryption(file, decryptedSymmetricKey, IV);

                        
                        int len = file.Length / 3;
                        byte[] toSend1 = file.Take(len).ToArray();
                        byte[] toSend2 = file.Skip(len).Take(len).ToArray();
                        int len2 = len + len;
                        byte[] toSend3 = file.Skip(len2).Take(len).ToArray();

                        String sqlQuery1 = ("update [dbo].[UserFiles1] set [File] = @toSend1, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "'");
                        cmd1 = new SqlCommand(sqlQuery1, con1);
                        SqlParameter para1 = new SqlParameter("@toSend1", toSend1);
                        cmd1.Parameters.Add(para1);
                        String sqlQuery2 = ("update [dbo].[UserFiles2] set [File] = @toSend2, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "'");
                        cmd2 = new SqlCommand(sqlQuery2, con2);
                        SqlParameter para2 = new SqlParameter("@toSend2", toSend2);
                        cmd2.Parameters.Add(para2);
                        String sqlQuery3 = ("update [dbo].[UserFiles3] set [File] = @toSend3, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "'");
                        cmd3 = new SqlCommand(sqlQuery3, con3);
                        SqlParameter para3 = new SqlParameter("@toSend3", toSend3);
                        cmd3.Parameters.Add(para3);

                        cmd1.ExecuteNonQuery();
                        cmd2.ExecuteNonQuery();
                        cmd3.ExecuteNonQuery();

                        File.Delete("temp.ppt");
                    };
                    
                }
            }
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


        //RIGHTCLICK -> SHARE
        private void shareClick(object sender, RoutedEventArgs e)
        {
            String selectedText = ((DataRowView)listView.SelectedItem)["Name"].ToString();

            con1.Open();
            
            string sqlQuery1 = "select fileType from [dbo].[UserFiles1] where where Username = '" + currentUserName + "' and Name = '" + selectedText + "'";
            cmd1 = new SqlCommand(sqlQuery1, con1);

            SqlDataReader reader1 = cmd1.ExecuteReader();
            reader1.Read();
            string ext = (reader1[0].ToString());

            con1.Close();

            if (ext == ".fol")
            {
                MessageBox.Show("You cannot share a folder.");
            }

            userField.Visibility = System.Windows.Visibility.Visible;
            shareWith.Text = "Share " + selectedText + ext + " with:";
            storage = selectedText;
            storage2 = ext;
        }


        //RIGHTCLICK -> UNFAVORITE
        private void unfavoriteClick(object sender, RoutedEventArgs e)
        {
            String selectedText = ((DataRowView)listView.SelectedItem)["Name"].ToString();

            openAllConnections();

            string sqlQuery1 = "update [dbo].[UserFiles1] set isFavorite = 'no' where Username = '" + currentUserName +"' and Name = '" + selectedText + "'";
            SqlCommand cmd1 = new SqlCommand(sqlQuery1, con1);
            string sqlQuery2 = "update [dbo].[UserFiles3] set isFavorite = 'no' where Username = '" + currentUserName + "' and Name = '" + selectedText + "'";
            SqlCommand cmd2 = new SqlCommand(sqlQuery2, con2);
            string sqlQuery3 = "update [dbo].[UserFiles2] set isFavorite = 'no' where Username = '" + currentUserName + "' and Name = '" + selectedText + "'";
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
            String selectedText = ((DataRowView)listView.SelectedItem)["Name"].ToString();

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

                sqlQuery1 = "delete from [dbo].[AccessControl] where FileName = '" + selectedText + "'";
                cmd1 = new SqlCommand(sqlQuery1, con1);

                cmd1.ExecuteNonQuery();
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


        //RIGHTCLICK -> RECOVER
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

            String sqlQuery = "select fileType from [dbo].[UserFiles1] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
            SqlCommand cmd = new SqlCommand(sqlQuery, con1);

            reader = cmd.ExecuteReader();
            reader.Read();

            ext = (reader[0].ToString());

            if (ext == ".fol")
            { 
                ArrayList list = new ArrayList();
                string sqlQuery1 = "select FileName from [dbo].[AccessControl] where Parent = '" + selectedText + "'";
                cmd1 = new SqlCommand(sqlQuery1, con1);
                reader = cmd1.ExecuteReader();

                if (reader.Read() == true)
                {
                    while (reader.Read())
                    {
                        string currentFile = reader.GetString(0);
                        list.Add(currentFile);
                    }
                }

                dt.Clear();

                for (int i = 0; i < list.Count; i++)
                {
                    sqlQuery = "select Name, sharedBy, lastModified, fileSize from [dbo].[UserFiles1] where Username = '" + currentUserName + "' and Name = '" + list[i] +"'";
                    cmd1 = new SqlCommand(sqlQuery, con1);
                    cmd1.ExecuteNonQuery();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                }

                listView.ItemsSource = dt.DefaultView;

                inWhere.Text = selectedText;
            }

            if (ext == ".doc" || ext == ".docx")
            {
                fileName.Content = selectedText;

                string sqlQuery1 = "select [File] from [dbo].[UserFiles1] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
                cmd1 = new SqlCommand(sqlQuery1, con1);
                string sqlQuery2 = "select [File] from [dbo].[UserFiles3] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
                cmd2 = new SqlCommand(sqlQuery2, con2);
                string sqlQuery3 = "select [File] from [dbo].[UserFiles2] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
                cmd3 = new SqlCommand(sqlQuery3, con3);

                SqlDataReader Reader1 = cmd1.ExecuteReader();
                SqlDataReader Reader2 = cmd2.ExecuteReader();
                SqlDataReader Reader3 = cmd3.ExecuteReader();

                try
                {
                    byte[] retrieve1 = ((byte[])Reader1[0]);
                    byte[] retrieve2 = ((byte[])Reader2[0]);
                    byte[] retrieve3 = ((byte[])Reader3[0]);

                    byte[] retrieve = retrieve1.Concat(retrieve3).Concat(retrieve2).ToArray();

                    //Sean's Decryption Codes
                   
                    sqlQuery = "SELECT keyPath FROM dbo.test WHERE UserID='" + currentUserName + "'";
                    cmd = new SqlCommand(sqlQuery, con);
                    SqlDataReader DataRead1 = cmd.ExecuteReader();
                    string bigPath = DataRead1.GetString(0);

                    //Gets IV & Encrypted Symmetric Key
                    byte[] IV = System.IO.File.ReadAllBytes(@bigPath+"\\IV.txt");
                    byte[] encryptedSymmetricKey = File.ReadAllBytes(@bigPath+"\\encryptedSymmetricKey.txt");
                    byte[] decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                    byte[] plainText = kc.symmetricDecryption(retrieve, decryptedSymmetricKey, IV);


                    theText = System.Text.Encoding.ASCII.GetString(plainText);
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
                string sqlQuery1 = "select [File] from [dbo].[UserFiles1] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
                SqlCommand cmd1 = new SqlCommand(sqlQuery1, con1);
                string sqlQuery2 = "select [File] from [dbo].[UserFiles3] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
                SqlCommand cmd2 = new SqlCommand(sqlQuery2, con2);
                string sqlQuery3 = "select [File] from [dbo].[UserFiles2] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
                SqlCommand cmd3 = new SqlCommand(sqlQuery3, con3);

                SqlDataReader Reader1 = cmd1.ExecuteReader();
                SqlDataReader Reader2 = cmd2.ExecuteReader();
                SqlDataReader Reader3 = cmd3.ExecuteReader();
               
                byte[] retrieve1 = ((byte[])Reader1[0]);
                byte[] retrieve2 = ((byte[])Reader2[0]);
                byte[] retrieve3 = ((byte[])Reader3[0]);

                byte[] retrieve = retrieve1.Concat(retrieve3).Concat(retrieve2).ToArray();

                //Sean's Decryption Codes

                sqlQuery = "SELECT keyPath FROM dbo.test WHERE UserID='" + currentUserName + "'";
                cmd = new SqlCommand(sqlQuery, con);
                SqlDataReader DataRead1 = cmd.ExecuteReader();
                string bigPath = DataRead1.GetString(0);

                //Gets IV & Encrypted Symmetric Key
                byte[] IV = System.IO.File.ReadAllBytes(@bigPath + "\\IV.txt");
                byte[] encryptedSymmetricKey = File.ReadAllBytes(@bigPath + "\\encryptedSymmetricKey.txt");
                byte[] decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                byte[] plainText = kc.symmetricDecryption(retrieve, decryptedSymmetricKey, IV);

                File.WriteAllBytes("temp.ppt", plainText);
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

                    //Sean's encryption codes
                    sqlQuery = "SELECT keyPath FROM dbo.test WHERE UserID='" + currentUserName + "'";
                    cmd = new SqlCommand(sqlQuery, con);
                    DataRead1 = cmd.ExecuteReader();
                    bigPath = DataRead1.GetString(0);

                    IV = System.IO.File.ReadAllBytes(@bigPath + "+\\IV.txt");
                    Console.WriteLine("Gets bytes of IV");
                    encryptedSymmetricKey = System.IO.File.ReadAllBytes(@bigPath + "\\encryptedSymmetricKey.txt");

                    //Gets the symmetric key by decrypting the encrypted symmetric key with the decryption (private) key
                    decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                    //Encrypts plaintext with symmetric key
                    byte[] cipherText = kc.symmetricEncryption(file, decryptedSymmetricKey, IV);

                    int len = file.Length / 3;
                    byte[] toSend1 = file.Take(len).ToArray();
                    byte[] toSend2 = file.Skip(len).Take(len).ToArray();
                    int len2 = len + len;
                    byte[] toSend3 = file.Skip(len2).Take(len).ToArray();

                    sqlQuery1 = "update [dbo].[UserFiles1] set [File] = @toSend1, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "'";
                    cmd1 = new SqlCommand(sqlQuery1, con1);
                    SqlParameter para1 = new SqlParameter("@toSend1", toSend1);
                    cmd1.Parameters.Add(para1);
                    sqlQuery2 = "update [dbo].[UserFiles2] set [File] = @toSend2, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "'";
                    cmd2 = new SqlCommand(sqlQuery2, con2);
                    SqlParameter para2 = new SqlParameter("@toSend1", toSend1);
                    cmd2.Parameters.Add(para2);
                    sqlQuery3 = "update [dbo].[UserFiles3] set [File] = @toSend3, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "'";
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
                string sqlQuery1 = "select [File] from [dbo].[UserFiles1] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
                SqlCommand cmd1 = new SqlCommand(sqlQuery1, con1);
                string sqlQuery2 = "select [File] from [dbo].[UserFiles3] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
                SqlCommand cmd2 = new SqlCommand(sqlQuery2, con2);
                string sqlQuery3 = "select [File] from [dbo].[UserFiles2] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
                SqlCommand cmd3 = new SqlCommand(sqlQuery3, con3);

                SqlDataReader Reader1 = cmd1.ExecuteReader();
                SqlDataReader Reader2 = cmd2.ExecuteReader();
                SqlDataReader Reader3 = cmd3.ExecuteReader();

                byte[] retrieve1 = ((byte[])Reader1[0]);
                byte[] retrieve2 = ((byte[])Reader2[0]);
                byte[] retrieve3 = ((byte[])Reader3[0]);

                byte[] retrieve = retrieve1.Concat(retrieve3).Concat(retrieve2).ToArray();

                //Sean's Decryption Codes

                sqlQuery = "SELECT keyPath FROM dbo.test WHERE UserID='" + currentUserName + "'";
                cmd = new SqlCommand(sqlQuery, con);
                SqlDataReader DataRead1 = cmd.ExecuteReader();
                string bigPath = DataRead1.GetString(0);

                //Gets IV & Encrypted Symmetric Key
                byte[] IV = System.IO.File.ReadAllBytes(@bigPath + "\\IV.txt");
                byte[] encryptedSymmetricKey = File.ReadAllBytes(@bigPath + "\\encryptedSymmetricKey.txt");
                byte[] decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                byte[] plainText = kc.symmetricDecryption(retrieve, decryptedSymmetricKey, IV);

                File.WriteAllBytes("temp.ppt", plainText);
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

                    //Sean's encryption codes
                    sqlQuery = "SELECT keyPath FROM dbo.test WHERE UserID='" + currentUserName + "'";
                    cmd = new SqlCommand(sqlQuery, con);
                    DataRead1 = cmd.ExecuteReader();
                    bigPath = DataRead1.GetString(0);

                    IV = System.IO.File.ReadAllBytes(@bigPath + "+\\IV.txt");
                    Console.WriteLine("Gets bytes of IV");
                    encryptedSymmetricKey = System.IO.File.ReadAllBytes(@bigPath + "\\encryptedSymmetricKey.txt");

                    //Gets the symmetric key by decrypting the encrypted symmetric key with the decryption (private) key
                    decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                    //Encrypts plaintext with symmetric key
                    byte[] cipherText = kc.symmetricEncryption(file, decryptedSymmetricKey, IV);

                    int len = file.Length / 3;
                    byte[] toSend1 = file.Take(len).ToArray();
                    byte[] toSend2 = file.Skip(len).Take(len).ToArray();
                    int len2 = len + len;
                    byte[] toSend3 = file.Skip(len2).Take(len).ToArray();

                    sqlQuery1 = "update [dbo].[UserFiles1] set [File] = @toSend1, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "'";
                    cmd1 = new SqlCommand(sqlQuery1, con1);
                    SqlParameter para1 = new SqlParameter("@toSend1", toSend1);
                    cmd1.Parameters.Add(para1);
                    sqlQuery2 = "update [dbo].[UserFiles2] set [File] = @toSend2, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "'";
                    cmd2 = new SqlCommand(sqlQuery2, con2);
                    SqlParameter para2 = new SqlParameter("@toSend2", toSend2);
                    cmd2.Parameters.Add(para2);
                    sqlQuery3 = "update [dbo].[UserFiles3] set [File] = @toSend3, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "'";
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


        private void OkButton2_Click(object sender, RoutedEventArgs e)
        {
            string toShare = textbox2.Text;

            openAllConnections();

            string sqlQuery = "select count(*) from [dbo].[test] where UserID = '" + toShare + "'";
            SqlCommand cmd = new SqlCommand(sqlQuery, con);
            int count = (int)cmd.ExecuteScalar();

            if (count == 0)
            {
                MessageBox.Show("User doesn't exist!");
            }

            else if (count > 0)
            {
                string sqlQuery1 = "select [File] from [dbo].[UserFiles1] where Name = '" + storage + "' and Username = '" + currentUserName + "'";
                SqlCommand cmd1 = new SqlCommand(sqlQuery1, con1);
                string sqlQuery2 = "select [File] from [dbo].[UserFiles3] where Name = '" + storage + "' and Username = '" + currentUserName + "'";
                SqlCommand cmd2 = new SqlCommand(sqlQuery2, con2);
                string sqlQuery3 = "select [File] from [dbo].[UserFiles2] where Name = '" + storage + "' and Username = '" + currentUserName + "'";
                SqlCommand cmd3 = new SqlCommand(sqlQuery3, con3);

                SqlDataReader Reader1 = cmd1.ExecuteReader();
                SqlDataReader Reader2 = cmd2.ExecuteReader();
                SqlDataReader Reader3 = cmd3.ExecuteReader();

                byte[] retrieve1 = ((byte[])Reader1[0]);
                byte[] retrieve2 = ((byte[])Reader2[0]);
                byte[] retrieve3 = ((byte[])Reader3[0]);

                byte[] retrieve = retrieve1.Concat(retrieve3).Concat(retrieve2).ToArray();


                sqlQuery1 = ("insert into [dbo].[UserFiles1] values('" + toShare + "'. '" + storage + "', @toSend1, '" + getFileSize(retrieve.Length) + "', '" + getCurrent() + "', 'no', 'no', '" + storage2 + "', '" + currentUserName + "')");
                cmd1 = new SqlCommand(sqlQuery1, con1);
                SqlParameter para1 = new SqlParameter("@toSend1", retrieve1);
                cmd1.Parameters.Add(para1);
                sqlQuery2 = ("insert into [dbo].[UserFiles2] values('" + toShare + "', '" + storage + "', @toSend2, '" + getFileSize(retrieve.Length) + "', '" + getCurrent() + "', 'no', 'no', '" + storage2 + "', '" + currentUserName + "')");
                cmd2 = new SqlCommand(sqlQuery2, con2);
                SqlParameter para2 = new SqlParameter("@toSend2", retrieve3);
                cmd2.Parameters.Add(para2);
                sqlQuery3 = ("insert into [dbo].[UserFiles3] values('" + toShare + "', '" + storage + "', @toSend3, '" + getFileSize(retrieve.Length) + "', '" + getCurrent() + "', 'no', 'no', '" + storage2 + "', '" + currentUserName + "')");
                cmd3 = new SqlCommand(sqlQuery3, con3);
                SqlParameter para3 = new SqlParameter("@toSend3", retrieve2);
                cmd3.Parameters.Add(para3);

                cmd1.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                cmd3.ExecuteNonQuery();

                sqlQuery1 = "insert into [dbo].[AccessControl] values('" + storage + "', '', '" + toShare + "'";
                cmd1 = new SqlCommand(sqlQuery1, con1);
                cmd1.ExecuteNonQuery();

                userField.Visibility = Visibility.Collapsed;
            }
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

            string sqlQuery1 = ("insert into [dbo].[UserFiles1] values('" + currentUserName + "', '" + textbox1.Text + "', @Null, '', '" + getCurrent() + "', 'no', 'no', '.doc',  '')");
            cmd1 = new SqlCommand(sqlQuery1, con1);
            cmd1.Parameters.Add("@Null", SqlDbType.VarBinary, -1);
            cmd1.Parameters["@Null"].Value = DBNull.Value;
            string sqlQuery2 = ("insert into [dbo].[UserFiles2] values('" + currentUserName + "', '" + textbox1.Text + "', @Null, '', '" + getCurrent() + "', 'no', 'no', '.doc',  '')");
            cmd2 = new SqlCommand(sqlQuery2, con2);
            cmd2.Parameters.Add("@Null", SqlDbType.VarBinary, -1);
            cmd2.Parameters["@Null"].Value = DBNull.Value;
            string sqlQuery3 = ("insert into [dbo].[UserFiles3] values('" + currentUserName + "', '" + textbox1.Text + "', @Null, '', '" + getCurrent() + "', 'no', 'no', '.doc',  '')");
            cmd3 = new SqlCommand(sqlQuery3, con3);
            cmd3.Parameters.Add("@Null", SqlDbType.VarBinary, -1);
            cmd3.Parameters["@Null"].Value = DBNull.Value;

            cmd1.ExecuteNonQuery();
            cmd2.ExecuteNonQuery();
            cmd3.ExecuteNonQuery();

            if (inWhere.Equals("My Folders") || inWhere.Equals("Recent") || inWhere.Equals("Shared") || inWhere.Equals("Favorites") || inWhere.Equals("Bin"))
            {
                sqlQuery1 = "insert into [dbo].[AccessControl] values('" + textbox1.Text + "', '', '" + currentUserName + "')";
                cmd1 = new SqlCommand(sqlQuery1, con1);

                cmd1.ExecuteNonQuery();
            }

            else if (!inWhere.Equals("My Folders") || !inWhere.Equals("Recent") || !inWhere.Equals("Shared") || !inWhere.Equals("Favorites") || !inWhere.Equals("Bin"))
            {
                sqlQuery1 = "insert into [dbo].[AccessControl] values('" + textbox1.Text + "', '" + inWhere.Text + "', '" + currentUserName + "')";
                cmd1 = new SqlCommand(sqlQuery1, con1);

                cmd1.ExecuteNonQuery();
            }
            

            textbox1.Text = String.Empty;

            closeAllConnections();

            listView.Visibility = System.Windows.Visibility.Collapsed;
            mainToolBar.Visibility = System.Windows.Visibility.Collapsed;
            rtbEditor.Visibility = System.Windows.Visibility.Visible;
            secondToolBar.Visibility = System.Windows.Visibility.Visible;
        }


        private void OkButton3_Click(object sender, RoutedEventArgs e)
        {
            string toCreate = textbox3.Text;

            openAllConnections();

            string sqlQuery1 = ("insert into [dbo].[UserFiles1] values('" + currentUserName + "', '" + textbox1.Text + "', @Null, '', '" + getCurrent() + "', 'no', 'no', '.fol',  '')");
            cmd1 = new SqlCommand(sqlQuery1, con1);
            cmd1.Parameters.Add("@Null", SqlDbType.VarBinary, -1);
            cmd1.Parameters["@Null"].Value = DBNull.Value;
            string sqlQuery2 = ("insert into [dbo].[UserFiles2] values('" + currentUserName + "', '" + textbox1.Text + "', @Null, '', '" + getCurrent() + "', 'no', 'no', '.fol',  '')");
            cmd2 = new SqlCommand(sqlQuery2, con2);
            cmd2.Parameters.Add("@Null", SqlDbType.VarBinary, -1);
            cmd2.Parameters["@Null"].Value = DBNull.Value;
            string sqlQuery3 = ("insert into [dbo].[UserFiles3] values('" + currentUserName + "', '" + textbox1.Text + "', @Null, '', '" + getCurrent() + "', 'no', 'no', '.fol',  '')");
            cmd3 = new SqlCommand(sqlQuery3, con3);
            cmd3.Parameters.Add("@Null", SqlDbType.VarBinary, -1);
            cmd3.Parameters["@Null"].Value = DBNull.Value;

            cmd1.ExecuteNonQuery();
            cmd2.ExecuteNonQuery();
            cmd3.ExecuteNonQuery();

            closeAllConnections();
        }


        //CANCEL 
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NewFile.Visibility = Visibility.Collapsed;
            textbox1.Text = String.Empty;
        }


        private void CancelButton2_Click(object sender, RoutedEventArgs e)
        {
            userField.Visibility = Visibility.Collapsed;
            textbox2.Text = String.Empty;
        }


        private void CancelButton3_Click(object sender, RoutedEventArgs e)
        {
            NewFolder.Visibility = Visibility.Collapsed;
            textbox3.Text = String.Empty;
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


        public void createFolder(object sender, RoutedEventArgs e)
        {
            NewFolder.Visibility = Visibility.Visible;
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

                    ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
                    string connectionString = conSettings.ConnectionString;

                    string VirusName;
                    string type;
                    SqlCommand cmd5;

                    String imgLocation = "";
                   


                    int virus = 0;

                    byte[] checkMD5;

                  
                      

                        Console.Write(fullfilename);

                        byte[] md5HashBytes = ComputeMd5Hash(fullfilename);

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

                                    MessageBox.Show(" \n VIRUS DECTED! Your File is not being Uploaded");
                                MessageBox.Show(" \n Virus Dected : " + VirusName + " | Type Of Virus : " + type);
                                }
                                else
                                {
                                MessageBox.Show("\n There is no virus! Very Good!! ");
                                MessageBox.Show("\n File is now being uploaded......");
                                }

                            }
                            else
                            {
                            MessageBox.Show("\n There is no virus! Very Good!! ");
                            MessageBox.Show("\n File is now being uploaded......");
                            }
                            con.Close();
                        }



                    if (virus == 0)
                    {
                        byte[] file;
                        using (var stream = new FileStream(fullfilename, FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = new BinaryReader(stream))
                            {
                                file = reader.ReadBytes((int)stream.Length);
                            }
                        }

                        //Sean's Encryption Codes
                        con.Open();
                        SqlCommand cmd;
                        string sqlQuery = "SELECT keyPath FROM dbo.test WHERE UserID='" + currentUserName + "'";
                        cmd = new SqlCommand(sqlQuery, con);
                        SqlDataReader DataRead1 = cmd.ExecuteReader();
                        string bigPath = DataRead1.GetString(0);

                        byte[] IV = System.IO.File.ReadAllBytes(@bigPath + "+\\IV.txt");
                        Console.WriteLine("Gets bytes of IV");
                        byte[] encryptedSymmetricKey = System.IO.File.ReadAllBytes(@bigPath + "\\encryptedSymmetricKey.txt");

                        //Gets the symmetric key by decrypting the encrypted symmetric key with the decryption (private) key
                        byte[] decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                        //Encrypts plaintext with symmetric key
                        byte[] cipherText = kc.symmetricEncryption(file, decryptedSymmetricKey, IV);

                        FileModel fm = new FileModel(currentUserName, filename, cipherText, getFileSize(cipherText.Length), getCurrent(), "no", "no", extension, "");
                        fm.setShow(true);
                        FileModel.setFileModel(fm);

                        if (inWhere.Equals("My Folders") || inWhere.Equals("Recent") || inWhere.Equals("Shared") || inWhere.Equals("Favorites") || inWhere.Equals("Bin"))
                        {
                            String sqlQuery1 = "insert into [dbo].[AccessControl] values('" + filename + "', '', '" + currentUserName + "')";
                            cmd1 = new SqlCommand(sqlQuery1, con1);

                            cmd1.ExecuteNonQuery();
                        }

                        else if (!inWhere.Equals("My Folders") || !inWhere.Equals("Recent") || !inWhere.Equals("Shared") || !inWhere.Equals("Favorites") || !inWhere.Equals("Bin"))
                        {
                            String sqlQuery1 = "insert into [dbo].[AccessControl] values('" + filename + "', '" + inWhere.Text + "', '" + currentUserName + "')";
                            cmd1 = new SqlCommand(sqlQuery1, con1);

                            cmd1.ExecuteNonQuery();
                        }

                        //FileModel fm = FileModel.getFileModel();
                        NavigationService.Navigate(new Uri("UploadingConsole.xaml"), UriKind.RelativeOrAbsolute);
                    }

                    /*int len = file.Length / 3;
                    byte[] toSend1 = file.Take(len).ToArray();
                    byte[] toSend2 = file.Skip(len).Take(len).ToArray();
                    int len2 = len + len;
                    byte[] toSend3 = file.Skip(len2).Take(len).ToArray();*/
                    
                    
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

                    byte[] byteArray = Encoding.ASCII.GetBytes(rtfText);
                    double fileSize = byteArray.Length;

                    con.Open();
                    //Sean's encryption codes
                    SqlCommand cmd;
                    string sqlQuery = "SELECT keyPath FROM dbo.test WHERE UserID='" + currentUserName + "'";
                    cmd = new SqlCommand(sqlQuery, con);
                    SqlDataReader DataRead1 = cmd.ExecuteReader();
                    string bigPath = DataRead1.GetString(0);

                    byte[] IV = System.IO.File.ReadAllBytes(@bigPath + "+\\IV.txt");
                    Console.WriteLine("Gets bytes of IV");
                    byte[] encryptedSymmetricKey = System.IO.File.ReadAllBytes(@bigPath + "\\encryptedSymmetricKey.txt");

                    //Gets the symmetric key by decrypting the encrypted symmetric key with the decryption (private) key
                    byte[] decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                    //Encrypts plaintext with symmetric key
                    byte[] cipherText = kc.symmetricEncryption(byteArray, decryptedSymmetricKey, IV);

                    //send encrypted file to model
                    //do navigation here

                    int len = byteArray.Length / 3;
                    byte[] toSend1 = byteArray.Take(len).ToArray();
                    byte[] toSend2 = byteArray.Skip(len).Take(len).ToArray();
                    int len2 = len + len;
                    byte[] toSend3 = byteArray.Skip(len2).Take(len).ToArray();

                    openAllConnections();

                    string sqlQuery1 = ("insert into [dbo].[UserFiles1] values('" + currentUserName + "', '" + filename + "', @toSend1, '" + getFileSize(byteArray.Length) + "', '" + getCurrent() + "', 'no', 'no', '" + extension + "', '')");
                    cmd1 = new SqlCommand(sqlQuery1, con1);
                    SqlParameter para1 = new SqlParameter("@toSend1", toSend1);
                    cmd1.Parameters.Add(para1);
                    string sqlQuery2 = ("insert into [dbo].[UserFiles2] values('" + currentUserName + "', '" + filename + "', @toSend2, '" + getFileSize(byteArray.Length) + "', '" + getCurrent() + "', 'no', 'no', '" + extension + "', '')");
                    cmd2 = new SqlCommand(sqlQuery2, con2);
                    SqlParameter para2 = new SqlParameter("@toSend2", toSend2);
                    cmd2.Parameters.Add(para2);
                    string sqlQuery3 = ("insert into [dbo].[UserFiles3] values('" + currentUserName + "', '" + filename + "', @toSend3, '" + getFileSize(byteArray.Length) + "', '" + getCurrent() + "', 'no', 'no', '" + extension + "', '')");
                    cmd3 = new SqlCommand(sqlQuery3, con3);
                    SqlParameter para3 = new SqlParameter("@toSend3", toSend3);
                    cmd3.Parameters.Add(para3);

                    cmd1.ExecuteNonQuery();
                    cmd2.ExecuteNonQuery();
                    cmd3.ExecuteNonQuery();

                    if (inWhere.Equals("My Folders") || inWhere.Equals("Recent") || inWhere.Equals("Shared") || inWhere.Equals("Favorites") || inWhere.Equals("Bin"))
                    {
                        sqlQuery1 = "insert into [dbo].[AccessControl] values('" + filename + "', '', '" + currentUserName + "')";
                        cmd1 = new SqlCommand(sqlQuery1, con1);

                        cmd1.ExecuteNonQuery();
                    }

                    else if (!inWhere.Equals("My Folders") || !inWhere.Equals("Recent") || !inWhere.Equals("Shared") || !inWhere.Equals("Favorites") || !inWhere.Equals("Bin"))
                    {
                        sqlQuery1 = "insert into [dbo].[AccessControl] values('" + filename + "', '" + inWhere.Text + "', '" + currentUserName + "')";
                        cmd1 = new SqlCommand(sqlQuery1, con1);

                        cmd1.ExecuteNonQuery();
                    }

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

                    con.Open();
                    //Sean's encryption codes
                    SqlCommand cmd;
                    string sqlQuery = "SELECT keyPath FROM dbo.test WHERE UserID='" + currentUserName + "'";
                    cmd = new SqlCommand(sqlQuery, con);
                    SqlDataReader DataRead1 = cmd.ExecuteReader();
                    string bigPath = DataRead1.GetString(0);

                    byte[] IV = System.IO.File.ReadAllBytes(@bigPath + "+\\IV.txt");
                    Console.WriteLine("Gets bytes of IV");
                    byte[] encryptedSymmetricKey = System.IO.File.ReadAllBytes(@bigPath + "\\encryptedSymmetricKey.txt");

                    //Gets the symmetric key by decrypting the encrypted symmetric key with the decryption (private) key
                    byte[] decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                    //Encrypts plaintext with symmetric key
                    byte[] cipherText = kc.symmetricEncryption(file, decryptedSymmetricKey, IV);
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

                    string sqlQuery1 = ("insert into [dbo].[UserFiles1] values('" + currentUserName + "', '" + filename + "', @toSend1, '" + getFileSize(file.Length) + "', '" + getCurrent() + "', 'no', 'no', '" + extension + "', '')");
                    cmd1 = new SqlCommand(sqlQuery1, con1);
                    SqlParameter para1 = new SqlParameter("@toSend1", toSend1);
                    cmd1.Parameters.Add(para1);
                    string sqlQuery2 = ("insert into [dbo].[UserFiles2] values('" + currentUserName + "', '" + filename + "', @toSend2, '" + getFileSize(file.Length) + "', '" + getCurrent() + "', 'no', 'no', '" + extension + "', '')");
                    cmd2 = new SqlCommand(sqlQuery2, con2);
                    SqlParameter para2 = new SqlParameter("@toSend2", toSend2);
                    cmd2.Parameters.Add(para2);
                    string sqlQuery3 = ("insert into [dbo].[UserFiles3] values('" + currentUserName + "', '" + filename + "', @toSend3, '" + getFileSize(file.Length) + "', '" + getCurrent() + "', 'no', 'no', '" + extension + "', '')");
                    cmd3 = new SqlCommand(sqlQuery3, con3);
                    SqlParameter para3 = new SqlParameter("@toSend3", toSend3);
                    cmd3.Parameters.Add(para3);

                    cmd1.ExecuteNonQuery();
                    cmd2.ExecuteNonQuery();
                    cmd3.ExecuteNonQuery();

                    if (inWhere.Equals("My Folders") || inWhere.Equals("Recent") || inWhere.Equals("Shared") || inWhere.Equals("Favorites") || inWhere.Equals("Bin"))
                    {
                        sqlQuery1 = "insert into [dbo].[AccessControl] values('" + filename + "', '', '" + currentUserName + "')";
                        cmd1 = new SqlCommand(sqlQuery1, con1);

                        cmd1.ExecuteNonQuery();
                    }

                    else if (!inWhere.Equals("My Folders") || !inWhere.Equals("Recent") || !inWhere.Equals("Shared") || !inWhere.Equals("Favorites") || !inWhere.Equals("Bin"))
                    {
                        sqlQuery1 = "insert into [dbo].[AccessControl] values('" + filename + "', '" + inWhere.Text + "', '" + currentUserName + "')";
                        cmd1 = new SqlCommand(sqlQuery1, con1);

                        cmd1.ExecuteNonQuery();
                    }

                    process.Exited += (sender2, eventArgs) =>
                    {
                        using (var stream = new FileStream("temp.xlsx", FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = new BinaryReader(stream))
                            {
                                file = reader.ReadBytes((int)stream.Length);
                            }
                        }

                        //Sean's Decryption Codes

                        sqlQuery = "SELECT keyPath FROM dbo.test WHERE UserID='" + currentUserName + "'";
                        cmd = new SqlCommand(sqlQuery, con);
                        DataRead1 = cmd.ExecuteReader();
                        bigPath = DataRead1.GetString(0);

                        //Gets IV & Encrypted Symmetric Key
                        IV = System.IO.File.ReadAllBytes(@bigPath + "\\IV.txt");
                        encryptedSymmetricKey = File.ReadAllBytes(@bigPath + "\\encryptedSymmetricKey.txt");
                        decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                        byte[] plainText = kc.symmetricDecryption(file, decryptedSymmetricKey, IV);

                        len = file.Length / 3;
                        toSend1 = file.Take(len).ToArray();
                        toSend2 = file.Skip(len).Take(len).ToArray();
                        len2 = len + len;
                        toSend3 = file.Skip(len2).Take(len).ToArray();

                        sqlQuery1 = "update [dbo].[UserFiles1] set [File] = @toSend1, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "'";
                        cmd1 = new SqlCommand(sqlQuery1, con1);
                        para1 = new SqlParameter("@toSend1", toSend1);
                        cmd1.Parameters.Add(para1);
                        sqlQuery2 = "update [dbo].[UserFiles2] set [File] = @toSend2, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "'";
                        cmd2 = new SqlCommand(sqlQuery2, con2);
                        para2 = new SqlParameter("@toSend2", toSend2);
                        cmd2.Parameters.Add(para2);
                        sqlQuery3 = "update [dbo].[UserFiles3] set [File] = @toSend3, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "'";
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
            inWhere.Text = currentPage;

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
            inWhere.Text = currentPage;
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
            inWhere.Text = currentPage;
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
            inWhere.Text = currentPage;
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
            inWhere.Text = currentPage;
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

        private void changeSettings(object sender, RoutedEventArgs e)
        {

        }
        
             private void moveTo(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {

        }
    }
}

