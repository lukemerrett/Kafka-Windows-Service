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

            _zookeeperProcess = StartProcess(Constants.ZookeeperProcess);
            _kafkaProcess = StartProcess(Constants.KafkaProcess);
        }

        public void Stop()
        {
            KillProcessAndChildren(_kafkaProcess.Id);
            KillProcessAndChildren(_zookeeperProcess.Id);
        }

        private static Process StartProcess(string command)
        {
            ProcessStartInfo processInfo;

            processInfo = new ProcessStartInfo("cmd.exe", "/c " + command)
            {
                CreateNoWindow = true,
                UseShellExecute = false
            };

            return Process.Start(processInfo);
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