using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Management;
using ServerManagement.Models;
using System.Text;
using Microsoft.Win32;
using System.Globalization;

namespace ServerManagement.Services
{
    public class ServerSpecService
    {
        public ServerSpecModel ComputerSystem(string ip, ref ServerSpecModel serverSpecModel)
        {
            var managementScope = new ManagementScope("\\\\" + ip + "\\root\\cimv2");
            var query = new ObjectQuery("Select * FROM Win32_ComputerSystem");
            var objSearcher = new ManagementObjectSearcher(managementScope, query);
            var managementCollection = objSearcher.Get();

            foreach (ManagementObject managementObject in managementCollection)
            {
                serverSpecModel.Manufacturer = managementObject.GetPropertyValue("Manufacturer").ToString();
                serverSpecModel.Memory = ReformatGigabyte(Convert.ToInt64(managementObject.GetPropertyValue("TotalPhysicalMemory")));
            }

            return serverSpecModel;
        }

        public ServerSpecModel OperatingSystem(string ip, ref ServerSpecModel serverSpecModel)
        {
            var managementScope = new ManagementScope("\\\\" + ip + "\\root\\cimv2");
            var query = new ObjectQuery("Select * FROM Win32_OperatingSystem");
            var objSearcher = new ManagementObjectSearcher(managementScope, query);
            var managementCollection = objSearcher.Get();
            var model = new ServerSpecModel();
            foreach (ManagementObject managementObject in managementCollection)
            {
                serverSpecModel.OperatingSystem = managementObject.GetPropertyValue("Caption").ToString();
                serverSpecModel.Version = managementObject.GetPropertyValue("Version").ToString();
            }

            return serverSpecModel;
        }

        public ServerSpecModel VolumeData(string ip, ref ServerSpecModel serverSpecModel)
        {
            var managementScope = new ManagementScope("\\\\" + ip + "\\root\\cimv2");
            var query = new ObjectQuery("Select * FROM Win32_LogicalDisk WHERE DriveType=3");
            var objSearcher = new ManagementObjectSearcher(managementScope, query);
            var managementCollection = objSearcher.Get();
            var diskDetail = new StringBuilder();
            foreach (ManagementObject managementObject in managementCollection)
            {
                var volumeName = string.IsNullOrWhiteSpace(managementObject.GetPropertyValue("VolumeName").ToString()) ?
                    "System" :
                    managementObject.GetPropertyValue("VolumeName").ToString();
                diskDetail.Append(volumeName + " (" + managementObject.GetPropertyValue("Name").ToString() + ")");
                diskDetail.Append("  ");
                diskDetail.Append(ReformatGigabyte(Convert.ToInt64(managementObject.GetPropertyValue("Size"))));
                diskDetail.Append(" Free of ");
                diskDetail.Append(ReformatGigabyte(Convert.ToInt64(managementObject.GetPropertyValue("FreeSpace"))));
                diskDetail.Append(" | ");
            }

            serverSpecModel.Disk = diskDetail.ToString().Substring(0, diskDetail.Length - 2);

            return serverSpecModel;
        }

        public ServerSpecModel ProcessorData(string ip, ref ServerSpecModel serverSpecModel)
        {
            var managementScope = new ManagementScope("\\\\" + ip + "\\root\\cimv2");
            var query = new ObjectQuery("Select * FROM Win32_Processor");
            var objSearcher = new ManagementObjectSearcher(managementScope, query);
            var managementCollection = objSearcher.Get();

            foreach (ManagementObject managementObject in managementCollection)
            {
                serverSpecModel.CPU = managementObject.GetPropertyValue("Name").ToString();
            }



            return serverSpecModel;
        }

        public ServerSpecModel NetworkData(string ip, ref ServerSpecModel serverSpecModel)
        {
            var managementScope = new ManagementScope("\\\\" + ip + "\\root\\cimv2");
            var query = new ObjectQuery("Select * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = True");
            var objSearcher = new ManagementObjectSearcher(managementScope, query);
            var managementCollection = objSearcher.Get();

            foreach (ManagementObject managementObject in managementCollection)
            {
                serverSpecModel.HostName = managementObject.GetPropertyValue("DNSHostName").ToString();
                if (managementObject.GetPropertyValue("IPAddress") != null)
                {
                    var ips = (Array)managementObject.GetPropertyValue("IPAddress");
                    foreach (var myIp in ips)
                    {
                        serverSpecModel.Ips += myIp + " | ";
                    }
                }
            }

            return serverSpecModel;
        }

        public string LastBootUpTime(string serverIp)
        {
            var managementScope = new ManagementScope("\\\\" + serverIp + "\\root\\cimv2");
            var query = new ObjectQuery("select LastBootUpTime from Win32_OperatingSystem");
            var objSearcher = new ManagementObjectSearcher(managementScope, query);
            var managementCollection = objSearcher.Get();
            DateTime lastBootUpTime = DateTime.Now;

            foreach (ManagementObject managementObject in managementCollection)
            {
                lastBootUpTime = ManagementDateTimeConverter.ToDateTime(managementObject["LastBootUpTime"].ToString());
            }

            return lastBootUpTime.ToString();
        }

        public Version GetIISVersion(string serverIp)
        {
            RegistryKey baseKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, serverIp);
            using (RegistryKey key = baseKey.OpenSubKey(@"Software\Microsoft\InetStp", false))
            {
                if (key == null) return new Version(0, 0);
                int majorVersion = (int)key.GetValue("MajorVersion", -1);
                int minorVersion = (int)key.GetValue("MinorVersion", -1);
                if (majorVersion == -1 || minorVersion == -1) return new Version(0, 0);

                return new Version(majorVersion, minorVersion);
            }
        }

        public string GetFrameworkVersion(string serverIp)
        {
            RegistryKey baseKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, serverIp);
            using (RegistryKey key = baseKey.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP", false))
            {
                string[] version_names = key.GetSubKeyNames();

                return version_names[version_names.Length - 1].Remove(0, 1);
            }
        }

        private string ReformatGigabyte(long value)
        {
            if (value.ToString().Length > 12)
            {
                return (value / 1024 / 1024 / 1024 / 1024).ToString("##0.00 TB");
            }
            else if (value.ToString().Length > 9)
            {
                return (value / 1024 / 1024 / 1024).ToString("###,##0.00 GB");
            }
            else if (value.ToString().Length > 6)
            {
                return (value / 1024 / 1024).ToString("###,###,##0.00 MB");
            }
            else if (value.ToString().Length > 3)
            {
                return (value / 1024).ToString("###,###,###,##0.00 KB");
            }
            else
            {
                return (value).ToString("###,###,###,###,##0.00 Bytes");
            }
        }
    }
}