using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using System.IO;
using CommonLibrary;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            string projectName = args[0];
            //@"D:\school\tfs\Src\TFS\common\tools\TFSAutoStarterService\TFSAutoStarterStub\TFSAutoStarterStub.csproj";
            if (!File.Exists(projectName))
                throw new Exception("Project not exists!");

            string msbuildPath = string.Empty;
            if (File.Exists(@"D:\SPITests\binaries\suitebin\msbuild.exe"))
                msbuildPath = @"D:\SPITests\binaries\suitebin\msbuild.exe";
            else if (File.Exists(@"D:\Program Files\MSBuild\12.0\Bin\msbuild.exe"))
                msbuildPath = @"D:\Program Files\MSBuild\12.0\Bin\msbuild.exe";
            else if (File.Exists(@"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe"))
                msbuildPath = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe";
            else if (File.Exists(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"))
                msbuildPath = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe";
            if (msbuildPath == string.Empty)
                throw new Exception("msbuild.exe could not found!");

            CommandManager.ExecuteCommand(msbuildPath, projectName);
        }
    }
}