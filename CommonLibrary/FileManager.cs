using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonLibrary
{
    public class FileManager
    {
        public const int count = 10;
        public readonly int num = 20;
        public static string ReadAll(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new Exception("File name can not be empty!");
            StreamReader sr = new StreamReader(filename);
            return sr.ReadToEnd();
        }
        public static void WriteFile(string content, string filename = null, bool append = false, bool appendNewLine = false)
        {
            if (string.IsNullOrEmpty(filename))
            {
                filename = string.Format("{0}\\temp.txt", Environment.CurrentDirectory);
                File.Create(filename);
                using (StreamWriter sw = new StreamWriter(filename))
                {                    
                    sw.Write(content);
                }
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(filename, append))
                {
                    sw.Write(appendNewLine ? (sw.NewLine + content) : content);
                }
            }
        }
    }

    public static class ImageManager
    {
        public static string CaptureScreenshot(string screenshotName)
        {
            Bitmap bmp = new Bitmap(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(SystemInformation.VirtualScreen.X, SystemInformation.VirtualScreen.Y, 0, 0, SystemInformation.VirtualScreen.Size, CopyPixelOperation.SourceCopy);
                return SaveImage(screenshotName, bmp);
            }
        }

        public static string SaveImage(string name, Bitmap image)
        {
            return SaveImage(name, image, ImageFormat.Jpeg);
        }
        public static string SaveImage(string name, Bitmap image, ImageFormat format)
        {
            //if (!Directory.Exists(ScreenshotDir))
            //{
            //    Directory.CreateDirectory(ScreenshotDir);
            //}

            // Get a unique filename
            string escapedFileName = ReplaceInvalidFileNameChars(name).Replace(" ", "");
            string baseFileName = Path.Combine(Environment.CurrentDirectory, escapedFileName);
            string filename;
            int index = 0;

            string fileExtension = format.ToString().ToLower();
            if (format == ImageFormat.Jpeg)
            {
                // MadDog doesn't recognize ".jpeg" when populating the Image tab for screenshots so use ".jpg" instead
                fileExtension = "jpg";
            }

            while (File.Exists(filename = string.Format("{0}{1:0000}.{2}", baseFileName, index, fileExtension)))
            {
                index++;
            }

            // Generate the file
            image.Save(filename, format);

            return filename;
        }

        private static string ReplaceInvalidFileNameChars(string filename, string replacementText = "-")
        {
            var invalidFileNameChars = Path.GetInvalidFileNameChars().ToDictionary(c => c);
            StringBuilder sb = new StringBuilder(filename.Length);
            foreach (char c in filename)
            {
                if (invalidFileNameChars.ContainsKey(c))
                {
                    sb.Append(replacementText);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }

    public class XMLManager : FileManager
    {
        
    }
}