using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class NetworkManager
    {
        public static string GetIPByHostName(string hostName)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                // Use local machine name when input string is empty
                hostName = Dns.GetHostName();
                //Logger.LogMessage("Local Machine's Host Name: " + hostName);
            }

            // Then using host name, get the IP address list
            IPHostEntry ipEntry = Dns.GetHostEntry(hostName);

            string machineIP = string.Empty;
            if (ipEntry.AddressList.Length > 0)
            {
                foreach (IPAddress ip in ipEntry.AddressList)
                {
                    if (ip.AddressFamily.ToString() == ProtocolFamily.InterNetwork.ToString())
                    {
                        machineIP = ip.ToString();
                    }
                }
                return machineIP;
            }
            else
            {
                Console.WriteLine("Fail to get IP address for host: " + hostName);
            }
            return string.Empty;
        }
    }
}