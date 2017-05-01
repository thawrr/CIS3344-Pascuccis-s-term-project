using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Utilities;

namespace GlobalMethods
{
    public class GMethods
    {
        // Deserialize the binary data to reconstruct the Account object
        public Account DeserializeAccount(Byte[] byteArray)
        {
            BinaryFormatter deSerializer = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream(byteArray);
            memStream.Position = 0;
            Account objAccount = (Account)deSerializer.Deserialize(memStream);

            return objAccount;
        }

        public string EncryptPassword(string plainTextPassword)
        {
            Byte[] key = { 250, 101, 18, 76, 45, 135, 207, 118, 4, 171, 3, 168, 202, 241, 37, 199 };
            Byte[] vector = { 146, 64, 191, 111, 23, 3, 113, 119, 231, 121, 252, 112, 79, 32, 114, 156 };
            string encryptedPassword;

            UTF8Encoding encoder = new UTF8Encoding();      
            Byte[] textBytes;                               

            // Perform Encryption
            textBytes = encoder.GetBytes(plainTextPassword);

            RijndaelManaged rmEncryption = new RijndaelManaged();
            MemoryStream myMemoryStream = new MemoryStream();
            CryptoStream myEncryptionStream = new CryptoStream(myMemoryStream, rmEncryption.CreateEncryptor(key, vector), CryptoStreamMode.Write);

            myEncryptionStream.Write(textBytes, 0, textBytes.Length);
            myEncryptionStream.FlushFinalBlock();

            myMemoryStream.Position = 0;
            Byte[] encryptedBytes = new Byte[myMemoryStream.Length];
            myMemoryStream.Read(encryptedBytes, 0, encryptedBytes.Length);

            // Close all the streams.
            myEncryptionStream.Close();
            myMemoryStream.Close();

            // Convert the bytes to a string and display it.
            encryptedPassword = Convert.ToBase64String(encryptedBytes);

            return encryptedPassword;
        }

        public string DecryptPassword(string encryptedPassword)
        {
            Byte[] key = { 250, 101, 18, 76, 45, 135, 207, 118, 4, 171, 3, 168, 202, 241, 37, 199 };
            Byte[] vector = { 146, 64, 191, 111, 23, 3, 113, 119, 231, 121, 252, 112, 79, 32, 114, 156 };

            Byte[] encryptedPasswordBytes = Convert.FromBase64String(encryptedPassword);
            Byte[] textBytes;
            String plainTextPassword;
            UTF8Encoding encoder = new UTF8Encoding();

            // Perform Decryption
            RijndaelManaged rmEncryption = new RijndaelManaged();
            MemoryStream myMemoryStream = new MemoryStream();
            CryptoStream myDecryptionStream = new CryptoStream(myMemoryStream, rmEncryption.CreateDecryptor(key, vector), CryptoStreamMode.Write);

            myDecryptionStream.Write(encryptedPasswordBytes, 0, encryptedPasswordBytes.Length);
            myDecryptionStream.FlushFinalBlock();

            myMemoryStream.Position = 0;
            textBytes = new Byte[myMemoryStream.Length];
            myMemoryStream.Read(textBytes, 0, textBytes.Length);

            // Close all the streams.
            myDecryptionStream.Close();
            myMemoryStream.Close();

            // Convert the bytes to a string and display it.
            plainTextPassword = encoder.GetString(textBytes);

            return plainTextPassword;
        }

        // Get the image url to show file type
        public string GetImageURL(string fileType)
        {
            string url;

            switch (fileType)
            {
                case "Text":
                    url = "images/text.png";
                    break;

                case "Portable Network Graphics":
                    url = "images/picture.png";
                    break;

                case "Joint Photographic Experts Group":
                    url = "images/picture.png";
                    break;

                case "Graphics Interchange Format":
                    url = "images/gif.png";
                    break;

                case "Windows Word Document":
                    url = "images/word.png";
                    break;

                case "Batch File":
                    url = "images/batch.png";
                    break;

                default:
                    url = "images/unknown.png";
                    break;
            }

            return url;
        }

        // Get file type by the extension
        public string GetFileType(string fileExtension)
        {
            string fileType;

            switch (fileExtension.ToLower())
            {
                case ".txt":
                    fileType = "Text";
                    break;

                case ".png":
                    fileType = "Portable Network Graphics";
                    break;

                case ".gif":
                    fileType = "Graphics Interchange Format";
                    break;

                case ".jpg":
                    fileType = "Joint Photographic Experts Group";
                    break;

                case ".jpeg":
                    fileType = "Joint Photographic Experts Group";
                    break;

                case ".docx":
                    fileType = "Windows Word Document";
                    break;

                case ".ppt":
                    fileType = "Windows Power Point";
                    break;

                case ".pptx":
                    fileType = "Windows Power Point";
                    break;

                case ".css":
                    fileType = "Cascading Style Sheet";
                    break;

                case ".html":
                    fileType = "HyperText Markup Language";
                    break;

                case ".xml":
                    fileType = "eXtensible Markup Language";
                    break;

                case ".pdf":
                    fileType = "Portable Document Format";
                    break;

                default:
                    fileType = "Unknown to app";
                    break;
            }

            return fileType;
        }

        // Get file type by the extension
        public string GetFileExtension(string fileType)
        {
            string extension;

            switch (fileType)
            {
                case "Text":
                    extension = ".txt";
                    break;

                case "Portable Network Graphics":
                    extension = ".png";
                    break;

                case "Graphics Interchange Format":
                    extension = ".gif";
                    break;

                case "Joint Photographic Experts Group":
                    extension = ".jpeg";
                    break;

                case "Windows Word Document":
                    extension = ".docx";
                    break;

                case "Batch File":
                    extension = ".bat";
                    break;

                case "Windows Power Point":
                    extension = ".ppt";
                    break;

                case "Cascading Style Sheet":
                    extension = ".css";
                    break;

                case "HyperText Markup Language":
                    extension = ".html";
                    break;

                case "eXtensible Markup Language":
                    extension = ".xml";
                    break;

                case "Portable Document Format":
                    extension = ".pdf";
                    break;

                default:
                    extension = ".txt";
                    break;
            }

            return extension;
        }

        public bool TestForLegalTypes(string fileExtension)
        {
            bool isStringContainedInList = new[] { ".txt", ".docx", ".doc", ".ppt", ".pptx", ".css", ".html", ".xml", ".pdf" }.Contains(fileExtension);

            if (isStringContainedInList == true)
                return true;
            else
                return false;
        }
    }
}
