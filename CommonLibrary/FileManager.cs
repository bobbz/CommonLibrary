using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonLibrary
{
    public class FileManager
    {
        public const int count = 10;
        public readonly int num = 20;

        public enum FileType { TXT, XML, HTML };
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

        public void CopyFiles(string source, string destination, bool overwriteExisting = true, bool logEachFile = true)
        {
            // Validate the input arguments.
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentException("Argument cannot be null or empty.", "source");
            }

            if (string.IsNullOrEmpty(destination))
            {
                throw new ArgumentException("Argument cannot be null or empty.", "destination");
            }

            // Make sure that both the source and destination are full paths.
            string fullDestination = destination;
            string fullSource = source;

            if (!Path.IsPathRooted(fullDestination))
            {
                fullDestination = Path.Combine(Environment.CurrentDirectory, fullDestination);
            }

            if (!Path.IsPathRooted(fullSource))
            {
                fullSource = Path.Combine(Environment.CurrentDirectory, fullSource);
            }

            if (string.IsNullOrEmpty(Path.GetExtension(fullDestination)) && !fullDestination.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                fullDestination = string.Format("{0}{1}", fullDestination, Path.DirectorySeparatorChar);
            }

            if (string.IsNullOrEmpty(Path.GetExtension(fullSource)) && !fullSource.EndsWith("*"))
            {
                // The source is a directory, make sure it has a trailing slash
                if (!fullSource.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    fullSource = fullSource + Path.DirectorySeparatorChar;
                }
            }

            string sourceDir = Path.GetDirectoryName(fullSource);
            string sourceFile = Path.GetFileName(fullSource);
            string destinationDir = Path.GetDirectoryName(fullDestination);

            // Search for all files we need to copy.
            //LogManager.LogMessage("Searching for files to copy.", LogMessageLevel.Info);
            if (string.IsNullOrEmpty(sourceFile))
            {
                sourceFile = "*";
            }

            string[] files = null;
            if (sourceFile.StartsWith("*"))
            {
                files = Directory.GetFiles(sourceDir, sourceFile, SearchOption.AllDirectories);
            }
            else
            {
                files = Directory.GetFiles(sourceDir, sourceFile, SearchOption.TopDirectoryOnly);
            }

            //LogManager.LogMessage("{0} files found.", LogMessageLevel.Info, files.Length);

            // If no files were found, simply return.
            if (files.Length == 0)
            {
                //return;
                throw new InvalidOperationException(String.Format("The source {0} could not be found", source));
            }

            if (files.Length > 1 && !string.IsNullOrEmpty(Path.GetExtension(fullDestination)))
            {
                throw new InvalidOperationException("The destination you specified is a file when the source contains multiple files.");
            }

            // Create a queue of files to copy.
            Queue<CopyFileCommand> m_fileCopyQueue;
            List<CopyFileCommand> m_fileCopiedList;
            m_fileCopyQueue = new Queue<CopyFileCommand>(files.Length);
            m_fileCopiedList = new List<CopyFileCommand>(files.Length);
            for (int i = 0; i < files.Length; ++i)
            {
                string destinationPath = string.Concat(destinationDir, files[i].Substring(sourceDir.Length));

                lock (m_fileCopyQueue)
                {
                    m_fileCopyQueue.Enqueue(new CopyFileCommand(files[i], destinationPath, overwriteExisting));
                }
            }

            // Launch the threads to perform the file copy.
            Thread copyThread1 = new Thread(new ThreadStart(MultiThreadedFileCopy));
            Thread copyThread2 = new Thread(new ThreadStart(MultiThreadedFileCopy));
            Thread copyThread3 = new Thread(new ThreadStart(MultiThreadedFileCopy));
            Thread copyThread4 = new Thread(new ThreadStart(MultiThreadedFileCopy));
            Thread copyThread5 = new Thread(new ThreadStart(MultiThreadedFileCopy));

            copyThread1.Start();
            copyThread2.Start();
            copyThread3.Start();
            copyThread4.Start();
            copyThread5.Start();

            while (copyThread1.IsAlive || copyThread2.IsAlive || copyThread3.IsAlive
                || copyThread4.IsAlive || copyThread5.IsAlive)
            {
                System.Threading.Thread.Sleep(500);
            }

            StringBuilder sb = new StringBuilder();

            //the verbosity here causes long delays in loading the results into the UI when there are tons of files...
            //users will get the old behavior by default
            if (logEachFile)
            {
                for (int i = 0; i < m_fileCopiedList.Count; ++i)
                {
                    sb.AppendLine(string.Format("copied {0} --> {1}", m_fileCopiedList[i].Source, m_fileCopiedList[i].Destination));
                }
            }
            LogManager.LogMessage(sb.ToString(), LogMessageLevel.Info);
            LogManager.LogMessage("{0} of {1} file(s) copied successfully.", LogMessageLevel.Info, m_fileCopiedList.Count, files.Length);

            if (m_fileCopyQueue.Count != 0 || m_fileCopiedList.Count != files.Length)
            {
                throw new Exception("An unexpected error has occurred during the file copy process.");
            }

        }

        /// <summary>
        /// Copies the list of files in the sources to the destination.
        /// </summary>
        /// <param name="sources">Either a file or directory.  Can contain wildcards.</param>
        /// <param name="destination">Either a file or directory.</param>
        public void CopyFiles(string[] sources, string destination)
        {
            // Validate the input arguments.
            if (sources == null)
            {
                throw new ArgumentNullException("Argument cannot be null.", "sources");
            }

            if (string.IsNullOrEmpty(destination))
            {
                throw new ArgumentException("Argument cannot be null or empty.", "destination");
            }

            foreach (string source in sources)
            {
                CopyFiles(source, destination);
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

    public class HtmlManager
    {
        public static string DownloadHtmlSourceCode(string uri, Encoding encd = null)
        {
            WebClient wc = new WebClient();
            if (encd != null)
                wc.Encoding = encd;
            string code = wc.DownloadString(uri);

            return code;
        }
        public static string RequestSourceCode(string uri)
        {
            string content = "";
            
            WebRequest req = WebRequest.Create(uri);
            
            WebResponse resp = req.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream());
            content = sr.ReadToEnd();
            
            return content;
        }
    }

}