using Layout.Controllers;
using Layout.Models;
using Layout.Upload;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
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

        string stegPass;
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

            if (fm == null)
            {

            }
            else {

                if (fm.getShow() == true)
                {


                    if (fm.ReturnFileType() == ".doc" || fm.ReturnFileType() == ".docx")
                    {
                        byte[] file = fm.ReturnFileBytes();
                        // Sean's Decryption Codes
                        String filename = fm.ReturnfileName();
                        sqlQuery = "SELECT keyPath FROM dbo.test WHERE UserID='" + currentUserName + "'";
                        SqlCommand cmd = new SqlCommand(sqlQuery, con);
                        SqlDataReader DataRead1 = cmd.ExecuteReader();
                        string bigPath = null;
                        while (DataRead1.Read())
                        {
                            bigPath = DataRead1.GetString(0);
                        }

                        //Gets IV & Encrypted Symmetric Key
                        byte[] IV = System.IO.File.ReadAllBytes(@bigPath + "\\IV.txt");
                        byte[] encryptedSymmetricKey = File.ReadAllBytes(@bigPath + "\\encryptedSymmetricKey.txt");
                        byte[] decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                        byte[] plainText = kc.symmetricDecryption(file, decryptedSymmetricKey, IV);

                        File.WriteAllBytes("temp.doc", plainText);

                        Process process = new Process();
                        process.StartInfo.FileName = "temp.doc";
                        process.StartInfo.UseShellExecute = true;
                        process.Start();
                        process.EnableRaisingEvents = true;

                        process.Exited += (sender2, eventArgs) =>
                        {
                            using (var stream = new FileStream("temp.doc", FileMode.Open, FileAccess.Read))
                            {
                                using (var reader = new BinaryReader(stream))
                                {
                                    file = reader.ReadBytes((int)stream.Length);
                                }
                            }

                            closeAllConnections();
                            openAllConnections();
                            //Sean's encryption codes
                            sqlQuery = "SELECT keyPath FROM dbo.test WHERE UserID='" + currentUserName + "'";
                            cmd = new SqlCommand(sqlQuery, con);
                            DataRead1 = cmd.ExecuteReader();
                            bigPath = null;
                            while (DataRead1.Read())
                            {
                                bigPath = DataRead1.GetString(0);
                            }

                            IV = System.IO.File.ReadAllBytes(@bigPath + "\\IV.txt");
                            Console.WriteLine("Gets bytes of IV");
                            encryptedSymmetricKey = System.IO.File.ReadAllBytes(@bigPath + "\\encryptedSymmetricKey.txt");

                            //Gets the symmetric key by decrypting the encrypted symmetric key with the decryption (private) key
                            decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                            //Encrypts plaintext with symmetric key
                            byte[] cipherText = kc.symmetricEncryption(file, decryptedSymmetricKey, IV);

                            byte[] toSend1 = null;
                            byte[] toSend2 = null;
                            byte[] toSend3 = null;

                            if (cipherText.Length % 3 == 0)
                            {
                                int len = cipherText.Length / 3;
                                toSend1 = cipherText.Take(len).ToArray();
                                toSend2 = cipherText.Skip(len).Take(len).ToArray();
                                int len2 = len + len;
                                toSend3 = cipherText.Skip(len2).Take(len).ToArray();
                            }

                            else if (cipherText.Length % 3 == 1)
                            {
                                int len = (cipherText.Length / 3) + 1;
                                int len2 = cipherText.Length - len;
                                int len3 = len2 / 2;
                                toSend1 = cipherText.Take(len3).ToArray();
                                toSend2 = cipherText.Skip(len3).Take(len3).ToArray();
                                toSend3 = cipherText.Skip(len2).Take(len).ToArray();
                            }

                            else if (cipherText.Length % 3 == 2)
                            {
                                int len = (cipherText.Length / 3) + 2;
                                int len2 = cipherText.Length - len;
                                int len3 = len2 / 2;
                                toSend1 = cipherText.Take(len3).ToArray();
                                toSend2 = cipherText.Skip(len3).Take(len3).ToArray();
                                toSend3 = cipherText.Skip(len2).Take(len).ToArray();
                            }

                            String sqlQuery1 = ("update [dbo].[UserFiles1] set [File] = @toSend1, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "' and Name = '" + filename + "'");
                            cmd1 = new SqlCommand(sqlQuery1, con1);
                            SqlParameter para1 = new SqlParameter("@toSend1", toSend1);
                            cmd1.Parameters.Add(para1);
                            String sqlQuery2 = ("update [dbo].[UserFiles3] set [File] = @toSend2, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "' and Name = '" + filename + "'");
                            cmd2 = new SqlCommand(sqlQuery2, con2);
                            SqlParameter para2 = new SqlParameter("@toSend2", toSend2);
                            cmd2.Parameters.Add(para2);
                            String sqlQuery3 = ("update [dbo].[UserFiles2] set [File] = @toSend3, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "' and Name = '" + filename + "'");
                            cmd3 = new SqlCommand(sqlQuery3, con3);
                            SqlParameter para3 = new SqlParameter("@toSend3", toSend3);
                            cmd3.Parameters.Add(para3);

                            Dispatcher.Invoke(() =>
                            {
                                loading.Visibility = Visibility.Visible;
                            });

                            cmd1.ExecuteNonQuery();
                            cmd2.ExecuteNonQuery();
                            cmd3.ExecuteNonQuery();

                            File.Delete("temp.doc");

                            Dispatcher.Invoke(() =>
                            {
                                loading.Visibility = Visibility.Collapsed;
                            });
                            MessageBox.Show("Saved.");
                        };
                    }

                    else if (fm.ReturnFileType() == ".ppt" || fm.ReturnFileType() == ".pptx")
                    {
                        byte[] file = fm.ReturnFileBytes();
                        // Sean's Decryption Codes
                        String filename = fm.ReturnfileName();
                        sqlQuery = "SELECT keyPath FROM dbo.test WHERE UserID='" + currentUserName + "'";
                        SqlCommand cmd = new SqlCommand(sqlQuery, con);
                        SqlDataReader DataRead1 = cmd.ExecuteReader();
                        string bigPath = null;
                        while (DataRead1.Read())
                        { 
                             bigPath = DataRead1.GetString(0);
                        }
                        //Gets IV & Encrypted Symmetric Key
                        byte[] IV = System.IO.File.ReadAllBytes(@bigPath + "\\IV.txt");
                        byte[] encryptedSymmetricKey = File.ReadAllBytes(@bigPath + "\\encryptedSymmetricKey.txt");
                        byte[] decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                        byte[] plainText = kc.symmetricDecryption(file, decryptedSymmetricKey, IV);

                        File.WriteAllBytes("temp.ppt", plainText);

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

                            closeAllConnections();
                            openAllConnections();

                            //Sean's encryption codes
                            sqlQuery = "SELECT keyPath FROM dbo.test WHERE UserID='" + currentUserName + "'";
                            cmd = new SqlCommand(sqlQuery, con);
                            DataRead1 = cmd.ExecuteReader();
                            bigPath = null;
                            while (DataRead1.Read())
                            {
                                bigPath = DataRead1.GetString(0);
                            }

                            IV = System.IO.File.ReadAllBytes(@bigPath + "\\IV.txt");
                            Console.WriteLine("Gets bytes of IV");
                            encryptedSymmetricKey = System.IO.File.ReadAllBytes(@bigPath + "\\encryptedSymmetricKey.txt");

                            //Gets the symmetric key by decrypting the encrypted symmetric key with the decryption (private) key
                            decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                            //Encrypts plaintext with symmetric key
                            byte[] cipherText = kc.symmetricEncryption(file, decryptedSymmetricKey, IV);

                            byte[] toSend1 = null;
                            byte[] toSend2 = null;
                            byte[] toSend3 = null;

                            if (cipherText.Length % 3 == 0)
                            {
                                int len = cipherText.Length / 3;
                                toSend1 = cipherText.Take(len).ToArray();
                                toSend2 = cipherText.Skip(len).Take(len).ToArray();
                                int len2 = len + len;
                                toSend3 = cipherText.Skip(len2).Take(len).ToArray();
                            }

                            else if (cipherText.Length % 3 == 1)
                            {
                                int len = (cipherText.Length / 3) + 1;
                                int len2 = cipherText.Length - len;
                                int len3 = len2 / 2;
                                toSend1 = cipherText.Take(len3).ToArray();
                                toSend2 = cipherText.Skip(len3).Take(len3).ToArray();
                                toSend3 = cipherText.Skip(len2).Take(len).ToArray();
                            }

                            else if (cipherText.Length % 3 == 2)
                            {
                                int len = (cipherText.Length / 3) + 2;
                                int len2 = cipherText.Length - len;
                                int len3 = len2 / 2;
                                toSend1 = cipherText.Take(len3).ToArray();
                                toSend2 = cipherText.Skip(len3).Take(len3).ToArray();
                                toSend3 = cipherText.Skip(len2).Take(len).ToArray();
                            }

                            String sqlQuery1 = ("update [dbo].[UserFiles1] set [File] = @toSend1, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "' and Name = '" + filename + "'");
                            cmd1 = new SqlCommand(sqlQuery1, con1);
                            SqlParameter para1 = new SqlParameter("@toSend1", toSend1);
                            cmd1.Parameters.Add(para1);
                            String sqlQuery2 = ("update [dbo].[UserFiles3] set [File] = @toSend2, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "' and Name = '" + filename + "'");
                            cmd2 = new SqlCommand(sqlQuery2, con2);
                            SqlParameter para2 = new SqlParameter("@toSend2", toSend2);
                            cmd2.Parameters.Add(para2);
                            String sqlQuery3 = ("update [dbo].[UserFiles2] set [File] = @toSend3, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "' and Name = '" + filename + "'");
                            cmd3 = new SqlCommand(sqlQuery3, con3);
                            SqlParameter para3 = new SqlParameter("@toSend3", toSend3);
                            cmd3.Parameters.Add(para3);

                            Dispatcher.Invoke(() =>
                            {
                                loading.Visibility = Visibility.Visible;
                            });

                            cmd1.ExecuteNonQuery();
                            cmd2.ExecuteNonQuery();
                            cmd3.ExecuteNonQuery();

                            File.Delete("temp.ppt");

                            Dispatcher.Invoke(() =>
                            {
                                loading.Visibility = Visibility.Collapsed;
                            });
                            MessageBox.Show("Saved.");
                        };
                    }

                    else if (fm.ReturnFileType() == ".xlsx")
                    {
                        byte[] file = fm.ReturnFileBytes();
                        // Sean's Decryption Codes
                        String filename = fm.ReturnfileName();
                        sqlQuery = "SELECT keyPath FROM dbo.test WHERE UserID='" + currentUserName + "'";
                        SqlCommand cmd = new SqlCommand(sqlQuery, con);
                        SqlDataReader DataRead1 = cmd.ExecuteReader();
                        string bigPath = null;
                        while (DataRead1.Read())
                        {
                            bigPath = DataRead1.GetString(0);
                        }

                        //Gets IV & Encrypted Symmetric Key
                        byte[] IV = System.IO.File.ReadAllBytes(@bigPath + "\\IV.txt");
                        byte[] encryptedSymmetricKey = File.ReadAllBytes(@bigPath + "\\encryptedSymmetricKey.txt");
                        byte[] decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                        byte[] plainText = kc.symmetricDecryption(file, decryptedSymmetricKey, IV);

                        File.WriteAllBytes("temp.xlsx", plainText);

                        Process process = new Process();
                        process.StartInfo.FileName = "temp.xlsx";
                        process.StartInfo.UseShellExecute = true;
                        process.Start();
                        process.EnableRaisingEvents = true;

                        process.Exited += (sender2, eventArgs) =>
                        {
                            using (var stream = new FileStream("temp.xlsx", FileMode.Open, FileAccess.Read))
                            {
                                using (var reader = new BinaryReader(stream))
                                {
                                    file = reader.ReadBytes((int)stream.Length);
                                }
                            }

                            closeAllConnections();
                            openAllConnections();


                            //Sean's encryption codes
                            sqlQuery = "SELECT keyPath FROM dbo.test WHERE UserID='" + currentUserName + "'";
                            cmd = new SqlCommand(sqlQuery, con);
                            DataRead1 = cmd.ExecuteReader();
                            bigPath = null;
                            while (DataRead1.Read())
                            {
                                bigPath = DataRead1.GetString(0);
                            }

                            IV = System.IO.File.ReadAllBytes(@bigPath + "\\IV.txt");
                            Console.WriteLine("Gets bytes of IV");
                            encryptedSymmetricKey = System.IO.File.ReadAllBytes(@bigPath + "\\encryptedSymmetricKey.txt");

                            //Gets the symmetric key by decrypting the encrypted symmetric key with the decryption (private) key
                            decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                            //Encrypts plaintext with symmetric key
                            byte[] cipherText = kc.symmetricEncryption(file, decryptedSymmetricKey, IV);

                            byte[] toSend1 = null;
                            byte[] toSend2 = null;
                            byte[] toSend3 = null;

                            if (cipherText.Length % 3 == 0)
                            {
                                int len = cipherText.Length / 3;
                                toSend1 = cipherText.Take(len).ToArray();
                                toSend2 = cipherText.Skip(len).Take(len).ToArray();
                                int len2 = len + len;
                                toSend3 = cipherText.Skip(len2).Take(len).ToArray();
                            }

                            else if (cipherText.Length % 3 == 1)
                            {
                                int len = (cipherText.Length / 3) + 1;
                                int len2 = cipherText.Length - len;
                                int len3 = len2 / 2;
                                toSend1 = cipherText.Take(len3).ToArray();
                                toSend2 = cipherText.Skip(len3).Take(len3).ToArray();
                                toSend3 = cipherText.Skip(len2).Take(len).ToArray();
                            }

                            else if (cipherText.Length % 3 == 2)
                            {
                                int len = (cipherText.Length / 3) + 2;
                                int len2 = cipherText.Length - len;
                                int len3 = len2 / 2;
                                toSend1 = cipherText.Take(len3).ToArray();
                                toSend2 = cipherText.Skip(len3).Take(len3).ToArray();
                                toSend3 = cipherText.Skip(len2).Take(len).ToArray();
                            }

                            String sqlQuery1 = ("update [dbo].[UserFiles1] set [File] = @toSend1, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "' and Name = '" + filename + "'");
                            cmd1 = new SqlCommand(sqlQuery1, con1);
                            SqlParameter para1 = new SqlParameter("@toSend1", toSend1);
                            cmd1.Parameters.Add(para1);
                            String sqlQuery2 = ("update [dbo].[UserFiles3] set [File] = @toSend2, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "' and Name = '" + filename + "'");
                            cmd2 = new SqlCommand(sqlQuery2, con2);
                            SqlParameter para2 = new SqlParameter("@toSend2", toSend2);
                            cmd2.Parameters.Add(para2);
                            String sqlQuery3 = ("update [dbo].[UserFiles2] set [File] = @toSend3, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "' and Name = '" + filename + "'");
                            cmd3 = new SqlCommand(sqlQuery3, con3);
                            SqlParameter para3 = new SqlParameter("@toSend3", toSend3);
                            cmd3.Parameters.Add(para3);

                            Dispatcher.Invoke(() =>
                            {
                                loading.Visibility = Visibility.Visible;
                            });

                            cmd1.ExecuteNonQuery();
                            cmd2.ExecuteNonQuery();
                            cmd3.ExecuteNonQuery();

                            File.Delete("temp.xlsx");

                            Dispatcher.Invoke(() =>
                            {
                                loading.Visibility = Visibility.Collapsed;
                            });
                            MessageBox.Show("Saved.");
                        };
                    }
                    

                }
            }
            closeAllConnections();

        }

        private void openAllConnections()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            if (con1.State == ConnectionState.Closed)
            {
                con1.Open();
            }
            if (con2.State == ConnectionState.Closed)
            {
                con2.Open();
            }
            if (con3.State == ConnectionState.Closed)
            {
                con3.Open();
            }

        }

        private void closeAllConnections()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (con1.State == ConnectionState.Open)
            {
                con1.Close();
            }
            if (con2.State == ConnectionState.Open)
            {
                con2.Close();
            }
            if (con3.State == ConnectionState.Open)
            {
                con3.Close();
            }
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

            string sqlQuery1 = "update [dbo].[UserFiles1] set isFavorite = 'yes' where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
            SqlCommand cmd1 = new SqlCommand(sqlQuery1, con1);
            string sqlQuery2 = "update [dbo].[UserFiles3] set isFavorite = 'yes' where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
            SqlCommand cmd2 = new SqlCommand(sqlQuery2, con2);
            string sqlQuery3 = "update [dbo].[UserFiles2] set isFavorite = 'yes' where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
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

            openAllConnections();

            string sqlQuery1 = "select fileType from [dbo].[UserFiles1] where Username = '" + currentUserName + "' and Name = '" + selectedText + "'";
            cmd1 = new SqlCommand(sqlQuery1, con1);

            SqlDataReader reader1 = cmd1.ExecuteReader();
            reader1.Read();
            string ext = (reader1[0].ToString());

            closeAllConnections();

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

            string sqlQuery1 = "update [dbo].[UserFiles1] set isFavorite = 'no' where Username = '" + currentUserName + "' and Name = '" + selectedText + "'";
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

            

            if (currentPage == "Bin")
            {
                openAllConnections();
                string sqlQuery1 = "delete from [dbo].[UserFiles1] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
                SqlCommand cmd1 = new SqlCommand(sqlQuery1, con1);
                string sqlQuery2 = "delete from [dbo].[UserFiles3] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
                SqlCommand cmd2 = new SqlCommand(sqlQuery2, con2);
                string sqlQuery3 = "delete from [dbo].[UserFiles2] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
                SqlCommand cmd3 = new SqlCommand(sqlQuery3, con3);

                cmd1.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                cmd3.ExecuteNonQuery();
                closeAllConnections();
                sortBin();
                openAllConnections();
                sqlQuery1 = "delete from [dbo].[AccessControl] where FileName = '" + selectedText + "' and AccessBy = '" + currentUserName + "'";
                cmd1 = new SqlCommand(sqlQuery1, con1);

                cmd1.ExecuteNonQuery();
                closeAllConnections();
            }

            else
            {
                openAllConnections();
                string sqlQuery1 = "update [dbo].[UserFiles1] set isDeleted = 'yes' where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
                SqlCommand cmd1 = new SqlCommand(sqlQuery1, con1);
                string sqlQuery2 = "update [dbo].[UserFiles3] set isDeleted = 'yes' where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
                SqlCommand cmd2 = new SqlCommand(sqlQuery2, con2);
                string sqlQuery3 = "update [dbo].[UserFiles2] set isDeleted = 'yes' where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
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
                closeAllConnections();
            }

            
        }


        //RIGHTCLICK -> RECOVER
        private void recoverClick(object sender, RoutedEventArgs e)
        {
            String selectedText = ((DataRowView)listView.SelectedItem)["Name"].ToString();

            openAllConnections();

            string sqlQuery1 = "update [dbo].[UserFiles1] set isDeleted = 'no' where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
            SqlCommand cmd1 = new SqlCommand(sqlQuery1, con1);
            string sqlQuery2 = "update [dbo].[UserFiles3] set isDeleted = 'no' where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
            SqlCommand cmd2 = new SqlCommand(sqlQuery2, con2);
            string sqlQuery3 = "update [dbo].[UserFiles2] set isDeleted = 'no' where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
            SqlCommand cmd3 = new SqlCommand(sqlQuery3, con3);

            closeAllConnections();

            sortBin();
        }

       

        private void openFile(String selectedText)
        {
            
            String ext = "";

            openAllConnections();

            String sqlQuery = "select fileType from [dbo].[UserFiles1] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
            SqlCommand cmd = new SqlCommand(sqlQuery, con1);

            reader = cmd.ExecuteReader();
            reader.Read();

            ext = (reader[0].ToString());

            closeAllConnections();

            //Gets name of username of SharedBy
            openAllConnections();
            string sqlQuery7 = "select sharedBy from [dbo].[UserFiles1] where Name = '" + selectedText + "'";
            SqlCommand cmd7 = new SqlCommand(sqlQuery7, con1);
            SqlDataReader reader7 = cmd7.ExecuteReader();
            reader7.Read();
            string owner = reader7[0].ToString();


            if (ext == ".fol")
            {
                openAllConnections();

                ArrayList list = new ArrayList();
                string sqlQuery1 = "select FileName from [dbo].[AccessControl] where Parent = '" + selectedText + "' and AccessBy = '" + currentUserName + "'";
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
                    sqlQuery = "select Name, sharedBy, lastModified, fileSize from [dbo].[UserFiles1] where Username = '" + currentUserName + "' and Name = '" + list[i] + "'";
                    cmd1 = new SqlCommand(sqlQuery, con1);
                    cmd1.ExecuteNonQuery();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(dt);
                }

                listView.ItemsSource = dt.DefaultView;

                inWhere.Text = selectedText;

                closeAllConnections();
            }

            if (ext == ".bmp")
            {
                openAllConnections();

                string sqlQuery1 = "select [File] from [dbo].[UserFiles1] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
                SqlCommand cmd1 = new SqlCommand(sqlQuery1, con1);
                string sqlQuery2 = "select [File] from [dbo].[UserFiles3] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
                SqlCommand cmd2 = new SqlCommand(sqlQuery2, con2);
                string sqlQuery3 = "select [File] from [dbo].[UserFiles2] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
                SqlCommand cmd3 = new SqlCommand(sqlQuery3, con3);

                SqlDataReader Reader1 = cmd1.ExecuteReader();
                SqlDataReader Reader2 = cmd2.ExecuteReader();
                SqlDataReader Reader3 = cmd3.ExecuteReader();

                //Cannot read data! Aku Justin Big Boi and Bryan Small Boi Tak Boleh Do this Functiono 
                //So we tiredo, so we helpo youdo tomorrowdo! Aso!

                Reader1.Read();
                Reader2.Read();
                Reader3.Read();

                byte[] retrieve1 = ((byte[])Reader1[0]);
                byte[] retrieve2 = ((byte[])Reader2[0]);
                byte[] retrieve3 = ((byte[])Reader3[0]);

                byte[] retrieve = retrieve1.Concat(retrieve2).Concat(retrieve3).ToArray();

                File.WriteAllBytes("temp.bmp", retrieve);
                Process.Start("temp.bmp");

                closeAllConnections();
            }

            if (ext == ".doc" || ext == ".docx")
            {
                openAllConnections();

                string sqlQuery1 = "select [File] from [dbo].[UserFiles1] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
                SqlCommand cmd1 = new SqlCommand(sqlQuery1, con1);
                string sqlQuery2 = "select [File] from [dbo].[UserFiles3] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
                SqlCommand cmd2 = new SqlCommand(sqlQuery2, con2);
                string sqlQuery3 = "select [File] from [dbo].[UserFiles2] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
                SqlCommand cmd3 = new SqlCommand(sqlQuery3, con3);

                SqlDataReader Reader1 = cmd1.ExecuteReader();
                SqlDataReader Reader2 = cmd2.ExecuteReader();
                SqlDataReader Reader3 = cmd3.ExecuteReader();

                Reader1.Read();
                Reader2.Read();
                Reader3.Read();

                byte[] retrieve1 = ((byte[])Reader1[0]);
                byte[] retrieve2 = ((byte[])Reader2[0]);
                byte[] retrieve3 = ((byte[])Reader3[0]);

                byte[] retrieve = retrieve1.Concat(retrieve2).Concat(retrieve3).ToArray();



                //Sean's Decryption Codes

                byte[] plainText;

                if (owner.Equals(currentUser))
                {
                    sqlQuery = "SELECT keyPath FROM dbo.test WHERE UserID='" + currentUserName + "'";
                    cmd = new SqlCommand(sqlQuery, con);
                    SqlDataReader DataRead1 = cmd.ExecuteReader();
                    string bigPath = null;
                    while (DataRead1.Read())
                    {
                        bigPath = DataRead1.GetString(0);
                    }

                    //Gets IV & Encrypted Symmetric Key
                    byte[] IV = System.IO.File.ReadAllBytes(@bigPath + "\\IV.txt");
                    byte[] encryptedSymmetricKey = File.ReadAllBytes(@bigPath + "\\encryptedSymmetricKey.txt");
                    byte[] decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                    plainText = kc.symmetricDecryption(retrieve, decryptedSymmetricKey, IV);
                }

                else
                {
                    System.Windows.MessageBox.Show("Select owner's key path");
                    System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                    string bigPath = fbd.SelectedPath + "\\" + owner;

                    //Gets IV & Encrypted Symmetric Key
                    byte[] IV = System.IO.File.ReadAllBytes(@bigPath + "\\IV.txt");
                    byte[] encryptedSymmetricKey = File.ReadAllBytes(@bigPath + "\\encryptedSymmetricKey.txt");
                    byte[] decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                    plainText = kc.symmetricDecryption(retrieve, decryptedSymmetricKey, IV);
                }
                File.WriteAllBytes("temp.doc", plainText);
                Process process = new Process();
                process.StartInfo.FileName = "temp.doc";
                process.StartInfo.UseShellExecute = true;
                process.Start();
                process.EnableRaisingEvents = true;
                process.Exited += (sender2, eventArgs) =>
                {
                    byte[] file;
                    using (var stream = new FileStream("temp.doc", FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = new BinaryReader(stream))
                        {
                            file = reader.ReadBytes((int)stream.Length);
                        }
                    }

                    openAllConnections();

                    //Sean's encryption codes
                    sqlQuery = "SELECT keyPath FROM dbo.test WHERE UserID='" + currentUserName + "'";
                    cmd = new SqlCommand(sqlQuery, con);
                    SqlDataReader DataRead1 = cmd.ExecuteReader();
                    string bigPath = "";
                    while (DataRead1.Read())
                    {
                        bigPath = DataRead1.GetString(0);
                    }

                    byte[] IV = System.IO.File.ReadAllBytes(@bigPath + "\\IV.txt");
                    Console.WriteLine("Gets bytes of IV");
                    byte[] encryptedSymmetricKey = System.IO.File.ReadAllBytes(@bigPath + "\\encryptedSymmetricKey.txt");

                    //Gets the symmetric key by decrypting the encrypted symmetric key with the decryption (private) key
                    byte[] decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                    //Encrypts plaintext with symmetric key
                    byte[] cipherText = kc.symmetricEncryption(file, decryptedSymmetricKey, IV);

                    byte[] toSend1 = null;
                    byte[] toSend2 = null;
                    byte[] toSend3 = null;

                    if (cipherText.Length % 3 == 0)
                    {
                        int len = cipherText.Length / 3;
                        toSend1 = cipherText.Take(len).ToArray();
                        toSend2 = cipherText.Skip(len).Take(len).ToArray();
                        int len2 = len + len;
                        toSend3 = cipherText.Skip(len2).Take(len).ToArray();
                    }

                    else if (cipherText.Length % 3 == 1)
                    {
                        int len = (cipherText.Length / 3) + 1;
                        int len2 = cipherText.Length - len;
                        int len3 = len2 / 2;
                        toSend1 = cipherText.Take(len3).ToArray();
                        toSend2 = cipherText.Skip(len3).Take(len3).ToArray();
                        toSend3 = cipherText.Skip(len2).Take(len).ToArray();
                    }

                    else if (cipherText.Length % 3 == 2)
                    {
                        int len = (cipherText.Length / 3) + 2;
                        int len2 = cipherText.Length - len;
                        int len3 = len2 / 2;
                        toSend1 = cipherText.Take(len3).ToArray();
                        toSend2 = cipherText.Skip(len3).Take(len3).ToArray();
                        toSend3 = cipherText.Skip(len2).Take(len).ToArray();
                    }

                    sqlQuery1 = "update [dbo].[UserFiles1] set [File] = @toSend1, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "' and Name = '" + selectedText + "'";
                    cmd1 = new SqlCommand(sqlQuery1, con1);
                    SqlParameter para1 = new SqlParameter("@toSend1", toSend1);
                    cmd1.Parameters.Add(para1);
                    sqlQuery2 = "update [dbo].[UserFiles3] set [File] = @toSend3, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "' and Name = '" + selectedText + "'";
                    cmd2 = new SqlCommand(sqlQuery2, con2);
                    SqlParameter para2 = new SqlParameter("@toSend3", toSend2);
                    cmd2.Parameters.Add(para2);
                    sqlQuery3 = "update [dbo].[UserFiles2] set [File] = @toSend2, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "' and Name = '" + selectedText + "'";
                    cmd3 = new SqlCommand(sqlQuery3, con3);
                    SqlParameter para3 = new SqlParameter("@toSend2", toSend3);
                    cmd3.Parameters.Add(para3);

                    Dispatcher.Invoke(() =>
                    {
                        loading.Visibility = Visibility.Visible;
                    });
                

                    cmd1.ExecuteNonQuery();
                    cmd2.ExecuteNonQuery();
                    cmd3.ExecuteNonQuery();

                    File.Delete("temp.doc");

                    Dispatcher.Invoke(() =>
                    {
                        loading.Visibility = Visibility.Collapsed;
                    });

                    MessageBox.Show("Saved.");
                    closeAllConnections();
                };
            
                closeAllConnections();
            }
    
            else if (ext == ".ppt" || ext == ".pptx")
            {
                openAllConnections();

                string sqlQuery1 = "select [File] from [dbo].[UserFiles1] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
                SqlCommand cmd1 = new SqlCommand(sqlQuery1, con1);
                string sqlQuery2 = "select [File] from [dbo].[UserFiles3] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
                SqlCommand cmd2 = new SqlCommand(sqlQuery2, con2);
                string sqlQuery3 = "select [File] from [dbo].[UserFiles2] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
                SqlCommand cmd3 = new SqlCommand(sqlQuery3, con3);

                SqlDataReader Reader1 = cmd1.ExecuteReader();
                SqlDataReader Reader2 = cmd2.ExecuteReader();
                SqlDataReader Reader3 = cmd3.ExecuteReader();

                //Cannot read data! Aku Justin Big Boi and Bryan Small Boi Tak Boleh Do this Functiono 
                //So we tiredo, so we helpo youdo tomorrowdo! Aso!

                Reader1.Read();
                Reader2.Read();
                Reader3.Read();

                byte[] retrieve1 = ((byte[])Reader1[0]);
                byte[] retrieve2 = ((byte[])Reader2[0]);
                byte[] retrieve3 = ((byte[])Reader3[0]);

                byte[] retrieve = retrieve1.Concat(retrieve2).Concat(retrieve3).ToArray();
                

                //Sean's Decryption Codes

                sqlQuery = "SELECT keyPath FROM dbo.test WHERE UserID='" + currentUserName + "'";
                cmd = new SqlCommand(sqlQuery, con);
                SqlDataReader DataRead1 = cmd.ExecuteReader();
                string bigPath = null;
                while (DataRead1.Read())
                {
                    bigPath = DataRead1.GetString(0);
                }
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

                    openAllConnections();

                    //Sean's encryption codes
                    sqlQuery = "SELECT keyPath FROM dbo.test WHERE UserID='" + currentUserName + "'";
                    cmd = new SqlCommand(sqlQuery, con);
                    DataRead1 = cmd.ExecuteReader();
                    while (DataRead1.Read())
                    {
                        bigPath = DataRead1.GetString(0);
                    }

                    IV = System.IO.File.ReadAllBytes(@bigPath + "\\IV.txt");
                    Console.WriteLine("Gets bytes of IV");
                    encryptedSymmetricKey = System.IO.File.ReadAllBytes(@bigPath + "\\encryptedSymmetricKey.txt");

                    //Gets the symmetric key by decrypting the encrypted symmetric key with the decryption (private) key
                    decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                    //Encrypts plaintext with symmetric key
                    byte[] cipherText = kc.symmetricEncryption(file, decryptedSymmetricKey, IV);

                    byte[] toSend1 = null;
                    byte[] toSend2 = null;
                    byte[] toSend3 = null;

                    if (cipherText.Length % 3 == 0)
                    {
                        int len = cipherText.Length / 3;
                        toSend1 = cipherText.Take(len).ToArray();
                        toSend2 = cipherText.Skip(len).Take(len).ToArray();
                        int len2 = len + len;
                        toSend3 = cipherText.Skip(len2).Take(len).ToArray();
                    }

                    else if (cipherText.Length % 3 == 1)
                    {
                        int len = (cipherText.Length / 3) + 1;
                        int len2 = cipherText.Length - len;
                        int len3 = len2 / 2;
                        toSend1 = cipherText.Take(len3).ToArray();
                        toSend2 = cipherText.Skip(len3).Take(len3).ToArray();
                        toSend3 = cipherText.Skip(len2).Take(len).ToArray();
                    }

                    else if (cipherText.Length % 3 == 2)
                    {
                        int len = (cipherText.Length / 3) + 2;
                        int len2 = cipherText.Length - len;
                        int len3 = len2 / 2;
                        toSend1 = cipherText.Take(len3).ToArray();
                        toSend2 = cipherText.Skip(len3).Take(len3).ToArray();
                        toSend3 = cipherText.Skip(len2).Take(len).ToArray();
                    }

                    sqlQuery1 = "update [dbo].[UserFiles1] set [File] = @toSend1, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "' and Name = '" + selectedText + "'";
                    cmd1 = new SqlCommand(sqlQuery1, con1);
                    SqlParameter para1 = new SqlParameter("@toSend1", toSend1);
                    cmd1.Parameters.Add(para1);
                    sqlQuery2 = "update [dbo].[UserFiles3] set [File] = @toSend3, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "' and Name = '" + selectedText + "'";
                    cmd2 = new SqlCommand(sqlQuery2, con2);
                    SqlParameter para2 = new SqlParameter("@toSend3", toSend2);
                    cmd2.Parameters.Add(para2);
                    sqlQuery3 = "update [dbo].[UserFiles2] set [File] = @toSend2, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "' and Name = '" + selectedText + "'";
                    cmd3 = new SqlCommand(sqlQuery3, con3);
                    SqlParameter para3 = new SqlParameter("@toSend2", toSend3);
                    cmd3.Parameters.Add(para3);

                    Dispatcher.Invoke(() =>
                    {
                        loading.Visibility = Visibility.Visible;
                    });

                    cmd1.ExecuteNonQuery();
                    cmd2.ExecuteNonQuery();
                    cmd3.ExecuteNonQuery();

                    closeAllConnections();

                    File.Delete("temp.ppt");

                    Dispatcher.Invoke(() =>
                    {
                        loading.Visibility = Visibility.Collapsed;
                    });
                    
                    MessageBox.Show("Saved.");
                };
                closeAllConnections();
            }   

            else if (ext == ".xlsx")
            {

                openAllConnections();

                string sqlQuery1 = "select [File] from [dbo].[UserFiles1] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
                SqlCommand cmd1 = new SqlCommand(sqlQuery1, con1);
                string sqlQuery2 = "select [File] from [dbo].[UserFiles3] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
                SqlCommand cmd2 = new SqlCommand(sqlQuery2, con2);
                string sqlQuery3 = "select [File] from [dbo].[UserFiles2] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
                SqlCommand cmd3 = new SqlCommand(sqlQuery3, con3);

                SqlDataReader Reader1 = cmd1.ExecuteReader();
                SqlDataReader Reader2 = cmd2.ExecuteReader();
                SqlDataReader Reader3 = cmd3.ExecuteReader();

                Reader1.Read();
                Reader2.Read();
                Reader3.Read();

                byte[] retrieve1 = ((byte[])Reader1[0]);
                byte[] retrieve2 = ((byte[])Reader2[0]);
                byte[] retrieve3 = ((byte[])Reader3[0]);

                byte[] retrieve = retrieve1.Concat(retrieve2).Concat(retrieve3).ToArray();

                //Sean's Decryption Codes

                sqlQuery = "SELECT keyPath FROM dbo.test WHERE UserID='" + currentUserName + "'";
                cmd = new SqlCommand(sqlQuery, con);
                SqlDataReader DataRead1 = cmd.ExecuteReader();
                string bigPath = null;
                while (DataRead1.Read())
                {
                    bigPath = DataRead1.GetString(0);
                }

                //Gets IV & Encrypted Symmetric Key
                byte[] IV = System.IO.File.ReadAllBytes(@bigPath + "\\IV.txt");
                byte[] encryptedSymmetricKey = File.ReadAllBytes(@bigPath + "\\encryptedSymmetricKey.txt");
                byte[] decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                byte[] plainText = kc.symmetricDecryption(retrieve, decryptedSymmetricKey, IV);

                File.WriteAllBytes("temp.xlsx", plainText);
                Process process = new Process();
                process.StartInfo.FileName = "temp.xlsx";
                process.StartInfo.UseShellExecute = true;
                process.Start();
                process.EnableRaisingEvents = true;
                process.Exited += (sender2, eventArgs) =>
                {
                    byte[] file;
                    using (var stream = new FileStream("temp.xlsx", FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = new BinaryReader(stream))
                        {
                            file = reader.ReadBytes((int)stream.Length);
                        }
                    }

                    openAllConnections();

                    //Sean's encryption codes
                    sqlQuery = "SELECT keyPath FROM dbo.test WHERE UserID='" + currentUserName + "'";
                    cmd = new SqlCommand(sqlQuery, con);
                    DataRead1 = cmd.ExecuteReader();
                    while (DataRead1.Read())
                    {
                        bigPath = DataRead1.GetString(0);
                    }

                    IV = System.IO.File.ReadAllBytes(@bigPath + "\\IV.txt");
                    Console.WriteLine("Gets bytes of IV");
                    encryptedSymmetricKey = System.IO.File.ReadAllBytes(@bigPath + "\\encryptedSymmetricKey.txt");

                    //Gets the symmetric key by decrypting the encrypted symmetric key with the decryption (private) key
                    decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                    //Encrypts plaintext with symmetric key
                    byte[] cipherText = kc.symmetricEncryption(file, decryptedSymmetricKey, IV);

                    byte[] toSend1 = null;
                    byte[] toSend2 = null;
                    byte[] toSend3 = null;

                    if (cipherText.Length % 3 == 0)
                    {
                        int len = cipherText.Length / 3;
                        toSend1 = cipherText.Take(len).ToArray();
                        toSend2 = cipherText.Skip(len).Take(len).ToArray();
                        int len2 = len + len;
                        toSend3 = cipherText.Skip(len2).Take(len).ToArray();
                    }

                    else if (cipherText.Length % 3 == 1)
                    {
                        int len = (cipherText.Length / 3) + 1;
                        int len2 = cipherText.Length - len;
                        int len3 = len2 / 2;
                        toSend1 = cipherText.Take(len3).ToArray();
                        toSend2 = cipherText.Skip(len3).Take(len3).ToArray();
                        toSend3 = cipherText.Skip(len2).Take(len).ToArray();
                    }

                    else if (cipherText.Length % 3 == 2)
                    {
                        int len = (cipherText.Length / 3) + 2;
                        int len2 = cipherText.Length - len;
                        int len3 = len2 / 2;
                        toSend1 = cipherText.Take(len3).ToArray();
                        toSend2 = cipherText.Skip(len3).Take(len3).ToArray();
                        toSend3 = cipherText.Skip(len2).Take(len).ToArray();
                    }

                    sqlQuery1 = "update [dbo].[UserFiles1] set [File] = @toSend1, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "' and Name = '" + selectedText + "'";
                    cmd1 = new SqlCommand(sqlQuery1, con1);
                    SqlParameter para1 = new SqlParameter("@toSend1", toSend1);
                    cmd1.Parameters.Add(para1);
                    sqlQuery2 = "update [dbo].[UserFiles3] set [File] = @toSend2, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "' and Name = '" + selectedText + "'";
                    cmd2 = new SqlCommand(sqlQuery2, con2);
                    SqlParameter para2 = new SqlParameter("@toSend2", toSend2);
                    cmd2.Parameters.Add(para2);
                    sqlQuery3 = "update [dbo].[UserFiles2] set [File] = @toSend3, fileSize = '" + getFileSize(file.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "' and Name = '" + selectedText + "'";
                    cmd3 = new SqlCommand(sqlQuery3, con3);
                    SqlParameter para3 = new SqlParameter("@toSend3", toSend3);
                    cmd3.Parameters.Add(para3);

                    Dispatcher.Invoke(() =>
                    {
                        loading.Visibility = Visibility.Visible;
                    });

                    cmd1.ExecuteNonQuery();
                    cmd2.ExecuteNonQuery();
                    cmd3.ExecuteNonQuery();

                    closeAllConnections();

                    File.Delete("temp.xlsx");

                    Dispatcher.Invoke(() =>
                    {
                        loading.Visibility = Visibility.Collapsed;
                    });
                    MessageBox.Show("Saved.");
                };
                closeAllConnections();
            }
            
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

                Reader1.Read();
                Reader2.Read();
                Reader3.Read();

                byte[] retrieve1 = ((byte[])Reader1[0]);
                byte[] retrieve2 = ((byte[])Reader2[0]);
                byte[] retrieve3 = ((byte[])Reader3[0]);

                byte[] retrieve = retrieve1.Concat(retrieve2).Concat(retrieve3).ToArray();

                closeAllConnections();
                openAllConnections();

                sqlQuery1 = ("insert into [dbo].[UserFiles1] values('" + toShare + "', '" + storage + "', @toSend1, '" + getFileSize(retrieve.Length) + "', '" + getCurrent() + "', 'no', 'no', '" + storage2 + "', '" + currentUserName + "')");
                cmd1 = new SqlCommand(sqlQuery1, con1);
                SqlParameter para1 = new SqlParameter("@toSend1", retrieve1);
                cmd1.Parameters.Add(para1);
                sqlQuery2 = ("insert into [dbo].[UserFiles3] values('" + toShare + "', '" + storage + "', @toSend2, '" + getFileSize(retrieve.Length) + "', '" + getCurrent() + "', 'no', 'no', '" + storage2 + "', '" + currentUserName + "')");
                cmd2 = new SqlCommand(sqlQuery2, con2);
                SqlParameter para2 = new SqlParameter("@toSend2", retrieve2);
                cmd2.Parameters.Add(para2);
                sqlQuery3 = ("insert into [dbo].[UserFiles2] values('" + toShare + "', '" + storage + "', @toSend3, '" + getFileSize(retrieve.Length) + "', '" + getCurrent() + "', 'no', 'no', '" + storage2 + "', '" + currentUserName + "')");
                cmd3 = new SqlCommand(sqlQuery3, con3);
                SqlParameter para3 = new SqlParameter("@toSend3", retrieve3);
                cmd3.Parameters.Add(para3);

                cmd1.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                cmd3.ExecuteNonQuery();

                closeAllConnections();
                openAllConnections();
                sqlQuery1 = "insert into [dbo].[AccessControl] values('" + storage + "', '', '" + toShare + "')";
                cmd1 = new SqlCommand(sqlQuery1, con1);
                cmd1.ExecuteNonQuery();
                closeAllConnections();
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

            

            openAllConnections();

            string sqlQuery1 = ("insert into [dbo].[UserFiles1] values('" + currentUserName + "', '" + textbox1.Text + "', @Null, '', '" + getCurrent() + "', 'no', 'no', '.cst',  '" + currentUserName + "')");
            cmd1 = new SqlCommand(sqlQuery1, con1);
            cmd1.Parameters.Add("@Null", SqlDbType.VarBinary, -1);
            cmd1.Parameters["@Null"].Value = DBNull.Value;
            string sqlQuery2 = ("insert into [dbo].[UserFiles3] values('" + currentUserName + "', '" + textbox1.Text + "', @Null, '', '" + getCurrent() + "', 'no', 'no', '.cst',  '" + currentUserName + "')");
            cmd2 = new SqlCommand(sqlQuery2, con2);
            cmd2.Parameters.Add("@Null", SqlDbType.VarBinary, -1);
            cmd2.Parameters["@Null"].Value = DBNull.Value;
            string sqlQuery3 = ("insert into [dbo].[UserFiles2] values('" + currentUserName + "', '" + textbox1.Text + "', @Null, '', '" + getCurrent() + "', 'no', 'no', '.cst',  '" + currentUserName + "')");
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

            fileName.Content = textbox1.Text;

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

            string sqlQuery1 = ("insert into [dbo].[UserFiles1] values('" + currentUserName + "', '" + textbox3.Text + "', @Null, '', '" + getCurrent() + "', 'no', 'no', '.fol',  '" + currentUserName + "')");
            cmd1 = new SqlCommand(sqlQuery1, con1);
            cmd1.Parameters.Add("@Null", SqlDbType.VarBinary, -1);
            cmd1.Parameters["@Null"].Value = DBNull.Value;
            string sqlQuery2 = ("insert into [dbo].[UserFiles3] values('" + currentUserName + "', '" + textbox3.Text + "', @Null, '', '" + getCurrent() + "', 'no', 'no', '.fol',  '" + currentUserName + "')");
            cmd2 = new SqlCommand(sqlQuery2, con2);
            cmd2.Parameters.Add("@Null", SqlDbType.VarBinary, -1);
            cmd2.Parameters["@Null"].Value = DBNull.Value;
            string sqlQuery3 = ("insert into [dbo].[UserFiles2] values('" + currentUserName + "', '" + textbox3.Text + "', @Null, '', '" + getCurrent() + "', 'no', 'no', '.fol',  '" + currentUserName + "')");
            cmd3 = new SqlCommand(sqlQuery3, con3);
            cmd3.Parameters.Add("@Null", SqlDbType.VarBinary, -1);
            cmd3.Parameters["@Null"].Value = DBNull.Value;

            cmd1.ExecuteNonQuery();
            cmd2.ExecuteNonQuery();
            cmd3.ExecuteNonQuery();

            closeAllConnections();
        }


        private void stego(object sender, RoutedEventArgs e)
        {
            string fileName = "";
            byte[] hideThis;
            Bitmap bmp = null;
            string fileExtension = "";

            while (bmp == null)
            {
                System.Windows.MessageBox.Show("Select an image carrier");

                OpenFileDialog dlg = new OpenFileDialog();
                if (dlg.ShowDialog() == true)
                {
                    fileName = dlg.FileName;
                    fileExtension = Path.GetExtension(fileName);
                    if (!fileExtension.Equals(".bmp"))
                    {
                        fileExtension = ".bmp";
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

                    System.Windows.MessageBox.Show("Select the txt file you wish to embed");


                    OpenFileDialog dlg1 = new OpenFileDialog();
                    if (dlg1.ShowDialog() == true)
                    {

                        hideThis = File.ReadAllBytes(dlg1.FileName);
                        stegPass = Layout.Controllers.Prompt.ShowDialog3("Enter password", "Prompt");
                        if (stegPass.Length < 8)
                        {
                            System.Windows.MessageBox.Show("Please enter an 8-character password");
                        }
                        else
                        {
                            if (stegPass.Length >= 8)
                            {
                                stegPass = stegPass.Substring(0, 8);
                            }
                            byte[] stegByte = Convert.FromBase64String(stegPass);

                            while (stegByte.Length != 32)
                            {
                                stegByte = stegByte.Concat(Encoding.ASCII.GetBytes("0")).ToArray();
                                Console.WriteLine(stegByte);
                            }

                            openAllConnections();
                            String sqlQuery = "SELECT keyPath FROM dbo.test WHERE UserID='" + currentUserName + "'";
                            SqlCommand cmd = new SqlCommand(sqlQuery, con);
                            SqlDataReader DataRead1 = cmd.ExecuteReader();
                            string bigPath = null;
                            while (DataRead1.Read())
                            {
                                bigPath = DataRead1.GetString(0);
                            }

                            byte[] IV = File.ReadAllBytes(@bigPath + "\\IV.txt");
                            KeyController kc = new KeyController();
                            byte[] EncryptedStr = kc.symmetricEncryption(hideThis, stegByte, IV);
                            String hideThisStr = Convert.ToBase64String(EncryptedStr);
                            bmp = Steganography.embedText(hideThisStr, bmp);
                            fileName = System.IO.Path.GetFileNameWithoutExtension(fileName);

                        }

                    }
                }
            }

            //Use a MemoryStream to get byte[] from bmp
            MemoryStream stream = new MemoryStream();
            bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] bmpInByteArr = stream.ToArray();


            //Slice the byte arrays into 3 parts
            byte[] toSend1 = null;
            byte[] toSend2 = null;
            byte[] toSend3 = null;

            if (bmpInByteArr.Length % 3 == 0)
            {
                int len = bmpInByteArr.Length / 3;
                toSend1 = bmpInByteArr.Take(len).ToArray();
                toSend2 = bmpInByteArr.Skip(len).Take(len).ToArray();
                int len2 = len + len;
                toSend3 = bmpInByteArr.Skip(len2).Take(len).ToArray();
            }

            else if (bmpInByteArr.Length % 3 == 1)
            {
                int len = (bmpInByteArr.Length / 3) + 1;
                int len2 = bmpInByteArr.Length - len;
                int len3 = len2 / 2;
                toSend1 = bmpInByteArr.Take(len3).ToArray();
                toSend2 = bmpInByteArr.Skip(len3).Take(len3).ToArray();
                toSend3 = bmpInByteArr.Skip(len2).Take(len).ToArray();
            }

            else if (bmpInByteArr.Length % 3 == 2)
            {
                int len = (bmpInByteArr.Length / 3) + 2;
                int len2 = bmpInByteArr.Length - len;
                int len3 = len2 / 2;
                toSend1 = bmpInByteArr.Take(len3).ToArray();
                toSend2 = bmpInByteArr.Skip(len3).Take(len3).ToArray();
                toSend3 = bmpInByteArr.Skip(len2).Take(len).ToArray();
            }

            openAllConnections();

            //SQL Update Statements
            string sqlQuery1 = ("insert into [dbo].[UserFiles1] values('" + currentUserName + "', '" + fileName+ "', @toSend1, '" + getFileSize(bmpInByteArr.Length) + "', '" + getCurrent() + "', 'no', 'no', '" + fileExtension + "', '" + currentUserName + "')");
            cmd1 = new SqlCommand(sqlQuery1, con1);
            SqlParameter para1 = new SqlParameter("@toSend1", toSend1);
            cmd1.Parameters.Add(para1);
            string sqlQuery2 = ("insert into [dbo].[UserFiles3] values('" + currentUserName + "', '" + fileName + "', @toSend2, '" + getFileSize(bmpInByteArr.Length) + "', '" + getCurrent() + "', 'no', 'no', '" + fileExtension + "', '" + currentUserName + "')");
            cmd2 = new SqlCommand(sqlQuery2, con2);
            SqlParameter para2 = new SqlParameter("@toSend2", toSend2);
            cmd2.Parameters.Add(para2);
            string sqlQuery3 = ("insert into [dbo].[UserFiles2] values('" + currentUserName + "', '" + fileName + "', @toSend3, '" + getFileSize(bmpInByteArr.Length) + "', '" + getCurrent() + "', 'no', 'no', '" + fileExtension + "', '" + currentUserName + "')");
            cmd3 = new SqlCommand(sqlQuery3, con3);
            SqlParameter para3 = new SqlParameter("@toSend3", toSend3);
            cmd3.Parameters.Add(para3);

            cmd1.ExecuteNonQuery();
            cmd2.ExecuteNonQuery();
            cmd3.ExecuteNonQuery();

            closeAllConnections();
        }
   
        
        private void stegoDecrypt(object sender, RoutedEventArgs e)
        {
            String selectedText = ((DataRowView)listView.SelectedItem)["Name"].ToString();

            openAllConnections();

            string sqlQuery1 = "select [File] from [dbo].[UserFiles1] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
            SqlCommand cmd1 = new SqlCommand(sqlQuery1, con1);
            string sqlQuery2 = "select [File] from [dbo].[UserFiles3] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
            SqlCommand cmd2 = new SqlCommand(sqlQuery2, con2);
            string sqlQuery3 = "select [File] from [dbo].[UserFiles2] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
            SqlCommand cmd3 = new SqlCommand(sqlQuery3, con3);

            SqlDataReader Reader1 = cmd1.ExecuteReader();
            SqlDataReader Reader2 = cmd2.ExecuteReader();
            SqlDataReader Reader3 = cmd3.ExecuteReader();

            //Cannot read data! Aku Justin Big Boi and Bryan Small Boi Tak Boleh Do this Functiono 
            //So we tiredo, so we helpo youdo tomorrowdo! Aso!

            Reader1.Read();
            Reader2.Read();
            Reader3.Read();

            byte[] retrieve1 = ((byte[])Reader1[0]);
            byte[] retrieve2 = ((byte[])Reader2[0]);
            byte[] retrieve3 = ((byte[])Reader3[0]);

            byte[] retrieve = retrieve1.Concat(retrieve2).Concat(retrieve3).ToArray();

            string extractThis;
            Bitmap bmp;
            //System.Drawing.Image img;
            KeyController kc = new KeyController();

            using (var ms = new MemoryStream(retrieve))
            {
                bmp = new Bitmap(ms);
            }

            //extractThis = Steganography.extractText(bmp);

            //bmp = new Bitmap(img);
            extractThis = Steganography.extractText(bmp);
     
            stegPass = Layout.Controllers.Prompt.ShowDialog3("Enter password", "Prompt");

            if (stegPass.Length > 8)
            {
                stegPass = stegPass.Substring(0, 8);
            }

            byte[] toBytes = Convert.FromBase64String(extractThis);
            byte[] stegByte = Convert.FromBase64String(stegPass);

            while (stegByte.Length != 32)
            {
                stegByte = stegByte.Concat(Encoding.ASCII.GetBytes("0")).ToArray();
                Console.WriteLine(stegByte);
            }

            openAllConnections();
            SqlCommand cmd;
            string sqlQuery = "SELECT keyPath FROM dbo.test WHERE UserID='" + currentUserName + "'";
            cmd = new SqlCommand(sqlQuery, con);
            SqlDataReader DataRead1 = cmd.ExecuteReader();
            string bigPath = null;
            while (DataRead1.Read())
            {
                bigPath = DataRead1.GetString(0);
            }
            closeAllConnections();

            byte[] IV = File.ReadAllBytes(@bigPath + "\\IV.txt");

            byte[] DecryptedStr;

            try
            {
                DecryptedStr = kc.symmetricDecryption(toBytes, stegByte, IV);
            }
            catch
            {
                DecryptedStr = new byte[69];
            }
            string result = System.Text.Encoding.UTF8.GetString(DecryptedStr);

            System.Windows.MessageBox.Show("Select where you want to save your file");
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.ShowDialog();

            System.IO.File.WriteAllText(@fbd.SelectedPath + "\\SECRET MESSAGE.txt", result);
           
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
        private void colorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
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
                rtfText = Encoding.Unicode.GetString(ms.ToArray());
            }

            byte[] byteArray = Encoding.Unicode.GetBytes(rtfText);
            
            openAllConnections();

            byte[] toSend1 = null;
            byte[] toSend2 = null;
            byte[] toSend3 = null;

            if (byteArray.Length % 3 == 0)
            {
                int len = byteArray.Length / 3;
                toSend1 = byteArray.Take(len).ToArray();
                toSend2 = byteArray.Skip(len).Take(len).ToArray();
                int len2 = len + len;
                toSend3 = byteArray.Skip(len2).Take(len).ToArray();
            }

            else if (byteArray.Length % 3 == 1)
            {
                int len = (byteArray.Length / 3) + 1;
                int len2 = byteArray.Length - len;
                int len3 = len2 / 2;
                toSend1 = byteArray.Take(len3).ToArray();
                toSend2 = byteArray.Skip(len3).Take(len3).ToArray();
                toSend3 = byteArray.Skip(len2).Take(len).ToArray();
            }

            else if (byteArray.Length % 3 == 2)
            {
                int len = (byteArray.Length / 3) + 2;
                int len2 = byteArray.Length - len;
                int len3 = len2 / 2;
                toSend1 = byteArray.Take(len3).ToArray();
                toSend2 = byteArray.Skip(len3).Take(len3).ToArray();
                toSend3 = byteArray.Skip(len2).Take(len).ToArray();
            }

            string sqlQuery1 = "update [dbo].[UserFiles1] set [File] = @toSend1, fileSize = '" + getFileSize(byteArray.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "' and Name = '" + fileName.Content + "'";
            cmd1 = new SqlCommand(sqlQuery1, con1);
            SqlParameter para1 = new SqlParameter("@toSend1", toSend1);
            cmd1.Parameters.Add(para1);
            string sqlQuery2 = "update [dbo].[UserFiles3] set [File] = @toSend2, fileSize = '" + getFileSize(byteArray.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "' and Name = '" + fileName.Content + "'";
            cmd2 = new SqlCommand(sqlQuery2, con2);
            SqlParameter para2 = new SqlParameter("@toSend2", toSend2);
            cmd2.Parameters.Add(para2);
            string sqlQuery3 = "update [dbo].[UserFiles2] set [File] = @toSend3, fileSize = '" + getFileSize(byteArray.Length) + "', lastModified = '" + getCurrent() + "' where Username = '" + currentUserName + "' and Name = '" + fileName.Content + "'";
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

            string sqlQuery1 = "select [File] from [dbo].[UserFiles1] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
            cmd1 = new SqlCommand(sqlQuery1, con1);
            string sqlQuery2 = "select [File] from [dbo].[UserFiles3] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
            cmd2 = new SqlCommand(sqlQuery2, con2);
            string sqlQuery3 = "select [File] from [dbo].[UserFiles2] where Name = '" + selectedText + "' and Username = '" + currentUserName + "'";
            cmd3 = new SqlCommand(sqlQuery3, con3);

            SqlDataReader Reader1 = cmd1.ExecuteReader();
            SqlDataReader Reader2 = cmd2.ExecuteReader();
            SqlDataReader Reader3 = cmd3.ExecuteReader();

            byte[] retrieve1 = ((byte[])Reader1[0]);
            byte[] retrieve2 = ((byte[])Reader2[0]);
            byte[] retrieve3 = ((byte[])Reader3[0]);

            byte[] retrieve = retrieve1.Concat(retrieve3).Concat(retrieve2).ToArray();

            theText = Encoding.Unicode.GetString(retrieve);

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
                //NavigationService.Navigate(new Uri("Page1.xaml", UriKind.RelativeOrAbsolute));
                Page seanPage = new Layout.Upload.Page1();
                NavigationService.Navigate(seanPage);
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

                    openAllConnections();

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
                        closeAllConnections();
                            virus = virus + 1;


                        }
                        else
                        {
                        closeAllConnections();
                        openAllConnections();

                            string finalPath = fullPath();

                            FileStream Stream = new FileStream(finalPath, FileMode.Open, FileAccess.Read);
                            BinaryReader brs = new BinaryReader(Stream);
                            byte[] images = brs.ReadBytes((int)Stream.Length);

                            String strImage = System.Text.Encoding.UTF8.GetString(images);


                            SqlCommand cmd10 = new SqlCommand("SELECT vxVirusName, vxType,vxMD5,BYTE FROM [User].[dbo].[vx] WHERE tID = 5002 ", con);

                        //  cmd2.Parameters.Add(new SqlParameter("@BYTE", strImage));
                      
                            SqlDataReader reader5 = cmd10.ExecuteReader();

                            if (reader5.Read() == true)
                            {
                                if (strImage.Contains(reader5.GetString(3)))
                                {
                                    virus = virus + 1;
                                    VirusName = reader5.GetString(0);
                                    type = reader5.GetString(1);

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
                        closeAllConnections();
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
                        openAllConnections();
                        SqlCommand cmd;
                        string sqlQuery = "SELECT keyPath FROM dbo.test WHERE UserID='" + currentUserName + "'";
                        cmd = new SqlCommand(sqlQuery, con);
                        SqlDataReader DataRead1 = cmd.ExecuteReader();
                        string bigPath = null;
                        while(DataRead1.Read())
                        { 
                         bigPath = DataRead1.GetString(0);
                        }
                        closeAllConnections();
                        
                      
                        byte[] IV = System.IO.File.ReadAllBytes(@bigPath + "\\IV.txt");
                        Console.WriteLine("Gets bytes of IV");
                        byte[] encryptedSymmetricKey = System.IO.File.ReadAllBytes(@bigPath + "\\encryptedSymmetricKey.txt");

                        //Gets the symmetric key by decrypting the encrypted symmetric key with the decryption (private) key
                        byte[] decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                        //Encrypts plaintext with symmetric key
                        byte[] cipherText = kc.symmetricEncryption(file, decryptedSymmetricKey, IV);

                        FileModel fm = new FileModel(currentUserName, filename, cipherText, getFileSize(cipherText.Length), getCurrent(), "no", "no", extension, "" + currentUserName + "");
                        fm.setShow(true);
                        FileModel.setFileModel(fm);

                        if (inWhere.Equals("My Folders") || inWhere.Equals("Recent") || inWhere.Equals("Shared") || inWhere.Equals("Favorites") || inWhere.Equals("Bin"))
                        {

                            openAllConnections();
                            //inWhere.Text is parent or child in the Datbase?!
                            String sqlQuery1 = "insert into [dbo].[AccessControl] values('" + filename + "', '', '" + currentUserName + "')";
                            cmd1 = new SqlCommand(sqlQuery1, con1);

                            cmd1.ExecuteNonQuery();
                            closeAllConnections();
                        }

                        else if (!inWhere.Equals("My Folders") || !inWhere.Equals("Recent") || !inWhere.Equals("Shared") || !inWhere.Equals("Favorites") || !inWhere.Equals("Bin"))
                        {
                            openAllConnections();
                            //inWhere.Text is parent or child in the Datbase?!
                            
                            String sqlQuery1 = "insert into [dbo].[AccessControl] values('" + filename + "', '" + inWhere.Text + "', '" + currentUserName + "')";
                            cmd1 = new SqlCommand(sqlQuery1, con1);

                            cmd1.ExecuteNonQuery();
                            closeAllConnections();
                        }

                        //FileModel fm = FileModel.getFileModel();
                        Page UploadingConsole = new Layout.Upload.Page2();
                        NavigationService.Navigate(UploadingConsole);
                    }
                    
                    closeAllConnections();
                
            }
        }
    }

        //UPLOAD AND OPEN MS WORD DOC FROM FILE EXPLORER
        public void openMSBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Layout.Controllers.Prompt.ShowDialog2("Do you want to ensure file integrity? (Includes extra steps)", "Prompt") == true)
            {
                Page seanPage = new Layout.Upload.Page1();
                NavigationService.Navigate(seanPage);
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


                    openAllConnections();
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
                        closeAllConnections();
                        virus = virus + 1;


                    }
                    else
                    {
                        openAllConnections();

                        string finalPath = fullPath();

                        FileStream Stream = new FileStream(finalPath, FileMode.Open, FileAccess.Read);
                        BinaryReader brs = new BinaryReader(Stream);
                        byte[] images = brs.ReadBytes((int)Stream.Length);

                        String strImage = System.Text.Encoding.UTF8.GetString(images);


                        cmd2 = new SqlCommand("SELECT vxVirusName, vxType,vxMD5,BYTE FROM [User].[dbo].[vx] WHERE tID = 5002 ", con);

                        //  cmd2.Parameters.Add(new SqlParameter("@BYTE", strImage));
                        closeAllConnections();
                        openAllConnections();
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
                        closeAllConnections();
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
                        openAllConnections();
                        SqlCommand cmd;
                        string sqlQuery = "SELECT keyPath FROM dbo.test WHERE UserID='" + currentUserName + "'";
                        cmd = new SqlCommand(sqlQuery, con);
                        SqlDataReader DataRead1 = cmd.ExecuteReader();
                        string bigPath = null;
                        while (DataRead1.Read())
                        {
                            bigPath = DataRead1.GetString(0);
                        }
                        closeAllConnections();


                        byte[] IV = System.IO.File.ReadAllBytes(@bigPath + "\\IV.txt");
                        Console.WriteLine("Gets bytes of IV");
                        byte[] encryptedSymmetricKey = System.IO.File.ReadAllBytes(@bigPath + "\\encryptedSymmetricKey.txt");

                        //Gets the symmetric key by decrypting the encrypted symmetric key with the decryption (private) key
                        byte[] decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                        //Encrypts plaintext with symmetric key
                        byte[] cipherText = kc.symmetricEncryption(file, decryptedSymmetricKey, IV);

                        FileModel fm = new FileModel(currentUserName, filename, cipherText, getFileSize(cipherText.Length), getCurrent(), "no", "no", extension, "" + currentUserName + "");
                        fm.setShow(true);
                        FileModel.setFileModel(fm);

                        if (inWhere.Equals("My Folders") || inWhere.Equals("Recent") || inWhere.Equals("Shared") || inWhere.Equals("Favorites") || inWhere.Equals("Bin"))
                        {
                            openAllConnections();
                            //inWhere.Text is parent or child in the Datbase?!
                            String sqlQuery1 = "insert into [dbo].[AccessControl] values('" + filename + "', '', '" + currentUserName + "')";
                            cmd1 = new SqlCommand(sqlQuery1, con1);

                            cmd1.ExecuteNonQuery();
                            closeAllConnections();
                        }

                        else if (!inWhere.Equals("My Folders") || !inWhere.Equals("Recent") || !inWhere.Equals("Shared") || !inWhere.Equals("Favorites") || !inWhere.Equals("Bin"))
                        {
                            openAllConnections();
                            //inWhere.Text is parent or child in the Datbase?!

                            String sqlQuery1 = "insert into [dbo].[AccessControl] values('" + filename + "', '" + inWhere.Text + "', '" + currentUserName + "')";
                            cmd1 = new SqlCommand(sqlQuery1, con1);

                            cmd1.ExecuteNonQuery();
                            closeAllConnections();
                        }

                        //FileModel fm = FileModel.getFileModel();
                        Page UploadingConsole = new Layout.Upload.Page2();
                        NavigationService.Navigate(UploadingConsole);
                    }

                    closeAllConnections();
                }
            }
        }


        //UPLOAD AND OPEN EXCEL FILE FROM FILE EXPLORER
        private void openEXLBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Layout.Controllers.Prompt.ShowDialog2("Do you want to ensure file integrity? (Includes extra steps)", "Prompt") == true)
            {
                Page seanPage = new Layout.Upload.Page1();
                NavigationService.Navigate(seanPage);
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

                    openAllConnections();

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
                        closeAllConnections();
                        virus = virus + 1;


                    }
                    else
                    {
                        closeAllConnections();
                        openAllConnections();

                        string finalPath = fullPath();

                        FileStream Stream = new FileStream(finalPath, FileMode.Open, FileAccess.Read);
                        BinaryReader brs = new BinaryReader(Stream);
                        byte[] images = brs.ReadBytes((int)Stream.Length);

                        String strImage = System.Text.Encoding.UTF8.GetString(images);


                        SqlCommand cmd10 = new SqlCommand("SELECT vxVirusName, vxType,vxMD5,BYTE FROM [User].[dbo].[vx] WHERE tID = 5002 ", con);

                        //  cmd2.Parameters.Add(new SqlParameter("@BYTE", strImage));

                        SqlDataReader reader5 = cmd10.ExecuteReader();

                        if (reader5.Read() == true)
                        {
                            if (strImage.Contains(reader5.GetString(3)))
                            {
                                virus = virus + 1;
                                VirusName = reader5.GetString(0);
                                type = reader5.GetString(1);

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
                        closeAllConnections();
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
                        openAllConnections();
                        SqlCommand cmd;
                        string sqlQuery = "SELECT keyPath FROM dbo.test WHERE UserID='" + currentUserName + "'";
                        cmd = new SqlCommand(sqlQuery, con);
                        SqlDataReader DataRead1 = cmd.ExecuteReader();
                        string bigPath = null;
                        while (DataRead1.Read())
                        {
                            bigPath = DataRead1.GetString(0);
                        }
                        closeAllConnections();


                        byte[] IV = System.IO.File.ReadAllBytes(@bigPath + "\\IV.txt");
                        Console.WriteLine("Gets bytes of IV");
                        byte[] encryptedSymmetricKey = System.IO.File.ReadAllBytes(@bigPath + "\\encryptedSymmetricKey.txt");

                        //Gets the symmetric key by decrypting the encrypted symmetric key with the decryption (private) key
                        byte[] decryptedSymmetricKey = kc.asymmetricDecryption(encryptedSymmetricKey);
                        //Encrypts plaintext with symmetric key
                        byte[] cipherText = kc.symmetricEncryption(file, decryptedSymmetricKey, IV);

                        FileModel fm = new FileModel(currentUserName, filename, cipherText, getFileSize(cipherText.Length), getCurrent(), "no", "no", extension, "" + currentUserName + "");
                        fm.setShow(true);
                        FileModel.setFileModel(fm);

                        if (inWhere.Equals("My Folders") || inWhere.Equals("Recent") || inWhere.Equals("Shared") || inWhere.Equals("Favorites") || inWhere.Equals("Bin"))
                        {

                            openAllConnections();
                            //inWhere.Text is parent or child in the Datbase?!
                            String sqlQuery1 = "insert into [dbo].[AccessControl] values('" + filename + "', '', '" + currentUserName + "')";
                            cmd1 = new SqlCommand(sqlQuery1, con1);

                            cmd1.ExecuteNonQuery();
                            closeAllConnections();
                        }

                        else if (!inWhere.Equals("My Folders") || !inWhere.Equals("Recent") || !inWhere.Equals("Shared") || !inWhere.Equals("Favorites") || !inWhere.Equals("Bin"))
                        {
                            openAllConnections();
                            //inWhere.Text is parent or child in the Datbase?!

                            String sqlQuery1 = "insert into [dbo].[AccessControl] values('" + filename + "', '" + inWhere.Text + "', '" + currentUserName + "')";
                            cmd1 = new SqlCommand(sqlQuery1, con1);

                            cmd1.ExecuteNonQuery();
                            closeAllConnections();
                        }

                        //FileModel fm = FileModel.getFileModel();
                        Page UploadingConsole = new Layout.Upload.Page2();
                        NavigationService.Navigate(UploadingConsole);
                    }

                    closeAllConnections();

                }
            }
        }

        private void MyFoldersButton_Click(object sender, RoutedEventArgs e)
        {
            unfavoriteHeader.Visibility = Visibility.Collapsed;
            recoverHeader.Visibility = Visibility.Collapsed;
            deleteHeader.Visibility = Visibility.Visible;

            listView.Visibility = Visibility.Visible;
            mainToolBar.Visibility = Visibility.Visible;
            rtbEditor.Visibility = Visibility.Collapsed;
            secondToolBar.Visibility = Visibility.Collapsed;

            currentPage = "MyFolders";
            inWhere.Text = currentPage;

            sortNormally();

            MyFoldersButton.Background = (System.Windows.Media.Brush)(new BrushConverter().ConvertFrom("#23aeff"));
            RecentButton.Background = (System.Windows.Media.Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            SharedButton.Background = (System.Windows.Media.Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            FavoritesButton.Background = (System.Windows.Media.Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            BinButton.Background = (System.Windows.Media.Brush)(new BrushConverter().ConvertFrom("#8c9199"));

        }


        private void RecentButton_Click(object sender, RoutedEventArgs e)
        {
            unfavoriteHeader.Visibility = Visibility.Collapsed;
            recoverHeader.Visibility = Visibility.Collapsed;
            deleteHeader.Visibility = Visibility.Visible;

            listView.Visibility = Visibility.Visible;
            mainToolBar.Visibility = Visibility.Visible;
            rtbEditor.Visibility = Visibility.Collapsed;
            secondToolBar.Visibility = Visibility.Collapsed;

            currentPage = "Recent";
            inWhere.Text = currentPage;
            sortByLastModified();

            MyFoldersButton.Background = (System.Windows.Media.Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            RecentButton.Background = (System.Windows.Media.Brush)(new BrushConverter().ConvertFrom("#23aeff"));
            SharedButton.Background = (System.Windows.Media.Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            FavoritesButton.Background = (System.Windows.Media.Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            BinButton.Background = (System.Windows.Media.Brush)(new BrushConverter().ConvertFrom("#8c9199"));
        }


        private void SharedButton_Click(object sender, RoutedEventArgs e)
        {
            unfavoriteHeader.Visibility = Visibility.Collapsed;
            recoverHeader.Visibility = Visibility.Collapsed;
            deleteHeader.Visibility = Visibility.Visible;

            listView.Visibility = Visibility.Visible;
            mainToolBar.Visibility = Visibility.Visible;
            rtbEditor.Visibility = Visibility.Collapsed;
            secondToolBar.Visibility = Visibility.Collapsed;

            currentPage = "Shared";
            inWhere.Text = currentPage;
            sortNormally();

            MyFoldersButton.Background = (System.Windows.Media.Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            RecentButton.Background = (System.Windows.Media.Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            SharedButton.Background = (System.Windows.Media.Brush)(new BrushConverter().ConvertFrom("#23aeff"));
            FavoritesButton.Background = (System.Windows.Media.Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            BinButton.Background = (System.Windows.Media.Brush)(new BrushConverter().ConvertFrom("#8c9199"));
        }


        private void FavoritesButton_Click(object sender, RoutedEventArgs e)
        {
            unfavoriteHeader.Visibility = Visibility.Visible;
            recoverHeader.Visibility = Visibility.Collapsed;
            deleteHeader.Visibility = Visibility.Visible;

            listView.Visibility = Visibility.Visible;
            mainToolBar.Visibility = Visibility.Visible;
            rtbEditor.Visibility = Visibility.Collapsed;
            secondToolBar.Visibility = Visibility.Collapsed;

            currentPage = "Favorites";
            inWhere.Text = currentPage;
            sortFavorites();

            MyFoldersButton.Background = (System.Windows.Media.Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            RecentButton.Background = (System.Windows.Media.Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            SharedButton.Background = (System.Windows.Media.Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            FavoritesButton.Background = (System.Windows.Media.Brush)(new BrushConverter().ConvertFrom("#23aeff"));
            BinButton.Background = (System.Windows.Media.Brush)(new BrushConverter().ConvertFrom("#8c9199"));
        }


        private void BinButton_Click(object sender, RoutedEventArgs e)
        {
            unfavoriteHeader.Visibility = Visibility.Collapsed;
            recoverHeader.Visibility = Visibility.Visible;

            listView.Visibility = Visibility.Visible;
            mainToolBar.Visibility = Visibility.Visible;
            rtbEditor.Visibility = Visibility.Collapsed;
            secondToolBar.Visibility = Visibility.Collapsed;

            currentPage = "Bin";
            inWhere.Text = currentPage;
            sortBin();

            MyFoldersButton.Background = (System.Windows.Media.Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            RecentButton.Background = (System.Windows.Media.Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            SharedButton.Background = (System.Windows.Media.Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            FavoritesButton.Background = (System.Windows.Media.Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            BinButton.Background = (System.Windows.Media.Brush)(new BrushConverter().ConvertFrom("#23aeff"));
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


        private void sortShared()
        {
            openAllConnections();

            string sqlQuery = "select Name, sharedBy, lastModified, fileSize from [dbo].[UserFiles1] where Username = '" + currentUserName + "' sharedBy <> '" + currentUserName + "'";
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

        private void editProfile(object sender, MouseEventArgs e)
        {

        }
        
             private void moveTo(object sender, RoutedEventArgs e)
        {

        }

        
    }
}

