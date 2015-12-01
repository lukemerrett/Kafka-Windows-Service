#region

using KafkaService.Installer;
using System;
using System.Diagnostics;
using System.Management;

#endregion

namespace KafkaService
{
    public class KafkaServiceWrapper
    {
        private Process _zookeeperProcess;

        private Process _kafkaProcess;

        public void Start()
        {
            ApacheKafkaInstaller.Install();

            //_zookeeperProcess = Process.Start("powershell.exe", "-file ServiceRunners\\StartZookeeper.ps1");
            //_kafkaProcess = Process.Start("powershell.exe", "-file ServiceRunners\\StartKafka.ps1");
        }

        public void Stop()
        {
            //KillProcessAndChildren(_kafkaProcess.Id);
            //KillProcessAndChildren(_zookeeperProcess.Id);
        }

        private static void KillProcessAndChildren(int pid)
        {
            using (var searcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + pid))
            {
                var managementObjects = searcher.Get();

                foreach (var obj in managementObjects)
                {
                    var managementObject = (ManagementObject) obj;
                    KillProcessAndChildren(Convert.ToInt32(managementObject["ProcessID"]));
                }

                try
                {
                    var proc = Process.GetProcessById(pid);
                    proc.Kill();
                }
                catch (ArgumentException)
                {
                    // Process already exited.
                }
            }
        }
    }
}