using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Drawing;
using System.IO;
using System.Management;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FK测试工具
{
    public static class CommonFunc
    {
        public static string GetMAC()
        {
            string s1 = GetMacAddress1();
            if (!string.IsNullOrEmpty(s1))
            {
                return s1;
            }
            string s2 = GetMacAddress2();
            if (!string.IsNullOrEmpty(s2))
            {
                return s2;
            }
            return "00-00-00-00-00-00";
        }

        public static string GetMacAddress2()
        {
            try
            {
                const int MIN_MAC_ADDR_LENGTH = 12;
                long maxSpeed = -1;
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    string tmpMac = nic.GetPhysicalAddress().ToString();
                    if (nic.Speed > maxSpeed && !string.IsNullOrEmpty(tmpMac) && tmpMac.Length >= MIN_MAC_ADDR_LENGTH)
                    {
                        return BitConverter.ToString(nic.GetPhysicalAddress().GetAddressBytes());
                    }
                }
            }
            catch (Exception) { return ""; }
            return "";
        }


        public static string GetMacAddress1()
        {
            try
            {
                string madAddr = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc2 = mc.GetInstances();
                foreach (ManagementObject mo in moc2)
                {
                    if (Convert.ToBoolean(mo["IPEnabled"]) == true)
                    {
                        madAddr = mo["MacAddress"].ToString();
                        madAddr = madAddr.Replace(':', '-');
                    }
                    mo.Dispose();
                }
                return madAddr;
            }
            catch
            {
                return "";
            }
        }
    }
}
