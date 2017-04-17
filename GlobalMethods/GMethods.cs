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
                    url = "images/unkown.png";
                    break;
            }

            return url;
        }

        // Get file type by the extension
        public string GetFileType(string fileExtension)
        {
            string fileType;

            switch (fileExtension)
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

                case ".bat":
                    fileType = "Batch File";
                    break;

                default:
                    fileType = "Unknown to app";
                    break;
            }

            return fileType;
        }



    }
}
