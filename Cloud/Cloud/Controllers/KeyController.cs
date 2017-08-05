using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserModel;

namespace Layout.Controllers
{
    class KeyController
    {
        public KeyController()
        {
            checkForKeys();
        }

        public static string bigPath = null;
        public static string currentUserName = UserModel.UserModel.currentUserID;

        public static void checkForKeys()
        {
            Console.WriteLine("Checking for keys...");

            //SQL Conenction & Queries to get path of keys from Bryan's Database
            SqlConnection con1;
            SqlCommand cmd;
            ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["connString"];
            string connectionString1 = conSettings.ConnectionString;
            con1 = new SqlConnection(connectionString1);
            con1.Open();
            string sqlQuery1 = "SELECT keyPath FROM dbo.test WHERE UserID='"+currentUserName+"'";
            string sqlQuery2;
            cmd = new SqlCommand(sqlQuery1, con1);
            SqlDataReader DataRead1 = cmd.ExecuteReader();


            //Creates a FolderBrowserDialog which may or may not be used to create a new directory for key storage
            FolderBrowserDialog fbd = new FolderBrowserDialog();

 
            if (DataRead1.Read())
            {
                //If path to keys does not exist for the user, then create the path, the keys, and the IV
                if (DataRead1.IsDBNull(0))
                {
                    DataRead1.Close();
                    Console.WriteLine("Keys not found!");
                    Console.WriteLine("Proceeding with Key generation, please wait...");


                    //User selects the path of where he/she wants to store his/her keys at
                    System.Windows.MessageBox.Show("Please select a directory to store your keys");
                    fbd.ShowDialog();
                    bigPath = fbd.SelectedPath + "\\" + currentUserName;
                    Directory.CreateDirectory(bigPath);


                    //Updates the key path for the user in Bryan's database
                    sqlQuery2 = "UPDATE dbo.test SET keyPath = @bigPath WHERE UserID='" + currentUserName + "'";
                    cmd = new SqlCommand(sqlQuery2, con1);
                    cmd.Parameters.Add(new SqlParameter("@bigPath", bigPath));
                    cmd.ExecuteNonQuery();


                    //My favourite part aka the methods I made that creates keys for the user
                    //Keys created include Private Key, Public Key, Symmetric Key (only encrypted version is stored), and IV
                    ivCreation();
                    asymmetricKeyCreation();
                    asymmetricEncryption(symmetricKeyCreation());
                    Console.WriteLine("New keys successfully generated!");
                    Console.WriteLine("Commencing encryption...");
                    Console.WriteLine("");
                }



                //If path already exists in the database, then just get it from there
                else
                {             
                    bigPath = DataRead1.GetString(0);


                    //If the obtained path does not exist in current machine, get user to select the new key path
                    if (!Directory.Exists(bigPath))
                    {
                        System.Windows.MessageBox.Show("You already have keys!");
                        System.Windows.MessageBox.Show("Please select your new path to keys");

                        //Path selection
                        FolderBrowserDialog fbd1 = new FolderBrowserDialog();
                        fbd1.ShowDialog();
                        bigPath = fbd1.SelectedPath;

                        //Updates the key path for the user in Bryan's database
                        sqlQuery2 = "UPDATE dbo.test SET keyPath = @bigPath WHERE UserID='" + currentUserName + "'";
                        cmd = new SqlCommand(sqlQuery2, con1);
                        cmd.Parameters.Add(new SqlParameter("@bigPath", bigPath));
                        cmd.ExecuteNonQuery();
                    }
                }

            }
            con1.Close();
        }
        public static void asymmetricKeyCreation()
        {
            //Creates a new CSP with 4096 bit RSA key pair
            var csp = new RSACryptoServiceProvider(4096);

            //Obtains keys
            var privKey = csp.ExportParameters(true);
            var pubKey = csp.ExportParameters(false);

            //Converts Public Key into String object
            string pubKeyString;
            {
                var stringWriter = new System.IO.StringWriter();
                //Creates a serializer
                var xmlSerialize = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                //Serializes the key into the stream
                xmlSerialize.Serialize(stringWriter, pubKey);
                //Gets the string from the stream
                pubKeyString = stringWriter.ToString();
            }

            //Converts Private Key into String object
            string privKeyString;
            {
                var stringWriter = new System.IO.StringWriter();
                //Creates a serializer
                var xmlSerialize = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                //Serializes the key into the stream
                xmlSerialize.Serialize(stringWriter, privKey);
                //Gets the string from the stream
                privKeyString = stringWriter.ToString();
            }



            //Sets key pair into txt files
            //CHANGE PATH WHEREVER NECESSARY 
            System.IO.File.WriteAllText(@bigPath + "\\pubKey.txt", pubKeyString);
            System.IO.File.WriteAllText(@bigPath + "\\privKey.txt", privKeyString);


        }
        public static byte[] symmetricKeyCreation()
        {
            //Creates an instance of Rijndael that will contain all the methods required for Symmetric Encryption
            RijndaelManaged Crypto = new RijndaelManaged();

            //Assigns key to byte[] and string variables
            byte[] byteSymmetricKey = Crypto.Key;
            string stringSymmetricKey = Convert.ToBase64String(byteSymmetricKey);

            //Saves string of IV
            //CHANGE PATH WHENEVER NECCESSARY
            //System.IO.File.WriteAllText(@"C:\\Users\\SengokuMedaru\\Desktop\\keys\\symmetricKey.txt", stringSymmetricKey);
            File.WriteAllBytes(@bigPath + "\\encryptedSymmetricKey.txt", byteSymmetricKey);


            return byteSymmetricKey;
        }
        public static void ivCreation()
        {
            RijndaelManaged Crypto = new RijndaelManaged();
            byte[] IV = Crypto.IV;
            string stringIV = Convert.ToBase64String(IV);

            //Saves IV
            File.WriteAllBytes(@bigPath + "\\IV.txt", IV);
        }





        public byte[] symmetricEncryption(byte[] plainText, byte[] Key, byte[] IV)
        {
            RijndaelManaged rm = null;
            MemoryStream ms = null;
            ICryptoTransform Encryptor = null;
            //Crypto streams allow encryption in memory
            CryptoStream cs = null;
            //Just get bytes from plain text
            byte[] plainBytes = plainText;

            try
            {
                rm = new RijndaelManaged();
                rm.Key = Key;
                rm.IV = IV;
                rm.Mode = CipherMode.CBC;
                rm.Padding = PaddingMode.PKCS7;
                rm.BlockSize = 128;


                ms = new MemoryStream();

                //Creates the method to Encrypt, inputs are the Symmetric Key & IV
                Encryptor = rm.CreateEncryptor(rm.Key, rm.IV);

                //Writes the data to memory to perform the transformation
                cs = new CryptoStream(ms, Encryptor, CryptoStreamMode.Write);

                //Takes in string to be encrypted, offset value, & length of string to be encrypted
                cs.Write(plainBytes, 0, plainBytes.Length);
                cs.FlushFinalBlock();
            }
            finally
            {
                if (rm != null)
                {
                    rm.Clear();
                }
            }
            //Returns the memory byte array
            return ms.ToArray();
        }
        public static void asymmetricEncryption(byte[] symmetricKey)
        {
            var csp = new RSACryptoServiceProvider();

            //Converts Public key back from String object to var(?)
            //Gets a stream from the publicKey string
            string pubKeyReader = System.IO.File.ReadAllText(@bigPath + "\\pubKey.txt");
            var stringReader1 = new System.IO.StringReader(pubKeyReader);
            //Use a serializer
            var xmlSeralize1 = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            //Gets the object back from the stream
            var pubKey = (RSAParameters)xmlSeralize1.Deserialize(stringReader1);

            //Loads the Public Key
            csp.ImportParameters(pubKey);

            //Encrypts symmetric key with 
            byte[] encryptedSymmetricKeyBytes = csp.Encrypt(symmetricKey, false);

            //Saves byte[] of encryptedSymmetricKey into a file
            File.WriteAllBytes(@bigPath + "\\encryptedSymmetricKey.txt", encryptedSymmetricKeyBytes);
        }
        public byte[] asymmetricDecryption(byte[] encryptedSymmetricKeyBytes)
        {
            //Makes another csp thing with privateKey as input parameter
            var csp = new RSACryptoServiceProvider();

            //Converts Private key back from String object to var(?)           
            //Gets a stream from the privateKey string
            string privKeyReader = System.IO.File.ReadAllText(@bigPath + "\\privKey.txt");
            var stringReader2 = new System.IO.StringReader(privKeyReader);
            //Use a serializer
            var xmlSeralize2 = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            //Gets the object back from the stream
            var privKey = (RSAParameters)xmlSeralize2.Deserialize(stringReader2);

            //Loads the Private Key
            csp.ImportParameters(privKey);

            //Decrypts the encrypted Symmetric Key
            byte[] decryptedSymmetricKey = csp.Decrypt(encryptedSymmetricKeyBytes, false);

            return decryptedSymmetricKey;
        }
        public byte[] symmetricDecryption(byte[] cipherText, byte[] Key, byte[] IV)
        {
            RijndaelManaged rm = null;
            MemoryStream ms = null;
            ICryptoTransform Decryptor = null;
            //Crypto streams allow encryption in memory
            CryptoStream cs = null;
            StreamReader sr = null;
            byte[] plainText;

            try
            {
                rm = new RijndaelManaged();
                rm.Key = Key;
                rm.IV = IV;
                rm.Mode = CipherMode.CBC;
                rm.Padding = PaddingMode.PKCS7;
                rm.BlockSize = 128;

                //Get stream of cipherText
                ms = new MemoryStream(cipherText);

                //Symmetric key & IV used here
                Decryptor = rm.CreateDecryptor(rm.Key, rm.IV);

                //For decryption, CryptoStreamMode.Read is used instead of .Write
                cs = new CryptoStream(ms, Decryptor, CryptoStreamMode.Read);

                //Reads CryptoStream
                sr = new StreamReader(cs);

                //Copy the streamReader to a memoryStream, then convert the result to a byteArray
                var bytes = default(byte[]);
                using (var memstream = new MemoryStream())
                {
                    sr.BaseStream.CopyTo(memstream);
                    bytes = memstream.ToArray();
                }

                plainText = bytes;
            }
            finally
            {
                if (rm != null)
                {
                    rm.Clear();
                }
                ms.Flush();
                ms.Close();
            }
            return plainText;
        }






        //PROBLEM  : CANNOT CREATE MAIN METHOD TO TEST <SOLVED>
        //PROBLEM 2: NEW KEYS WILL BE GENERATED EACH TIME IT IS RUN, NEED TO CREATE A METHOD TO CHECK IF KEY-PAIR ALREADY EXISTS <SOLVED>


        // 11.7.2017 Update
        // RSA has been used to encrypt files
        //
        // However, new problem has arose
        // RSA can only encrypt tiny files
        // Need to use symmetric algorithm to encrypt files instead
        // RSA can be used to encrypt the symmetric keys instead, if that makes any sense
        // - Sean

        // 13.7.2017 Update
        // Methods for symmetric key generation, symmetric encryption algorithm, & symmetric decryption algorithm have been completed
        // Next will be to alter asymmetric encryption & decryption methods so that they encrypt/decrypt the symmetric key instead of the files
        // After which, event handler for Upload button have to be changed to use the symmetric encryption first
        // - Sean

        // 15.7.2017 Update
        // Encryption successfully completed!
        // Will have to test decryption next
        // - Sean

        // 27.7.2017 Update
        // Decryption decrypts, but gives incomplete output.
        // Vanilla does not match, chocolate matches.
        // Also, I've temporarily disabled encryption in Page1.xaml
        // - Sean

        // 30.7.2017 Update
        // Decryption's output has been fixed.
        // Solution was to flush the final block in order to get those last bytes.
        // Encryption in Page1.xaml has ben re-enabled.
        //
        // However, (sigh)
        // I realised that encryption/decryption only works properly on txt files and not other file formats like .png and .docx
        // Thats a huge problem :/
        // - Sean

        // 31.7.2017 Update
        // Problem in previous update is solved >:D
        // - Sean

        // 5.8.2017 Update
        // All key creation & encryption/decryption methods have been finalized.
        // I forsee some small errors here and there during integration but they won't be too hard to fix.
        // - Sean
    }
}