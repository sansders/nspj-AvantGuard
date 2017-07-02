using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Layout.Controllers
{
    class KeyController
    {

        static void keyCreation()
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

            //Sets key pair into KeyModel
            Models.KeyModel.setPublicKey(pubKeyString);
            Models.KeyModel.setPrivateKey(privKeyString);



        }







        static void encrypt()
        {
            var csp = new RSACryptoServiceProvider();


            //Converts Public key back from String object to var(?)
            //Gets a stream from the publicKey string
            var stringReader1 = new System.IO.StringReader(Models.KeyModel.getPublicKey);
            //Use a serializer
            var xmlSeralize1 = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            //Gets the object back from the stream
            var pubKey = (RSAParameters)xmlSeralize1.Deserialize(stringReader1);

            //Loads the Public Key
            csp.ImportParameters(pubKey);



            //HARDCODED, PLEASE CHANGE AT A LATER DATE -S
            var plainTextData = "Bryan";

            //Translates plainText into Bytes
            var bytesPlainTextData = System.Text.Encoding.Unicode.GetBytes(plainTextData);

            //This is where the magic happens
            var bytesCipherText = csp.Encrypt(bytesPlainTextData, false);

            //Translates cipherText into Base64 so it's somewhat legible
            var cipherText = Convert.ToBase64String(bytesCipherText);
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




        static void decrypt(String cipherText)
        {

            //Converts cipherText from Base64 back to byte[]
            var bytesCipherText = Convert.FromBase64String(cipherText);





            //Makes another csp thing with privateKey as input parameter
            var csp = new RSACryptoServiceProvider();

            //Converts Private key back from String object to var(?)           
            //Gets a stream from the privateKey string
            var stringReader2 = new System.IO.StringReader(Models.KeyModel.getPrivateKey);
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
           
        }
        




        //PROBLEM  : CANNOT CREATE MAIN METHOD TO TEST
        //PROBLEM 2: NEW KEYS WILL BE GENERATED EACH TIME IT IS RUN, NEED TO CREATE A METHOD TO CHECK IF KEY-PAIR ALREADY EXISTS

    }
}
