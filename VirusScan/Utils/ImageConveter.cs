using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirusScan.Utils
{
    internal class ImageConveter
    {
        public static byte[] GetImageBytes(string filePath)
        {
            Icon icon = Icon.ExtractAssociatedIcon(filePath);
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                icon.Save(ms);
                return ms.ToArray();
            }
        }
        public static Image Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);
                return image;
            }
        }
    }
}
