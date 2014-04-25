using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using System.Threading;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine(filename);
            //TimerCallback tc = new TimerCallback(func);
            //TimeManager.RunInMinutes(0.1, tc);
            Console.Read();
        }

        public static void func(object o)
        {
            string filename = ImageManager.CaptureScreenshot("testscreenshot", "D:\\screenshot");
            Mailer.SendEmail()
            Console.WriteLine(DateTime.Now.ToString());
        }

        
    }
}
