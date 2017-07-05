using System;
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
        public static void keyCreation()
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
            FileStream ostrm;
            StreamWriter writer;
            TextWriter oldOut = Console.Out;
            ostrm = new FileStream(@"C:\\Users\\SengokuMedaru\\Desktop\\keys\\pubKey.txt", FileMode.OpenOrCreate, FileAccess.Write);
            writer = new StreamWriter(ostrm);
            Console.WriteLine(pubKeyString);
            writer.Close();
            ostrm.Close();

            FileStream ostrm1;
            StreamWriter writer1;
            TextWriter oldOut1 = Console.Out;
            ostrm1 = new FileStream(@"C:\\Users\\SengokuMedaru\\Desktop\\keys\\privKey.txt", FileMode.OpenOrCreate, FileAccess.Write);
            writer1 = new StreamWriter(ostrm1);
            Console.WriteLine(privKeyString);
            writer1.Close();
            ostrm1.Close();


        }







        public string encrypt()
        {
            var csp = new RSACryptoServiceProvider();


            //Converts Public key back from String object to var(?)
            //Gets a stream from the publicKey string
            string pubKeyReader = System.IO.File.ReadAllText(@"C:\\Users\\SengokuMedaru\\Desktop\\keys\\pubKey.txt");
            var stringReader1 = new System.IO.StringReader(pubKeyReader);
            //Use a serializer
            var xmlSeralize1 = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            //Gets the object back from the stream
            var pubKey = (RSAParameters)xmlSeralize1.Deserialize(stringReader1);

            //Loads the Public Key
            csp.ImportParameters(pubKey);



            //HARDCODED, PLEASE CHANGE AT A LATER DATE -S
            var plainTextData = "Bryan is Handsome af";

            //Translates plainText into Bytes
            var bytesPlainTextData = System.Text.Encoding.Unicode.GetBytes(plainTextData);

            //This is where the magic happens
            var bytesCipherText = csp.Encrypt(bytesPlainTextData, false);

            //Translates cipherText into Base64 so it's somewhat legible
            var cipherText = Convert.ToBase64String(bytesCipherText);

            Console.WriteLine("Encrypted String : " + cipherText);
            return cipherText;
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




        public void decrypt(String cipherText)
        {

            //Converts cipherText from Base64 back to byte[]
            var bytesCipherText = Convert.FromBase64String(cipherText);





            //Makes another csp thing with privateKey as input parameter
            var csp = new RSACryptoServiceProvider();

            //Converts Private key back from String object to var(?)           
            //Gets a stream from the privateKey string
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

        }







        //Method used to check if usable key pair exists
        public static void checkForKeys()
        {
            Console.WriteLine("Checking for keys");

            string path = @"C:\\Users\\SengokuMedaru\\Desktop\\keys\\pubKey.txt";
            if (!File.Exists(path))
            {
                keyCreation();
            }
        }



        //PROBLEM  : CANNOT CREATE MAIN METHOD TO TEST <SOLVED>
        //PROBLEM 2: NEW KEYS WILL BE GENERATED EACH TIME IT IS RUN, NEED TO CREATE A METHOD TO CHECK IF KEY-PAIR ALREADY EXISTS




    }
}