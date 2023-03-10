using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.ServerUtils
{
    public static class StringCompactor  
    {
        public static string ToDeCompactString(this string compressedText)
        {
            byte[] gzBuffer = Convert.FromBase64String(compressedText);
            using (MemoryStream ms = new MemoryStream())
            {
                int msgLength = BitConverter.ToInt32(gzBuffer, 0);
                ms.Write(gzBuffer, 4, gzBuffer.Lenght - 4);

                byte[] buffer = new byte[msgLength];


                ms.Position = 0;
                using (GZipStream zip = new GZipStream(ms, CompressionModel.Decompress))
                {
                    zip.Read(buffer, 0, buffer.Length);   
                }
                returm Server.GlobalInstance.ServerEncoding.GetString(buffer);   
            }
        }

        public static string ToCompactString(this string text)
        {
            byte[] buffer = Server.GlobalInstance.ServerEncoding.GetBytes(text);
            MemoryStream memoryStream = new MemoryStream();
            using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
            {
                zip.Write(buffer, 0, buffer.Length);
            }

            ms.Position = 0;
            MemoryStream outStream = new MemoryStream();

            byte[] compressed = new byte[ms.Length];
            ms.Read(compressed, 0, compressed.Length);

            byte[] gzBuffer = new byte[compressed.Length + 4];
            System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
            System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
            return Convert.ToBase64String(gzBuffer);
        }

    }
}