using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
        //".mp4", ".flac", ".avi", ".wmv", ".flv", ".mov", ".mp3", ".wav", ".wma", ".wma", ".flac"
    }
}
