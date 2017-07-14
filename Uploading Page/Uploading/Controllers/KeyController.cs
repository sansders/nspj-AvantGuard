﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Layout.Controllers
{
    class KeyController
    {
        public KeyController()
        {
            checkForKeys();
        }
        public static void checkForKeys()
        {
            Console.WriteLine("Checking for keys");

            //CHANGE PATH WHEREVER NECESSARY 
            string path = @"C:\\Users\\SengokuMedaru\\Desktop\\keys\\IV.txt";
            if (!File.Exists(path))
            {
                ivCreation();
                asymmetricKeyCreation();
                asymmetricEncryption(symmetricKeyCreation());
            }
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
            System.IO.File.WriteAllText(@"C:\\Users\\SengokuMedaru\\Desktop\\keys\\pubKey.txt", pubKeyString);
            System.IO.File.WriteAllText(@"C:\\Users\\SengokuMedaru\\Desktop\\keys\\privKey.txt", privKeyString);
            
           
        }
        public static byte[] symmetricKeyCreation()
        {
            /*FAILED CODE  
             *FAILED CODE  
             *FAILED CODE  
             *FAILED CODE  
             *FAILED CODE  
             * //Creates an instance of AES that will contain all the methods required for AES Symmetric Encryption
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();

                //Generates symmetric key & initialization vector
                aes.GenerateKey();
                aes.GenerateIV();



                /*string aesKeyString;
                {
                    var stringWriter = new System.IO.StringWriter();
                    //Creates a serializer
                    var xmlSerialize = new System.Xml.Serialization.XmlSerializer(typeof(AesCryptoServiceProvider));
                    //Serializes the key into the stream
                    xmlSerialize.Serialize(stringWriter, aes.GenerateKey());
                    //Gets the string from the stream
                    pubKeyString = stringWriter.ToString();
                }
            *FAILED CODE  
            *FAILED CODE  
            *FAILED CODE  
            *FAILED CODE   
            *PROBLEM: CREATED KEY CANNOT BE STORED. -Sean 13.7.2017
            */





            //Creates an instance of Rijndael that will contain all the methods required for AES Symmetric Encryption
            RijndaelManaged Crypto = new RijndaelManaged();
            System.Text.UTF8Encoding UTF = new System.Text.UTF8Encoding();

            //Assigns key to byte[] and string variables
            byte[] byteSymmetricKey = Crypto.Key;
            string stringSymmetricKey = UTF.GetString(byteSymmetricKey);


            string stingSymmetricKey = UTF.GetString(byteSymmetricKey);

            //Saves string of IV
            //CHANGE PATH WHENEVER NECCESSARY
            System.IO.File.WriteAllText(@"C:\\Users\\SengokuMedaru\\Desktop\\keys\\symmetricKey.txt", stringSymmetricKey);


            return byteSymmetricKey;
        }
        public static void ivCreation()
        {
            RijndaelManaged Crypto = new RijndaelManaged();
            System.Text.UTF8Encoding UTF = new System.Text.UTF8Encoding();

            byte[] IV = Crypto.IV;
            string stringIV = UTF.GetString(IV);

            //Saves string of IV
            //CHANGE PATH WHENEVER NECCESSARY
            System.IO.File.WriteAllText(@"C:\\Users\\SengokuMedaru\\Desktop\\keys\\IV.txt", stringIV);
        }



        public byte[] symmetricEncryption(string plainText, byte[] Key, byte[] IV)
        {
            System.Text.UTF8Encoding UTF = new System.Text.UTF8Encoding();

            //Just get bytes from plain text
            byte[] plainBytes = UTF.GetBytes(plainText);

            RijndaelManaged Crypto = new RijndaelManaged();
            Crypto.Key = Key;
            Crypto.IV = IV;

            MemoryStream MemStream = new MemoryStream();

            //Creates the method to Encrypt, inputs are the Symmetric Key & IV
            ICryptoTransform Encryptor = Crypto.CreateEncryptor(Crypto.Key, Crypto.IV);

            //Writes the data to memory to perform the transformation
            CryptoStream Crypto_Stream = new CryptoStream(MemStream, Encryptor, CryptoStreamMode.Write);

            //Takes in string to be encrypted, offset value, & length of string to be encrypted
            Crypto_Stream.Write(plainBytes, 0, plainBytes.Length);

            //Returns the memory byte array
            return MemStream.ToArray();
        }

        public static void asymmetricEncryption(byte[] symmetricKey)
        {
            //OBSELETE CODE
            //OBSELETE CODE
            /*
            var csp = new RSACryptoServiceProvider();


            //Converts Public key back from String object to var(?)
            //Gets a stream from the publicKey string
            //CHANGE PATH WHEREVER NECESSARY 
            string pubKeyReader = System.IO.File.ReadAllText(@"C:\\Users\\SengokuMedaru\\Desktop\\keys\\pubKey.txt");
            var stringReader1 = new System.IO.StringReader(pubKeyReader);
            //Use a serializer
            var xmlSeralize1 = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            //Gets the object back from the stream
            var pubKey = (RSAParameters)xmlSeralize1.Deserialize(stringReader1);

            //Loads the Public Key
            csp.ImportParameters(pubKey);



            //HARDCODED, PLEASE CHANGE AT A LATER DATE -S
            //plainTextData = "Bryan is Handsome af";

            //Translates plainText into Bytes
            var bytesPlainTextData = System.Text.Encoding.Unicode.GetBytes(symmetricKey);

            //This is where the magic happens
            var bytesCipherText = csp.Encrypt(bytesPlainTextData, false);

            //Translates cipherText into Base64 so it's somewhat legible
            var cipherText = Convert.ToBase64String(bytesCipherText);

            Console.WriteLine("Encrypted String : " + cipherText);
            return cipherText;
            */
            //OBSELETE CODE
            //OBSELETE CODE



            var csp = new RSACryptoServiceProvider();

            //Converts Public key back from String object to var(?)
            //Gets a stream from the publicKey string
            //CHANGE PATH WHEREVER NECESSARY 
            string pubKeyReader = System.IO.File.ReadAllText(@"C:\\Users\\SengokuMedaru\\Desktop\\keys\\pubKey.txt");
            var stringReader1 = new System.IO.StringReader(pubKeyReader);
            //Use a serializer
            var xmlSeralize1 = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            //Gets the object back from the stream
            var pubKey = (RSAParameters)xmlSeralize1.Deserialize(stringReader1);

            //Loads the Public Key
            csp.ImportParameters(pubKey);

            //Encrypts symmetric key with 
            byte[] encryptedSymmetricKeyBytes = csp.Encrypt(symmetricKey, false);

            //Changes byte[] of encryptedSymmetricKey into a string and saves it into a file
            System.Text.UTF8Encoding UTF = new System.Text.UTF8Encoding();
            string encryptedSymmetricKeyString = UTF.GetString(encryptedSymmetricKeyBytes);
            System.IO.File.WriteAllText(@"C:\\Users\\SengokuMedaru\\Desktop\\keys\\encryptedSymmetricKey.txt", encryptedSymmetricKeyString);
        }





        /*
         * Transfer cipherText into Database
         * 
         * 
         */


        /*
         * 
         * Retreive cipherText from Database
         * 
         */




        public byte[] asymmetricDecryption(byte[] encryptedSymmetricKeyBytes)
        {
            //OBSELETE CODE
            //OBSELETE CODE
            /*
            //Converts cipherText from Base64 back to byte[]
            var bytesCipherText = Convert.FromBase64String(cipherText);

            //Makes another csp thing with privateKey as input parameter
            var csp = new RSACryptoServiceProvider();

            //Converts Private key back from String object to var(?)           
            //Gets a stream from the privateKey string
            //CHANGE PATH WHEREVER NECESSARY 
            string privKeyReader = System.IO.File.ReadAllText(@"C:\\Users\\SengokuMedaru\\Desktop\\keys\\privKey.txt");
            var stringReader2 = new System.IO.StringReader(privKeyReader);
            //Use a serializer
            var xmlSeralize2 = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            //Gets the object back from the stream
            var privKey = (RSAParameters)xmlSeralize2.Deserialize(stringReader2);

            //Loads the Private Key
            csp.ImportParameters(privKey);


            //Where the magic happens
            var bytesPlainTextData = csp.Decrypt(bytesCipherText, false);

            //Gets plainText back in Unicode
            var plainTextData = System.Text.Encoding.Unicode.GetString(bytesPlainTextData);

            Console.WriteLine("Decryption Text : " + plainTextData);
            */
            //OBSELETE CODE
            //OBSELETE CODE





            //Makes another csp thing with privateKey as input parameter
            var csp = new RSACryptoServiceProvider();

            //Converts Private key back from String object to var(?)           
            //Gets a stream from the privateKey string
            //CHANGE PATH WHEREVER NECESSARY 
            string privKeyReader = System.IO.File.ReadAllText(@"C:\\Users\\SengokuMedaru\\Desktop\\keys\\privKey.txt");
            var stringReader2 = new System.IO.StringReader(privKeyReader);
            //Use a serializer
            var xmlSeralize2 = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            //Gets the object back from the stream
            var privKey = (RSAParameters)xmlSeralize2.Deserialize(stringReader2);

            //Loads the Private Key
            csp.ImportParameters(privKey);

            byte[] decryptedSymmetricKey = csp.Decrypt(encryptedSymmetricKeyBytes, false);

            return decryptedSymmetricKey;
        }


        public static string symmetricDecryption(byte[] cipherText, byte[] Key, byte[] IV)
        {
            RijndaelManaged Crypto = new RijndaelManaged();
            Crypto.Key = Key;
            Crypto.IV = IV;

            //Get stream of cipherText
            MemoryStream MemStream = new MemoryStream(cipherText);
            
            //Symmetric key & IV used here
            ICryptoTransform Decryptor = Crypto.CreateDecryptor(Crypto.Key, Crypto.IV);

            //For decryption, CryptoStreamMode.Read is used instead of .Write
            CryptoStream Crypto_Stream = new CryptoStream(MemStream, Decryptor, CryptoStreamMode.Read);

            //Reads CryptoStream
            StreamReader Stream_Read = new StreamReader(Crypto_Stream);
            //.ReadToEnd returns plain text
            string plainText = Stream_Read.ReadToEnd();

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


    }
}