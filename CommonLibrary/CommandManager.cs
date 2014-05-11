using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class CommandManager
    {
        public static void ExecuteCommand(string command, params string[] args)
        {
            string arguments = string.Empty;
            if (args != null && args.Length > 0)
            {
                for (int i = 0; i < args.Length; ++i)
                {
                    arguments = string.Format("{0} {1}", arguments, args[i]);
                }
            }

            //ClearResults();

            ProcessStartInfo startInfo = new ProcessStartInfo();

            startInfo.WorkingDirectory = Path.GetDirectoryName(command);
            startInfo.FileName = command;
            startInfo.Arguments = arguments;

            startInfo.LoadUserProfile = false;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;

            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;

            Process process = null;
            try
            {
                process = Process.Start(startInfo);
                string m_stdout = process.StandardOutput.ReadToEnd();
                //m_stderr = process.StandardError.ReadToEnd();
                process.WaitForExit();
                //FileManager.WriteFile(m_stdout, "D:\\ping.txt");
                //m_exitCode = process.ExitCode;
            }
            finally
            {
                if (process != null)
                {
                    process.Close();
                }
            }
        }
    }
}
